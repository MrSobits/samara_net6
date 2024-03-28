namespace Bars.Gkh.RegOperator.LogMap
{
    using B4.Utils;
    using Decisions.Nso.Entities;

    class MkdManagementDecisionLogMap : UltimateDecisionLogMap<MkdManagementDecision>
    {
        public MkdManagementDecisionLogMap()
        {
            Name("Решение о выборе управления");

            MapProperty(x => x.DecisionType, "DecisionType", "Тип управления", x => x.GetEnumMeta().Display);
        }
    }
}