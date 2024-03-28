namespace Bars.Gkh.RegOperator.ExecutionAction
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Utils;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.ExecutionAction;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount;

    using Castle.Windsor;

    using Ionic.Zip;
    using Ionic.Zlib;

    /// <summary>
    /// Копирование льготной категории из карточки абонента в ЛС
    /// </summary>
    public class CopyPrivilegedCategory : BaseExecutionAction
    {
        /// <summary>
        /// Список аккаунтов у которых нужно создать льготу
        /// </summary>
        private readonly List<PersonalAccountPrivilegedCategory> personalAccountPrivilegedCategoryList = new List<PersonalAccountPrivilegedCategory>();

        /// <summary>
        /// Лог
        /// </summary>
        private readonly StringBuilder finalLog = new StringBuilder();


        /// <summary>
        /// Описание
        /// </summary>
        public override string Description => @"Копирование льготной категории из карточки абонента в ЛС";

        /// <summary>
        /// Название
        /// </summary>
        public override string Name => "Копирование льготной категории из карточки абонента в ЛС";

        /// <summary>
        /// Метот запуска
        /// </summary>
        public override Func<IDataResult> Action => this.CreatePassportForAll;

        /// <summary>
        /// Домен сервис для абонента
        /// </summary>
        public IDomainService<PersonalAccountOwner> PersonalAccountOwner { get; set; }

        /// <summary>
        /// Домен сервис для лицевого счета
        /// </summary>
        public IDomainService<BasePersonalAccount> BasePersonalAccount { get; set; }

        /// <summary>
        /// Домен сервис для льготной категории аккаунта
        /// </summary>
        public IDomainService<PersonalAccountPrivilegedCategory> PersonalAccountPrivilegedCategies { get; set; }

        private BaseDataResult CreatePassportForAll()
        {
            var logOperationRepo = this.Container.ResolveRepository<LogOperation>();
            var fileManager = this.Container.Resolve<IFileManager>();
            var userManager = this.Container.Resolve<IGkhUserManager>();

            this.finalLog.AppendLine(
                "Тип абонента;ФИО/Наименование абонента;Наименование льготы;Действует с;Лицевой счет;" +
                    "Дата начала действия;Дата окончания действия;Статус копирования");

            var take = 1000;
            var count = 0;

            var query = this.BasePersonalAccount.GetAll()
                .Where(x => x.State.Code == "1")
                .Where(x => x.AccountOwner.PrivilegedCategory != null);

            var totalCount = query.Count();

            while (count <= totalCount)
            {
                var portion = query.Skip(count).Take(take);

                var foundAccs = portion
                    .Where(
                        x => this.PersonalAccountPrivilegedCategies.GetAll().Any(
                            y => y.PersonalAccount.Id == x.Id &&
                                y.PrivilegedCategory.Id == x.AccountOwner.PrivilegedCategory.Id &&
                                y.DateFrom != x.AccountOwner.PrivilegedCategory.DateFrom));

                foreach (var account in foundAccs)
                {
                    this.AddRecord(account, "Скопирован");
                }

                var notFoundAccs = portion.Where(x => !foundAccs.Select(y => y.Id).Contains(x.Id));
                var notTableAccounts =
                    notFoundAccs.Where(x => !this.PersonalAccountPrivilegedCategies.GetAll().Select(y => y.PersonalAccount.Id).Contains(x.Id));

                foreach (var account in notTableAccounts)
                {
                    this.AddRecord(account, "Скопирован");
                }

                var duplicateAccs =
                    notFoundAccs.Where(x => this.PersonalAccountPrivilegedCategies.GetAll().Select(y => y.PersonalAccount.Id).Contains(x.Id)).ToArray();

                foreach (var account in duplicateAccs)
                {
                    this.AddLog(account, "Не скопирована (дублирование)");
                }
                count += take;
            }

            var logsZip = new ZipFile(Encoding.UTF8)
            {
                CompressionLevel = CompressionLevel.Level3,
                AlternateEncoding = Encoding.GetEncoding("cp866")
            };

            var logOperation = new LogOperation
            {
                StartDate = DateTime.Now,
                Comment = "Копирование льгот в карточку ЛС из карточки абонента",
                OperationType = LogOperationType.RunAction,
                EndDate = DateTime.UtcNow,
                User = userManager.GetActiveUser()
            };

            using (var logFile = new MemoryStream())
            {
                var log = Encoding.GetEncoding(1251).GetBytes(this.finalLog.ToString());

                logsZip.AddEntry(string.Format("{0}.csv", logOperation.OperationType.GetEnumMeta().Display), log);

                logsZip.Save(logFile);

                var logFileInfo = fileManager.SaveFile(
                    logFile,
                    string.Format("{0}.zip", logOperation.OperationType.GetEnumMeta().Display));

                logOperation.LogFile = logFileInfo;
            }

            logOperationRepo.Save(logOperation);

            TransactionHelper.InsertInManyTransactions(this.Container, this.personalAccountPrivilegedCategoryList, 1000, true, true);
            return new BaseDataResult(true);
        }

        private void AddRecord(BasePersonalAccount account, string status)
        {
            PersonalAccountPrivilegedCategory personalAccountPrivilegedCategory =
                new PersonalAccountPrivilegedCategory
                {
                    PersonalAccount = account,
                    PrivilegedCategory = account.AccountOwner.PrivilegedCategory,
                    DateFrom = account.AccountOwner.PrivilegedCategory.DateFrom,
                    DateTo = account.AccountOwner.PrivilegedCategory.DateTo
                };

            this.AddLog(account, status);

            this.personalAccountPrivilegedCategoryList.Add(personalAccountPrivilegedCategory);
        }

        private void AddLog(BasePersonalAccount account, string status)
        {
            this.finalLog.AppendLine(
                @"" + account.AccountOwner.OwnerType.GetDisplayName() + ";"
                    + account.AccountOwner.Name + ";"
                    + account.AccountOwner.PrivilegedCategory.Name + ";"
                    + account.AccountOwner.PrivilegedCategory.DateFrom.ToStr() + ";"
                    + account.PersonalAccountNum + ";"
                    + account.AccountOwner.PrivilegedCategory.DateFrom.ToStr() + ";"
                    + account.AccountOwner.PrivilegedCategory.DateTo.ToStr() + ";"
                    + status);
        }
    }
}