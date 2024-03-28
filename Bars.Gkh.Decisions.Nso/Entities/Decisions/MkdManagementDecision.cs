namespace Bars.Gkh.Decisions.Nso.Entities
{
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums.Decisions;

    /// <summary>
    ///     Решение о выборе управления
    /// </summary>
    public class MkdManagementDecision : UltimateDecision
    {
        /// <summary>
        ///     Тип управления
        /// </summary>
        public virtual MkdManagementDecisionType DecisionType { get; set; }

        /// <summary>
        ///     Управляющая организация
        /// </summary>
        public virtual ManagingOrganization Decision { get; set; }
    }
}