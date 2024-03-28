namespace Bars.GkhCalendar.Entities
{
    using Bars.B4.DataAccess;
    using Bars.GkhCalendar.Enums;
    using System;

    /// <summary>
    /// Приёмное время
    /// </summary>
    public class AppointmentTime : BaseEntity
    {
        /// <summary>
        /// Очередь
        /// </summary>
        public virtual AppointmentQueue AppointmentQueue { get; set; }

        /// <summary>
        /// День недели
        /// </summary>
        public virtual DayOfWeekRus DayOfWeek { get; set; }

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
    }
}
