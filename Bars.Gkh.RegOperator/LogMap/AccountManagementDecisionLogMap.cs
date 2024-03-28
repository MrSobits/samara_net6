namespace Bars.Gkh.RegOperator.LogMap
{
    using B4.Modules.NHibernateChangeLog;
    using B4.Utils;
    using Decisions.Nso.Entities.Decisions;

    class AccountManagementDecisionLogMap : UltimateDecisionLogMap<AccountManagementDecision>
    {
        public AccountManagementDecisionLogMap()
        {
            Name("Решение по ведению лицевого счета");

            MapProperty(x => x.Decision, "Decision", "Способ ведения лицевого счета", x => x.GetEnumMeta().Display);
        }
    }
}