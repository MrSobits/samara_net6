namespace Bars.Gkh.RegOperator.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Тип пакета
    /// </summary>
    public enum UnacceptedPaymentPacketType
    {
        /// <summary>
        /// Оплата
        /// </summary>
        [Display("Оплата")]
        Payment = 10,

        /// <summary>
        /// Соцподдержка
        /// </summary>
        [Display("Соцподдержка")]
        SocialSupport = 20,
    }
}