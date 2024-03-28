
namespace Bars.GkhGji.Regions.BaseChelyabinsk.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Entities;
    
    
    /// <summary>Маппинг для "Bars.GkhCalendar.Entities.Day"</summary>
    public class TaskCalendarMap : BaseEntityMap<TaskCalendar>
    {
        
        public TaskCalendarMap() : 
                base("Bars.GkhGji.Regions.BaseChelyabinsk.Entities.TaskCalendar", "GJI_TASK_CALENDAR")
        {
        }

        public static string TableName => "GJI_TASK_CALENDAR";
        public static string SchemaName => "public"; 

        protected override void Map()
        {
            Property(x => x.DayDate, "DayDate").Column("DAY_DATE");
            Property(x => x.DayType, "DayType").Column("DAY_TYPE").NotNull();
        }
    }
}
