namespace Bars.GkhCalendar.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhCalendar.Entities;
    using NHibernate.Mapping.ByCode.Conformist;


    /// <summary>Маппинг для "Sobits.RosReg.ExtractEgrn"</summary>
    public class AppointmentQueueMap : BaseEntityMap<AppointmentQueue>
    {
        /// <summary>
        /// .ctor
        /// </summary>
        public AppointmentQueueMap()
            : base("Bars.GkhCalendar.Entities", AppointmentQueueMap.TableName)
        {
        }

        public static string TableName => "GKH_CALENDAR_AppointmentQueue";

        /// <summary>
        /// Мап
        /// </summary>
        protected override void Map()
        {
            this.Property(x => x.TypeOrganisation, "Тип организации").Column(nameof(AppointmentQueue.TypeOrganisation).ToLower());
            this.Property(x => x.RecordTo, "Подразделение / специалист").Column(nameof(AppointmentQueue.RecordTo).ToLower());
            this.Property(x => x.Description, "Описание").Column(nameof(AppointmentQueue.Description).ToLower());
            this.Property(x => x.TimeSlot, "Величина временного слота").Column(nameof(AppointmentQueue.TimeSlot).ToLower());
        }
    }
}