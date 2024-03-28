namespace Bars.Gkh.Enums.ClaimWork
{
    using Bars.B4.Utils;

    /// <summary>
    /// Тип суммы задолженности
    /// </summary>
    [Display("Сумма задолженности")]
    public enum DebtSumType
    {
        /// <summary>
        /// Без учета пени
        /// </summary>
        [Display("Без учета пени")]
        WithoutPenalty = 0,

        /// <summary>
        /// С учетом пени
        /// </summary>
        [Display("С учетом пени")]
        WithPenalty = 1,

        /// <summary>
        /// Не учитывается
        /// </summary>
        [Display("Не учитывается")]
        NotUsed = 2
    }
}