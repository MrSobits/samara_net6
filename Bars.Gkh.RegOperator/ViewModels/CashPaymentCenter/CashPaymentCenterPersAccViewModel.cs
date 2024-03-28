namespace Bars.Gkh.RegOperator.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.RegOperator.Entities;

    public class CashPaymentCenterPersAccViewModel : BaseViewModel<CashPaymentCenterPersAcc>
    {
        public override IDataResult List(IDomainService<CashPaymentCenterPersAcc> domain, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);
            var cashPaymentCenterId = loadParams.Filter.GetAs<long>("cashPaymentCenterId");

            var data = domain.GetAll()
                .WhereIf(cashPaymentCenterId > 0, x => x.CashPaymentCenter.Id == cashPaymentCenterId)
                .Select(x => new
                {
                    x.Id,
                    PersonalAccount = x.PersonalAccount.PersonalAccountNum,
                    x.PersonalAccount.Room.RealityObject.Address,
                    Municipality = x.PersonalAccount.Room.RealityObject.Municipality.Name,
                    x.DateStart,
                    x.DateEnd
                })
                .OrderIf(loadParams.Order.Length == 0, true, x => x.Municipality)
                .OrderThenIf(loadParams.Order.Length == 0, true, x => x.Address)
                .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }
    }
}