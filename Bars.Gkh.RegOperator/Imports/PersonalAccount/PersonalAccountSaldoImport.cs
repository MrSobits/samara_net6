namespace Bars.Gkh.RegOperator.Imports
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Domain.TableLocker;
    using Bars.Gkh.Enums.Import;
    using Bars.Gkh.Import;
    using Bars.Gkh.Import.Impl;
    using Bars.Gkh.RegOperator.DomainService.PersonalAccount;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount;
    using Bars.Gkh.Repositories.ChargePeriod;
    using Bars.Gkh.Utils;
    using Bars.GkhExcel;

    using EnumerableExtensions = Bars.B4.Modules.Analytics.Reports.Extensions.EnumerableExtensions;

    /// <summary>
    /// Импорт Excel файла с изменениями сальдо - Импорт документа с скорректированным сальдо
    /// </summary>
    public class PersonalAccountSaldoImport : GkhImportBase
    {
        private const bool needLockTable = false;

        #region Public Properties
        /// <summary>
        /// Ключ для регистрации импорта
        /// </summary>
        public static string Id = MethodBase.GetCurrentMethod().DeclaringType.FullName;

        /// <summary>
        /// Ключ импорта
        /// </summary>
        public override string Key => PersonalAccountSaldoImport.Id;

        /// <summary>
        /// Код группировки импорта (например группировка в меню)
        /// </summary>
        public override string CodeImport => "PersonalAccountImport";

        /// <summary>
        /// Наименование импорта
        /// </summary>
        public override string Name => "Импорт сальдо";

        /// <summary>
        /// Разрешенные расширения файлов
        /// </summary>
        public override string PossibleFileExtensions => "xlsx";

        /// <summary>
        /// Права
        /// </summary>
        public override string PermissionName => "GkhRegOp.PersonalAccount.Import.PersonalSaldoAccountImport";

        /// <summary>
        /// Сервис установки/изменения сальдо
        /// </summary>
        public IAccountSaldoChangeService AccountSaldoChangeService { get; set; }

        /// <summary>
        /// Репозиторий периода начислений
        /// </summary>
        public IChargePeriodRepository ChargePeriodRepository { get; set; }

        /// <summary>
        /// Поставщик сессий
        /// </summary>
        public ISessionProvider SessionProvider { get; set; }

        /// <summary>
        /// Домен-сервис <see cref="SaldoChangeExport"/>
        /// </summary>
        public IDomainService<SaldoChangeExport> SaldoChangeExportDomain { get; set; }

        /// <summary>
        /// Домен-сервис <see cref="AccountExcelSaldoChange"/>
        /// </summary>
        public IDomainService<AccountExcelSaldoChange> AccountExcelSaldoChangeDomain { get; set; }

        /// <summary>
        /// Домен-сервис <see cref="BasePersonalAccount"/>
        /// </summary>
        public IDomainService<BasePersonalAccount> PersonalAccountDomain { get; set; }

        /// <summary>
        /// Домен-сервис <see cref="PersonalAccountPeriodSummary"/>
        /// </summary>
        public IDomainService<PersonalAccountPeriodSummary> PersonalAccountPeriodSummaryDomain { get; set; }
        #endregion

        private SaldoChangeExport saldoChangeExport;
        private Dictionary<string, BasePersonalAccount> personalAccountByNumber;
        private PersonalAccountSaldoProxy[] importedRows;

        private readonly Dictionary<string, PropertyInfo> propertyInfos = new Dictionary<string, PropertyInfo>();

        private readonly Dictionary<string, Tuple<int, string>> propertyMapping = new Dictionary<string, Tuple<int, string>>
        {
            { "PersonalAccountNumber", new Tuple<int, string>(2, "Лицевой счет") },
            { "NewSaldoByBaseTariff", new Tuple<int, string>(4, "Сальдо по базовому тарифу") },
            { "NewSaldoByDecisionTariff", new Tuple<int, string>(5, "Сальдо по тарифу решения") },
            { "NewSaldoByPenalty", new Tuple<int, string>(6, "Сальдо по пени") },
        };

        /// <summary>
        /// Метод выполняет первоначальную проверку импортируемого файла
        /// </summary>
        public override bool Validate(BaseParams baseParams, out string message)
        {
            var tableLocker = this.Container.Resolve<ITableLocker>();
            try
            {
                if (tableLocker.CheckLocked<PersonalAccountPeriodSummary>("UPDATE"))
                {
                    message = TableLockedException.StandardMessage;
                    return false;
                }
            }
            finally
            {
                this.Container.Release(tableLocker);
            }

            if (!base.Validate(baseParams, out message))
            {
                return false;
            }

            var file = baseParams.Files.First().Value;

            var exportedSaldo = this.SaldoChangeExportDomain.GetAll()
                .Where(x => x.FileName == file.FileName)
                .Select(x => new {x.FileName, x.Imported, x.Period})
                .FirstOrDefault();

            if (exportedSaldo.IsNull())
            {
                message = "Не найден выгруженный файл с совпадающим названием";
                return false;
            }

            if (exportedSaldo.Imported)
            {
                message = "Данный файл уже был загружен";
                return false;
            }

            var currentPeriod = this.ChargePeriodRepository.GetCurrentPeriod();
            if (currentPeriod.Id != exportedSaldo.Period.Id)
            {
                message = "Импорт файла за предыдущие периоды невозможен";
                return false;
            }

            return true;
        }

        /// <summary>
        /// Загрузить импорт
        /// </summary>
        protected override ImportResult ImportUsingGkhApi(BaseParams baseParams)
        {
            string errorMessage;
            if (!this.Validate(baseParams, out errorMessage))
            {
                return new ImportResult(StatusImport.CompletedWithError, errorMessage);
            }

            this.TableLockerExecute(true);

            this.Indicate(5, "Инициализация кэша");
            this.InitParams(baseParams);
            this.InitCache();

            try
            {
                this.Indicate(10, "Чтение записей");
                var excel = this.Container.Resolve<IGkhExcelProvider>("ExcelEngineProvider");
                using (this.Container.Using(excel))
                {
                    if (excel == null)
                    {
                        throw new Exception("Не найдена реализация интерфейса IGkhExcelProvider");
                    }

                    using (var memoryStream = new MemoryStream(baseParams.Files.First().Value.Data))
                    {
                        if (this.ProcessFile(excel, memoryStream) == 0)
                        {
                            return new ImportResult(StatusImport.CompletedWithWarning, "Импортируемый файл пуст");
                        }
                    }
                }

                this.Indicate(20, "Обработка записей");
                try
                {
                    this.Container.InTransaction(() =>
                    {
                        this.ProcessRows();
                        
                        this.saldoChangeExport.SetImported();
                        this.SaldoChangeExportDomain.Update(this.saldoChangeExport);
                    });
                }
                finally
                {
                    this.WriteLogs();
                }
            }
            catch (ValidationException exception)
            {
                this.LogImport.Error("Ошибка при выполнении импорта документа с скорректированным сальдо", exception.Message);
                return new ImportResult(StatusImport.CompletedWithError);
            }

            this.TableLockerExecute(false);

            return this.importedRows.Any(x => !x.IsValid) ? new ImportResult(StatusImport.CompletedWithWarning) : new ImportResult(StatusImport.CompletedWithoutError);
        }

        private void InitParams(BaseParams baseParams)
        {
            var file = baseParams.Files.First().Value;
            this.saldoChangeExport = this.SaldoChangeExportDomain
                .GetAll()
                .FirstOrDefault(x => x.FileName == file.FileName);
        }

        private void InitCache()
        {
            var paQuery = this.PersonalAccountDomain.GetAll()
                .Where(x => this.AccountExcelSaldoChangeDomain.GetAll()
                        .Where(y => y.SaldoChangeExport.Id == this.saldoChangeExport.Id)
                        .Any(y => y.PersonalAccount.Id == x.Id));

            this.personalAccountByNumber = paQuery.ToDictionary(x => x.PersonalAccountNum);
        }

        private int ProcessFile(IGkhExcelProvider excel, Stream stream)
        {
            excel.UseVersionXlsx();
            excel.Open(stream);
            var data = excel.GetRows(0, 0);

            if (data.Count == 0)
            {
                return 0;
            }

            var startIndex = 0;
            var rowNum = 0;
            while (startIndex == 0 && rowNum < data.Count)
            {
                var row = data[rowNum].Where(x => x.Value != null).Select(x => x.Value.ToLower()).ToArray();

                if (row.Contains("муниципальное образование"))
                {
                    startIndex = rowNum + 1;
                    break;
                }

                rowNum++;
            }

            if (startIndex == 0)
            {
                throw new ValidationException("Формат шаблона не соответствует импорту!");
            }

            if (data.Count - startIndex > this.personalAccountByNumber.Count)
            {
                throw new ValidationException("Перечень загружаемых ЛС не совпадает с выгруженными");
            }

            var listResult = new List<PersonalAccountSaldoProxy>(data.Count);

            for (var i = startIndex; i < data.Count; i++)
            {
                var currentRow = data[i];

                var paChangeProxy = new PersonalAccountSaldoProxy
                {
                    RowNum = i
                };

                foreach (var mapping in this.propertyMapping)
                {
                    try
                    {
                        this.TryExtractProperty(paChangeProxy, mapping.Key, currentRow[mapping.Value.Item1].Value);
                    }
                    catch
                    {
                        paChangeProxy.LogError($"Значение столбца {mapping.Value.Item2} пусто или имеет неверный формат");
                    }
                }

                listResult.Add(paChangeProxy);
            }

            if (listResult.Select(x => x.PersonalAccountNumber).Distinct().Except(this.personalAccountByNumber.Keys).Any())
            {
                throw new ValidationException("Перечень загружаемых ЛС не совпадает с выгруженными");
            }

            this.importedRows = listResult.ToArray();
            return this.importedRows.Length;
        }

        private void ProcessRows()
        {
            IList<ISaldoChangeData> saldoData = new List<ISaldoChangeData>(this.importedRows.Length);
            this.importedRows.Where(x => x.IsValid)
                .ForEach(x =>
                {
                    var saldoInfo = this.saldoChangeExport.Accounts
                        .Where(y => x.PersonalAccountNumber == y.PersonalAccount.PersonalAccountNum)
                        .Select(y => new {y.SaldoByBaseTariffBefore, y.SaldoByDecisinTariffBefore, y.SaldoByPenaltyBefore})
                        .FirstOrDefault();

                    if (saldoInfo != null)
                    {
                        x.PersonalAccount = this.personalAccountByNumber[x.PersonalAccountNumber];

                        // заполняем старые данные из таблицы по всем тарифам
                        x.SaldoByBaseTariff = saldoInfo.SaldoByBaseTariffBefore
                            .RegopRoundDecimal(2);

                        x.SaldoByDecisionTariff = saldoInfo.SaldoByDecisinTariffBefore
                            .RegopRoundDecimal(2);

                        x.SaldoByPenalty = saldoInfo.SaldoByPenaltyBefore
                            .RegopRoundDecimal(2);

                        saldoData.Add(x);
                        this.LogImport.CountChangedRows++;
                    }
                    else
                    {
                        this.LogImport.Warn(x.PersonalAccountNumber, "Сальдо не изменено");
                    }
                });

            this.AccountSaldoChangeService.SetProgressIndicator(this.IndicateProcessing);
            this.AccountSaldoChangeService.ProcessSaldoChange(this.saldoChangeExport, saldoData);
        }

        private void IndicateProcessing(int percent, string message)
        {
            var realPercent = percent * 0.6;
            this.Indicate(20 + (int)realPercent, message);
        }

        private void WriteLogs()
        {
            foreach (var importedValue in this.importedRows.Where(x => !x.IsValid))
            {
                var rowIdtext = $"rowNum = {importedValue.RowNum}, accountNumber = {importedValue.PersonalAccountNumber}";
                this.LogImport.Warn(rowIdtext, EnumerableExtensions.AggregateWithSeparator(importedValue.Errors, ","));
            }
        }

        private void TableLockerExecute(bool lockTable)
        {
            if (!PersonalAccountSaldoImport.needLockTable)
            {
                return;
            }

            var tableLocker = this.Container.Resolve<ITableLocker>();
            try
            {
                if (lockTable)
                {
                    tableLocker.Lock<PersonalAccountPeriodSummary>("UPDATE");
                }
                else
                {
                    tableLocker.Unlock<PersonalAccountPeriodSummary>("UPDATE");
                }
            }
            finally
            {
                this.Container.Release(tableLocker);
            }
        }

        private void TryExtractProperty<TObject>(TObject obj, string propertyName, string value)
        {
            PropertyInfo propertyInfo;
            if (!this.propertyInfos.TryGetValue(typeof(TObject).Name + propertyName, out propertyInfo))
            {
                propertyInfo = typeof(TObject).GetProperty(propertyName);
                this.propertyInfos.Add(typeof(TObject).Name + propertyName, propertyInfo);
            }

            var convertedValue = ConvertHelper.ConvertTo(value, propertyInfo.PropertyType);
            propertyInfo.SetValue(obj, convertedValue);
        }

        #region Proxies
        /// <summary>
        /// Прокси класс, для импортированных столбцов Excel файла
        /// </summary>
        private class PersonalAccountSaldoProxy : ISaldoChangeData
        {
            private decimal newSaldoByBaseTariff;
            private decimal newSaldoByDecisionTariff;
            private decimal newSaldoByPenalty;

            public PersonalAccountSaldoProxy()
            {
                this.IsValid = true;
                this.Errors = new List<string>();
            }

            /// <summary>
            /// Номер лицевого счета
            /// </summary>
            public string PersonalAccountNumber { get; set; }

            /// <summary>
            /// Лицевой счет
            /// </summary>
            public BasePersonalAccount PersonalAccount { get; set; }

            /// <summary>
            /// Сальдо по базовому тарифу
            /// </summary>
            public decimal SaldoByBaseTariff { get; set; }

            /// <summary>
            /// Сальдо по тарифу решения
            /// </summary>
            public decimal SaldoByDecisionTariff { get; set; }

            /// <summary>
            /// Сальдо по пени
            /// </summary>
            public decimal SaldoByPenalty { get; set; }

            /// <summary>
            /// Новое сальдо по базовому тарифу
            /// </summary>
            public decimal NewSaldoByBaseTariff
            {
                get
                {
                    return this.newSaldoByBaseTariff.RegopRoundDecimal(2);
                }
                set
                {
                    this.newSaldoByBaseTariff = value;
                }
            }

            /// <summary>
            /// Новое сальдо по тарифу решения
            /// </summary>
            public decimal NewSaldoByDecisionTariff
            {
                get
                {
                    return this.newSaldoByDecisionTariff.RegopRoundDecimal(2);
                }
                set
                {
                    this.newSaldoByDecisionTariff = value;
                }
            }

            /// <summary>
            /// Новое сальдо по пени
            /// </summary>
            public decimal NewSaldoByPenalty
            {
                get
                {
                    return this.newSaldoByPenalty.RegopRoundDecimal(2);
                }
                set
                {
                    this.newSaldoByPenalty = value;
                }
            }

            /// <summary>
            /// Номер строки
            /// </summary>
            public int RowNum { get; set; }

            /// <summary>
            /// Объект импортирован успешно
            /// </summary>
            public bool IsValid { get; private set; }

            /// <summary>
            /// Список ошибок
            /// </summary>
            public IList<string> Errors { get; }

            /// <summary>
            /// Добавить лог ошибки объекта
            /// </summary>
            /// <param name="errorMessage">Сообщение об ошибке</param>
            public void LogError(string errorMessage)
            {
                this.IsValid = false;
                this.Errors.Add(errorMessage);
            }
        }
        #endregion
    }
}