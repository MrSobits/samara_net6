namespace Bars.Gkh.Regions.Tatarstan.Entities
{
    using System;

    using Bars.B4.DataAccess;

    /// <summary>
    /// Отчетный период договора
    /// </summary>
    public class ContractPeriod : BaseEntity
    {
        /// <summary>
        /// Начало периода
        /// </summary>
        public virtual DateTime StartDate { get; set; }

        /// <summary>
        /// Конец периода
        /// </summary>
        public virtual DateTime EndDate { get; set; }

        /// <summary>
        /// Название
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Количество УО
        /// </summary>
        public virtual int UoNumber { get; set; }

        /// <summary>
        /// Количество РСО
        /// </summary>
        public virtual int RsoNumber { get; set; }

        /// <summary>
        /// Количество жилых домов
        /// </summary>
        public virtual int RoNumber { get; set; }
    }
}