namespace Bars.Gkh.RegOperator.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.RegOperator.Entities;

    public class CashPaymentCenterMuViewModel : BaseViewModel<CashPaymentCenterMunicipality>
    {
        public override IDataResult List(IDomainService<CashPaymentCenterMunicipality> domain, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var cashPaymentCenterId = loadParams.Filter.GetAs<long>("cashPaymentCenterId");

            var data = domain.GetAll()
                .Where(x => x.CashPaymentCenter.Id == cashPaymentCenterId)
                .Select(x => new
                {
                    x.Id,
                    Municipality = x.Municipality.Name
                })
                .OrderIf(loadParams.Order.Length == 0, true, x => x.Municipality)
                .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }
    }
}