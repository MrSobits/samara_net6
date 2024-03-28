namespace Bars.Gkh.Decisions.Nso.Decisions.AccountOwner
{
    using Domain.Decisions;

    /// <summary>
    /// Владелец счета - РО
    /// </summary>
    public class RegOpOwnerDecision : AbstractDecision
    {
        public RegOpOwnerDecision(string name, IDecisionType decisionType) : base(name, decisionType)
        {
        }

        public override string Code => "RegOpOwner";
    }
}