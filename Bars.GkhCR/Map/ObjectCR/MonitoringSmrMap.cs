/// <mapping-converter-backup>
/// namespace Bars.GkhCr.Map
/// {
///     using Bars.Gkh.Map;;
///     using Bars.GkhCr.Entities;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Мониторинг СМР"
///     /// </summary>
///     public class MonitoringSmrMap : BaseGkhEntityMap<MonitoringSmr>
///     {
///         public MonitoringSmrMap() : base("CR_OBJ_MONITORING_CMP")
///         {
///             References(x => x.ObjectCr, "OBJECT_ID").Fetch.Join();
///             References(x => x.State, "STATE_ID").Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhCr.Map
{
    using Bars.B4.Modules.Mapping.Mappers; using Bars.Gkh.Map;
    using Bars.GkhCr.Entities;
    
    
    /// <summary>Маппинг для "Мониторинг СМР"</summary>
    public class MonitoringSmrMap : BaseImportableEntityMap<MonitoringSmr>
    {
        
        public MonitoringSmrMap() : 
                base("Мониторинг СМР", "CR_OBJ_MONITORING_CMP")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Reference(x => x.ObjectCr, "Объект капитального ремонта").Column("OBJECT_ID").Fetch();
            Reference(x => x.State, "Статус").Column("STATE_ID").Fetch();
        }
    }
}
