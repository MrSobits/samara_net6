namespace Bars.GkhGji.Regions.Tatarstan.Entities
{
    using System;

    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities.Base;
    using Bars.GkhGji.Regions.Tatarstan.Entities.Dict;

    /// <summary>
    /// Значение показателя эффективности и результативности.
    /// </summary>
    public class EffectivenessAndPerformanceIndexValue : BaseEntity, IUsedInTorIntegration
    {
        /// <summary>
        /// Показатель.
        /// </summary>
        public virtual EffectivenessAndPerformanceIndex EffectivenessAndPerformanceIndex { get; set; }

        /// <summary>
        /// Дата начала расчета.
        /// </summary>
        public virtual DateTime CalculationStartDate { get; set; }

        /// <summary>
        /// Дата окончания расчета.
        /// </summary>
        public virtual DateTime CalculationEndDate { get; set; }

        /// <summary>
        /// Значение.
        /// </summary>
        public virtual string Value { get; set; }

        /// <inheritdoc />
        public virtual Guid? TorId { get; set; }
    }
}
