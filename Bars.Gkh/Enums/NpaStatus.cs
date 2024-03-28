namespace Bars.Gkh.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Статус НПА
    /// </summary>
    public enum NpaStatus
    {
        /// <summary>
        /// Действует
        /// </summary>
        [Display("Действует")]
        Running = 1,

        /// <summary>
        /// Удалена
        /// </summary>
        [Display("Удалена")]
        Removed = 2,

        /// <summary>
        /// Аннулирована
        /// </summary>
        [Display("Аннулирована")]
        Canceled = 3
    }
}