namespace Bars.Gkh.Entities
{
    using System;
    using Bars.Gkh.Enums;

    /// <summary>
    /// Режим работы органа государственной власти местного самоуправления
    /// </summary>
    public class PoliticAuthorityWorkMode : BaseGkhEntity
    {
        /// <summary>
        /// Орган государственной власти
        /// </summary>
        public virtual PoliticAuthority PoliticAuthority { get; set; }

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