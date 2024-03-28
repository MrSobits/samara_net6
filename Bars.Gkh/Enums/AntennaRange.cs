namespace Bars.Gkh.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Диапазон
    /// </summary>
    public enum AntennaRange
    {
        /// <summary>
        /// Не выбран
        /// </summary>
        [Display("-")]
        None = 0,

        /// <summary>
        /// Метровый
        /// </summary>
        [Display("Метровый")]
        Meter = 10,

        /// <summary>
        /// Дециметровый
        /// </summary>
        [Display("Дециметровый")]
        Decimeter = 20,

        /// <summary>
        /// Смешанный
        /// </summary>
        [Display("Смешанный")]
        Hybrid = 30
    }
}