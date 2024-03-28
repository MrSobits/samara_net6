namespace Bars.Gkh.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Статус голосования
    /// </summary>
    public enum VotingStatus
    {
        /// <summary>
        /// Размещен
        /// </summary>
        [Display("Размещен")]
        Placed = 0,

        /// <summary>
        /// Отменены последние изменения
        /// </summary>
        [Display("Отменены последние изменения")]
        CanceledLastChanges = 1,

        /// <summary>
        /// Удален
        /// </summary>
        [Display("Удален")]
        Removed = 2
    }
}