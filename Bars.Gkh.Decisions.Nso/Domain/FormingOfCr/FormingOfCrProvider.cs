namespace Bars.Gkh.Decisions.Nso.Domain.FormingOfCr
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using B4;

    using Bars.Gkh.ConfigSections.RegOperator;
    using Bars.Gkh.DomainService.RegionalFormingOfCr;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Utils;

    using Castle.Windsor;

    using Entities;
    using Entities.Decisions;
    using Gkh.Entities;
    using Gkh.Enums.Decisions;

    /// <summary>
    /// 
    /// </summary>
    public class FormingOfCrProvider : ITypeOfFormingCrProvider
    {
        protected readonly IWindsorContainer container;

        private readonly IDomainService<CrFundFormationDecision> crDomain;
        private readonly IDomainService<AccountOwnerDecision> accDomain;
        private readonly IDomainService<GovDecision> govDecisionDomain;

        /// <summary>
        /// конструктор
        /// </summary>
        /// <param name="container"></param>
        /// <param name="crDomain"></param>
        /// <param name="govDecisionDomain"></param>
        /// <param name="accDomain"></param>
        public FormingOfCrProvider(
            IWindsorContainer container,
            IDomainService<CrFundFormationDecision> crDomain, 
            IDomainService<GovDecision> govDecisionDomain, 
            IDomainService<AccountOwnerDecision> accDomain)
        {
            this.crDomain = crDomain;
            this.accDomain = accDomain;
            this.govDecisionDomain = govDecisionDomain;
            this.container = container;
        }

        /// <summary>
        /// Определить способ формирования фонда дома
        /// </summary>
        /// <param name="realityObj"></param>
        /// <returns></returns>
        public CrFundFormationType GetTypeOfFormingCr(RealityObject realityObj)
        {
            var queries = new[]
            {
                // выбираем "Протокол решения собственников жилых помещений" с максимальной датой
                (
                    from v in this.crDomain.GetAll()
                    where v.Protocol.RealityObject.Id == realityObj.Id && v.Protocol.State.FinalState
                    orderby
                        v.Protocol.DateStart <= DateTime.MinValue ? v.Protocol.ProtocolDate : v.Protocol.DateStart
                            descending
                    select
                        new
                        {
                            v.Decision,
                            ProtocolId = v.Protocol.Id,
                            Date =
                                v.Protocol.DateStart <= DateTime.MinValue
                                    ? v.Protocol.ProtocolDate
                                    : v.Protocol.DateStart
                        })
                        .FirstOrDefault(),
                // выбираем "Протокол решения органа государственной власти" с максимальной датой
                (
                    from v in this.govDecisionDomain.GetAll()
                    where v.RealityObject.Id == realityObj.Id && v.State.FinalState
                    orderby
                        v.DateStart <= DateTime.MinValue ? v.ProtocolDate : v.DateStart
                            descending
                    select
                        new
                        {
                            Decision = v.FundFormationByRegop
                                ? CrFundFormationDecisionType.RegOpAccount
                                : CrFundFormationDecisionType.Unknown,
                            ProtocolId = v.Id,
                            Date = v.DateStart <= DateTime.MinValue ? v.ProtocolDate : v.DateStart
                        }).FirstOrDefault()
            };

            var data = (
                from v in queries
                where v != null
                orderby v.Date descending
                select v)
                .FirstOrDefault();

            if (data == null)
        {
                return CrFundFormationType.Unknown;
            }
            var desicion = data.Decision;

            if (desicion == CrFundFormationDecisionType.RegOpAccount)
            {
                return CrFundFormationType.RegOpAccount;
            }

            var accountOwner =
                (
                    from z in this.accDomain.GetAll()
                    where z.Protocol.Id == data.ProtocolId
                    select
                        //Владелец счета
                        z.DecisionType)
                    .FirstOrDefault();

            if (desicion == CrFundFormationDecisionType.SpecialAccount && accountOwner == AccountOwnerDecisionType.RegOp)
            {
                return CrFundFormationType.SpecialRegOpAccount;
            }

            if (desicion == CrFundFormationDecisionType.SpecialAccount && accountOwner == AccountOwnerDecisionType.Custom)
            {
                return CrFundFormationType.SpecialAccount;
            }

            return CrFundFormationType.Unknown;
        }

        public Dictionary<long, CrFundFormationType> GetTypeOfFormingCr(IQueryable<RealityObject> realityObjs)
        {
            return realityObjs.ToDictionary(x => x.Id, x => x.AccountFormationVariant ?? CrFundFormationType.Unknown);
        }

        /// <summary>
        /// Метод возвращает способы формирования фонда, которые участвуют в расчётах
        /// </summary>
        /// <returns>Список способов формирования</returns>
        public IList<CrFundFormationType> GetCrFundFormationTypesFromSettings()
        {
            var listResult = new List<CrFundFormationType>();
            var regopSettings = this.container.GetGkhConfig<RegOperatorConfig>().GeneralConfig;

            var accountFormationCalculable = regopSettings.HouseCalculationConfig;

            if (accountFormationCalculable.RegopCalcAccount)
            {
                listResult.Add(CrFundFormationType.RegOpAccount);
            }

            if (accountFormationCalculable.RegopSpecialCalcAccount)
            {
                listResult.Add(CrFundFormationType.SpecialRegOpAccount);
            }

            // по спеч счетам и неактивным пока вообще не считаем
            return listResult;
        }
    }
}