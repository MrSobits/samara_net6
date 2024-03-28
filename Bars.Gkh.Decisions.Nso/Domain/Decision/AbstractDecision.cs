namespace Bars.Gkh.Decisions.Nso.Domain.Decisions
{
    using B4.Utils;

    public abstract class AbstractDecision : IDecision
    {
        protected AbstractDecision(string name, IDecisionType decisionType, bool exclusive = true)
        {
            Name = name;
            DecisionType = decisionType;
            Exclusive = exclusive;
        }

        public virtual string Name { get; set; }

        public abstract string Code { get; }

        public virtual IDecisionType DecisionType { get; set; }

        public virtual bool Exclusive { get; set; }

        public override int GetHashCode()
        {
            return Code.IsNotEmpty() ? Code.GetHashCode() : base.GetHashCode();
        }
    }
}