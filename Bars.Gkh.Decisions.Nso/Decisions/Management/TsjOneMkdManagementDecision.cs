namespace Bars.Gkh.Decisions.Nso.Decisions
{
    using Domain.Decisions;

    /// <summary>
    /// ТСЖ (1 МКД) ТСЖ (&gt;1 МКД, &lt;30кв.)
    /// </summary>
    public class TsjOneMkdManagementDecision : AbstractDecision
    {
        public TsjOneMkdManagementDecision(string name, IDecisionType decisionType) : base(name, decisionType)
        {
        }

        public override string Code => "TsjOneMkdManagement";
    }
}