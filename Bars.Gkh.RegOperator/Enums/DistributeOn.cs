namespace Bars.Gkh.RegOperator.Enums
{
    using B4.Utils;

    /// <summary>
    /// Распределить по
    /// </summary>
    public enum DistributeOn
    {
        /// <summary>
        /// Начислениям и пени
        /// </summary>
        [Display("Начислениям и пени")]
        ChargesPenalties = 10,

        /// <summary>
        /// Пени
        /// </summary>
        [Display("Пени")]
        Penalties = 20,

        /// <summary>
        /// Начислениям
        /// </summary>
        [Display("Начислениям")]
        Charges = 30
    }
}