namespace Bars.Gkh.Enums.ClaimWork
{
    using Bars.B4.Utils;

    /// <summary>
    /// Тип суда
    /// </summary>
    [Display("Тип суда")]
    public enum CourtType
    {
        /// <summary>
        /// Не задано
        /// </summary>
        [Display("Не задано")]
        NotSet,

        /// <summary>
        /// Мировой суд
        /// </summary>
        [Display("Мировой суд")]
        Magistrate = 10,

        /// <summary>
        /// Арбитражный суд
        /// </summary>
        [Display("Арбитражный суд")]
        Arbitration = 20,
        
        /// <summary>
        /// Районный (городской) суд
        /// </summary>
        [Display("Районный (городской) суд")]
        District = 30
    }
}