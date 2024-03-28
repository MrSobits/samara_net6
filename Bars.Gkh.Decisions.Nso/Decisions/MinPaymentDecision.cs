namespace Bars.Gkh.Decisions.Nso.Decisions
{
    using Domain.Decisions;

    public class MinPaymentDecision : AbstractDecision
    {
        public MinPaymentDecision(string name, IDecisionType decisionType)
            : base(name, decisionType, true)
        {
        }

        public MinPaymentDecision(string name, IDecisionType decisionType, bool exclusive)
            : base(name, decisionType, exclusive)
        {
        }

        public override string Code => "MinPayment";
    }
}