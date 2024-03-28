namespace Bars.Gkh.ViewModel
{
    using System.Linq;
    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;

    public class SupplyResourceOrgRealtyObjViewModel : BaseViewModel<SupplyResourceOrgRealtyObject>
    {
        public override IDataResult List(IDomainService<SupplyResourceOrgRealtyObject> domain, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var supplyResOrgId = baseParams.Params.GetAs<long>("supplyResOrgId");

            var data = domain.GetAll()
                .Where(x => x.SupplyResourceOrg.Id == supplyResOrgId)
                .Select(x => new
                {
                    x.Id,
                    Municipality = x.RealityObject.Municipality.Name,
                    x.RealityObject.Address
                })
                .OrderIf(loadParams.Order.Length == 0, true, x => x.Municipality)
                .OrderThenIf(loadParams.Order.Length == 0, true, x => x.Address)
                .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }
    }
}