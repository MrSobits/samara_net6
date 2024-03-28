namespace Bars.Gkh.RegOperator.LogMap
{
    using B4.Utils;
    using Decisions.Nso.Entities;

    public class CrFundFormationDecisionLogMap : UltimateDecisionLogMap<CrFundFormationDecision>
    {
        public CrFundFormationDecisionLogMap()
        {
            Name("Решение о формировании фонда КР");

            MapProperty(x => x.Decision, "Decision", "Тип формировании фонда КР", x => x.GetEnumMeta().Display);
        }
    }
}