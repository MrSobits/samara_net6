﻿namespace Bars.Gkh.RegOperator.Regions.Chelyabinsk.Imports
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Threading;

    using Bars.B4;
    using Bars.B4.Modules.Tasks.Common.Service;
    using Bars.B4.Utils;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Import;
    using Bars.Gkh.Import.Impl;
    using Bars.Gkh.RegOperator.Regions.Chelyabinsk.Entities;
    using Bars.Gkh.Utils;

    using NHibernate.Linq;

    using ExecutionContext = Bars.B4.Modules.Tasks.Common.Contracts.ExecutionContext;

    /// <summary>
    /// Импорт ПИР
    /// </summary>
    public partial class AgentPIRDebtorCreditImport : GkhImportBase
    {
        /// <summary>
        /// Id
        /// </summary>
        public static string Id = MethodBase.GetCurrentMethod().DeclaringType.FullName;

        /// <summary>
        /// Ключ импорта
        /// </summary>
        public override string Key => AgentPIRDebtorCreditImport.Id;

        /// <summary>
        /// Код группировки импорта (например группировка в меню)
        /// </summary>
        public override string CodeImport => nameof(AgentPIRDebtorCreditImport);

        /// <summary>
        /// Наименование импорта
        /// </summary>
        public override string Name => "Импорт зачислений агента ПИР";

        /// <summary>
        /// Разрешенные расширения файлов
        /// </summary>
        public override string PossibleFileExtensions => "xls,xlsx";

        /// <summary>
        /// Права
        /// </summary>
        public override string PermissionName => "Import.AgentPIRDebtorCreditImport";

        /// <summary>
        /// Менеджер пользователей
        /// </summary>
        public IGkhUserManager UserManager { get; set; }

        public new ILogImport LogImport { get; set; }

        public new ILogImportManager LogImportManager { get; set; }

        public IDomainService<AgentPIRDebtorCredit> AgentPIRDebtorCreditDomain { get; set; }

        public IDomainService<AgentPIRDebtor> AgentPIRDebtorDomain { get; set; }

        private const string Success = "Строка успешно импортирована";

        private Dictionary<string, int> headersDict;
        private readonly Dictionary<Tuple<int, string>, Dictionary<bool, List<string>>> logDict
            = new Dictionary<Tuple<int, string>, Dictionary<bool, List<string>>>();

        private readonly List<AgentPIRDebtorCreditRecord> records = new List<AgentPIRDebtorCreditRecord>();

        private List<AgentPIRDebtorCredit> documentsForSave;

        /// <summary>
        /// Импорт
        /// </summary>
        /// <param name="baseParams">Параметры</param>
        /// <param name="ctx">Контекст</param>
        /// <param name="indicator">Индикатор прогресса</param>
        /// <param name="ct">Уведомление об отмене</param>
        /// <returns>Результат</returns>
        protected override ImportResult Import(BaseParams baseParams, ExecutionContext ctx, IProgressIndicator indicator, CancellationToken ct)
        {
            var file = baseParams.Files["FileImport"];

            this.InitLog(file.FileName);

            indicator.Report(null, 0, "Чтение из файла");

            this.ProcessData(file.FileName, file.Data, file.Extention);

            indicator.Report(null, 20, "Подготовка данных");

            this.CreateDocuments();

            indicator.Report(null, 60, "Сохранение данных");

            this.SaveChanges();

            indicator.Report(null, 80, "Запись логов");

            this.WriteLogs();

            this.LogImportManager.Add(file, this.LogImport);
            this.LogImportManager.Save();

            return new ImportResult();
        }

        /// <summary>
        /// Инициализация лога
        /// </summary>
        /// <param name="fileName">Название файла</param>
        public new void InitLog(string fileName)
        {
            this.LogImportManager.FileNameWithoutExtention = fileName;
            this.LogImportManager.UploadDate = DateTime.Now;

            this.LogImport.SetFileName(fileName);
            this.LogImport.ImportKey = this.Key;
        }

        private void AddLog(int rowNum, string accNum, string message, bool success = true)
        {
            var tuple = new Tuple<int, string>(rowNum, accNum);

            if (!this.logDict.ContainsKey(tuple))
            {
                this.logDict[tuple] = new Dictionary<bool, List<string>>();
            }

            var log = this.logDict[tuple];

            if (!log.ContainsKey(success))
            {
                log[success] = new List<string>();
            }

            log[success].Add(message);
        }

        private void WriteLogs()
        {
            foreach (var log in this.logDict.OrderBy(x => x.Key))
            {
                var rowNumber = $"Строка {log.Key.Item1}, лс {log.Key.Item2}";

                foreach (var rowLog in log.Value)
                {
                    if (rowLog.Key)
                    {
                        this.LogImport.Info(rowNumber, rowLog.Value.AggregateWithSeparator("."));
                    }
                    else
                    {
                        this.LogImport.Warn(rowNumber, rowLog.Value.AggregateWithSeparator("."));
                    }
                }
            }

            this.LogImport.CountChangedRows = 0;

            this.LogImport.CountAddedRows = this.documentsForSave.Count;
        }

        private void CreateDocuments()
        {
            documentsForSave = new List<AgentPIRDebtorCredit>();

            foreach (var record in this.records)
            {
                var debtors = this.AgentPIRDebtorDomain
                .GetAll()
                .Where(x => x.BasePersonalAccount.PersonalAccountNum == record.AccountNumber)
                .ToList();

                if (!debtors.IsEmpty())
                {
                    foreach (var debtor in debtors)
                    {
                        this.documentsForSave.Add(this.CreateCredit(debtor, record));

                        this.AddLog(record.RowNumber, record.AccountNumber, Success, true);
                    }
                }
                else
                    this.AddLog(record.RowNumber, record.AccountNumber, $"ЛС №{record.AccountNumber} не найден", false);
            }
        }

        private AgentPIRDebtorCredit CreateCredit(AgentPIRDebtor debtor, AgentPIRDebtorCreditRecord record)
        {
            var credit = new AgentPIRDebtorCredit
            {
                Debtor = debtor,
                Credit = record.Credit,
                Date = record.Date,
                User = record.User,
                File = record.File
            };

            return credit;
        }

        private void SaveChanges()
        {
            TransactionHelper.InsertInManyTransactions(this.Container, this.documentsForSave, useStatelessSession: true);
        }
    }
}