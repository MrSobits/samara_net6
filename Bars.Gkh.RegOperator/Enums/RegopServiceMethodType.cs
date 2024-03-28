namespace Bars.Gkh.RegOperator.Enums
{
    using B4.Utils;

    /// <summary>
    /// Методы сервиса регоператора
    /// </summary>
    public enum RegopServiceMethodType
    {
        /// <summary>
        /// Загрузка лицевых счетов по РКЦ
        /// </summary>
        [Display("Загрузка лицевых счетов по РКЦ")]
        ImportPersAccRkc = 10,

        /// <summary>
        /// Загрузка лицевых счетов по РКЦ
        /// </summary>
        [Display("Загрузка оплат и начислений по РКЦ")]
        ImportChargePayment = 20,

        /// <summary>
        /// Загрузка оплат по платежному агенту
        /// </summary>
        [Display("Загрузка оплат по платежному агенту")]
        ImportChargePaymentAgent = 30
    }
}
