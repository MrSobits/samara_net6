namespace Bars.Gkh.Modules.ClaimWork.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// ИП в отношении
    /// </summary>
    public enum CollectDebtFrom
    {
        /// <summary>
        /// Должник
        /// </summary>
        [Display("Должник")]
        Debtor = 10,

        /// <summary>
        /// Ответчик
        /// </summary>
        [Display("Ответчик")]
        Answerer = 20,

        /// <summary>
        /// ФКР
        /// </summary>
        [Display("ФКР")]
        Fkr = 30
    }
}