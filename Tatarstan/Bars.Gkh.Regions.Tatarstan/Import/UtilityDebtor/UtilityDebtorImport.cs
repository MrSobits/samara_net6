namespace Bars.Gkh.Regions.Tatarstan.Import.UtilityDebtor
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Threading;

    using Bars.B4;
    using Bars.B4.Modules.States;
    using Bars.B4.Modules.Tasks.Common.Service;
    using Bars.B4.Modules.Tasks.Common.Utils;
    using Bars.B4.Utils;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Import;
    using Bars.Gkh.Import.Impl;
    using Bars.Gkh.Modules.ClaimWork.Entities;
    using Bars.Gkh.Modules.ClaimWork.Enums;
    using Bars.Gkh.Regions.Tatarstan.Entities.UtilityDebtor;
    using Bars.Gkh.Repositories;
    using Bars.Gkh.Utils;

    using Castle.Core.Internal;

    using ExecutionContext = Bars.B4.Modules.Tasks.Common.Contracts.ExecutionContext;

    /// <summary>
    /// Импорт неплательщиков ЖКУ
    /// </summary>
    public partial class UtilityDebtorImport : GkhImportBase
    {
        /// <summary>
        /// Id
        /// </summary>
        public static string Id = MethodBase.GetCurrentMethod().DeclaringType.FullName;

        /// <summary>
        /// Ключ импорта
        /// </summary>
        public override string Key => UtilityDebtorImport.Id;

        /// <summary>
        /// Код группировки импорта (например группировка в меню)
        /// </summary>
        public override string CodeImport => "UtilityDebtorImport";

        /// <summary>
        /// Наименование импорта
        /// </summary>
        public override string Name => "Импорт неплательщиков ЖКУ";

        /// <summary>
        /// Разрешенные расширения файлов
        /// </summary>
        public override string PossibleFileExtensions => "xls,xlsx";

        /// <summary>
        /// Права
        /// </summary>
        public override string PermissionName => "Clw.ClaimWork.UtilityDebtor.Import";

        /// <summary>
        /// Менеджер лога
        /// </summary>
        public ILogImportManager LogImportManager { get; set; }

        /// <summary>
        /// Лог
        /// </summary>
        public new ILogImport LogImport { get; set; }

        private readonly Dictionary<int, Dictionary<bool, List<string>>> logDict = new Dictionary<int, Dictionary<bool, List<string>>>();
        private readonly List<UtilityDebtorRecord> records = new List<UtilityDebtorRecord>();
        private Dictionary<string, int> headersDict;

        private List<UtilityDebtorClaimWork> utilityDebtorsForSave;
        private List<ExecutoryProcess> executoryProcessForSave;
        private List<SeizureOfProperty> seizureOfPropertyForSave;
        private List<DepartureRestriction> departureRestrictionForSave;

        private Dictionary<Guid?, RealityObject> realityObjectDict;
        private Dictionary<string, JurInstitution> jurInstitutionDict;
        private Dictionary<string, State> execProcStateDict; 

        /// <summary>
        /// Домен-сервис <see cref="RealityObject" />
        /// </summary>
        public IDomainService<RealityObject> RealityObjectDomain { get; set; }

        /// <summary>
        /// Домен-сервис <see cref="State" />
        /// </summary>
        public IStateRepository StateRepository { get; set; }

        /// <summary>
        /// Домен-сервис <see cref="JurInstitution" />
        /// </summary>
        public IDomainService<JurInstitution> JurInstitutionDomain { get; set; }

        /// <summary>
        /// Провайдер состояний
        /// </summary>
        public IStateProvider StateProvider { get; set; }

        /// <summary>
        /// Менеджер пользователей
        /// </summary>
        public IGkhUserManager UserManager { get; set; }

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

            indicator.Indicate(null, 0, "Чтение из файла");

            this.ProcessData(file.Data, fileExtention);

            indicator.Indicate(null, 10, "Подготовка данных");

            this.PrepareData(indicator);

            indicator.Indicate(null, 80, "Сохранение данных");

            this.SaveChanges();

            indicator.Indicate(null, 90, "Запись логов");

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
            var currentOperator = this.UserManager.GetActiveOperator();

            this.LogImportManager.FileNameWithoutExtention = fileName;
            this.LogImportManager.UploadDate = DateTime.Now;

            this.LogImport.Operator = currentOperator;
            this.LogImport.SetFileName(fileName);
            this.LogImport.ImportKey = this.Key;
        }

        private void AddLog(int rowNum, string message, bool success = true)
        {
            if (!this.logDict.ContainsKey(rowNum))
            {
                this.logDict[rowNum] = new Dictionary<bool, List<string>>();
            }

            var log = this.logDict[rowNum];

            if (!log.ContainsKey(success))
            {
                log[success] = new List<string>();
            }

            log[success].Add(message);
        }

        private void PrepareData(IProgressIndicator indicator)
        {
            this.utilityDebtorsForSave = new List<UtilityDebtorClaimWork>();
            this.executoryProcessForSave = new List<ExecutoryProcess>();
            this.seizureOfPropertyForSave = new List<SeizureOfProperty>();
            this.departureRestrictionForSave = new List<DepartureRestriction>();

            this.jurInstitutionDict = this.JurInstitutionDomain.GetAll()
                .GroupBy(x => x.Code)
                .ToDictionary(x => x.Key, x => x.First());

            this.realityObjectDict = this.RealityObjectDomain.GetAll()
                .WhereContains(x => x.FiasAddress.HouseGuid, this.records.Select(y => y.HouseGuid))
                .GroupBy(x => x.FiasAddress.HouseGuid)
                .ToDictionary(x => x.Key, x => x.First());

            this.execProcStateDict = this.StateRepository.GetAllStates<ExecutoryProcess>()
                .GroupBy(x => x.Code)
                .ToDictionary(x => x.Key, x => x.First());

            uint start, result, index;
            start = 30;
            result = 50;
            index = 1;

            indicator.Indicate(null, start, "Обработка данных");

            foreach (var record in this.records)
            {
                var utilityDebtor = this.CreateUtilityDebtor(record);
                this.CreateExecutoryProcess(record, utilityDebtor);
                this.CreateSeizureOfProperty(record, utilityDebtor);
                this.CreateDepartureRestriction(record, utilityDebtor);

                var progress = start + result / this.records.Count * index;

                indicator.Indicate(null, (uint)progress, "Обработка данных");

                index++;
            }
        }

        private UtilityDebtorClaimWork CreateUtilityDebtor(UtilityDebtorRecord record)
        {
            var newUtilityDebtor = new UtilityDebtorClaimWork
            {
                AccountOwner = record.AccountOwner,
                OwnerType = record.OwnerType,
                RealityObject = this.realityObjectDict[record.HouseGuid],
                ChargeDebt = record.ChargeDebt,
                PenaltyDebt = record.PenaltyDebt,
                CountDaysDelay = record.CountDaysDelay,
                ClaimWorkTypeBase = ClaimWorkTypeBase.UtilityDebtor
            };

            if (newUtilityDebtor.State.IsNull())
            {
                this.StateProvider.SetDefaultState(newUtilityDebtor);
            }

            this.utilityDebtorsForSave.Add(newUtilityDebtor);

            this.AddLog(
                record.RowNumber,
                string.Format(
                    "Создан новый неплательщик ЖКУ по адресу {0}",
                    newUtilityDebtor.RealityObject.Address));

            return newUtilityDebtor;
        }

        private void CreateExecutoryProcess(UtilityDebtorRecord record, UtilityDebtorClaimWork utilityDebtor)
        {
            if (!record.RegistrationNumber.IsNullOrEmpty())
            {
                var newExecutoryProcess = new ExecutoryProcess
                {
                    ClaimWork = utilityDebtor,
                    JurInstitution = this.jurInstitutionDict.Get(record.JurInstitutionCode),
                    RegistrationNumber = record.RegistrationNumber,
                    DateStart = record.DateStart,
                    DateEnd = record.DateEnd,
                    Creditor = record.Creditor,
                    Clause = record.Clause,
                    Paragraph = record.Paragraph,
                    Subparagraph = record.Subparagraph,
                    DocumentNumber = record.DocumentNumber,
                    State = this.execProcStateDict.Get(record.StateCode),
                    AccountOwner = record.AccountOwner,
                    OwnerType = record.OwnerType,
                    RealityObject = this.realityObjectDict[record.HouseGuid],
                    DebtSum = (record.ChargeDebt ?? 0) + (record.PenaltyDebt ?? 0),
                    DocumentType = ClaimWorkDocumentType.ExecutoryProcess
                };

                if (newExecutoryProcess.State.IsNull())
                {
                    this.StateProvider.SetDefaultState(newExecutoryProcess);
                }

                this.executoryProcessForSave.Add(newExecutoryProcess);

                this.AddLog(
                record.RowNumber,
                string.Format(
                    "Создано исполнительное производство с номером {0}",
                    newExecutoryProcess.RegistrationNumber));
            }
        }

        private void CreateSeizureOfProperty(UtilityDebtorRecord record, UtilityDebtorClaimWork utilityDebtor)
        {
            if (!record.RegistrationNumber.IsNullOrEmpty() && record.HaveSeizureOfProperty)
            {
                var newSeizureOfProperty = new SeizureOfProperty
                {
                    ClaimWork = utilityDebtor,
                    JurInstitution = this.jurInstitutionDict.Get(record.JurInstitutionCode),
                    AccountOwner = record.AccountOwner,
                    OwnerType = record.OwnerType,
                    DocumentDate = record.SeizureOfPropertyDocDate,
                    DocumentType = ClaimWorkDocumentType.SeizureOfProperty
                };

                this.seizureOfPropertyForSave.Add(newSeizureOfProperty);

                this.AddLog(record.RowNumber, "Создано постановление о наложении ареста на имущество");
            }
        }

        private void CreateDepartureRestriction(UtilityDebtorRecord record, UtilityDebtorClaimWork utilityDebtor)
        {
            if (!record.RegistrationNumber.IsNullOrEmpty() && record.HaveDepartureRestriction)
            {
                var newDepartureRestriction = new DepartureRestriction
                {
                    ClaimWork = utilityDebtor,
                    JurInstitution = this.jurInstitutionDict.Get(record.JurInstitutionCode),
                    AccountOwner = record.AccountOwner,
                    OwnerType = record.OwnerType,
                    DocumentDate = record.SeizureOfPropertyDocDate,
                    DocumentType = ClaimWorkDocumentType.DepartureRestriction
                };

                this.departureRestrictionForSave.Add(newDepartureRestriction);

                this.AddLog(record.RowNumber, "Создано постановление  об ограничении выезда из РФ");
            }
        }

        private void SaveChanges()
        {
            TransactionHelper.InsertInManyTransactions(this.Container, this.utilityDebtorsForSave, useStatelessSession: true);
            TransactionHelper.InsertInManyTransactions(this.Container, this.executoryProcessForSave, useStatelessSession: true);
            TransactionHelper.InsertInManyTransactions(this.Container, this.seizureOfPropertyForSave, useStatelessSession: true);
            TransactionHelper.InsertInManyTransactions(this.Container, this.departureRestrictionForSave, useStatelessSession: true);
        }

        private void WriteLogs()
        {
            foreach (var log in this.logDict.OrderBy(x => x.Key))
            {
                var rowNumber = $"Строка {log.Key}";

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

            this.LogImport.CountAddedRows = this.utilityDebtorsForSave.Count;
        }
    }
}