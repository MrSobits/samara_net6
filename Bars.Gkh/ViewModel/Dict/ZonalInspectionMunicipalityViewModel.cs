namespace Bars.Gkh.ViewModel
{
    using System.Linq;

    using B4;
    using B4.Utils;
    using Entities;
    
    public class ZonalInspectionMunicipalityViewModel : BaseViewModel<ZonalInspectionMunicipality>
    {
        public override IDataResult List(IDomainService<ZonalInspectionMunicipality> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var zonalInspectionId = baseParams.Params.GetAs<long>("zonalInspectionId");

            var data = domainService.GetAll()
                .Where(x => x.ZonalInspection.Id == zonalInspectionId)
                .Select(x => new
                    {
                        x.Id,
                        Municipality = x.Municipality.Name
                    })
                .Filter(loadParams, Container);

            var totalCount = data.Count();

            data = data
                .OrderIf(loadParams.Order.Length == 0, true, x => x.Municipality);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), totalCount);
        }
    }
}