namespace Bars.Gkh1468.DomainService
{
    using System.Linq;
    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh1468.Entities;

    public class PublicServiceOrgRealtyObjectViewModel : BaseViewModel<PublicServiceOrgRealtyObject>
    {
        public override IDataResult List(IDomainService<PublicServiceOrgRealtyObject> domain, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var publicServOrgId = baseParams.Params.GetAs<long>("publicServOrgId");

            var data = domain.GetAll()
                .Where(x => x.PublicServiceOrg.Id == publicServOrgId)
                .Select(x => new
                {
                    x.Id,
                    Municipality = x.RealityObject.Municipality.Name,
                    x.RealityObject.Address
                })
                .OrderIf(loadParams.Order.Length == 0, true, x => x.Municipality)
                .OrderThenIf(loadParams.Order.Length == 0, true, x => x.Address)
                .Filter(loadParams, Container);

            int totalCount = data.Count();

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), totalCount);
        }
    }
}