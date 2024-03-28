namespace Bars.Gkh.RegOperator.LogMap
{
    using B4;
    using B4.Application;
    using B4.Modules.NHibernateChangeLog;
    using B4.Utils;
    using Decisions.Nso.Entities;
    using Gkh.Entities;

    public class AccumulationTransferDecisionLogMap : UltimateDecisionLogMap<AccumulationTransferDecision>
    {
        public AccumulationTransferDecisionLogMap()
        {
            Name("Решение о переводе накоплений");

            MapProperty(x => x.Decision, "Decision", "Сумма накоплений переводимая на спецсчет");
        }
    }
}