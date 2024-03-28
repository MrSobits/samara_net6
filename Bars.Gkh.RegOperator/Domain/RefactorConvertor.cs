namespace Bars.Gkh.RegOperator.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using B4.DataAccess;
    using B4.Modules.States;
    using B4.Utils;
    using B4;

    using Bars.Gkh.RegOperator.Entities.Refactor;

    using Repository;
    using ValueObjects;
    using DomainModelServices;
    using Entities;
    using Entities.PersonalAccount;
    using Entities.ValueObjects;
    using Entities.Wallet;
    using Enums;
    using Castle.Windsor;
    using Entities.Loan;
    using FastMember;
    using Gkh.Domain;
    using Gkh.Domain.CollectionExtensions;
    using Gkh.Entities;
    using Gkh.ExecutionAction;
    using Microsoft.Extensions.Logging;

    using Bars.Gkh.Repositories.ChargePeriod;

    using NHibernate;
    using NHibernate.Linq;

    /// <summary>Конвертер данных</summary>
    public partial class RefactorConvertor
    {
        private readonly IWindsorContainer container;

        private readonly IPersonalAccountPaymentCommand commandBasic;
        private readonly IPersonalAccountPaymentCommand commandPenalty;
        private readonly IPersonalAccountPaymentCommand commandSocialSupport;
        private readonly IPersonalAccountPaymentCommand commandRent;
        private readonly IPersonalAccountPaymentCommand commandPreviousWork;
        private readonly IPersonalAccountPaymentCommand commandAccumulatedFund;
        private ILogger logger;
        private Dictionary<DateTime, ChargePeriod> periods;


        public RefactorConvertor(IWindsorContainer container)
        {
            this.container = container;

            var personalAccountPaymentCommandFactory = container.Resolve<IPersonalAccountPaymentCommandFactory>();
            commandBasic = personalAccountPaymentCommandFactory.GetCommand(PaymentType.Basic);
            commandPenalty = personalAccountPaymentCommandFactory.GetCommand(PaymentType.Penalty);
            commandSocialSupport = personalAccountPaymentCommandFactory.GetCommand(PaymentType.SocialSupport);
            commandRent = personalAccountPaymentCommandFactory.GetCommand(PaymentType.Rent);
            commandPreviousWork = personalAccountPaymentCommandFactory.GetCommand(PaymentType.PreviousWork);
            commandAccumulatedFund = personalAccountPaymentCommandFactory.GetCommand(PaymentType.AccumulatedFund);

            this.logger = container.Resolve<ILogger>();

            this.periods = container.ResolveRepository<ChargePeriod>().GetAll().ToList().ToDictionary(x => x.StartDate);
        }

        /// <summary>Обработать НВС и неподтвержденную оплату</summary>
        /// <returns>Кол-во трансферов</returns>
        public int Process()
        {
            var transfers = new List<Transfer>();

            this.logger.LogDebug("Начало миграции данных");
            var action = container.ResolveAll<IExecutionAction>().FirstOrDefault(x => x.Code == "WalletsCreateAction");

            if (action != null)
            {
                action.Action();
            }

            var subQuery = container.Resolve<IDomainService<Transfer>>().GetAll();

            var data = new List<PaymentInfo>();

            var sessions = container.Resolve<ISessionProvider>();

            this.logger.LogDebug("Создание трансферов для проведенных начислений");

            ProcessCharges();

            this.logger.LogDebug("Созданы трансферы для проведенных начислений");

            sessions.CloseCurrentSession();

            // получаем трансферы для ручного изменения пени
            this.logger.LogDebug("Создание трансферов для изменений пеней");
            container.InTransaction(() => CreateTransferForPenaltyChanges(transfers));
            this.logger.LogDebug("Созданы трансферы для изменения пеней");

            sessions.CloseCurrentSession();

            // По аренде
            this.logger.LogDebug("Создание трансферов для проведенных распределений по оплате аренды");
            container.InTransaction(() => data.AddRange(
                container.Resolve<IDomainService<SuspenseAccountRentPaymentIn>>().GetAll()
                    .Fetch(x => x.SuspenceAccount)
                    .ThenFetchMany(x => x.Operations)
                    .Where(x => subQuery.All(y => y.SourceGuid != x.SuspenceAccount.TransferGuid))
                    .Select(
                        x => new PaymentInfo
                        {
                            MoneyOperationSource = x.SuspenceAccount,
                            TransferParty = x.SuspenceAccount,
                            Account = x.Payment.Account,
                            PaymentType = PaymentType.Rent,
                            DateReceipt = x.Payment.OperationDate,
                            Sum = x.Payment.Sum
                        })
                    .ToArray()));

            // По предыдущей работе
            this.logger.LogDebug("Создание трансферов для проведенных распределений по предыдущим работам");
            container.InTransaction(() => data.AddRange(
                container.Resolve<IDomainService<SuspenseAccountPreviousWorkPayment>>().GetAll()
                    .Fetch(x => x.SuspenceAccount)
                    .ThenFetchMany(x => x.Operations)
                    .Where(x => subQuery.All(y => y.SourceGuid != x.SuspenceAccount.TransferGuid))
                    .Select(
                        x => new PaymentInfo
                        {
                            MoneyOperationSource = x.SuspenceAccount,
                            TransferParty = x.SuspenceAccount,
                            Account = x.PreviousWorkPayment.Account,
                            PaymentType = PaymentType.PreviousWork,
                            DateReceipt = x.PreviousWorkPayment.OperationDate,
                            Sum = x.PreviousWorkPayment.Sum
                        })
                    .ToArray()));

            // Ранее накопленные средства
            this.logger.LogDebug("Создание трансферов для проведенных распределений по накопленным средствам");
            container.InTransaction(() => data.AddRange(
                container.Resolve<IDomainService<SuspenseAccountAccumFunds>>().GetAll()
                    .Fetch(x => x.SuspenceAccount)
                    .ThenFetchMany(x => x.Operations)
                    .Where(x => subQuery.All(y => y.SourceGuid != x.SuspenceAccount.TransferGuid))
                    .Select(
                        x => new PaymentInfo
                        {
                            MoneyOperationSource = x.SuspenceAccount,
                            TransferParty = x.SuspenceAccount,
                            Account = x.AccumFunds.Account,
                            PaymentType = PaymentType.AccumulatedFund,
                            DateReceipt = x.AccumFunds.OperationDate,
                            Sum = x.AccumFunds.Sum
                        })
                    .ToArray()));

            var paymentGuids = container.Resolve<IDomainService<UnacceptedPayment>>().GetAll()
                .Where(x => x.Accepted)
                .Where(x => subQuery.All(y => y.SourceGuid != x.TransferGuid))
                .Select(x => new
                {
                    x.Guid,
                    x.TransferGuid
                }).ToList().ToDictionary(x => x.Guid);

            GC.Collect();
            GC.WaitForPendingFinalizers();

            var ids = container.Resolve<IRepository<PersonalAccountPayment>>().GetAll()
                .Where(x => subQuery.All(s => s.SourceGuid != x.Guid))
                .Where(
                    x => container.Resolve<IDomainService<UnacceptedPayment>>().GetAll()
                        .Where(u => u.Accepted).All(u => subQuery.All(s => s.SourceGuid != u.TransferGuid)))
                .Select(x => x.Id).ToList();

            var count = 0;
            var idsCount = ids.Count();

            this.logger.LogDebug("Создание трансферов для оплат по ЛС");
            while (count < idsCount)
            {
                var take = ids.Skip(count).Take(1000).ToList();

                data.AddRange(
                    container.Resolve<IRepository<PersonalAccountPayment>>().GetAll()
                        .Fetch(x => x.BasePersonalAccount)
                        .ThenFetch(x => x.BaseTariffWallet)
                        .Fetch(x => x.BasePersonalAccount)
                        .ThenFetch(x => x.AccumulatedFundWallet)
                        .Fetch(x => x.BasePersonalAccount)
                        .ThenFetch(x => x.RestructAmicableAgreementWallet)
                        .Fetch(x => x.BasePersonalAccount)
                        .ThenFetch(x => x.DecisionTariffWallet)
                        .Fetch(x => x.BasePersonalAccount)
                        .ThenFetch(x => x.PenaltyWallet)
                        .Fetch(x => x.BasePersonalAccount)
                        .ThenFetch(x => x.PreviosWorkPaymentWallet)
                        .Fetch(x => x.BasePersonalAccount)
                        .ThenFetch(x => x.RentWallet)
                        .Fetch(x => x.BasePersonalAccount)
                        .ThenFetch(x => x.SocialSupportWallet)
                        .Where(x => take.Contains(x.Id))
                        .ToList()
                        .Select(
                            x => new PaymentInfo
                            {
                                MoneyOperationSource = new DummySource(new DummyParty(x.Guid)),
                                TransferParty = new DummyParty(x.Guid),
                                Account = x.BasePersonalAccount,
                                PaymentType = x.Type,
                                DateReceipt = x.PaymentDate,
                                Sum = x.Sum
                            })
                        .Select(
                            x => new PaymentInfo
                            {
                                MoneyOperationSource =
                                    new DummySource(
                                        new DummyParty(
                                            paymentGuids.Get(x.TransferParty.TransferGuid)
                                                .Return(p => p.TransferGuid, x.TransferParty.TransferGuid))),
                                TransferParty =
                                    new DummyParty(
                                        paymentGuids.Get(x.TransferParty.TransferGuid)
                                            .Return(p => p.TransferGuid, x.TransferParty.TransferGuid)),
                                Account = x.Account,
                                PaymentType = x.PaymentType,
                                DateReceipt = x.DateReceipt,
                                Sum = x.Sum
                            }).ToArray());

                sessions.CloseCurrentSession();

                GC.Collect();
                GC.WaitForPendingFinalizers();

                count += 1000;
            }

            container.InTransaction(() => ApplyPaymentsToAccount(data, transfers));

            sessions.CloseCurrentSession();

            // Проводим оплаты дома
            this.logger.LogDebug("Создание трансферов для оплат по домам");
            container.InTransaction(() => ApplyRealityObjectPayments(transfers));

            sessions.CloseCurrentSession();

            // Проводим оплаты по актам
            this.logger.LogDebug("Создание трансферов для оплат по актам");
            container.InTransaction(ApplyRealityObjectActPayments);

            sessions.CloseCurrentSession();

            var operations = transfers.Select(x => x.Operation).ToList();

            TransactionHelper.InsertInManyTransactions(container, operations, 1000, false, true);
            TransactionHelper.InsertInManyTransactions(container, transfers, 1000, false, true);

            this.logger.LogDebug("Обновление балансов");
            UpdateBalances();

            CreateLoanWalletsAndTransfers();

            sessions.OpenStatelessSession()
                .CreateSQLQuery("update regop_wallet set has_new_ops = :true")
                .SetBoolean("true", true)
                .ExecuteUpdate();

            return transfers.Count;
        }

        private void CreateTransferForPenaltyChanges(List<Transfer> transfers)
        {
        }

        public long UpdateMoneyLocks()
        {
            var paymentRepo = container.ResolveRepository<RealityObjectPaymentAccount>();
            var sessions = container.Resolve<ISessionProvider>();

            UpdatePaymentAccounts(paymentRepo, sessions);
            return 1;

        }

        private void UpdateBalances()
        {
            ProcessBalanceChange();

            var roDomain = container.ResolveDomain<RealityObject>();
            var personalAccRepo = container.ResolveRepository<BasePersonalAccount>();
            var paymentRepo = container.ResolveRepository<RealityObjectPaymentAccount>();
            var transfers = container.ResolveDomain<Transfer>();
            var summaryRepo = container.ResolveRepository<PersonalAccountPeriodSummary>();
            var sessions = container.Resolve<ISessionProvider>();

            var idsCount = roDomain.GetAll().Count();

            var processed = 0;
            while (processed < idsCount)
            {
                this.logger.LogDebug("Обновление балансов для домов {0}/{1}", processed, idsCount);

                var personalAccounts = personalAccRepo.GetAll()
                    .Where(x => roDomain.GetAll().Skip(processed).Take(1000).Any(p => p == x.Room.RealityObject))
                    .Select(x => new
                    {
                        RoId = x.Room.RealityObject.Id,
                        AccountId = x.Id,
                        b_Guid = x.BaseTariffWallet.WalletGuid,
                        d_Guid = x.DecisionTariffWallet.WalletGuid,
                        p_Guid = x.PenaltyWallet.WalletGuid,
                        af_Guid = x.AccumulatedFundWallet.WalletGuid,
                        raa_Guid = x.RestructAmicableAgreementWallet.WalletGuid,
                        pwp_Guid = x.PreviosWorkPaymentWallet.WalletGuid,
                        r_Guid = x.RentWallet.WalletGuid,
                        ss_Guid = x.SocialSupportWallet.WalletGuid
                    });

                var guids =
                    personalAccounts
                        .Select(x => new { x.b_Guid, x.d_Guid, x.p_Guid })
                        .ToList()
                        .SelectMany(x => new[] { x.b_Guid, x.d_Guid, x.p_Guid })
                        .Distinct()
                        .ToList();

                // Получаем инфу по периодам
                var summaries = summaryRepo.GetAll()
                    .Where(x => personalAccounts.Any(a => a.AccountId == x.PersonalAccount.Id))
                    .Select(x => new
                    {
                        x.Id,
                        AccountId = x.PersonalAccount.Id,
                        PeriodId = x.Period.Id,
                        ChargedByBaseTariff = x.ChargedByBaseTariff + x.RecalcByBaseTariff,
                        ChargedByDecisionTariff = x.ChargeTariff - x.ChargedByBaseTariff + x.RecalcByDecisionTariff,
                        x.Penalty,
                        Start = x.Period.StartDate,
                        End = x.Period.EndDate ?? DateTime.MaxValue
                    })
                    .ToList()
                    .GroupBy(x => x.AccountId)
                    .ToDictionary(x => x.Key);

                var trs = new List<MoneyByDateAndWallets>();

                var portion = 0;

                while (portion < guids.Count)
                {
                    var gs = guids.Skip(portion).Take(10000).ToList();

                    trs.AddRange(transfers.GetAll()
                    .Where(x => gs.Contains(x.TargetGuid))
                    .OrderBy(x => x.OperationDate)
                    .Select(x => new MoneyByDateAndWallets
                    {
                        OperationDate = x.OperationDate,
                        Amount = x.Amount,
                        TargetGuid = x.TargetGuid
                    })
                    .ToList());

                    portion += 10000;
                }

                var trs2 = trs
                    .GroupBy(x => x.OperationDate)
                    .Select(x => new
                    {
                        Date = x.Key,
                        ByWallets =
                            x.GroupBy(w => w.TargetGuid).ToDictionary(w => w.Key, w => w.Sum(s => s.Amount))
                    }).ToList();

                var accountSummary = new List<AccountSummary>();

                var session = sessions.OpenStatelessSession();
                using (var tr = session.BeginTransaction())
                {
                    this.logger.LogDebug("Обновление балансов ЛС");
                    foreach (var account in personalAccounts)
                    {
                        var group = summaries.Get(account.AccountId);

                        if (group != null)
                        {
                            // Обновляем инфу по периодам
                            foreach (var summary in group)
                            {
                                var summary1 = summary;
                                var tariff =
                                    trs2.Where(
                                        x =>
                                            x.Date.DateBetween(summary1.Start, summary1.End)
                                            && x.ByWallets.ContainsKey(account.b_Guid))
                                        .Sum(x => x.ByWallets[account.b_Guid]);
                                var decision =
                                    trs2.Where(
                                        x =>
                                            x.Date.DateBetween(summary1.Start, summary1.End)
                                            && x.ByWallets.ContainsKey(account.d_Guid))
                                        .Sum(x => x.ByWallets[account.d_Guid]);
                                var penalty =
                                    trs2.Where(
                                        x =>
                                            x.Date.DateBetween(summary1.Start, summary1.End)
                                            && x.ByWallets.ContainsKey(account.p_Guid))
                                        .Sum(x => x.ByWallets[account.p_Guid]);

                                session.CreateQuery(
                                    "update PersonalAccountPeriodSummary set " +
                                    "TariffPayment=:tariff," +
                                    "PenaltyPayment=:penalty," +
                                    "TariffDecisionPayment=:decision " +
                                    "where Id=:id")
                                    .SetInt64("id", summary1.Id)
                                    .SetDecimal("tariff", tariff)
                                    .SetDecimal("penalty", penalty)
                                    .SetDecimal("decision", decision)
                                    .ExecuteUpdate();

                                // Попутно заполняем инфу по периоду по лс
                                accountSummary.Add(
                                    new AccountSummary
                                    {
                                        AccountId = account.AccountId,
                                        PeriodId = summary1.PeriodId,
                                        RoId = account.RoId,
                                        ChargeByBase = summary.ChargedByBaseTariff,
                                        ChargeByDecision = summary1.ChargedByDecisionTariff,
                                        ChargeByPenalty = summary1.Penalty,
                                        PaidByBase = tariff,
                                        PaidByDecision = decision,
                                        PaidByPenalty = penalty
                                    });
                            }
                        }
                    }

                    var accsWallets = personalAccounts.ToList();

                    var accsWalletsGroup = accsWallets.ToList()
                        .GroupBy(x => x.RoId)
                        .ToDictionary(x => x.Key, x => x.Select(s => s));

                    var paymentIds = accsWalletsGroup.Select(x => x.Key).ToList();

                    var paymentAccWallets = paymentRepo.GetAll()
                        .Where(x => paymentIds.Contains(x.RealityObject.Id))
                        .Select(x => new
                        {
                            x.Id,
                            RoId = x.RealityObject.Id,
                            af = x.AccumulatedFundWallet.WalletGuid,
                            b = x.BaseTariffPaymentWallet.WalletGuid,
                            d = x.DecisionPaymentWallet.WalletGuid,
                            p = x.PenaltyPaymentWallet.WalletGuid,
                            pwp = x.PreviosWorkPaymentWallet.WalletGuid,
                            rw = x.RentWallet.WalletGuid,
                            ss = x.SocialSupportWallet.WalletGuid
                        })
                        .ToList();

                    var gs =
                        personalAccounts.ToList().SelectMany(
                            s => new[] { s.af_Guid, s.raa_Guid, s.b_Guid, s.d_Guid, s.p_Guid, s.pwp_Guid, s.r_Guid, s.ss_Guid })
                            .Distinct()
                            .ToArray();

                    var trans = new List<Transfer>();

                    var count = 0;
                    while (count < gs.Length)
                    {
                        var take = gs.Skip(count).Take(10000).ToList();

                        trans.AddRange(
                            transfers.GetAll()
                                .Fetch(x => x.Operation)
                                .Where(x => !x.IsInDirect)
                                .Where(x => take.Contains(x.TargetGuid))
                                .Where(x => transfers.GetAll().All(t => t.Originator != x))
                                .ToList());

                        count += 10000;
                    }

                    this.logger.LogDebug("Копирование трансферов из ЛС в дома");
                    foreach (var paymentAccWallet in paymentAccWallets)
                    {
                        if (!accsWalletsGroup.ContainsKey(paymentAccWallet.RoId)) continue;

                        var wallet = paymentAccWallet;
                        CopyTransfers(paymentAccWallet.af, trans.Where(x => accsWalletsGroup[wallet.RoId].Select(a => a.af_Guid).Contains(x.TargetGuid)), session);
                        CopyTransfers(paymentAccWallet.af, trans.Where(x => accsWalletsGroup[wallet.RoId].Select(a => a.raa_Guid).Contains(x.TargetGuid)), session);
                        CopyTransfers(paymentAccWallet.b, trans.Where(x => accsWalletsGroup[wallet.RoId].Select(a => a.b_Guid).Contains(x.TargetGuid)), session);
                        CopyTransfers(paymentAccWallet.d, trans.Where(x => accsWalletsGroup[wallet.RoId].Select(a => a.d_Guid).Contains(x.TargetGuid)), session);
                        CopyTransfers(paymentAccWallet.p, trans.Where(x => accsWalletsGroup[wallet.RoId].Select(a => a.p_Guid).Contains(x.TargetGuid)), session);
                        CopyTransfers(paymentAccWallet.pwp, trans.Where(x => accsWalletsGroup[wallet.RoId].Select(a => a.pwp_Guid).Contains(x.TargetGuid)), session);
                        CopyTransfers(paymentAccWallet.rw, trans.Where(x => accsWalletsGroup[wallet.RoId].Select(a => a.r_Guid).Contains(x.TargetGuid)), session);
                        CopyTransfers(paymentAccWallet.ss, trans.Where(x => accsWalletsGroup[wallet.RoId].Select(a => a.ss_Guid).Contains(x.TargetGuid)), session);
                    }

                    processed += 1000;

                    session.CreateSQLQuery(@"UPDATE regop_pers_acc acc
SET tariff_charge_balance = wallet.tariff_balance,
 decision_charge_balance = wallet.decision_balance,
 penalty_charge_balance = wallet.penalty_balance
FROM (
        SELECT
            A . ID AS account_id,
            sum(s.charge_base_tariff - tariff_payment + s.recalc) AS tariff_balance,
            sum(s.charge_tariff - s.charge_base_tariff - tariff_desicion_payment) AS decision_balance,
            sum(s.penalty - s.penalty_payment) AS penalty_balance
        FROM
            regop_pers_acc A
        JOIN regop_pers_acc_period_summ s ON A .ID = s. account_id		
        GROUP BY A.ID
    ) AS wallet
WHERE
    acc. ID = wallet.account_id").ExecuteUpdate();

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

                sessions.CloseCurrentSession();
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }

            UpdateOpenPeriodSummaries(sessions);

            UpdateWalletsBalances(sessions);

            UpdatePaymentAccounts(paymentRepo, sessions);

            UpdateChargeAccounts(sessions);
        }


        private void UpdateOpenPeriodSummaries(ISessionProvider sessions)
        {
            var summaries = container.ResolveRepository<PersonalAccountPeriodSummary>();
            sessions.CloseCurrentSession();

            var summariesOpenPeriod = summaries.GetAll().Where(x => !x.Period.IsClosed)
                .ToList();

            var session = sessions.OpenStatelessSession();

            using (session)
            using (var tr = session.BeginTransaction())
            {
                foreach (var personalAccountPeriodSummary in summariesOpenPeriod)
                {
                    personalAccountPeriodSummary.RecalcSaldoOut();
                    session.Update(personalAccountPeriodSummary);
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

            sessions.CloseCurrentSession();
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }


        private void UpdateChargeAccounts(ISessionProvider sessions)
        {
            var ros = container.ResolveRepository<RealityObject>();
            var opers = container.ResolveRepository<RealityObjectChargeAccountOperation>();
            var summaries = container.ResolveRepository<PersonalAccountPeriodSummary>();

            sessions.CloseCurrentSession();

            var count = 0;
            var rosCount = ros.GetAll().Count();

            while (count < rosCount)
            {
                var session = sessions.OpenStatelessSession();

                using (session)
                using (var tr = session.BeginTransaction())
                {
                    var realityObjects = ros.GetAll().Skip(count).Take(1000);

                    var sums = summaries.GetAll()
                        .Where(x => realityObjects.Any(r => r == x.PersonalAccount.Room.RealityObject))
                        .Select(x => new
                        {
                            RoId = x.PersonalAccount.Room.RealityObject.Id,
                            PeriodId = x.Period.Id,
                            ChargeBase = x.ChargedByBaseTariff,
                            ChargeDecision = x.ChargeTariff - x.ChargedByBaseTariff,
                            ChargePenalty = x.Penalty,
                            PaidBase = x.TariffPayment,
                            PaidDecision = x.TariffDecisionPayment,
                            PaidPenalty = x.PenaltyPayment,
                            BalanceChange = x.BaseTariffChange + x.DecisionTariffChange + x.PenaltyChange,
                            Recalc = x.RecalcByBaseTariff
                        })
                        .ToList()
                        .GroupBy(x => new { x.RoId, x.PeriodId })
                        .ToDictionary(x => x.Key,
                            x => new
                            {
                                ChargeBase = x.Sum(s => s.ChargeBase),
                                ChargeDecision = x.Sum(s => s.ChargeDecision),
                                ChargePenalty = x.Sum(s => s.ChargePenalty),
                                PaidBase = x.Sum(s => s.PaidBase),
                                PaidDecision = x.Sum(s => s.PaidDecision),
                                PaidPenalty = x.Sum(s => s.PaidPenalty),
                                BalanceChange = x.Sum(s => s.BalanceChange),
                                Recalc = x.Sum(s => s.Recalc)
                            });

                    var operations = opers.GetAll()
                        .Where(x => realityObjects.Any(r => r == x.Account.RealityObject))
                        .Select(x => new
                        {
                            RoId = x.Account.RealityObject.Id,
                            PeriodId = x.Period.Id,
                            AccountId = x.Account.Id,
                            x.Period.StartDate
                        })
                        .ToList();

                    var saldoDict = new Dictionary<long, decimal>();

                    foreach (var operation in operations.OrderBy(x => x.StartDate))
                    {
                        var key = new { operation.RoId, operation.PeriodId };

                        if (!sums.ContainsKey(key)) continue;

                        decimal saldo;
                        if (!saldoDict.TryGetValue(operation.RoId, out saldo))
                        {
                            saldoDict[operation.RoId] = 0;
                        }

                        var roInfo = sums[key];

                        var chargeTotal = (roInfo.ChargeBase + roInfo.ChargeDecision + roInfo.ChargePenalty
                                           + roInfo.BalanceChange + roInfo.Recalc);
                        var paymentTotal = (roInfo.PaidBase + roInfo.PaidDecision + roInfo.PaidPenalty);
                        var saldoOut = saldo + chargeTotal - paymentTotal;

                        session.CreateSQLQuery(
                            "update REGOP_RO_CHARGE_ACC_CHARGE set " +
                            "CCHARGED=:chargeTotal," +
                            "CCHARGED_PENALTY=:chargedPenalty," +
                            "CPAID=:paidTotal," +
                            "CPAID_PENALTY=:paidPenalty," +
                            "CSALDO_IN=:saldoIn," +
                            "CSALDO_OUT=:saldoOut" +
                            " where PERIOD_ID=:periodId and ACC_ID=(:roId)")
                            .SetInt64("periodId", operation.PeriodId)
                            .SetInt64("roId", operation.AccountId)
                            .SetDecimal("chargeTotal", chargeTotal)
                            .SetDecimal("chargedPenalty", roInfo.ChargePenalty)
                            .SetDecimal("paidTotal", paymentTotal)
                            .SetDecimal("paidPenalty", roInfo.PaidPenalty)
                            .SetDecimal("saldoIn", saldo)
                            .SetDecimal("saldoOut", saldoOut)
                            .ExecuteUpdate();

                        saldoDict[operation.RoId] = saldoOut;
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

                count += 1000;

                sessions.CloseCurrentSession();
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }

            sessions.OpenStatelessSession().CreateSQLQuery(@"UPDATE regop_ro_charge_account
SET charge_total = ch2.charged + ch2.c_penalty,
 paid_total = ch2.paid + ch2.p_penalty
FROM
    (
        SELECT
            ch.acc_id,
            SUM (ch.balance_change) AS balance,
            SUM (ch.ccharged) AS charged,
            SUM (ch.ccharged_penalty) AS c_penalty,
            SUM (ch.cpaid) AS paid,
            SUM (ch.cpaid_penalty) AS p_penalty
        FROM
            regop_ro_charge_acc_charge ch
        GROUP BY
            ch.acc_id
    ) AS ch2
WHERE
    ID = ch2.acc_id").ExecuteUpdate();
        }

        private void UpdatePaymentAccounts(IRepository<RealityObjectPaymentAccount> paymentRepo, ISessionProvider sessions)
        {
            this.logger.LogDebug("Обновление счетов оплат домов");
            var paymentAccs = paymentRepo.GetAll().Paging(new LoadParam { Start = 0, Limit = 100 }).FetchAllWallets().ToList();
            var locksDomain = container.Resolve<IDomainService<MoneyLock>>();
            var locks = locksDomain.GetAll().ToList().ToDictionary(x => x.Id, x => x.Amount);

            var accessor = TypeAccessor.Create(typeof(RealityObjectPaymentAccount));
            var props =
                typeof(RealityObjectPaymentAccount).GetProperties()
                    .Where(x => x.PropertyType == typeof(Wallet))
                    .ToArray();

            Func<RealityObjectPaymentAccount, decimal> debtFunc = acc =>
            {
                var accum = 0M;

                foreach (var walletProp in props)
                {
                    var wallet = ((Wallet)accessor[acc, walletProp.Name]);
                    accum += wallet.InTransfers.Where(x => !x.Operation.IsCancelled).SafeSum(x => x.Amount);
                }

                return accum;
            };

            Func<RealityObjectPaymentAccount, decimal> creditFunc = acc =>
            {
                var accum = 0M;

                foreach (var walletProp in props)
                {
                    var wallet = ((Wallet)accessor[acc, walletProp.Name]);
                    accum += wallet.OutTransfers.Where(x => !x.Operation.IsCancelled).SafeSum(x => x.Amount);
                }

                return accum;
            };

            Func<RealityObjectPaymentAccount, decimal> moneyLockFunc = acc =>
            {
                var accum = 0M;

                foreach (var walletProp in props)
                {
                    var wallet = ((Wallet)accessor[acc, walletProp.Name]);
                    accum += locks.Get(wallet.Id);
                }

                return accum;
            };

            var ss = sessions.OpenStatelessSession();

            using (var tr = ss.BeginTransaction())
            {
                foreach (var acc in paymentAccs)
                {
                    acc.DebtTotal = debtFunc(acc);
                    acc.CreditTotal = creditFunc(acc);
                    acc.MoneyLocked = moneyLockFunc(acc);

                    ss.Update(acc);
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

        private void UpdateWalletsBalances(ISessionProvider sessions)
        {
            this.logger.LogDebug("Обновление балансов кошельков");
            sessions.OpenStatelessSession().CreateSQLQuery(@"UPDATE regop_wallet w2
SET balance = round(wallet.balance,2),
FROM
    (
        SELECT
            w. ID,
            ((select COALESCE (sum(amount*target_coef), 0) from regop_transfer where target_guid = w.wallet_guid where amount*target_coef > 0 or is_affect)-
			 (select COALESCE (sum(t.amount*target_coef), 0) from regop_transfer t
               join regop_money_operation mo on mo.id = t.op_id 
                 where t.source_guid = w.wallet_guid and (t.is_affect or mo.canceled_op_id is not null)) -
            (select COALESCE (sum(amount), 0) from regop_money_lock where wallet_id = w.id)) as balance
        FROM
            regop_wallet w
    ) AS wallet
WHERE
    wallet.ID = w2.ID").ExecuteUpdate();
        }

        private void CopyTransfers(string af, IEnumerable<Transfer> @where, IStatelessSession session)
        {
        }

        /// <summary>
        /// Создание резервов по займам, и выполнение резервов
        /// </summary>
        private void CreateLoanWalletsAndTransfers()
        {
            /*_logger.LogDebug("Миграция займов");
            container.Resolve<ISessionProvider>().CloseCurrentSession();

            var loanRepo = container.ResolveRepository<RealityObjectLoan>();
            var loanWalletRepo = container.ResolveRepository<RealityObjectLoanWallet>();
            var paymentRepo = container.ResolveRepository<RealityObjectPaymentAccount>();
            var transferRepo = container.ResolveRepository<Transfer>();

            var existingWallets = loanWalletRepo.GetAll();

            var loansWithoutWallets = loanRepo.GetAll()
                .Where(x => existingWallets.Any(w => w.Loan.Id != x.Id))
                .Where(x => x.Operations.Count == 0)
                .FetchMany(x => x.Operations);

            var roIds =
                loanRepo.GetAll()
                    .Where(x => existingWallets.Any(w => w.Loan.Id != x.Id))
                    .Where(x => x.Operations.Count == 0)
                    .Select(x => new {Ro = x.LoanTaker.RealityObject.Id, Subj = x.LoanGiver.RealityObject.Id})
                    .ToList()
                    .SelectMany(x => new[] {x.Ro, x.Subj})
                    .ToList();

            var paymentAccs = paymentRepo.GetAll()
                .Where(x => roIds.Contains(x.RealityObject.Id))
                .ToList()
                .GroupBy(x => x.RealityObject.Id)
                .ToDictionary(x => x.Key, x => x.First());

            using (var tr = container.Resolve<IDataTransaction>())
            {
                foreach (var loan in loansWithoutWallets)
                {
                    Func<RealityObjectPaymentAccount, Wallet> walletFn;
                    switch (loan.TypeSourceLoan)
                    {
                        case TypeSourceLoan.FundSubsidy:
                            walletFn = acc => acc.FundSubsidyWallet;
                            break;
                        case TypeSourceLoan.TargetSubsidy:
                            walletFn = acc => acc.TargetSubsidyWallet;
                            break;
                        case TypeSourceLoan.PaymentByDesicionTariff:
                            walletFn = acc => acc.DecisionPaymentWallet;
                            break;
                        case TypeSourceLoan.PaymentByMinTariff:
                            walletFn = acc => acc.BaseTariffPaymentWallet;
                            break;
                        case TypeSourceLoan.Penalty:
                            walletFn = acc => acc.PenaltyPaymentWallet;
                            break;
                        case TypeSourceLoan.RegionalSubsidy:
                            walletFn = acc => acc.RegionalSubsidyWallet;
                            break;
                        case TypeSourceLoan.StimulateSubsidy:
                            walletFn = acc => acc.StimulateSubsidyWallet;
                            break;
                        default:
                            throw new NotSupportedException();
                    }

                    var wallet = new RealityObjectLoanWallet(
                        loan,
                        walletFn(paymentAccs[loan.LoanGiver.RealityObject.Id]),
                        walletFn(paymentAccs[loan.LoanTaker.RealityObject.Id]),
                        loan.TypeSourceLoan,
                        loan.LoanSum)
                    {
                        Sum = loan.LoanSum,
                        ReturnedSum = loan.LoanReturnedSum
                    };

                    loanWalletRepo.Save(wallet);

                    var takeTransfer = new Transfer(
                        wallet.SourceWallet.TransferGuid,
                        wallet.Wallet.TransferGuid,
                        wallet.Sum,
                        loan.CreateOperation()) {Reason = "Взятие займа", IsAffect = true, IsLoan = true};

                    transferRepo.Save(takeTransfer);

                    if (wallet.ReturnedSum > 0)
                    {
                        var returnTransfer = new Transfer(
                            wallet.Wallet.TransferGuid,
                            wallet.SourceWallet.TransferGuid,
                            wallet.Sum,
                            loan.CreateOperation()) {Reason = "Возврат займа"};

                        transferRepo.Save(returnTransfer);
                    }
                }

                try
                {
                    tr.Commit();
                    _logger.LogDebug("Миграция займов завершена");
                }
                catch
                {
                    tr.Rollback();
                    throw;
                }
            }*/
        }

        /// <summary>Перенести оплаты по актам</summary>
        private void ApplyRealityObjectActPayments()
        {
            var subQuery = container.Resolve<IDomainService<Transfer>>().GetAll();

            // Неподтвержденная оплата
            var data = container.Resolve<IDomainService<SuspenseAccountPerformedWorkActPayment>>().GetAll()
                .Where(x => subQuery.All(y => y.SourceGuid != x.PerformedWorkActPayment.TransferGuid))
                .Select(x => new
                {
                    x.PerformedWorkActPayment,
                    x.SuspenseAccount.DateReceipt,
                    x.PerformedWorkActPayment.TypeActPayment,
                    x.PerformedWorkActPayment.Paid
                })
                .ToArray();


            foreach (var value in data)
            {
                // TODO:
            }
        }

        /// <summary>Перенести оплаты дома</summary>
        /// <param name="transfers">Трансферы</param>
        private void ApplyRealityObjectPayments(ICollection<Transfer> transfers)
        {
            var subQuery = container.Resolve<IRepository<Transfer>>().GetAll();
            var period = container.Resolve<IChargePeriodRepository>().GetCurrentPeriod();

            var rroPayments = container.Resolve<IDomainService<SuspenseAccountRoPaymentAccOperation>>().GetAll()
                .Where(x => subQuery.All(s => s.SourceGuid != x.SuspenseAccount.TransferGuid))
                .Select(x => new
                {
                    x.SuspenseAccount,
                    x.Operation,
                    x.Operation.Account
                })
                .ToArray();

            foreach (var rroPayment in rroPayments)
            {
                IWallet wallet = null;
                switch (rroPayment.Operation.OperationType)
                {
                    case PaymentOperationType.IncomeFundSubsidy:
                        wallet = rroPayment.Account.FundSubsidyWallet;
                        break;
                    case PaymentOperationType.IncomeStimulateSubsidy:
                        wallet = rroPayment.Account.StimulateSubsidyWallet;
                        break;
                    case PaymentOperationType.IncomeRegionalSubsidy:
                        wallet = rroPayment.Account.RegionalSubsidyWallet;
                        break;
                    case PaymentOperationType.IncomeGrantInAid:
                        wallet = rroPayment.Account.TargetSubsidyWallet;
                        break;
                    case PaymentOperationType.OtherSources:
                        wallet = rroPayment.Account.OtherSourcesWallet;
                        break;
                    case PaymentOperationType.BankPercent:
                        wallet = rroPayment.Account.BankPercentWallet;
                        break;
                    case PaymentOperationType.Income:
                        throw new Exception("PaymentOperationType.Income");
                }

                var operationName = rroPayment.Operation.OperationType.GetEnumMeta().Display;

                var moneyOperation = rroPayment.SuspenseAccount.CreateOperation(period);

                moneyOperation.Reason = operationName;

                var moneyStream = new MoneyStream(
                    rroPayment.SuspenseAccount, 
                    moneyOperation, 
                    rroPayment.Operation.Date,
                    rroPayment.Operation.OperationSum)
                {
                    Description = operationName,
                    OperationDate = rroPayment.Operation.Date
                };
                var transfer = rroPayment.Account.StoreMoney(wallet, moneyStream, false);

                transfer.Reason = operationName;

                transfers.Add(transfer);
            }
        }

        /// <summary>Получить команду</summary>
        /// <param name="paymentType">Тип оплаты</param>
        /// <returns>Команда</returns>
        private IPersonalAccountPaymentCommand GetCommand(PaymentType paymentType)
        {
            IPersonalAccountPaymentCommand command;
            switch (paymentType)
            {
                case PaymentType.Basic:
                    command = commandBasic;
                    break;
                case PaymentType.Penalty:
                    command = commandPenalty;
                    break;
                case PaymentType.SocialSupport:
                    command = commandSocialSupport;
                    break;
                case PaymentType.Rent:
                    command = commandRent;
                    break;
                case PaymentType.PreviousWork:
                    command = commandPreviousWork;
                    break;
                case PaymentType.AccumulatedFund:
                    command = commandAccumulatedFund;
                    break;
                default:
                    throw new NotSupportedException();
            }

            return command;
        }

        /// <summary>Добавить платежи к аккаунту</summary>
        /// <param name="data">Платежи</param>
        /// <param name="transfers">Трансферы</param>
        private void ApplyPaymentsToAccount(IEnumerable<PaymentInfo> data, List<Transfer> transfers)
        {
            foreach (var value in data)
            {
                var command = GetCommand(value.PaymentType);
                transfers.AddRange(ApplyPaymentToAccount(value, command));
            }
        }

        /// <summary>Добавить платеж к аккаунту</summary>
        /// <param name="paymentInfo">Платеж</param>
        /// <param name="command">Команда</param>
        /// <returns>Список трансферов</returns>
        private IEnumerable<Transfer> ApplyPaymentToAccount(
            PaymentInfo paymentInfo,
            IPersonalAccountPaymentCommand command)
        {
            var date = new DateTime(paymentInfo.DateReceipt.Year, paymentInfo.DateReceipt.Month, 1);

            var repo = container.Resolve<IChargePeriodRepository>();

            var firstPeriod = repo.GetFirstPeriod();
            var currentPeriod = repo.GetCurrentPeriod();

            if (date < firstPeriod.StartDate)
            {
                date = firstPeriod.StartDate;
            }

            if (date > currentPeriod.GetEndDate())
            {
                date = currentPeriod.StartDate;
            }

            var operation = paymentInfo.MoneyOperationSource.CreateOperation(this.periods[date]);
            return paymentInfo.Account.ApplyPayment(
                command,
                new MoneyStream(
                    paymentInfo.TransferParty.TransferGuid,
                    operation,
                    paymentInfo.DateReceipt,
                    paymentInfo.Sum),
                mode: ExecutionMode.Bulk);
        }


        /// <summary>
        /// обработать операции изменения сальдо
        /// </summary>
        /// <returns></returns>
        public int ProcessBalanceChange()
        {
            var accountDomain = container.ResolveDomain<BasePersonalAccount>();
            var changesDomain = container.ResolveDomain<PeriodSummaryBalanceChange>();
            var transferDomain = container.ResolveDomain<Transfer>();

            var changes = changesDomain.GetAll()
                .Where(y => !transferDomain.GetAll()
                    .Any(x => x.SourceGuid == y.TransferGuid || x.TargetGuid == y.TransferGuid))
                .ToList();

            var transfersToSave = new List<Transfer>();
            var accountsToUpdate = new HashSet<BasePersonalAccount>();

            foreach (var change in changes)
            {
                var summary = change.PeriodSummary;
                var diff = change.NewValue - change.CurrentValue;
                var moneyOperation = new MoneyOperation(change.TransferGuid, change.PeriodSummary.Period);

                //summary.UpdateBalanceChange(summary.BaseTariffChange + diff);

                var moneyStream = new MoneyStream(change, moneyOperation, change.OperationDate, diff)
                {
                    Description = "Установка/изменение сальдо",
                    OperationDate = change.OperationDate
                };

                accountsToUpdate.Add(summary.PersonalAccount);
            }

            container.InTransaction(() =>
            {
                foreach (var item in accountsToUpdate)
                {
                    accountDomain.Update(item);
                }

                foreach (var item in transfersToSave.Where(x => x.IsNotNull()))
                {
                    transferDomain.Save(item);
                }
            });

            return transfersToSave.Count;
        }

        /// <summary>Внутрений класс, содержащий информацию достаточную для осуществения платежа</summary>
        private class PaymentInfo
        {
            /// <summary>IMoneyOperationSource</summary>
            public IMoneyOperationSource MoneyOperationSource { get; set; }

            /// <summary>Интерфейс участника денежного трансфера</summary>
            public ITransferParty TransferParty { get; set; }

            /// <summary>Аккаунт</summary>
            public BasePersonalAccount Account { get; set; }

            /// <summary>Тип оплаты</summary>
            public PaymentType PaymentType { get; set; }

            /// <summary>Дата платежа</summary>
            public DateTime DateReceipt { get; set; }

            /// <summary>Сумма платежа</summary>
            public decimal Sum { get; set; }
        }

        private class AccountSummary
        {
            public long RoId { get; set; }

            public long AccountId { get; set; }

            public long PeriodId { get; set; }

            public decimal ChargeByBase { get; set; }

            public decimal ChargeByDecision { get; set; }

            public decimal ChargeByPenalty { get; set; }

            public decimal PaidByBase { get; set; }

            public decimal PaidByDecision { get; set; }

            public decimal PaidByPenalty { get; set; }
        }

        private class DummyParty : ITransferParty
        {
            public DummyParty(string guid)
            {
                TransferGuid = guid;
            }

            /// <summary>
            /// Гуид, который запишется либо в SourceGuid, либо в TargetGuid объекта Transfer
            /// </summary>
            public string TransferGuid { get; private set; }
        }

        private class DummySource : IMoneyOperationSource
        {
            private readonly ITransferParty party;

            public DummySource(ITransferParty party)
            {
                this.party = party;
            }

            /// <summary>
            /// Создать операцию по передвижению денег
            /// </summary>
            /// <param name="period">Период операции</param>
            /// <returns><see cref="MoneyOperation"/></returns>
            public MoneyOperation CreateOperation(ChargePeriod period)
            {
                return new MoneyOperation(this.party.TransferGuid, period);
            }
        }

        private class MoneyByDateAndWallets
        {
            public DateTime OperationDate { get; set; }

            public string TargetGuid { get; set; }

            public decimal Amount { get; set; }
        }

        private class RoPeriodInfo
        {
            public decimal ChargeBase { get; set; }
            public decimal ChargeDecision { get; set; }
            public decimal ChargePenalty { get; set; }
            public decimal PaidBase { get; set; }
            public decimal PaidDecision { get; set; }
            public decimal PaidPenalty { get; set; }
            public decimal Saldo { get; set; }
        }

    }

    public static class DateExtensions
    {
        public static bool DateBetween(this DateTime date, DateTime left, DateTime right)
        {
            return date >= left && date <= right;
        }

        /// <summary>
        /// Кастомный ToShortDateString с оберткой в кавычки
        /// </summary>
        /// <param name="dt">DateTime</param>
        /// <param name="quote">кавычка</param>
        /// <returns></returns>
        public static string ToShortDateStringWithQuote(this DateTime dt, string quote = "'")
        {
            return String.Format("{0}{1}{0}", quote, dt.ToShortDateString());
        }
    }

    
}