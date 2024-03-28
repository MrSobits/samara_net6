namespace Bars.Gkh.RegOperator.Enums
{
    using System;

    using Bars.B4.Utils;

    /// <summary>
    /// Тип запрета перерасчета
    /// </summary>
    [Flags]
    public enum BanRecalcType
    {
        /// <summary>
        /// Запрет перерасчета по начислениям
        /// </summary>
        [Display("Запрет перерасчета по начислениям")]
        Charge = 1,

        /// <summary>
        /// Запрет перерасчета по пени
        /// </summary>
        [Display("Запрет перерасчета по пени")]
        Penalty = 2,

        /// <summary>
        /// Запрет перерасчета по начислениям и пени
        /// </summary>
        [Display("Запрет перерасчета по начислениям и пени")]
        ChargeAndPenalty = Charge | Penalty
    }
}