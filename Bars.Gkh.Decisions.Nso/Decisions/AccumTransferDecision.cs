namespace Bars.Gkh.Decisions.Nso.Decisions
{
    using Domain.Decisions;

    public class AccumTransferDecision : AbstractDecision
    {
        public AccumTransferDecision(string name, IDecisionType decisionType)
            : this(name, decisionType, true)
        {
            
        }

        public AccumTransferDecision(string name, IDecisionType decisionType, bool exclusive) : base(name, decisionType, exclusive)
        {
        }

        public override string Code => "AccumTransfer";
    }
}