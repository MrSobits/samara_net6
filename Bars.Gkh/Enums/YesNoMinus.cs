namespace Bars.Gkh.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Да/Нет/-
    /// </summary>
    public enum YesNoMinus
    {
        /// <summary>
        /// Не выбран
        /// </summary>
        [Display("-")]
        Minus = 0,

        /// <summary>
        /// Да
        /// </summary>
        [Display("Да")]
        Yes = 10,

        /// <summary>
        /// Нет
        /// </summary>
        [Display("Нет")]
        No = 20
    }
}