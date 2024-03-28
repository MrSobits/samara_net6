namespace Bars.Gkh.Overhaul.Tat.DomainService.FormingOfCr
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.DomainService;
    using Bars.Gkh.DomainService.Dict.RealEstateType;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Extensions;
    using Bars.Gkh.Overhaul.Entities;
    using Bars.Gkh.Overhaul.Enum;
    using Bars.Gkh.Overhaul.Tat.Entities;
    using Bars.Gkh.Overhaul.Tat.Enum;
    using Bars.Gkh.RegOperator.Enums;

    using Castle.Windsor;

    using NHibernate.Linq;

    /// <summary>
    /// Реализация работы с протоколами в Татарстане
    /// </summary>
    public class RealityObjectDecisionProtocolProxyService : FormingOfCrProvider, IRealityObjectDecisionProtocolProxyService
    {
        private IDictionary<long, PaySizeProxy[]> paysizeRecordByMunicipaltyCache;
        private IDictionary<long, PaySizeProxy[]> paySizeByMunicipaltyCache;
        private IDictionary<long, PaySizeProxy[]> paySizeByProtocolCache;

        public RealityObjectDecisionProtocolProxyService(
            IWindsorContainer container, 
            IDomainService<BasePropertyOwnerDecision> basePropertyOwnerDecisionDomain, 
            IDomainService<SpecialAccountDecision> domainService)
            : base(container, basePropertyOwnerDecisionDomain, domainService)
        {
            
        }
        /// <summary>
        /// Домен сервис для Жилой дом
        /// </summary>
        public IDomainService<RealityObject> RealityObjectDomainService { get; set; }

        /// <summary>
        /// Доменный сервис "Решение собственников помещений МКД (при формирования фонда КР на спец.счете)"
        /// </summary>
        public IDomainService<SpecialAccountDecision> SpecialAccountDecisionDomain { get; set; }

        /// <summary>
        /// Доменный сервис "Решение собственников помещений МКД (при формирования фонда КР на счете Рег.оператора)"
        /// </summary>
        public IDomainService<RegOpAccountDecision> RegOpAccountDecisionDomain { get; set; }

        /// <summary>
        ///  Доменный сервис "Протоколы собственников помещений МКД"
        /// </summary>
        public IDomainService<PropertyOwnerProtocols> PropertyOwnerProtocolsDomainService { get; set; }

        /// <summary>
        ///  Доменный сервис "Решение собственников помещений МКД (Установление минимального размера фонда кап.ремонта)"
        /// </summary>
        public IDomainService<MinAmountDecision> MinAmountDecisionDomainService { get; set; }

        /// <summary>
        /// Доменный сервис "Сущность записи взноса на КР по муниципальному образованию"
        /// </summary>
        public IDomainService<PaymentSizeMuRecord> PaymentSizeMuRecordDomainService { get; set; }

        /// <summary>
        /// Домен сервис для Муниципальное образование размера взноса на кр
        /// </summary>
        public IDomainService<PaysizeRecord> PaysizeRecordDomainService { get; set; }

        /// <summary>
        /// Получить прокси протокола или решения
        /// </summary>
        /// <param name="realityObject">Жилой дом</param>
        /// <returns>Прокси протокола или решения</returns>
        public RealityObjectDecisionProtocolProxy GetBothProtocolProxy(RealityObject realityObject)
        {
            return this.RegOpAccountDecisionDomain.GetAll()
                .Fetch(x => x.PropertyOwnerProtocol)
                .Where(x => x.RealityObject.Id == realityObject.Id && x.PropertyOwnerProtocol.DocumentDate != null)
                .Select(x => new
                {
                    x.Id,
                    x.PropertyOwnerProtocol,
                    x.RegOperator,
                    x.RealityObject,
                    TypeProtocol = (PropertyOwnerProtocolType?)x.PropertyOwnerProtocol.TypeProtocol
                })
                .AsEnumerable()
                .Select(x => new RealityObjectDecisionProtocolProxy
                {
                    Id = x.PropertyOwnerProtocol.Id,
                    ProtocolDate = x.PropertyOwnerProtocol.DocumentDate.GetValueOrDefault(),
                    ProtocolNumber = x.PropertyOwnerProtocol.DocumentNumber,
                    RealityObjectId = x.RealityObject.Id,
                    RegOpContragentName = x.RegOperator?.Contragent.Name,
                    RegOpContragentInn = x.RegOperator?.Contragent.Inn,
                    CrPaySize = this.GetPaysize(x.PropertyOwnerProtocol.Id, x.RealityObject.Id, null),
                    DecisionType = x.TypeProtocol == PropertyOwnerProtocolType.ResolutionOfTheBoard
                        ? CoreDecisionType.Government
                        : x.TypeProtocol == PropertyOwnerProtocolType.FormationFund
                            ? CoreDecisionType.Owners
                            : (CoreDecisionType?)null
                })
                .Union(
                    this.SpecialAccountDecisionDomain.GetAll()
                        .Where(x => x.RealityObject.Id == realityObject.Id && x.PropertyOwnerProtocol.DocumentDate != null)
                        .Select(x => new
                        {
                            x.Id,
                            x.PropertyOwnerProtocol,
                            x.RegOperator,
                            x.RealityObject,
                            x.ManagingOrganization,
                            TypeProtocol = (PropertyOwnerProtocolType?)x.PropertyOwnerProtocol.TypeProtocol
                        })
                        .AsEnumerable()
                        .Select(
                            x =>
                            { 
                                Contragent contragent = null;
                                if (x.RegOperator != null)
                                {
                                    contragent = x.RegOperator.Contragent;
                                }
                                else if (x.ManagingOrganization != null)
                                {
                                    contragent = x.ManagingOrganization.Contragent;
                                }

                                return new RealityObjectDecisionProtocolProxy
                                {
                                    Id = x.PropertyOwnerProtocol.Id,
                                    ProtocolDate = x.PropertyOwnerProtocol.DocumentDate.GetValueOrDefault(),
                                    ProtocolNumber = x.PropertyOwnerProtocol.DocumentNumber,
                                    RealityObjectId = x.RealityObject.Id,
                                    RegOpContragentName = contragent != null ? contragent.Name : "",
                                    RegOpContragentInn = contragent != null ? contragent.Inn : "",
                                    CrPaySize = this.GetPaysize(x.PropertyOwnerProtocol.Id, x.RealityObject.Id, null),
                                    DecisionType = x.TypeProtocol == PropertyOwnerProtocolType.ResolutionOfTheBoard
                                        ? CoreDecisionType.Government
                                        : x.TypeProtocol == PropertyOwnerProtocolType.FormationFund
                                            ? CoreDecisionType.Owners
                                            : (CoreDecisionType?)null
                                };
                            }))
                .OrderByDescending(x => x.ProtocolDate)
                .FirstOrDefault();
        }

        /// <summary>
        /// Получить прокси протокола или решения
        /// </summary>
        /// <param name="roId">Идентификатор жилого дома</param>
        /// <returns>Прокси протокола или решения</returns>
        public RealityObjectDecisionProtocolProxy GetBothProtocolProxy(long roId)
        {
            var ro = this.RealityObjectDomainService.Get(roId);

            if (ro.IsNull())
            {
                return null;
            }

            return this.GetBothProtocolProxy(ro);
        }

        /// <summary>
        /// Получить прокси протокола или решения
        /// </summary>
        /// <param name="realityObjects">Запрос домов</param>
        /// <returns>Прокси протокола или решения</returns>
        public Dictionary<long, RealityObjectDecisionProtocolProxy> GetBothProtocolProxy(IQueryable<RealityObject> realityObjects)
        {
            this.InitCache(realityObjects);

            Dictionary<long, RealityObjectDecisionProtocolProxy> result = null;
            this.container.InTransaction(() => result = this.GetDataInternal(realityObjects).ToDictionary(x => x.Key, x => x.OrderByDescending(y => y.ProtocolDate).FirstOrDefault()));
            return result;
        }

        /// <inheritdoc />
        public Dictionary<long, List<RealityObjectDecisionProtocolProxy>> GetAllBothProtocolProxy(IQueryable<RealityObject> realityObjects)
        {
            this.InitCache(realityObjects);

            Dictionary<long, List<RealityObjectDecisionProtocolProxy>> result = null;
            this.container.InTransaction(() => result = this.GetDataInternal(realityObjects).ToDictionary(x => x.Key, x => x.ToList()));
            return result;
        }

        /// <inheritdoc />
        public Dictionary<long, List<RealityObjectDecisionProtocolProxy>> GetAllBothProtocolByFormat(IQueryable<RealityObject> realityObjects)
        {
            return this.GetDataInternal(realityObjects, false).ToDictionary(x => x.Key, x => x.ToList());
        }

        private IEnumerable<IGrouping<long, RealityObjectDecisionProtocolProxy>> GetDataInternal(IQueryable<RealityObject> realityObjects, bool withPaySize = true)
        {
            return this.RegOpAccountDecisionDomain.GetAll()
                .Fetch(x => x.PropertyOwnerProtocol)
                .Where(x => realityObjects.Any(y => y.Id == x.RealityObject.Id) && x.PropertyOwnerProtocol.DocumentDate != null)
                .Select(x => new
                {
                    x.Id,
                    x.PropertyOwnerProtocol,
                    x.RegOperator,
                    x.RealityObject,
                    TypeProtocol = (PropertyOwnerProtocolType?) x.PropertyOwnerProtocol.TypeProtocol
                })
                .AsEnumerable()
                .Select(x => new RealityObjectDecisionProtocolProxy
                {
                    Id = x.PropertyOwnerProtocol.Id,
                    ProtocolDate = x.PropertyOwnerProtocol.DocumentDate.GetValueOrDefault(),
                    ProtocolNumber = x.PropertyOwnerProtocol.DocumentNumber,
                    RealityObjectId = x.RealityObject.Id,
                    RegOpContragentName = x.RegOperator?.Contragent.Name,
                    RegOpContragentInn = x.RegOperator?.Contragent.Inn,
                    CrPaySize = withPaySize ? this.GetPaysize(x.PropertyOwnerProtocol, x.RealityObject.Id, null) : default(decimal?),
                    DecisionType = x.TypeProtocol == PropertyOwnerProtocolType.ResolutionOfTheBoard
                        ? CoreDecisionType.Government
                        : x.TypeProtocol == PropertyOwnerProtocolType.FormationFund
                            ? CoreDecisionType.Owners
                            : (CoreDecisionType?) null
                })
                .Union(
                    this.SpecialAccountDecisionDomain.GetAll()
                        .Where(x => realityObjects.Any(y => y.Id == x.RealityObject.Id) && x.PropertyOwnerProtocol.DocumentDate != null)
                        .Select(x => new
                        {
                            x.Id,
                            x.PropertyOwnerProtocol,
                            x.RegOperator,
                            x.RealityObject,
                            x.ManagingOrganization,
                            TypeProtocol = (PropertyOwnerProtocolType?) x.PropertyOwnerProtocol.TypeProtocol
                        })
                        .AsEnumerable()
                        .Select(
                            x =>
                            {
                                Contragent contragent = null;
                                if (x.RegOperator != null)
                                {
                                    contragent = x.RegOperator.Contragent;
                                }
                                else if (x.ManagingOrganization != null)
                                {
                                    contragent = x.ManagingOrganization.Contragent;
                                }

                                return new RealityObjectDecisionProtocolProxy
                                {
                                    Id = x.PropertyOwnerProtocol.Id,
                                    ProtocolDate = x.PropertyOwnerProtocol.DocumentDate.GetValueOrDefault(),
                                    ProtocolNumber = x.PropertyOwnerProtocol.DocumentNumber,
                                    RealityObjectId = x.RealityObject.Id,
                                    RegOpContragentName = contragent != null ? contragent.Name : "",
                                    RegOpContragentInn = contragent != null ? contragent.Inn : "",
                                    CrPaySize = withPaySize ? this.GetPaysize(x.PropertyOwnerProtocol, x.RealityObject.Id, null) : default(decimal?),
                                    DecisionType = x.TypeProtocol == PropertyOwnerProtocolType.ResolutionOfTheBoard
                                        ? CoreDecisionType.Government
                                        : x.TypeProtocol == PropertyOwnerProtocolType.FormationFund
                                            ? CoreDecisionType.Owners
                                            : (CoreDecisionType?) null
                                };
                            }))
                .GroupBy(x => x.RealityObjectId);
        }

        /// <summary>
        /// Получить размер взноса на КР (либо из протокола, либо из справочника взносов)
        /// </summary>
        /// <param name="protocolId">Идентификатор протокола</param>
        /// <param name="roId">Идентификатор жилого дома</param>
        /// <returns>Размер взноса на КР</returns>
        public decimal? GetPaysize(long protocolId, long roId, DateTime? yearStart)
        {
            var protocol = this.PropertyOwnerProtocolsDomainService.Get(protocolId);
            return this.GetPaysize(protocol, roId, yearStart);
        }

        public IDataResult CheckDecisionProtocol(RealityObject ro)
        {
            return new BaseDataResult();
        }

        public decimal? GetPaysize(PropertyOwnerProtocols protocol, long roId, DateTime? yearStart)
        {
            decimal? paySize = 0m;
            if (protocol == null)
            {
                return paySize;
            }

            var value = this.paySizeByProtocolCache != null 
                ? this.paySizeByProtocolCache?.Get(protocol.Id)?.FirstOrDefault()
                : this.MinAmountDecisionDomainService.GetAll()
                    .Where(x => x.PropertyOwnerProtocol.Id == protocol.Id)
                    .Where(x => x.PropertyOwnerDecisionType == PropertyOwnerDecisionType.SetMinAmount)
                    .Select(x => new PaySizeProxy
                    {
                        PaymentSize = x.SizeOfPaymentOwners,
                        DateStartPeriod = x.ObjectCreateDate
                    }).FirstOrDefault();

            var paysizes = this.GetPaysizes(value, protocol);

            if (paysizes != null)
            {
                paySize = paysizes;
            }

            return paySize;
        }

        private decimal? GetPaysizes(PaySizeProxy decision, PropertyOwnerProtocols protocol)
        {
            return decision == null
                ? this.GetDefaultPaysizes(protocol)
                : this.GetDecisionPaysizes(decision, protocol.RealityObject.Municipality.Id);
        }

        private decimal? GetDefaultPaysizes(PropertyOwnerProtocols protocol)
        {
            decimal? results;

            using (this.container.Using(this.PaymentSizeMuRecordDomainService, this.RealityObjectDomainService))
            {
                var mu = protocol.RealityObject?.Municipality.Id ?? 0;

                var paysizeRecord = this.paysizeRecordByMunicipaltyCache != null 
                    ? (this.paysizeRecordByMunicipaltyCache.Get(mu) ?? this.paysizeRecordByMunicipaltyCache.Get(mu))
                        ?.Max(x => x.PaymentSize) ?? 0

                    : this.PaysizeRecordDomainService.GetAll()
                        .Where(x => x.Municipality.Id == mu)
                        .Max(x => (decimal?)x.Value) ?? 0;

                results = paysizeRecord;
            }

            return results;
        }

        private decimal? GetDecisionPaysizes(PaySizeProxy decision, long muId)
        {
            decimal? size;
            if (decision.PaymentSize == 0)
            {
                size = this.paySizeByMunicipaltyCache != null 
                    ? this.paySizeByMunicipaltyCache
                        .Get(muId)
                        .Where(x => x.TypeIndicator == TypeIndicator.MinSizeSqMetLivinSpace)
                        .Where(x => x.DateStartPeriod < decision.DateStartPeriod)
                        .FirstOrDefault(x => !x.DateEndPeriod.HasValue || x.DateEndPeriod > decision.DateStartPeriod)?.PaymentSize

                    : this.PaymentSizeMuRecordDomainService.GetAll()
                        .Where(x => x.Municipality.Id == muId)
                        .Where(x => x.PaymentSizeCr.TypeIndicator == TypeIndicator.MinSizeSqMetLivinSpace)
                        .Where(x => x.PaymentSizeCr.DateStartPeriod < decision.DateStartPeriod)
                        .Where(
                            x => !x.PaymentSizeCr.DateEndPeriod.HasValue
                                || x.PaymentSizeCr.DateEndPeriod > decision.DateStartPeriod)
                        .Select(x => x.PaymentSizeCr.PaymentSize)
                        .FirstOrDefault();
            }
            else
            {
                size = decision.PaymentSize;
            }

            return size;
        }

        private void InitCache(IQueryable<RealityObject> realityObjects)
        {
            this.paySizeByMunicipaltyCache?.Clear();
            this.paySizeByMunicipaltyCache = this.PaymentSizeMuRecordDomainService.GetAll()
                .Where(x => realityObjects.Any(y => y.Municipality.Id == x.Municipality.Id))
                .Select(x => new
                {
                    MunId = x.Municipality.Id,
                    x.Id,
                    x.PaymentSizeCr.DateStartPeriod,
                    x.PaymentSizeCr.DateEndPeriod,
                    x.PaymentSizeCr.PaymentSize,
                    x.PaymentSizeCr.TypeIndicator
                })
                .AsEnumerable()
                .GroupBy(
                    x => x.MunId,
                    x => new PaySizeProxy
                    {
                        RecordId = x.Id,
                        DateStartPeriod = x.DateStartPeriod,
                        DateEndPeriod = x.DateEndPeriod,
                        PaymentSize = x.PaymentSize,
                        TypeIndicator = x.TypeIndicator
                    })
                .ToDictionary(x => x.Key, x => x.ToArray());

            this.paySizeByProtocolCache?.Clear();
            this.paySizeByProtocolCache = this.MinAmountDecisionDomainService.GetAll()
                .Where(x => realityObjects.Any(y => y.Id == x.RealityObject.Id))
                .Where(x => x.PropertyOwnerDecisionType == PropertyOwnerDecisionType.SetMinAmount)
                .Where(x => x.SizeOfPaymentOwners != 0)
                .Select(x => new
                {
                    ProtocolId = x.PropertyOwnerProtocol.Id,
                    x.SizeOfPaymentOwners,
                    x.ObjectCreateDate
                })
                .AsEnumerable()
                .GroupBy(
                    x => x.ProtocolId,
                    x => new PaySizeProxy
                    {
                        PaymentSize = x.SizeOfPaymentOwners,
                        DateStartPeriod = x.ObjectCreateDate
                    })
                .ToDictionary(x => x.Key, x => x.ToArray());

            this.paysizeRecordByMunicipaltyCache?.Clear();
            this.paysizeRecordByMunicipaltyCache = this.PaysizeRecordDomainService.GetAll()
                .Where(x => realityObjects.Any(y => y.Municipality.Id == x.Municipality.Id))
                .Select(x => new
                {
                    MunId = x.Municipality.Id,
                    x.Value,
                    x.Paysize.Indicator,
                    x.Paysize.DateStart,
                    x.Paysize.DateEnd
                })
                .AsEnumerable()
                .GroupBy(
                    x => x.MunId,
                    x => new PaySizeProxy
                    {
                        DateStartPeriod = x.DateStart,
                        DateEndPeriod = x.DateEnd,
                        PaymentSize = x.Value ?? 0,
                        TypeIndicator = x.Indicator
                    })
                .ToDictionary(x => x.Key, x => x.ToArray());
        }

        private class PaySizeProxy
        {
            public long RecordId { get; set; }

            public Overhaul.Enum.TypeIndicator TypeIndicator { get;set; }
            public DateTime? DateStartPeriod { get; set; }
            public DateTime? DateEndPeriod { get; set; }
            public decimal PaymentSize { get; set; }
        }
    }
}
