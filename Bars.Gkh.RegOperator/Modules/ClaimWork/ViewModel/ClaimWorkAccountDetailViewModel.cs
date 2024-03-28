namespace Bars.Gkh.RegOperator.Modules.ViewModel
{
    using System.Linq;
    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.Utils;

    public class ClaimWorkAccountDetailViewModel : BaseViewModel<ClaimWorkAccountDetail>
    {
        public override IDataResult List(IDomainService<ClaimWorkAccountDetail> domainService, BaseParams baseParams)
        {
            var claimWorkId = baseParams.Params.GetAsId("claimWorkId");
            var claimWorkIds = baseParams.Params.GetAs<long[]>("claimWorkIds");
            var excludeAccountIds = baseParams.Params.GetAs<long[]>("excludeAccountIds");

            var loadParam = baseParams.GetLoadParam();
            var query = domainService.GetAll()
                .WhereIf(claimWorkId != 0, x => x.ClaimWork.Id == claimWorkId)
                .WhereIf(claimWorkIds.IsNotEmpty(), x=> claimWorkIds.Contains(x.ClaimWork.Id));

            if (excludeAccountIds != null)
            {
                query = query.Where(x => !excludeAccountIds.Contains(x.PersonalAccount.Id));
            }

            return query.Select(x => new
                {
                    x.Id,
                    AccountId = x.PersonalAccount.Id,
                    Municipality = x.PersonalAccount.Room.RealityObject.Municipality.Name,
                    RoomAddress = x.PersonalAccount.Room.RealityObject.Address + ", кв. " + x.PersonalAccount.Room.RoomNum,
                    x.PersonalAccount.PersonalAccountNum,
                    OwnerName = x.PersonalAccount.AccountOwner.Name,
                    PersAccState = x.PersonalAccount.State,
                    x.CurrChargeBaseTariffDebt,
                    x.CurrChargeDecisionTariffDebt,
                    x.CurrChargeDebt,
                    x.CurrPenaltyDebt,
                    x.CountDaysDelay,
                    x.CountMonthDelay
                })
                .ToListDataResult(loadParam);
        }
    }
}