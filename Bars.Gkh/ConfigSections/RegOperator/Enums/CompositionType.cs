namespace Bars.Gkh.ConfigSections.RegOperator.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Компоновка документов на оплату
    /// </summary>
    public enum CompositionType
    {
        /// <summary>
        /// По лицевым счетам
        /// </summary>
        [Display("По лицевым счетам")]
        Account,

        /// <summary>
        /// По агентам доставки
        /// </summary>
        [Display("По агентам доставки")]
        DeliveryAgent
    }
}