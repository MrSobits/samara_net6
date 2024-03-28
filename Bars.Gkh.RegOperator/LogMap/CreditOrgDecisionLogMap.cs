namespace Bars.Gkh.RegOperator.LogMap
{
    using B4;
    using B4.Application;
    using B4.Modules.NHibernateChangeLog;
    ﻿using Bars.Gkh.Decisions.Nso.Entities;
    using Gkh.Entities;

    public class CreditOrgDecisionLogMap : UltimateDecisionLogMap<CreditOrgDecision>
    {
        public CreditOrgDecisionLogMap()
        {
            Name("Решение о выборе кредитной организации");

            MapProperty(x => x.Decision, "Decision", "Кредитная организация");
        }
    }
}