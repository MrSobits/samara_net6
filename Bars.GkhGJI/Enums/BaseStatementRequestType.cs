namespace Bars.GkhGji.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Тип запроса обращения
    /// </summary>
    public enum BaseStatementRequestType
    {
        /// <summary>
        /// Обращение граждан
        /// </summary>
        [Display("Обращение граждан")]
        AppealCits = 10,

        /// <summary>
        /// Мотивировочное заключение
        /// </summary>
        [Display("Мотивировочное заключение")]
        MotivationConclusion = 20
    }
}