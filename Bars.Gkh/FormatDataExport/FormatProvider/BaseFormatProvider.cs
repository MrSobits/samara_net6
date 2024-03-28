namespace Bars.Gkh.FormatDataExport.FormatProvider
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Modules.Security;
    using Bars.B4.Utils;
    using Bars.B4.Utils.Annotations;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Extensions;
    using Bars.Gkh.FormatDataExport.Domain;
    using Bars.Gkh.FormatDataExport.ExportableEntities;
    using Bars.Gkh.FormatDataExport.ProxySelectors;
    using Bars.Gkh.Repositories.ChargePeriod;
    using Bars.Gkh.Utils;

    using Castle.Windsor;

    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Базовый провайдер работы с данными в системе ЖКХ
    /// </summary>
    public abstract class BaseFormatProvider : IExportFormatProvider, IDisposable
    {
        public event EventHandler<float> OnProgressChanged = delegate { };
        public event EventHandler<ICollection<string>> OnAfterExport = delegate { };

        public IWindsorContainer Container { get; set; }
        public ILogger LogManager { get; set; }

        public IChargePeriodRepository ChargePeriodRepository { get; set; }

        public IFileEntityCollection FileEntityCollection { get; set; }
        public IFormatDataExportRoleService FormatDataExportRoleService { get; set; }

        protected ICollection<string> ExportedEntityCodeList { get; } = new HashSet<string>();

        public IReadOnlyCollection<IExportableEntity> ExportableEntities { get; private set; } = new IExportableEntity[0];

        /// <inheritdoc />
        public IList<string> EntityCodeList => this.ExportableEntities.Select(x => x.Code).ToList();

        /// <inheritdoc />
        public ICollection<string> SelectedEntityCodeList { get; set; }

        /// <inheritdoc />
        public LogOperation LogOperation { get; set; }

        /// <inheritdoc />
        public DynamicDictionary DataSelectorParams { get; } = new DynamicDictionary();

        /// <inheritdoc />
        public abstract string FormatVersion { get; }

        /// <summary>
        /// Список наименований экспортируемых секций
        /// </summary>
        public IList<string>  SectionGroupNames { get; set; }

        /// <summary>
        /// Экспорт данных
        /// </summary>
        protected abstract IDataResult ExportData(string pathToSave);

        public abstract IList<string> ServiceEntityCodes { get; }

        /// <inheritdoc />
        public User User { get; set; }

        /// <inheritdoc />
        public Role UserRole => this.User?.Roles.FirstOrDefault()?.Role;

        /// <inheritdoc />
        public Contragent Contragent { get; set; }

        /// <inheritdoc />
        public CancellationToken CancellationToken { get; set; } = CancellationToken.None;

        private readonly IList<LogRecordEmptyFieldsProxy> emptyFieldsLogRecords = new List<LogRecordEmptyFieldsProxy>();
        private readonly IList<LogRecordErrorProxy> errorLogRecords = new List<LogRecordErrorProxy>();
        private readonly IList<LogRecordWarningProxy> warningLogRecords = new List<LogRecordWarningProxy>();

        /// <inheritdoc />
        public IDataResult Export(string pathToSave)
        {
            var proxySelectorFactory = this.Container.Resolve<IProxySelectorFactory>();
            var formatDataExportFilterService = this.Container.Resolve<IFormatDataExportFilterService>();
            var formatDataExportIncrementalService = this.GetIncrementalService();

            try
            {
                this.InitParams();

                formatDataExportFilterService.Init(this.FormatDataExportRoleService.GetProviderType(this.UserRole),
                    this.DataSelectorParams);

                formatDataExportIncrementalService?.SetIgnoreEtities(this.ServiceEntityCodes);

                this.DataSelectorParams["ProxySelectorFactory"] = proxySelectorFactory;
                this.DataSelectorParams["FormatDataExportFilterService"] = formatDataExportFilterService;
                this.DataSelectorParams["FormatDataExportIncrementalService"] = formatDataExportIncrementalService;

                proxySelectorFactory.SetDefaultSelectorParams(this.DataSelectorParams);
                proxySelectorFactory.SetSelectedEntityCodes(this.SelectedEntityCodeList);

                var result = this.ExportData(pathToSave);

                if (this.HasNoErrors)
                {
                    formatDataExportIncrementalService?.SaveNewDataInfo();
                }

                return result;
            }
            catch(Exception e)
            {
                this.AddErrorToLog(string.Empty, "Инициализация экспорта", e);
                throw;
            }
            finally
            {
                this.SaveLogFile(this.LogOperation);
                this.DataSelectorParams.Clear();
                this.Container.Release(proxySelectorFactory);
                this.Container.Release(formatDataExportFilterService);
                this.Container.Release(formatDataExportIncrementalService);

                this.OnAfterExport(this, this.ExportedEntityCodeList);

                this.ExportableEntities.ForEach(x => this.Container.Release(x));
                this.ExportableEntities = new IExportableEntity[0];
            }
        }

        private IFormatDataExportIncrementalService GetIncrementalService()
        {
            //if (this.DataSelectorParams.GetAs("UseIncremental", false))
            //{
            //    return this.Container.Resolve<IFormatDataExportIncrementalService>();
            //}

            return null;
        }

        /// <summary>
        /// Инициализировать коллекцию экспортируемых сущностей
        /// </summary>
        /// <param name="exportableEntities">Коллекция экспортируемых сущностей</param>
        public void InitExportableEntities(IList<IExportableEntity> exportableEntities)
        {
            this.ExportableEntities = new ReadOnlyCollection<IExportableEntity>(exportableEntities);
        }

        /// <summary>
        /// Инициализация параметров экспорта <see cref="DataSelectorParams"/>
        /// </summary>
        protected virtual void InitParams()
        {
            ArgumentChecker.NotNull(this.User, nameof(this.User));
            ArgumentChecker.NotNull(this.Contragent, nameof(this.Contragent));
            ArgumentChecker.NotNull(this.FileEntityCollection, nameof(this.FileEntityCollection));

            this.DataSelectorParams["Contragent"] = this.Contragent;
            this.DataSelectorParams["FileEntityCollection"] = this.FileEntityCollection;

            if (this.ChargePeriodRepository != null
                && (this.EntityCodeList.Contains("EPD") || this.EntityCodeList.Contains("OPLATA")))
            {
                this.DataSelectorParams["Period"] = this.ChargePeriodRepository.GetLastClosedPeriod();
            }

            this.DataSelectorParams["Info.Date"] = DateTime.Now;
            this.DataSelectorParams["Info.SenderName"] = this.User.Name;
        }

        /// <summary>
        /// Добавить записи с ошибками в лог, если они существуют
        /// </summary>
        /// <param name="entity">Экспортируемая сущность</param>
        protected void AddErrorRecordsToLog(IExportableEntity entity)
        {
            if (entity?.EmptyMandatoryFields.IsNotEmpty() ?? false)
            {
                this.emptyFieldsLogRecords.Add(new LogRecordEmptyFieldsProxy(entity));
            }
        }

        /// <summary>
        /// Добавить в лог сообщение о предупреждении
        /// </summary>
        /// <param name="entityCode">Код сущности</param>
        /// <param name="warningMessage">Сообщение предупреждения</param>
        protected void AddWarningToLog(string entityCode, string warningMessage)
        {
            this.warningLogRecords.Add(new LogRecordWarningProxy
            {
                EntityCode = entityCode,
                Warning = warningMessage
            });
        }

        /// <summary>
        /// Добавить в лог сообщение об ошибке
        /// </summary>
        /// <param name="entityCode">Код сущности</param>
        /// <param name="errorMessage">Сообщение об ошибке</param>
        /// <param name="exception">Исключение</param>
        protected void AddErrorToLog(string entityCode, string errorMessage, Exception exception = null)
        {
            this.errorLogRecords.Add(new LogRecordErrorProxy
            {
                EntityCode = entityCode,
                Error = errorMessage,
                Exception = exception
            });
        }

        /// <summary>
        /// Сохранить лог незаполненных обязательных полей
        /// </summary>
        /// <param name="logOperation"></param>
        protected void SaveLogFile(LogOperation logOperation)
        {
            this.Container.UsingForResolved<IFileManager>((container, fileManager) =>
            {
                using (var memoryStream = new MemoryStream())
                using (var writer = new StreamWriter(memoryStream))
                {
                    var logData = new
                    {
                        ExportWarnings = this.warningLogRecords,
                        ExportErrors = this.errorLogRecords,
                        EmptyFields = this.emptyFieldsLogRecords
                    }.ToJson();

                    writer.Write(logData);
                    writer.Flush();

                    logOperation.LogFile = fileManager.SaveFile(memoryStream, $"{logOperation.OperationType.GetDisplayName()}.json");
                }
            });

            logOperation.EndDate = DateTime.Now;

            this.Container.UsingForResolved<IRepository<LogOperation>>((container, repository) =>
            {
                repository.SaveOrUpdate(logOperation);
            });
        }

        /// <inheritdoc />
        public virtual string SummaryErrors {
            get
            {
                var sb = new StringBuilder();

                if (!this.errorLogRecords.IsEmpty())
                {
                    sb.AppendLine($"Количество ошибок: {this.errorLogRecords.Count}");
                }

                if (!this.emptyFieldsLogRecords.IsEmpty())
                {
                    sb.Append("Ошибки в секциях: ");
                    sb.Append(string.Join(", ",
                        this.emptyFieldsLogRecords
                            .Select(x => $"{x.EntityCode}: {x.ErrorRecordsCount}")));
                }
                return sb.ToString();
            }
        }

        private bool HasNoErrors => this.errorLogRecords.IsEmpty() && this.emptyFieldsLogRecords.IsEmpty();

        protected void ProgressNotify(float value)
        {
            this.OnProgressChanged(this, value);
        }

        /// <inheritdoc />
        public void Dispose()
        {
        }

        /// <summary>
        /// Запись о предупреждении во время экспорта
        /// </summary>
        protected class LogRecordWarningProxy
        {
            /// <summary>
            /// Код сущности
            /// </summary>
            public string EntityCode { get; set; }

            /// <summary>
            /// Предупреждение
            /// </summary>
            public string Warning { get; set; }
        }

        /// <summary>
        /// Запись об ошибке экспорта
        /// </summary>
        protected class LogRecordErrorProxy
        {
            /// <summary>
            /// Код сущности
            /// </summary>
            public string EntityCode { get; set; }

            /// <summary>
            /// Сообщение об ошибке
            /// </summary>
            public string Error { get; set; }

            /// <summary>
            /// Исключение об ошибке
            /// </summary>
            public Exception Exception { get; set; }
        }

        /// <summary>
        /// Запись незаполненных обязательных полей в файле лога
        /// </summary>
        protected class LogRecordEmptyFieldsProxy
        {
            /// <summary>
            /// Код сущности
            /// </summary>
            public string EntityCode { get; }

            /// <summary>
            /// Количество незаполненных полей
            /// </summary>
            public int ErrorRecordsCount { get; }

            /// <summary>
            /// Номера незаполненных полей
            /// </summary>
            public IList<IdEmptyFieldsProxy> ErrorRecords { get; }

            /// <summary>
            /// .ctor
            /// </summary>
            public LogRecordEmptyFieldsProxy(IExportableEntity entity)
            {
                this.EntityCode = entity.Code;
                this.ErrorRecords = entity.EmptyMandatoryFields.Select(x => new IdEmptyFieldsProxy(x)).ToList();
                this.ErrorRecordsCount = this.ErrorRecords.Count;
            }

            /// <summary>
            /// Идентификатор - Список номеров незаполненных полей
            /// </summary>
            public class IdEmptyFieldsProxy
            {
                /// <summary>
                /// Идентификатор сущности
                /// </summary>
                public long Id { get; }

                /// <summary>
                /// Список номеров незаполненных полей
                /// </summary>
                public IEnumerable<int> EmptyFields { get; }

                /// <summary>
                /// .ctor
                /// </summary>
                public IdEmptyFieldsProxy(KeyValuePair<long, IEnumerable<int>> keyValuePair)
                {
                    this.Id = keyValuePair.Key;
                    this.EmptyFields = keyValuePair.Value;
                }
            }
        }
    }
}