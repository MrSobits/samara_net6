namespace Bars.GkhCalendar.Entities
{
    using System;

    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities;
    using Bars.GkhCalendar.Enums;

    public class Day : BaseEntity
    {
        /// <summary>
        /// Дата
        /// </summary>
        public virtual DateTime DayDate { get; set; }

        /// <summary>
        /// Тип дня
        /// </summary>
        public virtual DayType DayType { get; set; }
    }
}
