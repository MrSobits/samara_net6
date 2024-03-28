namespace Bars.Gkh.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Правомерность собрания
    /// </summary>
    public enum LegalityMeeting
    {
        /// <summary>
        /// Правомерно
        /// </summary>
        [Display("Правомерно")]
        Lawfully = 0,

        /// <summary>
        /// Неправомерно
        /// </summary>
        [Display("Неправомерно")]
        Wrongfully = 1
    }
}