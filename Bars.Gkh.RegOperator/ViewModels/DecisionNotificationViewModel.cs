namespace Bars.Gkh.RegOperator.ViewModel
{
    using System;
    using System.Linq;
    using B4;
    using B4.Utils;

    using Bars.Gkh.Modules.RegOperator.Entities.RegOperator;
    using Bars.Gkh.Utils;

    using Decisions.Nso.Entities;
    using Gkh.Domain;
    using Gkh.DomainService;
    using Gkh.Entities;
    using Gkh.Enums;
    using Gkh.Enums.Decisions;

    public class DecisionNotificationViewModel : BaseViewModel<DecisionNotification>
    {
        public IDomainService<RealityObjectServiceOrg> RoServiceOrgDomain { get; set; }
        public IDomainService<ManOrgContractRealityObject> RoManOrgRoDomain { get; set; }
        public IDomainService<RegOperator> RegOperatorDomain { get; set; }
        public IDomainService<CreditOrgDecision> CreditOrgDecisionDomain { get; set; }
        public IDomainService<CrFundFormationDecision> FundFormationDecisionDomain{get;set;}
        public IDomainService<AccountOwnerDecision> AccountOwnerDecisionDomain{get;set;}
        public IManagingOrgRealityObjectService ManagingOrgRealityObjectService { get; set; }

        public override IDataResult List(IDomainService<DecisionNotification> domainService, BaseParams baseParams)
        {
            var manOrgByRoQuery = this.RoManOrgRoDomain.GetAll()
                .Select(x => new
                {
                    x.ManOrgContract.StartDate,
                    x.ManOrgContract.EndDate,
                    RoId = x.RealityObject.Id,
                    ContragentName = x.ManOrgContract.ManagingOrganization.Contragent.Name
                });

            var regopName = this.RegOperatorDomain.GetAll()
                .Where(x => x.Contragent.ContragentState == ContragentState.Active)
                .Select(x => x.Contragent.Name)
                .FirstOrDefault();

            var result = domainService.GetAll()
                .Select(x => new
                {
                    roId = x.Protocol.RealityObject.Id,
                    protocolId = x.Protocol.Id,
                    x.Protocol.DateStart,
                    x.Id,
                    x.Number,
                    Date = x.Date <= DateTime.MinValue ? (DateTime?) null : x.Date,
                    x.AccountNum,
                    OpenDate = x.OpenDate <= DateTime.MinValue ? (DateTime?) null : x.OpenDate,
                    CloseDate = x.CloseDate <= DateTime.MinValue ? (DateTime?) null : x.CloseDate,
                    x.IncomeNum,
                    RegistrationDate = x.RegistrationDate <= DateTime.MinValue ? (DateTime?) null : x.RegistrationDate,
                    x.Protocol.RealityObject.Address,
                    Mu = x.Protocol.RealityObject.Municipality.Name,
                    MoSettlement = x.Protocol.RealityObject.MoSettlement.Name,
                    x.State,
                    HasCertificate = x.BankDoc != null ? "Да" : "Нет",
                    OrgName = this.AccountOwnerDecisionDomain.GetAll()
                            .Where(y => y.Protocol == x.Protocol)
                            .Any(y => y.DecisionType == AccountOwnerDecisionType.Custom)
                        ? manOrgByRoQuery.Where(y => y.StartDate <= x.Protocol.DateStart)
                            .Where(y => !y.EndDate.HasValue || y.EndDate >= x.Protocol.DateStart)
                            .Where(y => y.RoId == x.Protocol.RealityObject.Id)
                            .OrderByDescending(y => y.StartDate)
                            .Select(y => y.ContragentName)
                            .FirstOrDefault()
                            : regopName,
                    FormFundType = (CrFundFormationDecisionType?)this.FundFormationDecisionDomain.GetAll()
                        .Where(y => y.Protocol == x.Protocol)
                        .Select(y => y.Decision)
                        .FirstOrDefault(),
                    CreditOrgName = this.CreditOrgDecisionDomain.GetAll()
                        .Where(y => y.Protocol == x.Protocol)
                        .Select(y => y.Decision.Name)
                        .FirstOrDefault()
                });

            return result.ToListDataResult(baseParams.GetLoadParam(), this.Container);
        }

        public override IDataResult Get(IDomainService<DecisionNotification> domainService, BaseParams baseParams)
        {
            var notification = domainService.Get(baseParams.Params.GetAsId("id"));

            var protocolId = notification.Return(x => x.Protocol).Return(x => x.Id);
            var ro = notification.Return(x => x.Protocol).Return(x => x.RealityObject);

            var creditOrg = this.CreditOrgDecisionDomain.GetAll()
                .Where(x => x.Protocol.Id == protocolId)
                .Select(x => x.Decision)
                .FirstOrDefault();

            var crFundFormationDecision = this.FundFormationDecisionDomain.GetAll()
                .Where(x => x.Protocol.Id == protocolId)
                .Select(x => (CrFundFormationDecisionType?) x.Decision)
                .FirstOrDefault();

            var owner = this.AccountOwnerDecisionDomain.GetAll()
                .Where(x => x.Protocol.Id == protocolId)
                .Select(x => (AccountOwnerDecisionType?) x.DecisionType)
                .FirstOrDefault();

            var regOpCalcAccount = this.Container.Resolve<IDomainService<RegOperator>>().GetAll()
                .FirstOrDefault(x => x.Contragent.ContragentState == ContragentState.Active)
                .Return(x => new
                {
                    Id = x.Return(y => y.Contragent).Return(y => y.Id),
                    Name = x.Return(y => y.Contragent).Return(y => y.Name),
                    Inn = x.Return(y => y.Contragent).Return(y => y.Inn),
                    Kpp = x.Return(y => y.Contragent).Return(y => y.Kpp),
                    Ogrn = x.Return(y => y.Contragent).Return(y => y.Ogrn),
                    Oktmo = x.Return(y => y.Contragent).Return(y => y.Oktmo),
                    MailingAddress = x.Return(y => y.Contragent).Return(y => y.MailingAddress)
                });

            //Способ управления МКД
            var contract = this.Container.Resolve<IManagingOrgRealityObjectService>().GetManOrgOnDate(ro, notification.Protocol.DateStart);

            string typeManag = null;
            Contragent contragent = null;

            if (contract != null)
            {
                typeManag = contract.ManagingOrganization?.TypeManagement.GetDisplayName();
                contragent = contract.ManagingOrganization?.Contragent;
            }

            return new BaseDataResult(new
            {
                notification.Id,
                Protocol = notification.Protocol.Id,
                notification.Number,
                notification.Date,
                notification.Document,
                notification.ProtocolFile,
                notification.AccountNum,
                notification.OpenDate,
                notification.CloseDate,
                notification.BankDoc,
                notification.IncomeNum,
                notification.RegistrationDate,
                notification.OriginalIncome,
                notification.CopyIncome,
                notification.CopyProtocolIncome,
                notification.State,
                MoSettlement = ro.Return(x => x.MoSettlement).Return(x => x.Name),
                Mu = ro.Return(x => x.Municipality).Return(x => x.Name),
                Address = ro.Return(x => x.Address),
                ro?.AreaLivingNotLivingMkd,
                Manage = typeManag,
                FormFundType = (crFundFormationDecision ?? CrFundFormationDecisionType.Unknown).GetDisplayName(),
                CreditOrgName = creditOrg.Return(x => x.Name),
                CreditOrgAddress = creditOrg.Return(x => x.Address),
                CreditOrgBik = creditOrg.Return(x => x.Bik),
                CreditOrgCorAcc = creditOrg.Return(x => x.CorrAccount),
                CreditOrgInn = creditOrg.Return(x => x.Inn),
                CreditOrgKpp = creditOrg.Return(x => x.Kpp),
                CreditOrgOgrn = creditOrg.Return(x => x.Ogrn),
                CreditOrgOktmo = "",
                OrgName =
                    owner.HasValue && owner == AccountOwnerDecisionType.Custom
                        ? contragent.Return(x => x.Name)
                        : regOpCalcAccount.Return(x => x.Name),

                PostAddress =
                    owner.HasValue && owner == AccountOwnerDecisionType.Custom
                        ? contragent.Return(x => x.MailingAddress)
                        : regOpCalcAccount.Return(x => x.MailingAddress),

                Inn =
                    owner.HasValue && owner == AccountOwnerDecisionType.Custom
                        ? contragent.Return(x => x.Inn)
                        : regOpCalcAccount.Return(x => x.Inn),

                Kpp =
                    owner.HasValue && owner == AccountOwnerDecisionType.Custom
                        ? contragent.Return(x => x.Kpp)
                        : regOpCalcAccount.Return(x => x.Kpp),

                Ogrn =
                    owner.HasValue && owner == AccountOwnerDecisionType.Custom
                        ? contragent.Return(x => x.Ogrn)
                        : regOpCalcAccount.Return(x => x.Ogrn),

                Oktmo =
                    owner.HasValue && owner == AccountOwnerDecisionType.Custom
                        ? contragent.Return(x => x.Oktmo).ToStr()
                        : regOpCalcAccount.Return(x => x.Oktmo).ToStr(),

                ProtocolId = protocolId,
                RealObjId = ro.Return(x => x.Id)
            });
        }
    }
}