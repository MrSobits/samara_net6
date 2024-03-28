namespace Bars.Gkh.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Ставка рефинансирования (при отсутствии оплат)
    /// </summary>
    public enum RefinancingRate
    {
        /// <summary>
        /// По ставке, действующей на текущий расчетный период
        /// </summary>
        [Display("По ставке, действующей на текущий расчетный период")]
        CurrentPeriod = 0,

        /// <summary>
        /// По ставке, согласно указанным настройкам
        /// </summary>
        [Display("По ставке, согласно указанным настройкам")]
        AsConfigured = 1
    }
}