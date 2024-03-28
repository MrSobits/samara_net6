namespace Bars.GkhRf.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Тип оплаты подрядчику
    /// </summary>
    public enum TypePaymentRfCtr
    {
        /// <summary>
        /// Аванс
        /// </summary>
        [Display("Аванс")]
        Prepayment = 10,

        /// <summary>
        /// Оплата за капремонт
        /// </summary>
        [Display("Оплата за капремонт")]
        CrPayment = 20,
    }
}
