namespace Bars.GkhCalendar.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhCalendar.Entities;
    using NHibernate.Mapping.ByCode.Conformist;


    /// <summary>Маппинг для "Sobits.RosReg.ExtractEgrn"</summary>
    public class AppointmentRecordMap : BaseEntityMap<AppointmentRecord>
    {
        /// <summary>
        /// .ctor
        /// </summary>
        public AppointmentRecordMap()
            : base("Bars.GkhCalendar.Entities", AppointmentRecordMap.TableName)
        {
        }

        public static string TableName => "GKH_CALENDAR_AppointmentRecord";

        /// <summary>
        /// Мап
        /// </summary>
        protected override void Map()
        {
            this.Reference(x => x.AppointmentQueue, "Очередь").Column(nameof(AppointmentRecord.AppointmentQueue).ToLower()).Fetch();
            this.Reference(x => x.Day, "День").Column(nameof(AppointmentRecord.Day).ToLower()).Fetch();
            this.Property(x => x.RecordTime, "Время приёма").Column(nameof(AppointmentRecord.RecordTime).ToLower());
            this.Property(x => x.Comment, "Комментарий").Column(nameof(AppointmentRecord.Comment).ToLower());
        }
    }
}