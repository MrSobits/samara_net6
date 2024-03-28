namespace Bars.Gkh.Decisions.Nso.Decisions
{
    using Domain.Decisions;

    /// <summary>
    /// Непосредственное управление
    /// </summary>
    public class DirectManagementDecision : AbstractDecision
    {
        public DirectManagementDecision(string name, IDecisionType decisionType) : base(name, decisionType)
        {
        }

        public override string Code => "DirectManagement";
    }
}