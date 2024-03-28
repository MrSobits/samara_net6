namespace Bars.GisIntegration.Base.Enums
{
    /// <summary>
    /// Тип ошибки обработки пакета
    /// </summary>
    public enum PackageProcessingErrorType
    {
        /// <summary>
        /// Ошибка отправки пакета
        /// </summary>
        Sending = 10,

        /// <summary>
        /// Ошибка получения состояния
        /// </summary>
        GettingState = 20,

        /// <summary>
        /// Ошибка обработки результата
        /// </summary>
        ProcessingResult = 30,

        /// <summary>
        /// Ошибка превышения таймаута
        /// Превышено максимальное количество попыток получения результата асинхронного запроса
        /// </summary>
        Timeout = 40
    }
}
