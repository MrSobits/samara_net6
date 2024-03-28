namespace Bars.Gkh.Regions.Tatarstan.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Modules.ClaimWork.DomainService;
    using Bars.Gkh.Regions.Tatarstan.Entities.UtilityDebtor;

    public class UtilityDebtorClaimWorkViewModel : BaseViewModel<UtilityDebtorClaimWork>
    {
        public override IDataResult List(IDomainService<UtilityDebtorClaimWork> domainService, BaseParams baseParams)
        {
            var service = this.Container.Resolve<IBaseClaimWorkService<UtilityDebtorClaimWork>>();

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

        public override IDataResult Get(IDomainService<UtilityDebtorClaimWork> domainService, BaseParams baseParams)
        {
            var id = baseParams.Params.GetAsId();

            var entity = domainService.GetAll()
                .Where(x => x.Id == id)
                .Select(x => new
                {
                    x.Id,
                    x.State,
                    Municipality = x.RealityObject.Municipality.Name,
                    Settlement = x.RealityObject.MoSettlement.Name,
                    x.RealityObject,
                    x.AccountOwner,
                    x.OwnerType,
                    x.PersonalAccountState,
                    x.PersonalAccountNum,
                    x.ChargeDebt,
                    x.PenaltyDebt,
                    x.CountDaysDelay,
                    x.IsDebtPaid,
                    x.DebtPaidDate
                })
                .FirstOrDefault();

            return new BaseDataResult(entity);
        }
    }
}