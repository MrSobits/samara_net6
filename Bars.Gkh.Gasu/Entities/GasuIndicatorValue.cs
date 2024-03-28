using System;

namespace Bars.Gkh.Gasu.Entities
{
    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities;

    /// <summary>
    /// Сведения показателя ГАСУ
    /// </summary>
    public class GasuIndicatorValue : BaseEntity
    {
        /// <summary>
        /// Показатель ГАСУ
        /// </summary>
        public virtual GasuIndicator GasuIndicator { get; set; }

        /// <summary>
        /// Муниципальное образование
        /// </summary>
        public virtual Municipality Municipality { get; set; }

        /// <summary>
        /// Значение
        /// </summary>
        public virtual decimal Value { get; set; }

        /// <summary>
        /// Начало периода
        /// </summary>
        public virtual DateTime PeriodStart { get; set; }

        /// <summary>
        /// Год
        /// </summary>
        public virtual int Year { get; set; }

        /// <summary>
        /// Месяц
        /// </summary>
        public virtual int Month { get; set; } 
    }
}