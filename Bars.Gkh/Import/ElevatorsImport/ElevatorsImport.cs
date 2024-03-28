namespace Bars.Gkh.Import.ElevatorsImport
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using B4;
    using B4.DataAccess;
    using B4.Utils;

    using Bars.B4.IoC;
    using Bars.GkhExcel;

    using Castle.Windsor;
    using Entities;
    using Gkh.Domain;
    using Gkh.Enums.Import;
    using Gkh.Import;
    using Gkh.Import.Impl;

    using Microsoft.Extensions.Logging;

    /// <summary>
    ///     Импорт лифтов (раздел 4.2)
    /// </summary>
    public class ElevatorsImport : GkhImportBase
    {
        public static string Id = MethodBase.GetCurrentMethod().DeclaringType.FullName;

        public const string FormCode = "Form_4_2_1";

        private readonly Dictionary<int, Dictionary<bool, List<string>>> _logDict =
            new Dictionary<int, Dictionary<bool, List<string>>>();

        private Dictionary<string, int> _headers;
        private Dictionary<long, ElevatorRecord[]> _records;
        private List<TehPassportValue> _valuesToDelete;
        private List<TehPassportValue> _valuesToSave;
        private List<TehPassport> _passportsToSave;

        /// <summary>
        ///     IoC-контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <summary>
        ///     Логгер
        /// </summary>
        public ILogger Logger { get; set; }

        /// <summary>
        /// </summary>
        public IDomainService<TehPassportValue> TehPassportValueDomainService { get; set; }

        /// <summary>
        /// </summary>
        public IDomainService<TehPassport> TehPassportDomainService { get; set; }

        /// <summary>
        ///     SessionProvider
        /// </summary>
        public ISessionProvider SessionProvider { get; set; }

        public override string Key
        {
            get { return Id; }
        }

        public override string CodeImport
        {
            get { return "TpElevatorsImport"; }
        }

        public override string Name
        {
            get { return "Импорт лифтов (техпаспорт)"; }
        }

        public override string PossibleFileExtensions
        {
            get { return "xls,xlsx"; }
        }

        public override string PermissionName
        {
            get { return "Import.TpElevatorsImport"; }
        }

        public ElevatorsImport(ILogImportManager logManager, ILogImport logImport)
        {
            this.LogImportManager = logManager;
            this.LogImport = logImport;
        }

        public override ImportResult Import(BaseParams baseParams)
        {
            var file = baseParams.Files["FileImport"];
            var message = "";

            try
            {
                InitLog(file.FileName);

                ReadData(file.Data, file.Extention);

                ProcessData();

                var roIds = _records.Keys;

                if (roIds.Count > 0)
                {
                    var session = SessionProvider.GetCurrentSession();

                    try
                    {
                        for (var i = 0; i < roIds.Count; i += 1000)
                        {
                            var ids = roIds.Skip(i).Take(1000).ToList();

                            session.CreateSQLQuery(@" delete from TP_TEH_PASSPORT_VALUE where id in(
                                    select tpv.id  from TP_TEH_PASSPORT_VALUE tpv
                                    join TP_TEH_PASSPORT tp on tpv.TEH_PASSPORT_ID = tp.ID
                                    where tpv.FORM_CODE = 'Form_4_2_1' and tp.REALITY_OBJ_ID in (:ids))")
                                .SetParameterList("ids", ids)
                                .ExecuteUpdate();
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.LogError(ex, "Ошибка импорта лифтов");
                    }
                }

                TransactionHelper.InsertInManyTransactions(Container, _passportsToSave, useStatelessSession: true);
                TransactionHelper.InsertInManyTransactions(Container, _valuesToSave, useStatelessSession: true);

                WriteLogs();
            }
            catch (Exception e)
            {
                this.LogImport.Error(e.Message,
                    String.Format("Произошла непредвиденная ошибка.\n{0} {1} {2}", e.Message,
                        e.InnerException.ReturnSafe(x => x.Message), e.StackTrace));
                message =
                    String.Format("Произошла непредвиденная ошибка.\n {0} {1} {2}",
                        e.Message, e.InnerException.ReturnSafe(x => x.Message), e.StackTrace);
            }
            finally
            {
                _logDict.Clear();
            }

            this.LogImportManager.Add(file, this.LogImport);
            this.LogImportManager.Save();

            message = string.IsNullOrEmpty(message) ? this.LogImportManager.GetInfo() : message;

            var status = this.LogImportManager.CountError > 0
                ? StatusImport.CompletedWithError
                : StatusImport.CompletedWithoutError;

            var logField = this.LogImportManager.LogFileId;

            return new ImportResult(status, message, string.Empty, logField);
        }

        public override bool Validate(BaseParams baseParams, out string message)
        {
            message = null;
            if (!baseParams.Files.ContainsKey("FileImport"))
            {
                message = "Не выбран файл для импорта";
                return false;
            }

            var extention = baseParams.Files["FileImport"].Extention;

            var fileExtentions = PossibleFileExtensions.Contains(",")
                ? PossibleFileExtensions.Split(',')
                : new[] {PossibleFileExtensions};

            if (fileExtentions.All(x => x != extention))
            {
                message = string.Format("Необходимо выбрать файл с допустимым расширением: {0}", PossibleFileExtensions);
                return false;
            }

            return true;
        }

        private void ReadData(byte[] data, string extension)
        {
            _records = new Dictionary<long, ElevatorRecord[]>();
            _valuesToSave = new List<TehPassportValue>();
            _valuesToDelete = new List<TehPassportValue>();
            _passportsToSave = new List<TehPassport>();

            var records = new List<ElevatorRecord>();
            try
            {
                var excel = this.Container.Resolve<IGkhExcelProvider>("ExcelEngineProvider");
                using (this.Container.Using(excel))
                {
                    if (excel == null)
                    {
                        throw new ImportException("Не найдена реализация интерфейса IGkhExcelProvider");
                    }

                    if (extension == "xlsx")
                    {
                        excel.UseVersionXlsx();
                    }

                    using (var memoryStreamFile = new MemoryStream(data))
                    {
                        memoryStreamFile.Seek(0, SeekOrigin.Begin);

                        excel.Open(memoryStreamFile);

                        var rows = excel.GetRows(0, 0);

                        // получаем строку с идентификаторами заголовков
                        var headerIdsRow = GetHeaderIdsRow(rows);

                        InitHeader(headerIdsRow);

                        // начинаем цикл с i = номер строки после строки с идентификаторами
                        var startIndex = rows.IndexOf(headerIdsRow) + 1;

                        for (var i = startIndex; i < rows.Count; ++i)
                        {
                            var record = ProcessLine(rows[i], i + 1);
                            if (record.RoId > 0)
                            {
                                records.Add(record);
                            }
                            else
                            {
                                AddLog(record.RowNumber, "Не заполнено обязательное поле ID (Идентификатор дома)", false);
                            }
                        }

                        _records = records
                            .GroupBy(x => x.RoId)
                            .ToDictionary(x => x.Key,
                                x => x.ToArray());
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Некорректный файл", ex);
            }
        }

        private void ProcessData()
        {
            foreach (var records in _records)
            {
                TehPassport tehPassport = null;
                var createdCount = 0;
                foreach (var record in records.Value)
                {
                    if (tehPassport == null)
                    {
                        tehPassport =
                            TehPassportDomainService.GetAll().FirstOrDefault(x => x.RealityObject.Id == records.Key);
                        if (tehPassport == null)
                        {
                            tehPassport = new TehPassport { RealityObject = new RealityObject { Id = records.Key } };
                            _passportsToSave.Add(tehPassport);
                        }
                    }

                    _valuesToSave.AddRange(record.CreateElevator(++createdCount, tehPassport));
                    AddLog(record.RowNumber,
                            string.Format("Добавлена информация о лифте. ID: {0}, Подъезд: {1}", record.RoId,
                                            record.PorchNum));
                }
            }
        }

        private GkhExcelCell[] GetHeaderIdsRow(IEnumerable<GkhExcelCell[]> rows)
        {
            try
            {
                return rows.First(row => row.FirstOrDefault(cell => cell.Value == "ID") != null);
            }
            catch (Exception)
            {
                throw new Exception("Импортируемый файл не соответствует формату");
            }
        }

        private void InitHeader(GkhExcelCell[] data)
        {
            _headers = new Dictionary<string, int>();

            _headers["ID"] = -1;
            _headers["PORCH"] = -1;
            _headers["CAPACITY"] = -1;
            _headers["STOP"] = -1;
            _headers["YEAR_INSTALLATION"] = -1;
            _headers["LIFETIME"] = -1;
            _headers["YEAR"] = -1;
            _headers["PERIOD"] = -1;
            _headers["SERIAL_NUMBER"] = -1;
            _headers["SUM"] = -1;
            _headers["OCENKA"] = -1;

            for (var index = 0; index < data.Length; ++index)
            {
                var header = data[index].Value.ToUpper();
                if (_headers.ContainsKey(header))
                {
                    _headers[header] = index;
                }
            }
        }

        private ElevatorRecord ProcessLine(GkhExcelCell[] data, int rowNumber)
        {
            var record = new ElevatorRecord {RowNumber = rowNumber};

            if (data.Length <= 1)
            {
                return record;
            }

            record.RoId = GetValue(data, "ID").ToLong();
            record.PorchNum = GetValue(data, "PORCH");
            record.Capacity = GetValue(data, "CAPACITY");
            record.StopCount = GetValue(data, "STOP");
            record.InstallationYear = GetValue(data, "YEAR_INSTALLATION");
            record.Lifetime = GetValue(data, "LIFETIME");
            record.Year = GetValue(data, "YEAR");
            record.Period = GetValue(data, "PERIOD");
            record.SerialNumber = GetValue(data, "SERIAL_NUMBER");
            record.Ocenka = GetValue(data, "OCENKA");
            record.Sum = GetValue(data, "SUM");

            return record;
        }

        private string GetValue(GkhExcelCell[] data, string field)
        {
            var result = string.Empty;

            if (_headers.ContainsKey(field))
            {
                var index = _headers[field];
                if (data.Length > index && index > -1)
                {
                    result = data[index].Value;
                }
                else
                {
                    return null;
                }
            }

            return result == null ? string.Empty : result.Trim();
        }

        private void InitLog(string fileName)
        {
            if (!Container.Kernel.HasComponent(typeof (ILogImportManager)))
            {
                throw new ImportException("Не найдена реализация интерфейса ILogImportManager");
            }

            this.LogImportManager.FileNameWithoutExtention = fileName;
            this.LogImportManager.UploadDate = DateTime.Now;

            if (this.LogImport == null)
            {
                throw new ImportException("Не найдена реализация интерфейса ILogImport");
            }

            this.LogImport.SetFileName(fileName);
            this.LogImport.ImportKey = Key;
        }

        private void AddLog(int rowNum, string message, bool success = true, Exception ex = null)
        {
            if (!_logDict.ContainsKey(rowNum))
            {
                _logDict[rowNum] = new Dictionary<bool, List<string>>();
            }

            var log = _logDict[rowNum];

            if (!log.ContainsKey(success))
            {
                log[success] = new List<string>();
            }

            log[success].Add(message);

            if (ex.IsNotNull())
            {
                Logger.LogError(ex, "Ошибка в импорте лифтов");
            }
        }

        private void WriteLogs()
        {
            foreach (var log in _logDict.OrderBy(x => x.Key))
            {
                var rowNumber = string.Format("Строка {0}", log.Key);

                foreach (var rowLog in log.Value)
                {
                    if (rowLog.Key)
                    {
                        this.LogImport.Info(rowNumber, string.Join(". ", rowLog.Value));
                        this.LogImport.CountAddedRows++;
                    }
                    else
                    {
                        this.LogImport.Warn(rowNumber, string.Join(". ", rowLog.Value));
                    }
                }
            }
        }
    }
}