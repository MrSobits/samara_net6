namespace Bars.Gkh.RegOperator.DomainService.PersonalAccount.Impl
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Modules.Analytics.Reports.Utils;
    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Utils;
    using Bars.B4.Utils.Annotations;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Domain;
    using Bars.Gkh.DomainEvent.Infrastructure;
    using Bars.Gkh.Entities;
    using Bars.Gkh.RegOperator.DataProviders.Meta;
    using Bars.Gkh.RegOperator.Domain;
    using Bars.Gkh.RegOperator.Domain.Extensions;
    using Bars.Gkh.RegOperator.Domain.Repository;
    using Bars.Gkh.RegOperator.DomainEvent;
    using Bars.Gkh.RegOperator.DomainEvent.Events.PersonalAccount;
    using Bars.Gkh.RegOperator.DomainModelServices;
    using Bars.Gkh.RegOperator.DomainModelServices.PersonalAccount;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount.Operations;
    using Bars.Gkh.RegOperator.Entities.ValueObjects;
    using Bars.Gkh.RegOperator.Enums;
    using Bars.Gkh.RegOperator.Report;
    using Bars.Gkh.Repositories.ChargePeriod;
    using Bars.GkhExcel;

    using Castle.Windsor;

    using FileInfo = Bars.B4.Modules.FileStorage.FileInfo;

    /// <summary>
    /// Сервис массового изменения сальдо
    /// </summary>
    public class AccountSaldoChangeService : IAccountSaldoChangeService, IAccountMassSaldoExportService
    {
        private Action<int, string> progressIndicator;

        #region Public properties
        /// <summary>
        /// Контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Сервис фильтрации ЛС
        /// </summary>
        public IPersonalAccountFilterService PersonalAccountFilterService { get; set; }

        /// <summary>
        /// Репозиторий периода начислений
        /// </summary>
        public IChargePeriodRepository ChargePeriodRepository { get; set; }

        /// <summary>
        /// Менеджер пользователей
        /// </summary>
        public IGkhUserManager UserManager { get; set; }

        /// <summary>
        /// Домен-сервис <see cref="SaldoChangeExport"/>
        /// </summary>
        public IDomainService<SaldoChangeExport> SaldoChangeExportDomain { get; set; }

        /// <summary>
        /// Домен-сервис <see cref="BasePersonalAccount"/>
        /// </summary>
        public IDomainService<BasePersonalAccount> PersonalAccountDomain { get; set; }

        /// <summary>
        /// Домен-сервис <see cref="PersonalAccountPeriodSummary"/>
        /// </summary>
        public IDomainService<PersonalAccountPeriodSummary> PersonalAccountPeriodSummaryDomain { get; set; }

        /// <summary>
        /// Домен-сервис <see cref="Transfer"/>
        /// </summary>
        public IDomainService<Transfer> TransferDomain { get; set; }

        /// <summary>
        /// Домен-сервис <see cref="MoneyOperation"/>
        /// </summary>
        public IDomainService<MoneyOperation> MoneyOperationDomain { get; set; }

        /// <summary>
        /// Домен-сервис <see cref="SaldoChangeSource"/>
        /// </summary>
        public IDomainService<SaldoChangeSource> SaldoChangeSourceDomain { get; set; }

        /// <summary>
        /// Сессия счета оплат дома
        /// </summary>
        public IRealtyObjectPaymentSession RealtyObjectPaymentSession { get; set; }

        /// <summary>
        /// Менеджер файлов
        /// </summary>
        public IFileManager FileManager { get; set; }

        /// <summary>
        /// Интерфейс создания запрета перерасчета
        /// </summary>
        public IPersonalAccountBanRecalcManager PersonalAccountBanRecalcManager { get; set; }
        #endregion

        /// <summary>
        /// Экспортировать сальдо ЛС
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>Результат</returns>
        public ReportResult ExportSaldo(BaseParams baseParams)
        {
            var query = this.GetPersonalAccounts<PersonalAccountSaldoInfo>(baseParams);
            var currentPeriod = this.ChargePeriodRepository.GetCurrentPeriod(false);

            var saldoChangeExport = new SaldoChangeExport(currentPeriod);

            query.ForEach(
                x => saldoChangeExport.AddPersonalAccount(
                    this.PersonalAccountDomain.Load(x.Id),
                    x.SaldoByBaseTariff,
                    x.SaldoByDecisionTariff,
                    x.SaldoByPenalty));

            using (var transaction = this.Container.Resolve<IDataTransaction>())
            {
                try
                {
                    this.SaldoChangeExportDomain.Save(saldoChangeExport);
                    transaction.Commit();

                    MemoryStream outputStream;

                    var excelProvider = this.Container.Resolve<IGkhExcelProvider>("ExcelEngineProvider");
                    using (this.Container.Using(excelProvider))
                    {
                        if (excelProvider == null)
                        {
                            throw new Exception("Не найдена реализация интерфейса IGkhExcelProvider");
                        }

                        var report = new AccountSaldoCustomExcelReport(excelProvider, query);
                        outputStream = report.GetReportStream();
                    }
                    
                    return new ReportResult
                    {
                        ReportStream = outputStream,
                        FileName = saldoChangeExport.FileName + ".xlsx"
                    };
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        /// <summary>
        /// Обработать импортированные изменения
        /// </summary>
        /// <param name="saldoChangeCreator">
        /// Инициатор изменения сальдо
        /// </param>
        /// <param name="changeData">
        /// Список изменений сальдо
        /// </param>
        /// <returns>
        /// The <see cref="IDataResult"/>.
        /// </returns>
        public IDataResult ProcessSaldoChange(ISaldoChangeCreator saldoChangeCreator, IList<ISaldoChangeData> changeData)
        {
            var saldoChangeOriginator = saldoChangeCreator.CreateSaldoChangeOperation(this.UserManager.GetActiveUser()?.Login ?? "anonymous");

            var listToSave = new List<PersistentObject>
            {
                saldoChangeOriginator,
                saldoChangeOriginator.Operation
            };

            var current = 1;
            try
            {
                foreach (var change in changeData)
                {
                    var result = saldoChangeOriginator.ApplyChange(change);

                    listToSave.AddRange(result);
                    listToSave.Add(change.PersonalAccount);
                    listToSave.Add(change.PersonalAccount.GetOpenedPeriodSummary());

                    var isCharge = change.SaldoByBaseTariff - change.NewSaldoByBaseTariff != 0
                        || change.SaldoByDecisionTariff - change.NewSaldoByDecisionTariff != 0
                            ? BanRecalcType.Charge
                            : 0;
                    var isPenalty = change.SaldoByPenalty - change.NewSaldoByPenalty != 0 ? BanRecalcType.Penalty : 0;
                    var type = isCharge | isPenalty;

                    if (Enum.IsDefined(typeof(BanRecalcType), type))
                    {
                        this.PersonalAccountBanRecalcManager.CreateBanRecalc(
                            change.PersonalAccount,
                            saldoChangeOriginator.Period.StartDate,
                            saldoChangeOriginator.Period.GetEndDate(),
                            type,
                            saldoChangeOriginator.Document,
                            saldoChangeOriginator.Reason);
                    }

                    var percent = Math.Min(100, (int)((float)current / changeData.Count * 100));
                    this.progressIndicator?.Invoke(percent, $"Обработано {++current} из {changeData.Count} записей");
                }

                listToSave.AddRange(this.GenerateHistory(saldoChangeOriginator));

                this.progressIndicator?.Invoke(100, "Сохранение записей");

                this.PersonalAccountBanRecalcManager.SaveBanRecalcs();

                // применяем изменения на домах
                this.RealtyObjectPaymentSession.Complete();
                TransactionHelper.InsertInManyTransactions(this.Container, listToSave);
            }
            catch (Exception)
            {
                this.RealtyObjectPaymentSession.Rollback();
                throw;
            }

            return new BaseDataResult();
        }

        /// <summary>
        /// Обработать импортированные изменения
        /// </summary>
        /// <param name="baseParams">
        /// Параметры запроса
        /// </param>
        /// <returns>
        /// The <see cref="IDataResult"/>.
        /// </returns>
        public IDataResult ProcessSaldoChange(BaseParams baseParams)
        {
            var changeData = this.ExtractChangeData(baseParams);
            var reason = baseParams.Params.GetAs("Reason", string.Empty);
            var openedPeriod = this.ChargePeriodRepository.GetCurrentPeriod();

            IDataResult result = new BaseDataResult();
            this.Container.InTransaction(() =>
            {
                FileInfo fileInfo = null;
                if (baseParams.Files.Any())
                {
                    var file = baseParams.Files.FirstOrDefault().Value;
                    fileInfo = this.FileManager.SaveFile(file.FileName, file.Extention, file.Data);
                }

                var saldoChange = new MassSaldoChangeCreator(openedPeriod, reason, fileInfo);
                result = this.ProcessSaldoChange(saldoChange, changeData);

            });

            return result;
        }

        /// <summary>
        /// Изменение баланса за период по базовому тарифу и тарифу решения
        /// </summary>
        /// <param name="account"> Лицевой счет </param>
        ///  <param name="newValue"> Новое значение баланса </param>
        /// <param name="document"> Документ-основание </param>
        /// <param name="reason"> Причина </param>
        /// <returns>The <see cref="IDataResult"/>. </returns>
        public IDataResult ProcessSaldoChange(BasePersonalAccount account, decimal newValue, FileData document, string reason)
        {
            ArgumentChecker.NotNull(account, nameof(account));
            var period = this.ChargePeriodRepository.GetCurrentPeriod();

            FileInfo fileInfo = null;

            if (document != null)
            {
                fileInfo = this.FileManager.SaveFile(document);
            }

            var saldoChangeSource = new SaldoChangeSource(TypeChargeSource.BalanceChange, period)
            {
                User = this.UserManager.GetActiveUser()?.Login ?? "anonymous",
                OperationDate = DateTime.Now,
                Document = fileInfo,
                Reason = reason
            };

            var summary = account.GetOpenedPeriodSummary();

            var baseTariffDebt = summary.BaseTariffDebt + summary.GetBaseTariffDebt();
            var decisionTariffDebt = summary.DecisionTariffDebt + summary.GetDecisionTariffDebt();

            var saldoOut = baseTariffDebt + decisionTariffDebt; // задолженность по базовому + решения
            var delta = newValue - saldoOut;                    // общее изменение сальдо

            // Общий алгоритм изменения сальдо:
            // вначале пытаемся свести к 0 базовый тариф (вне зависимости от знака)
            // потом - тариф решений
            // остаток снова кидаем на базовый
            var before = Math.Sign(baseTariffDebt);

            var newBaseTariffDebt = baseTariffDebt + delta;
            var after = Math.Sign(newBaseTariffDebt);

            //если знаки задолженности разные до и после, то перескочили 0
            if (after != before)
            {
                newBaseTariffDebt = 0;
                delta += baseTariffDebt; //остаток после зануления
            }
            else
            {
                delta = 0;
            }

            var newDecisionTariffDebt = decisionTariffDebt;

            // если задолженность по тарифу решения != 0
            // то пытаемся ее аналогично занулить
            if (decisionTariffDebt != 0 && delta != 0)
            {
                before = Math.Sign(decisionTariffDebt);
                               
                newDecisionTariffDebt = decisionTariffDebt + delta;
                after = Math.Sign(newDecisionTariffDebt);

                if (after != before)
                {
                    newDecisionTariffDebt = 0;
                    delta += decisionTariffDebt;
                }
                else
                {
                    delta = 0;
                }
            }

            // остаток сажаем на базовый
            newBaseTariffDebt += delta;

            var transfers = saldoChangeSource.ApplyChange(new SaldoChangeData
            {
                PersonalAccount = account,

                SaldoByBaseTariff = baseTariffDebt,
                SaldoByDecisionTariff = decisionTariffDebt,

                NewSaldoByBaseTariff = newBaseTariffDebt,
                NewSaldoByDecisionTariff = newDecisionTariffDebt,
            });

            this.PersonalAccountBanRecalcManager.CreateBanRecalc(
                        account,
                        saldoChangeSource.Period.StartDate,
                        saldoChangeSource.Period.GetEndDate(),
                        BanRecalcType.Charge,
                        saldoChangeSource.Document,
                        saldoChangeSource.Reason);

            this.SaldoChangeSourceDomain.Save(saldoChangeSource);
            this.PersonalAccountPeriodSummaryDomain.Update(summary);
            this.MoneyOperationDomain.Save(saldoChangeSource.Operation);
            transfers.ForEach(this.TransferDomain.Save);
            this.RealtyObjectPaymentSession.Complete();
            this.PersonalAccountBanRecalcManager.SaveBanRecalcs();

            // генерируем события изменения
            saldoChangeSource.ChangeDetails.ForEach(x =>
                    DomainEvents.Raise(
                        new PersonalAccountBalanceChangeEvent(
                            account,
                            saldoChangeSource.OperationDate,
                            x.ChangeType,
                            x.OldValue,
                            x.NewValue,
                              null,
                            reason)));

            return new BaseDataResult();
        }

        /// <summary>
        /// Добавить обработку прогресса
        /// </summary>
        /// <param name="progressIndicatorMethod">
        /// Метод индикации процесса обработки
        /// </param>
        public void SetProgressIndicator(Action<int, string> progressIndicatorMethod)
        {
            this.progressIndicator = progressIndicatorMethod;
        }

        /// <summary>
        /// Вернуть информацию о лицевых счетах
        /// </summary>
        /// <typeparam name="T">Тип данных об лс</typeparam>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns></returns>
        public IList<T> GetPersonalAccounts<T>(BaseParams baseParams) where T : PersonalAccountSaldoInfo, new()
        {
            var accIds = baseParams.Params.GetAs<long[]>("accIds");
            var loadParams = baseParams.GetLoadParam();
            var currentPeriod = this.ChargePeriodRepository.GetCurrentPeriod();

            var queryDto = this.PersonalAccountDomain.GetAll().ToDto();

            if (accIds.IsNotEmpty())
            {
                queryDto = queryDto
                    .Where(x => accIds.Contains(x.Id))
                    .Order(loadParams)
                    .OrderIf(loadParams.Order.IsEmpty(), true, x => x.PersonalAccountNum);
            }
            else
            {
                queryDto = queryDto
                .FilterByBaseParams(baseParams, this.PersonalAccountFilterService)
                .Filter(baseParams.GetLoadParam(), this.Container)
                .Order(loadParams)
                .OrderIf(loadParams.Order.IsEmpty(), true, x => x.PersonalAccountNum);
            }

            var summaries = this.PersonalAccountPeriodSummaryDomain.GetAll()
                .Where(x => x.Period.Id == currentPeriod.Id && queryDto.Any(y => y.Id == x.PersonalAccount.Id))
                .Select(x => new
                {
                    x.PersonalAccount.Id,
                    SaldoByBaseTariff = x.BaseTariffDebt + x.ChargedByBaseTariff + x.RecalcByBaseTariff + x.BaseTariffChange - x.TariffPayment - x.PerformedWorkChargedBase,

                    SaldoByDecisionTariff = x.DecisionTariffDebt + (x.ChargeTariff - x.ChargedByBaseTariff) + x.RecalcByDecisionTariff
                                          + x.DecisionTariffChange - x.TariffDecisionPayment - x.PerformedWorkChargedDecision,

                    SaldoByPenalty = x.PenaltyDebt + x.Penalty + x.RecalcByPenalty + x.PenaltyChange - x.PenaltyPayment
                })
                .AsEnumerable()
                .ToDictionary(x => x.Id);

            return queryDto.AsEnumerable()
                .Select(
                    x => new T
                    {
                        Id = x.Id,
                        AccountNumber = x.PersonalAccountNum,
                        Address = x.Address,
                        Municipality = x.Municipality,
                        State = x.State.Name,
                        SaldoByBaseTariff = summaries.Get(x.Id)?.SaldoByBaseTariff ?? 0,
                        SaldoByDecisionTariff = summaries.Get(x.Id)?.SaldoByDecisionTariff ?? 0,
                        SaldoByPenalty = summaries.Get(x.Id)?.SaldoByPenalty ?? 0
                    })
                .ToList();
        }

        private IEnumerable<PersonalAccountChange> GenerateHistory(SaldoChangeSource saldoChangeSource)
        {
            foreach (var saldoChangeDetail in saldoChangeSource.ChangeDetails)
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

                yield return new PersonalAccountChange
                {
                    PersonalAccount = saldoChangeDetail.PersonalAccount,
                    ChangeType = type,
                    OldValue = saldoChangeDetail.OldValue.ToStr(),
                    NewValue = saldoChangeDetail.NewValue.ToStr(),
                    ActualFrom = saldoChangeSource.OperationDate,
                    Date = saldoChangeSource.OperationDate,
                    Description = $"{this.GetDescription(saldoChangeSource)} с {saldoChangeDetail.OldValue} на {saldoChangeDetail.NewValue}",
                    Operator = saldoChangeSource.User,
                    Document = saldoChangeSource.Document,
                    Reason = saldoChangeSource.Reason,
                    ChargePeriod = this.ChargePeriodRepository.GetCurrentPeriod()
                };
            }
        }

        private string GetDescription(SaldoChangeSource saldoChangeSource)
        {
            return saldoChangeSource.ChargeSource.GetDescriptionName() + " ";
        }

        private IList<ISaldoChangeData> ExtractChangeData(BaseParams baseParams)
        {
            var resultList = baseParams.Params.GetAs<List<SaldoChangeData>>("modifRecs").Select(
                x =>
                {
                    x.PersonalAccount = this.PersonalAccountDomain.Load(x.Id);
                    return x;
                });

            return resultList.Cast<ISaldoChangeData>().ToList();
        }

        private class MassSaldoChangeCreator : ISaldoChangeCreator
        {
            private readonly ChargePeriod period;
            private readonly string reason;
            private readonly FileInfo file;

            public MassSaldoChangeCreator(ChargePeriod period, string reason, FileInfo file)
            {
                this.period = period;
                this.reason = reason;
                this.file = file;
            }

            public SaldoChangeSource CreateSaldoChangeOperation(string userName)
            {
                return new SaldoChangeSource(TypeChargeSource.SaldoChangeMass, this.period)
                {
                    User = userName,
                    OperationDate = DateTime.Now,
                    Reason = this.reason,
                    Document = this.file
                };
            }
        }       
    }
}