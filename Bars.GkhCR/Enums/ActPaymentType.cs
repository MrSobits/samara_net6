namespace Bars.GkhCr.Enums
{
    using B4.Utils;

    /// <summary>
    /// Вид оплаты
    /// </summary>
    public enum ActPaymentType
    {
        [Display("Аванс")]
        PrePayment = 10,

        [Display("Оплата акта")]
        Payment = 20
    }
}
