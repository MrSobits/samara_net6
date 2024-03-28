namespace Bars.GkhCalendar.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhCalendar.Entities;
    using NHibernate.Mapping.ByCode.Conformist;


    /// <summary>Маппинг для "Sobits.RosReg.ExtractEgrn"</summary>
    public class AppointmentTimeMap : BaseEntityMap<AppointmentTime>
    {
        /// <summary>
        /// .ctor
        /// </summary>
        public AppointmentTimeMap()
            : base("Bars.GkhCalendar.Entities", AppointmentTimeMap.TableName)
        {
        }

        public static string TableName => "GKH_CALENDAR_AppointmentTime";

        /// <summary>
        /// Мап
        /// </summary>
        protected override void Map()
        {
            this.Reference(x => x.AppointmentQueue, "Очередь").Column(nameof(AppointmentTime.AppointmentQueue).ToLower()).Fetch();
            this.Property(x => x.DayOfWeek, "День недели").Column(nameof(AppointmentTime.DayOfWeek).ToLower());
            this.Property(x => x.StartTime, "Начальное время приёма").Column(nameof(AppointmentTime.StartTime).ToLower());
            this.Property(x => x.EndTime, "Конечное время приёма").Column(nameof(AppointmentTime.EndTime).ToLower());
            this.Property(x => x.StarPauseTime, "Начало перерыва").Column(nameof(AppointmentTime.StarPauseTime).ToLower());
            this.Property(x => x.EndPauseTime, "Конец перерыва").Column(nameof(AppointmentTime.EndPauseTime).ToLower());
        }
    }
}