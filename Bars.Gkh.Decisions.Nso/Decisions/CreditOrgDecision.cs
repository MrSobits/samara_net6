namespace Bars.Gkh.Decisions.Nso.Decisions
{
    using Domain.Decisions;

    public class CreditOrgDecision : AbstractDecision
    {
        public CreditOrgDecision(string name, IDecisionType decisionType)
            : this(name, decisionType, true)
        {
        }

        public CreditOrgDecision(string name, IDecisionType decisionType, bool exclusive) : base(name, decisionType, exclusive)
        {
        }

        public override string Code => "CreditOrg";
    }
}