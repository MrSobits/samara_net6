namespace Bars.Gkh.RegOperator.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Тип возврата средств
    /// </summary>
    public enum RefundType
    {
        /// <summary>
        /// Возврат взносов на КР
        /// </summary>
        [Display("Возврат взносов на КР")]
        CrPayments = 10,

        /// <summary>
        /// Возврат МСП
        /// </summary>
        [Display("Возврат МСП")]
        Msp = 20,

        /// <summary>
        /// Возврат пени
        /// </summary>
        [Display("Возврат пени")]
        Penalty = 30
    }
}