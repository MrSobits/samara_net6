namespace Bars.GkhGji.Regions.BaseChelyabinsk.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities;

    public class LicenseActionFileMap : BaseEntityMap<LicenseActionFile>
    {
        
        public LicenseActionFileMap() : 
                base("Bars.GkhGji.Regions.Chelyabinsk.Entities.LicenseActionFile", "GJI_LICENSE_ACTION_FILE")
        {
        }
        
        protected override void Map()
        {
            this.Reference(x => x.LicenseAction, "LICENSE_ACTION").Column("LICENSE_ACTION_ID").NotNull().Fetch();
            this.Reference(x => x.FileInfo, "File").Column("FILE_ID").Fetch();
            this.Property(x => x.FileName, "FileName").Column("FILE_NAME").Length(500);
            this.Property(x => x.SignedInfo, "SignedInfo").Column("SERT_INFO").Length(1500);
        }
    }
}
