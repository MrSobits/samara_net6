namespace Bars.Gkh.RegOperator.Enums
{
    using B4.Utils;

    /// <summary>
    /// Тип оплаты
    /// </summary>
    public enum PaymentType
    {
        /// <summary>
        /// Основной
        /// </summary>
        [Display("Основной")]
        Basic = 0x1,

        /// <summary>
        /// Пени
        /// </summary>
        [Display("Пени")]
        Penalty = 0x2,

        /// <summary>
        /// Социальная поддержка
        /// </summary>
        [Display("Социальная поддержка")]
        SocialSupport = 0x3,

        /// <summary>
        /// По аренде
        /// </summary>
        [Display("По аренде")]
        Rent = 0x4,

        /// <summary>
        /// По предыдущей работе
        /// </summary>
        [Display("По предыдущей работе")]
        PreviousWork = 0x5,

        /// <summary>
        /// Ранее накопленные средства
        /// </summary>
        [Display("Ранее накопленные средства")]
        AccumulatedFund = 0x6
    }
}