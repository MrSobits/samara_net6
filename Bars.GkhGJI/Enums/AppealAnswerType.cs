namespace Bars.GkhGji.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Тип ответа на обращение
    /// </summary>
    public enum AppealAnswerType
    {
        /// <summary>
        /// Дан ответ
        /// </summary>
        [Display("Дан ответ")]
        Answer = 10,

        /// <summary>
        /// Перенаправлено
        /// </summary>
        [Display("Перенаправлено")]
        Redirect = 20,

        /// <summary>
        /// Перенаправлено
        /// </summary>
        [Display("Перенаправлено и дан ответ")]
        RedirectAndAnswer = 25,

        /// <summary>
        /// Не требует ответа
        /// </summary>
        [Display("Не требует ответа")]
        NotNeedAnswer = 30,

        /// <summary>
        /// Рассмотрение продлено
        /// </summary>
        [Display("Рассмотрение продлено")]
        RollOver = 40,

        /// <summary>
        /// Не определено
        /// </summary>
        [Display("Не определено")]
        NotSet = 50
    }
}