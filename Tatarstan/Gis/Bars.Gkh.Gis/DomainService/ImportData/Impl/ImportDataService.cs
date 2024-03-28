namespace Bars.Gkh.Gis.DomainService.ImportData.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Bars.B4;
    using Bars.B4.Config;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Modules.Security;
    using Bars.B4.Utils;
    using Bars.Gkh.Gis.DataResult;
    using Bars.Gkh.Gis.Entities.ImportIncrementalData;
    using Bars.Gkh.Gis.Entities.Register.LoadedFileRegister;
    using Bars.Gkh.Gis.Enum;
    using Bars.Gkh.Gis.RabbitMQ;

    using Castle.MicroKernel.Lifestyle;
    using Castle.MicroKernel.Lifestyle.Scoped;
    using Castle.Windsor;

    /// <summary>
    /// Сервис импорта данных
    /// </summary>
    public class ImportDataService : IImportDataService
    {
        protected IWindsorContainer Container;
        protected IFileManager FileManager;
        protected IRepository<User> UserRepository;
        protected IRepository<LoadedFileRegister> LoadedFileRepository;
        protected IDomainService<OpenTatarstanData> OpenTatarstanDomain;
        protected IProducerService ProducerService;

        protected User CurrentUser;

        protected readonly StringBuilder ImportLog = new StringBuilder();

        public ImportDataService(
            IWindsorContainer container,
            IRepository<LoadedFileRegister> loadedFileRepository,
            IDomainService<OpenTatarstanData> openTatarstanDomain,
            IFileManager fileManager,
            IRepository<User> userRepository)
        {
            this.Container = container;
            this.FileManager = fileManager;
            this.UserRepository = userRepository;
            this.LoadedFileRepository = loadedFileRepository;
            this.OpenTatarstanDomain = openTatarstanDomain;

            var userIdentity = this.Container.Resolve<IUserIdentity>();
            using (this.Container.Using(userIdentity))
            {
                if (userIdentity.IsAuthenticated)
                {
                    this.CurrentUser = this.UserRepository.Load(userIdentity.UserId);
                }
            }
        }

        /// <summary>
        /// Добавляет задачу на импорт файла
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>Результат импорта</returns>
        public ImportDataResult AddTask(BaseParams baseParams)
        {
            var fileDataList = baseParams.Files.Values.ToList();
            var format = baseParams.Params.GetAs<TypeImportFormat>("Format");

            return (ImportDataResult) this.InternalAddTask(fileDataList, format);
        }

        /// <summary>
        /// Импорт файлов
        /// </summary>
        /// <param name="loadedFileList">Загруженные файлы</param>
        /// <returns>Результат импорта файлов</returns>
        public IDataResult Import(IEnumerable<LoadedFileRegister> loadedFileList)
        {
            using (this.Container.BeginScope())
            {
                var loadedFileRegisters = loadedFileList as LoadedFileRegister[] ?? loadedFileList.ToArray();
                try
                {
                    loadedFileRegisters.ForEach(x => this.UpdateImport(x, "", TypeStatus.InProgress));

                    BaseImportDataHandler importHandler;

                    //считаем что файлы одного формата
                    //поэтому формат определяется по первому файлу
                    switch (loadedFileRegisters.First().Format)
                    {
                        case TypeImportFormat.IncPgu:
                        {
                            importHandler = this.Container.Resolve<BaseImportDataHandler>("IncrementalPguHandler2");
                            break;
                        }
                        case TypeImportFormat.IncGis:
                        {
                            importHandler = this.Container.Resolve<BaseImportDataHandler>("IncrementalGisHandler2");
                            break;
                        }

                        case TypeImportFormat.SzDataForMinStroyReport:
                        {
                            importHandler = this.Container.Resolve<BaseImportDataHandler>("SzDataForMinStroyReportHandler2");
                            break;
                        }

                        case TypeImportFormat.GkhDataForMinStroyReport:
                        {
                            importHandler = this.Container.Resolve<BaseImportDataHandler>("GkhDataForMinStroyReportHandler2");
                            break;
                        }

                        default:
                        {
                            return new ImportDataResult(false, "Неизвестный формат");
                        }
                    }

                    var result = importHandler.ImportData(loadedFileRegisters);

                    if (result.Count(x => x.Success == false) > 0)
                    {
                        return new ImportDataResult(false, "Во время загрузки файла произошла ошибка! ");
                    }
                }
                catch (Exception ex)
                {
                    loadedFileRegisters.ForEach(x => this.UpdateImport(x, "Во время загрузки файла произошла ошибка \r\n" + ex, TypeStatus.Error));
                    return new ImportDataResult(false, "Во время загрузки файла произошла ошибка!");
                }
            }

            return new ImportDataResult(true, "Успешно загружено!");
        }

        /// <summary>
        /// Список загруженных файлов
        /// </summary>
        /// <param name="baseParams">Параметры</param>
        /// <returns>Список загруженных файлов</returns>
        public ListDataResult GetLoadedFilesList(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();

            var typeFormat = baseParams.Params.GetAs<TypeImportFormat?>("typeImport");

            var data = this.LoadedFileRepository.GetAll()
                .Where(x => x.Format == typeFormat && x.File != null)
                //если текущий пользователь не админ, то показываем только его файлы
                .WhereIf(this.CurrentUser.Roles.All(y => y.Role.Name != "Администратор"), x => x.B4User == this.CurrentUser)
                .Select(x => new
                {
                    x.Id,
                    x.ObjectCreateDate,
                    UserId = x.B4User,
                    x.FileName,
                    x.Format,
                    x.TypeStatus,
                    FileId = x.File.Id,
                    LogId = x.Log != null ? x.Log.Id : (long?) null
                })
                .ToList()
                .Select(x => new
                {
                    x.Id,
                    ObjectCreateDate = x.ObjectCreateDate.ToUniversalTime(),
                    x.UserId,
                    x.FileName,
                    x.Format,
                    x.TypeStatus,
                    x.FileId,
                    x.LogId
                })
                .AsQueryable()
                .OrderByDescending(x => x.ObjectCreateDate)
                .Filter(loadParams, this.Container);

            return new ListDataResult(data.Paging(loadParams).ToList(), data.Count());
        }

        /// <summary>
        /// Список всех загруженных файлов
        /// </summary>
        /// <param name="baseParams">Параметры</param>
        /// <returns>Список загруженных файлов</returns>
        public ListDataResult GetAllLoadedFiles(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();

            var typeFormat = baseParams.Params.GetAs<TypeImportFormat?>("typeImport");

            var data = this.LoadedFileRepository.GetAll()
                .WhereIf(typeFormat != null, x => x.Format == typeFormat)
                .Where(x => x.File != null)
                .ToList()
                .Select(x => new
                {
                    x.Id,
                    ObjectCreateDate = x.ObjectCreateDate.ToUniversalTime(),
                    UserId = x.B4User,
                    x.FileName,
                    x.Format,
                    x.TypeStatus,
                    FileId = x.File.Id,
                    LogId = x.Log != null ? x.Log.Id : (long?) null
                })
                .AsQueryable()
                .OrderByDescending(x => x.ObjectCreateDate)
                .Filter(loadParams, this.Container);

            return new ListDataResult(data.Paging(loadParams).ToList(), data.Count());
        }

        /// <summary>
        /// Список загрузок в "Открытый Татарстан"
        /// </summary>
        /// <param name="baseParams">Параметры</param>
        /// <returns>Список загрузок</returns>
        public ListDataResult GetOpenTatarstanData(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();

            var data = this.OpenTatarstanDomain.GetAll()
                .Where(x => x.File != null)
                .OrderIf(loadParams.Order.Length == 0, true, x => x.ObjectCreateDate)
                .Select(
                    x => new
                    {
                        x.Id,
                        ObjectCreateDate = x.ObjectCreateDate.ToUniversalTime(),
                        UserName = x.B4User.Name,
                        FileId = x.File == null ? (long?) null : x.File.Id,
                        FileName = x.File == null ? string.Empty : x.File.Name,
                        ImportResult = x.ImportResult.GetEnumMeta().Display,
                        x.ImportName,
                        x.ResponseCode,
                        x.ResponseInfo
                    })
                .Filter(loadParams, this.Container);

            return new ListDataResult(data.Order(loadParams).Skip(loadParams.Start).Take(loadParams.Limit), data.Count());
        }

        /// <summary>
        /// Удалить загруженные данные
        /// </summary>
        public IDataResult DeleteLoadedData(BaseParams baseParams)
        {
            this.LoadedFileRepository.Delete(baseParams.Params.GetAs<long>("Id"));
            return new BaseDataResult(true, "Данные успешно удалены");
        }

        /// <summary>
        /// Удалить загруженные данные "Открытый Татарстан"
        /// </summary>
        public IDataResult DeleteOtData(BaseParams baseParams)
        {
            var id = baseParams.Params.GetAs<long>("Id");

            this.OpenTatarstanDomain.Delete(id);

            return new BaseDataResult(true, "Данные успешно удалены");
        }

        /// <summary>
        /// Добавление задания на импорт файла
        /// </summary>
        /// <param name="fileDataList">Контейнер с файлами</param>
        /// <param name="format">Формат загрузки</param>
        /// <returns></returns>
        private IDataResult InternalAddTask(IEnumerable<FileData> fileDataList, TypeImportFormat format)
        {
            var loadedFileRegisterList = fileDataList.Select(
                x => new LoadedFileRegister
                {
                    ObjectCreateDate = DateTime.Now,
                    ObjectEditDate = DateTime.Now,
                    B4User = this.CurrentUser,
                    FileName = string.Format("{0}.{1}", x.FileName, x.Extention),
                    Size = x.Data.Length,
                    File = this.FileManager.SaveFile(x.FileName, x.Extention, x.Data),
                    TypeStatus = TypeStatus.PreQueuing,
                    Format = format,
                })
                .ToList();

            loadedFileRegisterList.ForEach(x => this.LoadedFileRepository.Save(x));

            var task = new FileTask
            {
                LoadedFileRegisterId = loadedFileRegisterList.FirstOrDefault().Return(x => x.Id),
                Type = typeof(FileTask)
            };

            var appSettings = this.Container.Resolve<IConfigProvider>().GetConfig().GetModuleConfig("Bars.Gkh.Gis");
            if (false //TODO: Убрать false после реализации FileTaskHandler
                && appSettings != null && appSettings[SettingsKeyStore.Enable].To<bool>())
            {
                // добавление задания на импорт файла
                try
                {
                    this.ProducerService.SendMessage(TypeQueueName.ProcessFile.GetEnumMeta().Display, task);
                    loadedFileRegisterList.ForEach(x => x.TypeStatus = TypeStatus.Queuing);
                }
                catch
                {
                    loadedFileRegisterList.ForEach(x => x.TypeStatus = TypeStatus.QueuingError);
                }

                loadedFileRegisterList.ForEach(x => this.LoadedFileRepository.Update(x));
                return new ImportDataResult(true, "Файл(ы) добавлен(ы) в очередь загрузки");
            }

            // импорт файла напрямую
            try
            {
                return this.Import(loadedFileRegisterList);
            }
            catch
            {
                loadedFileRegisterList.ForEach(
                    x =>
                    {
                        x.TypeStatus = TypeStatus.Error;
                        x.ImportResult = ImportResult.Error;
                        this.LoadedFileRepository.Update(x);
                    });
                return new ImportDataResult(false, "Ошибка обработки файла");
            }
        }

        /// <summary>
        /// Сохранить файл логов
        /// </summary>
        /// <param name="logs">объект логов</param>
        /// <param name="messagesHeader"></param>
        /// <param name="messages"></param>
        /// <returns></returns>
        private FileInfo SaveLogs(StringBuilder logs, string messagesHeader = "", StringBuilder messages = null)
        {
            //Сохраняем заголовок
            if (!string.IsNullOrEmpty(messagesHeader))
            {
                logs.AppendLine(messagesHeader);
            }

            //Если есть список сообщений, добавляем их
            if (messages != null)
            {
                logs.Append(messages);
            }
            return this.FileManager.SaveFile(
                "Лог" + DateTime.Now.GetHashCode(),
                "log",
                Encoding.UTF8.GetBytes(logs.ToString()));
        }

        private void UpdateImport(LoadedFileRegister loadedFile, string message, TypeStatus typeStatus)
        {
            if (!message.IsEmpty())
            {
                this.ImportLog.Insert(0, string.Concat(message, Environment.NewLine));
            }

            loadedFile.TypeStatus = typeStatus;
            loadedFile.Log = this.SaveLogs(this.ImportLog);
            this.LoadedFileRepository.Update(loadedFile);
        }
    }
}