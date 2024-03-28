namespace Bars.Gkh.Entities.EfficiencyRating
{
    using System;

    using Bars.B4.DataAccess;

    /// <summary>
    /// Муниципалитеты, по которым производится отображение графика
    /// </summary>
    public class AnaliticsGraphMunicipality : PersistentObject
    {
        /// <summary>
        /// График
        /// </summary>
        public virtual EfficiencyRatingAnaliticsGraph Graph { get; set; }

        /// <summary>
        /// Муниципалитет
        /// </summary>
        public virtual Municipality Municipality { get; set; }
    }
}