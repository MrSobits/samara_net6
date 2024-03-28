namespace Bars.Gkh.RegOperator.Enums
{
    using B4.Utils;

    /// <summary>
    /// Тип расчетного счета
    /// </summary>
    public enum TypeCalcAccount
    {
        /// <summary>
        /// Не задано
        /// </summary>
        [Display("Не задано")]
        NotSet = 0,

        /// <summary>
        /// Счет рег. оператора
        /// </summary>
        [Display("Расчетный счет рег. оператора")]
        Regoperator = 10,

        /// <summary>
        /// Спец. счет
        /// </summary>
        [Display("Специальный счет")]
        Special = 20
    }
}