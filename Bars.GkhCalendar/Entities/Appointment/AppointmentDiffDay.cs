namespace Bars.GkhCalendar.Entities
{
    using Bars.B4.DataAccess;
    using Bars.GkhCalendar.Enums;
    using System;

    /// <summary>
    /// Нестандартный приёмный день
    /// </summary>
    public class AppointmentDiffDay : BaseEntity
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
        /// Начальное время приёма
        /// </summary>
        public virtual DateTime StartTime { get; set; }

        /// <summary>
        /// Конечное время приёма
        /// </summary>
        public virtual DateTime EndTime { get; set; }

        /// <summary>
        /// Начало перерыва
        /// </summary>
        public virtual DateTime? StarPauseTime { get; set; }

        /// <summary>
        /// Конец перерыва
        /// </summary>
        public virtual DateTime? EndPauseTime { get; set; }

        /// <summary>
        /// Изменение
        /// </summary>
        public virtual ChangeAppointmentDay ChangeAppointmentDay { get; set; }
    }
}
