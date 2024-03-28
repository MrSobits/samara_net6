namespace Bars.Gkh.RegOperator.Modules.ViewModel
{
    using System.Linq;
    using Bars.B4;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Modules.ClaimWork.DomainService;
    using Bars.Gkh.RegOperator.Entities;

    public class IndividualClaimWorkViewModel : BaseViewModel<IndividualClaimWork>
    {
        /// <inheritdoc />
        public override IDataResult List(IDomainService<IndividualClaimWork> domainService, BaseParams baseParams)
        {
            var service = this.Container.Resolve<IBaseClaimWorkService<IndividualClaimWork>>();

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

        /// <inheritdoc />
        public override IDataResult Get(IDomainService<IndividualClaimWork> domainService, BaseParams baseParams)
        {
            var id = baseParams.Params.GetAsId();

            var value = domainService.GetAll()
                .Where(x => x.Id == id)
                .Select(x => new
                {
                    x.Id,
                    AccountOwnerName = ((IndividualAccountOwner)x.AccountOwner).Name,
                    RegistrationAddress = ((IndividualAccountOwner)x.AccountOwner).RegistrationAddress != null
                        ? ((IndividualAccountOwner)x.AccountOwner).RegistrationAddress.Address
                        : string.Empty,
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
                    x.AccountOwner,
                    x.DebtorType,
                    x.ClaimWorkTypeBase,
                    x.PIRCreateDate,
                    x.SubContractDate,
                    x.SubContractNum,
                    x.SubContragent,
                    x.State,
                    HasCharges185FZ = x.RealityObject.HasCharges185FZ
                })
                .FirstOrDefault();

            return new BaseDataResult(value);
        }
    }
}