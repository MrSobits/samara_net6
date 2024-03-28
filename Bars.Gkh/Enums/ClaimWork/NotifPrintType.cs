namespace Bars.Gkh.Enums.ClaimWork
{
    using Bars.B4.Utils;

    /// <summary>
    /// Печатать / не печатать
    /// </summary>
    public enum PrintType
    {
        /// <summary>
        /// Не печатать
        /// </summary>
        [Display("Не печатать")]
        NoPrint = 0,

        /// <summary>
        /// Печатать
        /// </summary>
        [Display("Печатать")]
        Print = 1
    }
}
