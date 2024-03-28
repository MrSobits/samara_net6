namespace Bars.Gkh.RegOperator.DomainService.PersonalOperationAccount
{
    using Bars.B4;
    using Bars.B4.Config;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Modules.Reports;
    using Bars.B4.Modules.Security;
    using Bars.B4.Modules.States;
    using Bars.B4.Utils;

    using Bars.Gkh.Authentification;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Domain.CollectionExtensions;
    using Bars.Gkh.Domain.DatabaseMutex;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Modules.ClaimWork.Entities;
    using Bars.Gkh.RegOperator.Distribution;
    using Bars.Gkh.RegOperator.DomainEvent.Events.PersonalAccount;
    using Bars.Gkh.RegOperator.DomainModelServices;
    using Bars.Gkh.RegOperator.DomainModelServices.PersonalAccount;
    using Bars.Gkh.RegOperator.DomainService.PersonalAccount;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Entities.Dict;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount;
    using Bars.Gkh.RegOperator.Entities.ValueObjects;
    using Bars.Gkh.RegOperator.Enums;
    using Bars.Gkh.Repositories.ChargePeriod;
    using Bars.Gkh.StimulReport;
    using Bars.Gkh.Utils;

    using Castle.Windsor;
    using Domain.Extensions;
    using Gkh.Enums;

    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;

    using Bars.B4.Modules.Analytics.Reports.Enums;

    using EnumerableExtensions = Bars.B4.Modules.Analytics.Reports.Extensions.EnumerableExtensions;

    public class PersonalAccountOperationService : IPersonalAccountOperationService
    {
        public IDomainService<PeriodSummaryBalanceChange> SaldoChangeDomain { get; set; }
        public IDomainService<Debtor> DebtorDomain { get; set; }
        public IDomainService<DebtorClaimWork> DebtorClaimWorkDomain { get; set; }
        public IDomainService<DocumentClw> DocumentClwDomain { get; set; }
        public IDomainService<UnacceptedPayment> UnacceptedPaymentDomain { get; set; }
        public IDomainService<PenaltyChange> PenaltyChangeDomain { get; set; }
        public IDomainService<BasePersonalAccount> PersonalAccountDomain { get; set; }
        public IDomainService<PersonalAccountCharge> PersonalAccountChargeDomain { get; set; }
        public IDomainService<PersonalAccountPayment> PersonalAccountPaymentDomain { get; set; }
        public IDomainService<PersonalAccountPeriodSummary> PeriodSummaryDomain { get; set; }
        public IDomainService<State> StateDomain { get; set; }
        public IDomainService<PersonalAccountChange> AccChangeDomain { get; set; }
        public IDomainService<SuspenseAccountPersonalAccountPayment> SuspAccPaymentDomain { get; set; }
        public IDomainService<RentPaymentIn> RentPaymentDomain { get; set; }
        public IDomainService<CashPaymentCenterPersAcc> CashPaymentCenterPersAccDomain { get; set; }
        public IDomainService<AccumulatedFunds> AccumFundsDomain { get; set; }
        public IDomainService<UnacceptedCharge> UnacceptedChargeDomain { get; set; }
        public IDomainService<PersonalAccountCalculatedParameter> CalcParamDomain { get; set; }
        public IDomainService<ImportedPayment> ImportedPaymentDomain { get; set; }
        public IDomainService<EntityLogLight> EntLogLightService { get; set; }
        public IDomainService<User> UserRepo { get; set; }
        public IDomainService<PersonalAccountPaymentTransfer> TransferDomain { get; set; }
        public IDomainService<PersonalAccountChargeTransfer> ChargeTransferDomain { get; set; }
        public IDomainService<PaymentPenaltiesExcludePersAcc> PaymentPenaltiesExcludePersAccDomain { get; set; }
        public IDomainService<PersonalAccountCalcParam_tmp> CalcParamTmpDomain { get; set; }

        public IStateProvider StateProvider { get; set; }
        public IWindsorContainer Container { get; set; }
        public IGkhUserManager UserManager { get; set; }
        public IChargePeriodRepository ChargePeriodRepo { get; set; }
        public IUserIdentity UserIdentity { get; set; }
        public IFileManager FileManager { get; set; }
        public IPerformedWorkDistribution PerformedWorkDistribution { get; set; }
        public IAccountMassSaldoExportService AccountMassSaldoExportService { get; set; }
        public IConfigProvider ConfigProvider { get; set; }
        public IPersonalAccountRecalcEventManager RecalcEventManager { get; set; }

        /// <summary>
        /// Создать историю операции, проведенной над счетом
        /// </summary>
        /// <param name="account">Лицевой счет</param>
        /// <param name="type">Тип аккаунта</param>
        /// <param name="descriptionFunc">Функция дескрипции</param>
        /// <param name="actualFrom">Дата актуальности</param>
        public void CreateAccountHistory(
            BasePersonalAccount account,
            PersonalAccountChangeType type,
            Func<string> descriptionFunc,
            DateTime? actualFrom = null)
        {
            this.CreateAccountHistoryInternal(account, type, descriptionFunc, actualFrom);
        }

        /// <summary>
        /// Закрыть ЛС
        /// </summary>
        /// <param name="baseParams">Входные параметры</param>
        /// <returns>Результат закрытия</returns>
        public IDataResult ClosePersonalAccount(BaseParams baseParams)
        {
            IDataResult result = new BaseDataResult(false);

            try
            {
                this.Container.InTransaction(
                    () =>
                    {
                        using (new DatabaseMutexContext("Close_Account", "Закрыть ЛС"))
                        {
                            var accId = baseParams.Params.GetAs<long>("accId");
                            var closeDate = baseParams.Params.GetAs<DateTime>("closeDate");
                            var acc = this.PersonalAccountDomain.Get(accId);
                            var resetAreaShare = baseParams.Params.GetAs<bool>("resetAreaShare");

                            var changeInfo = PersonalAccountChangeInfo.FromParams(baseParams);
                            result = this.CloseAccount(acc, PersonalAccountChangeType.Close, closeDate, null, changeInfo, resetAreaShare);
                        }
                    });

                return result;
            }
            catch (DatabaseMutexException)
            {
                return BaseDataResult.Error("Закрыть лицевого счета уже запущен");
            }
        }

        /// <summary>
        /// Закрыть несколько лс
        /// </summary>
        /// <param name="baseParams"></param>
        public IDataResult ClosePersonalAccounts(BaseParams baseParams)
        {
            var accIds = baseParams.Params.GetAs<string>("accIds").ToLongArray();
            var closeDate = baseParams.Params.GetAs<DateTime?>("closeDate");

            var statusId = baseParams.Params.GetAs<long>("status");
            var changeInfo = PersonalAccountChangeInfo.FromParams(baseParams);
            var login = this.UserRepo.Get(this.UserIdentity.UserId).Return(u => u.Login);
            var result = new BaseDataResult();

            try
            {
                this.Container.InTransaction(
                    () =>
                    {
                        using (new DatabaseMutexContext("Close_Account", "Закрыть ЛС"))
                        {
                            var accs = this.PersonalAccountDomain.GetAll().Where(x => accIds.Contains(x.Id));

                            //проверяем на закрытые
                            var existClosedStates = accs
                                .Where(x => x.State.Code == BasePersonalAccount.StateCloseCode || x.State.Code == BasePersonalAccount.StateCloseDebtCode)
                                .Select(x => x.State.Name)
                                .Distinct()
                                .ToList();

                            if (existClosedStates.Count > 0)
                            {
                                var states = EnumerableExtensions.AggregateWithSeparator(existClosedStates, ", ");
                                result = BaseDataResult.Error($"Имеются закрытые счета со статусами: {states}.");
                                return;
                            }

                            //если не передали статус, проставляем его в зависимости от задолженности
                            if (statusId == 0)
                            {
                                var entityInfo = this.StateProvider.GetStatefulEntityInfo(typeof(BasePersonalAccount));

                                var closedState = this.StateDomain.GetAll()
                                    .FirstOrDefault(x => x.TypeId == entityInfo.TypeId && x.Name.ToLower() == "закрыт");
                                var closedWithDebtState = this.StateDomain.GetAll()
                                    .FirstOrDefault(x => x.TypeId == entityInfo.TypeId && x.Name.ToLower() == "закрыт с долгом");

                                var persAccsWithBalanceDict = this.PeriodSummaryDomain.GetAll()
                                    .Where(x => accs.Any(y => y.Id == x.PersonalAccount.Id))
                                    .Select(
                                        x => new
                                        {
                                            x.PersonalAccount,
                                            x.Period,
                                            x.SaldoOut
                                        })
                                    .AsEnumerable()
                                    .GroupBy(x => x.PersonalAccount)
                                    .ToDictionary(x => x.Key, x => x.OrderByDescending(y => y.Period.StartDate).Select(y => y.SaldoOut).First());

                                var persAccsToClose = persAccsWithBalanceDict.Where(x => x.Value <= 0).Select(x => x.Key).ToList();
                                var persAccsToCloseWithDebt = persAccsWithBalanceDict.Where(x => x.Value > 0).Select(x => x.Key).ToList();

                                persAccsToClose.ForEach(
                                    x =>
                                    {
                                        x.SetCloseDate(closeDate ?? DateTime.UtcNow);
                                        x.State = closedState;
                                        x.AreaShare = 0m;
                                        this.PersonalAccountDomain.Update(x);
                                    });

                                persAccsToCloseWithDebt.ForEach(
                                    x =>
                                    {
                                        x.SetCloseDate(closeDate ?? DateTime.UtcNow);
                                        x.State = closedWithDebtState;
                                        x.AreaShare = 0m;
                                        this.PersonalAccountDomain.Update(x);
                                    });
                            }

                            //иначе проставляем указанный статус
                            else
                            {
                                var state = this.StateDomain.GetAll().FirstOrDefault(x => x.Id == statusId);

                                if (state.IsNull())
                                {
                                    result = BaseDataResult.Error("Не найден указанный статус.");
                                    return;
                                }

                                accs.ForEach(
                                    x =>
                                    {
                                        x.SetCloseDate(closeDate ?? DateTime.UtcNow);
                                        x.State = state;
                                        x.AreaShare = 0m;
                                        this.PersonalAccountDomain.Update(x);
                                    });
                            }

                            //создаем логи
                            foreach (var acc in accs)
                            {
                                this.EntLogLightService.Save(
                                    new EntityLogLight
                                    {
                                        ClassName = "BasePersonalAccount",
                                        EntityId = acc.Id,
                                        PropertyDescription = "Изменение доли собственности в связи с закрытием ЛС",
                                        PropertyName = "AreaShare",
                                        PropertyValue = "0",
                                        DateActualChange = closeDate ?? DateTime.UtcNow,
                                        DateApplied = DateTime.UtcNow,
                                        Document = changeInfo != null ? changeInfo.Document : null,
                                        Reason = changeInfo != null ? changeInfo.Reason : null,
                                        ParameterName = "area_share",
                                        User = login.IsEmpty() ? "anonymous" : login
                                    });

                                //создаем запись в истории изменений
                                this.CreateAccountHistoryInternal(acc, PersonalAccountChangeType.Close, null, closeDate, changeInfo);
                            }
                        }
                    });

                return result;
            }
            catch (DatabaseMutexException)
            {
                return BaseDataResult.Error("Закрыть лицевого счета уже запущен");
            }
            catch (Exception e)
            {
                return BaseDataResult.Error(e.Message);
            }
        }

        /// <summary>
        /// Вернуть список статусов для закрытия лс
        /// </summary>
        public List<State> GetCloseStates()
        {
            var result = this.StateDomain.GetAll()
                .Where(x => x.TypeId == "gkh_regop_personal_account" && x.Code == BasePersonalAccount.StateCloseCode)
                .ToList();

            result.Insert(
                0,
                new State
                {
                    Id = 0,
                    Name = "Автоматическое присвоение статуса"
                });

            return result;
        }
        
        public IDataResult TrySetOpenRecord(BaseParams baseParams)
        {
            var MobileAppAccountComparsionDomain = Container.Resolve<IDomainService<MobileAppAccountComparsion>>();

            try
            {
                var docId = baseParams.Params.ContainsKey("docId") ? baseParams.Params["docId"].ToLong() : 0;
                Operator thisOperator = UserManager.GetActiveOperator();
                if (docId > 0)
                {
                    var eds = MobileAppAccountComparsionDomain.Get(docId);
                    if (eds != null)
                    {
                        eds.IsViewed = true;
                        MobileAppAccountComparsionDomain.Update(eds);
                    }


                }

                return new BaseDataResult();
            }
            catch (ValidationException e)
            {
                return new BaseDataResult { Success = false, Message = e.Message };
            }
        }

        /// <summary>
        /// Удаляет персональные счета с указанными идентификаторами
        /// </summary>
        /// <param name="accountIds">Идентификаторы счетом для удаления</param>
        public IDataResult RemoveAccounts(long[] accountIds)
        {
            var accounts = this.PersonalAccountDomain.GetAll().Where(item => accountIds.Contains(item.Id)).ToArray();
            var messages = new StringBuilder();
            var errors = false;

            foreach (var account in accounts)
            {
                if (this.HasTransfers(account))
                {
                    errors = true;
                    messages.AppendFormat(
                        "{0};{1};\n",
                        account.PersonalAccountNum,
                        "На данном счете присутствуют операции. Удаление запрещено");
                    continue;
                }

                using (var transaction = this.Container.Resolve<IDataTransaction>())
                {
                    try
                    {
                        var checkImportedPayment = this.ImportedPaymentDomain.GetAll()
                            .Where(x => x.PersonalAccount.Id == account.Id)
                            .Select(x => x.Id);

                        if (checkImportedPayment.Any())
                        {
                            errors = true;
                            transaction.Rollback();
                            messages.AppendFormat(
                                "{0};{1};\n",
                                account.PersonalAccountNum,
                                "по данному ЛС есть оплата в реестре оплат платежных агентов");

                            continue;
                        }

                        var chargeIds =
                            this.PersonalAccountChargeDomain.GetAll()
                                .Where(item => item.BasePersonalAccount.Id == account.Id)
                                .Select(item => item.Id)
                                .ToList();

                        var parameters = new BaseParams();
                        parameters.Params["records"] = chargeIds;
                        this.PersonalAccountChargeDomain.Delete(parameters);

                        var paymentIds =
                            this.PersonalAccountPaymentDomain.GetAll()
                                .Where(item => item.BasePersonalAccount.Id == account.Id)
                                .Select(item => item.Id)
                                .ToList();
                        parameters.Params["records"] = paymentIds;
                        this.PersonalAccountPaymentDomain.Delete(parameters);

                        var summaryIds = this.PeriodSummaryDomain.GetAll()
                            .Where(item => item.PersonalAccount.Id == account.Id)
                            .Select(item => item.Id)
                            .ToList();
                        parameters.Params["records"] = summaryIds;
                        this.PeriodSummaryDomain.Delete(parameters);

                        var changeIds =
                            this.AccChangeDomain.GetAll()
                                .Where(item => item.PersonalAccount.Id == account.Id)
                                .Select(item => item.Id)
                                .ToList();
                        parameters.Params["records"] = changeIds;
                        this.AccChangeDomain.Delete(parameters);

                        var suspendPaymentIds = this.SuspAccPaymentDomain.GetAll()
                            .Where(item => item.Payment.Id == account.Id)
                            .Select(item => item.Id)
                            .ToList();
                        parameters.Params["records"] = suspendPaymentIds;
                        this.SuspAccPaymentDomain.Delete(parameters);

                        var accumFundsIds =
                            this.AccumFundsDomain.GetAll()
                                .Where(item => item.Account.Id == account.Id)
                                .Select(item => item.Id)
                                .ToList();
                        parameters.Params["records"] = accumFundsIds;
                        this.AccumFundsDomain.Delete(parameters);

                        var penaltyChangeIds = this.PenaltyChangeDomain.GetAll()
                            .Where(item => item.Account.Id == account.Id)
                            .Select(item => item.Id)
                            .ToList();
                        parameters.Params["records"] = penaltyChangeIds;
                        this.PenaltyChangeDomain.Delete(parameters);

                        var rentPaymentIds =
                            this.RentPaymentDomain.GetAll()
                                .Where(item => item.Account.Id == account.Id)
                                .Select(item => item.Id)
                                .ToList();
                        parameters.Params["records"] = rentPaymentIds;
                        this.RentPaymentDomain.Delete(parameters);

                        var saldoChangeIds =
                            this.SaldoChangeDomain.GetAll()
                                .Where(item => item.PeriodSummary.PersonalAccount.Id == account.Id)
                                .Select(item => item.Id)
                                .ToList();
                        parameters.Params["records"] = saldoChangeIds;
                        this.SaldoChangeDomain.Delete(parameters);

                        var unacceptedChargeIds =
                            this.UnacceptedChargeDomain.GetAll()
                                .Where(item => item.PersonalAccount.Id == account.Id)
                                .Select(item => item.Id)
                                .ToList();
                        parameters.Params["records"] = unacceptedChargeIds;
                        this.UnacceptedChargeDomain.Delete(parameters);

                        var calcParamIds = this.CalcParamDomain.GetAll()
                            .Where(item => item.PersonalAccount.Id == account.Id)
                            .Select(item => item.Id)
                            .ToList();
                        parameters.Params["records"] = calcParamIds;
                        this.CalcParamDomain.Delete(parameters);

                        var calcParamTempIds = this.CalcParamTmpDomain.GetAll()
                            .Where(x => x.PersonalAccount.Id == account.Id)
                            .Select(x => x.Id)
                            .ToList();
                        parameters.Params["records"] = calcParamTempIds;
                        this.CalcParamTmpDomain.Delete(parameters);

                        this.DebtorDomain.GetAll()
                            .Where(x => x.PersonalAccount.Id == account.Id)
                            .Select(x => x.Id)
                            .ForEach(x => this.DebtorDomain.Delete(x));

                        this.UnacceptedPaymentDomain.GetAll()
                            .Where(x => x.PersonalAccount.Id == account.Id)
                            .Select(x => x.Id)
                            .ForEach(x => this.UnacceptedPaymentDomain.Delete(x));

                        this.PaymentPenaltiesExcludePersAccDomain.GetAll()
                            .Where(x => x.PersonalAccount.Id == account.Id)
                            .Select(x => x.Id)
                            .ForEach(x => this.PaymentPenaltiesExcludePersAccDomain.Delete(x));

                        this.CashPaymentCenterPersAccDomain.GetAll()
                            .Where(x => x.PersonalAccount.Id == account.Id)
                            .Select(x => x.Id)
                            .ForEach(x => this.CashPaymentCenterPersAccDomain.Delete(x));

                        this.PersonalAccountDomain.Delete(account.Id);

                        transaction.Commit();
                    }
                    catch (Exception e)
                    {
                        errors = true;
                        transaction.Rollback();
                        messages.AppendFormat(
                            "{0};{1}: {2};\n",
                            account.PersonalAccountNum,
                            "В процессе удаления ЛС произошла ошибка",
                            e.Message);

                        continue;
                    }
                }

                messages.AppendFormat(
                    "{0};{1};\n",
                    account.PersonalAccountNum,
                    "Удаление прошло успешно.");
            }

            using (var fileLog = new MemoryStream())
            {
                this.WriteLog(string.Format("{0};{1};\n", "Номер лицевого счета", "Описание"), fileLog);
                this.WriteLog(messages.ToString(), fileLog);

                var sessionProvider = this.Container.Resolve<ISessionProvider>();
                using (this.Container.Using(sessionProvider))
                {
                    sessionProvider.CurrentSession.Clear();

                    var fileData = this.FileManager.SaveFile(fileLog, "Удаление ЛС.log.csv");
                    return new BaseDataResult(new { Errors = errors, LogfileId = fileData.Id });
                }
            }
        }

        /// <summary>
        /// Применение распределения зачета средств за ранее выполненные работы
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns>Результат выполнения запроса</returns>
        public IDataResult ApplyPerformedWorkDistribution(BaseParams baseParams)
        {
            var result = this.PerformedWorkDistribution.Apply(baseParams);
            return result;
        }

        public IDataResult ListPersAccWithRestructAmAgreement(BaseParams baseParams)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Экспортировать сальдо ЛС
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>Результат</returns>
        public IDataResult ExportExcelSaldo(BaseParams baseParams)
        {
            try
            {
                return this.AccountMassSaldoExportService.ExportSaldo(baseParams);
            }
            catch (Exception exception)
            {
                return BaseDataResult.Error(exception.Message);
            }
        }

        /// <summary>
        /// Получение информации по лицевым счетам для зачета средств за ранее выполненные работы
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns>Результат выполнения</returns>
        public IDataResult GetAccountsInfoForPerformedWorkDistribution(BaseParams baseParams)
        {
            var result = this.PerformedWorkDistribution.GetAccountsInfo(baseParams);

            return result;
        }

        /// <summary>
        /// Получить долю собственности по ЛС
        /// </summary>
        /// <param name="baseParams">Входные параметры</param>
        /// <returns>Доля собственности</returns>
        public IDataResult RoomAccounts(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();
            var accId = loadParams.Filter.GetAs<long>("accId");

            var acc = this.PersonalAccountDomain.Get(accId);

            var data = this.PersonalAccountDomain.GetAll()
                .Where(x => x.Room.Id == acc.Room.Id && x.Id != accId)
                .Select(x => new { x.Id, x.PersonalAccountNum, x.AreaShare, NewShare = 0 })
                .ToList();

            return new ListDataResult(data, data.Count);
        }

        /// <summary>
        /// Получить счёта по дому
        /// </summary>
        /// <param name="baseParams">Входные параметры</param>
        /// <returns>Список счетов</returns>
        public IDataResult GetAccountByRealtyObject(BaseParams baseParams)
        {
            var roId = baseParams.Params.GetAs<long>("roId");

            var loadParams = baseParams.GetLoadParam();

            var result = this.PersonalAccountDomain.GetAll()
                .Where(x => x.Room.RealityObject.Id == roId)
                .Select(x => new { x.Id, x.AccountOwner })
                .ToArray()
                .GroupBy(x => x.AccountOwner.Id)
                .Select(x => x.First())
                .Select(
                    x => new
                    {
                        x.Id,
                        AccountOwnerName =
                        x.AccountOwner.OwnerType == PersonalAccountOwnerType.Legal
                            ? (x.AccountOwner as LegalAccountOwner).Contragent.Name
                            : x.AccountOwner.Name
                    })
                .AsQueryable()
                .Filter(loadParams, this.Container);

            return new ListDataResult(result.Order(loadParams).ToArray(), result.Count());
        }

        /// <summary>
        /// Закрытие счета
        /// </summary>
        /// <param name="acc"></param>
        /// <param name="type"></param>
        /// <param name="closeDate"></param>
        /// <param name="descriptionFunc"></param>
        public IDataResult CloseAccount(
            BasePersonalAccount acc,
            PersonalAccountChangeType type,
            DateTime? closeDate = null,
            Func<string> descriptionFunc = null,
            PersonalAccountChangeInfo changeInfo = null,
            bool resetAreaShare = false)
        {
            if (acc.State.FinalState)
            {
                return new BaseDataResult(false, "Счет уже закрыт!");
            }

            if (acc.AreaShare > 0)
            {
                var login = this.UserRepo.Get(this.UserIdentity.UserId).Return(u => u.Login);

                this.EntLogLightService.Save(
                    new EntityLogLight
                    {
                        ClassName = "BasePersonalAccount",
                        EntityId = acc.Id,
                        PropertyDescription = "Изменение доли собственности в связи с закрытием ЛС",
                        PropertyName = "AreaShare",
                        PropertyValue = "0",
                        DateActualChange = closeDate ?? DateTime.UtcNow,
                        DateApplied = DateTime.UtcNow,
                        Document = changeInfo != null ? changeInfo.Document : null,
                        Reason = changeInfo != null ? changeInfo.Reason : null,
                        ParameterName = "area_share",
                        User = login.IsEmpty() ? "anonymous" : login
                    });

                acc.AreaShare = 0;
            }

            var summaries = acc.Summaries.ToList();
            var debtTotal = summaries.SafeSum(x => x.GetTotalCharge() - x.GetTotalPayment()).RegopRoundDecimal(2);

            var entityInfo = this.StateProvider.GetStatefulEntityInfo(typeof(BasePersonalAccount));

            if (type != PersonalAccountChangeType.Close)
            {
                var state = this.StateDomain.GetAll()
                    .FirstOrDefault(x => x.FinalState && x.TypeId == entityInfo.TypeId && x.Name.ToLower() == "закрыт");
                acc.State = state;
            }

            if (debtTotal != 0)
            {
                if (acc.State.Name != null && (acc.State.Code == BasePersonalAccount.StateCloseCode
                    || acc.State.Code == BasePersonalAccount.StateCloseDebtCode))
                {
                    return new BaseDataResult(false, "Имеется долг. Счет уже закрыт с долгом!");
                }

                var closeState = this.StateDomain.GetAll()
                    .FirstOrDefault(x => x.TypeId == entityInfo.TypeId && x.Name.ToLower() == "закрыт с долгом");

                if (closeState == null)
                {
                    return new BaseDataResult(
                        false,
                        "Для лицевого счета не определен статус с названием \"Закрыт с долгом\"");
                }

                acc.State = closeState;
            }
            else
            {
                var closeState = this.StateDomain.GetAll()
                    .FirstOrDefault(x => x.FinalState && x.TypeId == entityInfo.TypeId && x.Name.ToLower() == "закрыт");

                if (closeState == null)
                {
                    return new BaseDataResult(
                        false,
                        "Для лицевого счета не определен конечный статус с названием \"Закрыт\"");
                }

                acc.State = closeState;
            }

            acc.SetCloseDate(closeDate ?? DateTime.Now);

            this.CreateAccountHistoryInternal(
                acc,
                type,
                descriptionFunc ?? (() => string.Format("Для ЛС установлен статус \"{0}\"", acc.State.Name)),
                acc.CloseDate,
                changeInfo);

            this.PersonalAccountDomain.Update(acc);

            return new BaseDataResult(true, "Счет закрыт успешно!");
        }

        /// <summary>
        /// Сохранение легковесное сущность для хранения изменения ЛС Для установки статуса "Не активен\"",
        /// </summary>
        /// <param name="accountId">Id ЛС</param>
        /// <param name="dateActualChange">Дата начала действия значения</param>
        /// <param name="dateEnd">Дата окончания действия нового значения</param>
        /// <param name="reason">Причина</param>
        public void LogAccountDeactivate(BasePersonalAccount account, DateTime dateActualChange, DateTime dateEnd, string reason)
        {
            this.AccChangeDomain.Save(new PersonalAccountChange
            {
                PersonalAccount = account,
                ChangeType = PersonalAccountChangeType.NonActive,
                Date = DateTime.Now,
                Description = "Для ЛС установлен статус \"Не активен\"",
                ActualFrom = DateTime.Now,
                NewValue = "Статус ЛС = Не активен",
                Reason = reason,
                ChargePeriod = this.ChargePeriodRepo.GetCurrentPeriod(),
                Operator = this.UserManager.GetActiveUser()?.Login ?? ""
            });
        }

        /// <summary>
        /// Массовое закрытие счетов
        /// </summary>
        /// <param name="realObjs"></param>
        /// <param name="type"></param>
        /// <param name="closeDate"></param>
        /// <param name="descriptionFunc"></param>
        public void MassClosingAccounts(
            long[] realObjs,
            PersonalAccountChangeType type,
            DateTime? closeDate = null,
            Func<string> descriptionFunc = null)
        {
            var logEntityLogOperation = this.Container.Resolve<IDomainService<LogOperation>>();
            var logOperation = new LogOperation
            {
                StartDate = DateTime.UtcNow,
                OperationType = LogOperationType.StateChangeToEmergency,
                User = this.UserManager.GetActiveUser()
            };

            StreamWriter logFile = new StreamWriter(new MemoryStream(), Encoding.GetEncoding(1251));
            try
            {
                logFile.WriteLine("Номер счёта;исходное состояние;финальное состояние;адрес;время");
                var entityInfo = this.StateProvider.GetStatefulEntityInfo(typeof(BasePersonalAccount));
                var closedState = this.StateDomain.GetAll()
                    .FirstOrDefault(x => x.TypeId == entityInfo.TypeId && x.Name.ToLower() == "закрыт");
                var closedWithDebtState = this.StateDomain.GetAll()
                    .FirstOrDefault(x => x.TypeId == entityInfo.TypeId && x.Name.ToLower() == "закрыт с долгом");

                if (closedState == null || closedWithDebtState == null)
                {
                    return;
                }

                var persAccs = this.PersonalAccountDomain.GetAll()
                    .Where(x => realObjs.Contains(x.Room.RealityObject.Id))
                    .Where(x => x.State.Id != closedState.Id && x.State.Id != closedWithDebtState.Id);

                var persAccsWithBalanceDict = this.PeriodSummaryDomain.GetAll()
                    .Where(x => persAccs.Any(y => y.Id == x.PersonalAccount.Id))
                    .ToList()
                    .Select(
                        x => new
                        {
                            x.PersonalAccount,
                            TotalCharge = x.GetTotalCharge(),
                            TotalPayment = x.GetTotalPayment()
                        })
                    .GroupBy(x => x.PersonalAccount)
                    .ToDictionary(x => x.Key, x => x.SafeSum(y => y.TotalCharge - y.TotalPayment).RegopRoundDecimal(2));

                var persAccsToClose = persAccsWithBalanceDict.Where(x => x.Value == 0).Select(x => x.Key).ToList();
                var persAccsToCloseWithDebt = persAccsWithBalanceDict.Where(x => x.Value != 0).Select(x => x.Key).ToList();

                int closeWithoutDebt = 0;
                persAccsToClose.ForEach(
                    x =>
                    {
                        var beginningState = x.State;
                        x.SetCloseDate(closeDate ?? DateTime.Now);
                        x.State = closedState;
                        this.PersonalAccountDomain.Update(x);
                        logFile.WriteLine(
                            "{0};{1};{2};{3};{4}",
                            x.PersonalAccountNum,
                            beginningState.Name,
                            closedState.Name,
                            x.Room.RealityObject.Address,
                            x.CloseDate);
                        closeWithoutDebt++;
                    });

                int closeWithDebt = 0;
                persAccsToCloseWithDebt.ForEach(
                    x =>
                    {
                        var beginningState = x.State;
                        x.SetCloseDate(closeDate ?? DateTime.Now);
                        x.State = closedWithDebtState;
                        this.PersonalAccountDomain.Update(x);
                        logFile.WriteLine(
                            "{0};{1};{2};{3};{4}",
                            x.PersonalAccountNum,
                            beginningState.Name,
                            closedWithDebtState.Name,
                            x.Room.RealityObject.Address,
                            x.CloseDate);
                        closeWithDebt++;
                    });

                logOperation.EndDate = DateTime.UtcNow;
                logFile.Flush();
                logOperation.Comment = String.Format(
                    "Дата операции: " + DateTime.UtcNow.ToShortDateString() + "; Закрыто без долга: {0}; Закрыто с долгом: {1}",
                    closeWithoutDebt,
                    closeWithDebt);
                var logFileInfo = this.FileManager.SaveFile(logFile.BaseStream, $"Report.csv");
                logOperation.LogFile = logFileInfo;
                logEntityLogOperation.Save(logOperation);
                this.RecalcEventManager.SaveEvents();

                this.MassCreatingPersAccHistory(persAccsWithBalanceDict.Keys.ToList(), type, descriptionFunc, closeDate);
            }
            catch(Exception e)
            {
                logOperation.Comment = "Ошибка: "+e.Message;
                logFile.Flush();
                logEntityLogOperation.Save(logOperation);
            }
            finally
            {            
                logFile.Close();
            }
        }

        private void MassCreatingPersAccHistory(
            List<BasePersonalAccount> accounts,
            PersonalAccountChangeType type,
            Func<string> descriptionFunc,
            DateTime? actualFrom = null)
        {
            accounts.ForEach(
                x =>
                {
                    this.AccChangeDomain.Save(
                        new PersonalAccountChange
                        {
                            PersonalAccount = x,
                            ChangeType = type,
                            Date = DateTime.UtcNow,
                            Description = descriptionFunc.Return(y => y(), string.Empty),
                            Operator = this.UserManager.GetActiveUser().Return(y => y.Login),
                            ActualFrom = actualFrom,
                            ChargePeriod = this.ChargePeriodRepo.GetCurrentPeriod()
                        });
                });
        }

        private void CreateAccountHistoryInternal(
            BasePersonalAccount account,
            PersonalAccountChangeType type,
            Func<string> descriptionFunc,
            DateTime? actualFrom = null,
            PersonalAccountChangeInfo changeInfo = null)
        {
            this.Container.UsingForResolved<IDataTransaction>(
                (c, tr) =>
                {
                    this.AccChangeDomain.Save(
                        new PersonalAccountChange
                        {
                            PersonalAccount = account,
                            ChangeType = type,
                            Date = DateTime.UtcNow,
                            Description = descriptionFunc.Return(x => x(), string.Empty),
                            Operator = this.UserManager.GetActiveUser().Return(x => x.Login),
                            ActualFrom = actualFrom,
                            Document = changeInfo.With(ci => ci.Document),
                            Reason = changeInfo.With(ci => ci.Reason),
                            ChargePeriod = this.ChargePeriodRepo.GetCurrentPeriod()
                        });

                    tr.Commit();
                });
        }

        private Stream GenerateStimulReport(IBaseReport printForm)
        {
            var reportParams = new ReportParams();
            var report = printForm as StimulReport;
            report.ExportFormat = StiExportFormat.Excel2007;
            if (printForm != null)
            {
                printForm.PrepareReport(reportParams);
                report.Open(printForm.GetTemplate());
            }

            var stream = new MemoryStream();
            report.Generate(stream, reportParams);
            stream.Seek(0, SeekOrigin.Begin);
            return stream;
        }

        private bool HasTransfers(BasePersonalAccount account)
        {
            return this.TransferDomain.GetAll().Any(x => x.Owner == account) || this.ChargeTransferDomain.GetAll().Any(x => x.Owner == account);
        }

        private void WriteLog(string message, Stream fileLog)
        {
            var msg = Encoding.GetEncoding(1251).GetBytes(message);
            fileLog.Write(msg, 0, msg.Length);
        }
    }
}