/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Map
/// {
///     using Bars.B4.DataAccess.ByCode;
///     using Bars.GkhGji.Entities;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Период подачи тепла"
///     /// </summary>
///     public class HeatInputReportPeriodMap : BaseEntityMap<HeatInputPeriod>
///     {
///         public HeatInputReportPeriodMap()
///             : base("GJI_HEATING_INPUT_PERIOD")
///         {
///             References(x => x.Municipality, "MUNICIPALITY_ID");
///             Map(x => x.Year, "PERIOD_YEAR");
///             Map(x => x.Month, "PERIOD_MONTH");
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;
    
    
    /// <summary>Маппинг для "Период подачи тепла"</summary>
    public class HeatInputPeriodMap : BaseEntityMap<HeatInputPeriod>
    {
        
        public HeatInputPeriodMap() : 
                base("Период подачи тепла", "GJI_HEATING_INPUT_PERIOD")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.Municipality, "Муниципальное образование").Column("MUNICIPALITY_ID");
            Property(x => x.Year, "а Год").Column("PERIOD_YEAR");
            Property(x => x.Month, "Месяц").Column("PERIOD_MONTH");
        }
    }
}
