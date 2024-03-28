namespace Bars.Gkh.RegOperator.Enums
{
    using B4.Utils;

    /// <summary>
    /// Тип импортируемой оплаты
    /// </summary>
    public enum ImportedPaymentType
    {
        /// <summary>
        /// Основной
        /// </summary>
        [Display("Основной")]
        Basic = 10,

        /// <summary>
        /// Пени
        /// </summary>
        [Display("Пени")]
        Penalty = 20,

        /// <summary>
        /// Платеж
        /// </summary>
        [Display("Платеж")]
        Payment = 30,

        /// <summary>
        /// Оплата взноса
        /// </summary>
        [Display("Оплата взноса")]
        ChargePayment = 40,

        /// <summary>
        /// Сумма
        /// </summary>
        [Display("Сумма")]
        Sum = 50,

        /// <summary>
        /// Социальная поддержка
        /// </summary>
        [Display("Социальная поддержка")]
        SocialSupport = 60,

        /// <summary>
        /// Возврат средств
        /// </summary>
        [Display("Возврат средств")]
        Refund = 70,

        /// <summary>
        /// Возврат пени
        /// </summary>
        [Display("Возврат пени")]
        PenaltyRefund = 80
    }
}