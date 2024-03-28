namespace Bars.Gkh1468.ViewModel
{
    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh1468.Entities;
    using System.Linq;

    public class PublicServiceOrgMunicipalityViewModel : BaseViewModel<PublicServiceOrgMunicipality>
    {
        public override IDataResult List(IDomainService<PublicServiceOrgMunicipality> domain, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            /* Показывать актуальные */
            var publicServOrgId = baseParams.Params.GetAs<long>("publicServOrgId");

            var data = domain.GetAll()
                .Where(x => x.PublicServiceOrg.Id == publicServOrgId)
                .Select(x => new
                {
                    x.Id,
                    PublicServiceOrg = x.PublicServiceOrg.Id,
                    Municipality = x.Municipality.Name
                })
                .OrderIf(loadParams.Order.Length == 0, true, x => x.Municipality)
                .Filter(loadParams, Container);

            int totalCount = data.Count();

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), totalCount);
        }
    }
}