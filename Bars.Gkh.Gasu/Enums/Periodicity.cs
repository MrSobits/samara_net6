namespace Bars.Gkh.Gasu.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Периодичность
    /// </summary>
    public enum Periodicity
    {
        /// <summary>
        /// Ежегодно
        /// </summary>
        [Display("Ежегодно")]
        Annually = 1,

        /// <summary>
        /// Ежеквартально
        /// </summary>
        [Display("Ежеквартально")]
        Quarterly = 3,

        /// <summary>
        /// Ежемесячно
        /// </summary>
        [Display("Ежемесячно")]
        Monthly = 4
    }
}