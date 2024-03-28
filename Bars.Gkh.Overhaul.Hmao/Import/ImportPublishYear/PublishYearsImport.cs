using System;
using System.Collections.Generic;
using System.Linq;

namespace Bars.Gkh.Overhaul.Hmao.Import.ImportPublishYear
{
    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Microsoft.Extensions.Logging;
    using Bars.B4.Utils;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Enums.Import;
    using Bars.Gkh.Import;
    using Bars.Gkh.Import.Impl;
    using Bars.Gkh.Overhaul.Hmao.ConfigSections;
    using Bars.Gkh.Overhaul.Hmao.DomainService.Version;
    using Bars.Gkh.Overhaul.Hmao.Entities;
    using Bars.Gkh.Overhaul.Hmao.Entities.Version;
    using Bars.Gkh.Overhaul.Hmao.Enum;
    using Bars.Gkh.Overhaul.Hmao.Reports;
    using Bars.Gkh.Utils;
    using Npgsql;
    using System.Data;
    using System.IO;

    /// <summary>
    /// Импорт сведений о сроках проведения капитального ремонта
    /// </summary>
    public class PublishYearsImport : GkhImportBase
    {
        /// <summary>
        /// Лог сервис
        /// </summary>
        public IActualizeVersionLogService<ImportPublishYearLogRecord, ImportPublishYearLogReport> LogService { get; set; }

        /// <summary>
        /// Идентификатор импорта
        /// </summary>
        public static string Id = "PublishYearsImport";

        /// <inheritdoc/>
        public override string Key => PublishYearsImport.Id;

        /// <inheritdoc/>
        public override string CodeImport => PublishYearsImport.Id;

        /// <inheritdoc/>
        public override string Name => "Импорт сведений о сроках проведения капитального ремонта";

        /// <inheritdoc/>
        public override string PossibleFileExtensions => "xls, xlsx";

        /// <inheritdoc/>
        public override string PermissionName => "Ovrhl.LongProgram.Import_View";

        /// <inheritdoc cref="PublishYearsImport" />
        public PublishYearsImport(ILogImportManager logImportManager, ILogImport logImport)
        {
            this.LogImportManager = logImportManager;
            this.LogImport = logImport;
        }

        /// <inheritdoc/>
        public override ImportResult Import(BaseParams baseParams)
        {
            var versionId = baseParams.Params.GetAsId("versionId");
            var fileData = baseParams.Files["FileImport"];
            var result = false;
            var message = string.Empty;

            var logDomain = this.Container.ResolveDomain<VersionActualizeLog>();
            var programVersionDomain = this.Container.ResolveDomain<ProgramVersion>();
            var versionRecordDomain = this.Container.ResolveDomain<VersionRecord>();
            var publishedRecordDomain = this.Container.ResolveDomain<PublishedProgramRecord>();
            var verReStage1Domain = this.Container.ResolveDomain<VersionRecordStage1>();
            var verReStage2Domain = this.Container.ResolveDomain<VersionRecordStage2>();
            var versionActualizeLogRecordDomain = this.Container.ResolveDomain<VersionActualizeLogRecord>();
            var sessionProvider = this.Container.Resolve<ISessionProvider>();

            var userManager = this.Container.Resolve<IGkhUserManager>();

            using (this.Container.Using(programVersionDomain, userManager, logDomain, publishedRecordDomain,
                versionRecordDomain, versionActualizeLogRecordDomain, verReStage1Domain, verReStage2Domain))
            {
                var logRecords = new List<ImportPublishYearLogRecord>();
                var publishedRecForUpdate = new List<PublishedProgramRecord>();
                var versionRecForUpdate = new List<VersionRecord>();

                var programVersion = programVersionDomain.Get(versionId);

                var importData = this.GetImportData(fileData);

                var importDict = importData
                    .Where(x => x.Municipality == programVersion.Municipality.Name)
                    .Where(x => x.NewYear.HasValue)
                    .Select(x => new { x.Id, x.NewYear })
                    .ToDictionary(x => x.Id, y => y.NewYear);

                var publishedRecord = publishedRecordDomain.GetAll()
                    .Where(x => x.PublishedProgram.ProgramVersion.Id == versionId)
                    .Where(x => importDict.Keys.Contains(x.Id))
                    .ToList();

                var logValidation = this.Validation(importData, publishedRecord);
                if (logValidation?.Any() ?? false)
                {
                    logRecords.AddRange(logValidation);
                }

                if (!logRecords.Any())
                {
                    foreach (var item in publishedRecord)
                    {
                        var newYear = (int)importDict[item.Id];

                        logRecords.Add(new ImportPublishYearLogRecord
                        {
                            Id = item.Id,
                            Address = item.Address,
                            Ceo = item.CommonEstateobject,
                            Number = item.IndexNumber,
                            Sum = item.Sum,
                            PublishYear = item.PublishedYear,
                            ChangePublishYear = newYear,
                            Note = string.Empty
                        });

                        item.PublishedYear = newYear;
                        item.Stage2.Stage3Version.IsChangedPublishYear = true;

                        publishedRecForUpdate.Add(item);
                        versionRecForUpdate.Add(item.Stage2.Stage3Version);
                    }

                    result = logRecords.Any();
                }

                publishedRecForUpdate.ForEach(x => publishedRecordDomain.Update(x));
                versionRecForUpdate.ForEach(x => versionRecordDomain.Update(x));
                this.LogImport.CountChangedRows = versionRecForUpdate.Count();

                var logFile = this.LogService.CreateLogFile(
                        logRecords.OrderBy(x => x.Address).ThenBy(x => x.Number),
                        baseParams);

                var log = new VersionActualizeLog
                {
                    ActualizeType = VersionActualizeType.ImportPublishYearImport,
                    DateAction = DateTime.Now,
                    Municipality = programVersion.Municipality,
                    ProgramVersion = new ProgramVersion { Id = versionId },
                    UserName = userManager.GetActiveUser().Name,
                    CountActions = logRecords.Count,
                    LogFile = logFile
                };

                logDomain.Save(log);

                var recIds = logRecords.Select(y => y.Id).ToList();
                var logDataDict = publishedRecordDomain.GetAll()
                    .Where(x => recIds.Contains(x.Id))
                    .Join(verReStage2Domain.GetAll(),
                        x => x.Stage2,
                        y => y,
                        (x, y) => new
                        {
                            x.Id,
                            VerRecSt2Id = x.Stage2.Id,
                            RoId = x.RealityObject.Id,
                            y.Stage3Version.WorkCode,
                            PlanYear = y.Stage3Version.Year
                        })
                    .Join(verReStage1Domain.GetAll(),
                        x => x.VerRecSt2Id,
                        y => y.Stage2Version.Id,
                        (x, y) => new
                        {
                            x.Id,
                            x.RoId,
                            x.WorkCode,
                            x.PlanYear,
                            y.Volume
                        })
                    .GroupBy(x => new { x.Id, x.RoId, x.WorkCode, x.PlanYear })
                    .Select(x => new
                    {
                        x.Key.Id,
                        x.Key.RoId,
                        x.Key.WorkCode,
                        x.Key.PlanYear,
                        Volume = x.Sum(y => y.Volume)
                    })
                    .ToDictionary(x => x.Id, y => y);

                var warnItems = logValidation?.Select(y => y.Id).ToList();
                var vmLogRecords = logRecords
                    .Where(x => !warnItems?.Contains(x.Id) ?? true)
                    .Select(x =>
                    {
                        var rec = logDataDict.Get(x.Id);

                        return new VersionActualizeLogRecord
                        {
                            ActualizeLog = log,
                            RealityObject = rec != null ? new Gkh.Entities.RealityObject { Id = rec.RoId } : null,
                            WorkCode = rec?.WorkCode,
                            Ceo = x.Ceo,
                            PlanYear = rec?.PlanYear ?? 0,
                            PublishYear = x.PublishYear ?? 0,
                            ChangePublishYear = x.ChangePublishYear,
                            Volume = rec?.Volume ?? 0,
                            Sum = x.Sum,
                            Number = x.Number
                        };
                    });

                if (vmLogRecords.Any())
                {
                    var connection = sessionProvider.GetCurrentSession().Connection as NpgsqlConnection;

                    using (var importer = connection.BeginBinaryImport(
                                @"COPY OVRHL_ACTUALIZE_LOG_RECORD 
                                (object_version, object_create_date, object_edit_date, actualize_log_id, action, reality_object_id,
                                 work_code, ceo, plan_year, change_plan_year, publish_year, change_publish_year, volume, change_volume,
                                 sum, change_sum, number, change_number) FROM STDIN (FORMAT BINARY)"))
                    {
                        vmLogRecords.ForEach(x =>
                            importer.WriteRow(0L, DateTime.Now, DateTime.Now, log.Id, x.Action, x.RealityObject.Id, x.WorkCode, x.Ceo,
                            x.PlanYear, x.ChangePlanYear, x.PublishYear, x.ChangePublishYear, x.Volume, x.ChangeVolume, x.Sum, x.ChangeSum,
                            x.Number, x.ChangeNumber));

                        importer.Complete();
                    }
                }

                this.LogImport.ImportKey = PublishYearsImport.Id;
                this.LogImportManager.FileNameWithoutExtention = nameof(ImportPublishYearLogReport);
                this.LogImportManager.Add(fileData, logFile, this.LogImport);
                this.LogImportManager.Save();

                message += this.LogImportManager.GetInfo();
                var status = this.LogImportManager.CountError > 0
                    ? StatusImport.CompletedWithError
                    : (this.LogImportManager.CountWarning > 0
                        ? StatusImport.CompletedWithWarning
                        : StatusImport.CompletedWithoutError);

                return new ImportResult(status, message, string.Empty, this.LogImportManager.LogFileId);
            }
        }

        /// <summary>
        /// Получить данные импорта
        /// </summary>
        /// <param name="fileData">Данные из файла</param>
        /// <returns>Список сведений о сроках проведения кап. ремонта</returns>
        private List<ImportPublishYearObject> GetImportData(FileData fileData)
        {
            var importItems = new List<ImportPublishYearObject>();
            var excel = this.Container.Resolve<GkhExcel.IGkhExcelProvider>("ExcelEngineProvider");

            if (fileData.Extention.ToLower() == "xlsx")
            {
                excel.UseVersionXlsx();
            }

            using (this.Container.Using(excel))
            {
                using (var memoryStreamFile = new MemoryStream(fileData.Data))
                {
                    excel.Open(memoryStreamFile);
                    try
                    {
                        // Объекты импорта, полученные из файла
                        importItems.AddRange(excel.GetRows(0, 0)
                            .Skip(1)
                            .Select(x => new ImportPublishYearObject
                            {
                                Id = long.TryParse(x[0].Value, out var id) ? id : default(long),
                                WorkCode = x[1].Value,
                                Municipality = x[2].Value,
                                Address = x[3].Value,
                                CommonEstateObject = x[4].Value,
                                ConstructionElement = x[5].Value,
                                YearPublished = int.TryParse(x[6].Value, out var year) ? year : default(int),
                                NewYear = int.TryParse(x[7].Value, out var newYear) ? (int?)newYear : null
                            }));
                    }
                    catch (Exception e)
                    {
                        this.Container.UsingForResolved<ILogger>((ioc, logManager) =>
                        {
                            logManager.LogDebug("Ошибка при получении данных из Excel-файла", e);
                        });
                    }
                }
            }

            return importItems;
        }

        /// <summary>
        /// Проверка данных для импорта
        /// </summary>
        /// <param name="importItems">Данные для импорта</param>
        /// <param name="publishedRecord">Записи опубликованной программы</param>
        /// <returns>Записи лога импорта</returns>
        private List<ImportPublishYearLogRecord> Validation(IEnumerable<ImportPublishYearObject> importItems, List<PublishedProgramRecord> publishedRecord)
        {
            var validateMsg = new List<LogMessage>();

            var config = this.Container.GetGkhConfig<OverhaulHmaoConfig>();
            var periodStartYear = config.ProgrammPeriodStart;
            var periodEndYear = config.ProgrammPeriodEnd;

            void AddLog(long id, string message)
            {
                validateMsg.Add(new LogMessage
                {
                    Id = id,
                    Message = message
                });
                this.LogImport.CountWarning++;
            }

            foreach (var item in importItems)
            {
                if (item.Id == 0 && item.NewYear.HasValue)
                {
                    AddLog(item.Id, "Некорректно заполнено обязательное поле");
                }

                if (item.NewYear >= periodEndYear || item.NewYear <= periodStartYear)
                {
                    AddLog(item.Id, "Год выходит за пределы периода формирования ДПКР");
                }
            }

            var nonExistInVersionRecords = importItems
                .Where(x => !publishedRecord.Any(y => y.Id == x.Id && y.CommonEstateobject == x.CommonEstateObject) && x.NewYear.HasValue)
                .Select(x => x.Id)
                .ToList();

            if (nonExistInVersionRecords.Any())
            {
                nonExistInVersionRecords.ForEach(x =>
                {
                    AddLog(x, "Запись отсутствует в основной версии программы");
                });
            }

            if (!validateMsg.Any())
            {
                return null;
            }

            var itemsDict = importItems.ToDictionary(x => x.Id);

            return validateMsg
                .GroupBy(x => x.Id)
                .Select(y =>
                {
                    var item = itemsDict[y.Key];

                    return new ImportPublishYearLogRecord
                    {
                        Id = item.Id,
                        Address = item.Address,
                        Ceo = item.CommonEstateObject,
                        PublishYear = item.YearPublished,
                        ChangePublishYear = item.NewYear ?? 0,
                        Note = string.Join(";", y.Select(x => x.Message))
                    };
                }).ToList();
        }

        /// <summary>
        /// Запись лога
        /// </summary>
        private class LogMessage
        {
            public long Id { get; set; }

            public string Message { get; set; }
        }
    }
}