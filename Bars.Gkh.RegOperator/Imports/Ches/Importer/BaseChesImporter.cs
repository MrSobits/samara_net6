namespace Bars.Gkh.RegOperator.Imports.Ches
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Text;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Import;
    using Bars.Gkh.RegOperator.Imports.Ches.PreImport;
    using Bars.Gkh.Repositories.ChargePeriod;

    using Castle.Windsor;

    /// <summary>
    /// Интерфейс для импорта данных из ЧЭС
    /// </summary>
    /// <typeparam name="T">Файл импорта</typeparam>
    public interface IChesImporter<T> where T : ImportFileInfo
    {
        /// <summary>
        /// Задать логгер
        /// </summary>
        void SetLogImport(ILogImport logImport);

        /// <summary>
        /// Задать сервис фильтрации
        /// </summary>
        void SetBillingFilterService(IBillingFilterService billingFilterService);

        /// <summary>
        /// Импортировать файл
        /// </summary>
        /// <param name="importFileInfo">Файл импорта</param>
        void Import(T importFileInfo);

        /// <summary>
        /// Проинициализировать справочники
        /// </summary>
        /// <param name="importFileInfo">Файл импорта</param>
        void InitDicts(T importFileInfo);

        /// <summary>
        /// Обработать данные импорта
        /// </summary>
        /// <param name="importFileInfo">Файл импорта</param>
        void ProcessData(T importFileInfo);

        Action<int, string> Indicate { get; set; }

        /// <summary>
        /// Задать параметры
        /// </summary>
        void SetBaseParams(BaseParams baseParams);
    }

    /// <summary>
    /// Базовый класс для импорта данных из ЧЭС
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class BaseChesImporter<T> : IChesImporter<T> where T : ImportFileInfo
    {
        /// <summary>
        /// Контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Логгер
        /// </summary>
        protected ILogImport LogImport { get; set; }

        /// <summary>
        /// Сервис фильтрации
        /// </summary>
        protected IBillingFilterService BillingFilterService { get; set; }

        /// <summary>
        /// Параметры
        /// </summary>
        protected BaseParams BaseParams { get; set; }

        /// <summary>
        /// Менеджер пользователей
        /// </summary>
        public IGkhUserManager UserManager { get; set; }

        /// <summary>
        /// Репозиторий для <see cref="ChargePeriod"/>
        /// </summary>
        public IChargePeriodRepository PeriodRepo { get; set; }

        public IDomainService<Entities.Import.Ches.ChesImport> ChesImportDomain { get; set; }

        public IChesTempDataProviderBuilder ProviderBuilder { get; set; }

        /// <summary>
        /// Название импорта
        /// </summary>
        public virtual string ImportName { get; set; }

        public Action<int, string> Indicate { get; set; }

        /// <summary>
        /// Логин текущего пользователя
        /// </summary>
        protected virtual string Login => this.UserManager.GetActiveUser()?.Login ?? "anonymous";

        /// <summary>
        /// Наименование родительского иморта
        /// </summary>
        protected string ParentImportName => ChesImport.ImportName;

        protected IChesTempDataProvider ChesTempDataProvider { get; private set; }

        /// <summary>
        /// Импортировать файл
        /// </summary>
        /// <param name="importFileInfo">Файл импорта</param>
        public virtual void Import(T importFileInfo)
        {
            this.ChesTempDataProvider = this.GetTemporaryImporter(importFileInfo);
            try
            {
                if (this.BillingFilterService.IsNotAllowAllRows)
                {
                    throw new Exception("Ошибка настройки импорта сведений от биллинга");
                }

                if (!importFileInfo.Validate())
                {
                    return;
                }

                using (var stream = this.GetStream(importFileInfo))
                {
                    using (var reader = new StreamReader(stream, Encoding.GetEncoding(1251)))
                    {
                        // пропускаем заголовок
                        reader.ReadLine();

                        importFileInfo.FillRows(reader);
                    }
                }

                this.InitDicts(importFileInfo);
                this.ProcessData(importFileInfo);

                var periodFileInfo = importFileInfo as IPeriodImportFileInfo;
                if (periodFileInfo.IsNotNull())
                {
                    var chesImport = this.ChesImportDomain.GetAll().FirstOrDefault(x => x.Period == periodFileInfo.Period);
                    if (chesImport.IsNotNull())
                    {
                        chesImport.ImportedFiles.Add(periodFileInfo.FileType);
                        this.ChesImportDomain.Save(chesImport);
                    }
                }
            }
            catch (Exception e)
            {
                this.LogImport.Error("Ошибка", $"Произошла ошибка при импорте: \"{e.Message}\"");
                throw;
            }
        }

        /// <summary>
        /// Метод получения потока
        /// </summary>
        /// <param name="importFileInfo">Информация о файле</param>
        /// <param name="stream">Переменная потока, для записи</param>
        /// <returns>Возвращает true, если необходимо прочесть заголовочную строку</returns>
        private Stream GetStream(T importFileInfo)
        {
            return importFileInfo.FileData.IsNotNull()
                ? new MemoryStream(importFileInfo.FileData.Data)
                : this.ChesTempDataProvider.GetOutputStream();
        }

        /// <summary>
        /// Проинициализировать справочники
        /// </summary>
        /// <param name="importFileInfo">Файл импорта</param>
        public virtual void InitDicts(T importFileInfo)
        {
        }

        /// <summary>
        /// Обработать данные импорта
        /// </summary>
        /// <param name="saldoChangeFileInfo">Файл импорта</param>
        public abstract void ProcessData(T saldoChangeFileInfo);

        /// <summary>
        /// Задать логгер
        /// </summary>
        public void SetLogImport(ILogImport logImport)
        {
            this.LogImport = logImport;
        }

        /// <summary>
        /// Задать сервис фильтрации
        /// </summary>
        public void SetBillingFilterService(IBillingFilterService billingFilterService)
        {
            this.BillingFilterService = billingFilterService;
        }

        protected IChesTempDataProvider GetTemporaryImporter(T fileInfo)
        {
            return this.ProviderBuilder.SetParams(this.BaseParams).Build(fileInfo as IPeriodImportFileInfo);
        }

        public void SetBaseParams(BaseParams baseParams)
        {
            this.BaseParams = baseParams;
        }
    }
}