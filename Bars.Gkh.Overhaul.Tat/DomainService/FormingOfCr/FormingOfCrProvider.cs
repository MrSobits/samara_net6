namespace Bars.Gkh.Overhaul.Tat.DomainService.FormingOfCr
{
    using System.Collections.Generic;
    using System.Linq;
    using B4;

    using Bars.Gkh.ConfigSections.RegOperator;
    using Bars.Gkh.DomainService.RegionalFormingOfCr;
    using Bars.Gkh.Utils;

    using Castle.Windsor;

    using Entities;
    using Enum;
    using Enums;
    using Gkh.Entities;

    public class FormingOfCrProvider : ITypeOfFormingCrProvider
    {
        protected readonly IWindsorContainer container;
        private readonly IDomainService<BasePropertyOwnerDecision> basePropertyOwnerDecisionDomain;
        private readonly IDomainService<SpecialAccountDecision> domainService;

        public FormingOfCrProvider(
            IWindsorContainer container, 
            IDomainService<BasePropertyOwnerDecision> basePropertyOwnerDecisionDomain, 
            IDomainService<SpecialAccountDecision> domainService)
        {
            this.basePropertyOwnerDecisionDomain = basePropertyOwnerDecisionDomain;
            this.domainService = domainService;
            this.container = container;
        }

        public CrFundFormationType GetTypeOfFormingCr(RealityObject realityObj)
        {
            var type = this.basePropertyOwnerDecisionDomain.GetAll()
                .Where(x => x.RealityObject.Id == realityObj.Id &&
                        (x.PropertyOwnerProtocol.TypeProtocol == PropertyOwnerProtocolType.FormationFund
                        || x.PropertyOwnerProtocol.TypeProtocol == PropertyOwnerProtocolType.ResolutionOfTheBoard) 
                            && x.PropertyOwnerDecisionType == PropertyOwnerDecisionType.SelectMethodForming)
                .OrderByDescending(x => x.PropertyOwnerProtocol.DocumentDate)
                .Select(
                    x => new ProtocolProxy
                    {
                        MethodFormFundCr = x.MethodFormFund,
                        ProtocolId = x.PropertyOwnerProtocol.Id
                    })
                .FirstOrDefault();
                

            return this.GetTypeOfFormingCrInternal(type);
        }

        private CrFundFormationType GetTypeOfFormingCrInternal(ProtocolProxy data)
        {
            if (data == null)
            {
                return CrFundFormationType.NotSelected;
            }

            if (data.MethodFormFundCr == MethodFormFundCr.RegOperAccount)
            {
                return CrFundFormationType.RegOpAccount;
            }

            if (data.MethodFormFundCr != MethodFormFundCr.SpecialAccount)
            {
                return CrFundFormationType.NotSelected;
            }

            var orgType = this.domainService.GetAll()
                .Where(x => x.PropertyOwnerProtocol.Id == data.ProtocolId)
                .Select(x => new {x.TypeOrganization})
                .FirstOrDefault();

            if (orgType != null && orgType.TypeOrganization == TypeOrganization.RegOperator)
            {
                return CrFundFormationType.SpecialRegOpAccount;
            }
            if (orgType != null
                && (orgType.TypeOrganization == TypeOrganization.TSJ || orgType.TypeOrganization == TypeOrganization.ManOrg
                    || orgType.TypeOrganization == TypeOrganization.JSK))
            {
                return CrFundFormationType.SpecialAccount;
            }
            return CrFundFormationType.Unknown;
        }

        public Dictionary<long, CrFundFormationType> GetTypeOfFormingCr(IQueryable<RealityObject> realityObjs)
        {
            return this.basePropertyOwnerDecisionDomain.GetAll()
                .Where(
                    x => realityObjs.Any(y => y.Id == x.RealityObject.Id) && (
                        x.PropertyOwnerProtocol.TypeProtocol == PropertyOwnerProtocolType.FormationFund
                            || x.PropertyOwnerProtocol.TypeProtocol == PropertyOwnerProtocolType.ResolutionOfTheBoard)
                            && x.PropertyOwnerDecisionType == PropertyOwnerDecisionType.SelectMethodForming)
                .Select(
                    x => new
                    {
                        RoId = x.RealityObject.Id,
                        x.MethodFormFund,
                        ProtocolId = x.PropertyOwnerProtocol.Id,
                        ProtocolDate = x.PropertyOwnerProtocol.DocumentDate
                    })
                .AsEnumerable()
                .GroupBy(x => x.RoId)
                .ToDictionary(
                    x => x.Key,
                    y => y.OrderByDescending(x => x.ProtocolDate).Select(
                        x =>
                        {
                            var protocolProxy = new ProtocolProxy
                            {
                                MethodFormFundCr = x.MethodFormFund,
                                ProtocolId = x.ProtocolId
                            };

                            return this.GetTypeOfFormingCrInternal(protocolProxy);
                        }).FirstOrDefault());
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

        private class ProtocolProxy
        {
            public MethodFormFundCr? MethodFormFundCr { get; set; }
            public long ProtocolId { get; set; }
        }
    }
}
