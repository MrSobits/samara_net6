namespace Bars.Gkh.Regions.Tatarstan.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Regions.Tatarstan.Entities;

    public class ConstructObjMonitoringSmrMap : BaseEntityMap<ConstructObjMonitoringSmr>
    {
        public ConstructObjMonitoringSmrMap():
                base("Мониторинг СМР", "GKH_CONSTRUCT_OBJ_MONITORING_SMR")
        {
        }

        protected override void Map()
        {
            this.Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            this.Reference(x => x.ConstructionObject, "Объект строительства").Column("OBJECT_ID").Fetch();
            this.Reference(x => x.State, "Статус").Column("STATE_ID").Fetch();
        }
    }
}