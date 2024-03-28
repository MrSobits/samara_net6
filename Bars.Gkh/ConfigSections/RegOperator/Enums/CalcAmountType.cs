namespace Bars.Gkh.ConfigSections.RegOperator.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Тип расчета суммы распределения
    /// </summary>
    public enum CalcAmountType
    {
        /// <summary>
        /// Начислено + перерасчет
        /// </summary>
        [Display("Начислено + перерасчет")]
        ChargeAndRecalc = 0,

        /// <summary>
        /// Исходящее сальдо
        /// </summary>
        [Display("Исходящее сальдо")]
        SaldoOut = 1
    }
}