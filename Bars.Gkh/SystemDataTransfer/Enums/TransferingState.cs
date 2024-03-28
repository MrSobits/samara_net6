namespace Bars.Gkh.SystemDataTransfer.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Состояние интеграции
    /// </summary>
    public enum TransferingState
    {
        /// <summary>
        /// Отправка запроса на экспорт
        /// </summary>
        [Display("Отправка запроса на экспорт")]
        SendExportTask = 0,

        /// <summary>
        /// В очереди на экспорт
        /// </summary>
        [Display("В очереди на экспорт")]
        ExportQueued = 10,

        /// <summary>
        /// Формирование экспортируемых данных
        /// </summary>
        [Display("Формирование экспортируемых данных")]
        GenerateExportData = 20,

        /// <summary>
        /// Отправка файла
        /// </summary>
        [Display("Отправка файла")]
        SendFile = 30,

        /// <summary>
        /// В очереди на импорт
        /// </summary>
        [Display("В очереди на импорт")]
        ImportQueued = 40,

        /// <summary>
        /// Импорт файла
        /// </summary>
        [Display("Импорт файла")]
        ImportFile = 50,

        /// <summary>
        /// Интеграция завершена
        /// </summary>
        [Display("Интеграция завершена")]
        IntegrationComplete = 60
    }
}