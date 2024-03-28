namespace Bars.Gkh.Entities.EfficiencyRating
{
    using System;

    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities.Dicts;

    /// <summary>
    /// Период, за который производится построение графика
    /// </summary>
    public class AnaliticsGraphPeriod : PersistentObject
    {
        /// <summary>
        /// График-аналитики
        /// </summary>
        public virtual EfficiencyRatingAnaliticsGraph Graph { get; set; }

        /// <summary>
        /// Период рейтинга эффективности
        /// </summary>
        public virtual EfficiencyRatingPeriod Period { get; set; }
    }
}