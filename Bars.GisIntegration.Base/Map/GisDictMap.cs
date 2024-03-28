namespace Bars.GisIntegration.Base.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GisIntegration.Base.Entities;

    /// <summary>Маппинг для "Bars.GisIntegration.Base.Entities.GisDict"</summary>
    public class GisDictMap : BaseEntityMap<GisDict>
    {
        
        public GisDictMap() : 
                base("Bars.GisIntegration.Base.Entities.GisDict", "GI_INTEGR_DICT")
        {
        }
        
        protected override void Map()
        {
            this.Property(x => x.Name, "Name").Column("NAME").Length(500);
            this.Property(x => x.ActionCode, "ActionCode").Column("ACTION_CODE").Length(500).NotNull();
            this.Property(x => x.NsiRegistryNumber, "NsiRegistryNumber").Column("REGISTRY_NUMBER").Length(50);
            //this.Property(x => x.DateIntegration, "DateIntegration").Column("DATE_INTEG");
            this.Property(x => x.Group, "Group").Column("DICT_GROUP");
            this.Property(x => x.LastRecordsCompareDate, "LastRecordsCompareDate").Column("REC_COMPARE_DATE");
            this.Property(x => x.State, "State").Column("STATE");
        }
    }
}
