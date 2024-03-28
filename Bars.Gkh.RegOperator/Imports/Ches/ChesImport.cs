namespace Bars.Gkh.RegOperator.Imports.Ches
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;

    using Bars.B4;
    using Bars.B4.IoC;
    using Microsoft.Extensions.Logging;
    using Bars.B4.Modules.Security;
    using Bars.B4.Modules.Tasks.Common.Entities;
    using Bars.B4.Utils;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums.Import;
    using Bars.Gkh.Extensions;
    using Bars.Gkh.Import;
    using Bars.Gkh.Import.Impl;
    using Bars.Gkh.RegOperator.DomainService.Import.Ches;
    using Bars.Gkh.RegOperator.Entities.Import.Ches;
    using Bars.Gkh.RegOperator.Enums;
    using Bars.Gkh.RegOperator.Imports.Ches.PreImport;
    using Bars.Gkh.Repositories.ChargePeriod;

    using Ionic.Zip;

    using ChesImportEntity = Entities.Import.Ches.ChesImport;

    /// <summary>
    /// Импорт сведений от биллинга
    /// </summary>
    public class ChesImport : GkhImportBase
    {
        private readonly IDictionary<ChargePeriod, ChesImportEntity> importDataDict = new Dictionary<ChargePeriod, ChesImportEntity>();
        private readonly IDictionary<ChargePeriod, ChesImportState> importDataStateDict = new Dictionary<ChargePeriod, ChesImportState>();

        public IChesTempDataProviderBuilder ProviderBuilder { get; set; }

        /// <summary>
        /// Идентификатор импорта
        /// </summary>
        public static string Id = MethodBase.GetCurrentMethod().DeclaringType.FullName;

        /// <summary>
        /// Наименование импорта
        /// </summary>
        public static string ImportName = "Импорт сведений от биллинга";

        #region Properties

        /// <summary>
        /// Идентификатор импорта
        /// </summary>
        public override string Key
        {
            get { return ChesImport.Id; }
        }

        /// <summary>
        /// Код импорта
        /// </summary>
        public override string CodeImport
        {
            get { return "ChesImport"; }
        }

        /// <summary>
        /// Наименование импорта
        /// </summary>
        public override string Name
        {
            get { return ChesImport.ImportName; }
        }

        /// <summary>
        /// Допустимые расширения
        /// </summary>
        public override string PossibleFileExtensions
        {
            get { return "zip"; }
        }

        /// <summary>
        /// Права доступа
        /// </summary>
        public override string PermissionName
        {
            get { return "Import.ChesImport.View"; }
        }

        /// <summary>
        /// Репозиторий для <see cref="ChargePeriod"/>
        /// </summary>
        public IChargePeriodRepository PeriodRepository { get; set; }
        public IGkhUserManager UserManager { get; set; }
        public IDomainService<Entities.Import.Ches.ChesImport> ChesImportDomain { get; set; }
        public IDomainService<Entities.Import.Ches.ChesImportFile> ChesImportFileDomain { get; set; }
        public IBillingFilterService BillingFilterService { get; set; }
        public ILogger LogManager { get; set; }
        #endregion Properties

        private bool isTemporaryImport;

        /// <summary>
        /// Провалидировать файл
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <param name="message">Сообщение</param>
        /// <returns>Результат валидации</returns>
        public override bool Validate(BaseParams baseParams, out string message)
        {
            var importFromTables = baseParams.Params.GetAs("importFromTables", false);
            message = null;

            if (importFromTables)
            {
                if (!baseParams.Params.ContainsKey("periodId"))
                {
                    message = "Не передан параметр: Период";
                    return false;
                }
                if (!baseParams.Params.ContainsKey("fileTypes"))
                {
                    message = "Не передан параметр: Секции";
                    return false;
                }
            }
            else
            {
                if (!baseParams.Files.ContainsKey("FileImport"))
                {
                    message = "Не выбран файл для импорта";
                    return false;
                }

                var fileData = baseParams.Files["FileImport"];
                var extention = fileData.Extention;

                var fileExtentions = this.PossibleFileExtensions.Contains(",")
                    ? this.PossibleFileExtensions.Split(',')
                    : new[] { this.PossibleFileExtensions };

                if (fileExtentions.All(x => x != extention))
                {
                    message = $"Необходимо выбрать файл с допустимым расширением: {this.PossibleFileExtensions}";
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Импортировать файл
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns>Результат выполнения импорта</returns>
        public override ImportResult Import(BaseParams baseParams)
        {
            var importFromTables = baseParams.Params.GetAs("importFromTables", false);

            // маркер загрузки во временные таблицы может быть true, только если грузим из файлов
            this.isTemporaryImport = baseParams.Params.GetAs<bool>("IsTemporaryImport") && !importFromTables;
            this.BillingFilterService.Initialize(DateTime.Now);

            if (importFromTables)
            {
                return this.ImportFromTables(baseParams);
            }
            else
            {
                return this.ImportFromFile(baseParams);
            }
        }

        private ImportResult ImportFromFile(BaseParams baseParams)
        {
            var file = baseParams.Files["FileImport"];
            if (file == null)
            {
                return new ImportResult(StatusImport.CompletedWithError) { Message = "Нет файла для импорта" };
            }

            this.InitLog(file.FileName);

            using (var stream = new MemoryStream(file.Data))
            {
                using (var zipFile = ZipFile.Read(stream))
                {
                    var zipEntries = zipFile.Where(x => x.FileName.EndsWith(".csv")).ToArray();
                    if (zipEntries.Length < 1)
                    {
                        this.LogImport.Error("Ошибка", "Отсутствуют файлы для импорта");
                        return new ImportResult(StatusImport.CompletedWithError, "Отсутствуют файлы для импорта");
                    }

                    try
                    {
                        this.LogImport.Info("Настройки импорта", this.BillingFilterService.ConfigDescription);
                        this.BillingFilterService.ValidateConfig();

                        var fileList = zipEntries.Select(this.GetFileInfo).ToList();

                        //если не импорт во временные таблицы, то требуем загрузки Calc
                        if (!this.isTemporaryImport)
                        {
                            if (fileList.Any(x => x.FileType == FileType.CalcProt || x.FileType == FileType.SaldoChange || x.FileType == FileType.Recalc)
                                && fileList.All(x => x.FileType != FileType.Calc))
                            {
                                throw new ImportException(
                                    "В архиве присутствует один из файлов: " +
                                    $"{FileType.CalcProt.GetEnumMeta().Display}, {FileType.SaldoChange.GetEnumMeta().Display}, {FileType.Recalc.GetEnumMeta().Display}; "
                                    +
                                    $"но отсутствует обязательный для них файл {FileType.Calc.GetEnumMeta().Display}");
                            }
                        }

                        this.ImportInternal(fileList, baseParams);
                    }
                    catch (ImportException exception)
                    {
                        this.LogImport.Error("Ошибка", $"Ошибка при чтении файлов: {exception.Message}");
                    }
                    catch (Exception exception)
                    {
                        this.LogImport.Error("Ошибка", $"Необработанное исключение во время импорта файла: {exception.Message}");
                    }
                }
            }

            this.LogImportManager.Add(file, this.LogImport);
            this.LogImportManager.Save();

            return new ImportResult(
                this.LogImport.CountError > 0 ? StatusImport.CompletedWithError : StatusImport.CompletedWithoutError,
                $"Импортировано {this.LogImport.CountAddedRows} записей/файлов",
                string.Empty,
                this.LogImportManager.LogFileId);
        }

        private ImportResult ImportFromTables(BaseParams baseParams)
        {
            var types = baseParams.Params.GetAs<FileType[]>("fileTypes");
            var periodId = baseParams.Params.GetAsId("periodId");
            var period = this.PeriodRepository.Get(periodId);
            var chesImport = this.ChesImportDomain.GetAll().First(x => x.Period == period);

            try
            {
                this.InitLog(period.Name);

                var fileList = types.Select(x => this.GetFileInfo(x, period)).ToList();

                var anyFileRequiresCalcFile = fileList.Any(x => x.FileType == FileType.CalcProt || x.FileType == FileType.SaldoChange || x.FileType == FileType.Recalc);
                var hasImporterCalcFile = fileList.Any(x => x.FileType == FileType.Calc) || chesImport.ImportedFiles.Contains(FileType.Calc);

                if (anyFileRequiresCalcFile && !hasImporterCalcFile)
                {
                    throw new ImportException(
                        "В архиве присутствует один из файлов: " +
                        $"{FileType.CalcProt.GetEnumMeta().Display}, {FileType.SaldoChange.GetEnumMeta().Display}, {FileType.Recalc.GetEnumMeta().Display}; " +
                        $"но отсутствует или не загружен обязательный для них файл {FileType.Calc.GetEnumMeta().Display}");
                }

                this.ImportInternal(fileList, baseParams);
            }
            catch (ImportException exception)
            {
                this.LogImport.Error("Ошибка", $"Ошибка при чтении файлов: {exception.Message}");
                this.LogManager.LogError(exception, "Ошибка");
            }
            catch (Exception exception)
            {
                this.LogImport.Error("Ошибка", $"Необработанное исключение во время импорта файла: {exception.Message}");
                this.LogManager.LogError(exception, "Ошибка");
            }

            this.LogImportManager.AddLog(this.LogImport);
            this.LogImportManager.Save();

            return new ImportResult(
                this.LogImport.CountError > 0 ? StatusImport.CompletedWithError : StatusImport.CompletedWithoutError,
                $"Импортировано {this.LogImport.CountAddedRows} записей/файлов",
                string.Empty,
                this.LogImportManager.LogFileId);
        }

        private void ImportInternal(List<ImportFileInfo> fileList, BaseParams baseParams)
        {
            if (this.isTemporaryImport)
            {
                this.ImportFilesToTemp(fileList);
            }
            else
            {
                this.ImportFiles(fileList, baseParams);
            }
        }

        private ImportFileInfo GetFileInfo(ZipEntry zipEntry)
        {
            var fileType = this.GetFileType(zipEntry.FileName);
            var fileData = this.GetFileData(zipEntry);

            return fileType == FileType.Pay
                ? ChesFileInfoProvider.GetFileInfoEx(fileType, fileData, this.LogImport, this.Indicate)
                : ChesFileInfoProvider.GetFileInfo(fileType, fileData, this.LogImport, this.Indicate);
        }

        private ImportFileInfo GetFileInfo(FileType type, ChargePeriod period)
        {
            return ChesFileInfoProvider.GetFileInfo(type, period.StartDate.Year, period.StartDate.Month, logImport: this.LogImport, indicate: this.Indicate);
        }

        private void ImportFiles(List<ImportFileInfo> fileList, BaseParams baseParams)
        {
            this.TryImportFile(fileList.FirstOrDefault(x => x.FileType == FileType.Account) as AccountFileInfo);
            this.TryImportFile(fileList.FirstOrDefault(x => x.FileType == FileType.Calc) as CalcFileInfo);
            this.TryImportFile(fileList.FirstOrDefault(x => x.FileType == FileType.CalcProt) as CalcProtFileInfo);
            this.TryImportFile(fileList.FirstOrDefault(x => x.FileType == FileType.Recalc) as RecalcFileInfo);
            this.TryImportFile(fileList.FirstOrDefault(x => x.FileType == FileType.Pay) as PayFileInfo, baseParams);
            this.TryImportFile(fileList.FirstOrDefault(x => x.FileType == FileType.SaldoChange) as SaldoChangeFileInfo);
        }

        private void TryImportFile<T>(T fileInfo, BaseParams baseParams = null, string importerId = null)
            where T : ImportFileInfo
        {
            if (fileInfo != null)
            {
                var importer = string.IsNullOrEmpty(importerId)
                    ? this.Container.Resolve<IChesImporter<T>>()
                    : this.Container.Resolve<IChesImporter<T>>(importerId);

                using (this.Container.Using(importer))
                {
                    importer.SetLogImport(this.LogImport);
                    importer.SetBillingFilterService(this.BillingFilterService);
                    importer.Indicate = fileInfo.Indicate;
                    importer.SetBaseParams(baseParams);
                    importer.Import(fileInfo);
                }
            }
        }

        private void ImportFilesToTemp(List<ImportFileInfo> fileList)
        {
            var count = fileList.Count + 1;
            var current = 1;
            fileList.Where(x => x.Is<IPeriodImportFileInfo>()).ForEach(x =>
            { 
                this.Indicate(100 * current++ / count, "Загрузка секций");
                this.ImportSection(x);
            });

            this.Indicate(99, "Сохранение результатов");
            this.Container.InStatelessTransaction(session =>
            {
                foreach (var kvp in this.importDataDict)
                {
                    kvp.Value.State = this.importDataStateDict[kvp.Key];
                    session.InsertOrUpdate(kvp.Value);
                }
            });
        }

        private void ImportSection(ImportFileInfo fileInfo)
        {
            var importer = this.ProviderBuilder.Build(fileInfo as IPeriodImportFileInfo);

            var chesImport = this.importDataDict.Get(importer.Period);
            ChesImportFile chesImportFile = null;
            if (chesImport.IsNull())
            {
                chesImport = this.ChesImportDomain.GetAll()
                    .FirstOrDefault(z => z.Period == importer.Period) ?? new ChesImportEntity();

               long? userID = UserManager.GetActiveUser()?.Id ?? UserManager.GetActiveOperator()?.Id ?? null;

               if(!userID.HasValue)
                    throw new ValidationException("Ваш пользователь не может в импорт");

                chesImport.User = new User { Id = userID.Value};

                chesImport.Task = new TaskEntry { Id = this.TaskId };
                chesImport.Period = importer.Period;

                chesImport.State = ChesImportState.Loading;
                this.Container.InStatelessTransaction(session => session.InsertOrUpdate(chesImport));

                this.importDataDict[importer.Period] = chesImport;
                this.importDataStateDict[importer.Period] = ChesImportState.Loaded;
            }

            if (fileInfo.FileType != FileType.Pay)
            {
                chesImportFile = this.ChesImportFileDomain.GetAll()
                    .Where(x => x.ChesImport.Id == chesImport.Id)
                    .FirstOrDefault(x => x.FileType == fileInfo.FileType) ?? new ChesImportFile { ChesImport = chesImport, FileType = fileInfo.FileType };
                this.Container.InStatelessTransaction(session => session.InsertOrUpdate(chesImportFile));
            }

            try
            {
                importer.Import();

                if (fileInfo is AccountFileInfo)
                {
                    this.Container.UsingForResolved<IChesComparingService>((cnt, serv) => serv.ProcessAccountImported(importer));
                }

                chesImport.LoadedFiles.Add(fileInfo.FileType);
                this.Container.InStatelessTransaction(session => session.InsertOrUpdate(chesImport));
                this.LogImport.CountAddedRows++;
            }
            catch (ValidationException exception)
            {
                this.LogImport.Error("Ошибка", exception.Message);
                this.importDataStateDict[chesImport.Period] = ChesImportState.LoadedWithError;
            }
        }

        private FileData GetFileData(ZipEntry x)
        {
            FileData fileData;
            using (var ms = new MemoryStream())
            {
                x.Extract(ms);
                ms.Seek(0, SeekOrigin.Begin);

                var data = ms.ToArray();
                fileData = new FileData(Path.GetFileNameWithoutExtension(x.FileName), "csv", data);
            }

            return fileData;
        }

        private FileType GetFileType(string fileName)
        {
            var fileType = FileType.Unknown;
            if (fileName.ToLower().StartsWith("account"))
            {
                fileType = FileType.Account;
            }
            else if (fileName.ToLower().StartsWith("calc_"))
            {
                fileType = FileType.Calc;
            }
            else if (fileName.ToLower().StartsWith("calcprot_"))
            {
                fileType = FileType.CalcProt;
            }
            else if (fileName.ToLower().StartsWith("saldochange_"))
            {
                fileType = FileType.SaldoChange;
            }
            else if (fileName.ToLower().StartsWith("recalc_"))
            {
                fileType = FileType.Recalc;
            }
            else if (fileName.ToLower().StartsWith("pay_"))
            {
                fileType = FileType.Pay;
            }

            return fileType;
        } 
    }
}