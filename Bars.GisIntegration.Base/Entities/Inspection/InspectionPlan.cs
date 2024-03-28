namespace Bars.GisIntegration.Base.Entities.Inspection
{
    using System;
    using Bars.GisIntegration.Base.Entities;

    /// <summary>
    /// План проверок юридических лиц
    /// </summary>
    public class InspectionPlan : BaseRisEntity
    {
        /// <summary>
        /// Год плана
        /// </summary>
        public virtual int? Year { get; set; }

        /// <summary>
        /// Дата утверждения плана
        /// </summary>
        public virtual DateTime? ApprovalDate { get; set; }

        /// <summary>
        /// Регистрационный номер плана в едином реестре проверок
        /// </summary>
        public virtual int? UriRegistrationNumber { get; set; }
    }
}