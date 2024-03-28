namespace Bars.Gkh.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Entities;

    public class SupplyResourceOrgServViewModel : BaseViewModel<SupplyResourceOrgService>
    {
        public override IDataResult List(IDomainService<SupplyResourceOrgService> domain, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var supplyResOrgId = baseParams.Params.GetAs<long>("supplyResOrgId");

            var data = domain.GetAll()
                .Where(x => x.SupplyResourceOrg.Id == supplyResOrgId)
                .Select(x => new
                {
                    x.Id,
                    TypeService = x.TypeService.Name
                })
                .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }
    }
}