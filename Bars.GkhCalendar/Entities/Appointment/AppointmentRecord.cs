namespace Bars.GkhCalendar.Entities
{
    using Bars.B4.DataAccess;
    using System;

    /// <summary>
    /// Приём в организации (на конкретный временной слот)
    /// </summary>
    public class AppointmentRecord : BaseEntity
    {
        /// <summary>
        /// Очередь
        /// </summary>
        public virtual AppointmentQueue AppointmentQueue { get; set; }

        /// <summary>
        /// День
        /// </summary>
        public virtual Day Day { get; set; }

        /// <summary>
        /// Время приёма
        /// </summary>
        public virtual DateTime RecordTime { get; set; }

        /// <summary>
        /// Комментарий
        /// </summary>
        public virtual string Comment { get; set; }
    }
}
