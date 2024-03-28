namespace Bars.Gkh.Import.Impl
{
    using System;
    using System.Linq;
    using System.Threading;
    using B4;
    using B4.Modules.Tasks.Common.Service;

    using Bars.B4.Modules.Tasks.Common.Entities;

    using Enums.Import;
    using Castle.Windsor;
    
    using Fasterflect;

    using ExecutionContext = B4.Modules.Tasks.Common.Contracts.ExecutionContext;

    public abstract class GkhImportBase : IGkhImport
    {
        protected ILogImport LogImport;
        public ILogImportManager LogImportManager { get; set; }
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Индикатор события
        /// </summary>
        private IProgressIndicator Indicator { get; set; }

        /// <summary>
        /// Токен отмены
        /// </summary>
        private CancellationToken CancellationToken { get; set; }

        /// <summary>
        /// Задача сервера вычислений
        /// </summary>
        public IDomainService<TaskEntry> TaskEntryDomain { get; set; }

        #region Implementation of IGkhImport

        /// <summary>
        /// Ключ импорта
        /// </summary>
        public abstract string Key { get; }

        /// <summary>
        /// Код группировки импорта (например группировка в меню)
        /// </summary>
        public abstract string CodeImport { get; }

        /// <summary>
        /// Наименование импорта
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// Разрешенные расширения файлов
        /// </summary>
        public abstract string PossibleFileExtensions { get; }

        /// <summary>
        /// Права
        /// </summary>
        public abstract string PermissionName { get; }

        /// <summary>
        /// Зависимости от других импортов
        /// </summary>
        public virtual string[] Dependencies { get;}

        /// <summary>
        /// Идентификатор задачи
        /// <para>0, если идентификтор не удалось получить</para>
        /// </summary>
        public long TaskId { get; private set; }

        /// <summary>
        /// Импорт данных
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        public virtual ImportResult Import(BaseParams baseParams)
        {
            var fileCount = baseParams.Files.Count;
            if (fileCount == 0 || baseParams.Files.First().Value == null)
            {
                return new ImportResult(StatusImport.CompletedWithError){Message = "Нет файла для импорта"};
            }

            var file = baseParams.Files.First().Value;
            this.InitLog(file.FileName);

            ImportResult result;
            try
            {
                result = this.ImportUsingGkhApi(baseParams);
            }
            catch (Exception ex)
            {
                this.LogImport.Error("Ошибка импорта данных", ex.ToString());
                result = new ImportResult(StatusImport.CompletedWithError);
            }

            this.LogImportManager.Add(file, this.LogImport);
            this.LogImportManager.Task = this.TaskEntryDomain.Load(this.TaskId);
            this.LogImportManager.Save();

            return result;
        }

        /// <summary>
        /// Первоночальная валидация файла перед импортом
        /// </summary>
        /// <param name="baseParams"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public virtual bool Validate(BaseParams baseParams, out string message)
        {
            message = string.Empty;

            if (!baseParams.Params.GetAs<bool>("not_from_file"))
            {
                var fileCount = baseParams.Files.Count;
                if (fileCount == 0 || baseParams.Files.First().Value == null)
                {
                    message = "Не выбран файл для импорта";

                    return false;
                }
            }

            return true;
        }

        #endregion

        #region Implementation of ITaskExecutor

        public IDataResult Execute(BaseParams @params, ExecutionContext ctx, IProgressIndicator indicator, CancellationToken ct)
        {
            this.Indicator = indicator;
            this.CancellationToken = ct;
            this.TaskId = this.GetTaskId();

            return this.Import(@params, ctx, indicator, ct);
        }

        public string ExecutorCode { get { return this.Key; } }

        #endregion

        /// <summary>
        /// Загрузить импорт
        /// </summary>
        protected virtual ImportResult Import(BaseParams @params,
            ExecutionContext ctx,
            IProgressIndicator indicator,
            CancellationToken ct)
        {
            return this.Import(@params);
        }

        /// <summary>
        /// Загрузить импорт
        /// </summary>
        protected virtual ImportResult ImportUsingGkhApi(BaseParams @params)
        {
            return new ImportResult();
        }

        #region Utils

        /// <summary>
        /// Инициализация лога
        /// </summary>
        public void InitLog(string fileName)
        {
            this.LogImportManager.FileNameWithoutExtention = fileName;
            this.LogImportManager.UploadDate = DateTime.Now;

            this.LogImport = this.Container.ResolveAll<ILogImport>().FirstOrDefault(x => x.Key == MainLogImportInfo.Key);
            if (this.LogImport == null)
            {
                throw new Exception("Не найдена реализация интерфейса ILogImport");
            }

            this.LogImport.SetFileName(fileName);
            this.LogImport.ImportKey = this.Key;
        }

        /// <summary>
        /// Сообщить об изменении прогресса задачи
        /// </summary>
        /// <param name="percent"></param>
        /// <param name="message"></param>
        protected void Indicate(int percent, string message)
        {
            this.Indicator?.Report(null, (uint)percent, message);
        }

        protected bool IsCancelled()
        {
            return this.CancellationToken.IsCancellationRequested;
        }
        #endregion

        private long GetTaskId()
        {
            long res = 0;
            try
            {
                var entry = this.Indicator?.GetFieldValue("_entry", Flags.NonPublic | Flags.Instance) as TaskEntry;
                if (entry != null)
                {
                    res = entry.Id;
                }
            }
            catch
            {
                res = 0;
            }
            return res;
        }
    }
}