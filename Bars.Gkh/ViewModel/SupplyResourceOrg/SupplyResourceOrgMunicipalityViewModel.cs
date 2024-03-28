namespace Bars.Gkh.ViewModel
{
    using System.Linq;
    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;

    public class SupplyResourceOrgMunicipalityViewModel : BaseViewModel<SupplyResourceOrgMunicipality>
    {
        public override IDataResult List(IDomainService<SupplyResourceOrgMunicipality> domain, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var supplyResOrgId = baseParams.Params.GetAs<long>("supplyResOrgId");

            var data = domain.GetAll()
                .Where(x => x.SupplyResourceOrg.Id == supplyResOrgId)
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