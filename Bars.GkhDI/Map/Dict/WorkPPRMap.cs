/// <mapping-converter-backup>
/// namespace Bars.GkhDi.Map
/// {
///     using Bars.GkhDi.Entities;
///     using Bars.B4.DataAccess;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Работы по ППР"
///     /// </summary>
///     public class WorkPprMap : BaseImportableEntityMap<WorkPpr>
///     {
///         public WorkPprMap() : base("DI_DICT_WORK_PPR")
///         {
///             Map(x => x.Name, "NAME").Length(300);
/// 
///             References(x => x.GroupWorkPpr, "GROUP_WORK_PPR_ID").Fetch.Join();
///             References(x => x.Measure, "MEASURE_ID").Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhDi.Map
{
    using Bars.Gkh.Map; using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Map; using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhDi.Entities;
    
    
    /// <summary>Маппинг для "Bars.GkhDi.Entities.WorkPpr"</summary>
    public class WorkPprMap : BaseImportableEntityMap<WorkPpr>
    {
        
        public WorkPprMap() : 
                base("Bars.GkhDi.Entities.WorkPpr", "DI_DICT_WORK_PPR")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.Name, "Name").Column("NAME").Length(300);
            Reference(x => x.GroupWorkPpr, "GroupWorkPpr").Column("GROUP_WORK_PPR_ID").Fetch();
            Reference(x => x.Measure, "Measure").Column("MEASURE_ID").Fetch();
        }
    }
}
