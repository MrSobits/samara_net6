namespace Bars.Gkh.RegOperator.ExecutionAction
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Domain.CollectionExtensions;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.ExecutionAction;
    using Bars.Gkh.RegOperator.Domain;
    using Bars.Gkh.RegOperator.Domain.Repository.Transfers;
    using Bars.Gkh.RegOperator.Domain.ValueObjects;
    using Bars.Gkh.RegOperator.DomainModelServices;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Entities.ValueObjects;
    using Bars.Gkh.RegOperator.Entities.Wallet;
    using Bars.Gkh.RegOperator.Enums;
    using Bars.Gkh.Repositories.ChargePeriod;

    using Castle.Windsor;

    /// <summary>
    /// Действие выявляет и исправляет неверно распределенные реестры оплат
    /// </summary>
    public class FixInvalidBankDocumentPayments : BaseExecutionAction
    {
        private readonly HashSet<RealityObjectPaymentAccount> realityObjectPaymentAccounts = new HashSet<RealityObjectPaymentAccount>();
        private ChargePeriod startRecalc;
        private readonly HashSet<long> persAccs = new HashSet<long>();

        /// <summary>
        /// Домен-сервис <see cref="BankDocumentImport" />
        /// </summary>
        public IDomainService<BankDocumentImport> BankDocumentImportDomain { get; set; }

        /// <summary>
        /// Домен-сервис <see cref="ImportedPayment" />
        /// </summary>
        public IDomainService<ImportedPayment> ImportedPaymentDomain { get; set; }

        /// <summary>
        /// Домен-сервис <see cref="RealityObjectPaymentAccount" />
        /// </summary>
        public IDomainService<RealityObjectPaymentAccount> RealityObjectPaymentAccountDomain { get; set; }

        /// <summary>
        /// Домен-сервис <see cref="MoneyOperation" />
        /// </summary>
        public IDomainService<MoneyOperation> MoneyOperationDomain { get; set; }

        /// <summary>
        /// Домен-сервис <see cref="Transfer" />
        /// </summary>
        public IDomainService<Transfer> TransferDomain { get; set; }

        /// <summary>
        /// Домен-сервис <see cref="PersonalAccountPaymentTransfer" />
        /// </summary>
        public IDomainService<PersonalAccountPaymentTransfer> PersonalAccountPaymentTransferDomain { get; set; }

        /// <summary>
        /// Домен-сервис <see cref="RealityObjectTransfer" />
        /// </summary>
        public IDomainService<RealityObjectTransfer> RealityObjectTransferDomain { get; set; }

        /// <summary>
        /// Репозиторий <see cref="Transfer" />
        /// </summary>
        public ITransferRepository<RealityObjectTransfer> TransferRepository { get; set; }

        /// <summary>
        /// Репозиторий периодов начисления
        /// </summary>
        public IChargePeriodRepository ChargePeriodRepository { get; set; }

        /// <summary>
        /// Фабрика для создания команды
        /// </summary>
        public IPersonalAccountPaymentCommandFactory CommandFactory { get; set; }

        /// <summary>
        /// Фабрика для создания команды возвратов
        /// </summary>
        public IPersonalAccountRefundCommandFactory RefundCommandFactory { get; set; }

        /// <summary>
        /// Сессия оплат дома
        /// </summary>
        public IRealtyObjectPaymentSession RealtyObjectPaymentSession { get; set; }

        /// <summary>
        /// Провайдер сессии
        /// </summary>
        public ISessionProvider SessionProvider { get; set; }

        private BaseDataResult Execute()
        {
            this.startRecalc = this.ChargePeriodRepository.GetCurrentPeriod();

            const string Query = @"SELECT d.id as Id FROM regop_bank_doc_import d
              join regop_money_operation op on op.originator_guid = d.transfer_guid and not is_cancelled and op.canceled_op_id is null
              left join (SELECT sum(amount) sum, tr.op_id op_id from regop_transfer tr where GROUP BY tr.op_id) tr on tr.op_id = op.id
            where d.imported_sum != tr.sum";

            var ids = this.SessionProvider.GetCurrentSession().CreateSQLQuery(Query).List<object>().Select(x => long.Parse(x.ToString()));

            var bankDocumentQuery =
                this.BankDocumentImportDomain.GetAll()
                    .Where(x => x.PaymentConfirmationState == PaymentConfirmationState.Distributed)
                    .Where(x => ids.Contains(x.Id));

            var paymentsDict =
                this.ImportedPaymentDomain.GetAll()
                    .Where(
                        x => bankDocumentQuery.Any(y => y.Id == x.BankDocumentImport.Id) &&
                            x.PaymentConfirmationState == ImportedPaymentPaymentConfirmState.Distributed)
                    .AsEnumerable()
                    .GroupBy(x => x.BankDocumentImport.Id)
                    .ToDictionary(x => x.Key, x => x.ToArray());

            var listToDelete = new List<Transfer>();
            var listToSave = new List<Transfer>();

            foreach (var document in bankDocumentQuery)
            {
                var transfersDoc = this.PersonalAccountPaymentTransferDomain.GetAll()
                    .Where(x => x.Operation.OriginatorGuid == document.TransferGuid)
                    .Where(x => !x.Operation.IsCancelled && x.Operation.CanceledOperation == null)
                    .AsEnumerable()
                    .Cast<Transfer>()
                    .ToList();

                var copies = this.RealityObjectTransferDomain.GetAll()
                    .Where(x => x.Operation.OriginatorGuid == document.TransferGuid)
                    .Where(x => !x.Operation.IsCancelled && x.Operation.CanceledOperation == null && x.CopyTransfer != null)
                    .ToList();

                transfersDoc.AddRange(copies);

                var operations = transfersDoc.Select(x => x.Operation).Distinct().ToList();

                if (transfersDoc.Except(copies).Sum(x => x.Amount) == document.ImportedSum)
                {
                    continue;
                }

                var payments = paymentsDict[document.Id];
                var transfersToPersAcc = transfersDoc.Except(copies);
                var persAccWallets = payments
                    .Select(x => x.PersonalAccount)
                    .Distinct()
                    .Select(
                        x => new PersonalAccountWalletProxy
                        {
                            Id = x.Id,
                            AccountNumber = x.PersonalAccountNum,
                            RoId = x.Room.RealityObject.Id,
                            WalletGuids = new[]
                            {
                                new Tuple<WalletType, string>(WalletType.BaseTariffWallet, x.BaseTariffWallet.WalletGuid),
                                new Tuple<WalletType, string>(WalletType.DecisionTariffWallet, x.DecisionTariffWallet.WalletGuid),
                                new Tuple<WalletType, string>(WalletType.PenaltyWallet, x.PenaltyWallet.WalletGuid)
                            }
                        })
                    .ToList();

                foreach (var persAccWallet in persAccWallets)
                {
                    var paymentsByAcc = payments.Where(x => x.PersonalAccount.Id == persAccWallet.Id).GroupBy(x => x.PaymentDate).ToArray();

                    // трансферы по датам по текущему ЛС
                    var transfers =
                        transfersToPersAcc.Where(
                            x => persAccWallet.GetWalletGuids().Contains(x.TargetGuid) || persAccWallet.GetWalletGuids().Contains(x.SourceGuid))
                            .GroupBy(x => x.PaymentDate)
                            .ToDictionary(x => x.Key, x => x.ToArray());

                    // разбираем оплаты/возвраты ЛС
                    foreach (var importedPayments in paymentsByAcc)
                    {
                        var paymentSum = importedPayments.Sum(x => x.Sum);
                        if (paymentSum == (transfers.Get(importedPayments.Key)?.Sum(x => x.Amount) ?? 0M))
                        {
                            // всё ок, всё оплаты по дате присутствуют, проверим копии трансферов
                            var copyTransfers =
                                copies.Where(x => transfers[importedPayments.Key].Select(y => y.Id).Contains(x.Originator.Id))
                                    .ToList();

                            if (copyTransfers.Sum(x => x.Amount) == paymentSum)
                            {
                                continue;
                            }

                            // обработать копии на дом
                            var originTransfers = transfers[importedPayments.Key].ToDictionary(x => x.Id);
                            var transfersToDelete = copyTransfers.Where(x => originTransfers[x.Originator.Id].Amount != x.Amount).ToArray();
                            listToDelete.AddRange(transfersToDelete);

                            // трансферы для обработки: те, которых нет вообще и те, которые удаляем
                            var originalTransfersToProcess =
                                originTransfers.Values.Except(copyTransfers.Select(x => x.Originator)).Union(transfersToDelete.Select(x => x.Originator));

                            listToSave.AddRange(this.ProcessCopyForSource(persAccWallet, originalTransfersToProcess));
                        }
                        else
                        {
                            // обработать создание всех трансферов в этот день, удалив все имеющиеся, удаляем все копии на дом тоже
                            var moneyOperation = transfers.Get(importedPayments.Key)?.First().Operation ?? operations.First();
                            var chargePeriod = this.ChargePeriodRepository.GetPeriodByDate(moneyOperation.ObjectCreateDate.Date);

                            if (this.startRecalc.Id > chargePeriod.Id)
                            {
                                this.startRecalc = chargePeriod;
                            }

                            if (transfers.ContainsKey(importedPayments.Key))
                            {
                                listToDelete.AddRange(transfers[importedPayments.Key]);
                                listToDelete.AddRange(
                                    copies.Where(
                                        x => transfers[importedPayments.Key].Select(y => y.Id).Contains(x.Originator.Id)));
                            }

                            var amountDistributionType = document.DistributePenalty == YesNo.Yes
                                ? AmountDistributionType.TariffAndPenalty
                                : AmountDistributionType.Tariff;

                            foreach (var importedPayment in importedPayments)
                            {
                                listToSave.AddRange(
                                    this.ApplyPayment(
                                        document.TransferGuid,
                                        moneyOperation,
                                        importedPayment,
                                        chargePeriod,
                                        amountDistributionType,
                                        document.ObjectEditDate));
                            }

                            // список ЛС к перерасчету
                            this.persAccs.Add(persAccWallet.Id);
                        }
                    }
                }
            }

            this.Container.InTransaction(
                () =>
                {
                    try
                    {
                        var session = this.SessionProvider.GetCurrentSession();
                        listToDelete.ForEach(x => session.Delete(x.Id));
                        listToSave.ForEach(x => this.TransferDomain.SaveOrUpdate(x));

                        this.RealtyObjectPaymentSession.Complete();
                    }
                    catch
                    {
                        this.RealtyObjectPaymentSession.Rollback();
                    }
                });

            this.RecalcPaymentsAndSaldo();

            return new BaseDataResult();
        }

        private IEnumerable<Transfer> ProcessCopyForSource(PersonalAccountWalletProxy account, IEnumerable<Transfer> originalTransfers)
        {
            var result = new List<Transfer>();
            var realityObjectPaymentAccount = this.GetRealityObjectPaymentAccount(account);

            foreach (var transfer in originalTransfers)
            {
                var period = this.ChargePeriodRepository.GetPeriodByDate(transfer.Operation.ObjectCreateDate);
                if (this.startRecalc.Id > period.Id)
                {
                    this.startRecalc = period;
                }

                // если оплата
                var walletType = this.GetRealityObjectWalletTypeTransfer(account, transfer);
                var walletSelector = this.GetWalletSelectr(walletType.Item1);

                var additionalInfo = walletType.Item2 ? string.Empty : " " + walletType.Item1.GetDisplayName().ToLower();
                var moneyStream = new MoneyStream(transfer)
                {
                    Description = transfer.Reason + additionalInfo,
                    OriginalTransfer = transfer,
                    OriginatorName = account.AccountNumber,
                    OperationDate = transfer.OperationDate
                };

                // если true - то это входящий трансфер, сорян за такое решение
                if (walletType.Item2)
                {
                    var transferToSave = realityObjectPaymentAccount.StoreMoney(walletSelector(realityObjectPaymentAccount), moneyStream);
                    transferToSave?.AddTo(result);
                }
                else
                {
                    var transferToSave = realityObjectPaymentAccount.TakeMoney(walletSelector(realityObjectPaymentAccount), moneyStream);
                    transferToSave?.AddTo(result);
                }
            }

            return result;
        }

        private RealityObjectPaymentAccount GetRealityObjectPaymentAccount(PersonalAccountWalletProxy account)
        {
            var realityObjectPaymentAccount = this.realityObjectPaymentAccounts.FirstOrDefault(x => x.RealityObject.Id == account.RoId);

            if (realityObjectPaymentAccount != null)
            {
                return realityObjectPaymentAccount;
            }

            realityObjectPaymentAccount = this.RealityObjectPaymentAccountDomain.GetAll().First(x => x.RealityObject.Id == account.RoId);
            this.realityObjectPaymentAccounts.Add(realityObjectPaymentAccount);
            return realityObjectPaymentAccount;
        }

        private Tuple<WalletType, bool> GetRealityObjectWalletTypeTransfer(PersonalAccountWalletProxy account, Transfer transfer)
        {
            var type = account.WalletGuids.FirstOrDefault(x => x.Item2 == transfer.TargetGuid);
            if (type != null)
            {
                return new Tuple<WalletType, bool>(type.Item1, true);
            }

            type = account.WalletGuids.First(x => x.Item2 == transfer.SourceGuid);
            return new Tuple<WalletType, bool>(type.Item1, false);
        }

        private Func<RealityObjectPaymentAccount, Wallet> GetWalletSelectr(WalletType walletType)
        {
            switch (walletType)
            {
                case WalletType.BaseTariffWallet:
                    return x => x.BaseTariffPaymentWallet;
                case WalletType.DecisionTariffWallet:
                    return x => x.DecisionPaymentWallet;
                case WalletType.PenaltyWallet:
                    return x => x.PenaltyPaymentWallet;
                default:
                    throw new ArgumentOutOfRangeException(nameof(walletType), walletType, null);
            }
        }

        private void RecalcPaymentsAndSaldo()
        {
            const string calcPayments = @"UPDATE regop_pers_acc_period_summ ps
SET tariff_payment=
  (SELECT coalesce(sum(tt.amount), 0)
   FROM
     (SELECT t.amount
      FROM regop_transfer t
      JOIN regop_wallet w ON w.wallet_guid = t.target_guid
      JOIN regop_pers_acc pa ON pa.bt_wallet_id = w.id
      AND pa.id = ps.account_id
      WHERE t.reason = 'Оплата по базовому тарифу'
        AND p.cstart <= cast(t.operation_date AS date)
        AND (p.cend IS NULL
             OR cast(t.operation_date AS date) <= p.cend)
      UNION ALL SELECT t.amount
      FROM regop_transfer t
      JOIN regop_wallet w ON w.wallet_guid = t.target_guid
      JOIN regop_pers_acc pa ON pa.bt_wallet_id = w.id
      AND pa.id = ps.account_id
      WHERE t.reason = 'Поступление денег соц. поддержки'
        AND p.cstart <= cast(t.operation_date AS date)
        AND (p.cend IS NULL
             OR cast(t.operation_date AS date) <= p.cend)
      UNION ALL SELECT t.amount
      FROM regop_transfer t
      JOIN regop_wallet w ON w.wallet_guid = t.target_guid
      JOIN regop_pers_acc pa ON pa.bt_wallet_id = w.id
      AND pa.id = ps.account_id
      WHERE t.reason = 'Поступленее ранее накопленных средств'
        AND p.cstart <= cast(t.operation_date AS date)
        AND (p.cend IS NULL
             OR cast(t.operation_date AS date) <= p.cend)
      UNION ALL SELECT (-1)*t.amount AS amount
      FROM regop_transfer t
      JOIN regop_wallet w ON w.wallet_guid = t.source_guid
      JOIN regop_pers_acc pa ON pa.bt_wallet_id = w.id
      AND pa.id = ps.account_id
      WHERE t.reason = 'Возврат взносов на КР'
        AND p.cstart <= cast(t.operation_date AS date)
        AND (p.cend IS NULL
             OR cast(t.operation_date AS date) <= p.cend)
      UNION ALL SELECT (-1)*t.amount AS amount
      FROM regop_transfer t
      JOIN regop_wallet w ON w.wallet_guid = t.source_guid
      JOIN regop_pers_acc pa ON pa.bt_wallet_id = w.id
      AND pa.id = ps.account_id
      WHERE t.reason = 'Отмена оплаты по базовому тарифу'
        AND p.cstart <= cast(t.operation_date AS date)
        AND (p.cend IS NULL
             OR cast(t.operation_date AS date) <= p.cend)
      UNION ALL SELECT t.amount AS amount
      FROM regop_transfer t
      JOIN regop_wallet w ON w.wallet_guid = t.target_guid
      JOIN regop_pers_acc pa ON pa.bt_wallet_id = w.id
      AND pa.id = ps.account_id
      WHERE t.reason = 'Корректировка оплат по базовому тарифу'
        AND p.cstart <= cast(t.operation_date AS date)
        AND (p.cend IS NULL
             OR cast(t.operation_date AS date) <= p.cend)
      UNION ALL SELECT (-1)*t.amount AS amount
      FROM regop_transfer t
      JOIN regop_wallet w ON w.wallet_guid = t.source_guid
      JOIN regop_pers_acc pa ON pa.bt_wallet_id = w.id
      AND pa.id = ps.account_id
      WHERE t.reason = 'Возврат средств'
        AND p.cstart <= cast(t.operation_date AS date)
        AND (p.cend IS NULL
             OR cast(t.operation_date AS date) <= p.cend)
      UNION ALL SELECT (-1)*t.amount AS amount
      FROM regop_transfer t
      JOIN regop_wallet w ON w.wallet_guid = t.source_guid
      JOIN regop_pers_acc pa ON pa.bt_wallet_id = w.id
      AND pa.id = ps.account_id
      WHERE t.reason = 'Отмена поступления по соц. поддержке'
        AND p.cstart <= cast(t.operation_date AS date)
        AND (p.cend IS NULL
             OR cast(t.operation_date AS date) <= p.cend)
      UNION ALL SELECT t.amount
      FROM regop_transfer t
      JOIN regop_wallet w ON w.wallet_guid = t.source_guid
      JOIN regop_pers_acc pa ON pa.bt_wallet_id = w.id
      AND pa.id = ps.account_id
      WHERE t.reason = 'Зачисление по базовому тарифу в счет отмены возврата средств'
        AND p.cstart <= cast(t.operation_date AS date)
        AND (p.cend IS NULL
             OR cast(t.operation_date AS date) <= p.cend)
      UNION ALL SELECT -t.target_coef*t.amount
      FROM regop_transfer t
      JOIN regop_wallet w ON w.wallet_guid = t.source_guid
      JOIN regop_pers_acc pa ON pa.bt_wallet_id = w.id  --по базовому тарифу
      AND pa.id = ps.account_id
	  JOIN regop_money_operation mo on mo.id = t.op_id
      WHERE mo.reason = 'Перенос долга при слиянии'
        AND t.amount < 0
        AND p.cstart <= cast(t.operation_date AS date)
        AND (p.cend IS NULL OR cast(t.operation_date AS date) <= p.cend) 
      UNION ALL SELECT t.target_coef*t.amount
      FROM regop_transfer t
      JOIN regop_wallet w ON w.wallet_guid = t.target_guid
      JOIN regop_pers_acc pa ON pa.bt_wallet_id = w.id  --по базовому тарифу
      AND pa.id = ps.account_id
	  JOIN regop_money_operation mo on mo.id = t.op_id
      WHERE mo.reason = 'Перенос долга при слиянии'
	    AND t.amount < 0
	    AND p.cstart <= cast(t.operation_date AS date)
	    AND (p.cend IS NULL OR cast(t.operation_date AS date) <= p.cend)) tt),
    TARIFF_DESICION_PAYMENT =
  (SELECT coalesce(sum(tt.amount), 0)
   FROM
     (SELECT t.amount
      FROM regop_transfer t
      JOIN regop_wallet w ON w.wallet_guid = t.target_guid
      JOIN regop_pers_acc pa ON pa.dt_wallet_id = w.id
      AND pa.id = ps.account_id
      WHERE t.reason = 'Оплата по тарифу решения'
        AND p.cstart <= cast(t.operation_date AS date)
        AND (p.cend IS NULL
             OR cast(t.operation_date AS date) <= p.cend)
      UNION ALL SELECT (-1)*t.amount AS amount
      FROM regop_transfer t
      JOIN regop_wallet w ON w.wallet_guid = t.source_guid
      JOIN regop_pers_acc pa ON pa.dt_wallet_id = w.id
      AND pa.id = ps.account_id
      WHERE t.reason = 'Отмена оплаты по тарифу решения'
        AND p.cstart <= cast(t.operation_date AS date)
        AND (p.cend IS NULL
             OR cast(t.operation_date AS date) <= p.cend)
      UNION ALL SELECT t.target_coef*t.amount
      FROM regop_transfer t
      JOIN regop_wallet w ON w.wallet_guid = t.target_guid
      JOIN regop_pers_acc pa ON pa.dt_wallet_id = w.id  --по решению
      AND pa.id = ps.account_id
	  JOIN regop_money_operation mo on mo.id = t.op_id
      WHERE mo.reason = 'Перенос долга при слиянии'
        AND t.amount < 0
        AND p.cstart <= cast(t.operation_date AS date)
        AND (p.cend IS NULL OR cast(t.operation_date AS date) <= p.cend)
      UNION ALL SELECT -t.target_coef*t.amount
      FROM regop_transfer t
      JOIN regop_wallet w ON w.wallet_guid = t.source_guid
      JOIN regop_pers_acc pa ON pa.dt_wallet_id = w.id  --по решению
      AND pa.id = ps.account_id
	  JOIN regop_money_operation mo on mo.id = t.op_id
      WHERE mo.reason = 'Перенос долга при слиянии'
	    AND t.amount < 0
	    AND p.cstart <= cast(t.operation_date AS date)
	    AND (p.cend IS NULL OR cast(t.operation_date AS date) <= p.cend)
      UNION ALL SELECT t.amount AS amount
      FROM regop_transfer t
      JOIN regop_wallet w ON w.wallet_guid = t.target_guid
      JOIN regop_pers_acc pa ON pa.dt_wallet_id = w.id
      AND pa.id = ps.account_id
      WHERE t.reason = 'Корректировка оплат по тарифу решения'
        AND p.cstart <= cast(t.operation_date AS date)
        AND (p.cend IS NULL
             OR cast(t.operation_date AS date) <= p.cend)
      UNION ALL SELECT t.amount
      FROM regop_transfer t
      JOIN regop_wallet w ON w.wallet_guid = t.source_guid
      JOIN regop_pers_acc pa ON pa.dt_wallet_id = w.id
      AND pa.id = ps.account_id
      WHERE t.reason = 'Зачисление по тарифу решения в счет отмены возврата средств'
        AND p.cstart <= cast(t.operation_date AS date)
        AND (p.cend IS NULL
             OR cast(t.operation_date AS date) <= p.cend)) tt),
    PENALTY_PAYMENT = 
  (SELECT coalesce(sum(tt.amount), 0)
   FROM
     (SELECT t.amount
      FROM regop_transfer t
      JOIN regop_wallet w ON w.wallet_guid = t.target_guid
      JOIN regop_pers_acc pa ON pa.p_wallet_id = w.id
      AND pa.id = ps.account_id
      WHERE t.reason = 'Оплата пени'
        AND p.cstart <= cast(t.operation_date AS date)
        AND (p.cend IS NULL
             OR cast(t.operation_date AS date) <= p.cend)
      UNION ALL SELECT (-1)*t.amount AS amount
      FROM regop_transfer t
      JOIN regop_wallet w ON w.wallet_guid = t.source_guid
      JOIN regop_pers_acc pa ON pa.p_wallet_id = w.id
      AND pa.id = ps.account_id
      WHERE t.reason = 'Отмена оплаты пени'
        AND p.cstart <= cast(t.operation_date AS date)
        AND (p.cend IS NULL
             OR cast(t.operation_date AS date) <= p.cend)
      UNION ALL SELECT t.target_coef*t.amount
      FROM regop_transfer t
      JOIN regop_wallet w ON w.wallet_guid = t.target_guid
      JOIN regop_pers_acc pa ON pa.p_wallet_id = w.id  --по пени
      AND pa.id = ps.account_id
	  JOIN regop_money_operation mo on mo.id = t.op_id
      WHERE mo.reason = 'Перенос долга при слиянии'
        AND t.amount < 0
        AND p.cstart <= cast(t.operation_date AS date)
        AND (p.cend IS NULL OR cast(t.operation_date AS date) <= p.cend)
      UNION ALL SELECT -t.target_coef*t.amount
      FROM regop_transfer t
      JOIN regop_wallet w ON w.wallet_guid = t.source_guid
      JOIN regop_pers_acc pa ON pa.p_wallet_id = w.id  --по пени
      AND pa.id = ps.account_id
	  JOIN regop_money_operation mo on mo.id = t.op_id
      WHERE mo.reason = 'Перенос долга при слиянии'
      AND t.amount < 0
	  AND p.cstart <= cast(t.operation_date AS date)
	  AND (p.cend IS NULL OR cast(t.operation_date AS date) <= p.cend)
      UNION ALL SELECT t.amount
      FROM regop_transfer t
      JOIN regop_wallet w ON w.wallet_guid = t.source_guid
      JOIN regop_pers_acc pa ON pa.p_wallet_id = w.id
      AND pa.id = ps.account_id
      WHERE t.reason = 'Зачисление по пеням в счет отмены возврата'
        AND p.cstart <= cast(t.operation_date AS date)
        AND (p.cend IS NULL
             OR cast(t.operation_date AS date) <= p.cend)
      UNION ALL SELECT t.amount AS amount
      FROM regop_transfer t
      JOIN regop_wallet w ON w.wallet_guid = t.target_guid
      JOIN regop_pers_acc pa ON pa.p_wallet_id = w.id
      AND pa.id = ps.account_id
      WHERE t.reason = 'Корректировка оплат по пени'
        AND p.cstart <= cast(t.operation_date AS date)
        AND (p.cend IS NULL
             OR cast(t.operation_date AS date) <= p.cend)
      UNION ALL SELECT t.amount AS amount
      FROM regop_transfer t
      JOIN regop_wallet w ON w.wallet_guid = t.target_guid
      JOIN regop_pers_acc pa ON pa.p_wallet_id = w.id
      AND pa.id = ps.account_id
      WHERE t.reason = 'Корректировка оплат по пени'
        AND p.cstart <= cast(t.operation_date AS date)
        AND (p.cend IS NULL
             OR cast(t.operation_date AS date) <= p.cend)
      UNION ALL SELECT (-1)*t.amount AS amount
      FROM regop_transfer t
      JOIN regop_wallet w ON w.wallet_guid = t.source_guid
      JOIN regop_pers_acc pa ON pa.p_wallet_id = w.id
      AND pa.id = ps.account_id
      WHERE t.reason = 'Возврат пени'
        AND p.cstart <= cast(t.operation_date AS date)
        AND (p.cend IS NULL
             OR cast(t.operation_date AS date) <= p.cend)) tt)
    FROM regop_period p
    WHERE p.id = ps.period_id
        AND p.id >= :pid
        AND ps.account_id IN (:ids)";

            const string SaldoRecalc = @"CREATE or REPLACE FUNCTION  recalc_saldo (acc_id integer, start_period integer default 0) RETURNS void AS '
                DECLARE
                    _period_id integer;
                    _next_period_id integer;
                BEGIN  
                    FOR _period_id IN select  id from regop_period order by cstart
                    LOOP
                    select coalesce(id, 0) into _next_period_id from regop_period
                        where id > _period_id and (start_period <= 0 or start_period <= id)
                        order by cstart
                        limit 1 ;
   
      
                    update regop_pers_acc_period_summ ps1
                    set 
                    saldo_out = ps2.new_saldo_out from
                    (
                        SELECT account_id, saldo_in  + penalty - penalty_payment + recalc_penalty + recalc_decision + recalc + charge_tariff - tariff_payment - tariff_desicion_payment + balance_change as new_saldo_out
                        FROM regop_pers_acc_period_summ
                        where period_id = _period_id
                    ) ps2
                    where period_id = _period_id 
                    and ps1.account_id = ps2.account_id 
                    and ps1.account_id = acc_id;

                    update regop_pers_acc_period_summ ps1
                    set 
                    saldo_in = ps2.saldo_out
                    from
                    (
                        select account_id, saldo_out from regop_pers_acc_period_summ 
                        where period_id = _period_id
                    ) ps2
                    where period_id = _next_period_id
                    and ps1.account_id = ps2.account_id 
                    and ps1.account_id = acc_id;
    
                    END LOOP;
                END;
                ' LANGUAGE plpgsql;";

            const string RecalcSaldoDelete = "drop function recalc_saldo()";

            var recalcSaldoExecute = @"SELECT recalc_saldo(id, " + this.startRecalc.Id.ToString() + ") from regop_pers_acc where id in (:ids)";

            using (var statelessSession = this.SessionProvider.OpenStatelessSession())
            {
                statelessSession.CreateSQLQuery(SaldoRecalc).ExecuteUpdate();
                foreach (var persAccIds in this.persAccs.Section(1000))
                {
                    statelessSession.CreateSQLQuery(calcPayments)
                        .SetParameter("pid", this.startRecalc.Id)
                        .SetParameterList("ids", persAccIds)
                        .ExecuteUpdate();
                    statelessSession.CreateSQLQuery(recalcSaldoExecute).SetParameterList("ids", persAccIds).List();
                }
                statelessSession.CreateSQLQuery(RecalcSaldoDelete);
            }
        }

        private List<Transfer> ApplyPayment(
            string transferGuid,
            MoneyOperation operation,
            ImportedPayment importedPayment,
            ChargePeriod chargePeriod,
            AmountDistributionType amountDistrType,
            DateTime operationDate)
        {
            var refundCommand = this.RefundCommandFactory.GetCommand(importedPayment.PaymentType);

            if (refundCommand != null)
            {
                return
                    importedPayment.PersonalAccount.ApplyRefund(
                        refundCommand,
                        new MoneyStream(transferGuid, operation, importedPayment.PaymentDate, importedPayment.Sum)
                        {
                            Description = "Подтверждение возврата",
                            OperationDate = operationDate
                        }).ToList();
            }

            var command = this.CommandFactory.GetCommand(importedPayment.PaymentType);

            return importedPayment.PersonalAccount.ApplyPayment(
                command,
                new MoneyStream(transferGuid, operation, importedPayment.PaymentDate, importedPayment.Sum)
                {
                    Description = "Подтверждение оплаты",
                    OperationDate = operationDate
                },
                amountDistrType);
        }

        private class PersonalAccountWalletProxy
        {
            private string[] walletGuids;

            public long Id { get; set; }

            public string AccountNumber { get; set; }

            public long RoId { get; set; }

            public Tuple<WalletType, string>[] WalletGuids { get; set; }

            public IEnumerable<string> GetWalletGuids()
            {
                return this.walletGuids ?? (this.walletGuids = this.WalletGuids.Select(x => x.Item2).ToArray());
            }
        }

        private struct QueryDto
        {
            public long Id;
        }

        #region Implementation of IExecutionAction

        /// <summary>
        /// Описание действия
        /// </summary>
        public override string Description
            =>
                "Действие выявляет и исправляет неверно распределенные реестры оплат. Выполнять с осторожностью, работает с закрытыми периодами, вызывает перерасчёт. Пересчитывает оплаты и сальдо ЛС, у которых найдутся проблемы!"
            ;

        /// <summary>
        /// Наименование
        /// </summary>
        public override string Name => "РегОператор - Исправить реестры оплат с несевшими оплатами.";

        /// <summary>
        /// Выполняемое действие
        /// </summary>
        public override Func<IDataResult> Action => this.Execute;
        #endregion
    }
}