namespace Bars.GkhCr.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Тип счета
    /// </summary>
    public enum ReportAccountType
    {
        /// <summary>
        /// Счет рег. оператора
        /// </summary>
        [Display("Счет рег. оператора")]
        RegOperator = 0,

        /// <summary>
        /// Спец счет
        /// </summary>
        [Display("Спец счет")]
        Special = 1
    }
}
