namespace Bars.Gkh.Enums.ClaimWork
{
    using Bars.B4.Utils;

    /// <summary>
    /// Уведомление. Сумма задолженности
    /// </summary>
    [Display("Сумма задолженности")]
    public enum NotifSumType
    {
        /// <summary>
        /// Не учитывается
        /// </summary>
        [Display("Не учитывается")]
        Ignore = 0,

        /// <summary>
        /// С учетом пени
        /// </summary>
        [Display("С учетом пени")]
        WithPenalty = 1,

        /// <summary>
        /// Без учета пени
        /// </summary>
        [Display("Без учета пени")]
        WithoutPenalty = 2
    }
}