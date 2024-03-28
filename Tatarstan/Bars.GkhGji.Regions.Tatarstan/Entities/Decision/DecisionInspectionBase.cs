namespace Bars.GkhGji.Regions.Tatarstan.Entities.Decision
{
    using System;

    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities.Base;
    using Bars.GkhGji.Entities.Dict;
    using Bars.GkhGji.Regions.Tatarstan.Entities.Dict;
    using Bars.GkhGji.Regions.Tatarstan.Entities.TatarstanDecision;

    /// <summary>
    /// Основание проведения
    /// </summary>
    public class DecisionInspectionBase : BaseEntity, IEntityUsedInErknm
    {
        /// <summary>
        /// Основание проведения
        /// </summary>
        public virtual InspectionBaseType InspectionBaseType { get; set; }

        /// <summary>
        /// Иное основание проведения
        /// </summary>
        public virtual string OtherInspBaseType { get; set; }

        /// <summary>
        /// Дата основания
        /// </summary>
        public virtual DateTime? FoundationDate { get; set; }

        /// <summary>
        /// Индикатор риска
        /// </summary>
        public virtual ControlTypeRiskIndicators RiskIndicator { get; set; }

        /// <summary>
        /// Решение
        /// </summary>
        public virtual TatarstanDecision Decision { get; set; }

        /// <summary>
        /// Идентифкатор Erknm
        /// </summary>
        public virtual string ErknmGuid { get; set; }
    }
}