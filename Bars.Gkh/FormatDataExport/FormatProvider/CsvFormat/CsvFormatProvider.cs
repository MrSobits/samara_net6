namespace Bars.Gkh.FormatDataExport.FormatProvider.CsvFormat
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Text;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.FormatDataExport.ExportableEntities;

    using Ionic.Zip;

    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Выгрузка данных в csv формат
    /// </summary>
    public class CsvFormatProvider : BaseFormatProvider
    {
        public override IList<string> ServiceEntityCodes { get; } = new List<string> { "_INFO" };

        private const string FileDirectoryName = "files";
        private CsvFileList csvFileList;
        private const int DefaultMaxArchiveSize = 1024 * 1024 * 200; // 200 MB
        private readonly Encoding defaultArchiveEncoding = Encoding.GetEncoding(866);
        private const float EntitiesPart = 50;
        private const float FilesPart = 90;
        protected float Progress = 0f;

        /// <inheritdoc />
        public override string FormatVersion => BaseExportableEntity.Version;

        /// <inheritdoc />
        protected override IDataResult ExportData(string pathToSave)
        {
            var entityDelta = CsvFormatProvider.EntitiesPart / this.ExportableEntities.Count;

            var isSeparateArch = this.DataSelectorParams.GetAs("IsSeparateArch", false);

            var pathSection = pathToSave;

            var pathFiles = string.Empty;

            if (isSeparateArch)
            {
                var path = pathToSave.ToLower().Replace(".zip", "");
                pathSection = path.Append("_sections.zip");
                pathFiles = path.Append("_files.zip");
            }

            using (var zipArchiveSection = new ZipFile(pathSection, this.defaultArchiveEncoding))
            {
                var maxFileSize = this.DataSelectorParams.GetAs("MaxFileSize", 0) * 1024 * 1024;
                zipArchiveSection.MaxOutputSegmentSize = maxFileSize != 0 ? maxFileSize : CsvFormatProvider.DefaultMaxArchiveSize;

                this.csvFileList = new CsvFileList();

                var infoEntity = this.GetEntityData(zipArchiveSection, entityDelta);

                this.ProgressNotify(CsvFormatProvider.EntitiesPart);

                this.CancellationToken.ThrowIfCancellationRequested();

                if (isSeparateArch)
                {
                    if (this.FileEntityCollection.Count > 0)
                    {
                        zipArchiveSection.AddDirectoryByName(CsvFormatProvider.FileDirectoryName);
                        using (var zipArchiveFiles = new ZipFile(pathFiles, this.defaultArchiveEncoding))
                        {
                            this.AddFilesToArchive(zipArchiveFiles);
                            this.AddEntry(zipArchiveFiles, infoEntity);
                            this.AddCsvFileList(zipArchiveFiles, true);
                            zipArchiveFiles.Save();
                        }
                    }
                }
                else
                {
                    this.AddFilesToArchive(zipArchiveSection);
                }

                this.AddCsvFileList(zipArchiveSection);

                this.CancellationToken.ThrowIfCancellationRequested();

                zipArchiveSection.Save();

                return new BaseDataResult(zipArchiveSection.NumberOfSegmentsForMostRecentSave);
            }
        }

        private CsvFileData GetEntityData(ZipFile zipArchiveFile, float entityDelta)
        {
            var sw = new Stopwatch();
            CsvFileData infoEntity = null;
            foreach (var entity in this.ExportableEntities)
            {
                this.CancellationToken.ThrowIfCancellationRequested();

                this.LogManager.LogDebug($"Экспорт сущности '{entity.Code}'");
                sw.Start();
                try
                {
                    var csvData = this.GetCsv(entity);

                    if (csvData.Data == null)
                    {
                        if (this.SelectedEntityCodeList.Contains(entity.Code))
                        {
                            this.AddErrorToLog(entity.Code, "Нет данных для экспорта информации");
                        }
                        else
                        {
                            this.AddWarningToLog(entity.Code, "Нет данных для экспорта информации");
                        }

                        this.LogManager.LogDebug($"Нет данных для экспорта информации с кодом: '{entity.Code}'");
                        continue;
                    }

                    if (csvData.IsEmpty())
                    {
                        this.AddWarningToLog(entity.Code, "Данные не изменялись");
                        this.LogManager.LogDebug($"Отсутствуют данные по причине совпадения с предыдущей дельтой: '{entity.Code}'");
                        continue;
                    }

                    this.AddEntry(zipArchiveFile, csvData);

                    if (entity.Code == "_INFO")
                    {
                        infoEntity = csvData;
                    }
                }
                catch (Exception exception)
                {
                    this.AddErrorToLog(entity.Code, exception.Message, exception);
                    this.LogManager.LogError(exception, $"Ошибка экспорта информации с кодом: '{entity.Code}'");
                }
                finally
                {
                    sw.Stop();

                    this.AddErrorRecordsToLog(entity);

                    this.LogManager.LogDebug($"Экспорт сущности '{entity.Code}' завершен: {sw.Elapsed}");
                    sw.Reset();

                    this.Container.Release(entity);

                    this.Progress += entityDelta;
                    this.ProgressNotify(this.Progress);
                }
            }

            return infoEntity;
        }

        private CsvFileData GetCsv(IExportableEntity entity)
        {
            if (!DataSelectorParams.ContainsKey("SectionGroupNames"))
            {
                DataSelectorParams.Add("SectionGroupNames", SectionGroupNames);
            }
            
            var dataResult = entity.GetData(this.DataSelectorParams);
            return CsvHelper.GetContent(entity.Code.ToLowerInvariant(), entity.GetHeader(), dataResult.Data, dataResult.Success);
        }

        private CsvFileData GetExpotrableFiles()
        {
            var exportableFiles = this.FileEntityCollection as IExportableEntity;
            if (exportableFiles != null)
            {
                return this.GetCsv(exportableFiles);
            }
            else
            {
                return CsvFileData.CreateEmpty();
            }
        }

        private void AddFilesToArchive(ZipFile zipArchiveFile)
        {
            if (this.FileEntityCollection.Count == 0)
            {
                return;
            }

            var csvData = this.GetExpotrableFiles();

            if (!csvData.IsEmpty())
            {
                this.AddEntry(zipArchiveFile, csvData);
            }

            zipArchiveFile.AddDirectoryByName(CsvFormatProvider.FileDirectoryName);

            var filesDelta = (CsvFormatProvider.FilesPart - CsvFormatProvider.EntitiesPart)
                / this.FileEntityCollection.Count;

            foreach (var fileInfo in this.FileEntityCollection.GetFileStreams())
            {
                var fileName = Path.Combine(CsvFormatProvider.FileDirectoryName, fileInfo.Name);

                if (!zipArchiveFile.ContainsEntry(fileName))
                {
                    zipArchiveFile.AddEntry(fileName, fileInfo.FileStream);
                }
               
                this.Progress += filesDelta;
                this.ProgressNotify(this.Progress);
            }

            this.ProgressNotify(CsvFormatProvider.FilesPart);
        }

        private void AddCsvFileList(ZipFile zipArchiveFile, bool onlyBaseFiles = false)
        {
            var fileList = onlyBaseFiles ? this.csvFileList.GetBaseCsvFileData() : this.csvFileList.GetCsvFileData();
            zipArchiveFile.AddEntry(fileList.FullName, fileList.Data ?? new byte[0]);
        }

        private void AddEntry(ZipFile zipArchiveFile, CsvFileData csvData)
        {
            if (csvData == null)
            {
                return;
            }

            zipArchiveFile.AddEntry(csvData.FullName, csvData.Data ?? new byte[0]);
            this.csvFileList.Add(csvData);

            this.ExportedEntityCodeList.Add(csvData.FileName.ToUpper());
        }
    }
}