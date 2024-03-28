namespace Bars.Gkh.RegOperator.Imports.Ches
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Domain.CollectionExtensions;
    using Bars.Gkh.Extensions;
    using Bars.Gkh.RegOperator.Domain.ValueObjects;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount.Operations;
    using Bars.Gkh.RegOperator.Entities.Refactor;
    using Bars.Gkh.RegOperator.Entities.ValueObjects;
    using Bars.Gkh.RegOperator.Entities.Wallet;
    using Bars.Gkh.RegOperator.Enums;
    using Bars.Gkh.RegOperator.Exceptions;
    using Bars.Gkh.RegOperator.Imports.Ches.PreImport;
    using Bars.Gkh.RegOperator.Tasks.TaskHelpers;

    using Dapper;

    using NHibernate.Linq;

    /// <summary>
    /// Импорт оплат
    /// </summary>
    public class PayImporter : BaseChesImporter<PayFileInfo>
    {
        public static string Id = MethodBase.GetCurrentMethod().DeclaringType.FullName;

        private const int PartSize = 50000;

        /// <summary>
        /// Название импорта
        /// </summary>
        public override string ImportName => "Импорт оплат";

        private readonly List<Transfer> tranfersForSave = new List<Transfer>();
        private readonly List<MoneyOperation> moneyOperationsForSave = new List<MoneyOperation>();
        private readonly List<PaymentImport> paymentImportsForSave = new List<PaymentImport>();
        private readonly List<Wallet> walletsForSave = new List<Wallet>();
        private readonly List<PersonalAccountPeriodSummary> periodSummaryForSave = new List<PersonalAccountPeriodSummary>();
        private Dictionary<long, Dictionary<WalletType, string>> walletGuidByRoId;
        private Dictionary<long, RealityObjectPaymentAccount> realityObjectPayAccDictByRoId;
        private Dictionary<string, BasePersonalAccount> accountDict = new Dictionary<string, BasePersonalAccount>();
        private Dictionary<string, PersonalAccountPeriodSummary> periodSummaryDict = new Dictionary<string, PersonalAccountPeriodSummary>();
        private ProgressSender progress;

        /// <summary>
        /// Домен-сервис Лицевой счет
        /// </summary>
        public IDomainService<BasePersonalAccount> PersonalAccountDomian { get; set; }

        /// <summary>
        /// Домен-сервис Трансфер
        /// </summary>
        public IDomainService<Transfer> TransferDomian { get; set; }

        /// <summary>
        /// Домен-сервис Операция
        /// </summary>
        public IDomainService<MoneyOperation> MoneyOperationDomian { get; set; }

        /// <summary>
        /// Домен-сервис Импорт оплат
        /// </summary>
        public IDomainService<PaymentImportSource> PaymentImportSourceDomain { get; set; }

        public IDomainService<PersonalAccountPeriodSummary> PeriodSummaryDomain { get; set; }

        ///<inheritdoc/>
        public override void InitDicts(PayFileInfo importFileInfo)
        {
            this.progress = new ProgressSender(importFileInfo.Rows.Count, this.Indicate, 80);

            this.progress.ForceSend(0, "Импорт оплат: инициализация справочников");

            var domain = this.Container.ResolveDomain<RealityObjectPaymentAccount>();
            using (this.Container.Using(domain))
            {
                this.realityObjectPayAccDictByRoId = domain.GetAll()
                    .Fetch(x => x.BaseTariffPaymentWallet)
                    .Fetch(x => x.DecisionPaymentWallet)
                    .Fetch(x => x.PenaltyPaymentWallet)
                    .ToDictionary(x => x.RealityObject.Id);
            }

            this.walletGuidByRoId = this.realityObjectPayAccDictByRoId
                .Select(
                    x => new
                    {
                        Id = x.Key,
                        BaseTariffPaymentWallet = x.Value.BaseTariffPaymentWallet.WalletGuid,
                        DecisionPaymentWallet = x.Value.DecisionPaymentWallet.WalletGuid,
                        PenaltyPaymentWallet = x.Value.PenaltyPaymentWallet.WalletGuid,
                    })
                .AsEnumerable()
                .ToDictionary(
                    x => x.Id,
                    x => new Dictionary<WalletType, string>
                    {
                        { WalletType.BaseTariffWallet, x.BaseTariffPaymentWallet },
                        { WalletType.DecisionTariffWallet, x.DecisionPaymentWallet },
                        { WalletType.PenaltyWallet, x.PenaltyPaymentWallet }
                    });
        }

        ///<inheritdoc/>
        public override void ProcessData(PayFileInfo payFileInfo)
        {
            this.progress.ForceSend(20, "Импорт оплат");
            var percent = 0;

            var paymentSource = new PaymentImportSource(payFileInfo.Period)
            {
                User = this.Login
            };

            this.PaymentImportSourceDomain.Save(paymentSource);

            foreach (var section in payFileInfo.Rows.OrderBy(x => x.LsNum).ThenBy(x => x.PaymentDate).ThenBy(x => x.PaymentType).Section(PayImporter.PartSize))
            {
                var sect = section.ToArray();

                var accountNums = sect.Select(x => x.LsNum).Distinct().ToArray();

                this.accountDict = this.PersonalAccountDomian.GetAll()
                    .Where(x => accountNums.Contains(x.PersonalAccountNum))
                    .Fetch(x => x.BaseTariffWallet)
                    .Fetch(x => x.DecisionTariffWallet)
                    .Fetch(x => x.PenaltyWallet)
                    .Fetch(x => x.Room)
                    .AsEnumerable()
                    .ToDictionary(x => x.PersonalAccountNum);

                this.periodSummaryDict = this.PeriodSummaryDomain.GetAll()
                    .Where(x => x.Period.Id == payFileInfo.Period.Id)
                    .Where(x => accountNums.Contains(x.PersonalAccount.PersonalAccountNum))
                    .ToDictionary(x => x.PersonalAccount.PersonalAccountNum);

                var paymentOperation = paymentSource.CreateOperation(payFileInfo.Period);

                var canceledOperation = paymentSource.CreateOperation(payFileInfo.Period);

                var cancelOperation = canceledOperation.Cancel(payFileInfo.Period);

                foreach (var row in sect)
                {
                    if (!this.accountDict.ContainsKey(row.LsNum))
                    {
                        payFileInfo.AddErrorRow(row, $"Лицевой счет {row.LsNum} не найден. Строка {row.RowNumber}");
                        continue;
                    }

                    var account = this.accountDict.Get(row.LsNum);

                    var periodSummary = this.periodSummaryDict.Get(row.LsNum);

                    if (row.PaymentType == ImportPaymentType.Payment)
                    {
                        if (row.TariffPayment != 0)
                        {
                            var transfer = account
                                .BaseTariffWallet.StoreMoney(TransferBuilder.Create(
                                    account,
                                    new MoneyStream(paymentSource, paymentOperation, row.PaymentDate, row.TariffPayment)
                                    {
                                        Description = "Оплата по базовому тарифу",
                                        IsAffect = true
                                    }));

                            this.tranfersForSave.Add(transfer);
                            this.tranfersForSave.Add(this.CopyIncomeTransfer(transfer, account, WalletType.BaseTariffWallet));
                            this.walletsForSave.Add(account.BaseTariffWallet);
                        }

                        if (row.TariffDecisionPayment != 0)
                        {
                            var transfer = account
                                .DecisionTariffWallet.StoreMoney(TransferBuilder.Create(
                                    account,
                                    new MoneyStream(paymentSource, paymentOperation, row.PaymentDate, row.TariffDecisionPayment)
                                    {
                                        Description = "Оплата по тарифу решения",
                                        IsAffect = true
                                    }));

                            this.tranfersForSave.Add(transfer);
                            this.tranfersForSave.Add(this.CopyIncomeTransfer(transfer, account, WalletType.DecisionTariffWallet));
                            this.walletsForSave.Add(account.DecisionTariffWallet);
                        }

                        if (row.PenaltyPayment != 0)
                        {
                            var transfer = account
                                .PenaltyWallet.StoreMoney(TransferBuilder.Create(
                                    account,
                                    new MoneyStream(paymentSource, paymentOperation, row.PaymentDate, row.PenaltyPayment)
                                    {
                                        Description = "Оплата пени",
                                        IsAffect = true
                                    }));

                            this.tranfersForSave.Add(transfer);
                            this.tranfersForSave.Add(this.CopyIncomeTransfer(transfer, account, WalletType.PenaltyWallet));
                            this.walletsForSave.Add(account.PenaltyWallet);
                        }

                        periodSummary.ApplyPayment(row.TariffPayment, row.TariffDecisionPayment, row.PenaltyPayment);
                        this.periodSummaryForSave.Add(periodSummary);

                        var paymentImport = new PaymentImport
                        {
                            PaymentOperation = paymentSource,
                            BasePersonalAccount = account,
                            PaymentDate = row.PaymentDate,
                            PaymentType = row.PaymentType,
                            TariffPayment = row.TariffPayment,
                            TariffDecisionPayment = row.TariffDecisionPayment,
                            PenaltyPayment = row.PenaltyPayment,
                            RegistryNum = row.RegistryNum,
                            RegistryDate = row.RegistryDate
                        };

                        this.paymentImportsForSave.Add(paymentImport);
                    }
                    else
                    {
                        var description = row.PaymentType == ImportPaymentType.CancelPayment ? "Отмена оплаты " : "Возврат взносов на КР ";

                        var operation = row.PaymentType == ImportPaymentType.CancelPayment ? cancelOperation : paymentOperation;

                        try
                        {
                            this.ApplyRefund(row, account, paymentSource, operation, description);
                            periodSummary.ApplyRefund(row.TariffPayment, row.TariffDecisionPayment, row.PenaltyPayment);
                            this.periodSummaryForSave.Add(periodSummary);
                        }
                        catch (WalletBalanceException exception)
                        {
                            payFileInfo.AddErrorRow(row, $"{account.PersonalAccountNum}, строка {row.RowNumber}. {exception.Message}");
                        }

                        var paymentImport = new PaymentImport
                        {
                            PaymentOperation = paymentSource,
                            BasePersonalAccount = account,
                            PaymentDate = row.PaymentDate,
                            PaymentType = row.PaymentType,
                            TariffPayment = row.TariffPayment,
                            TariffDecisionPayment = row.TariffDecisionPayment,
                            PenaltyPayment = row.PenaltyPayment,
                            RegistryNum = row.RegistryNum,
                            RegistryDate = row.RegistryDate
                        };

                        this.paymentImportsForSave.Add(paymentImport);
                    }

                    row.IsImported = true;

                    this.LogImport.CountAddedRows++;
                    this.LogImport.Info(this.ImportName, string.Format("На лс {0} добавлен(а) {1} по базовому:{2}; по тарифу:{3}; по пени:{4}",
                        account.PersonalAccountNum, row.PaymentType.GetDisplayName(), row.TariffPayment, row.TariffDecisionPayment, row.PenaltyPayment));
                }

                if (this.paymentImportsForSave.Count(x => x.PaymentType == ImportPaymentType.Payment || x.PaymentType == ImportPaymentType.ReturnPayment) != 0)
                {
                    this.moneyOperationsForSave.Add(paymentOperation);
                }

                if (this.paymentImportsForSave.Count(x => x.PaymentType != ImportPaymentType.Payment) != 0)
                {
                    this.moneyOperationsForSave.Add(canceledOperation);
                    this.moneyOperationsForSave.Add(cancelOperation);
                }

                percent = Math.Min(percent + PayImporter.PartSize, payFileInfo.Rows.Count);
                var percentValue = ((double)percent / payFileInfo.Rows.Count) * 85;
                this.Indicate?.Invoke(Math.Min((int)percentValue, 100), $"Сохранение ({percent}/{payFileInfo.Rows.Count} строк)");

                TransactionHelper.InsertInManyTransactions(this.Container, this.moneyOperationsForSave, 50000, true, true);
                TransactionHelper.InsertInManyTransactions(this.Container, this.tranfersForSave, 50000, true, true);
                TransactionHelper.InsertInManyTransactions(this.Container, this.paymentImportsForSave, 50000, true, true);
                TransactionHelper.InsertInManyTransactions(this.Container, this.walletsForSave, 50000, true, true);
                TransactionHelper.InsertInManyTransactions(this.Container, this.periodSummaryForSave, 50000, true, true);

                this.moneyOperationsForSave.Clear();
                this.tranfersForSave.Clear();
                this.paymentImportsForSave.Clear();
                this.walletsForSave.Clear();
                this.periodSummaryForSave.Clear();

                this.Container.Resolve<ISessionProvider>().GetCurrentSession().Clear();
            }

            this.UpdateImportedRows(payFileInfo);

            this.progress.ForceSend(100, "Импорт оплат: Завершен");
        }

        private void ApplyRefund(PayFileInfo.Row row, BasePersonalAccount account, PaymentImportSource paymentSource, MoneyOperation operation, string description)
        {
            if (row.TariffPayment != 0)
            {
                var transfer = account
                    .BaseTariffWallet.TakeMoney(
                        TransferBuilder.Create(
                            account,
                            new MoneyStream(paymentSource, operation, row.PaymentDate, row.TariffPayment)
                            {
                                Description = description + "по базовому тарифу",
                                IsAffect = true
                            }));

                this.tranfersForSave.Add(transfer);

                this.tranfersForSave.Add(this.CopyOutcomeTransfer(transfer, account, WalletType.BaseTariffWallet));
            }

            if (row.TariffDecisionPayment != 0)
            {
                var transfer = account
                    .DecisionTariffWallet.TakeMoney(
                        TransferBuilder.Create(
                            account,
                            new MoneyStream(paymentSource, operation, row.PaymentDate, row.TariffDecisionPayment)
                            {
                                Description = description + "по тарифу решения",
                                IsAffect = true
                            }));

                this.tranfersForSave.Add(transfer);

                this.tranfersForSave.Add(this.CopyOutcomeTransfer(transfer, account, WalletType.DecisionTariffWallet));
            }

            if (row.PenaltyPayment != 0)
            {
                var transfer = account
                    .PenaltyWallet.TakeMoney(
                        TransferBuilder.Create(
                            account,
                            new MoneyStream(paymentSource, operation, row.PaymentDate, row.PenaltyPayment)
                            {
                                Description = description + "пени",
                                IsAffect = true
                            }));

                this.tranfersForSave.Add(transfer);
                this.tranfersForSave.Add(this.CopyOutcomeTransfer(transfer, account, WalletType.PenaltyWallet));
            }
        }

        private Transfer CopyIncomeTransfer(Transfer originator, BasePersonalAccount account, WalletType walletType)
        {
            var roWalletGuids = this.walletGuidByRoId.Get(account.Room.RealityObject.Id);

            if (roWalletGuids.IsEmpty())
            {
                return null;
            }

            return new RealityObjectTransfer(
                this.realityObjectPayAccDictByRoId.Get(account.Room.RealityObject.Id),
                originator.TargetGuid, 
                roWalletGuids.Get(walletType),
                originator.Amount, 
                originator.Operation)
            {
                Reason = originator.Reason,
                PaymentDate = originator.PaymentDate,
                OperationDate = originator.OperationDate,
                CopyTransfer = originator.As<PersonalAccountPaymentTransfer>(),
                OriginatorName = account.PersonalAccountNum,
                IsAffect = originator.IsAffect
            };
        }

        private Transfer CopyOutcomeTransfer(Transfer originator, BasePersonalAccount account, WalletType walletType)
        {
            var roWalletGuids = this.walletGuidByRoId.Get(account.Room.RealityObject.Id);
            if (roWalletGuids.IsEmpty())
            {
                return null;
            }

            return new RealityObjectTransfer(
                this.realityObjectPayAccDictByRoId.Get(account.Room.RealityObject.Id), 
                roWalletGuids.Get(walletType), 
                originator.TargetGuid, 
                originator.Amount, 
                originator.Operation)
            {
                Reason = originator.Reason,
                PaymentDate = originator.PaymentDate,
                OperationDate = originator.OperationDate,
                CopyTransfer = originator.As<PersonalAccountPaymentTransfer>(),
                OriginatorName = account.PersonalAccountNum,
                IsAffect = originator.IsAffect
            };
        }

        private void UpdateImportedRows(PayFileInfo payFileInfo)
        {
            this.Container.InStatelessConnectionTransaction((conn, tr) =>
            {
                var importedRowIds = payFileInfo.Rows.Where(x => x.IsImported).Select(x => x.Id).ToList();
                var sql = $@"UPDATE {this.ChesTempDataProvider.TableName}
                                 SET isimported=true
                                 WHERE id in ({string.Join(",", importedRowIds)});";
                conn.Execute(sql, transaction: tr);
            });
        }
    }
}