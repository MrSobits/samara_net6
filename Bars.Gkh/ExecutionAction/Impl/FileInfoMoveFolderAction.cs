namespace Bars.Gkh.ExecutionAction.Impl
{
    using System;
    using System.IO;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Application;
    using Bars.B4.Config;
    using Bars.B4.DataAccess;

    using FileInfo = Bars.B4.Modules.FileStorage.FileInfo;

    /// <summary>
    /// Действие по переносу существующих файлов из текущей папки FileDirectory в папку FileDirectory\Year\Month
    /// </summary>
    public class FileInfoMoveFolderAction : BaseExecutionAction
    {
        private DirectoryInfo filesDirectory;

        /// <summary>
        /// Контейнер
        /// </summary>
        /// <summary>
        /// Домен-сервис для <see cref="B4.Modules.FileStorage.FileInfo" />
        /// </summary>
        public IDomainService<FileInfo> FileInfoDomainService { get; set; }

        /// <summary>
        /// Интерфейс провайдера конфигураций
        /// </summary>
        public IConfigProvider ConfigProvider { get; set; }

        /// <summary>
        /// Провайдер сессий NHibernate
        /// </summary>
        public ISessionProvider SessionProvider { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        public override string Description => "Перенос существующих файлов из текущей папки FileDirectory в папку FileDirectory\\Year\\Month";

        /// <summary>
        /// Наименование действия
        /// </summary>
        public override string Name => "Перенос существующих файлов из текущей папки FileDirectory в папку FileDirectory\\Year\\Month";

        /// <summary>
        /// Действие
        /// </summary>
        public override Func<IDataResult> Action => this.MoveFiles;

        private DirectoryInfo FilesDirectory
        {
            get
            {
                if (this.filesDirectory != null)
                {
                    return this.filesDirectory;
                }
                else
                {
                    var path = this.ConfigProvider.GetConfig().ModulesConfig["Bars.B4.Modules.FileSystemStorage"].GetAs("FileDirectory", string.Empty);

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

        /// <summary>
        /// Переместить файлы
        /// </summary>
        public BaseDataResult MoveFiles()
        {
            var filesQuery = this.FileInfoDomainService.GetAll()
                .OrderBy(x => x.Id);

            var totalCount = filesQuery.Count();
            var take = 10000;
            for (int skip = 0; skip < totalCount; skip += take)
            {
                try
                {
                    var fileInfos = filesQuery.Skip(skip).Take(take);
                    foreach (var fileInfo in fileInfos)
                    {
                        this.Move(fileInfo);
                    }
                }
                finally
                {
                    this.SessionProvider.GetCurrentSession().Clear();
                }
            }

            return new BaseDataResult();
        }

        private void Move(FileInfo fileInfo)
        {
            var currentFilePath = Path.Combine(
                this.FilesDirectory.FullName,
                string.Format("{0}.{1}", fileInfo.Id, fileInfo.Extention));

            var file = new System.IO.FileInfo(currentFilePath);
            if (file.Exists)
            {
                var newFilePath = Path.Combine(
                    this.FilesDirectory.FullName,
                    fileInfo.ObjectCreateDate.Year.ToString(),
                    fileInfo.ObjectCreateDate.Month.ToString(),
                    string.Format("{0}.{1}", fileInfo.Id, fileInfo.Extention));

                var newDirectoryPath = Path.GetDirectoryName(newFilePath);
                if (!Directory.Exists(newDirectoryPath))
                {
                    Directory.CreateDirectory(newDirectoryPath);
                }

                file.MoveTo(newFilePath);
            }
        }
    }
}