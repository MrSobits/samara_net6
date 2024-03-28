namespace Bars.Gkh.RegOperator.LogMap
{
    using B4;
    using B4.Application;
    using B4.Modules.NHibernateChangeLog;
    using B4.Utils;
    using Decisions.Nso.Entities;
    using Gkh.Entities;

    class AccountOwnerDecisionLogMap : UltimateDecisionLogMap<AccountOwnerDecision>
    {
        public AccountOwnerDecisionLogMap()
        {
            Name("Решение о владельце лс");

            MapProperty(x => x.DecisionType, "DecisionType", "Владелец счета", x => x.GetEnumMeta().Display);
        }
    }
}