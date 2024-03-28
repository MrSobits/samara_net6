namespace Bars.Gkh.ExecutionAction.Impl
{
    using System;
    using System.IO;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Config;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.Gkh.Utils;

    using FileInfo = Bars.B4.Modules.FileStorage.FileInfo;

    /// <summary>
    /// Удаление лишних файлов в FileStorage
    /// </summary>
    public class ClearFileStorageAction : BaseExecutionAction
    {
        public IConfigProvider ConfigProvider { get; set; }

        public override string Description => "Удаление лишних файлов в FileStorage";

        public override string Name => "Удаление лишних файлов в FileStorage";

        public override Func<IDataResult> Action => this.ClearFiles;

        /// <summary>
        /// Удаление лишних файлов в FileStorage
        /// </summary>
        /// <returns></returns>
        public BaseDataResult ClearFiles()
        {
            var fileInfoDomainService = this.Container.ResolveDomain<FileInfo>();

            using (this.Container.Using(fileInfoDomainService))
            {
                var filesInfos = fileInfoDomainService.GetAll()
                    .Select(x => string.Format("{0}.{1}", x.Id, x.Extention))
                    .ToList();

                var path = this.ConfigProvider.GetConfig().ModulesConfig["Bars.B4.Modules.FileSystemStorage"].GetAs("FileDirectory", string.Empty);

                var yearDirs = Directory.GetDirectories(path);

                int count = 0;

                foreach (var yearDir in yearDirs)
                {
                    var monthDirs = Directory.GetDirectories(yearDir);

                    foreach (var monthDir in monthDirs)
                    {
                        using (var fileManager = new FileManagerHelper())
                        {
                            var files = Directory.GetFiles($"{monthDir}");

                            foreach (var file in files)
                            {
                                var fileName = Path.GetFileName(file) ?? string.Empty;

                                if (!filesInfos.Contains(fileName))
                                {
                                    fileManager.DeleteFile(file);
                                    count++;
                                }
                            }

                            fileManager.Commit();
                        }
                    }
                }

                return new BaseDataResult(true, $"Количество удаленных файлов: {count}");
            }
        }
    }
}