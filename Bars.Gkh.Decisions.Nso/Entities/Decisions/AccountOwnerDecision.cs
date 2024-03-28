namespace Bars.Gkh.Decisions.Nso.Entities
{
    using Bars.Gkh.Enums.Decisions;

    /// <summary>
    /// Решение о владельце счета
    /// </summary>
    public class AccountOwnerDecision : UltimateDecision
    {
        /// <summary>
        /// Владелец счета
        /// </summary>
        public virtual AccountOwnerDecisionType DecisionType { get; set; }
    }
}