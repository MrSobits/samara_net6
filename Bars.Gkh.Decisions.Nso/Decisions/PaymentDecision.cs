namespace Bars.Gkh.Decisions.Nso.Decisions
{
    using Domain.Decisions;

    public class PaymentDecision : AbstractDecision
    {
        public PaymentDecision(string name, IDecisionType decisionType)
            : this(name, decisionType, true)
        {
        }

        public PaymentDecision(string name, IDecisionType decisionType, bool exclusive)
            : base(name, decisionType, exclusive)
        {
        }

        public override string Code => "Payment";
    }
}