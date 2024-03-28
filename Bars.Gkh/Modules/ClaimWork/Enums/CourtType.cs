namespace Bars.Gkh.Modules.ClaimWork.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Тип суда
    /// </summary>
    public enum CourtType
    {
        /// <summary>
        /// Судебный участок мирового суда
        /// </summary>
        [Display("Судебный участок мирового суда")]
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