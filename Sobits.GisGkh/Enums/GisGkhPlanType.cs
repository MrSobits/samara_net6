namespace Sobits.GisGkh.Enums
{
    using Bars.B4.Utils;
    using GisGkhLibrary.HcsCapitalRepairAsync;

    /// <summary>
    /// Вид КПР для запроса exportPlan
    /// Соответствует exportPlanRequestType
    /// </summary>
    public enum GisGkhPlanType
    {
        /// <summary>
        /// КПР
        /// </summary>
        [Display("КПР")]
        KPR = 0,

        /// <summary>
        /// МАПКР
        /// </summary>
        [Display("МАПКР")]
        MAPKR = 1,

        /// <summary>
        /// РАПКР
        /// </summary>
        [Display("РАПКР")]
        RAPKR = 2
    }
}