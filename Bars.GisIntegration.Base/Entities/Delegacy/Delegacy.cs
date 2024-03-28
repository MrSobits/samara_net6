namespace Bars.GisIntegration.Base.Entities.Delegacy
{
    using System;
    using Bars.B4.DataAccess;

    /// <summary>
    /// Делегирование
    /// </summary>
    public class Delegacy : BaseEntity
    {
        /// <summary>
        /// Оператор информационной системы
        /// </summary>
        public virtual RisContragent OperatorIS { get; set; }

        /// <summary>
        /// Поставщик информации - доверитель
        /// </summary>
        public virtual RisContragent InformationProvider { get; set; }

        /// <summary>
        /// Дата начала делегирования
        /// </summary>
        public virtual DateTime StartDate { get; set; }

        /// <summary>
        /// Дата окончания делегирования
        /// </summary>
        public virtual DateTime EndDate { get; set; }
    }
}