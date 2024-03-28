namespace Bars.Gkh.Regions.Tatarstan.Import.Fssp.CourtOrderGku
{
    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.B4.Modules.DataExport;
    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Enums.Import;
    using Bars.Gkh.Import;
    using Bars.Gkh.Import.Impl;
    using Bars.Gkh.Regions.Tatarstan.Dto;
    using Bars.Gkh.Regions.Tatarstan.Entities.Fssp.CourtOrderGku;
    using Bars.Gkh.Regions.Tatarstan.Enums;
    using Bars.GkhExcel;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using FileInfo = B4.Modules.FileStorage.FileInfo;

    public class CourtOrderGkuInfoImport : GkhImportBase
    {
        #region Свойства
        public static string Id = MethodBase.GetCurrentMethod().DeclaringType.FullName;

        /// <inheritdoc />
        public override string Key => CourtOrderGkuInfoImport.Id;

        /// <inheritdoc />
        public override string CodeImport => "UploadDownloadInfoImport";

        /// <inheritdoc />
        public override string Name => "Импорт информации об исполнительных производствах";

        /// <inheritdoc />
        public override string PossibleFileExtensions => "csv,xls,xlsx";

        /// <inheritdoc />
        public override string PermissionName => string.Empty;
        #endregion

        public IDomainService<UploadDownloadInfo> UploadDownloadInfoDomain { get; set; }
        public IDomainService<FsspAddress> FsspAddressDomain { get; set; }
        public IDomainService<PgmuAddress> PgmuAddressDomain { get; set; }
        public IDomainService<FsspAddressMatch> FsspAddressMatchDomain { get; set; }
        public IDomainService<Litigation> LitigationDomain { get; set; }
        public IFileManager FileManager { get; set; }

        private UploadDownloadInfo uploadDownloadInfo;
        private readonly List<LogRecord> log = new List<LogRecord>();
        private List<CourtOrderUploadFileInfoDto> records = new List<CourtOrderUploadFileInfoDto>();
        private Dictionary<long, Litigation> litigationDict = new Dictionary<long, Litigation>();

        /// <inheritdoc />
        public override bool Validate(BaseParams baseParams, out string message)
        {
            message = string.Empty;
            var fileCount = baseParams.Files.Count;
            var id = baseParams.Params.GetAsId();

            if (id == 0 && (fileCount == 0 || baseParams.Files.First().Value == null))
            {
                message = "Не выбран файл для импорта";

                return false;
            }

            return true;
        }

        /// <summary>
        /// Проверка строк на соответствие формату
        /// </summary>
        /// <param name="rows">Строки из файла</param>
        /// <returns>Результат проверки</returns>
        private bool Validate(List<string[]> rows)
        {
            return !rows
                .Any(x => x.Length != 8
                    || x[1].Trim().Length > 255
                    || x[2].Trim().Length > 80
                    || x[3].Trim().Length > 40
                    || x[4].Trim().ToDateTime() == DateTime.MinValue
                    || !decimal.TryParse(x[5].Trim().Replace(CultureInfo.InvariantCulture.NumberFormat.CurrencyDecimalSeparator,
                        CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator), out _)
                    || x[6].Trim().Length > 100
                    || x[7].Trim().Length > 100
                    || x[7].Trim().Split(',').Length != 8);
        }

        /// <inheritdoc />
        public override ImportResult Import(BaseParams baseParams)
        {
            try
            {
                var id = baseParams.Params.GetAsId();
                this.uploadDownloadInfo = this.UploadDownloadInfoDomain.Get(id);

                this.uploadDownloadInfo.Status = FsspFileState.InProcess;
                this.UploadDownloadInfoDomain.Update(this.uploadDownloadInfo);

                var fileStream = this.FileManager.GetFile(this.uploadDownloadInfo.DownloadFile);

                IGkhExcelProvider excelProvider;

                var rows = new List<string[]>();
                if (this.uploadDownloadInfo.DownloadFile.Extention == "csv")
                {
                    using (var sr = new StreamReader(fileStream, Encoding.GetEncoding(1251)))
                    {
                        string line;
                        while ((line = sr.ReadLine()) != null)
                        {
                            rows.Add(line.Split(';'));
                        }
                    }
                }
                else
                {
                    using (this.Container.Using(excelProvider = this.Container.Resolve<IGkhExcelProvider>("ExcelEngineProvider")))
                    {
                        if (this.uploadDownloadInfo.DownloadFile.Extention == "xlsx")
                        {
                            excelProvider.UseVersionXlsx();
                        }

                        excelProvider.Open(fileStream);
                        rows = excelProvider.GetRows(0, 0)
                            .Select(x => Array.ConvertAll(x, y => y.Value))
                            .ToList();
                    }
                }

                if (rows.Count < 2)
                {
                    this.AddLog(null, "Файл не содержит записей");
                    this.SaveLogAndStatus(FsspFileState.Failed);
                    return new ImportResult(StatusImport.CompletedWithError, string.Empty, string.Empty, 0);
                }

                // Удаляем строку с заголовком
                rows.RemoveAt(0);

                //Выводим записи с ошибками в лог
                rows.ForEach((row, i) =>
                {
                    if (row[4].Trim().IsEmpty())
                    {
                        this.AddLog(i, $@"Данные делопроизводства по адресу ""{row[7]}"" не были загружены, " +
                                                 "так как не заполнено обязательное поле \"Дата создания ИП\"");
                    }

                    if (row[5].Trim().IsEmpty())
                    {
                        this.AddLog(i, $@"Данные делопроизводства по адресу ""{row[7]}"" не были загружены, " +
                                                "так как не заполнено обязательное поле \"Сумма задолженности по ИП\"");
                    }
                });

                //Оставляем записи без ошибок
                rows = rows.Where(x => x[4].Trim().IsNotEmpty() && x[5].Trim().IsNotEmpty()).ToList();

                if (!this.Validate(rows))
                {
                    this.AddLog(null, "Не соответствует формат загруженного файла.");
                    this.SaveLogAndStatus(FsspFileState.Failed);
                    return new ImportResult(StatusImport.CompletedWithError, string.Empty, string.Empty, 0);
                }

                this.records = rows
                    .Select((x, i) =>
                    {
                        if (long.TryParse(x[0], out var outerId))
                        {
                            return new CourtOrderUploadFileInfoDto
                            {
                                RowNum = i,
                                OuterId = outerId,
                                JurInstitution = x[1].Trim(),
                                State = x[2].Trim(),
                                RegNumber = x[3].Trim(),
                                EntrepreneurCreateDate = x[4].Trim().ToDateTime(),
                                EntrepreneurDebtSum = x[5].Trim().ToDecimal(),
                                Debtor = x[6].Trim(),
                                Address = x[7]
                            };
                        }

                        this.AddLog(i, "Не удалось распознать id ИП");
                        return null;
                    })
                    .Where(x => x.IsNotNull())
                    .ToList();

                var outerIds = this.records.Select(x => x.OuterId);

                this.litigationDict = this.LitigationDomain.GetAll()
                    .Where(x => outerIds.Contains(x.OuterId))
                    .ToDictionary(x => x.OuterId, y => y);

                foreach (var rec in this.records)
                {
                    if (this.litigationDict.TryGetValue(rec.OuterId, out var litigation))
                    {
                        litigation.JurInstitution = rec.JurInstitution;
                        litigation.State = rec.State;
                        litigation.IndEntrRegistrationNumber = rec.RegNumber;
                        litigation.Debtor = rec.Debtor;
                        litigation.EntrepreneurCreateDate = rec.EntrepreneurCreateDate;
                        litigation.EntrepreneurDebtSum = rec.EntrepreneurDebtSum;

                        if (litigation.DebtorFsspAddress.Address != rec.Address)
                        {
                            litigation.DebtorFsspAddress.Address = rec.Address;
                            litigation.DebtorFsspAddress.PgmuAddress = MatchPgmuAddress(rec);
                        }
                        else
                        {
                            litigation.DebtorFsspAddress.PgmuAddress = litigation.DebtorFsspAddress.PgmuAddress ?? MatchPgmuAddress(rec);
                        }

                        this.LitigationDomain.Update(litigation);
                        rec.PgmuAddress = litigation.DebtorFsspAddress.PgmuAddress;
                    }
                    else
                    {
                        var fsspAddress = new FsspAddress
                        {
                            Address = rec.Address,
                            PgmuAddress = MatchPgmuAddress(rec)
                        };
                        this.FsspAddressDomain.Save(fsspAddress);

                        var fsspAddressMatch = new FsspAddressMatch
                        {
                            UploadDownloadInfo = this.uploadDownloadInfo,
                            FsspAddress = fsspAddress
                        };
                        this.FsspAddressMatchDomain.Save(fsspAddressMatch);

                        var newLitigation = new Litigation
                        {
                            OuterId = rec.OuterId,
                            JurInstitution = rec.JurInstitution,
                            State = rec.State,
                            IndEntrRegistrationNumber = rec.RegNumber,
                            Debtor = rec.Debtor,
                            DebtorFsspAddress = fsspAddress,
                            EntrepreneurCreateDate = rec.EntrepreneurCreateDate,
                            EntrepreneurDebtSum = rec.EntrepreneurDebtSum
                        };

                        this.LitigationDomain.Save(newLitigation);  
                        rec.PgmuAddress = newLitigation.DebtorFsspAddress.PgmuAddress;
                    }
                }

                if (!this.log.Any())
                {
                    this.uploadDownloadInfo.Status = FsspFileState.Success;
                }
                else if (this.records.Any(x => x.PgmuAddress.IsNotNull()))
                {
                    this.uploadDownloadInfo.Status = FsspFileState.UploadWithErrors;
                }
                else
                {
                    this.uploadDownloadInfo.Status = FsspFileState.Failed;
                }

                var resultStates = new[] { FsspFileState.Success, FsspFileState.UploadWithErrors };
                var logStates = new[] { FsspFileState.UploadWithErrors, FsspFileState.Failed };

                if (resultStates.Contains(this.uploadDownloadInfo.Status))
                {
                    var connPgmu = baseParams.Params.GetAs<string>("connPgmu");
                    var uploadFile = this.CreateCourtOrderInfoReport(connPgmu);

                    this.uploadDownloadInfo.UploadFile = uploadFile;
                }

                if (logStates.Contains(this.uploadDownloadInfo.Status))
                {
                    this.GenerateLogFile();
                }

                this.UploadDownloadInfoDomain.Update(this.uploadDownloadInfo);
            }
            catch (Exception e)
            {
                this.AddLog(null, $"Произошла ошибка: {e.Message}");
                this.SaveLogAndStatus(FsspFileState.Failed);
            }

            return new ImportResult(StatusImport.CompletedWithoutError, string.Empty, string.Empty, 0);
        }

        /// <summary>
        /// Сопоставление адреса ПГМУ
        /// </summary>
        /// <param name="rec">Запись из файла</param>
        /// <returns>адрес ПГМУ</returns>
        private PgmuAddress MatchPgmuAddress(CourtOrderUploadFileInfoDto rec)
        {
            var addressFields = rec.Address.Trim().Split(',');
            var address = this.PgmuAddressDomain.GetAll()
                .FirstOrDefault(x =>
                        (((x.PostCode.Trim() == "-" || x.PostCode == null) && (addressFields[0].Trim() == "-" || addressFields[0].Trim() == "")) ||
                        x.PostCode.Trim().ToLower() == addressFields[0].ToLower().Trim()) &&

                        (((x.Town.Trim() == "-" || x.Town == null) && (addressFields[1].Trim() == "-" || addressFields[1].Trim() == "")) ||
                        x.Town.Trim().ToLower() == addressFields[1].ToLower().Trim()) &&

                        (((x.District.Trim() == "-" || x.District == null) && (addressFields[2].Trim() == "-" || addressFields[2].Trim() == "")) ||
                        x.District.Trim().ToLower() == addressFields[2].ToLower().Trim()) &&

                        (((x.Street.Trim() == "-" || x.Street == null) && (addressFields[3].Trim() == "-" || addressFields[3].Trim() == "")) ||
                        x.Street.Trim().ToLower() == addressFields[3].ToLower().Trim()) &&

                        (((x.House.Trim() == "-" || x.House == null) && (addressFields[4].Trim() == "-" || addressFields[4].Trim() == "")) ||
                        x.House.Trim().ToLower() == addressFields[4].ToLower().Trim()) &&

                        (((x.Building.Trim() == "-" || x.Building == null) && (addressFields[5].Trim() == "-" || addressFields[5].Trim() == "")) ||
                        x.Building.Trim().ToLower() == addressFields[5].ToLower().Trim()) &&

                        (((x.Apartment.Trim() == "-" || x.Apartment == null) && (addressFields[6].Trim() == "-" || addressFields[6].Trim() == "")) ||
                        x.Apartment.Trim().ToLower() == addressFields[6].ToLower().Trim()) &&

                        (((x.Room.Trim() == "-" || x.Room == null) && (addressFields[7].Trim() == "-" || addressFields[7].Trim() == "")) ||
                        x.Room.Trim().ToLower() == addressFields[7].ToLower().Trim()));

            if (address.IsNull())
            {
                this.AddLog(rec.RowNum, $"Данные делопроизводства по адресу \"{rec.Address}\" не были загружены," +
                    " так как адрес не сопоставился автоматически.  Необходимо перейти в раздел сопоставления адресов и" +
                    " сопоставить адреса вручную. После ручного сопоставления нажать кнопку \"Обновить\" в столбце \"Обновить файлы\"," +
                    " чтобы повторить загрузку файла.");
            }

            return address;
        }

        /// <summary>
        /// Формирование файла выгрузки
        /// </summary>
        private FileInfo CreateCourtOrderInfoReport(string connPgmu)
        {
            var printForm = this.Container.Resolve<IDataExportReport>("CourtOrderInfoUploadReport");
            var generator = this.Container.Resolve<IReportGenerator>("XlsIoGenerator");

            using (this.Container.Using(printForm, generator))
            {
                var rp = new ReportParams();
                rp.SimpleReportParams.СписокПараметров.Add("records", this.records.Where(x => x.PgmuAddress.IsNotNull()).ToList());
                rp.SimpleReportParams.СписокПараметров.Add("connPgmu", connPgmu);
                printForm.PrepareReport(rp);
                var template = printForm.GetTemplate();

                using (var result = new MemoryStream())
                {
                    generator.Open(template);
                    generator.Generate(result, rp);
                    result.Seek(0, SeekOrigin.Begin);

                    var date = DateTime.Now;
                    var timeStr = $"{date.Day}{date.Month}{date.Year}_{date.Hour}{date.Minute}{date.Second}";

                    return this.FileManager.SaveFile(result, $"Исполнительные_делопроизводства__{timeStr}.xlsx");
                }
            }
        }

        /// <summary>
        /// Запись лога и статуса импорта
        /// </summary>
        /// <param name="state">Статус импорта</param>
        private void SaveLogAndStatus(FsspFileState state)
        {
            this.GenerateLogFile();

            this.uploadDownloadInfo.Status = state;
            this.UploadDownloadInfoDomain.Update(this.uploadDownloadInfo);
        }

        /// <summary>
        /// Сгенерировать лог-файл
        /// </summary>
        private void GenerateLogFile()
        {
            var printForm = this.Container.Resolve<IDataExportReport>("LogReport");
            var generator = this.Container.Resolve<IReportGenerator>("XlsIoGenerator");

            using (this.Container.Using(printForm, generator))
            {
                var rp = new ReportParams();
                var sec = rp.ComplexReportParams.ДобавитьСекцию("Section");

                foreach (var rec in this.log)
                {
                    sec.ДобавитьСтроку();
                    sec["Row"] = rec.NumberLine;
                    sec["Message"] = rec.Message;
                }

                var template = printForm.GetTemplate();

                using (var result = new MemoryStream())
                {
                    generator.Open(template);
                    generator.Generate(result, rp);
                    result.Seek(0, SeekOrigin.Begin);

                    this.uploadDownloadInfo.LogFile = this.FileManager.SaveFile(result,
                        $"ПРОТОКОЛ_ЗАГРУЗКИ_{this.uploadDownloadInfo.DownloadFile.Name}.xlsx");
                }
            }
        }

        /// <summary>
        /// Добавление лога в список логов
        /// </summary>
        /// <param name="row">Номер строки</param>
        /// <param name="message">Сообщение</param>
        private void AddLog(int? row, string message)
        {
            this.log.Add(new LogRecord
            {
                NumberLine = ++row,
                Message = message
            });
        }

        /// <summary>
        /// Сущность лога
        /// </summary>
        private class LogRecord
        {
            /// <summary>
            /// Номер строки
            /// </summary>
            public int? NumberLine { get; set; }

            /// <summary>
            /// Сообщение
            /// </summary>
            public string Message { get; set; }
        }
    }
}