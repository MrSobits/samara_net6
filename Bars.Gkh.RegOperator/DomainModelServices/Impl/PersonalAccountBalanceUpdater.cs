namespace Bars.Gkh.RegOperator.DomainModelServices.Impl
{
    using System.Collections.Generic;
    using System.Linq;
    using B4.DataAccess;
    using B4.Utils;

    using Bars.Gkh.Entities;
    using Bars.Gkh.RegOperator.Entities.ValueObjects;
    using Bars.Gkh.Repositories.ChargePeriod;

    using Castle.Windsor;
    using Domain;
    using Domain.Repository;
    using Domain.Repository.Wallets;
    using Entities;
    using Entities.ValueObjects;
    using Gkh.Domain;
    using NHibernate;
    using NHibernate.Linq;

    /// <summary>
    /// Сервис обновления баланса ЛС
    /// </summary>
    public class PersonalAccountBalanceUpdater : IPersonalAccountBalanceUpdater
    {
        private readonly IRepository<PersonalAccountPaymentTransfer> accountPaymentTransferRepo;
        private readonly IRepository<RealityObjectTransfer> roTransferRepo;
        private readonly IRepository<RealityObjectPaymentAccount> paymentAccountRepo;
        private readonly IRepository<BasePersonalAccount> accountRepo;
        private readonly IWindsorContainer container;
        private readonly IChargePeriodRepository periodRepo;
        private readonly ISessionProvider sessions;
        private readonly IWalletRepository walletRepo;

        /// <summary>
        /// Конструктор
        /// </summary>
        public PersonalAccountBalanceUpdater(
            IRepository<PersonalAccountPaymentTransfer> accountPaymentTransferRepo,
            IRepository<RealityObjectPaymentAccount> paymentAccountRepo,
            IRepository<RealityObjectTransfer> roTransferRepo,
            IRepository<BasePersonalAccount> accountRepo,
            IWindsorContainer container,
            IChargePeriodRepository periodRepo,
            ISessionProvider sessions,
            IWalletRepository walletRepo)
        {
            this.accountPaymentTransferRepo = accountPaymentTransferRepo;
            this.roTransferRepo = roTransferRepo;
            this.paymentAccountRepo = paymentAccountRepo;
            this.accountRepo = accountRepo;
            this.container = container;
            this.periodRepo = periodRepo;
            this.sessions = sessions;
            this.walletRepo = walletRepo;
        }

        /// <inheritdoc />
        public void UpdateBalance(IQueryable<BasePersonalAccount> accounts)
        {
            if (!accounts.Any())
            {
                return;
            }

            var openedPeriod = this.periodRepo.GetCurrentPeriod(false);

            var finish = accounts.Count();

            var session = this.sessions.OpenStatelessSession();

            var processed = 0;
            var step = 1000;
            var oldFlush = this.sessions.GetCurrentSession().FlushMode;
            this.sessions.GetCurrentSession().FlushMode = FlushMode.Never;
            while (processed < finish)
            {
                var stepItems = accounts.OrderBy(x => x.Id).Skip(processed).Take(step);
                var ids = stepItems.Select(x => x.Id).ToArray();

                var accountDtos = stepItems
                    .Select(x => new AccountWallets
                    {
                        Id = x.Id,
                        RoId = x.Room.RealityObject.Id,
                        BaseGuid = x.BaseTariffWallet.WalletGuid,
                        DecisionGuid = x.DecisionTariffWallet.WalletGuid,
                        PenaltyGuid = x.PenaltyWallet.WalletGuid,
                        AccumFundGuid = x.AccumulatedFundWallet.WalletGuid,
                        PwpGuid = x.PreviosWorkPaymentWallet.WalletGuid,
                        RentGuid = x.RentWallet.WalletGuid,
                        SocialGuid = x.SocialSupportWallet.WalletGuid,
                        RestructAmicableAgreementGuid = x.RestructAmicableAgreementWallet.WalletGuid
                    })
                    .ToList();

                var guids = this.accountRepo.GetAll()
                    .Where(x => ids.Contains(x.Id))
                    .Select(x => new
                    {
                        BaseGuid = x.BaseTariffWallet.WalletGuid,
                        DecisionGuid = x.DecisionTariffWallet.WalletGuid,
                        PenaltyGuid = x.PenaltyWallet.WalletGuid
                    })
                    .ToList()
                    .SelectMany(x => new[] { x.BaseGuid, x.DecisionGuid, x.PenaltyGuid })
                    .ToList();

                // входящие трансферы
                var mainTransfers = this.accountPaymentTransferRepo.GetAll()
                    .Fetch(x => x.Operation)
                    .Where(x => guids.Contains(x.TargetGuid))
                    .Where(x => x.ChargePeriod == openedPeriod)
                    .Select(x => new { x.TargetGuid, x.Amount, x.TargetCoef })
                    .ToList()
                    .GroupBy(x => x.TargetGuid)
                    .ToDictionary(x => x.Key, x => x.Sum(z => z.Amount));

                // исходящие трансферы
                var undoTransfers = this.accountPaymentTransferRepo.GetAll()
                    .Where(x => guids.Contains(x.SourceGuid))
                    .Where(x => x.ChargePeriod == openedPeriod)
                    .Select(x => new { x.SourceGuid, x.Amount })
                    .ToList()
                    .GroupBy(x => x.SourceGuid)
                    .ToDictionary(x => x.Key, x => x.Sum(z => z.Amount));

                this.walletRepo.UpdateWalletBalance(
                    accountDtos.SelectMany(
                        x => new[]
                        {
                            x.BaseGuid,
                            x.DecisionGuid,
                            x.PenaltyGuid,
                            x.AccumFundGuid,
                            x.PwpGuid,
                            x.RentGuid,
                            x.SocialGuid,
                            x.RestructAmicableAgreementGuid
                        }).ToList());

                session.CreateSQLQuery(@"UPDATE regop_pers_acc acc
                        SET tariff_charge_balance = wallet.tariff_balance,
                         decision_charge_balance = wallet.decision_balance,
                         penalty_charge_balance = wallet.penalty_balance
                        FROM (
                                SELECT
                                    A . ID AS account_id,
                                    sum(s.charge_base_tariff - tariff_payment + s.recalc) AS tariff_balance,
                                    sum(s.charge_tariff - s.charge_base_tariff + recalc_decision - tariff_desicion_payment) AS decision_balance,
                                    sum(s.penalty + s.recalc_penalty - s.penalty_payment) AS penalty_balance
                                FROM
                                    regop_pers_acc A
                                JOIN regop_pers_acc_period_summ s ON A .ID = s. account_id
                                WHERE A . ID IN (:ids)
                                GROUP BY A.ID
                            ) AS wallet
                        WHERE acc. ID = wallet.account_id")
                    .SetParameterList("ids", ids)
                    .ExecuteUpdate();

                var tr = session.BeginTransaction();

                using (tr)
                {
                    this.container.InTransaction(() =>
                    {
                        foreach (var account in accountDtos)
                        {
                            var baseTariff = this.GetSummary(
                                account.BaseGuid,
                                mainTransfers,
                                undoTransfers);

                            var decisionTariff = this.GetSummary(
                                account.DecisionGuid,
                                mainTransfers,
                                undoTransfers);

                            var penalty = this.GetSummary(
                                account.PenaltyGuid,
                                mainTransfers,
                                undoTransfers);

                            session.CreateQuery(@"update PersonalAccountPeriodSummary s set 
                                TariffPayment=:tariff,
                                PenaltyPayment=:penalty,
                                TariffDecisionPayment=:decision
                                 where s.PersonalAccount in (from BasePersonalAccount where Id=:accountId)
                                 and s.Period in (from ChargePeriod where IsClosed=:isClosed)")

                                .SetInt64("accountId", account.Id)
                                .SetBoolean("isClosed", false)

                                .SetDecimal("tariff", baseTariff)
                                .SetDecimal("decision", decisionTariff)
                                .SetDecimal("penalty", penalty)

                                .ExecuteUpdate();
                        }

                        session.CreateSQLQuery(@"update regop_pers_acc_period_summ set 
                             SALDO_OUT=SALDO_IN + CHARGE_TARIFF + PENALTY 
                                + RECALC + RECALC_DECISION + RECALC_PENALTY - PERF_WORK_CHARGE
                                + BALANCE_CHANGE + DEC_BALANCE_CHANGE + PENALTY_BALANCE_CHANGE
                                - (TARIFF_PAYMENT + TARIFF_DESICION_PAYMENT + PENALTY_PAYMENT)
                             where ACCOUNT_ID in (:accounts) and PERIOD_ID=:periodId")
                            .SetParameterList("accounts", ids)
                            .SetInt64("periodId", openedPeriod.Id)
                            .ExecuteUpdate();

                        this.CopyTransfers(accountDtos);

                        this.sessions.GetCurrentSession().FlushMode = oldFlush;
                    });

                    try
                    {
                        tr.Commit();
                        processed += step;
                    }
                    catch
                    {
                        tr.Rollback();

                        throw;
                    }
                }
            }
        }

        private decimal GetSummary(
            string guid,
            IDictionary<string, decimal> mainTransfers,
            IDictionary<string, decimal> undoTransfers)
        {
            var mainTransferSum = mainTransfers.Get(guid);
            var undoTransferSum = undoTransfers.Get(guid);

            return mainTransferSum - undoTransferSum;
        }

        private void CopyTransfers(IList<AccountWallets> accountDtos)
        {
            var accounts = accountDtos;
            var roIds = accounts.Select(x => x.RoId).Distinct().ToList();

            var period = this.periodRepo.GetCurrentPeriod();

            var targetWallets = this.paymentAccountRepo.GetAll()
                .Where(x => roIds.Contains(x.RealityObject.Id))
                .Select(x => new
                {
                    x.Id,
                    RoId = x.RealityObject.Id,
                    BaseGuid = x.BaseTariffPaymentWallet.WalletGuid,
                    DecisionGuid = x.DecisionPaymentWallet.WalletGuid,
                    PenaltyGuid = x.PenaltyPaymentWallet.WalletGuid,
                    AccumFundGuid = x.AccumulatedFundWallet.WalletGuid,
                    PwpGuid = x.PreviosWorkPaymentWallet.WalletGuid,
                    RentGuid = x.RentWallet.WalletGuid,
                    SocialGuid = x.SocialSupportWallet.WalletGuid,
                    RestructAmicableAgreementGuid = x.RestructAmicableAgreementWallet.WalletGuid,
                })
                .ToList()
                .ToDictionary(x => x.RoId);

            var sourceGuids =
                accounts
                    .SelectMany(x => new[]
                    {
                        x.AccumFundGuid,
                        x.BaseGuid,
                        x.DecisionGuid,
                        x.PenaltyGuid,
                        x.PwpGuid,
                        x.RentGuid,
                        x.SocialGuid,
                        x.RestructAmicableAgreementGuid,
                    })
                    .ToList();

            var sourceTransfers = new List<Transfer>();
            var refundTransfers = new List<Transfer>();
            int counter = 0, step = 1000;
            while (counter < sourceGuids.Count)
            {
                var stepItems = sourceGuids.Skip(counter).Take(step).ToList();

                // получаем все оплаты по дому
                var roTransfers = roTransferRepo.GetAll().Where(t => t.ChargePeriod == period)
                     .Where(x => roIds.Contains(x.Owner.RealityObject.Id))
                       .Where(x => stepItems.Contains(x.SourceGuid))
                      .Select(x => new
                      {
                          x.Id,
                          x.Amount,
                          x.TargetCoef,
                          x.TargetGuid,
                          x.SourceGuid,
                          x.PaymentDate,
                          x.OperationDate,
                          Operation = x.Operation.Id
                      });//.ToList();
                //получаем оплаты которые уже на доме

                var accPayments = accountPaymentTransferRepo.GetAll()
                    .Where(x => stepItems.Contains(x.TargetGuid))
                    .Where(x => x.ChargePeriod == period)
                    .Where(x => !x.IsInDirect && x.IsAffect)
                     .Where(x => roTransfers.Any(y => y.Operation == x.Operation.Id && y.SourceGuid == x.TargetGuid && x.Amount == y.Amount && x.OperationDate == y.OperationDate
                      && x.PaymentDate == y.PaymentDate))
                    .Select(x => x.Id).ToList();


                // приходные трансферы
                sourceTransfers.AddRange(this.accountPaymentTransferRepo.GetAll()
                    .Fetch(x => x.Operation)
                    .Where(x => stepItems.Contains(x.TargetGuid))
                    .Where(x => !x.IsInDirect && x.IsAffect) // копируем только трансферы, влияющие на баланс (бабловые трансферы) и не копии на дом
                    .Where(x => x.ChargePeriod == period)
                    //  .Where(x => this.roTransferRepo.GetAll().Where(t => t.ChargePeriod == period)
                    .Where(x => !accPayments.Contains(x.Id))
                    .Where(x => x.Reason != null));
                // 
                // отсекаем перенос долга при слиянии

                //// приходные трансферы
                //sourceTransfers.AddRange(this.accountPaymentTransferRepo.GetAll()
                //    .Fetch(x => x.Operation)
                //    .Where(x => stepItems.Contains(x.TargetGuid))
                //    .Where(x => !x.IsInDirect && x.IsAffect) // копируем только трансферы, влияющие на баланс (бабловые трансферы) и не копии на дом
                //    .Where(x => x.ChargePeriod == period)
                //    .Where(x => this.roTransferRepo.GetAll().Where(t => t.ChargePeriod == period).All(t => t.CopyTransfer != x))
                //    .Where(x => x.Reason != null)); // отсекаем перенос долга при слиянии

                // расходные трансферы
                refundTransfers.AddRange(this.accountPaymentTransferRepo.GetAll()
                    .Fetch(x => x.Operation)
                    .Where(x => stepItems.Contains(x.SourceGuid))
                    .Where(x => !x.IsInDirect && x.IsAffect) // копируем только трансферы, влияющие на баланс (бабловые трансферы) и не копии на дом
                    .Where(x => x.ChargePeriod == period)
                    .Where(x => this.roTransferRepo.GetAll().Where(t => t.ChargePeriod == period).All(t => t.CopyTransfer != x))
                    .Where(x => x.Reason != null)); // отсекаем перенос долга при слиянии

                counter += step;
            }

            var groupByRo = accounts
                .GroupBy(x => x.RoId)
                .ToDictionary(x => x.Key);

            using (var tr = this.container.Resolve<IDataTransaction>()) 
            { 
                foreach (var targetWallet in targetWallets)
                {
                    if (!groupByRo.ContainsKey(targetWallet.Key))
                    {
                        continue;
                    }

                    var wallet = targetWallet;
                    this.CopyTargetTransfers(targetWallet.Value.Id, targetWallet.Value.AccumFundGuid, sourceTransfers.Where(x => groupByRo[wallet.Key].Select(g => g.AccumFundGuid).Contains(x.TargetGuid)));
                    this.CopyTargetTransfers(targetWallet.Value.Id, targetWallet.Value.BaseGuid, sourceTransfers.Where(x => groupByRo[wallet.Key].Select(g => g.BaseGuid).Contains(x.TargetGuid)));
                    this.CopyTargetTransfers(targetWallet.Value.Id, targetWallet.Value.DecisionGuid, sourceTransfers.Where(x => groupByRo[wallet.Key].Select(g => g.DecisionGuid).Contains(x.TargetGuid)));
                    this.CopyTargetTransfers(targetWallet.Value.Id, targetWallet.Value.PenaltyGuid, sourceTransfers.Where(x => groupByRo[wallet.Key].Select(g => g.PenaltyGuid).Contains(x.TargetGuid)));
                    this.CopyTargetTransfers(targetWallet.Value.Id, targetWallet.Value.PwpGuid, sourceTransfers.Where(x => groupByRo[wallet.Key].Select(g => g.PwpGuid).Contains(x.TargetGuid)));
                    this.CopyTargetTransfers(targetWallet.Value.Id, targetWallet.Value.RentGuid, sourceTransfers.Where(x => groupByRo[wallet.Key].Select(g => g.RentGuid).Contains(x.TargetGuid)));
                    this.CopyTargetTransfers(targetWallet.Value.Id, targetWallet.Value.SocialGuid, sourceTransfers.Where(x => groupByRo[wallet.Key].Select(g => g.SocialGuid).Contains(x.TargetGuid)));
                    this.CopyTargetTransfers(targetWallet.Value.Id, targetWallet.Value.RestructAmicableAgreementGuid, sourceTransfers.Where(x => groupByRo[wallet.Key].Select(g => g.RestructAmicableAgreementGuid).Contains(x.TargetGuid)));

                    this.CopySourceTransfers(targetWallet.Value.Id, targetWallet.Value.AccumFundGuid, refundTransfers.Where(x => groupByRo[wallet.Key].Select(g => g.AccumFundGuid).Contains(x.SourceGuid)));
                    this.CopySourceTransfers(targetWallet.Value.Id, targetWallet.Value.BaseGuid, refundTransfers.Where(x => groupByRo[wallet.Key].Select(g => g.BaseGuid).Contains(x.TargetGuid)));
                    this.CopySourceTransfers(targetWallet.Value.Id, targetWallet.Value.DecisionGuid, refundTransfers.Where(x => groupByRo[wallet.Key].Select(g => g.DecisionGuid).Contains(x.SourceGuid)));
                    this.CopySourceTransfers(targetWallet.Value.Id, targetWallet.Value.PenaltyGuid, refundTransfers.Where(x => groupByRo[wallet.Key].Select(g => g.PenaltyGuid).Contains(x.SourceGuid)));
                    this.CopySourceTransfers(targetWallet.Value.Id, targetWallet.Value.PwpGuid, refundTransfers.Where(x => groupByRo[wallet.Key].Select(g => g.PwpGuid).Contains(x.SourceGuid)));
                    this.CopySourceTransfers(targetWallet.Value.Id, targetWallet.Value.RentGuid, refundTransfers.Where(x => groupByRo[wallet.Key].Select(g => g.RentGuid).Contains(x.SourceGuid)));
                    this.CopySourceTransfers(targetWallet.Value.Id, targetWallet.Value.SocialGuid, refundTransfers.Where(x => groupByRo[wallet.Key].Select(g => g.SocialGuid).Contains(x.SourceGuid)));
                    this.CopySourceTransfers(targetWallet.Value.Id, targetWallet.Value.RestructAmicableAgreementGuid, refundTransfers.Where(x => groupByRo[wallet.Key].Select(g => g.RestructAmicableAgreementGuid).Contains(x.SourceGuid)));
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
        /// Копирование приходных трансферов на дом
        /// </summary>
        private void CopyTargetTransfers(long ropaId, string targetGuid, IEnumerable<Transfer> transfers)
        {
            var realityObjectPaymentAccount = new RealityObjectPaymentAccount { Id = ropaId };
            foreach (var transfer in transfers)
            {
                this.roTransferRepo.Save(new RealityObjectTransfer(realityObjectPaymentAccount, transfer.TargetGuid, targetGuid, transfer.Amount, transfer.Operation)
                {
                    OperationDate = transfer.OperationDate,
                    PaymentDate = transfer.PaymentDate,
                    Reason = transfer.Reason,
                    CopyTransfer = transfer.To<PersonalAccountPaymentTransfer>(),
                    IsAffect = transfer.IsAffect,
                    OriginatorName = transfer.OriginatorName
                });
            }
        }

        /// <summary>
        /// Копирование расходных трансферов на дом
        /// </summary>
        private void CopySourceTransfers(long ropaId, string sourceGuid, IEnumerable<Transfer> transfers)
        {
            var realityObjectPaymentAccount = new RealityObjectPaymentAccount { Id = ropaId };
            foreach (var transfer in transfers)
            {
                this.roTransferRepo.Save(new RealityObjectTransfer(realityObjectPaymentAccount, sourceGuid, transfer.TargetGuid, transfer.Amount, transfer.Operation)
                    {
                        OperationDate = transfer.OperationDate,
                        PaymentDate = transfer.PaymentDate,
                        Reason = transfer.Reason,
                        CopyTransfer = transfer.To<PersonalAccountPaymentTransfer>(),
                        IsAffect = transfer.IsAffect,
                        OriginatorName = transfer.OriginatorName
                    });
            }
        }

        private class AccountWallets
        {
            public long Id { get; set; }
            public long RoId { get; set; }
            public string BaseGuid { get; set; }
            public string DecisionGuid { get; set; }
            public string PenaltyGuid { get; set; }
            public string AccumFundGuid { get; set; }
            public string PwpGuid { get; set; }
            public string RentGuid { get; set; }
            public string SocialGuid { get; set; }
            public string RestructAmicableAgreementGuid { get; set; }
        }
    }
}
