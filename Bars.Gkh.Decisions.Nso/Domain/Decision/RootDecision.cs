namespace Bars.Gkh.Decisions.Nso.Domain.Decisions
{
    using B4;
    using Entities;

    public class RootDecision : IDecision
    {
        public static string RootCode = "Root";

        public string Name { get; set; }

        public string Code { get { return RootCode; } set{}}

        public IDecisionType DecisionType { get; set; }

        public bool Exclusive { get; set; }

        public IDataResult GetValue(GenericDecision baseDecision)
        {
            throw new System.NotImplementedException();
        }
    }
}