namespace Bars.Gkh.RegOperator.DomainModelServices.Impl
{
    using System;
    using System.Linq;
    using B4.DataAccess;

    using Bars.Gkh.Repositories.ChargePeriod;

    using Domain.Repository;
    using Domain.Repository.Wallets;
    using Entities;
    using Gkh.Domain.CollectionExtensions;
    using Gkh.Entities;

    /// <summary>
    /// Сервис обновления баланса дома
    /// </summary>
    public class RealityObjectAccountUpdater : IRealityObjectAccountUpdater
    {
        private readonly IPersonalAccountBalanceUpdater personalAccountUpdater;
        private readonly IRepository<PersonalAccountPeriodSummary> periodSummaryRepo;
        private readonly IRepository<RealityObjectPaymentAccount> paymentAccountRepo;
        private readonly IRepository<BasePersonalAccount> accountRepo;
        private readonly IChargePeriodRepository periodRepo;
        private readonly ISessionProvider sessions;
        private readonly IWalletRepository walletRepo;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="personalAccountUpdater">Сервис обновления баланса ЛС</param>
        /// <param name="periodSummaryRepo">Ситуация по ЛС на период</param>
        /// <param name="paymentAccountRepo">Сччёт оплат дома</param>
        /// <param name="accountRepo">Репозиторий ЛС</param>
        /// <param name="periodRepo">Репозиторий периодов</param>
        /// <param name="sessions">Провайдер сессий</param>
        /// <param name="walletRepo">Репозиторий кошельков</param>
        public RealityObjectAccountUpdater(IPersonalAccountBalanceUpdater personalAccountUpdater,
            IRepository<PersonalAccountPeriodSummary> periodSummaryRepo,
            IRepository<RealityObjectPaymentAccount> paymentAccountRepo,
            IRepository<BasePersonalAccount> accountRepo,
            IChargePeriodRepository periodRepo,
            ISessionProvider sessions,
            IWalletRepository walletRepo)
        {
            this.personalAccountUpdater = personalAccountUpdater;
            this.periodSummaryRepo = periodSummaryRepo;
            this.paymentAccountRepo = paymentAccountRepo;
            this.accountRepo = accountRepo;
            this.periodRepo = periodRepo;
            this.sessions = sessions;
            this.walletRepo = walletRepo;
        }

        /// <summary>
        /// Обновить баланс домов
        /// </summary>
        /// <param name="realityObjects">Запрос-фильтр домов</param>
        /// <param name="accountIds">Список лс, которые нужно пересчитать</param>
        public void UpdateBalance(IQueryable<RealityObject> realityObjects, long[] accountIds = null)
        {
            if (!realityObjects.Any())
            {
                return;
            }

            if (accountIds != null)
            {
                this.personalAccountUpdater.UpdateBalance(this.accountRepo.GetAll().Where(x => accountIds.Contains(x.Id)));
            }
            else
            {
                this.personalAccountUpdater.UpdateBalance(this.accountRepo.GetAll().Where(x => realityObjects.Any(r => r == x.Room.RealityObject)));
            }

            var openedPeriod = this.periodRepo.GetCurrentPeriod();

            if (openedPeriod == null)
            {
                throw new Exception("Нет открытого периода. Заведите открытый период.");
            }

            var accountSummaries = this.periodSummaryRepo.GetAll()
                .Where(x => x.Period.Id == openedPeriod.Id)
                .Where(x => realityObjects.Any(r => r == x.PersonalAccount.Room.RealityObject))
                .GroupBy(x => x.PersonalAccount.Room.RealityObject.Id)
                .Select(x => new
                {
                    RoId = x.Key,
                    ChargeTariff = x.Sum(y => y.ChargeTariff + y.RecalcByBaseTariff + y.RecalcByDecisionTariff +
                                                y.BaseTariffChange + y.DecisionTariffChange + y.PenaltyChange +
                                                y.Penalty + y.RecalcByPenalty),
                    ChargePenalty = x.Sum(s => s.Penalty + s.RecalcByPenalty + s.PenaltyChange),
                    PaidTariff = x.Sum(s => s.TariffPayment + s.TariffDecisionPayment),
                    PaidPenalty = x.Sum(s => s.PenaltyPayment),
                    SaldoIn = x.Sum(s => s.SaldoIn),
                    SaldoOut = x.Sum(s => s.SaldoOut)
                })
                .ToList();

            using(var session = this.sessions.OpenStatelessSession())
            using (var tr = session.BeginTransaction())
            {
                foreach (var summary in accountSummaries)
                {
                    session.CreateQuery(
                        "update RealityObjectChargeAccountOperation set " +
                        "ChargedTotal=:chargeTotal," +
                        "ChargedPenalty=:chargePenalty," +
                        "PaidTotal=:paidTotal," +
                        "PaidPenalty=:paidPenalty," +
                        "SaldoOut=:saldoOut," +
                        "SaldoIn=:saldoIn" +
                        " where Account in (from RealityObjectChargeAccount where RealityObject.Id=:roId)" +
                        " and Period.Id=:periodId")
                        .SetInt64("roId", summary.RoId)
                        .SetInt64("periodId", openedPeriod.Id)
                        .SetDecimal("chargeTotal", summary.ChargeTariff)
                        .SetDecimal("chargePenalty", summary.ChargePenalty)
                        .SetDecimal("paidTotal", summary.PaidTariff)
                        .SetDecimal("paidPenalty", summary.PaidPenalty)
                        .SetDecimal("saldoIn", summary.SaldoIn)
                        .SetDecimal("saldoOut", summary.SaldoOut)
                        .ExecuteUpdate();
                }

                var ros = realityObjects.Select(x => x.Id).ToList();

                var updateChargeAccsQuery = @"UPDATE regop_ro_charge_account acc
                    SET charge_total =
                    (
                        SELECT
                            SUM (ccharged + ccharged_penalty) AS charged
                        FROM
                            regop_ro_charge_acc_charge op
                        JOIN regop_ro_charge_account acc ON op.acc_id = acc. ID
                        JOIN gkh_reality_object ro ON acc.ro_id = ro. ID and ro_id = :roId	
                    ),
                     paid_total = 
                    (
                        SELECT
                            SUM (cpaid + cpaid_penalty) AS paid
                        FROM
                            regop_ro_charge_acc_charge op
                        JOIN regop_ro_charge_account acc ON op.acc_id = acc. ID
                        JOIN gkh_reality_object ro ON acc.ro_id = ro. ID  and ro_id = :roId	
                    )
                    where 
                     acc.ro_id = :roId";

                var updateChargeAccChargesQuery = @"
                    DROP table if exists t_summ;
                    CREATE temp table t_summ as
	                    SELECT d.id, a.period_id, sum(a.penalty) penalty, sum (a.recalc_penalty) recalc_penalty, sum (a.penalty_payment) penalty_payment
		                FROM 
                            REGOP_PERS_ACC_PERIOD_SUMM a
			            JOIN REGOP_PERS_ACC b ON b.id = a.account_id
			            JOIN gkh_room c ON c.id = b.room_id
			            JOIN REGOP_RO_CHARGE_ACCOUNT d ON d.ro_id = c.ro_id
		                WHERE c.ro_id = :roId" +
                            " GROUP BY d.id, a.period_id;" +
                    "UPDATE REGOP_RO_CHARGE_ACC_CHARGE c set ccharged_penalty = s.penalty + s.recalc_penalty, cpaid_penalty = s.penalty_payment " +
                        "FROM t_summ s " +
                        "WHERE s.id = c.acc_id " +
                            "and s.period_id = c.period_id;";

                foreach (var roId in ros)
                {
                    session.CreateSQLQuery(updateChargeAccsQuery).SetInt64("roId", roId).ExecuteUpdate();
                    session.CreateSQLQuery(updateChargeAccChargesQuery).SetInt64("roId", roId).ExecuteUpdate();
                }

                var walletsGuids = this.paymentAccountRepo.GetAll()
                    .Where(x => realityObjects.Any(r => r == x.RealityObject))
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

                

                foreach (var wallets in walletsGuids.Section(1000))
                {
                    var guids = wallets
                    .SelectMany(x => new[]
                    {
                        x.guid1, x.guid2, x.guid3,
                        x.guid4, x.guid5, x.guid6,
                        x.guid7, x.guid8, x.guid9,
                        x.guid10, x.guid11, x.guid12,
                        x.guid13
                    })
                    .ToList();

                    this.walletRepo.UpdateWalletBalance(guids, realityObject: true);
                }

                string creditQuery =
                    "((select coalesce(sum(Amount), 0) from RealityObjectTransfer where Owner.Id = :id and SourceGuid in (:guids) and Operation.CanceledOperation = null)"
                    + " - (select coalesce(sum(Amount), 0) from RealityObjectTransfer where Owner.Id = :id and TargetGuid in (:guids) and Operation.CanceledOperation <> null))";

                string debtQuery =
                    "((select coalesce(sum(Amount), 0) from RealityObjectTransfer where Owner.Id = :id and TargetGuid in (:guids) and Operation.CanceledOperation = null)"
                    + " - (select coalesce(sum(Amount), 0) from RealityObjectTransfer where Owner.Id = :id and SourceGuid in (:guids) and Operation.CanceledOperation <> null))";

                string moneyLocksQuery = "(select coalesce(sum(Amount), 0) from MoneyLock"
                                      + " where Wallet in (from Wallet where Id in (:ids))"
                                      + " and IsActive=:isActive)";

                string query = string.Format("update RealityObjectPaymentAccount set "
                                             + " DebtTotal = {0} - {1},"
                                             + " CreditTotal = {2},"
                                             + " MoneyLocked = {1}"
                                             + " where Id=:id", debtQuery, moneyLocksQuery, creditQuery);

                foreach (var x in walletsGuids)
                {
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
                }

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

        /// <summary>
        /// Обновить баланс кредит и дебет 
        /// </summary>
        /// <param name="realityObjects">Запрос-фильтр домов</param>
        public void UpdateCreditsAndDebts(IQueryable<RealityObject> realityObjects)
        {
            if (!realityObjects.Any())
                return;

            var openedPeriod = this.periodRepo.GetCurrentPeriod();

            if (openedPeriod == null)
            {
                throw new Exception("Нет открытого периода. Заведите открытый период.");
            }

            using (var session = this.sessions.OpenStatelessSession())
            using (var tr = session.BeginTransaction())
            {
                var walletsGuids = this.paymentAccountRepo.GetAll()
                    .Where(x => realityObjects.Any(r => r.Id == x.RealityObject.Id))
                    .Where(
                        x =>
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
                                && x.TargetSubsidyWallet != null)
                    .Select(
                        x => new
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
                    "((select coalesce(sum(Amount), 0) from RealityObjectTransfer where Owner.Id = :id and SourceGuid in (:guids) and Operation.CanceledOperation = null)"
                        + " - (select coalesce(sum(Amount), 0) from RealityObjectTransfer where Owner.Id = :id and TargetGuid in (:guids) and Operation.CanceledOperation <> null))";

                string debtQuery =
                    "((select coalesce(sum(Amount), 0) from RealityObjectTransfer where Owner.Id = :id and where TargetGuid in (:guids) and Operation.CanceledOperation = null)"
                        + " - (select coalesce(sum(Amount), 0) RealityObjectTransfer where Owner.Id = :id and SourceGuid in (:guids) and Operation.CanceledOperation <> null))";

                string moneyLocksQuery = "(select coalesce(sum(Amount), 0) from MoneyLock"
                    + " where Wallet in (from Wallet where Id in (:ids))"
                    + " and IsActive=:isActive)";

                string query = string.Format(
                    "update RealityObjectPaymentAccount set "
                        + " DebtTotal = {0} - {1},"
                        + " CreditTotal = {2},"
                        + " MoneyLocked = {1}"
                        + " where Id=:id",
                    debtQuery,
                    moneyLocksQuery,
                    creditQuery);

                foreach (var x in walletsGuids)
                {
                    session.CreateQuery(query)
                        .SetParameterList(
                            "guids",
                            new[]
                            {
                                x.guid1,
                                x.guid2,
                                x.guid3,
                                x.guid4,
                                x.guid5,
                                x.guid6,
                                x.guid7,
                                x.guid8,
                                x.guid9,
                                x.guid10,
                                x.guid11,
                                x.guid12,
                                x.guid13
                            })
                        .SetParameterList(
                            "ids",
                            new[]
                            {
                                x.w1,
                                x.w2,
                                x.w3,
                                x.w4,
                                x.w5,
                                x.w6,
                                x.w7,
                                x.w8,
                                x.w9,
                                x.w10,
                                x.w11,
                                x.w12,
                                x.w13
                            })
                        .SetBoolean("isActive", true)
                        .SetInt64("id", x.AccountId)
                        .ExecuteUpdate();
                }

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
}