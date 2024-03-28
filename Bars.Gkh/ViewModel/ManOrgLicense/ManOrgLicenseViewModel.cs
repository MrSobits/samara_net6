namespace Bars.Gkh.ViewModel
{
    using System.Linq;
    using B4;
    using B4.DataAccess;
    using Domain;
    using Entities;
    using Bars.Gkh.DomainService;


    public class ManOrgLicenseViewModel : BaseViewModel<ManOrgLicense>
    {
        public IManOrgLicenseService Service { get; set; }

        public override IDataResult List(IDomainService<ManOrgLicense> domain, BaseParams baseParams)
        {
            var totalCount = 0;

            var result = Service.GetList(baseParams, true, ref totalCount);

            return new ListDataResult(result, totalCount);
        }
    }
}