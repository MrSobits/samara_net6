namespace Bars.Gkh.Overhaul.Nso.ViewModel
{
    using System.Linq;
    using B4;
    using Gkh.Domain;
    using Overhaul.Entities;

    public class RealEstateTypeMunicipalityViewModel : BaseViewModel<RealEstateTypeMunicipality>
    {
        public override IDataResult List(IDomainService<RealEstateTypeMunicipality> domainService, BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();
            var retId = baseParams.Params.GetAsId("RealEstateTypeId");

            var data = domainService.GetAll()
                .Where(x => x.RealEstateType.Id == retId)
                .Select(x => new
                {
                    x.Id,
                    Municipality = x.Municipality.Name
                })
                .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }
    }
}
