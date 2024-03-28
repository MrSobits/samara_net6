namespace Bars.Gkh.Decisions.Nso.Decisions.AccountOwner
{
    using Domain.Decisions;

    /// <summary>
    /// Владелец счета - ТСЖ
    /// </summary>
    public class TsjOwnerDecision : AbstractDecision
    {
        public TsjOwnerDecision(string name, IDecisionType decisionType) : base(name, decisionType)
        {
        }

        public override string Code => "TsjOwner";
    }
}