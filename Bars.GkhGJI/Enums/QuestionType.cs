namespace Bars.GkhGji.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Типы вопросов
    /// </summary>
    public enum QuestionType
    {
        /// <summary>
        /// Не обращение
        /// </summary>
        [Display("Не обращение")]
        NotTreatment = 1,

        /// <summary>
        /// Жалоба
        /// </summary>
        [Display("Жалоба")]
        Complaint = 2,

        /// <summary>
        /// Заявление
        /// </summary>
        [Display("Заявление")]
        Statement = 3,

        /// <summary>
        /// Предложение
        /// </summary>
        [Display("Предложение")]
        Sentence = 4
    }
}
