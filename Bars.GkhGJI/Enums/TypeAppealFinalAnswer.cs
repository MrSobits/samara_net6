namespace Bars.GkhGji.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// тип ответа на обращение
    /// </summary>
    public enum TypeAppealFinalAnswer
    {
        /// <summary>
        /// Не задано
        /// </summary>
        [Display("Не задано")]
        NotSet = 2,

        /// <summary>
        /// Проект ответа
        /// </summary>
        [Display("Проект ответа")]
        Project = 0,

        /// <summary>
        /// Ответ заявителю
        /// </summary>
        [Display("Ответ заявителю")]
        Answer = 1,

    }
}