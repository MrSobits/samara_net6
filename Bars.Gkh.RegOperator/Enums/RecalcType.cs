namespace Bars.Gkh.RegOperator.Enums
{
    using B4.Utils;

    /// <summary>
    /// Тип перерасчета
    /// </summary>
    public enum RecalcType
    {
        /// <summary>
        /// Перерачет начисления по базовому тарифу
        /// </summary>
        [Display("Перерачет начисления по базовому тарифу")]
        BaseTariffCharge = 10,

        /// <summary>
        /// Перерачет начисления по тарифу решения
        /// </summary>
        [Display("Перерачет начисления по тарифу решения")]
        DecisionTariffCharge = 20,

        /// <summary>
        /// Перерачет пени
        /// </summary>
        [Display("Перерачет пени")]
        Penalty = 30
    }
}
