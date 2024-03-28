namespace Bars.Gkh.ViewModel
{
    using System.Linq;
    using B4;
    using B4.Utils;
    using Entities;

    public class ServiceOrgMunicipalityViewModel : BaseViewModel<ServiceOrgMunicipality>
    {
        public override IDataResult List(IDomainService<ServiceOrgMunicipality> domain, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var servorgId = baseParams.Params.GetAs<long>("servorgId");

            var data = domain.GetAll()
                .Where(x => x.ServOrg.Id == servorgId)
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