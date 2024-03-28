namespace Sobits.GisGkh.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Статус проверки ГИС ЖКХ
    /// </summary>
    public enum GisGkhExaminationState
    {
        /// <summary>
        /// Не отправлена
        /// </summary>
        [Display("Не отправлена")]
        notSent = 10,

        ///// <summary>
        ///// Отправлена со всеми документами
        ///// </summary>
        //[Display("Отправлена со всеми документами")]
        //fullSent = 20,

        /// <summary>
        /// Отправлена без результата
        /// </summary>
        [Display("Отправлена без результата")]
        sentWithoutResult = 30,

        /// <summary>
        /// Отправлена с результатом, но без предписания / протокола
        /// </summary>
        [Display("Отправлена с результатом, но без предписания / протокола")]
        sentWithoutDocs = 40,

        ///// <summary>
        ///// В процессе отправки
        ///// </summary>
        //[Display("В процессе отправки")]
        //inProcess = 50
    }
}