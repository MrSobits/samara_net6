namespace Bars.Gkh.RegOperator.Tasks.Rounding
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Threading;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.Tasks.Common.Service;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.RegOperator.Domain.Repository.Wallets;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Entities.ValueObjects;
    using Bars.Gkh.RegOperator.ExecutionAction;
    using Bars.Gkh.RegOperator.Utils;

    using Castle.Windsor;

    using Microsoft.Extensions.Logging;

    using ExecutionContext = Bars.B4.Modules.Tasks.Common.Contracts.ExecutionContext;

    internal class RoundingValuesTasks : RoundingValuesAccrualAction, ITaskExecutor
    {
        public static string Id = MethodBase.GetCurrentMethod().DeclaringType?.FullName;

        /// <summary>
        /// Контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Logger
        /// </summary>
        public ILogger Logger { get; set; }

        /// <summary>
        /// DomainService лицевых счетов
        /// </summary>
        public IDomainService<BasePersonalAccount> PersAccDomain { get; set; }

        /// <summary>
        /// DomainService срезов на период по ЛС
        /// </summary>
        public IDomainService<PersonalAccountPeriodSummary> PersAccSummaryDomain { get; set; }

        /// <summary>
        /// DomainService счетов оплат
        /// </summary>
        public IDomainService<RealityObjectPaymentAccount> RealityObjectPaymentAccountDomain { get; set; }

        /// <summary>
        /// DomainService трансферов
        /// </summary>
        public IDomainService<Transfer> TransferDomain { get; set; }

        /// <summary>
        /// IWalletRepository
        /// </summary>
        public IWalletRepository WalletRepository { get; set; }

        IDataResult ITaskExecutor.Execute(BaseParams @params, ExecutionContext ctx, IProgressIndicator indicator, CancellationToken ct)
        {
            var periodDomain = this.Container.ResolveDomain<ChargePeriod>();

            try
            {
                var periods = periodDomain.GetAll()
                    .OrderBy(x => x.StartDate)
                    .ToArray();

                this.ExecuteFunction1(periods);
                this.ExecuteFunction2(periods);
                this.ExecuteFunction3();

                return new BaseDataResult();
            }

            catch (Exception e)
            {
                return BaseDataResult.Error(" message: {0} \r\n stacktrace: {1}".FormatUsing(e.Message, e.StackTrace));
            }
            finally
            {
                this.Container.Release(periodDomain);
            }
        }

        public string ExecutorCode { get; }

        /// <summary>
        /// Функция 1: округление начислений/оплат и перерасчет сальдо.
        /// Надо сделать, начиная с самого первого периода по всем периодам.
        /// Действия:
        /// 1. Входящее сальдо в период скопировать из Исходящего сальдо предыдущего периода.Для начального периода предыдущего периода нетю Поэтому копировать не надо.
        /// Входящее сальдо = PersonalAccountPeriodSummary.SaldoIn
        /// Исходящего сальдо = PersonalAccountPeriodSummary.SaldoOut
        /// 2. Округлить в периоде значения. Использовать арифметическое округление.Значения для округления в карточке ЛС:
        ///  - 1) Начислено по базовому тарифу. Брать из раздела "Операции" в карточке ЛС. - PersonalAccountPeriodSummary.ChargedByBaseTariff
        ///  - 2) Оплачено по базовому тарифу. Брать из раздела "Операции" в карточке ЛС. - PersonalAccountPeriodSummary.TariffPayment
        ///  - 3) Начислено по тарифу решений. Брать из раздела "Операции" в карточке ЛС. - ChargeByDecision - вычисляемое поле = PersonalAccountPeriodSummary.ChargeTariff
        /// - PersonalAccountPeriodSummary.ChargedByBaseTariff + PersonalAccountPeriodSummary.RecalcByDecisionTariff
        ///  - 4) Оплачено по тарифу решений. Брать из раздела "Операции" в карточке ЛС. - TariffDecisionPayment
        ///  - 5) Перерасчет.Брать из раздела "Операции" в карточке ЛС. - Recalc - вычисляемое поле = PersonalAccountPeriodSummary.RecalcByBaseTariff +
        /// PersonalAccountPeriodSummary.RecalcByDecisionTariff,
        ///  - 6) Пени.Брать из раздела "Операции" в карточке ЛС. - НЕТ ТАКОГО ПОЛЯ - возможно PersonalAccountPeriodSummary.Penalty
        ///  - 7) Перерасчет пени. Брать из раздела "Операции" в карточке ЛС.- НЕТ ТАКОГО ПОЛЯ - возможно PersonalAccountPeriodSummary.RecalcByPenalty
        ///  - 8) Оплата пени. Брать из раздела "Операции" в карточке ЛС.- НЕТ ТАКОГО ПОЛЯ - возможно PersonalAccountPeriodSummary.PenaltyPayment
        /// 3. Рассчитать Исходящее сальдо по тому же алгоритму, что сделано сейчас.Т.е.надо запустить расчет исходящего сальдо по готовому алгоритму.
        /// </summary>
        private void ExecuteFunction1(ChargePeriod[] periods)
        {
            this.Logger.LogInformation("Начало выполнения, метода '{0}'".FormatUsing(MethodBase.GetCurrentMethod()));

            using (var session = this.Container.Resolve<ISessionProvider>().OpenStatelessSession())
            {
                var transfersQuery = this.TransferDomain.GetAll()
                    .Where(x => (long)Math.Round(x.Amount * 100, 0) - x.Amount * 100 != 0);

                var persAccBtWalletSorce = this.PersAccDomain.GetAll()
                    .Where(x => transfersQuery.Any(y => y.SourceGuid == x.BaseTariffWallet.WalletGuid))
                    .Select(x => x.Id).ToHashSet();

                this.MakeRounde(persAccBtWalletSorce, periods, "persAccBtWalletSorce");

                var persAccDtWalletSorce = this.PersAccDomain.GetAll()
                    .Where(x => transfersQuery.Any(y => y.SourceGuid == x.DecisionTariffWallet.WalletGuid))
                    .Select(x => x.Id).ToHashSet();

                this.MakeRounde(persAccDtWalletSorce, periods, "persAccDtWalletSorce");

                var persAccPWalletSource = this.PersAccDomain.GetAll()
                    .Where(x => transfersQuery.Any(y => y.SourceGuid == x.PenaltyWallet.WalletGuid))
                    .Select(x => x.Id).ToHashSet();

                this.MakeRounde(persAccPWalletSource, periods, "persAccPWalletSource");

                var persAccBtWalletTarget = this.PersAccDomain.GetAll()
                    .Where(x => transfersQuery.Any(y => y.TargetGuid == x.BaseTariffWallet.WalletGuid))
                    .Select(x => x.Id).ToHashSet();

                this.MakeRounde(persAccBtWalletTarget, periods, "persAccBtWalletTarget");

                var persAccDtWalletTarget = this.PersAccDomain.GetAll()
                    .Where(x => transfersQuery.Any(y => y.TargetGuid == x.DecisionTariffWallet.WalletGuid))
                    .Select(x => x.Id).ToHashSet();

                this.MakeRounde(persAccDtWalletTarget, periods, "persAccDtWalletTarget");

                var persAccPWalletTransfer = this.PersAccDomain.GetAll()
                    .Where(x => transfersQuery.Any(y => y.TargetGuid == x.PenaltyWallet.WalletGuid))
                    .Select(x => x.Id).ToHashSet();

                this.MakeRounde(persAccPWalletTransfer, periods, "persAccPWalletTransfer");

                var pers = this.PersAccSummaryDomain.GetAll()
                    .Where(x => ((long)Math.Round(x.ChargeTariff * 100, 0) - x.ChargeTariff * 100 != 0) ||
                        ((long)Math.Round(x.ChargedByBaseTariff * 100, 0) - x.ChargedByBaseTariff * 100 != 0) ||
                        ((long)Math.Round(x.RecalcByBaseTariff * 100, 0) - x.RecalcByBaseTariff * 100 != 0) ||
                        ((long)Math.Round(x.RecalcByDecisionTariff * 100, 0) - x.RecalcByDecisionTariff * 100 != 0) ||
                        ((long)Math.Round(x.RecalcByPenalty * 100, 0) - x.RecalcByPenalty * 100 != 0) ||
                        ((long)Math.Round(x.Penalty * 100, 0) - x.Penalty * 100 != 0) ||
                        ((long)Math.Round(x.PenaltyPayment * 100, 0) - x.PenaltyPayment * 100 != 0) ||
                        ((long)Math.Round(x.TariffPayment * 100, 0) - x.TariffPayment * 100 != 0) ||
                        ((long)Math.Round(x.TariffDecisionPayment * 100, 0) - x.TariffDecisionPayment * 100 != 0) ||
                        ((long)Math.Round(x.BaseTariffChange * 100, 0) - x.BaseTariffChange * 100 != 0) ||
                        ((long)Math.Round(x.DecisionTariffChange * 100, 0) - x.DecisionTariffChange * 100 != 0) ||
                        ((long)Math.Round(x.PenaltyChange * 100, 0) - x.PenaltyChange * 100 != 0))
                    .Select(x => x.PersonalAccount.Id).ToHashSet();

                this.MakeRounde(pers, periods, "pers");

                this.Logger.LogInformation("Начало округления {0}".FormatUsing("walletGuids"));

                var walletGuids = this.TransferDomain.GetAll()
                    .Where(x => (long)Math.Round(x.Amount * 100, 0) - x.Amount * 100 != 0)
                    .Select(x => x.SourceGuid)
                    .ToList();

                walletGuids.AddRange(this.TransferDomain.GetAll()
                    .Where(x => (long)Math.Round(x.Amount * 100, 0) - x.Amount * 100 != 0)
                    .Select(x => x.TargetGuid)
                    .ToList());

                this.Logger.LogInformation("Началось округление {0}".FormatUsing("walletGuids"));

                // округляем поле в базе
                session.CreateSQLQuery(@"update regop_transfer set amount = round(amount, 2) where round(amount, 2) - amount <> 0 ")
                    .ExecuteUpdate();

                // если после округления остались нули - удаляем такие записи
                session.CreateSQLQuery(@"delete from regop_transfer where amount=0")
                    .ExecuteUpdate();

                var walletStep = 1000;
                for (int startIndex = 0; startIndex <= walletGuids.Count; startIndex += walletStep)
                {
                    var tempWalletGuids = walletGuids.Skip(startIndex).Take(walletStep).ToList();

                    this.WalletRepository.UpdateWalletBalance(tempWalletGuids);
                }

                session.CreateSQLQuery(@"update regop_pers_acc_charge 
                        set charge = round(charge, 2),
                        charge_tariff= round(charge_tariff, 2),
                        penalty= round(penalty, 2),
                        recalc= round(recalc, 2),
                        overplus= round(overplus, 2),
                        recalc_decision= round(recalc_decision, 2),
                        recalc_penalty= round(recalc_penalty, 2),
                        penalty_change= round(penalty_change, 2)
                        where 
                        round(charge, 2) - charge <> 0 or
                        round(charge_tariff, 2) - charge_tariff <> 0 or
                        round(penalty, 2) - penalty <> 0 or
                        round(recalc, 2) - recalc <> 0 or
                        round(overplus, 2) - overplus <> 0 or
                        round(recalc_decision, 2) - recalc_decision <> 0 or
                        round(recalc_penalty, 2) - recalc_penalty <> 0 or
                        round(penalty_change, 2) - penalty_change <> 0 ")
                    .ExecuteUpdate();
            }

            this.Logger.LogInformation("Успешное завершение, метода '{0}'".FormatUsing(MethodBase.GetCurrentMethod()));
        }

        /// <summary>
        /// Функция 2: В паспорте жилого дома, в разделе "Счет начислений" сделать обновление данных.
        /// Такое обновление данных производится при закрытии периода.
        /// </summary>
        private void ExecuteFunction2(ChargePeriod[] periods)
        {
            this.Logger.LogInformation("Обновление данных, метод '{0}'".FormatUsing(MethodBase.GetCurrentMethod()));
            using (var session = this.Container.Resolve<ISessionProvider>().OpenStatelessSession())
            {
                var action = new UpdateSaldoSqlAction(session);
                action.UpdateRoSaldos(periods);
            }

            this.Logger.LogInformation("Успешное завершение обновления данных, метод '{0}'".FormatUsing(MethodBase.GetCurrentMethod()));
        }

        /// <summary>
        /// Функция 3: В паспорте жилого дома, в разделе "Счет оплат" сделать округление значений в поле "Сумма (руб.)".
        /// </summary>
        private void ExecuteFunction3()
        {
            this.Logger.LogInformation("округление значений в поле 'Сумма(руб.)', метод '{0}'".FormatUsing(MethodBase.GetCurrentMethod()));

            using (var session = this.Container.Resolve<ISessionProvider>().OpenStatelessSession())
            {
                using (var tr = session.BeginTransaction())
                {
                    var walletsGuids = this.RealityObjectPaymentAccountDomain.GetAll()
                        .Where(x =>
                            x.AccumulatedFundWallet != null
                            && x.BankPercentWallet != null
                            && x.BaseTariffPaymentWallet != null
                            && x.DecisionPaymentWallet != null
                            && x.FundSubsidyWallet != null
                            && x.OtherSourcesWallet != null
                            && x.PenaltyPaymentWallet != null
                            && x.PreviosWorkPaymentWallet != null
                            && x.RegionalSubsidyWallet != null
                            && x.SocialSupportWallet != null
                            && x.RentWallet != null
                            && x.StimulateSubsidyWallet != null
                            && x.TargetSubsidyWallet != null
                        )
                        .Select(x => new
                        {
                            AccountId = x.Id,
                            guid1 = x.AccumulatedFundWallet.WalletGuid,
                            guid2 = x.BankPercentWallet.WalletGuid,
                            guid3 = x.BaseTariffPaymentWallet.WalletGuid,
                            guid4 = x.DecisionPaymentWallet.WalletGuid,
                            guid5 = x.FundSubsidyWallet.WalletGuid,
                            guid6 = x.OtherSourcesWallet.WalletGuid,
                            guid7 = x.PenaltyPaymentWallet.WalletGuid,
                            guid8 = x.PreviosWorkPaymentWallet.WalletGuid,
                            guid9 = x.RegionalSubsidyWallet.WalletGuid,
                            guid10 = x.SocialSupportWallet.WalletGuid,
                            guid11 = x.RentWallet.WalletGuid,
                            guid12 = x.StimulateSubsidyWallet.WalletGuid,
                            guid13 = x.TargetSubsidyWallet.WalletGuid,
                            w1 = x.AccumulatedFundWallet.Id,
                            w2 = x.BankPercentWallet.Id,
                            w3 = x.BaseTariffPaymentWallet.Id,
                            w4 = x.DecisionPaymentWallet.Id,
                            w5 = x.FundSubsidyWallet.Id,
                            w6 = x.OtherSourcesWallet.Id,
                            w7 = x.PenaltyPaymentWallet.Id,
                            w8 = x.PreviosWorkPaymentWallet.Id,
                            w9 = x.RegionalSubsidyWallet.Id,
                            w10 = x.SocialSupportWallet.Id,
                            w11 = x.RentWallet.Id,
                            w12 = x.StimulateSubsidyWallet.Id,
                            w13 = x.TargetSubsidyWallet.Id
                        })
                        .ToList();

                    string creditQuery =
                        "((select coalesce(sum(Amount), 0) from Transfer where SourceGuid in (:guids) and Operation.CanceledOperation = null)"
                        +
                        " - (select coalesce(sum(Amount), 0) from Transfer where TargetGuid in (:guids) and Operation.CanceledOperation <> null))";

                    string debtQuery =
                        "((select coalesce(sum(Amount), 0) from Transfer where TargetGuid in (:guids) and Operation.CanceledOperation = null)"
                        +
                        " - (select coalesce(sum(Amount), 0) from Transfer where SourceGuid in (:guids) and Operation.CanceledOperation <> null))";

                    string moneyLocksQuery = "(select coalesce(sum(Amount), 0) from MoneyLock"
                        + " where Wallet in (from Wallet where Id in (:ids))"
                        + " and IsActive=:isActive)";

                    string query = string.Format("update RealityObjectPaymentAccount set "
                        + " DebtTotal = {0} - {1},"
                        + " CreditTotal = {2},"
                        + " MoneyLocked = {1}"
                        + " where Id=:id",
                        debtQuery,
                        moneyLocksQuery,
                        creditQuery);

                    foreach (var x in walletsGuids)
                        session.CreateQuery(query)
                            .SetParameterList(
                                "guids",
                                new[]
                                {
                                    x.guid1, x.guid2, x.guid3,
                                    x.guid4, x.guid5, x.guid6,
                                    x.guid7, x.guid8, x.guid9,
                                    x.guid10, x.guid11, x.guid12,
                                    x.guid13
                                })
                            .SetParameterList(
                                "ids",
                                new[]
                                {
                                    x.w1, x.w2, x.w3,
                                    x.w4, x.w5, x.w6,
                                    x.w7, x.w8, x.w9,
                                    x.w10, x.w11, x.w12,
                                    x.w13
                                })
                            .SetBoolean("isActive", true)
                            .SetInt64("id", x.AccountId)
                            .ExecuteUpdate();

                    this.Logger.LogInformation("Обновление данных, метод '{0}'".FormatUsing(MethodBase.GetCurrentMethod()));

                    try
                    {
                        tr.Commit();
                    }
                    catch
                    {
                        tr.Rollback();
                        throw;
                    }
                }
            }
        }

        /// <summary>
        /// метод Округление
        /// </summary>
        /// <param name="persAccIds"></param>
        /// <param name="periods"></param>
        /// <param name="nameList"></param>
        private void MakeRounde(HashSet<long> persAccIds, ChargePeriod[] periods, string nameList)
        {
            if (nameList != null)
                this.Logger.LogInformation("Началось округление {0}".FormatUsing(nameList));

            using (var session = this.Container.Resolve<ISessionProvider>().OpenStatelessSession())
            {
                var excludePersAccs = this.GetExcludePersAccs();

                // удаляем id Лс 
                excludePersAccs.ForEach(x => persAccIds.Remove(x));

                var prevPeriodId = 0L;

                foreach (var period in periods)
                {
                    var step = 100;

                    for (int startIndex = 0; startIndex <= persAccIds.Count; startIndex += step)
                    {
                        var tempAccLists = persAccIds.Skip(startIndex).Take(step).ToArray();

                        var tempAccList = tempAccLists.IsEmpty() ? new long[] { 0 } : tempAccLists;

                        session.CreateSQLQuery(@"update regop_pers_acc_period_summ ps
                                set 
                                 charge_tariff = round(charge_tariff, 2),
                                 penalty = round(penalty, 2),
                                 recalc = round(recalc, 2),
                                 charge_base_tariff = round(charge_base_tariff, 2),
                                 recalc_decision = round(recalc_decision, 2),
                                 recalc_penalty = round(recalc_penalty, 2),
                                 saldo_in = coalesce((select ps2.saldo_out from regop_pers_acc_period_summ ps2 where ps2.account_id = ps.account_id and ps2.period_id = :prevPeriodId),0),
                                 base_tariff_debt = coalesce((select 
                                    coalesce(ps2.base_tariff_debt + ps2.charge_base_tariff + ps2.balance_change + ps2.recalc - ps2.tariff_payment, 0)
                                    from regop_pers_acc_period_summ ps2 where ps2.account_id = ps.account_id and ps2.period_id = :prevPeriodId),0),
                                 dec_tariff_debt = coalesce((select 
                                    coalesce(ps2.dec_tariff_debt + ps2.charge_tariff - ps2.charge_base_tariff + ps2.recalc_decision - ps2.tariff_desicion_payment, 0)
                                    from regop_pers_acc_period_summ ps2 where ps2.account_id = ps.account_id and ps2.period_id = :prevPeriodId),0),
                                 penalty_debt = coalesce((select 
                                    coalesce(ps2.penalty_debt  + ps2.penalty - ps2.penalty_payment, 0)
                                    from regop_pers_acc_period_summ ps2 where ps2.account_id = ps.account_id and ps2.period_id = :prevPeriodId),0),
                                 balance_change =  ( select coalesce(round(sum(tt.amount), 2),0)
                                     from (
                                     select
                                     t.amount
                                     from regop_transfer t
                                     join regop_wallet w on w.wallet_guid = t.source_guid
                                     join regop_pers_acc pa on pa.bt_wallet_id = w.id and pa.id = ps.account_id
                                     where t.reason = 'Установка/изменение сальдо' and date(p.cstart) <= date(t.operation_date) and (p.cend is null or date(t.operation_date) <= date(p.cend))) tt ),
                                 tariff_payment=(
                                 select coalesce(round(sum(tt.amount), 2),0)
                                 from (
                                 select
                                 t.amount
                                 from regop_transfer t
                                 join regop_wallet w on w.wallet_guid = t.target_guid
                                 join regop_pers_acc pa on pa.bt_wallet_id = w.id and pa.id = ps.account_id
                                 where (t.reason = 'Оплата по базовому тарифу' or t.reason = 'Поступление денег соц. поддержки' or t.reason = 'Зачисление по базовому тарифу в счет отмены возврата средств')
                                  and date(p.cstart) <= date(t.operation_date) and (p.cend is null or date(t.operation_date) <= date(p.cend))
 
                                 union all
 
                                 select
                                 (-1)*t.target_coef*t.amount as amount
                                 from regop_transfer t
                                 join regop_money_operation mo on mo.id = t.op_id
                                 join regop_wallet w on w.wallet_guid = t.source_guid
                                 join regop_pers_acc pa on pa.bt_wallet_id = w.id and pa.id = ps.account_id
                                 where (t.reason = 'Отмена оплаты по базовому тарифу' or t.reason = 'Возврат средств' or t.reason = 'Возврат взносов на КР' 
                                        or t.reason = 'Отмена поступления по соц. поддержке' or (mo.reason = 'Перенос долга при слиянии' and t.amount < 0)) 
                                   and date(p.cstart) <= date(t.operation_date) and (p.cend is null or date(t.operation_date) <= date(p.cend))
                                 ) tt
                                ),
 
                                TARIFF_DESICION_PAYMENT = (
                                 select coalesce(round(sum(tt.amount), 2),0)
                                 from (
                                 select
                                 t.amount
                                 from regop_transfer t
                                 join regop_wallet w on w.wallet_guid = t.target_guid
                                 join regop_pers_acc pa on pa.dt_wallet_id = w.id and pa.id = ps.account_id
                                 where (t.reason = 'Оплата по тарифу решения' or t.reason = 'Зачисление по тарифу решения в счет отмены возврата средств') and date(p.cstart) <= date(t.operation_date) and (p.cend is null or date(t.operation_date) <= date(p.cend))
 
                                 union all
 
                                 select
                                 (-1)*t.target_coef*t.amount as amount
                                 from regop_transfer t
                                 join regop_money_operation mo on mo.id = t.op_id
                                 join regop_wallet w on w.wallet_guid = t.source_guid
                                 join regop_pers_acc pa on pa.dt_wallet_id = w.id and pa.id = ps.account_id
                                 where (t.reason = 'Отмена оплаты по тарифу решения' or t.reason = 'Возврат средств' or t.reason = 'Возврат взносов на КР' or (mo.reason = 'Перенос долга при слиянии' and t.amount < 0))
                                   and date(p.cstart) <= date(t.operation_date) and (p.cend is null or date(t.operation_date) <= date(p.cend))
                                 ) tt
                                ),
                                PENALTY_PAYMENT = (
                                select coalesce(round(sum(tt.amount), 2),0)
                                 from (
                                 select
                                 t.amount
                                 from regop_transfer t
                                 join regop_wallet w on w.wallet_guid = t.target_guid
                                 join regop_pers_acc pa on pa.p_wallet_id = w.id and pa.id = ps.account_id
                                 where t.reason = 'Оплата пени' and date(p.cstart) <= date(t.operation_date) and (p.cend is null or date(t.operation_date) <= date(p.cend))
 
                                 union all
 
                                 select
                                 (-1)*t.target_coef*t.amount as amount
                                 from regop_transfer t
                                 join regop_money_operation mo on mo.id = t.op_id
                                 join regop_wallet w on w.wallet_guid = t.source_guid
                                 join regop_pers_acc pa on pa.p_wallet_id = w.id and pa.id = ps.account_id
                                 where (t.reason = 'Отмена оплаты пени' or (mo.reason = 'Перенос долга при слиянии' and t.amount < 0)) and date(p.cstart) <= date(t.operation_date) and (p.cend is null or date(t.operation_date) <= date(p.cend))
                                 ) tt
                                )
                                from regop_period p
                                where p.id = ps.period_id and p.id = :periodId and ps.account_id in (:accountIds)")
                            .SetInt64("periodId", period.Id)
                            .SetInt64("prevPeriodId", prevPeriodId)
                            .SetParameterList("accountIds", tempAccList)
                            .ExecuteUpdate();

                        session.CreateSQLQuery(@"update regop_pers_acc_period_summ ps
                                set saldo_out = saldo_in + charge_tariff + recalc + penalty + balance_change + recalc_decision - tariff_payment - tariff_desicion_payment - penalty_payment
                                  where ps.period_id = :periodId and ps.account_id in (:accountIds)")
                            .SetInt64("periodId", period.Id)
                            .SetParameterList("accountIds", tempAccList)
                            .ExecuteUpdate();
                    }

                    prevPeriodId = period.Id;
                }

                this.Logger.LogInformation("Успешное завершение округления {0}".FormatUsing(nameList));
            }
        }
    }
}