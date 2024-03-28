namespace Bars.GisIntegration.Base.Service.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Cryptography.X509Certificates;

    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.B4.Modules.NH.Extentions;
    using Bars.B4.Utils;
    using Bars.GisIntegration.Base.Entities;
    using Bars.GisIntegration.Base.Enums;
    using Bars.GisIntegration.Base.Exporters;
    using Bars.GisIntegration.Base.GisServiceProvider;

    using Castle.Windsor;

    /// <summary>
    /// Сервис для работы с настройками интеграции ГИС
    /// </summary>
    public class GisSettingsService : IGisSettingsService
    {
        /// <summary>
        /// Домен-сервис "Настройка сервиса"
        /// </summary>
        public IDomainService<ServiceSettings> ServiceSettingsDomain { get; set; }

        public IDomainService<ContextSettings> ContextSettingsDomain { get; set; }

        /// <summary>
        /// Контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Получить список сохраненных настроек контекстов
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns>Результат выполнения операции</returns>
        public IDataResult GetStorableContextSettings(BaseParams baseParams)
        {
            var admissibleFileStorages = this.GetAdmissibleFileStorages();

            var result = this.ContextSettingsDomain.GetAll()
                .WhereContains(x => x.FileStorageName, admissibleFileStorages)
                .ToList();

            return new ListDataResult(result, result.Count);
        }

        /// <summary>
        /// Метод возвращает адрес сервиса из Единых настроек
        /// </summary>
        /// <param name="integrationService">Тип сервиса</param>
        /// <param name="isAsync">Асинхронный адрес или нет</param>
        /// <param name="defaultUrl">Адрес по умолчанию</param>
        /// <returns>Адрес сервиса</returns>
        public string GetServiceAddress(IntegrationService integrationService, bool isAsync, string defaultUrl = null)
        {
           var result = this.ServiceSettingsDomain.GetAll().FirstOrDefault(x => x.IntegrationService == integrationService);

            if (result.IsNotNull())
            {
                return isAsync ? result.AsyncServiceAddress : result.ServiceAddress;
            }

            return defaultUrl;
        }

        /// <summary>
        /// Получить контекст подсистемы ГИС РФ
        /// </summary>
        /// <param name="fileStorageName">Хранилище ГИС РФ</param>
        /// <returns>Контекст</returns>
        public string GetContext(FileStorageName fileStorageName)
        {
            var result = this.ContextSettingsDomain.GetAll().FirstOrDefault(x => x.FileStorageName == fileStorageName);

            if (result == null)
            {
                throw new Exception($"Не найдены настройки контекста подсистемы ГИС РФ {fileStorageName.GetDisplayName()}");
            }

            return result.Context;
        }

        /// <summary>
        /// Метод возвращает типы сервисов, которые не настроены
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Результат операции</returns>
        public IDataResult GetMissedSettings(BaseParams baseParams)
        {
            var settings = this.GetMissedIntegrationServices(this.GetRegisteredSettings());

            var result = settings.Select(
                x => new
                {
                    Value = (int)x,
                    Display = x.GetDisplayName()
                }).ToList();

            return new ListDataResult(result.ToList(), result.Count);
        }

        /// <summary>
        /// Метод возвращает хранилища данных ГИС, для которых не настроены контексты 
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Результат выполнения операции</returns>
        public IDataResult GetMissedContextSettings(BaseParams baseParams)
        {
            var admissibleFileStorages = this.GetAdmissibleFileStorages();

            var storableSettings = this.ContextSettingsDomain.GetAll().ToList();

            var result = admissibleFileStorages.Where(x => storableSettings.All(y => y.FileStorageName != x))
                .Select(z => new
                {
                    Value = (int)z,
                    Display = z.GetDisplayName()
                }).ToList();

            return new ListDataResult(result, result.Count);
        }

        /// <summary>
        /// Вернуть допустимые настройки по зарегестрированным в контейнере провайдерам
        /// </summary>
        /// <returns>Список настроек</returns>
        public IList<ServiceSettings> GetRegisteredSettings()
        {
            IList<IntegrationService> registeredServices = null;

            this.Container.UsingForResolvedAll<IGisServiceProvider>((c, services) => registeredServices = services.Select(x => x.IntegrationService).ToList());

            return this.ServiceSettingsDomain.GetAll()
                .WhereContains(x => x.IntegrationService, registeredServices)
                .ToList();
        }

        /// <summary>
        /// Получить допустимые хранилища ГИС
        /// </summary>
        /// <returns>Список хранилищ ГИС</returns>
        public List<FileStorageName> GetAdmissibleFileStorages()
        {
            var exporters = this.Container.ResolveAll<IDataExporter>();

            return exporters.Where(x => x.FileStorage.HasValue).Select(x => x.FileStorage.Value).Distinct().ToList();
        }

        /// <summary>
        /// Вернуть данные для Единых настроек
        /// </summary>
        /// <param name="createNew">Создавать недостающие настройки</param>
        /// <returns>Результат операции</returns>
        public IDataResult GetRegisteredSettings(bool createNew)
        {
            var registeredSettings = this.GetRegisteredSettings();

            if (!createNew)
            {
                return new ListDataResult(registeredSettings, registeredSettings.Count);
            }

            this.GetAllAndCreate(registeredSettings);
            return new ListDataResult(registeredSettings, registeredSettings.Count);
        }

        private void GetAllAndCreate(IList<ServiceSettings> registeredSettings)
        {
            var missedValues = this.GetMissedIntegrationServices(registeredSettings);

            this.Container.InTransaction(
                () =>
                {
                    foreach (var integrationService in missedValues)
                    {
                        var settings = new ServiceSettings
                        {
                            IntegrationService = integrationService
                        };
                        registeredSettings.Add(settings);

                        this.ServiceSettingsDomain.Save(settings);
                    }
                });
        }

        private IEnumerable<IntegrationService> GetMissedIntegrationServices(IList<ServiceSettings> registeredSettings)
        {
            IList<IntegrationService> registeredServices = null;
            this.Container.UsingForResolvedAll<IGisServiceProvider>((c, services) => registeredServices = services.Select(x => x.IntegrationService).ToList());

            var enumValues = ((IntegrationService[])Enum.GetValues(typeof(IntegrationService))).OfType<IntegrationService>().ToList();
            var missedValues = enumValues.Where(x => registeredSettings.All(y => y.IntegrationService != x));
            return missedValues.Where(x => registeredServices.Contains(x));
        }
    }
}