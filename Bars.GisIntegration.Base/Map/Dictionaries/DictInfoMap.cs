namespace Bars.GisIntegration.Base.Map.DeviceMetering
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GisIntegration.Base.Entities.Dictionaries;
    using Bars.GisIntegration.Base.Map;
    using Entities.DeviceMetering;

    public class DictInfoMap : BaseEntityMap<DictInfo>
    {
        public DictInfoMap()
            : base("Bars.GisIntegration.Base.Entities.Dictionaries.DictInfo", "GIS_GKH_DICT_DATA")
        {
        }

        protected override void Map()
        {
            this.Property(x => x.RegistryNumber, "RegistryNumber").Column("RegistryNumber");
            this.Property(x => x.Name, "Name").Column("Name");
            this.Property(x => x.Modified, "Modified").Column("Modified");
            this.Property(x => x.LastRequest, "LastRequest").Column("LastRequest");
            this.Property(x => x.RawReply, "RawReply").Column("RawReply");
        }
    }
}
