namespace Bars.GisIntegration.Base.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Статус пакета
    /// </summary>
    public enum PackageState
    {
        /// <summary>
        /// Новый
        /// </summary>
        [Display("Новый")]
        New = 10,

        /// <summary>
        /// Подписан
        /// </summary>
        [Display("Подписан")]
        Signed = 20,

        /// <summary>
        /// Отправлен
        /// </summary>
        [Display("Отправлен")]
        Sent = 30,

        /// <summary>
        /// Ошибка отправки пакета
        /// </summary>
        [Display("Ошибка отправки пакета")]
        SendingError = 40,

        /// <summary>
        /// Успешно обработан
        /// </summary>
        [Display("Успешно обработан")]
        SuccessProcessed = 50,

        /// <summary>
        /// Обработан с ошибками
        /// </summary>
        [Display("Обработан с ошибками")]
        ProcessedWithErrors = 55,

        /// <summary>
        /// Ошибка получения состояния
        /// </summary>
        [Display("Ошибка получения состояния")]
        GettingStateError = 60,

        /// <summary>
        /// Ошибка обработки результата
        /// </summary>
        [Display("Ошибка обработки результата")]
        ProcessingResultError = 70,

        /// <summary>
        /// Ошибка превышения таймаута
        /// Превышено максимальное количество попыток получения результата асинхронного запроса
        /// </summary>
        [Display("Ошибка превышения таймаута")]
        TimeoutError = 80
    }
}
