namespace Bars.Gkh.RegOperator.Enums
{
    using B4.Utils;

    /// <summary>
    /// Тип формирования получения займа
    /// </summary>
    public enum TypeLoanProcess
    {
        /// <summary>
        /// Автоматическая
        /// </summary>
        [Display("Автоматическая")]
        Auto = 0,

        /// <summary>
        /// Ручная
        /// </summary>
        [Display("Ручная")]
        Manual = 10
    }
}