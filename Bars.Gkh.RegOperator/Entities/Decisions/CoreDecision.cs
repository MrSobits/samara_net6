namespace Bars.Gkh.RegOperator.Entities.Decisions
{
    using Bars.Gkh.Decisions.Nso.Entities;
    using Bars.Gkh.Decisions.Nso.Entities.Decisions;
    using Bars.Gkh.Entities;
    using Bars.Gkh.RegOperator.Enums;

    /// <summary>
    ///     Протокол решения
    /// </summary>
    public class CoreDecision : BaseImportableEntity
    {
        /// <summary>
        ///     Тип протокола решения
        /// </summary>
        public virtual CoreDecisionType DecisionType { get; set; }

        /// <summary>
        ///     Протокол решения органа государственной власти
        /// </summary>
        public virtual GovDecision GovDecision { get; set; }

        /// <summary>
        ///     Протокол решения собственников.
        /// </summary>
        public virtual UltimateDecision UltimateDecision { get; set; }
    }
}