namespace Bars.Gkh.RegOperator.ViewModel
{
    using System.Linq;
    using B4;
    using B4.Utils;
    using Entities.Dict;

    public class PaymentPenaltiesExcludePersAccViewModel : BaseViewModel<PaymentPenaltiesExcludePersAcc>
    {
        public override IDataResult List(IDomainService<PaymentPenaltiesExcludePersAcc> domain, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);
            var payPenaltiesId = loadParams.Filter.GetAs<long>("payPenaltiesId");

            var data = domain.GetAll()
                 .Where(x => x.PaymentPenalties.Id == payPenaltiesId)
                 .Select(x => new
                 {
                     x.Id,
                     x.PersonalAccount.Room.RealityObject.Address,
                     Municipality = x.PersonalAccount.Room.RealityObject.Municipality.Name,
                     x.PersonalAccount.PersonalAccountNum,
                 })
                 .OrderIf(loadParams.Order.Length == 0, true, x => x.Municipality)
                 .OrderThenIf(loadParams.Order.Length == 0, true, x => x.Address)
                 .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }
    }
}