namespace Bars.Gkh.Enums.EfficiencyRating
{
    using Bars.B4.Utils;

    /// <summary>
    /// Отображать график в разрезе
    /// </summary>
    public enum ViewParam
    {
        /// <summary>
        /// По детализации
        /// </summary>
        [Display("По детализации")]
        ByLevel = 10,

        /// <summary>
        /// По периоду
        /// </summary>
        [Display("По периоду")]
        ByYear = 20
    }
}