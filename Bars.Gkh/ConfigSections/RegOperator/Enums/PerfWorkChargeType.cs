namespace Bars.Gkh.ConfigSections.RegOperator.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Тип зачета средств за ранее выполненные работы
    /// </summary>
    public enum PerfWorkChargeType
    {
        /// <summary>
        /// Для оплаты задолженности за предыдущие периоды
        /// </summary>
        [Display("Для оплаты задолженности за предыдущие периоды")]
        ForExistingCharges = 1,

        /// <summary>
        /// Для оплаты новых начислений
        /// </summary>
        [Display("Для оплаты новых начислений")]
        ForNewCharges = 2
    }
}