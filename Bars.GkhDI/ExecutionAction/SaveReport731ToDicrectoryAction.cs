namespace Bars.GkhDi.ExecutionAction
{
    using System;
    using System.IO;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.Config;
    using Bars.Gkh.ExecutionAction;

    using FileInfo = Bars.B4.Modules.FileStorage.FileInfo;

    public class SaveReport731ToDicrectoryAction : BaseExecutionAction
    {
        public IFileManager _fileManager { get; set; }

        public override string Description => "Сохранить актуальные отчеты для Реформы ЖКХ под своими именами в директории fileStorage";

        public override string Name => "Сохранить актуальные (не позднее 14 дней) отчеты для Реформы ЖКХ под своими именами в директории fileStorage";

        public override Func<IDataResult> Action => this.SaveReport731ToDicrectory;

        public BaseDataResult SaveReport731ToDicrectory()
        {
            var fileRepos = this.Container.ResolveRepository<FileInfo>();

            var appParams = this.Container.Resolve<IGkhParams>().GetParams();

            var needSaveReportToDirecory = appParams.ContainsKey("DI_SaveReport731ToDirectory")
                ? appParams["DI_SaveReport731ToDirectory"].ToString()
                : "";

            var dirPath = needSaveReportToDirecory;
            var filesDirectory = new DirectoryInfo(dirPath);

            try
            {
                if (!filesDirectory.Exists)
                {
                    filesDirectory.Create();
                }
            }
            catch (Exception)
            {
                needSaveReportToDirecory = "";
            }

            if (!string.IsNullOrEmpty(needSaveReportToDirecory))
            {
                var dateActuality = DateTime.Now.Date.AddDays(-14);

                var files =
                    fileRepos.GetAll()
                        .Where(x => x.Name.Contains("report731_year"))
                        .Where(x => x.ObjectCreateDate >= dateActuality)
                        .Where(x => x.Extention.Contains("xlsx"))
                        .GroupBy(x => x.Name)
                        .ToDictionary(x => x.Key, x => x.OrderByDescending(y => y.ObjectCreateDate).FirstOrDefault());

                foreach (var fileInfo in files.Values)
                {
                    Stream file = null;

                    try
                    {
                        file = this._fileManager.GetFile(fileInfo);
                    }
                    catch (Exception)
                    {
                        // todo log
                    }

                    if (file != null)
                    {
                        var buffer = new byte[file.Length];

                        file.Seek(0, SeekOrigin.Begin);
                        file.Read(buffer, 0, buffer.Length);
                        file.Seek(0, SeekOrigin.Begin);

                        File.WriteAllBytes(
                            Path.Combine(
                                filesDirectory.FullName,
                                string.Format("{0}.{1}", fileInfo.Name, "xlsx")),
                            buffer);
                    }
                }
            }
            else
            {
                return new BaseDataResult
                {
                    Success = false,
                    Message = "Не задана папка для хранения отчетов Реформы ЖКХ"
                };
            }

            return new BaseDataResult();
        }
    }
}