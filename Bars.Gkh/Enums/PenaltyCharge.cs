namespace Bars.Gkh.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Начисление пени
    /// </summary>
    public enum PenaltyCharge
    {
        /// <summary>
        /// По нарастающему итогу
        /// </summary>
        [Display("По нарастающему итогу")]
        CumulativeTotal = 10,

        /// <summary>
        /// По месячным начислениям
        /// </summary>
        [Display("По месячным начислениям")]
        Monthly = 20
    }
}