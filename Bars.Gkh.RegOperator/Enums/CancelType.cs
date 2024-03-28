namespace Bars.Gkh.RegOperator.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Тип отмены начислений
    /// </summary>
    public enum CancelType
    {
        /// <summary>
        /// Отмена начисления по базовому тарифу
        /// </summary>
        [Display("Отмена начисления по базовому тарифу")]
        BaseTariffCharge = 10,

        /// <summary>
        /// Отмена начисления по тарифу решения
        /// </summary>
        [Display("Отмена начисления по тарифу решения")]
        DecisionTariffCharge = 20,

        /// <summary>
        /// Отмена пени
        /// </summary>
        [Display("Отмена пени")]
        Penalty = 30,

        /// <summary>
        /// Ручная корректировка по базовому тарифу
        /// </summary>
        [Display("Ручная корректировка по базовому тарифу")]
        BaseTariffChange = 40,

        /// <summary>
        /// Ручная корректировка по тарифу решения
        /// </summary>
        [Display("Ручная корректировка по тарифу решения")]
        DecisionTariffChange = 50,

        /// <summary>
        /// Ручная корректировка по пени
        /// </summary>
        [Display("Ручная корректировка по пени")]
        PenaltyChange = 60
    }
}
