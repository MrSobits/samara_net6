namespace Bars.GkhCr.Map
{
    using Bars.Gkh.Map;
    using Bars.GkhCr.Entities;
    
    /// <summary>Маппинг для "Мониторинг СМР"</summary>
    public class SpecialMonitoringSmrMap : BaseImportableEntityMap<SpecialMonitoringSmr>
    {
        public SpecialMonitoringSmrMap() : 
                base("Мониторинг СМР", "CR_SPECIAL_OBJ_MONITORING_CMP")
        {
        }
        
        protected override void Map()
        {
            this.Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            this.Reference(x => x.ObjectCr, "Объект капитального ремонта").Column("OBJECT_ID").Fetch();
            this.Reference(x => x.State, "Статус").Column("STATE_ID").Fetch();
        }
    }
}
