﻿namespace Bars.Gkh.RegOperator.Regions.Chelyabinsk.Imports
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Threading;

    using Bars.B4;
    using Bars.B4.Modules.States;
    using Bars.B4.Modules.Tasks.Common.Service;
    using Bars.B4.Utils;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Import;
    using Bars.Gkh.Import.Impl;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Regions.Chelyabinsk.Entities;
    using Bars.Gkh.Utils;

    using NHibernate.Linq;

    using ExecutionContext = Bars.B4.Modules.Tasks.Common.Contracts.ExecutionContext;

    /// <summary>
    /// Импорт ПИР
    /// </summary>
    public partial class AgentPIRDocumentImport : GkhImportBase
    {
        /// <summary>
        /// Id
        /// </summary>
        public static string Id = MethodBase.GetCurrentMethod().DeclaringType.FullName;

        /// <summary>
        /// Ключ импорта
        /// </summary>
        public override string Key => AgentPIRDocumentImport.Id;

        /// <summary>
        /// Код группировки импорта (например группировка в меню)
        /// </summary>
        public override string CodeImport => nameof(AgentPIRDocumentImport);

        /// <summary>
        /// Наименование импорта
        /// </summary>
        public override string Name => "Импорт документов агента ПИР";

        /// <summary>
        /// Разрешенные расширения файлов
        /// </summary>
        public override string PossibleFileExtensions => "xls,xlsx";

        /// <summary>
        /// Права
        /// </summary>
        public override string PermissionName => "Import.AgentPIRDocumentImport";

        /// <summary>
        /// Менеджер пользователей
        /// </summary>
        public IGkhUserManager UserManager { get; set; }

        public new ILogImport LogImport { get; set; }

        public new ILogImportManager LogImportManager { get; set; }

        public IDomainService<AgentPIRDocument> AgentPIRDocumentDomain { get; set; }

        public IDomainService<AgentPIRDebtor> AgentPIRDebtorDomain { get; set; }

        private const string Success = "Строка успешно импортирована";

        private Dictionary<string, int> headersDict;
        private readonly Dictionary<Tuple<int, string>, Dictionary<bool, List<string>>> logDict
            = new Dictionary<Tuple<int, string>, Dictionary<bool, List<string>>>();

        private readonly List<AgentPIRDocumentRecord> records = new List<AgentPIRDocumentRecord>();

        private List<AgentPIRDocument> documentsForSave;

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
            var fileExtention = file.Extention;

            this.InitLog(file.FileName);

            indicator.Report(null, 0, "Чтение из файла");

            this.ProcessData(file.Data, fileExtention);

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
            documentsForSave = new List<AgentPIRDocument>();

            var recordList = this.records.ToList();

            var documentNumbers = records.Select(x => x.Number).ToArray();
            var documentDates = records.Select(x => x.DocumentDate).Distinct().ToArray();
            var documentTypes = records.Select(x => x.DocumentType).Distinct().ToArray();

            var systemAccNumbers = this.AgentPIRDebtorDomain
                .GetAll()
                .WhereContains(x => x.BasePersonalAccount.PersonalAccountNum, this.records.Select(x => x.AccountNumber))
                .Select(x => x.BasePersonalAccount.PersonalAccountNum)
                .Distinct()
                .ToArray();

            var accountNumbersWithClaimwork = this.AgentPIRDocumentDomain
                .GetAll()
                .WhereContains(x => x.AgentPIRDebtor.BasePersonalAccount.PersonalAccountNum, systemAccNumbers)
                .WhereContains(x => x.DocumentDate, documentDates)
                .WhereContains(x => x.DocumentType, documentTypes)
                .WhereContains(x => x.Number, documentNumbers)
                .Select(x => x.AgentPIRDebtor.BasePersonalAccount.PersonalAccountNum)
                .ToArray();

            var notFoundRecords = this.records.Where(x => !systemAccNumbers.Contains(x.AccountNumber));
            var recordsWithClaimork = this.records.Where(x => accountNumbersWithClaimwork.Contains(x.AccountNumber));


            for (int i = 0; i < recordList.Count; i++)
            {
                if(notFoundRecords.Contains(recordList[i]))
                {
                    this.AddLog(recordList[i].RowNumber, recordList[i].AccountNumber, $"ЛС №{recordList[i].AccountNumber} не найден", false);
                    recordList.Remove(recordList[i]);
                    i--;
                }
            }

            for (int i = 0; i < recordList.Count; i++)
            {
                if (recordsWithClaimork.Contains(recordList[i]))
                {
                    this.AddLog(recordList[i].RowNumber, recordList[i].AccountNumber, $"По ЛС №{recordList[i].AccountNumber} уже ведется ПИР", false);
                    recordList.Remove(recordList[i]);
                    i--;
                }
            }

            var accNumForSave = recordList.Select(x => x.AccountNumber).ToArray();

            var debtors = this.AgentPIRDebtorDomain.GetAll()
                .Where(x => accNumForSave.Contains(x.BasePersonalAccount.PersonalAccountNum))
                .ToArray();

            foreach (var record in recordList)
            {
                var debtor = debtors.FirstOrDefault(x => x.BasePersonalAccount.PersonalAccountNum == record.AccountNumber);

                this.documentsForSave.Add(this.CreateDocument(debtor, record));

                this.AddLog(record.RowNumber, record.AccountNumber, AgentPIRDocumentImport.Success, true);
            }
        }

        private AgentPIRDocument CreateDocument(AgentPIRDebtor debtor, AgentPIRDocumentRecord record)
        {
            var document = new AgentPIRDocument
            {
                Number = record.Number,
                DocumentDate = record.DocumentDate,
                DebtSum = record.DebtSum,
                PeniSum = record.PeniSum,
                Duty = record.Duty,
                DocumentType = record.DocumentType,
                AgentPIRDebtor = debtor,
                Repaid = 0
            };

            return document;
        }

        private void SaveChanges()
        {
            TransactionHelper.InsertInManyTransactions(this.Container, this.documentsForSave, useStatelessSession: true);
        }
    }
}