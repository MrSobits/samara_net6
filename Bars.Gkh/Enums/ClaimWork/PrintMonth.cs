namespace Bars.Gkh.Enums.ClaimWork
{
    using Bars.B4.Utils;

    /// <summary>
    /// Месяц печати
    /// </summary>
    [Display("Месяц печати")]
    public enum PrintMonth
    {
        /// <summary>
        /// Текущий
        /// </summary>
        [Display("Текущий")]
        Current = 0,

        /// <summary>
        /// Следующий
        /// </summary>
        [Display("Следующий")]
        Next = 1
    }
}
