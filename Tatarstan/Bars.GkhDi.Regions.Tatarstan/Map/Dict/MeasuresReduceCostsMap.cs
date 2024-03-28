/// <mapping-converter-backup>
/// namespace Bars.GkhDi.Regions.Tatarstan.Map.Dict
/// {
///     using Bars.B4.DataAccess;
///     using Bars.GkhDi.Regions.Tatarstan.Entities.Dict;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Меры по снижению расходов"
///     /// </summary>
///     public class MeasuresReduceCostsMap : BaseEntityMap<MeasuresReduceCosts>
///     {
///         public MeasuresReduceCostsMap()
///             : base("DI_TAT_MEAS_RED_COSTS")
///         {
///             Map(x => x.MeasureName, "MEASURE_NAME").Not.Nullable().Length(300);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhDi.Regions.Tatarstan.Map.Dict
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhDi.Regions.Tatarstan.Entities.Dict;
    
    
    /// <summary>Маппинг для "Bars.GkhDi.Regions.Tatarstan.Entities.Dict.MeasuresReduceCosts"</summary>
    public class MeasuresReduceCostsMap : BaseEntityMap<MeasuresReduceCosts>
    {
        
        public MeasuresReduceCostsMap() : 
                base("Bars.GkhDi.Regions.Tatarstan.Entities.Dict.MeasuresReduceCosts", "DI_TAT_MEAS_RED_COSTS")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.MeasureName, "MeasureName").Column("MEASURE_NAME").Length(300).NotNull();
        }
    }
}
