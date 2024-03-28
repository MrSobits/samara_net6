namespace Bars.GkhGji.Regions.Chelyabinsk.ViewModel
{
    using System.Linq;

    using B4;

    using Entities;

    public class LicenseReissuanceProvDocViewModel : BaseViewModel<LicenseReissuanceProvDoc>
    {

        public override IDataResult List(IDomainService<LicenseReissuanceProvDoc> domain, BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();
            var requestId = loadParams.Filter.GetAs("requestId", 0L);

           var data = domain.GetAll()
               .Where(x=>x.LicenseReissuance.Id == requestId)
               .Select(x => new
               {
                   x.Id,
                   LicProvidedDoc = x.LicProvidedDoc.Name,
                   x.Number,
                   x.Date,
                   x.File
               })
               .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }
    }
}