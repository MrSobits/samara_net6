namespace Bars.Gkh.RegOperator.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Тип платежа для Счета невыясненных сумм
    /// </summary>
    public enum SuspenseAccountTypePayment
    {
        [Display("Платеж")]
        Payment = 10,

        [Display("Оплата взноса")]
        ChargePayment = 20
    }
}