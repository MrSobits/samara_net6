namespace Bars.GisIntegration.Base.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GisIntegration.Base.Entities;

    public class RisPackageDataMap : PersistentObjectMap<RisPackageData>
    {
        public RisPackageDataMap()
            : base("Bars.GisIntegration.Base.Entities.RisPackageData", "GI_PACKAGE_DATA")
        {
        }

        protected override void Map()
        {
            this.Reference(x => x.Data, "Data").Column("DATA_ID");
            this.Reference(x => x.SignedData, "SignedData").Column("SIG_DATA_ID");
            this.Property(x => x.TransportGuidDictionary, "TransportGuidDictionary").Column("TRANSPORT_GUID_DICTIONARY");
        }
    }
}