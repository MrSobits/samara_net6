namespace Bars.Gkh.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;

    public class ServiceOrgRealityObjectViewModel : BaseViewModel<ServiceOrgRealityObject>
    {
        public override IDataResult List(IDomainService<ServiceOrgRealityObject> domain, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);
            var servorgId = baseParams.Params.GetAs<long>("servorgId");

            var data = domain.GetAll()
                 .Where(x => x.ServiceOrg.Id == servorgId)
                 .Select(x => new
                 {
                     x.Id,
                     x.RealityObject.Address,
                     Municipality = x.RealityObject.Municipality.Name
                 })
                 .OrderIf(loadParams.Order.Length == 0, true, x => x.Municipality)
                 .OrderThenIf(loadParams.Order.Length == 0, true, x => x.Address)
                 .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }
    }
}
