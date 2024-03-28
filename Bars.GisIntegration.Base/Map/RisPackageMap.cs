namespace Bars.GisIntegration.Base.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GisIntegration.Base.Entities;

    /// <summary>
    /// Маппинг сущности Bars.GisIntegration.Base.Entities.RisPackage
    /// </summary>
    public class RisPackageMap : BaseEntityMap<RisPackage>
    {
        /// <summary>
        /// Конструктор маппинга
        /// </summary>
        public RisPackageMap() :
            base("Bars.GisIntegration.Base.Entities.RisPackage", "GI_PACKAGE")
        {
        }

        /// <summary>
        /// Инициализация маппинга
        /// </summary>
        protected override void Map()
        {
            //this.Property(x => x.NotSignedData, "NotSignedData").Column("NOT_SIGNED_DATA");
            //this.Property(x => x.SignedData, "SignedData").Column("SIGNED_DATA");
            //this.Property(x => x.TransportGuidDictionary, "TransportGuidDictionary").Column("TRANSPORT_GUID_DICTIONARY");
            this.Property(x => x.Name, "Name").Column("NAME").Length(250);
            this.Property(x => x.UserName, "UserName").Column("USER_NAME").Length(200);
            this.Reference(x => x.RisContragent, "RisContragent").Column("CONTRAGENT_ID").Fetch();
            this.Property(x => x.Signed, "Signed").Column("SIGNED");
            this.Property(x => x.PackageDataId, "PackageDataId").Column("PACKAGE_DATA_ID");
            this.Property(x => x.IsDelegacy, "IsDelegacy").Column("IS_DELEGACY");
        }
    }
}