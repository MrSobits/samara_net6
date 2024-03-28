namespace Bars.GkhGji.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// тип ответа на обращение
    /// </summary>
    public enum TypeAppealAnswer
    {
        /// <summary>
        /// Не задано
        /// </summary>
        [Display("Не задано")]
        NotSet = 0,

        /// <summary>
        /// Выездная
        /// </summary>
        [Display("Служебная записка")]
        Note = 10,

        /// <summary>
        /// Документарная
        /// </summary>
        [Display("Ответ")]
        Answer = 20,

        /// <summary>
        /// Уведомление о продлении
        /// </summary>
        [Display("Уведомление о продлении")]
        Notice = 30,


        /// <summary>
        /// Дополнительный ответ
        /// </summary>
        [Display("Дополнительный ответ")]
        AdditAnswer = 40,

        /// <summary>
        /// Визуальное обследование
        /// </summary>
        [Display("Письмо")]
        Letter = 50

    }
}