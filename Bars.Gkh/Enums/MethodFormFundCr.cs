namespace Bars.Gkh.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Способ формирования фонда КР
    /// </summary>
    public enum MethodFormFundCr
    {
        /// <summary>
        /// На счете регионального оператора
        /// </summary>
        [Display("На счете регионального оператора")]
        RegOperAccount = 10,

        /// <summary>
        /// На специальном счете
        /// </summary>
        [Display("На специальном счете")]
        SpecialAccount = 20,

        /// <summary>
        /// Не задано
        /// </summary>
        [Display("Не задано")]
        NotSet = 0
    }
}
