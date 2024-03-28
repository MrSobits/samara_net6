using System.Linq;

namespace Bars.Gkh.ViewModel
{

    using B4;
    using Entities;

    public class ManOrgLicenseDocViewModel : BaseViewModel<ManOrgLicenseDoc>
    {

        public override IDataResult List(IDomainService<ManOrgLicenseDoc> domain, BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();
            var id = loadParams.Filter.GetAs("licenseId", 0L);

           var data = domain.GetAll()
                .Where(x => x.ManOrgLicense.Id == id)
               .Select(x => new
               {
                   x.Id,
                   x.DocType,
                   x.DocNumber,
                   x.DocDate,
                   x.File
               })
               .AsQueryable()
               .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }
    }
}