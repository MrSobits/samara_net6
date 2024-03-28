namespace Bars.Gkh.RegOperator.LogMap
{
    using Decisions.Nso.Entities;

    public class MinFundAmountDecisionLogMap : UltimateDecisionLogMap<MinFundAmountDecision>
    {
        public MinFundAmountDecisionLogMap()
        {
            Name("Минимальный размер фонда КР");

            MapProperty(x => x.Decision, "Decision", "Текущее значение");
        }
    }
}