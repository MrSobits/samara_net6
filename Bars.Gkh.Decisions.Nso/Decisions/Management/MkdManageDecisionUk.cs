namespace Bars.Gkh.Decisions.Nso.Decisions
{
    using Domain.Decisions;

    public class MkdManageDecisionUk : AbstractDecision
    {
        public MkdManageDecisionUk(string name, IDecisionType decisionType)
            : base(name, decisionType)
        { }

        public override string Code => "UK";
    }
}