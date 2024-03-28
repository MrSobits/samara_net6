namespace Bars.Gkh.Decisions.Nso.Decisions
{
    using Domain.Decisions;

    /// <summary>
    /// Кооператив
    /// </summary>
    public class CooperativeManagementDecision : AbstractDecision
    {
        public CooperativeManagementDecision(string name, IDecisionType decisionType) : base(name, decisionType)
        {
        }

        public override string Code => "CooperativeManagement";
    }
}