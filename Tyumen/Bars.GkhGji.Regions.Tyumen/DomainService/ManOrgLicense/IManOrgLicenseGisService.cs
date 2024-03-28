using System.Collections;

namespace Bars.GkhGji.Regions.Tyumen.DomainService
{
    using B4;

    public interface IManOrgLicenseGisService
    {
     
        IList GetListWithRO(BaseParams baseParams, bool isPaging, ref int totalCount);

        IList GetRO(BaseParams baseParams, bool isPaging, ref int totalCount);

    }



    public class ManOrgLicenseGisInfo
    {
        public long licenseId { get; set; }

        public long requestId { get; set; }
    }
}