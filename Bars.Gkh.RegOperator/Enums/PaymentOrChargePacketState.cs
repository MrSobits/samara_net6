namespace Bars.Gkh.RegOperator.Enums
{
    using B4.Utils;

    /// <summary>
    /// Состояние пакета неподтвержденных оплат/начислений
    /// </summary>
    public enum PaymentOrChargePacketState
    {
        /// <summary>
        /// Подтверждено
        /// </summary>
        [Display("Подтверждено")]
        Accepted = 10,

        /// <summary>
        /// Ожидание
        /// </summary>
        [Display("Не подтверждено")]
        Pending = 20,

        /// <summary>
        /// Обрабатывается
        /// </summary>
        [Display("Ожидание подтверждения")]
        InProgress = 30
    }
}