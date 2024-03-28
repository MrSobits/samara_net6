namespace Bars.Gkh.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Статус запроса
    /// </summary>
    public enum SMEVRequestState
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
        /// Запрос поставлен в очередь запросов СМЭВ
        /// </summary>
        [Display("Запрос поставлен в очередь запросов СМЭВ")]
        Queued = 30,

        /// <summary>
        /// Ответ получен
        /// </summary>
        [Display("Ответ от СМЭВ получен")]
        ResponseReceived = 40,

        /// <summary>
        /// Ошибка
        /// </summary>
        [Display("Ошибка")]
        Error = 50,

        /// <summary>
        /// Начисление принято
        /// </summary>
        [Display("Начисление принято")]
        CalculationReceived = 60,

        /// <summary>
        /// Сквитировано
        /// </summary>
        [Display("Сквитировано")]
        Reconcile = 70

    }
}