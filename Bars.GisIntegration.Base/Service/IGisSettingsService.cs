namespace Bars.GisIntegration.Base.Service
{
    using System.Collections.Generic;

    using Bars.B4;
    using Bars.GisIntegration.Base.Entities;
    using Bars.GisIntegration.Base.Enums;

    /// <summary>
    /// Сервис для работы с настройками интеграции ГИС
    /// </summary>
    public interface IGisSettingsService
    {
        /// <summary>
        /// Вернуть допустимые настройки по зарегестрированным в контейнере провайдерам
        /// </summary>
        /// <returns>Список настроек</returns>
        IList<ServiceSettings> GetRegisteredSettings();

        /// <summary>
        /// Вернуть данные для Единых настроек
        /// </summary>
        /// <param name="createNew">Создавать недостающие настройки</param>
        /// <returns>Результат операции</returns>
        IDataResult GetRegisteredSettings(bool createNew);

        /// <summary>
        /// Получить список сохраненных настроек контекстов
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns>Результат выполнения операции</returns>
        IDataResult GetStorableContextSettings(BaseParams baseParams);

        /// <summary>
        /// Метод возвращает адрес сервиса из Единых настроек
        /// </summary>
        /// <param name="integrationService">Тип сервиса</param>
        /// <param name="isAsync">Асинхронный адрес или нет</param>
        /// <param name="defaultUrl">Адрес по умолчанию</param>
        /// <returns>Адрес сервиса</returns>
        string GetServiceAddress(IntegrationService integrationService, bool isAsync, string defaultUrl = null);

        /// <summary>
        /// Метод возвращает типы сервисов, которые не настроены
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Результат операции</returns>
        IDataResult GetMissedSettings(BaseParams baseParams);

        /// <summary>
        /// Метод возвращает хранилища данных ГИС, для которых не настроены контексты 
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Результат выполнения операции</returns>
        IDataResult GetMissedContextSettings(BaseParams baseParams);

        /// <summary>
        /// Получить контекст подсистемы ГИС РФ
        /// </summary>
        /// <param name="fileStorageName">Хранилище ГИС РФ</param>
        /// <returns>Контекст</returns>
        string GetContext(FileStorageName fileStorageName);
    }
}