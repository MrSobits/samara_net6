namespace Bars.Gkh.Decisions.Nso.Decisions
{
    using Domain.Decisions;

    /// <summary>
    /// ТСЖ &gt;1 МКД
    /// </summary>
    public class TsjMoreThanOneMkdManagementDecision : AbstractDecision
    {
        public TsjMoreThanOneMkdManagementDecision(string name, IDecisionType decisionType) : base(name, decisionType)
        {
        }

        public override string Code => "TsjMoreThanOneMkdManagement";
    }
}