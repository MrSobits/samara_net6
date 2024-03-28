namespace Bars.Gkh.Decisions.Nso.Decisions
{
    using Domain.Decisions;

    public class WorkDatesDecision : AbstractDecision
    {
        public WorkDatesDecision(string name, IDecisionType decisionType)
            : base(name, decisionType, true)
        {
        }

        public WorkDatesDecision(string name, IDecisionType decisionType, bool exclusive) : base(name, decisionType, exclusive)
        {
        }

        public override string Code => "WorkDates";
    }
}