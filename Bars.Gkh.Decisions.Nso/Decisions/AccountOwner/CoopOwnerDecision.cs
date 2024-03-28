namespace Bars.Gkh.Decisions.Nso.Decisions.AccountOwner
{
    using Domain.Decisions;

    /// <summary>
    /// Владелец счета - Кооператив
    /// </summary>
    public class CoopOwnerDecision : AbstractDecision
    {
        public CoopOwnerDecision(string name, IDecisionType decisionType) : base(name, decisionType)
        {
        }

        public override string Code => "CoopOwner";
    }
}