namespace Bars.Gkh.RegOperator.DomainService.RealityObjectAccount.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Microsoft.Extensions.Logging;
    using Bars.B4.Modules.Tasks.Common.Service;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.Utils;

    using Castle.Windsor;

    /// <summary>
    /// Сервис операций начислений на доме
    /// </summary>
    public class RealtyObjectRegopOperationService : IRealtyObjectRegopOperationService
    {
        private readonly IWindsorContainer container;

        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="container">Контейнер</param>
        public RealtyObjectRegopOperationService(IWindsorContainer container)
        {
            this.container = container;
        }

        /// <summary>
        /// Создать операции начислений для домов
        /// </summary>
        /// <param name="period">Период</param>
        /// <param name="ro">Дом</param>
        /// <param name="indicator">Индикатор</param>
        public void CreateRealtyObjectChargeOperations(ChargePeriod period, RealityObject ro, IProgressIndicator indicator = null)
        {
            var chargeAccountDomain = this.container.ResolveDomain<RealityObjectChargeAccount>();
            var chargeOperationDomain = this.container.ResolveDomain<RealityObjectChargeAccountOperation>();
            var periodRepo = this.container.ResolveRepository<ChargePeriod>();
            var logger = this.container.Resolve<ILogger>();
            var sessionProvider = this.container.Resolve<ISessionProvider>();

            using (this.container.Using(chargeAccountDomain, chargeOperationDomain, periodRepo))
            {
                var prevPeriod = periodRepo.GetAll()
                    .Where(x => x.StartDate < period.StartDate)
                    .OrderByDescending(x => x.StartDate)
                    .FirstOrDefault();

                var chargeAccountIds = chargeAccountDomain.GetAll()
                    .WhereIf(ro != null, x => x.RealityObject.Id == ro.Id)
                    .Select(x => x.Id)
                    .ToArray();

                var existsOperations = chargeOperationDomain.GetAll()
                    .WhereIf(ro != null, x => x.Account.RealityObject.Id == ro.Id)
                    .Where(x => x.Period.Id == period.Id)
                    .Select(x => x.Account.Id)
                    .ToHashSet();

                var accountIds = chargeAccountIds
                    .Except(existsOperations)
                    .ToArray();

                RealityObjectChargeAccountOperation operation = null;

                const int take = 10000;

                var cnt = chargeAccountIds.Length;

                for (int done = 0; done < cnt; done += take)
                {
                    var portion = accountIds.Skip(done).Take(take).ToArray();

                    var previousOperations = prevPeriod == null
                        ? null
                        : chargeOperationDomain.GetAll()
                            .WhereIf(ro != null, x => x.Account.RealityObject.Id == ro.Id)
                            .Where(x => x.Period.Id == prevPeriod.Id)
                            .Select(
                                x => new
                                {
                                    x.SaldoOut,
                                    x.Date,
                                    AccountId = x.Account.Id
                                })
                            .ToDictionary(x => x.AccountId);

                    var list = new List<RealityObjectChargeAccountOperation>();

                    foreach (var accountId in portion)
                    {
                        var saldoOut = previousOperations == null
                            ? 0
                            : previousOperations.Get(accountId).Return(x => x.SaldoOut);

                        operation = new RealityObjectChargeAccountOperation(
                            chargeAccountDomain.Load(accountId),
                            period)
                        {
                            SaldoIn = saldoOut,
                            SaldoOut = saldoOut
                        };

                        list.Add(operation);
                    }

                    if (ro != null && operation != null)
                    {
                        chargeOperationDomain.SaveOrUpdate(operation);
                    }
                    else
                    {
                        TransactionHelper.InsertInManyTransactions(this.container, list, list.Count, true, true);
                    }

                    sessionProvider.GetCurrentSession().Clear();

                    var processed = Math.Min(done + take, cnt);
                    int percent = processed * 10 / cnt;

                    var msg = string.Format("Создано {0} счетов по домам", processed);

                    logger.LogDebug(msg);
                    this.Indicate(indicator, percent, msg);
                }
            }
        }

        /// <summary>
        /// Создать операции начислений для всех домов
        /// </summary>
        /// <param name="indicator">Индикатор</param>
        public void CreateAllRealtyObjectChargeOperations(IProgressIndicator indicator = null)
        {
            var logger = this.container.Resolve<ILogger>();
            var sessionProvider = this.container.Resolve<ISessionProvider>();

            using (this.container.Using(logger, sessionProvider))
            {
                var sessioin = sessionProvider.GetCurrentSession();

                const string createRoChargeQuery = @"INSERT INTO regop_ro_charge_acc_charge(
                            object_version, object_create_date, object_edit_date, ccharged, 
                            acc_id, period_id, cpaid, ccharged_penalty, cpaid_penalty, csaldo_in, 
                            csaldo_out, cdate, balance_change)
                            select 0,now(),now(), 0, roa_id, (select id from regop_period order by cstart desc limit 1), 0, 0, 0, 
                            saldo_in, saldo_out, (select cstart from regop_period order by cstart desc limit 1), 0
                            from (
                            select roa.id roa_id, coalesce(roch.CSALDO_OUT, 0) saldo_in, coalesce(roch.CSALDO_OUT, 0) saldo_out from REGOP_RO_CHARGE_ACCOUNT roa
                            left join REGOP_RO_CHARGE_ACC_CHARGE roch on roa.id = roch.ACC_ID and  
                            roch.PERIOD_ID = (select id from regop_period where CIS_CLOSED = true order by cstart desc limit 1)
                            ) prev_period
                            where not exists 
                            (select 1 from REGOP_RO_CHARGE_ACC_CHARGE where period_id = (select id from regop_period order by cstart desc limit 1) 
                            and acc_id = prev_period.roa_id);";

                var query = sessioin.CreateSQLQuery(createRoChargeQuery);

                query.ExecuteUpdate();

                const string msg = "Созданы начисления по домам";
                const int percent = 10;

                logger.LogDebug(msg);
                this.Indicate(indicator, percent, msg);
            }
        }

        /// <summary>
        /// Создать операции для лс
        /// </summary>
        /// <param name="period">Период</param>
        /// <param name="ro">Дом</param>
        /// <param name="indicator">Индикатор</param>
        public void CreatePersonalAccountSummaries(ChargePeriod period, RealityObject ro, IProgressIndicator indicator = null)
        {
            var personalAccRepo = this.container.ResolveRepository<BasePersonalAccount>();
            var persAccSummaryRepo = this.container.ResolveRepository<PersonalAccountPeriodSummary>();
            var periodRepo = this.container.ResolveRepository<ChargePeriod>();
            var logger = this.container.Resolve<ILogger>();
            var sessionProvider = this.container.Resolve<ISessionProvider>();

            var roId = ro == null ? null : (long?) ro.Id;

            using (this.container.Using(personalAccRepo, persAccSummaryRepo, periodRepo))
            {
                var personalAccountsIds = personalAccRepo.GetAll()
                    .WhereIf(roId.HasValue, x => x.Room.RealityObject.Id == roId)
                    .Select(x => x.Id)
                    .ToArray();

                var existsSummaries =
                    persAccSummaryRepo.GetAll()
                        .WhereIf(roId.HasValue, x => x.PersonalAccount.Room.RealityObject.Id == roId)
                        .Where(x => x.Period.Id == period.Id)
                        .Select(x => x.PersonalAccount.Id)
                        .ToHashSet();

                var prevPeriod = periodRepo.GetAll()
                    .Where(x => x.StartDate < period.StartDate)
                    .OrderByDescending(x => x.StartDate)
                    .FirstOrDefault();

                var accountIds = personalAccountsIds
                    .Except(existsSummaries)
                    .OrderBy(x => x)
                    .ToArray();

                const int take = 10000;

                var accountIdsCount = accountIds.Length;

                for (int done = 0; done < accountIdsCount; done += take)
                {
                    var portion = accountIds.Skip(done).Take(take).ToArray();

                    var list = new List<PersonalAccountPeriodSummary>();

                    if (prevPeriod.IsNotNull())
                    {
                        this.CreatePeriodSummariesWithPrevPeriod(period, persAccSummaryRepo, prevPeriod, portion, personalAccRepo, list);
                    }
                    else
                    {
                        this.CreatePeriodSummariesWithoutPrevPeriod(period, portion, personalAccRepo, list);
                    }

                    TransactionHelper.InsertInManyTransactions(this.container, list, 1000, true, true);

                    sessionProvider.GetCurrentSession().Clear();

                    var processed = Math.Min(done + take, accountIdsCount);
                    int percent = processed * 90 / accountIdsCount;

                    var msg = string.Format("Создано {0} агрегаций по периодам для ЛС", processed);

                    logger.LogDebug(msg);
                    this.Indicate(indicator, percent, msg);
                }
            }
        }

        /// <summary>
        /// Создать операции для всех лс
        /// </summary>
        /// <param name="period">Новый период</param>
        /// <param name="indicator">Индикатор</param>
        public void CreateAllPersonalAccountSummaries(ChargePeriod period,IProgressIndicator indicator = null)
        {
            var logger = this.container.Resolve<ILogger>();
            var sessionProvider = this.container.Resolve<ISessionProvider>();

            using (this.container.Using(logger, sessionProvider))
            {
                var session = sessionProvider.GetCurrentSession();
                
                var prevPeriodId =
                    session.CreateSQLQuery("SELECT CAST(id as BIGINT) FROM regop_period WHERE CIS_CLOSED = true ORDER BY cstart DESC LIMIT 1").UniqueResult<long>();
                
                string createPerSummQuery = $@"insert into regop_pers_acc_period_summ_period_{period.Id} 
                    (object_version,object_create_date,object_edit_date,charge_tariff,penalty,recalc,saldo_in,saldo_out,tariff_payment,
                    period_id,account_id,penalty_payment,charge_base_tariff,
                    tariff_desicion_payment,balance_change,recalc_decision,overhaul_payment,
                    recruitment_payment,recalc_penalty,base_tariff_debt,dec_tariff_debt,penalty_debt,perf_work_charge)
                    select 0,now(),now(),0,0,0,saldo_in,saldo_out,0,
                    (select id from regop_period order by cstart desc limit 1),
                    account_id,0,0,0,0,0,0,0,0,base_tariff_debt,dec_tariff_debt,penalty_debt,0
                    from
                    (select pa.id account_id,
                    coalesce(saldo_out, 0) saldo_in, 
                    coalesce(saldo_out, 0) saldo_out, 
                    coalesce(BASE_TARIFF_DEBT + CHARGE_BASE_TARIFF + BALANCE_CHANGE + RECALC - TARIFF_PAYMENT - PERF_WORK_CHARGE, 0) base_tariff_debt,
                    coalesce(DEC_TARIFF_DEBT + CHARGE_TARIFF + DEC_BALANCE_CHANGE - CHARGE_BASE_TARIFF + RECALC_DECISION - TARIFF_DESICION_PAYMENT - PERF_WORK_CHARGE_DEC, 0) dec_tariff_debt,
                    coalesce(PENALTY_DEBT + PENALTY + RECALC_PENALTY + PENALTY_BALANCE_CHANGE - PENALTY_PAYMENT, 0) penalty_debt
                    from REGOP_PERS_ACC pa
                    left join regop_pers_acc_period_summ_period_{prevPeriodId}  prev on pa.id = prev.account_id

                    where not exists 
                    (select 1 from regop_pers_acc_period_summ_period_{period.Id} cur where cur.account_id = prev.account_id))a;";
                var query = session.CreateSQLQuery(createPerSummQuery);

                query.ExecuteUpdate();

                const string msg = "Созданы агрегации по периодам для ЛС";
                const int percent = 90;

                logger.LogDebug(msg);
                this.Indicate(indicator, percent, msg);
            }
        }

        private void CreatePeriodSummariesWithoutPrevPeriod(
            ChargePeriod period,
            long[] portion,
            IRepository<BasePersonalAccount> personalAccRepo,
            List<PersonalAccountPeriodSummary> list)
        {
            foreach (var accountId in portion)
            {
                var summary = new PersonalAccountPeriodSummary
                {
                    PersonalAccount = personalAccRepo.Load(accountId),
                    Period = period,
                };

                list.Add(summary);
            }
        }

        private void CreatePeriodSummariesWithPrevPeriod(
            ChargePeriod period,
            IRepository<PersonalAccountPeriodSummary> persAccSummaryRepo,
            ChargePeriod prevPeriod,
            long[] portion,
            IRepository<BasePersonalAccount> personalAccRepo,
            List<PersonalAccountPeriodSummary> list)
        {
            var previousSummaries = persAccSummaryRepo.GetAll()
                .Where(x => x.Period.Id == prevPeriod.Id)
                .Where(x => portion.Contains(x.PersonalAccount.Id))
                .ToDictionary(x => x.PersonalAccount.Id);

            foreach (var accountId in portion)
            {
                var prevSummary = previousSummaries.Get(accountId);

                var summary = new PersonalAccountPeriodSummary
                {
                    PersonalAccount = personalAccRepo.Load(accountId),
                    Period = period,
                    SaldoIn = prevSummary.Return(x => x.SaldoOut),
                    SaldoOut = prevSummary.Return(x => x.SaldoOut),
                    BaseTariffDebt =
                        prevSummary.Return(x => x.BaseTariffDebt + x.GetChargedByBaseTariff() - x.TariffPayment),
                    DecisionTariffDebt =
                        prevSummary.Return(
                            x => x.DecisionTariffDebt + x.GetChargedByDecisionTariff() - x.TariffDecisionPayment),
                    PenaltyDebt = prevSummary.Return(x => x.PenaltyDebt + x.Penalty - x.PenaltyPayment + x.RecalcByPenalty)
                };

                list.Add(summary);
            }
        }

        private void Indicate(IProgressIndicator indicator, int percent, string message)
        {
            if (indicator == null)
            {
                return;
            }

            indicator.Report(null, (uint) percent, message);
        }
    }
}