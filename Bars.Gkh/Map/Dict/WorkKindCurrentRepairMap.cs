/// <mapping-converter-backup>
/// namespace Bars.Gkh.Map
/// {
///     using Bars.Gkh.Entities;
///     using Bars.Gkh.Enums;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Материалы стен"
///     /// </summary>
///     public class WorkKindCurrentRepairMap : BaseGkhEntityMap<WorkKindCurrentRepair>
///     {
///         public WorkKindCurrentRepairMap() : base("GKH_DICT_WORK_CUR_REPAIR")
///         {
///             Map(x => x.Name, "NAME").Not.Nullable().Length(300);
///             Map(x => x.Code, "CODE").Not.Nullable().Length(300);
///             References(x => x.UnitMeasure, "UNIT_MEASURE_ID").Nullable().Fetch.Join();
/// 
///             Map(x => x.TypeWork, "TYPE_WORK").Not.Nullable().CustomType<TypeWork>();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities;
    
    
    /// <summary>Маппинг для "Bars.Gkh.Entities.WorkKindCurrentRepair"</summary>
    public class WorkKindCurrentRepairMap : BaseImportableEntityMap<WorkKindCurrentRepair>
    {
        
        public WorkKindCurrentRepairMap() : 
                base("Bars.Gkh.Entities.WorkKindCurrentRepair", "GKH_DICT_WORK_CUR_REPAIR")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Property(x => x.Name, "Наименование").Column("NAME").Length(300).NotNull();
            Property(x => x.Code, "Код").Column("CODE").Length(300).NotNull();
            Property(x => x.TypeWork, "Тип работы").Column("TYPE_WORK").NotNull();
            Reference(x => x.UnitMeasure, "Единица измерения").Column("UNIT_MEASURE_ID").Fetch();
        }
    }
}
