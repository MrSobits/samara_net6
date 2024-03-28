namespace Bars.Gkh.Entities.EfficiencyRating
{
    using System;

    using Bars.B4.DataAccess;

    /// <summary>
    /// Управляющие организации, по которым производится отображение графика
    /// </summary>
    public class AnaliticsGraphManagingOrganization : PersistentObject
    {
        /// <summary>
        /// График
        /// </summary>
        public virtual EfficiencyRatingAnaliticsGraph Graph { get; set; }

        /// <summary>
        /// УО
        /// </summary>
        public virtual ManagingOrganization ManagingOrganization { get; set; }
    }
}