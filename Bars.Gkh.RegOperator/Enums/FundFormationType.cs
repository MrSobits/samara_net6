namespace Bars.Gkh.RegOperator.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Способ формирования фонда
    /// </summary>
    public enum FundFormationType
    {
        /// <summary>
        /// Не задано
        /// </summary>
        [Display("-")]
        NotSet = 0,

        /// <summary>
        /// Счет регионального оператора
        /// </summary>
        [Display("Счет регионального оператора")]
        Regop = 10,

        /// <summary>
        /// Специальный счет регионального оператора
        /// </summary>
        [Display("Специальный счет регионального оператора")]
        SpecRegop = 20,

        /// <summary>
        /// Специальный счет
        /// </summary>
        [Display("Специальный счет")]
        Special = 30
    }
}