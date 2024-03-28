namespace Bars.Gkh.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Состояние конструктивного элемента
    /// </summary>
    public enum ConditionStructElement
    {
        /// <summary>
        /// Не определялось
        /// </summary>
        [Display("Не определялось")]
        NotDetermined = 10,

        /// <summary>
        /// Удовлетворительное
        /// </summary>
        [Display("Удовлетворительное")]
        Satisfactory = 20,

        /// <summary>
        /// Неудовлетворительное
        /// </summary>
        [Display("Неудовлетворительное")]
        Unsatisfactory = 30,

        /// <summary>
        /// Снесен
        /// </summary>
        [Display("Аварийное")]
        Emergency = 40,

        /// <summary>
        /// Снесен
        /// </summary>
        [Display("Нормативное")]
        Normative = 50,

        /// <summary>
        /// Снесен
        /// </summary>
        [Display("Ограниченно-работоспособное")]
        LimitedUsable = 60,

        /// <summary>
        /// Снесен
        /// </summary>
        [Display("Работоспособное")]
        Workable = 70
    }
}