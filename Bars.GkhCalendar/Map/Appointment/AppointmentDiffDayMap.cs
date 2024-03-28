namespace Bars.GkhCalendar.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhCalendar.Entities;
    using NHibernate.Mapping.ByCode.Conformist;


    /// <summary>Маппинг для "Sobits.RosReg.ExtractEgrn"</summary>
    public class AppointmentDiffDayMap : BaseEntityMap<AppointmentDiffDay>
    {
        /// <summary>
        /// .ctor
        /// </summary>
        public AppointmentDiffDayMap()
            : base("Bars.GkhCalendar.Entities", AppointmentDiffDayMap.TableName)
        {
        }

        public static string TableName => "GKH_CALENDAR_AppointmentDiffDay";

        /// <summary>
        /// Мап
        /// </summary>
        protected override void Map()
        {
            this.Reference(x => x.AppointmentQueue, "Очередь").Column(nameof(AppointmentDiffDay.AppointmentQueue).ToLower()).Fetch();
            this.Reference(x => x.Day, "День").Column(nameof(AppointmentDiffDay.Day).ToLower()).Fetch();
            this.Property(x => x.StartTime, "Начальное время приёма").Column(nameof(AppointmentDiffDay.StartTime).ToLower());
            this.Property(x => x.EndTime, "Конечное время приёма").Column(nameof(AppointmentDiffDay.EndTime).ToLower());
            this.Property(x => x.StarPauseTime, "Начало перерыва").Column(nameof(AppointmentDiffDay.StarPauseTime).ToLower());
            this.Property(x => x.EndPauseTime, "Конец перерыва").Column(nameof(AppointmentDiffDay.EndPauseTime).ToLower());
            this.Property(x => x.ChangeAppointmentDay, "Изменение").Column(nameof(AppointmentDiffDay.ChangeAppointmentDay).ToLower());
        }
    }
}