namespace Bars.GisIntegration.Base.Entities.Services
{
    using Bars.GisIntegration.Base.Entities;

    /// <summary>
    /// План по перечню работ/услуг
    /// </summary>
    public class WorkingPlan : BaseRisEntity
    {
        /// <summary>
        /// Перечень работ/услуг
        /// </summary>
        public virtual WorkList WorkList { get; set; }

        /// <summary>
        /// Год
        /// </summary>
        public virtual short Year { get; set; }
    }
}