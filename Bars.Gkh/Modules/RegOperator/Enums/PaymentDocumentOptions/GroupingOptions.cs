namespace Bars.Gkh.RegOperator.Enums.PaymentDocumentOptions
{
    using Bars.B4.Utils;

    /// <summary>
    /// Настройка группировки
    /// </summary>
    public enum GroupingOption
    {
        /// <summary>
        /// По улицам
        /// </summary>
        [Display("По улицам")]
        ByStreet,

        /// <summary>
        /// По агентам доставки
        /// </summary>
        [Display("По агентам доставки")]
        ByDeliveryAgent
    }
}