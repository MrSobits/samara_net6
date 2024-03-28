/// <mapping-converter-backup>
/// namespace Bars.Gkh.Map
/// {
///     using Bars.Gkh.Entities;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Группа капитальности"
///     /// </summary>
///     public class CapitalGroupMap : BaseGkhEntityMap<CapitalGroup>
///     {
///         public CapitalGroupMap()
///             : base("GKH_DICT_CAPITAL_GROUP")
///         {
///             Map(x => x.Name, "NAME").Not.Nullable().Length(300);
///             Map(x => x.Description, "DESCRIPTION").Length(1000);
///             Map(x => x.Code, "CODE").Length(300);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities;
    
    
    /// <summary>Маппинг для "Группа капитальности"</summary>
    public class MonitoringTypeDictMap : BaseEntityMap<MonitoringTypeDict>
    {
        
        public MonitoringTypeDictMap() : 
                base("Группа капитальности", "GKH_DICT_MONITORING_TYPE")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.Name, "Наименование").Column("NAME").Length(500);
            Property(x => x.Description, "Описание").Column("DESCRIPTION").Length(1000);
            Property(x => x.Code, "Код").Column("CODE").Length(50);
        }
    }
}
