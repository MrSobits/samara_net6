using System.Collections;

namespace Bars.GkhGji.Regions.Voronezh.DomainService
{
    using B4;

    public interface IManOrgLicenseGisService
    {
     
        IList GetListWithRO(BaseParams baseParams, bool isPaging, ref int totalCount);
        IDataResult GetInfo(BaseParams baseParams);
        IDataResult GetInfo(long id);

        IDataResult ListManOrg(BaseParams baseParams);

        IList GetLicenseRO(BaseParams baseParams, bool isPaging, ref int totalCount);

        IList GetResolutionsByMCID(BaseParams baseParams, bool isPaging, ref int totalCount);

        IDataResult ListManOrgWithLicense(BaseParams baseParams);

        IDataResult GetContragentInfoById(BaseParams baseParams);

        IDataResult ListManOrgWithLicenseAndHouse(BaseParams baseParams);

        IDataResult ListManOrgWithLicenseAndHouseByType(BaseParams baseParams);

        IDataResult GetListPersonByContragentId(BaseParams baseParams, bool isPaging, out int totalCount);

    }

    public class ManOrgLicenseGisInfo
    {
        public long licenseId { get; set; }

        public long requestId { get; set; }
    }
}