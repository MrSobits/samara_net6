namespace Bars.Gkh.RegOperator.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Тип оплаты
    /// </summary>
    public enum ImportPaymentType
    {
        /// <summary>
        /// Оплата
        /// </summary>
        [Display("Оплата")]
        Payment = 1,

        /// <summary>
        /// Отмена оплаты
        /// </summary>
        [Display("Отмена оплаты")]
        CancelPayment = 2,

        /// <summary>
        /// Возврат оплаты
        /// </summary>
        [Display("Возврат оплаты")]
        ReturnPayment = 3
    }
}