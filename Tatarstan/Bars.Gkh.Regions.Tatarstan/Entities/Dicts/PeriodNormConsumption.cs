namespace Bars.Gkh.Regions.Tatarstan.Entities.Dicts
{
    using System;

    using Bars.Gkh.Entities;

    /// <summary>
    /// Отчетные периоды нормативов потребления
    /// </summary>
    public class PeriodNormConsumption : BaseGkhEntity
    {
        /// <summary>
        ///  Наименование
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Дата начала периода
        /// </summary>
        public virtual DateTime StartDate { get; set; }

        /// <summary>
        /// Дата окончания периода
        /// </summary>
        public virtual DateTime EndDate { get; set; }
    }
}