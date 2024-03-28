namespace Bars.Gkh.Decisions.Nso.Decisions
{
    using Domain.Decisions;

    /// <summary>
    /// Решение о выборе Счета РО
    /// </summary>
    public class CrFundFormingMethodRegOpAccDecision : AbstractDecision
    {
        public CrFundFormingMethodRegOpAccDecision(string name, IDecisionType decisionType) : base(name, decisionType)
        {
        }

        public override string Code => "CrFundFormingMethodRegOpAcc";
    }
}