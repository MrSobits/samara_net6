namespace Bars.Gkh.RegOperator.Distribution.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Utils;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Domain.CollectionExtensions;
    using Bars.Gkh.Domain.DatabaseMutex;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount;
    using Bars.Gkh.RegOperator.Entities.ValueObjects;
    using Bars.Gkh.RegOperator.Enums;
    using Bars.Gkh.Repositories.ChargePeriod;

    using Castle.Windsor;

    /// <summary>
    /// Распределение средств за ранее выполненные работы
    /// </summary>
    public class PerformedWorkDistribution : IPerformedWorkDistribution
    {
        /// <summary>
        /// Контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Домен-сервис л/с
        /// </summary>
        public IDomainService<BasePersonalAccount> PersonalAccountDomain { get; set; }

        /// <summary>
        /// Интерфейс репозитория Период начислений
        /// </summary>
        public IChargePeriodRepository PeriodRepository { get; set; }

        /// <summary>
        /// Файловый менеджер
        /// </summary>
        public IFileManager FileManager { get; set; }

        /// <summary>
        /// Домен-сервис для Описание файла
        /// </summary>
        public IDomainService<FileInfo> FileInfoDomain { get; set; }

        /// <summary>
        /// Домен-сервис для Трансфер
        /// </summary>
        public IDomainService<Transfer> TransferDomain { get; set; }

        /// <summary>
        /// Домен-сервис для Движение денег
        /// </summary>
        public IDomainService<MoneyOperation> MoneyOperationDomain { get; set; }

        /// <summary>
        /// Домен-сервис для Ситуация по ЛС на период
        /// </summary>
        public IDomainService<PersonalAccountPeriodSummary> PersonalAccountPeriodSummaryDomain { get; set; }

        /// <summary>
        /// Домен-сервис для Операция изменения ЛС
        /// </summary>
        public IDomainService<PersonalAccountChange> ChangeDomain { get; set; }

        /// <summary>
        /// Интерфейс для работы Менеджера пользователей
        /// </summary>
        public IGkhUserManager UserManager { get; set; }

        /// <summary>
        /// Применить распределение
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>Результат</returns>
        public virtual IDataResult Apply(BaseParams baseParams)
        {
            try
            {
                this.Container.InTransaction(
                    () =>
                    {
                        using (new DatabaseMutexContext("Apply", "Применить распределение"))
                        {
                            var distributionDate = baseParams.Params.GetAs<DateTime>("OperationDate");
                            var distributionReason = baseParams.Params.GetAs<string>("Reason");
                            var distributionDocument = baseParams.Files.ContainsKey("Document") ? baseParams.Files["Document"] : null;

                            var records = baseParams.Params.GetAs<List<AccountProxy>>("records");
                            var accIds = records.Select(x => x.Id).ToArray();

                            var accounts = this.PersonalAccountDomain.GetAll()
                                .Where(x => accIds.Contains(x.Id))
                                .GroupBy(x => x.Id)
                                .ToDictionary(x => x.Key, x => x.FirstOrDefault());

                            foreach (var record in records)
                            {
                                var account = accounts.Get(record.Id);
                                if (account != null)
                                {
                                    this.DistributeFunds(account, record.DistributionSum, distributionDate, distributionDocument, distributionReason);
                                }
                            }
                        }
                    });

                return new BaseDataResult();
            }
            catch (DatabaseMutexException)
            {
                return BaseDataResult.Error("Применить распределение уже запущенао");
            }
        }

        /// <summary>
        /// Получить объекты распределения
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>Результат</returns>
        public IDataResult GetAccountsInfo(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();
            var persAccIds = baseParams.Params.GetAs<string>("persAccIds").ToLongArray();
            var distributionSum = baseParams.Params.GetAs<decimal>("distributionSum");
            var distributionType = baseParams.Params.GetAs<PerformedWorkFundsDistributionType>("distributionType");

            var query = this.PersonalAccountDomain.GetAll()
                .Where(x => persAccIds.Contains(x.Id))
                .Order(loadParams)
                .Filter(loadParams, this.Container);

            var totalCount = query.Count();
            var accounts = query.ToArray();

            var checkCloseState = accounts.Any(x => x.IsClosed() && !x.IsClosedWithCredit());

            if (checkCloseState)
            {
                return new BaseDataResult(false, "Имеются лицевые счета со статусом \"Закрыт\"!");
            }

            var proxies = new List<AccountProxy>();

            switch (distributionType)
            {
                case PerformedWorkFundsDistributionType.Uniform:

                    proxies = Utils.MoneyAndCentDistribution(
                        accounts,
                        x => distributionSum / accounts.Length,
                        distributionSum,
                        (acc, sum) => new AccountProxy(acc, sum),
                        (proxy, coin) =>
                        {
                            if (proxy.DistributionSum > 0)
                            {
                                proxy.DistributionSum += coin;
                                return true;
                            }
                            proxy.DistributionSum = 0;
                            return false;
                        });

                    break;

                case PerformedWorkFundsDistributionType.ProportionalArea:
                    var sumDistrib = accounts.SafeSum(x => x.Room.Area * x.AreaShare);

                    if (sumDistrib == 0m)
                    {
                        return new BaseDataResult(
                            false,
                            "Невозможно выполнить распределение по площадям, сумма площадей равна нулю");
                    }

                    sumDistrib = distributionSum / sumDistrib;

                    proxies = Utils.MoneyAndCentDistribution(
                        accounts,
                        x => x.Room.Area * x.AreaShare * sumDistrib,
                        distributionSum,
                        (acc, sum) => new AccountProxy(acc, sum),
                        (proxy, coin) =>
                        {
                            if (proxy.DistributionSum > 0)
                            {
                                proxy.DistributionSum += coin;
                                return true;
                            }
                            proxy.DistributionSum = 0;
                            return false;
                        });
                    break;
            }

            return new ListDataResult(proxies, totalCount);
        }

        /// <summary>
        /// Получить остаток за ранее выполненные работы
        /// <para>
        /// Заглушка
        /// </para>
        /// </summary>
        /// <param name="accountId">
        /// ЛС
        /// </param>
        /// <returns>
        /// The <see cref="decimal" />.
        /// </returns>
        public virtual decimal GetPerformedWorkChargeBalance(long accountId)
        {
            return 0m;
        }

        /// <summary>
        /// Распределить зачеты заранее выполненные работы для пакета ЛС
        /// <para>Заглушка</para>
        /// </summary>
        /// <param name="personalAccounts">Список ЛС</param>
        public virtual void DistributeForAccountPacket(IQueryable<BasePersonalAccount> personalAccounts)
        {
        }

        private void DistributeFunds(BasePersonalAccount account, decimal newValue, DateTime operationDate, FileData document, string reason)
        {
            var period = this.PeriodRepository.GetCurrentPeriod();

            FileInfo fileInfo = null;
            if (document != null)
            {
                fileInfo = this.FileManager.SaveFile(document);
                this.FileInfoDomain.Save(fileInfo);
            }

            var transfer = account.ApplyPerformedWorkDistribution(newValue, reason, operationDate, fileInfo, period);
            var summary = account.GetOpenedPeriodSummary();

            this.PersonalAccountPeriodSummaryDomain.Update(summary);
            this.PersonalAccountDomain.Update(account);
            this.MoneyOperationDomain.SaveOrUpdate(transfer.Operation);
            this.TransferDomain.SaveOrUpdate(transfer);

            this.SaveHistory(
                account,
                PersonalAccountChangeType.PerformedWorkFundsDistribution,
                "Распределение суммы на ЛС",
                newValue.ToString(),
                fileInfo,
                reason,
                operationDate);
        }

        private void SaveHistory(
            BasePersonalAccount account,
            PersonalAccountChangeType type,
            string description,
            string newValue,
            FileInfo document,
            string reason,
            DateTime? actualFrom = null)
        {
            this.ChangeDomain.Save(
                new PersonalAccountChange
                {
                    PersonalAccount = account,
                    ChangeType = type,
                    Date = DateTime.UtcNow,
                    Description = description,
                    Operator = this.UserManager.GetActiveUser().Return(x => x.Login),
                    ActualFrom = actualFrom,
                    NewValue = newValue,
                    Document = document,
                    Reason = reason,
                    ChargePeriod = this.PeriodRepository.GetCurrentPeriod()
                });
        }
    }

    /// <summary>
    /// Прокси лицевого счёта
    /// </summary>
    internal class AccountProxy
    {
        public AccountProxy()
        {
        }

        public AccountProxy(BasePersonalAccount account, decimal sum)
        {
            this.Id = account.Id;
            this.Municipality = account.Room.RealityObject.Municipality.Name;
            this.Address = account.Room.RealityObject.Address;
            this.PersonalAccountNum = account.PersonalAccountNum;
            this.Area = account.Room.Area;
            this.AreaShare = account.AreaShare;
            this.DistributionSum = sum;
        }

        /// <summary>
        /// Идентификатор
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Адрес квартиры
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Муниципальное образование
        /// </summary>
        public string Municipality { get; set; }

        /// <summary>
        /// Номер лицевого счёта
        /// </summary>
        public string PersonalAccountNum { get; set; }

        /// <summary>
        /// Площадь
        /// </summary>
        public decimal Area { get; set; }

        /// <summary>
        /// Доля собственности
        /// </summary>
        public decimal AreaShare { get; set; }

        /// <summary>
        /// Сумма
        /// </summary>
        public decimal DistributionSum { get; set; }
    }
}