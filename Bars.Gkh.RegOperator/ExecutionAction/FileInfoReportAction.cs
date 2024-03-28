namespace Bars.Gkh.ExecutionAction
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Text;

    using Bars.B4;
    using Bars.B4.Application;
    using Bars.B4.Config;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Utils;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount;

    using Ionic.Zip;
    using Ionic.Zlib;

    using FileInfo = Bars.B4.Modules.FileStorage.FileInfo;

    /// <summary>
    /// Действие по проверки файла в директории FileDirectory\\Year\\Month"
    /// </summary>
    public class FileInfoReportAction : BaseExecutionAction
    {
        private DirectoryInfo filesDirectory;

        /// <summary>
        /// Контейнер
        /// </summary>
        /// <summary>
        /// Домен-сервис для <see cref="EntityLogLight" />
        /// </summary>
        public IDomainService<EntityLogLight> EntityLogLightDomainService { get; set; }

        /// <summary>
        /// Домен-сервис для <see cref="PersonalAccountChange" />
        /// </summary>
        public IDomainService<PersonalAccountChange> AccountChangeDomain { get; set; }

        /// <summary>
        /// Домен-сервис для <see cref="BasePersonalAccount" />
        /// </summary>
        public IDomainService<BasePersonalAccount> AccountDomain { get; set; }

        /// <summary>
        /// Домен-сервис для <see cref="LogOperation" />
        /// </summary>
        public IDomainService<LogOperation> LogOperationDomain { get; set; }

        /// <summary>
        /// Пользователь
        /// </summary>
        public IGkhUserManager UserManager { get; set; }

        /// <summary>
        /// Интерфейс провайдера конфигураций
        /// </summary>
        public IConfigProvider ConfigProvider { get; set; }

        /// <summary>
        /// Провайдер сессий NHibernate
        /// </summary>
        public ISessionProvider SessionProvider { get; set; }

        public IFileManager FileManager { get; set; }

        /// <summary>
        /// Код действия
        /// </summary>
        /// <summary>
        /// Описание
        /// </summary>
        public override string Description => "Формирует лог в «Логах операциях»";

        /// <summary>
        /// Наименование действия
        /// </summary>
        public override string Name => "Действие по проверке файла в директории FileDirectory\\Year\\Month";

        /// <summary>
        /// Действие
        /// </summary>
        public override Func<IDataResult> Action => this.Files;

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

        private BaseDataResult Files()
        {
            var logOperation = new LogOperation
            {
                StartDate = DateTime.UtcNow,
                OperationType = LogOperationType.FileReport,
                User = this.UserManager.GetActiveUser()
            };

            var accId = this.AccountDomain.GetAll().Select(x => x.Id);

            var accountChangeFiles = this.AccountChangeDomain.GetAll()
                .Where(x => accId.Contains(x.PersonalAccount.Id))
                .Where(x => x.Document != null)
                .Select(x => new LogFileProxy {File = x.Document, AccId = x.PersonalAccount.Id, AccountNum = x.PersonalAccount.PersonalAccountNum})
                .AsEnumerable();

            var entityLogLightFiles = this.EntityLogLightDomainService.GetAll()
                .Where(x => x.ClassName == "BasePersonalAccount")
                .Where(x => accId.Contains(x.EntityId))
                .Where(x => x.Document != null)
                .Join(
                    this.AccountDomain.GetAll(),
                    z => z.EntityId,
                    y => y.Id,
                    (z, y) => new {Log = z, Acc = y})
                .Select(x => new LogFileProxy {File = x.Log.Document, AccId = x.Log.EntityId, AccountNum = x.Acc.PersonalAccountNum})
                .AsEnumerable().ToList()
                .Union(accountChangeFiles)
                .AsQueryable();

            var totalCount = entityLogLightFiles.Count();
            var take = 100;
            var finalStr = new StringBuilder();
            var numberOfPathsUnfound = 0;
            var numberOfPathsFound = 0;

            for (int skip = 0; skip < totalCount; skip += take)
            {
                foreach (var file in entityLogLightFiles.Skip(skip).Take(take))
                {
                    var newFilePath = Path.Combine(
                        this.FilesDirectory.FullName,
                        file.File.ObjectCreateDate.Year.ToString(),
                        file.File.ObjectCreateDate.Month.ToString(),
                        $"{file.File.Id}.{file.File.Extention}");

                    var fileValidation = new System.IO.FileInfo(newFilePath);

                    if (!fileValidation.Exists)
                    {
                        var currentFilePath = Path.Combine(
                            this.FilesDirectory.FullName,
                            $"{file.File.Id}.{file.File.Extention}");

                        var check = new System.IO.FileInfo(currentFilePath);
                        if (check.Exists)
                        {
                            finalStr.AppendLine($"Лс: {file.AccountNum} ЛсId: {file.AccId} Путь(если есть в FileDirectory): {currentFilePath}");
                            numberOfPathsFound++;
                        }
                        else
                        {
                            finalStr.AppendLine($"Лс: {file.AccountNum} ЛсId: {file.AccId}");
                            numberOfPathsUnfound++;
                        }
                    }
                }
            }
            using (var logFile = new MemoryStream())
            {
                var logsZip = new ZipFile(Encoding.UTF8)
                {
                    CompressionLevel = CompressionLevel.Level3,
                    AlternateEncoding = Encoding.GetEncoding("cp866")
                };

                var log = Encoding.GetEncoding(1251).GetBytes(finalStr.ToString());

                logsZip.AddEntry($"{logOperation.OperationType.GetEnumMeta().Display}.csv", log);

                logsZip.Save(logFile);

                var logFileInfo = this.FileManager.SaveFile(logFile, $"{logOperation.OperationType.GetEnumMeta().Display}.zip");

                logOperation.LogFile = logFileInfo;
            }

            logOperation.EndDate = DateTime.UtcNow;
            logOperation.Comment =
                $"Количество файлов без пути: {numberOfPathsUnfound}, Находится в директории FileDirectory {numberOfPathsFound}. Всего: {numberOfPathsUnfound + numberOfPathsFound} ";

            this.LogOperationDomain.Save(logOperation);
            return new BaseDataResult();
        }

        private class LogFileProxy
        {
            public FileInfo File { get; set; }

            public long AccId { get; set; }

            public string AccountNum { get; set; }
        }
    }
}