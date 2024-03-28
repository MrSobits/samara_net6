namespace Bars.Gkh.RegOperator.Imports.Ches
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.Caching.LinqExtensions;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.RegOperator.Domain.ValueObjects;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount.Operations;
    using Bars.Gkh.RegOperator.Entities.Refactor;
    using Bars.Gkh.RegOperator.Entities.ValueObjects;
    using Bars.Gkh.RegOperator.Entities.Wallet;
    using Bars.Gkh.RegOperator.Enums;
    using Bars.Gkh.RegOperator.Extenstions;
    using Bars.Gkh.RegOperator.Tasks.TaskHelpers;
    using Bars.Gkh.Repositories.ChargePeriod;
    using Bars.Gkh.Utils;

    /// <summary>
    /// Импорт корректировок сальдо
    /// </summary>
    public class SaldoChangeImporter : BaseChesImporter<SaldoChangeFileInfo>
    {
        /// <summary>
        /// Код импорта
        /// </summary>
        public static string Id = MethodBase.GetCurrentMethod().DeclaringType.FullName;

        private Dictionary<string, AccountProxy> accountDict;
        private Dictionary<string, long> accountIds;

        private List<TransferProxy> saldoChangesCache;

        private List<TransferProxy> cancelChargesCache;

        private ProgressSender progress;

        /// <summary>
        /// Домен-сервис
        /// </summary>
        public IDomainService<BasePersonalAccount> PersonalAccountDomain { get; set; }

        /// <summary>
        /// Домен-сервис
        /// </summary>
        public IDomainService<ChargeOperationBase> ChargeOperationBaseDomain { get; set; }

        /// <summary>
        /// Домен-сервис
        /// </summary>
        public IDomainService<MoneyOperation> MoneyOperationDomain { get; set; }

        /// <summary>
        /// Домен-сервис
        /// </summary>
        public IDomainService<ImportCancelChargeSource> ImportCancelChargeSourceDomain { get; set; }

        /// <summary>
        /// Домен-сервис
        /// </summary>
        public IDomainService<SaldoChangeSource> SaldoChangeSourceDomain { get; set; }

        /// <summary>
        /// Домен-сервис
        /// </summary>
        public IDomainService<CancelCharge> CancelChargeDomain { get; set; }

        /// <summary>
        /// Домен-сервис
        /// </summary>
        public IDomainService<SaldoChangeDetail> SaldoChangeDetailDomain { get; set; }

        /// <summary>
        /// Домен-сервис
        /// </summary>
        public IDomainService<Transfer> TransferDomain { get; set; }

        /// <summary>
        /// Домен-сервис
        /// </summary>
        public IDomainService<PersonalAccountChange> PersonalAccountChangeDomain { get; set; }

        /// <summary>
        /// Репозиторий периода начислений
        /// </summary>
        public IChargePeriodRepository PeriodRepository { get; set; }

        /// <summary>
        /// Проинициализировать справочники
        /// </summary>
        /// <param name="importFileInfo">Файл импорта</param>
        public override void InitDicts(SaldoChangeFileInfo importFileInfo)
        {
            this.progress = new ProgressSender(importFileInfo.Rows.Count, this.Indicate);

            this.progress.ForceSend(0, "Импорт корректировок сальдо: инициализация справочников");

            this.accountIds = this.PersonalAccountDomain.GetAll()
                .Select(x => new
                {
                    x.Id,
                    x.PersonalAccountNum
                })
                .ToDictionary(x => x.PersonalAccountNum, x => x.Id);

            var saldoChangeSourceQuery = this.ChargeOperationBaseDomain.GetAll()
                .Where(y => y.ChargeSource == TypeChargeSource.SaldoChangeMass);

            this.saldoChangesCache = this.TransferDomain.GetAll<PersonalAccountChargeTransfer>()
                .Where(x => x.ChargePeriod == importFileInfo.Period)
                .Where(x => saldoChangeSourceQuery.Any(y => y.OriginatorGuid == x.TargetGuid))
                .Select(x => new TransferProxy
                {
                    AccountId = x.Id,
                    Amount = x.Amount,
                    OperationDate = x.OperationDate
                })
                .ToList();

            var cancelChargeSourceQuery = this.ChargeOperationBaseDomain.GetAll()
                .Where(y => y.ChargeSource == TypeChargeSource.CancelCharge);

            this.cancelChargesCache = this.TransferDomain.GetAll<PersonalAccountChargeTransfer>()
                .Where(x => x.ChargePeriod == importFileInfo.Period)
                .Where(x => cancelChargeSourceQuery.Any(y => y.OriginatorGuid == x.TargetGuid))
                .Select(x => new TransferProxy
                {
                    AccountId = x.Owner.Id,
                    Amount = x.Amount,
                    OperationDate = x.OperationDate
                })
                .ToList();
        }

        /// <summary>
        /// Обработать данные импорта
        /// </summary>
        /// <param name="saldoChangeFileInfo">Файл импорта</param>
        public override void ProcessData(SaldoChangeFileInfo saldoChangeFileInfo)
        {
            this.progress.ForceSend(20, "Импорт корректировок сальдо");

            var cancelCharges = saldoChangeFileInfo.Rows.Where(x => x.ChangeType == TypeChargeSource.ImportCancelCharge).ToList();

            if (cancelCharges.Any())
            {
                this.ProcessCancelCharges(cancelCharges, saldoChangeFileInfo);
            }

            this.progress.ForceSend(60, "Импорт корректировок сальдо");

            var saldoChanges = saldoChangeFileInfo.Rows.Where(x => x.ChangeType == TypeChargeSource.SaldoChangeMass).ToList();

            if (saldoChanges.Any())
            {
                this.ProcessSaldoChanges(saldoChanges, saldoChangeFileInfo);
            }

            this.progress.ForceSend(100, "Импорт корректировок сальдо");
        }

        private void ProcessSaldoChanges(List<SaldoChangeFileInfo.Row> saldoChanges, SaldoChangeFileInfo accountFileInfo)
        {
            var saldoChangeSource = new SaldoChangeSource(TypeChargeSource.SaldoChangeMass, accountFileInfo.Period)
            {
                User = this.Login,
                OperationDate = DateTime.Now,
                Reason = this.ParentImportName
            };

            var moneyOperation = saldoChangeSource.CreateOperation(accountFileInfo.Period);

            this.SaldoChangeSourceDomain.Save(saldoChangeSource);
            this.MoneyOperationDomain.Save(moneyOperation);

            // 40% делим на количество порций по 50 000,чтобы рассчитать шаг
            var current = 0;
            var step = 50000;
            foreach (var saldoChangePart in saldoChanges.Split(step))
            {
                this.progress.ForceSend(60 + (int)(current * 100d / saldoChanges.Count * 0.4), "Импорт корректировок сальдо");

                var transfers = new List<Transfer>(step);
                var details = new List<SaldoChangeDetail>(step);
                
                this.accountDict = this.PersonalAccountDomain.GetAll()
                    .Select(x => new AccountProxy
                    {
                        Id = x.Id,
                        PersonalAccountNum = x.PersonalAccountNum,
                        BaseTariffWallet = x.BaseTariffWallet,
                        PenaltyWallet = x.PenaltyWallet,
                        DecisionTariffWallet = x.DecisionTariffWallet
                    })
                    .GetPortioned(x => x.Id, saldoChangePart.Select(x => this.accountIds.Get(x.LsNum)).ToArray(), 2000)
                    .ToDictionary(x => x.PersonalAccountNum);

                foreach (var saldoChange in saldoChangePart)
                {
                    if (!this.accountDict.ContainsKey(saldoChange.LsNum))
                    {
                        this.LogImport.Error("Ошибка", $"Лицевой счет {saldoChange.LsNum} не найден. Строка {saldoChange.RowNumber}");
                        continue;
                    }

                    var account = this.accountDict.Get(saldoChange.LsNum);

                    var exists = this.saldoChangesCache
                        .Where(x => x.AccountId == account.Id)
                        .Where(x => x.Amount == saldoChange.SaldoChangeBase || x.Amount == saldoChange.SaldoChangeTr || x.Amount == saldoChange.SaldoChangePeni)
                        .Any(x => x.OperationDate == saldoChange.ChangeDate);

                    if (exists)
                    {
                        this.LogImport.Error("Ошибка", $"Сумма уже была загружена. Строка {saldoChange.RowNumber}");
                        continue;
                    }

                    if (saldoChange.SaldoChangeBase != 0)
                    {
                        details.Add(new SaldoChangeDetail(saldoChangeSource, account.ToAccount())
                        {
                            ChangeType = WalletType.BaseTariffWallet,
                            OldValue = 0,
                            NewValue = saldoChange.SaldoChangeBase
                        });

                        transfers.Add(
                            account.BaseTariffWallet.TakeMoney(TransferBuilder.Create(
                                account.ToAccount(),
                                new MoneyStream(saldoChangeSource, moneyOperation, saldoChange.ChangeDate, saldoChange.SaldoChangeBase)
                                {
                                    Description = "Установка/изменение сальдо по базовому тарифу"
                                })));
                    }

                    if (saldoChange.SaldoChangeTr != 0)
                    {
                        details.Add(new SaldoChangeDetail(saldoChangeSource, account.ToAccount())
                        {
                            ChangeType = WalletType.DecisionTariffWallet,
                            OldValue = 0,
                            NewValue = saldoChange.SaldoChangeTr
                        });
                        
                        transfers.Add(
                            account.DecisionTariffWallet.TakeMoney(TransferBuilder.Create(
                                account.ToAccount(),
                                new MoneyStream(saldoChangeSource, moneyOperation, saldoChange.ChangeDate, saldoChange.SaldoChangeTr)
                                {
                                    Description = "Установка/изменение сальдо по тарифу решения"
                                })));
                    }

                    if (saldoChange.SaldoChangePeni != 0)
                    {
                        details.Add(new SaldoChangeDetail(saldoChangeSource, account.ToAccount())
                        {
                            ChangeType = WalletType.PenaltyWallet,
                            OldValue = 0,
                            NewValue = saldoChange.SaldoChangePeni
                        });

                        transfers.Add(
                            account.PenaltyWallet.TakeMoney(TransferBuilder.Create(
                                account.ToAccount(),
                                new MoneyStream(saldoChangeSource, moneyOperation, saldoChange.ChangeDate, saldoChange.SaldoChangePeni)
                                {
                                    Description = "Установка/изменение пени"
                                })));
                    }

                    this.LogImport.CountAddedRows++;
                    this.LogImport.Info("Импорт изменений сальдо",
                        $"На лс {account.PersonalAccountNum} добавлены установки/изменения сальдо " +
                        $"по базовому:{saldoChange.SaldoChangeBase}; по тарифу:{saldoChange.SaldoChangeTr}; по пени:{saldoChange.SaldoChangePeni}");
                }

                TransactionHelper.InsertInManyTransactions(this.Container, details, 10000, true, true);
                TransactionHelper.InsertInManyTransactions(this.Container, transfers, 10000, true, true);

                this.SaveSaldoChangeHistory(saldoChangeSource, details);

                current += step;

                // делаем так, чтобы не сжиралось место в оперативе
                this.Container.Resolve<ISessionProvider>().GetCurrentSession().Clear();
            }
        }

        private void ProcessCancelCharges(List<SaldoChangeFileInfo.Row> cancelCharges, SaldoChangeFileInfo saldoChangeFileInfo)
        {
            var cancelChargeSource = new ImportCancelChargeSource(saldoChangeFileInfo.Period)
            {
                User = this.Login,
                OperationDate = DateTime.Now,
                Reason = this.ParentImportName
            };

            var moneyOperation = cancelChargeSource.CreateOperation(saldoChangeFileInfo.Period);

            this.ImportCancelChargeSourceDomain.Save(cancelChargeSource);
            this.MoneyOperationDomain.Save(moneyOperation);

            // 30% делим на количество порций по 50 000,чтобы рассчитать шаг
            var current = 0;
            var step = 50000;
            foreach (var cancelChargePart in cancelCharges.Split(step))
            {
                this.progress.ForceSend(20 + (int)(current * 100d / cancelCharges.Count * 0.4), "Импорт корректировок сальдо");

                var transfers = new List<Transfer>();
                var details = new List<CancelCharge>();
                
                this.accountDict = this.PersonalAccountDomain.GetAll()
                    .Select(x => new AccountProxy
                    {
                        Id = x.Id,
                        PersonalAccountNum = x.PersonalAccountNum,
                        BaseTariffWallet = x.BaseTariffWallet,
                        PenaltyWallet = x.PenaltyWallet,
                        DecisionTariffWallet = x.DecisionTariffWallet
                    })
                    .GetPortioned(x => x.Id, cancelChargePart.Select(x => this.accountIds.Get(x.LsNum)).ToArray(), 2000)
                    .ToDictionary(x => x.PersonalAccountNum);

                foreach (var cancelCharge in cancelChargePart)
                {
                    if (!this.accountDict.ContainsKey(cancelCharge.LsNum))
                    {
                        this.LogImport.Error("Ошибка", $"Лицевой счет {cancelCharge.LsNum} не найден. Строка {cancelCharge.RowNumber}");
                        continue;
                    }

                    var account = this.accountDict.Get(cancelCharge.LsNum);

                    var exists = this.cancelChargesCache
                        .Where(x => x.AccountId == account.Id)
                        .Where(x => x.Amount == cancelCharge.SaldoChangeBase || x.Amount == cancelCharge.SaldoChangeTr || x.Amount == cancelCharge.SaldoChangePeni)
                        .Any(x => x.OperationDate == cancelCharge.ChangeDate);

                    if (exists)
                    {
                        this.LogImport.Error("Ошибка", $"Сумма уже была загружена. Строка {cancelCharge.RowNumber}");
                        continue;
                    }

                    var cancelPeriod = this.PeriodRepository.GetPeriodByDate(cancelCharge.ChangeMonth.Value);

                    var btTransfer = account.BaseTariffWallet.TakeMoney(TransferBuilder.Create(
                        account.ToAccount(),
                        new MoneyStream(cancelChargeSource, moneyOperation, cancelCharge.ChangeDate, cancelCharge.SaldoChangeBase)
                        {
                            Description = "Отмена начислений по базовому тарифу"
                        }));

                    if (btTransfer != null)
                    {
                        details.Add(new CancelCharge(
                            cancelChargeSource,
                            account.ToAccount(),
                            cancelPeriod,
                            btTransfer.Amount,
                            CancelType.BaseTariffCharge));

                        transfers.Add(btTransfer);
                    }

                    var dtTransfer = account.DecisionTariffWallet.TakeMoney(TransferBuilder.Create(
                        account.ToAccount(),
                        new MoneyStream(cancelChargeSource, moneyOperation, cancelCharge.ChangeDate, cancelCharge.SaldoChangeTr)
                        {
                            Description = "Отмена начислений по тарифу решений"
                        }));

                    if (dtTransfer != null)
                    {
                        details.Add(new CancelCharge(
                            cancelChargeSource,
                            account.ToAccount(),
                            cancelPeriod,
                            dtTransfer.Amount,
                            CancelType.DecisionTariffCharge));

                        transfers.Add(dtTransfer);
                    }

                    var pTransfer = account.PenaltyWallet.TakeMoney(TransferBuilder.Create(
                        account.ToAccount(),
                        new MoneyStream(cancelChargeSource, moneyOperation, cancelCharge.ChangeDate, cancelCharge.SaldoChangePeni)
                        {
                            Description = "Отмена начисления пени"
                        }));

                    if (pTransfer != null)
                    {
                        details.Add(new CancelCharge(
                            cancelChargeSource,
                            account.ToAccount(),
                            cancelPeriod,
                            pTransfer.Amount,
                            CancelType.Penalty));

                        transfers.Add(pTransfer);
                    }

                    this.LogImport.CountAddedRows++;
                    this.LogImport.Info("Импорт изменений сальдо",
                        $"На лс {account.PersonalAccountNum} добавлены отмены начислений " +
                        $"по базовому:{cancelCharge.SaldoChangeBase}; по тарифу:{cancelCharge.SaldoChangeTr}; по пени:{cancelCharge.SaldoChangePeni}");
                }

                transfers.ForEach(x => x.TargetCoef = -1);

                TransactionHelper.InsertInManyTransactions(this.Container, transfers, 10000, true, true);
                TransactionHelper.InsertInManyTransactions(this.Container, details, 10000, true, true);

                this.SaveCancelChargeHistory(cancelChargeSource, details);
            }
        }

        private void SaveSaldoChangeHistory(SaldoChangeSource saldoChangeSource, List<SaldoChangeDetail> details)
        {
            var accountChanges = new List<PersonalAccountChange>();

            foreach (var saldoChangeDetail in details)
            {
                var type = PersonalAccountChangeType.Unknown;

                switch (saldoChangeDetail.ChangeType)
                {
                    case WalletType.BaseTariffWallet:
                        type = PersonalAccountChangeType.SaldoBaseChange;
                        break;

                    case WalletType.DecisionTariffWallet:
                        type = PersonalAccountChangeType.SaldoDecisionChange;
                        break;

                    case WalletType.PenaltyWallet:
                        type = PersonalAccountChangeType.SaldoPenaltyChange;
                        break;
                }

                accountChanges.Add(
                    new PersonalAccountChange
                    {
                        PersonalAccount = saldoChangeDetail.PersonalAccount,
                        ChangeType = type,
                        OldValue = saldoChangeDetail.OldValue.ToStr(),
                        NewValue = saldoChangeDetail.NewValue.ToStr(),
                        ActualFrom = saldoChangeSource.OperationDate,
                        Date = saldoChangeSource.OperationDate,
                        Description = $"{saldoChangeSource.ChargeSource.GetDescriptionName()} на {saldoChangeDetail.NewValue}",
                        Operator = saldoChangeSource.User,
                        Document = saldoChangeSource.Document,
                        Reason = saldoChangeSource.Reason,
                        ChargePeriod = saldoChangeSource.Period
                    });
            }

            TransactionHelper.InsertInManyTransactions(this.Container, accountChanges, 10000, true, true);
        }

        private void SaveCancelChargeHistory(ImportCancelChargeSource cancelChargeSource, List<CancelCharge> details)
        {
            var accountChanges = new List<PersonalAccountChange>();

            foreach (var cancelCharge in details)
            {
                var type = PersonalAccountChangeType.Unknown;
                switch (cancelCharge.CancelType)
                {
                    case CancelType.BaseTariffCharge:
                    case CancelType.DecisionTariffCharge:
                        type = PersonalAccountChangeType.ChargeUndo;
                        break;

                    case CancelType.Penalty:
                        type = PersonalAccountChangeType.PenaltyUndo;
                        break;
                }

                accountChanges.Add(
                    new PersonalAccountChange
                    {
                        PersonalAccount = cancelCharge.PersonalAccount,
                        ChangeType = type,
                        OldValue = cancelCharge.CancelSum.ToStr(),
                        NewValue = cancelCharge.CancelSum.ToStr(),
                        ActualFrom = cancelChargeSource.OperationDate,
                        Date = cancelChargeSource.OperationDate,
                        Description = $"{cancelCharge.CancelType.GetDisplayName()} за период {cancelCharge.CancelPeriod.Name} на сумму {cancelCharge.CancelSum}",
                        Operator = cancelChargeSource.User,
                        Document = cancelChargeSource.Document,
                        Reason = cancelChargeSource.Reason,
                        ChargePeriod = cancelChargeSource.Period
                    });
            }

            TransactionHelper.InsertInManyTransactions(this.Container, accountChanges, 10000, true, true);
        }

        private class TransferProxy
        {
            public long AccountId { get; set; }

            public decimal Amount { get; set; }

            public DateTime OperationDate { get; set; }
        }

        private class AccountProxy
        {
            public long Id { get; set; }
            public string PersonalAccountNum { get; set; }
            public Wallet BaseTariffWallet { get; set; }
            public Wallet DecisionTariffWallet { get; set; }
            public Wallet PenaltyWallet { get; set; }

            public BasePersonalAccount ToAccount()
            {
                return new BasePersonalAccount
                {
                    Id = this.Id,
                    PersonalAccountNum = this.PersonalAccountNum
                };
            }
        }
    }
}