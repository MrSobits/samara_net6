namespace Bars.Gkh.RegOperator.Modules.ViewModel
{
    using System.Linq;
    using Bars.B4;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Modules.ClaimWork.DomainService;
    using Bars.Gkh.RegOperator.Entities;

    public class LegalClaimWorkViewModel : BaseViewModel<LegalClaimWork>
    {       
        public override IDataResult List(IDomainService<LegalClaimWork> domainService, BaseParams baseParams)
        {
            var service = this.Container.Resolve<IBaseClaimWorkService<LegalClaimWork>>();

            try
            {
                var totalCount = 0;
                var result = service.GetList(baseParams, true, ref totalCount);
                return new ListDataResult(result, totalCount);
            }
            finally
            {
                this.Container.Release(service);
            }
        }
        
        public override IDataResult Get(IDomainService<LegalClaimWork> domainService, BaseParams baseParams)
        {
            var id = baseParams.Params.GetAsId();

            var value = domainService.GetAll()
                .Where(x => x.Id == id)
                .Select(
                    x => new
                    {
                        x.Id,
                        MunicipalityId = ((LegalAccountOwner) x.AccountOwner).Contragent.Municipality != null
                            ? ((LegalAccountOwner) x.AccountOwner).Contragent.Municipality.Id
                            : 0,
                        AccountOwnerName = ((LegalAccountOwner) x.AccountOwner).Contragent.Name,
                        ((LegalAccountOwner) x.AccountOwner).Contragent.JuridicalAddress,
                        ((LegalAccountOwner) x.AccountOwner).Contragent.FactAddress,
                        x.JurisdictionAddress,

                        ((LegalAccountOwner) x.AccountOwner).Contragent.Inn,
                        ((LegalAccountOwner) x.AccountOwner).Contragent.Kpp,
                        OrganizationForm = ((LegalAccountOwner) x.AccountOwner).Contragent.OrganizationForm.Name,
                        ParentContragentName = ((LegalAccountOwner) x.AccountOwner).Contragent.Parent != null
                            ? ((LegalAccountOwner) x.AccountOwner).Contragent.Parent.Name
                            : string.Empty,
                        ((LegalAccountOwner) x.AccountOwner).Contragent.ContragentState,
                        ((LegalAccountOwner) x.AccountOwner).Contragent.DateTermination,

                        x.CurrChargeBaseTariffDebt,
                        x.CurrChargeDecisionTariffDebt,
                        x.CurrChargeDebt,
                        x.CurrPenaltyDebt,
                        x.OrigChargeBaseTariffDebt,
                        x.OrigChargeDecisionTariffDebt,
                        x.OrigChargeDebt,
                        x.OrigPenaltyDebt,
                        StateName = x.State.Name,
                        x.DebtorState,
                        x.IsDebtPaid,
                        x.DebtPaidDate,
                        x.PIRCreateDate,
                        x.SubContractDate,
                        x.SubContractNum,
                        x.SubContragent,

                        x.AccountOwner,
                        x.State,
                        HasCharges185FZ = x.RealityObject.HasCharges185FZ,

                        OperManagement = x.OperManagement != null ? x.OperManagement.Name : string.Empty,
                        x.OperManReason,
                        x.OperManDate
                    })
                .FirstOrDefault();

            return new BaseDataResult(value);
        }
    }
}