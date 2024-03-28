namespace Bars.Gkh.Decisions.Nso.Decisions
{
    using Domain.Decisions;

    public class CrFundFormingMethodSpecAccDecision : AbstractDecision
    {
        public CrFundFormingMethodSpecAccDecision(string name, IDecisionType decisionType)
            : base(name, decisionType)
        {
        }

        public override string Code => "CrFundFormingMethodSpecAccDecision";
    }
}