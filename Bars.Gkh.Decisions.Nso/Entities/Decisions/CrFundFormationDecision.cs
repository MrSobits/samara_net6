namespace Bars.Gkh.Decisions.Nso.Entities
{
    using Bars.Gkh.Enums.Decisions;

    /// <summary>
    ///     Решение о формировании фонда КР
    /// </summary>
    public class CrFundFormationDecision : UltimateDecision
    {
        /// <summary>
        ///     Тип формирования фонда КР
        /// </summary>
        public virtual CrFundFormationDecisionType Decision { get; set; }
    }
}