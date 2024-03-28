/// <mapping-converter-backup>
/// namespace Bars.GkhDi.Map
/// {
///     using System;
///     using B4.DataAccess.ByCode;
///     using B4.Utils.Annotations;
///     using Entities;
/// 
///     public class DisclosureInfoLicenseMap : BaseImportableEntityMap<DisclosureInfoLicense>
///     {
///         public DisclosureInfoLicenseMap() : base("di_disinfo_license")
///         {
///             References(x => x.DisclosureInfo, "disinfo_id", ReferenceMapConfig.NotNullAndFetch);
///             Map(x => x.LicenseNumber, "license_number", true, 200);
///             Map(x => x.DateReceived, "date_Received");
///             Map(x => x.LicenseOrg, "license_org", true, 300);
///             References(x => x.LicenseDoc, "file_id", ReferenceMapConfig.Fetch);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhDi.Map
{
    using Bars.Gkh.Map; using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhDi.Entities;
    
    
    /// <summary>Маппинг для "Bars.GkhDi.Entities.DisclosureInfoLicense"</summary>
    public class DisclosureInfoLicenseMap : BaseImportableEntityMap<DisclosureInfoLicense>
    {
        
        public DisclosureInfoLicenseMap() : 
                base("Bars.GkhDi.Entities.DisclosureInfoLicense", "DI_DISINFO_LICENSE")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.DisclosureInfo, "DisclosureInfo").Column("DISINFO_ID").NotNull().Fetch();
            Property(x => x.LicenseNumber, "LicenseNumber").Column("LICENSE_NUMBER").Length(200).NotNull();
            Property(x => x.DateReceived, "DateReceived").Column("DATE_RECEIVED");
            Property(x => x.LicenseOrg, "LicenseOrg").Column("LICENSE_ORG").Length(300).NotNull();
            Reference(x => x.LicenseDoc, "LicenseDoc").Column("FILE_ID").Fetch();
        }
    }
}
