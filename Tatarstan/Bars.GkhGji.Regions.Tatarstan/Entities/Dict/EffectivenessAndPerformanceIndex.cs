namespace Bars.GkhGji.Regions.Tatarstan.Entities.Dict
{
    using System;
    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities.Base;
    using Bars.GkhGji.Regions.Tatarstan.Entities.Orgs;

    /// <summary>
    /// Показатель эффективности и результативности
    /// </summary>
    public class EffectivenessAndPerformanceIndex : BaseEntity, IUsedInTorIntegration
    {
        /// <summary>
        /// Код.
        /// </summary>
        public virtual string Code { get; set; }

        /// <inheritdoc />
        public virtual Guid? TorId { get; set; }

        /// <summary>
        /// Наименование показателя.
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Наименование параметра.
        /// </summary>
        public virtual string ParameterName { get; set; }

        /// <summary>
        /// Единица измерения параметра.
        /// </summary>
        public virtual string UnitMeasure { get; set; }

        /// <summary>
        /// Контрольно-надзорный орган (КНО).
        /// </summary>
        public virtual ControlOrganization ControlOrganization { get; set; }
    }
}
