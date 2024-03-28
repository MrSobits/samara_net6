namespace Sobits.GisGkh.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Статус запроса
    /// </summary>
    public enum GisGkhRequestState
    {
        /// <summary>
        /// Пакет не сформирован
        /// </summary>
        [Display("Пакет не сформирован")]
        NotFormed = 0,

        /// <summary>
        /// Пакет сформирован
        /// </summary>
        [Display("Пакет сформирован")]
        Formed = 10,

        /// <summary>
        /// Пакет подписан
        /// </summary>
        [Display("Пакет сформирован и подписан")]
        Signed = 20,

        /// <summary>
        /// Запрос поставлен в очередь запросов ГИС ЖКХ
        /// </summary>
        [Display("Запрос поставлен в очередь запросов ГИС ЖКХ")]
        Queued = 30,

        /// <summary>
        /// Ответ получен
        /// </summary>
        [Display("Ответ от ГИС ЖКХ получен")]
        ResponseReceived = 40,

        /// <summary>
        /// Ошибка
        /// </summary>
        [Display("Ошибка")]
        Error = 50,

        /// <summary>
        /// Ответ получен
        /// </summary>
        [Display("Ответ обработан")]
        ResponseProcessed = 60

    }
}