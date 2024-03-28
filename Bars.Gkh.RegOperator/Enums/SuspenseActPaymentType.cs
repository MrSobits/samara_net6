namespace Bars.Gkh.RegOperator.Enums
{
    using B4.Utils;

    /// <summary>
    /// Вид оплаты акта
    /// </summary>
    public enum SuspenseActPaymentType
    {
        [Display("Аванс")]
        PrePayment = 0x2,

        [Display("Оплата акта")]
        ActPayment = 0x4
    }
}