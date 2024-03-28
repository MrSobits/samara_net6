namespace Bars.Gkh.FormatDataExport.FormatProvider.CsvFormat
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities.Administration.FormatDataExport;
    using Bars.Gkh.FormatDataExport.NetworkWorker;
    using Bars.Gkh.FormatDataExport.NetworkWorker.Responses;

    using NHibernate.Util;

    /// <summary>
    /// Экспорт данных в csv с последующей передачей в РИС
    /// </summary>
    public class NetCsvFormatProvider : CsvFormatProvider
    {
        public IFormatDataTransferService FormatDataTransferService { get; set; }

        /// <inheritdoc />
        protected override IDataResult ExportData(string pathToSave)
        {
            this.DataSelectorParams["IsSeparateArch"] = false;
            this.DataSelectorParams["MaxFileSize"] = 2047; // Максимальный размер архива 2 Гб
            var numberOfSegments = base.ExportData(pathToSave).Data.ToInt();
            if (File.Exists(pathToSave))
            {
                return this.SendData(pathToSave, numberOfSegments);
            }
            else
            {
                throw new FileNotFoundException("Не удалось получить файл для экспорта");
            }
        }

        private IDataResult SendData(string dataPath, int numberOfSegments)
        {
            var uploadResult = this.FormatDataTransferService.UploadFile(dataPath, this.CancellationToken, numberOfSegments);
            var result = new List<IDataResult>();
            var cleanFiles = DataSelectorParams.GetAs<bool>("CleanFiles");

            try
            {
                foreach (var uploadItem in uploadResult)
                {
                    var remoteResult = new FormatDataExportRemoteResult();
                    if (uploadItem.Success && uploadItem.Data is UploadSuccess remoteFile)
                    {
                        remoteResult.FileId = remoteFile.Id;

                        var importStatus = this.FormatDataTransferService.StartImport(remoteFile.Id, this.CancellationToken);
                        var taskResult = importStatus.Data as DataSuccess;
                        if (!importStatus.Success)
                        {
                            this.AddErrorToLog("StartImport", importStatus.Message, importStatus.Data as Exception);
                        }
                        else if (taskResult != null)
                        {
                            remoteResult.TaskId = taskResult.Id;
                            remoteResult.Status = taskResult.Status;
                        }
                    }
                    else if (!uploadItem.Success)
                    {
                        this.AddErrorToLog("UploadFile", uploadItem.Message, uploadItem.Data as Exception);
                    }

                    remoteResult.UploadResult = uploadItem;
                    result.Add(uploadItem);
                }

                return new BaseDataResult(result.LastOrDefault());
            }
            finally
            {
                if (cleanFiles)
                {
                    File.Delete(dataPath);
                    
                    for (int i = 1; i <= numberOfSegments; i++)
                    {
                        File.Delete(dataPath.Replace(".zip", $".z{i:00}"));
                    }
                }
            }
        }
    }
}