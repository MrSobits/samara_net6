namespace Bars.Gkh.Entities
{
    using System;
    using Bars.Gkh.Enums;

    /// <summary>
    /// Режим работы органа местного самоуправления
    /// </summary>
    public class LocalGovernmentWorkMode : BaseGkhEntity
    {
        /// <summary>
        /// Орган местного самоуправления
        /// </summary>
        public virtual LocalGovernment LocalGovernment { get; set; }

        /// <summary>
        /// Код раздела
        /// </summary>
        public virtual TypeMode TypeMode { get; set; }

        /// <summary>
        /// День недели
        /// </summary>
        public virtual TypeDayOfWeek TypeDayOfWeek { get; set; }

        /// <summary>
        /// Время начала
        /// </summary>
        public virtual DateTime? StartDate { get; set; }

        /// <summary>
        /// Время окончания
        /// </summary>
        public virtual DateTime? EndDate { get; set; }

        /// <summary>
        /// Перерыв
        /// </summary>
        public virtual string Pause { get; set; }

        /// <summary>
        /// Круглоcуточно
        /// </summary>
        public virtual bool AroundClock { get; set; }
    }
}
