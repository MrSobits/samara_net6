namespace Bars.Gkh.Decisions.Nso.Domain.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.Decisions.Nso.Entities;
    using Bars.Gkh.Decisions.Nso.Entities.Decisions;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums.Decisions;
    using Bars.Gkh.Utils;

    using Castle.Windsor;

    using NHibernate;

    /// <summary>
    /// Сервис Протокол решений собственников
    /// </summary>
    public class RealityObjectDecisionService : IRealityObjectDecisionsService
    {
        private readonly IWindsorContainer container;
        private readonly IDomainService<RealityObjectDecisionProtocol> roDecisionProtocolDomainService;
        private readonly IDomainService<CrFundFormationDecision> crFundDecisionDomainService;
        private readonly IDomainService<CreditOrgDecision> creditOrgDecisionDomainService;
        private readonly IDomainService<GovDecision> govDecisonDomain;

        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="container"></param>
        /// <param name="roDecisionProtocolDomainService">Протокол решений собственников</param>
        /// <param name="crFundDecisionDomainService">Решение о формировании фонда КР</param>
        /// <param name="creditOrgDecisionDomainService">Решение о выборе кредитной организации</param>
        /// <param name="govDecisonDomain">Протокол решения органа государственной власти</param>
        public RealityObjectDecisionService(
            IWindsorContainer container,
            IDomainService<RealityObjectDecisionProtocol> roDecisionProtocolDomainService,
            IDomainService<CrFundFormationDecision> crFundDecisionDomainService,
            IDomainService<CreditOrgDecision> creditOrgDecisionDomainService,
            IDomainService<GovDecision> govDecisonDomain)
        {
            this.container = container;
            this.roDecisionProtocolDomainService = roDecisionProtocolDomainService;
            this.crFundDecisionDomainService = crFundDecisionDomainService;
            this.creditOrgDecisionDomainService = creditOrgDecisionDomainService;
            this.govDecisonDomain = govDecisonDomain;
        }

        /// <summary>
        /// Возвращает список домов с решением формирования фонда дома на специальном счете.
        /// </summary>
        public ListDataResult RealityObjectsOnSpecialAccount()
        {
            return this.RealityObjectsByCrFundDecisionType(CrFundFormationDecisionType.SpecialAccount);
        }

        /// <summary>
        /// Возвращает список домов с решением формирования фонда дома у регоператора.
        /// </summary>
        /// <returns></returns>
        public ListDataResult RealityObjectsOnRegOpAccount()
        {
            return this.RealityObjectsByCrFundDecisionType(CrFundFormationDecisionType.RegOpAccount);
        }

        /// <summary>
        /// Возвращает список домов с решением формирования фонда дома определенного типа.
        /// </summary>
        /// <param name="type">Тип решения по формированию дома</param>
        /// <returns></returns>
        public ListDataResult RealityObjectsByCrFundDecisionType(CrFundFormationDecisionType type)
        {
            var realiesWithDecisions = this.roDecisionProtocolDomainService.GetAll()
                .Join(
                    this.crFundDecisionDomainService.GetAll(),
                    protocol => protocol.Id,
                    decision => decision.Protocol.Id,
                    (protocol, decision) => new
                    {
                        protocol.RealityObject,
                        Decision = decision
                    })
                .ToList()
                .GroupBy(x => x.RealityObject)
                .Select(
                    group => new
                    {
                        RealityObject = group.Key,
                        LatestDecision = group.OrderByDescending(y => y.Decision.StartDate).First().Decision
                    })
                .Where(x => x.LatestDecision.Decision == type);

            var result = realiesWithDecisions.ToList();

            return new ListDataResult(result, result.Count());
        }

        /// <inheritdoc />
        public ListDataResult RealityObjectCreditOrgDecisions()
        {
            var realiesWithDecisions = this.roDecisionProtocolDomainService.GetAll()
                .Join(
                    this.crFundDecisionDomainService.GetAll(),
                    protocol => protocol.Id,
                    decision => decision.Protocol.Id,
                    (protocol, decision) => new
                    {
                        Protocol = protocol,
                        Decision = decision
                    })
                .ToList()
                .GroupBy(x => x.Protocol)
                .Select(
                    group => new
                    {
                        Protocol = group.Key,
                        LatestDecision = group
                            .OrderByDescending(y => y.Decision.StartDate)
                            .First()
                            .Decision
                    })
                .Where(x => x.LatestDecision.Decision == CrFundFormationDecisionType.SpecialAccount)
                .Join(
                    this.creditOrgDecisionDomainService.GetAll(),
                    join => join.Protocol.Id,
                    decision => decision.Protocol.Id,
                    (join, decision) => new
                    {
                        join.Protocol.RealityObject,
                        Decision = decision
                    })
                .ToList()
                .GroupBy(x => x.RealityObject)
                .Select(
                    group => new
                    {
                        RealityObject = group.Key,
                        CreditOrg = group
                            .OrderByDescending(y => y.Decision.StartDate)
                            .First()
                            .Decision
                            .Decision,
                        group.OrderByDescending(y => y.Decision.StartDate)
                            .First()
                            .Decision
                            .BankFile
                    });

            var result = realiesWithDecisions.ToList();

            return new ListDataResult(result, result.Count());
        }

        /// <summary>
        /// Возвращает актуальное решение на доме
        /// </summary>
        /// <typeparam name="T">Тип решения</typeparam>
        /// <param name="realityObject">Жилой дом</param>
        /// <param name="inFinalState">Статус конечный</param>
        /// <param name="protocolsToFilter">При проверке пропускать эти протоколы</param>
        /// <param name="date"></param>
        /// <returns>Решение</returns>
        public T GetActualDecision<T>(
            RealityObject realityObject,
            bool inFinalState = false,
            IEnumerable<RealityObjectDecisionProtocol> protocolsToFilter = null,
            DateTime? date = null) where T : UltimateDecision
        {
            var protocols = this.roDecisionProtocolDomainService.GetAll()
                .WhereIf(inFinalState, x => x.State.FinalState)
                .WhereIf(date != null, x => x.DateStart <= date)
                .Where(x => x.RealityObject.Id == realityObject.Id)
                .Select(x => x.Id)
                .ToArray();

            var excludedProtocols = protocolsToFilter.IsNotNull()
                ? protocolsToFilter.Select(p => p.Id).ToList()
                : new List<long>();

            var domain = this.container.ResolveDomain<T>();
            using (this.container.Using(domain))
            {
               var decisions = domain.GetAll()
                    .WhereContains(x => x.Protocol.Id, protocols)
                    .WhereIf(excludedProtocols.Any(), x => !excludedProtocols.Contains(x.Protocol.Id));

                return decisions.OrderByDescending(decision => decision.Protocol.ProtocolDate).FirstOrDefault();
            }
        }

        /// <summary>
        /// Возвращает список актуальных решение на доме
        /// </summary>
        /// <typeparam name="T">Тип решения</typeparam>
        /// <param name="realityObject">Жилой дом</param>
        /// <param name="protocolsToFilter">При проверке пропускать эти протоколы</param>
        public List<T> GetActualDecisions<T>(
            RealityObject realityObject,
            IEnumerable<RealityObjectDecisionProtocol> protocolsToFilter = null) where T : UltimateDecision
        {
            var excludedProtocols = protocolsToFilter.IsNotNull()
                ? protocolsToFilter.Select(p => p.Id).ToList()
                : new List<long>();

            var protocols = this.roDecisionProtocolDomainService.GetAll()
                .Where(x => x.RealityObject.Id == realityObject.Id && excludedProtocols.Contains(x.Id));

            var domain = this.container.ResolveDomain<T>();
            using (this.container.Using(domain))
            {
                var decisions = domain.GetAll()
                    .Where(x => protocols.Any(p => p == x.Protocol))
                    .WhereIf(excludedProtocols.Any(), x => !excludedProtocols.Contains(x.Protocol.Id));

                return decisions.OrderByDescending(decision => decision.Protocol.ProtocolDate).ToList();
            }
        }

        /// <summary>
        /// Возвращает словарь актуальных решение по домам
        /// </summary>
        /// <typeparam name="T">Тип решения</typeparam>
        /// <param name="realityObjectIds">Жилые дома</param>
        /// <param name="date"></param>
        public Dictionary<long, IGrouping<long, T>> GetActualDecisions<T>(List<long> realityObjectIds, DateTime? date = null) where T : UltimateDecision
        {
            var protocols = this.roDecisionProtocolDomainService.GetAll()
                .Where(x => x.State.FinalState)
                .WhereContainsBulked(x => x.RealityObject.Id, realityObjectIds).Select(x => x.Id).ToList();

            var domain = this.container.ResolveDomain<T>();
            using (this.container.Using(domain))
            {
                var decisions = domain.GetAll()
                    .Where(x => x.Protocol.State.FinalState)
                    .WhereIf(date != null, x => x.Protocol.DateStart <= date)
                    .WhereContainsBulked(x => x.Protocol.Id, protocols);

                return decisions.OrderByDescending(decision => decision.Protocol.ProtocolDate)
                    .GroupBy(x => x.Protocol.RealityObject.Id)
                    .ToDictionary(x => x.Key, x => x);
            }
        }

        /// <summary>
        /// Возвращает словарь актуальных решений по домам
        /// </summary>
        /// <typeparam name="T">Тип решения</typeparam>
        /// <param name="ros">Жилые дома</param>
        /// <param name="inFinalState">Флаг для выбора только конечного статуса </param>
        /// <param name="date">На дату</param>
        /// <returns></returns>
        public Dictionary<RealityObject, T> GetActualDecisionForCollection<T>(
            IEnumerable<RealityObject> ros,
            bool inFinalState)
            where T : UltimateDecision
        {
            List<long> ids = null;

            if (ros.IsNot<IQueryable<RealityObject>>())
            {
                ids = ros.Select(x => x.Id).Distinct().ToList();
            }

            var protocols = this.roDecisionProtocolDomainService.GetAll()
                .WhereIf(inFinalState, x => x.State.FinalState)
                .WhereIf(ros.Is<IQueryable<RealityObject>>(), p => ros.Any(x => x == p.RealityObject))
                .WhereIf(ids.IsNotNull(), x => ids.Contains(x.RealityObject.Id));

            var domain = this.container.ResolveDomain<T>();

            using (this.container.Using(domain))
            {
                var decisions = domain.GetAll()
                    .Where(x => protocols.Any(p => p == x.Protocol))
                    .Select(
                        x => new
                        {
                            Decision = x,
                            x.Protocol.ProtocolDate,
                            RoId = x.Protocol.RealityObject.Id
                        })
                    .ToList()
                    .GroupBy(x => x.RoId)
                    .Select(
                        x => new
                        {
                            Ro = new RealityObject {Id = x.Key},
                            Dec = x.OrderByDescending(d => d.ProtocolDate).First().Decision
                        })
                    .ToDictionary(x => x.Ro, x => x.Dec);

                return decisions;
            }
        }

        /// <summary>
        /// Подсчет периодов действия способа формирования фонда
        /// </summary>
        /// <param name="roIds"></param>
        /// <returns></returns>
        public Dictionary<long, IEnumerable<Tuple<DateTime, CrFundFormationDecisionType>>> GetRobjectsFundFormation(ICollection<long> roIds)
        {
            var session = this.container.Resolve<ISessionProvider>().GetCurrentSession();
            var oldFlushMode = session.FlushMode;
            session.FlushMode = FlushMode.Never;

            try
            {
                var govDecisionQuery = this.govDecisonDomain.GetAll();
                var crFundFormationDecisionQuery = this.crFundDecisionDomainService.GetAll();
                if (roIds != null)
                {
                    govDecisionQuery = govDecisionQuery
                        .WhereContainsBulked(x => x.RealityObject.Id, roIds);
                    crFundFormationDecisionQuery = crFundFormationDecisionQuery
                        .WhereContainsBulked(x => x.Protocol.RealityObject.Id, roIds);
                }
                return this.GetDicisionUnionData(govDecisionQuery, crFundFormationDecisionQuery);
            }
            finally
            {
                session.FlushMode = oldFlushMode;
            }
        }

        /// <summary>
        /// Подсчет периодов действия способа формирования фонда для перерасчета
        /// </summary>
        /// <param name="roIds"></param>
        /// <returns></returns>
        public Dictionary<long, IEnumerable<Tuple<DateTime, CrFundFormationDecisionType>>> GetRobjectsFundFormationForRecalc(ICollection<long> roIds = null)
        {
            var session = this.container.Resolve<ISessionProvider>().GetCurrentSession();
            var oldFlushMode = session.FlushMode;
            session.FlushMode = FlushMode.Never;

            try
            {
                var govDecisionQuery = this.govDecisonDomain.GetAll();
                var crFundFormationDecisionQuery = this.crFundDecisionDomainService.GetAll();
                if (roIds != null)
                {
                    govDecisionQuery = govDecisionQuery
                        .WhereContainsBulked(x => x.RealityObject.Id, roIds);
                    crFundFormationDecisionQuery = crFundFormationDecisionQuery
                        .WhereContainsBulked(x => x.Protocol.RealityObject.Id, roIds);
                }
                return this.GetDicisionUnionDataForRecalc(govDecisionQuery, crFundFormationDecisionQuery);
            }
            finally
            {
                session.FlushMode = oldFlushMode;
            }
        }

        /// <summary>
        /// Подсчет периодов действия способа формирования фонда
        /// </summary>
        /// <param name="roIdsQuery"></param>
        /// <returns></returns>
        public Dictionary<long, IEnumerable<Tuple<DateTime, CrFundFormationDecisionType>>> GetRobjectsFundFormation(IQueryable<long> roIdsQuery)
        {
            var session = this.container.Resolve<ISessionProvider>().GetCurrentSession();
            var oldFlushMode = session.FlushMode;
            session.FlushMode = FlushMode.Never;

            try
            {
                var govDecisionQuery = this.govDecisonDomain.GetAll();
                var crFundFormationDecisionQuery = this.crFundDecisionDomainService.GetAll();
                if (roIdsQuery != null)
                {
                    govDecisionQuery = govDecisionQuery
                        .Where(x => roIdsQuery.Any(id => id == x.RealityObject.Id));
                    crFundFormationDecisionQuery = crFundFormationDecisionQuery
                        .Where(x => roIdsQuery.Any(y => y == x.Protocol.RealityObject.Id));
                }
                return this.GetDicisionUnionData(govDecisionQuery, crFundFormationDecisionQuery);
            }
            finally
            {
                session.FlushMode = oldFlushMode;
            }
        }

        private Dictionary<long, IEnumerable<Tuple<DateTime, CrFundFormationDecisionType>>> GetDicisionUnionData(
            IQueryable<GovDecision> govDecisionQuery,
            IQueryable<CrFundFormationDecision> crFundFormationDecisionQuery)
        {
            var govData = govDecisionQuery
                //.Where(x => x.State.FinalState)
                .Select(x => new
                {
                    x.RealityObject.Id,
                    x.DateStart,
                    Decision = x.FundFormationByRegop
                        ? CrFundFormationDecisionType.RegOpAccount
                        : CrFundFormationDecisionType.Unknown
                })
                .ToList();

            var ownersData = crFundFormationDecisionQuery
                //.Where(x => x.Protocol.State.FinalState)
                .Select(x => new
                {
                    x.Protocol.RealityObject.Id,
                    x.Protocol.DateStart,
                    x.Decision,
                })
                .ToList();

            return govData.Union(ownersData)
                .GroupBy(x => x.Id)
                .Select(z => new
                {
                    RoId = z.Key,
                    Decisions = z.Select(x => new Tuple<DateTime, CrFundFormationDecisionType>(x.DateStart, x.Decision)).OrderByDescending(x => x.Item1).AsEnumerable()
                })
                .ToDictionary(x => x.RoId, z => z.Decisions);
        }

        private Dictionary<long, IEnumerable<Tuple<DateTime, CrFundFormationDecisionType>>> GetDicisionUnionDataForRecalc(
            IQueryable<GovDecision> govDecisionQuery,
            IQueryable<CrFundFormationDecision> crFundFormationDecisionQuery)
        {
            var govData = govDecisionQuery
                .Where(x => x.State.FinalState)
                .Select(x => new
                {
                    x.RealityObject.Id,
                    x.DateStart,
                    Decision = x.FundFormationByRegop
                        ? CrFundFormationDecisionType.RegOpAccount
                        : CrFundFormationDecisionType.Unknown
                })
                .ToList();

            var ownersData = crFundFormationDecisionQuery
                .Where(x => x.Protocol.State.FinalState)
                .Select(x => new
                {
                    x.Protocol.RealityObject.Id,
                    x.Protocol.DateStart,
                    x.Decision,
                })
                .ToList();

            return govData.Union(ownersData)
                .GroupBy(x => x.Id)
                .Select(z => new
                {
                    RoId = z.Key,
                    Decisions = z.Select(x => new Tuple<DateTime, CrFundFormationDecisionType>(x.DateStart, x.Decision)).OrderByDescending(x => x.Item1).AsEnumerable()
                })
                .ToDictionary(x => x.RoId, z => z.Decisions);
        }

        /// <summary>
        /// Получение протокол решения органа государственной власти по дому
        /// </summary>
        /// <param name="robject">Жилой дом</param>
        /// <returns></returns>
        public GovDecision GetCurrentGovDecision(RealityObject robject)
        {
            return this.govDecisonDomain.GetAll()
                .Where(x => x.RealityObject.Id == robject.Id)
                .Where(x => x.DateStart <= DateTime.Today)
                .OrderByDescending(x => x.DateStart)
                .FirstOrDefault();
        }
    }
}