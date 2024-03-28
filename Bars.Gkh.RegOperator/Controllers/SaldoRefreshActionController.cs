namespace Bars.Gkh.RegOperator.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using B4;
    using Bars.B4.DataAccess;
    using Bars.Gkh.RegOperator.Entities;
    using DomainService.Interface;
    using System.Linq;
    using Bars.Gkh.Entities;
    using System.Collections.Generic;
    using Bars.Gkh.Domain;
    using System;

    /// <summary>
    /// 
    /// </summary>
    public class SaldoRefreshActionController : BaseController
    {
        public IDomainService<PersAccGroupRelation> PersonalAccountGroupDomain { get; set; }
        
        public IDomainService<PersonalAccountPeriodSummary> PersonalAccountPeriodSummaryDomain { get; set; }

        public IDomainService<SaldoRefresh> SaldoRefreshDomain { get; set; }

        public ActionResult RefreshSaldo()
        {
            var groupIds = SaldoRefreshDomain.GetAll()
                    .Select(x => x.Group.Id)
                    .ToArray();

            var summariesForSave = new List<PersonalAccountPeriodSummary>();

            var accounts = PersonalAccountGroupDomain.GetAll()
                .Where(x => groupIds.Contains(x.Group.Id))
                .Select(x => x.PersonalAccount.Id)
                .ToList();

            if (accounts.Count == 0)
            {
                return JsFailure("Нет лицевых счетов для обновления");
            }

            foreach (var acc in accounts)
            {
                var summaries = PersonalAccountPeriodSummaryDomain.GetAll()
                    .Where(x => acc == x.PersonalAccount.Id)
                    .OrderBy(x => x.Period.Id);

                PersonalAccountPeriodSummary prevSummary = null;

                foreach (var summary in summaries)
                {
                    if (prevSummary != null)
                    {
                        var saldoIn = prevSummary.SaldoOut;

                        var saldoOut = saldoIn + summary.Penalty - summary.PenaltyPayment + summary.RecalcByPenalty + summary.RecalcByDecisionTariff
                            + summary.RecalcByBaseTariff + summary.ChargeTariff - summary.TariffPayment - summary.TariffDecisionPayment + summary.BaseTariffChange
                            + summary.DecisionTariffChange + summary.PenaltyChange - summary.PerformedWorkChargedBase - summary.PerformedWorkChargedDecision;

                        var debtDecision = prevSummary.DecisionTariffDebt + (prevSummary.ChargeTariff - prevSummary.ChargedByBaseTariff) + prevSummary.DecisionTariffChange
                            + prevSummary.RecalcByDecisionTariff - prevSummary.TariffDecisionPayment;
                        var debtPenalty = prevSummary.PenaltyDebt + prevSummary.Penalty + prevSummary.PenaltyChange + prevSummary.RecalcByPenalty - prevSummary.PenaltyPayment;
                        var debtBase = prevSummary.BaseTariffDebt + prevSummary.ChargedByBaseTariff + prevSummary.BaseTariffChange + prevSummary.RecalcByBaseTariff
                            - prevSummary.TariffPayment - prevSummary.PerformedWorkChargedBase;

                        if (saldoOut != summary.SaldoOut || saldoIn != summary.SaldoIn || debtDecision != summary.DecisionTariffDebt || debtPenalty != summary.PenaltyDebt || debtBase != summary.BaseTariffDebt)
                        {
                            summary.SaldoIn = saldoIn;
                            summary.SaldoOut = saldoOut;
                            summary.DecisionTariffDebt = debtDecision;
                            summary.PenaltyDebt = debtPenalty;
                            summary.BaseTariffDebt = debtBase;
                            summariesForSave.Add(summary);
                        }
                    }
                    else
                    {
                        var saldoOut = summary.Penalty - summary.PenaltyPayment + summary.RecalcByPenalty + summary.RecalcByDecisionTariff
                            + summary.RecalcByBaseTariff + summary.ChargeTariff - summary.TariffPayment - summary.TariffDecisionPayment + summary.BaseTariffChange
                            + summary.DecisionTariffChange + summary.PenaltyChange - summary.PerformedWorkChargedBase - summary.PerformedWorkChargedDecision;

                        if (saldoOut != summary.SaldoOut)
                        {
                            summary.SaldoOut = saldoOut;
                            summariesForSave.Add(summary);
                        }
                    }
                    prevSummary = summary;
                }
            }

            TransactionHelper.InsertInManyTransactions(this.Container, summariesForSave);

            return JsSuccess();
        }

        public ActionResult RemoveAll()
        {
            try
            {
                var groups = SaldoRefreshDomain.GetAll()
                    .Select(x => x.Id)
                    .ToArray();

                if (groups.Length == 0)
                {
                    return JsFailure("Сначала нужно добавить группы");
                }

                foreach (var group in groups)
                {
                    SaldoRefreshDomain.Delete(group);
                }

                return JsSuccess();
            }
            catch (Exception msg)
            {
                return JsFailure(msg.ToString());
            }
        }
    }
}