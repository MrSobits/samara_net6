namespace Bars.Gkh.RegOperator.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Статус оплаты платежного документа
    /// </summary>
    public enum PaymentDocumentPaymentState
    {
        /// <summary>
        /// Не оплачено
        /// </summary>
        [Display("Не оплачено")]
        NotPaid = 0,

        /// <summary>
        /// Оплачено
        /// </summary>
        [Display("Оплачено")]
        Paid = 1,

        /// <summary>
        /// Частично оплачено
        /// </summary>
        [Display("Частично оплачено")]
        PartiallyPaid = 2
    }
}