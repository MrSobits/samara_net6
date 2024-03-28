namespace Bars.GkhGji.Regions.Tyumen
{
    using System.Linq;
    using B4;
    using B4.DataAccess;
    using Entities;
    using Bars.Gkh.DomainService;
    using Gkh.Entities;
    using DomainService;

    public class ManOrgLicenseGisViewModel : BaseViewModel<ManOrgLicense>
    {
        public IManOrgLicenseGisService Service { get; set; }

        public override IDataResult List(IDomainService<ManOrgLicense> domain, BaseParams baseParams)
        {
            var totalCount = 0;

            var result = Service.GetListWithRO(baseParams, true, ref totalCount);

            return new ListDataResult(result, totalCount);
        }
    }
}