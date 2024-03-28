/// <mapping-converter-backup>
/// namespace Bars.GkhDi.Regions.Tatarstan.Map
/// {
///     using Bars.B4.DataAccess;
///     using Bars.GkhDi.Regions.Tatarstan.Entities;
/// 
///     public class PlanReduceMeasureNameMap : BaseEntityMap<PlanReduceMeasureName>
///     {
///         public PlanReduceMeasureNameMap()
///             : base("DI_TAT_PLAN_RED_MEAS_NAME")
///         {
///             References(x => x.PlanReductionExpenseWorks, "PLAN_RED_EXPWORK_ID").Not.Nullable().Fetch.Join();
///             References(x => x.MeasuresReduceCosts, "MEASURE_RED_COSTS_ID").Not.Nullable().Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhDi.Regions.Tatarstan.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhDi.Regions.Tatarstan.Entities;
    
    
    /// <summary>Маппинг для "Bars.GkhDi.Regions.Tatarstan.Entities.PlanReduceMeasureName"</summary>
    public class PlanReduceMeasureNameMap : BaseEntityMap<PlanReduceMeasureName>
    {
        
        public PlanReduceMeasureNameMap() : 
                base("Bars.GkhDi.Regions.Tatarstan.Entities.PlanReduceMeasureName", "DI_TAT_PLAN_RED_MEAS_NAME")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.PlanReductionExpenseWorks, "PlanReductionExpenseWorks").Column("PLAN_RED_EXPWORK_ID").NotNull().Fetch();
            Reference(x => x.MeasuresReduceCosts, "MeasuresReduceCosts").Column("MEASURE_RED_COSTS_ID").NotNull().Fetch();
        }
    }
}
