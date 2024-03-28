namespace Bars.Gkh.RegOperator.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.RegOperator.Entities;

    public class CashPaymentCenterRealObjViewModel : BaseViewModel<CashPaymentCenterRealObj>
    {
        public override IDataResult List(IDomainService<CashPaymentCenterRealObj> domain, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);
            var cashPaymentCenterId = loadParams.Filter.GetAs<long>("cashPaymentCenterId");
            var realityObjectId = loadParams.Filter.GetAs<long>("realityObjectId");
            var data = domain.GetAll()
                 .WhereIf(cashPaymentCenterId > 0, x => x.CashPaymentCenter.Id == cashPaymentCenterId)
                 .WhereIf(realityObjectId > 0, x => x.RealityObject.Id == realityObjectId)
                 .Select(x => new
                 {
                     x.Id,
                     x.RealityObject.Address,
                     Municipality = x.RealityObject.Municipality.Name,
                     x.CashPaymentCenter.Contragent.Name,
                     x.CashPaymentCenter.Contragent.Inn,
                     x.CashPaymentCenter.Contragent.Kpp,
                     x.CashPaymentCenter.Contragent.ContragentState,
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