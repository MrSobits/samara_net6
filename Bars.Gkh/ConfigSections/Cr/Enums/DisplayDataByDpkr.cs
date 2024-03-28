namespace Bars.Gkh.ConfigSections.Cr.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Отображение данных по ДПКР
    /// </summary>
    public enum DisplayDataByDpkr
    {
        /// <summary>
        /// Значения на текущий момент
        /// </summary>
        [Display("Значения на текущий момент")]
        Current = 0,

        /// <summary>
        /// Значения на момент создания ДПКР
        /// </summary>
        [Display("Значения на момент создания ДПКР")]
        Previous = 10
    }
}
