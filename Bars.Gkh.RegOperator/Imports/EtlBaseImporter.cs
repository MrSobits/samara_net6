namespace Bars.Gkh.RegOperator.Imports
{
    using System;
    using System.IO;
    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.Domain;
    using Bars.Gkh.ETL;
    using Bars.Gkh.ETL.Entities;
    using Bars.Gkh.Import;

    public abstract class EtlBaseImporter<T> : BaseBillingImporter<T>, IImporter where T : BaseEntity
    {
        public abstract string Code { get; }

        IDataResult IImporter.Import(ImportQueueItem importItem)
        {
            var fileManager = Container.Resolve<IFileManager>();
            var logImport = Container.Resolve<ILogImport>();
            var logImportManager = Container.Resolve<ILogImportManager>();
            var result = new BaseDataResult();
            using (Container.Using(fileManager, logImport, logImportManager))
            {
                Stream stream;
                var fileInfo = importItem.FileInfo;
                try
                {
                    stream = fileManager.GetFile(importItem.FileInfo);
                    logImport.SetFileName(fileInfo.Name);
                    logImport.ImportKey = Code;
                    logImportManager.FileNameWithoutExtention = fileInfo.Name;
                    Import(stream, importItem.FileInfo.Name, logImport);
                }
                catch (Exception e)
                {
                    result.Success = false;
                    logImport.Debug("Критическая ошибка", string.Format("{0} - {1}", e.Message, e.StackTrace));
                    logImport.Error("Ошибка", "Критическая ошибка");
                }
                finally
                {
                    stream = fileManager.GetFile(importItem.FileInfo);
                    logImportManager.Add(stream, fileInfo.Name, logImport);
                    logImportManager.Save();
                }
                return result;
            }
        }

        public abstract IDataResult Validate(ImportQueueItem importItem);
    }
}
