namespace Bars.B4.Modules.FIAS.AutoUpdater.Impl
{
<<<<<<< HEAD
    using Bars.B4.DataAccess;
=======
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Threading;

    using Bars.B4.IoC;
>>>>>>> net6
    using Bars.B4.Logging;
    using Bars.B4.Modules.FIAS.AutoUpdater.ArchiveReader;
    using Bars.B4.Modules.FIAS.AutoUpdater.Converter;
    using Bars.B4.Modules.FIAS.AutoUpdater.DownloadService;
    using Bars.B4.Modules.FIAS.AutoUpdater.Helpers;
    using Bars.B4.Modules.Tasks.Common.Service;
    using Bars.B4.Utils.Annotations;
    using Castle.Core.Internal;
    using Castle.Windsor;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading;

    /// <summary>
    /// Сервис автоматического обновления справочника ФИАС
    /// </summary>
    public class FiasAutoUpdater : IFiasAutoUpdater
    {
        #region Properties

        public IWindsorContainer Container { get; set; }

        public ILogManager LogManager { get; set; }

        public IFiasDownloadService FiasDownloadService { get; set; }

        public IRepository<FiasHouse> FiasHouseRepo { get; set; }

        public IRepository<Fias> FiasRepo { get; set; }

        #endregion

        #region Fields

        /// <summary>
        /// Список скачанных файлов
        /// </summary>
        private readonly HashSet<string> updateFiles = new HashSet<string>();

        #endregion

        #region Public methods

        /// <summary>
        /// Скачивание архивов фиаса с dbf
        /// </summary>
        public virtual IDataResult Download(BaseParams baseParams, CancellationToken ct, IProgressIndicator indicator = null)
        {
            try
            {
                var isDelta = baseParams.Params.GetAs("delta", false, true);

                var downloadResult = isDelta ? FiasDownloadService.GetLastDeltaUpdate(baseParams, ct, indicator) : FiasDownloadService.GetLastFullUpdate(baseParams, ct, indicator);

                AddFileToCollect(downloadResult.Data as string);

                return downloadResult;
            }
            catch (Exception e)
            {
                return new BaseDataResult(false, "Download error: " + e.Message);
            }
        }

        /// <summary>
        /// Обновление фиас табличек из фиаса
        /// </summary>
        public virtual IDataResult Update(BaseParams baseParams, CancellationToken ct, IProgressIndicator indicator = null)
        {
            ct.ThrowIfCancellationRequested();

<<<<<<< HEAD
            var archReader = this.Container.Resolve<IFiasArchiveReader>();
            var dbConverter = this.Container.Resolve<IFiasDbConverter>();
            try
=======
            var isDbfFormatUpdate = baseParams.Params.GetAs("dbfformatupdate", false, true);
            var archReader = isDbfFormatUpdate
                ? this.Container.Resolve<IFiasArchiveReader>("DbfFiasArchiveReader")
                : this.Container.Resolve<IFiasArchiveReader>("GarFiasArchiveReader");
            var dbConverter = isDbfFormatUpdate
                ? this.Container.Resolve<IFiasDbConverter>("DbfFiasConverter")
                : this.Container.Resolve<IFiasDbConverter>("GarFiasConverter");
            var tableUpdater = this.Container.Resolve<ITableUpdater>();

            using(this.Container.Using(archReader, dbConverter, tableUpdater))
>>>>>>> net6
            {
                var archFilePath = baseParams.Params.GetAs("archFilePath", string.Empty, true);
                ArgumentChecker.NotNullOrEmpty(archFilePath, nameof(archFilePath));

                AddFileToCollect(archFilePath);

<<<<<<< HEAD
                //распаковка архива в словарь 
                var unpackResult = archReader.UnpackFiles(archFilePath, ct, indicator);
=======
                LogManager.Info("Обновление ФИАС: начат процесс распаковки файлов архива");
                var unpackResult = archReader.UnpackFiles(archFilePath);
>>>>>>> net6
                if (!unpackResult.Success)
                {
                    LogManager.Info($"Обновление ФИАС: ошибка при распаковки файлов архива: {unpackResult.Message}");
                    return unpackResult;
                }
<<<<<<< HEAD
                this.AddFileRangeToCollect(unpackResult.Data.Values);
                ct.ThrowIfCancellationRequested();

                //TODO: очистка таблиц
                var cleanTable = !baseParams.Params.GetAs("delta", false, true);

                //-----сохранение в базу-----
                //Fias 
                if (unpackResult.Data.ContainsKey(archReader.FiasEntityName))
                {
                    indicator?.Report(null, 0, $"Парсинг таблицы Fias");
                    var fiasRecords = dbConverter.GetFiasRecords(unpackResult.Data[archReader.FiasEntityName]);

                    int entriescount = fiasRecords.Count();

                    int progressPrescaler = entriescount / 100 != 0 ? entriescount / 100 : 1;
                    int i = 0;
                    foreach (var fiasRecord in fiasRecords)
                    {
                        if (i++ % progressPrescaler == 0)
                            indicator?.Report(null, (uint)(i * 100 / entriescount), $"Сохранение записи FIAS {i} из {entriescount}");
                        UpdateFiasRecord(fiasRecord);
                        ct.ThrowIfCancellationRequested();
                    }
                }

                //FiasHouse
                if (unpackResult.Data.ContainsKey(archReader.FiasHouseEntityName))
                {
                    indicator?.Report(null, 0, $"Парсинг таблицы FiasHouse");
                    var fiasHouseRecords = dbConverter.GetFiasHouseRecords(unpackResult.Data[archReader.FiasHouseEntityName]);

                    int entriescount = fiasHouseRecords.Count();

                    int progressPrescaler = entriescount / 100 != 0 ? entriescount / 100 : 1;
                    int i = 0;
                    foreach (var houseRecord in fiasHouseRecords)
                    {
                        if (i++ % progressPrescaler == 0)
                            indicator?.Report(null, (uint)(i * 100 / entriescount), $"Сохранение записи FiasHouse {i} из {entriescount}");
                        UpdateHouseRecord(houseRecord);
                        ct.ThrowIfCancellationRequested();
                    }
                }

                return new BaseDataResult();
            }
            finally
            {
                this.Container.Release(archReader);
                this.Container.Release(dbConverter);
            }
=======
                LogManager.Info("Обновление ФИАС: распаковка файлов архива завершена");

                this.AddFileRangeToCollect(archReader);
                this.CancellationToken.ThrowIfCancellationRequested();

                var isDelta = baseParams.Params.GetAs("delta", false, true);
                List<Fias> fiasRecords;
                List<FiasHouse> fiasHouseRecords;
                try
                {
                    fiasRecords = dbConverter.GetFiasRecords(archReader, isDelta).ToList();
                    LogManager.Info("Обновление ФИАС: конвертация адресных объектов завершена");
                    
                    fiasHouseRecords = dbConverter.GetFiasHouseRecords(archReader, isDelta).ToList();
                    LogManager.Info("Обновление ФИАС: конвертация домов завершена");
                }
                catch (Exception ex)
                {
                    LogManager.Info("Обновление ФИАС: произошла ошибка при конвертации", ex);
                    return BaseDataResult.Error(ex.Message);
                }

                try
                {
                    this.Container.InTransaction(() =>
                    {
                        LogManager.Info("Обновление ФИАС: начат процесс обновления адресных объектов");
                        tableUpdater.UpdateRecords(fiasRecords, !isDelta);
                        this.CancellationToken.ThrowIfCancellationRequested();
                        LogManager.Info("Обновление ФИАС: обновление адресных объектов завершено");
                        fiasRecords = null;

                        LogManager.Info("Обновление ФИАС: начат процесс обновления домов");
                        tableUpdater.UpdateRecords(fiasHouseRecords, !isDelta);
                        this.CancellationToken.ThrowIfCancellationRequested();
                        LogManager.Info("Обновление ФИАС: обновление домов завершено");
                    });
                    LogManager.Info("Обновление ФИАС: изменения применены, обновление завершено");
                }
                catch (Exception ex)
                {
                    LogManager.Info("Обновление ФИАС: произошла ошибка при обновлении", ex);
                    return BaseDataResult.Error(ex.Message);
                }
                

                return new BaseDataResult();
            }
>>>>>>> net6
        }

        /// <summary>
        /// Очистка скачанных файлов
        /// </summary>
        /// <returns></returns>
        public IDataResult CleanFiles(CancellationToken ct, IProgressIndicator indicator = null)
        {
            int entriescount = updateFiles.Count();
            int i = 0;
            foreach (var file in updateFiles)
            {
                indicator?.Report(null, (uint)(i++ * 100 / entriescount), $"Удаление файла {file}");
                if (File.Exists(file))
                {
                    File.Delete(file);
                    ct.ThrowIfCancellationRequested();
                }
            }

            return new BaseDataResult();
        }

        #endregion

        #region Private methods

        private void UpdateFiasRecord(AddressObjectsObject fiasRecord)
        {
            //TODO: AOId сделать Guid типа
            var oldRecords = FiasRepo.GetAll().Where(x => x.AOId == fiasRecord.AOID);
            if (oldRecords.Any())
            {
                //TODO: добавить ключ уникальности в таблицу
                var oldRecordsArray = oldRecords.ToArray();
                //первую модифицируем
                var onceRecord = oldRecordsArray[0];
                ConvertHelper.CopyToFias(fiasRecord, onceRecord);
                FiasRepo.Update(onceRecord);
                FiasRepo.Evict(onceRecord);
                //остальные удаляем
                for (int i = 1; i < oldRecordsArray.Count(); i++)
                {
                    FiasRepo.Delete(oldRecordsArray[i]);
                }
            }
            else
            {
                //сохраняем новую
                var fias = ConvertHelper.GetFias(fiasRecord);
                FiasRepo.Save(fias);
                FiasRepo.Evict(fias);
            }            
        }

        private void UpdateHouseRecord(HousesHouse houseRecord)
        {
            var oldRecords = FiasHouseRepo.GetAll().Where(x => x.HouseId == Guid.Parse(houseRecord.HOUSEID));
            if (oldRecords.Any())
            {
                //TODO: добавить ключ уникальности в таблицу
                var oldRecordsArray = oldRecords.ToArray();
                //первую модифицируем
                var onceRecord = oldRecordsArray[0];
                ConvertHelper.CopyToFiasHouse(houseRecord, onceRecord);
                FiasHouseRepo.Update(onceRecord);
                FiasHouseRepo.Evict(onceRecord);
                //остальные удаляем
                for (int i = 1; i < oldRecordsArray.Count(); i++)
                {
                    FiasHouseRepo.Delete(oldRecordsArray[i]);
                }
            }
            else
            {
                //сохраняем новую
                var fiasHouse = ConvertHelper.GetFiasHouse(houseRecord);
                FiasHouseRepo.Save(fiasHouse);
                FiasHouseRepo.Evict(fiasHouse);
            }
        }

        private void AddFileToCollect(string filePath)
        {
            if (!string.IsNullOrWhiteSpace(filePath))
            {
                this.updateFiles.Add(filePath.ToLowerInvariant());
            }
        }

        private void AddFileRangeToCollect(IFiasArchiveReader reader)
        {
            var filePathsCollection = reader
                    .FiasLinkedFilesDict
                    .Union(reader.FiasHouseLinkedFilesDict)
                    .Select(x => x.Value);

            filePathsCollection.ForEach(this.AddFileToCollect);
        }

        #endregion
    }
}