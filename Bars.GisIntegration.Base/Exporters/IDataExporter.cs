namespace Bars.GisIntegration.Base.Exporters
{
    using System;
    using System.Collections.Generic;
    using Bars.B4.Utils;
    using Bars.GisIntegration.Base.Enums;

    /// <summary>
    /// Интерфейс экспортера данных
    /// </summary>
    public interface IDataExporter
    {
        /// <summary>
        /// Наименование экспортера 
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Порядок выполнения
        /// </summary>
        int Order { get; }

        /// <summary>
        /// Описание экспортера
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Необходимо подписывать данные
        /// </summary>
        bool NeedSign { get; }

        /// <summary>
        /// Метод может выполняться только от имени поставщика данных
        /// </summary>
        bool DataSupplierIsRequired { get; }

        /// <summary>
        /// Максимальное количество повторов триггера получения результатов экспорта
        /// </summary>
        int MaxRepeatCount { get; }

        /// <summary>
        /// Интервал запуска триггера получения результатов экспорта - в секундах
        /// </summary>
        int Interval { get; }

        /// <summary>
        /// Получить список методов, которые должны быть выполнены перед текущим
        /// </summary>
        /// <returns>Список методов, которые должны быть выполнены перед текущим</returns>
        List<string> GetDependencies();

        /// <summary>
        /// Тип задачи отправки данных
        /// </summary>
        Type SendDataTaskType { get; }

        /// <summary>
        /// Получить дополнительные параметры отправки данных
        /// Передаются в контекст задачи отправки данных
        /// </summary>
        /// <returns></returns>
        DynamicDictionary GetSendDataParams();

        /// <summary>
        /// Тип задачи подготовки данных
        /// </summary>
        Type PrepareDataTaskType { get; }

        /// <summary>
        /// Наименование хранилища данных ГИС для загрузки вложений
        /// </summary>
        FileStorageName? FileStorage { get; }
    }
}