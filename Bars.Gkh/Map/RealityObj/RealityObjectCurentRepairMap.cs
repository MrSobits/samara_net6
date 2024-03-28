/// <mapping-converter-backup>
/// namespace Bars.Gkh.Map
/// {
///     using Bars.Gkh.Entities;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Текущий ремонт жилого дома"
///     /// </summary>
///     public class RealityObjectCurentRepairMap : BaseGkhEntityMap<RealityObjectCurentRepair>
///     {
///         public RealityObjectCurentRepairMap() : base("GKH_OBJ_CURENT_REPAIR")
///         {
///             Map(x => x.FactDate, "FACT_DATE");
///             Map(x => x.FactSum, "FACT_SUM");
///             Map(x => x.FactWork, "FACT_WORK");
///             Map(x => x.PlanDate, "PLAN_DATE");
///             Map(x => x.PlanSum, "PLAN_SUM");
///             Map(x => x.PlanWork, "PLAN_WORK");
///             Map(x => x.UnitMeasure, "UNIT_MEASURE").Length(50);
/// 
///             References(x => x.RealityObject, "REALITY_OBJECT_ID").Not.Nullable().Fetch.Join();
///             References(x => x.WorkKind, "WORK_KIND_ID").Not.Nullable().Fetch.Join();
///             References(x => x.Builder, "BUILDER_ID").Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities;
    
    
    /// <summary>Маппинг для "Текущий ремонт жилого дома"</summary>
    public class RealityObjectCurentRepairMap : BaseImportableEntityMap<RealityObjectCurentRepair>
    {
        
        public RealityObjectCurentRepairMap() : 
                base("Текущий ремонт жилого дома", "GKH_OBJ_CURENT_REPAIR")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Property(x => x.FactDate, "Факт дата").Column("FACT_DATE");
            Property(x => x.FactSum, "факт на сумму").Column("FACT_SUM");
            Property(x => x.FactWork, "факт объем работ").Column("FACT_WORK");
            Property(x => x.PlanDate, "План дата").Column("PLAN_DATE");
            Property(x => x.PlanSum, "План на сумму").Column("PLAN_SUM");
            Property(x => x.PlanWork, "План объем работ").Column("PLAN_WORK");
            Property(x => x.UnitMeasure, "ед. измерения").Column("UNIT_MEASURE").Length(50);
            Reference(x => x.RealityObject, "Жилой дом").Column("REALITY_OBJECT_ID").NotNull().Fetch();
            Reference(x => x.WorkKind, "Вид работы").Column("WORK_KIND_ID").NotNull().Fetch();
            Reference(x => x.Builder, "Подрядчик").Column("BUILDER_ID").Fetch();
        }
    }
}
