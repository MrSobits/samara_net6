namespace Sobits.GisGkh.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Вид запроса
    /// </summary>
    public enum GisGkhFileType
    {
        /// <summary>
        /// Запрос
        /// </summary>
        [Display("Запрос")]
        request = 10,

        /// <summary>
        /// Подписанный запрос
        /// </summary>
        [Display("Подписанный запрос")]
        signedRequest = 20,

        /// <summary>
        /// Ответ о статусе запроса
        /// </summary>
        [Display("Ответ о статусе запроса")]
        requestStateAnswer = 30,

        /// <summary>
        /// Запрос на получение результата
        /// </summary>
        [Display("Запрос на получение результата")]
        getStateRequest = 40,

        /// <summary>
        /// Ответ с результатом
        /// </summary>
        [Display("Ответ с результатом")]
        answer = 50,

        /// <summary>
        /// Ошибка
        /// </summary>
        [Display("Ошибка")]
        error = 60

    }
}