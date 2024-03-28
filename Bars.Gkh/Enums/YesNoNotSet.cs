namespace Bars.Gkh.Enums
{
    using System;

    using Bars.B4.Utils;

    /// <summary>
    /// Да/Нет/Не задано
    /// </summary>
    [Flags]
    public enum YesNoNotSet
    {
        /// <summary>
        /// Да
        /// </summary>
        [Display("Да")]
        Yes = 10,

        /// <summary>
        /// Нет
        /// </summary>
        [Display("Нет")]
        No = 20,

        /// <summary>
        /// Не задано
        /// </summary>
        [Display("Не задано")]
        NotSet = 30
    }
}
