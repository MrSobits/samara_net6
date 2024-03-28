using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace Bars.B4.Modules.Analytics.Reports.DomainServices
{
    using Bars.B4.Config;
    using Bars.B4.DataAccess;
    using Bars.B4.Application;
    using Bars.B4.DomainService.BaseParams;
    using Bars.B4.Modules.Analytics.Reports.Entities.History;
    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Modules.FileStorage.DomainService;

    internal class ReportHistoryDomainService : FileStorageDomainService<ReportHistory>
    {
        public DirectoryInfo filesDirectory;

        public IDomainService<FileInfo> fileInfoDomainService { get; set; }

        public DirectoryInfo FilesDirectory
        {
            get
            {
                if (this.filesDirectory != null)
                {
                    return this.filesDirectory;
                }

                if (this.filesDirectory == null)
                {
                    var path = this.Container.Resolve<IConfigProvider>().GetConfig().ModulesConfig["Bars.B4.Modules.FileSystemStorage"]
                        .GetAs("FileDirectory", string.Empty);

                    this.filesDirectory =
                        new DirectoryInfo(
                            Path.IsPathRooted(path) ? path : ApplicationContext.Current.MapPath("~/" + path.TrimStart('~', '/')));
                }

                if (!this.filesDirectory.Exists)
                {
                    this.filesDirectory.Create();
                }

                return this.filesDirectory;
            }
        }

        public string GetFilePath(FileInfo fileInfoForDelete)
        {
            return this.GetFilePath(fileInfoForDelete.Id, fileInfoForDelete.Extention, fileInfoForDelete.ObjectCreateDate);
        }

        public string GetFilePath(long fileInfoId, string extension, DateTime objectCreateDate)
        {
            return Path.Combine(this.FilesDirectory.FullName,
                objectCreateDate.Year.ToString(),
                objectCreateDate.Month.ToString(),
                string.Format("{0}.{1}", fileInfoId, extension));
        }

        public IDataResult DeleteFile(FileInfo file)
        {
            if (file != null)
            {
                var fileInfo = new System.IO.FileInfo(this.GetFilePath(file));
                if (!fileInfo.Exists)
                {
                    return new FileStorageDataResult { Success = false, Message = "Файл не найден" };
                }

                File.Delete(fileInfo.FullName);

                return new FileStorageDataResult { Success = true, Message = "Файл удален" };
            }

            return new FileStorageDataResult { Success = false, Message = "Файл не найден" };
        }

        public override IDataResult Delete(BaseParams baseParams)
        {
            var ids = Converter.ToLongArray(baseParams.Params, "records");
            var filesForDelete = new List<FileInfo>();

            using (IDataTransaction dataTransaction = this.BeginTransaction())
            {
                foreach (var id in ids)
                {
                    filesForDelete.AddRange(this.GetFileInfoValues(Get(id)));
                    this.Delete((object) id);
                }

                foreach (var file in filesForDelete.Where(file => file != null))
                {
                    this.DeleteFile(file);
                }

                dataTransaction.Commit();
            }

            return new BaseDataResult(ids);

        }
    }
}