/// <mapping-converter-backup>
/// namespace Bars.GkhCalendar.Map
/// {
///     using Bars.B4.DataAccess;
///     using Bars.GkhCalendar.Enums;
/// 
///     using Entities;
/// 
///     public class DayMap : BaseEntityMap<Day>
///     {
///         public DayMap()
///             : base("GKH_CALENDAR_DAY")
///         {
///             Map(x => x.DayDate, "DAY_DATE");
///             Map(x => x.DayType, "DAY_TYPE").Not.Nullable().CustomType<DayType>();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhCalendar.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhCalendar.Entities;
    
    
    /// <summary>Маппинг для "Bars.GkhCalendar.Entities.Day"</summary>
    public class DayMap : BaseEntityMap<Day>
    {
        
        public DayMap() : 
                base("Bars.GkhCalendar.Entities.Day", "GKH_CALENDAR_DAY")
        {
        }

        public static string TableName => "GKH_CALENDAR_DAY";
        public static string SchemaName => "public"; 

        protected override void Map()
        {
            Property(x => x.DayDate, "DayDate").Column("DAY_DATE");
            Property(x => x.DayType, "DayType").Column("DAY_TYPE").NotNull();
        }
    }
}
