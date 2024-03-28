/// <mapping-converter-backup>
/// namespace Bars.Gkh.RegOperator.Map.RealityAccount
/// {
///     using B4.DataAccess.ByCode;
///     using Bars.Gkh.Gasu.Entities;
/// 
///     public class GasuIndicatorValueMap : BaseEntityMap<GasuIndicatorValue>
///     {
///         public GasuIndicatorValueMap()
///             : base("GASU_INDICATOR_VALUE")
///         {
///             Map(x => x.Value, "VALUE");
///             Map(x => x.Month, "MONTH");
///             Map(x => x.Year, "YEAR");
///             Map(x => x.PeriodStart, "PERIOD_START");
/// 
///             References(x => x.GasuIndicator, "INDICATOR_ID", ReferenceMapConfig.NotNullAndFetch);
///             References(x => x.Municipality, "MU_ID", ReferenceMapConfig.NotNullAndFetch);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Gasu.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Gasu.Entities;
    
    
    /// <summary>Маппинг для "Bars.Gkh.Gasu.Entities.GasuIndicatorValue"</summary>
    public class GasuIndicatorValueMap : BaseEntityMap<GasuIndicatorValue>
    {
        
        public GasuIndicatorValueMap() : 
                base("Bars.Gkh.Gasu.Entities.GasuIndicatorValue", "GASU_INDICATOR_VALUE")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.GasuIndicator, "GasuIndicator").Column("INDICATOR_ID").NotNull().Fetch();
            Reference(x => x.Municipality, "Municipality").Column("MU_ID").NotNull().Fetch();
            Property(x => x.Value, "Value").Column("VALUE");
            Property(x => x.PeriodStart, "PeriodStart").Column("PERIOD_START");
            Property(x => x.Year, "Year").Column("YEAR");
            Property(x => x.Month, "Month").Column("MONTH");
        }
    }
}
