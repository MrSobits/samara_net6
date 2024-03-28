namespace Bars.Gkh.Modules.ClaimWork.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Статус реструктуризации долга
    /// </summary>
    public enum RestructDebtStatus
    {
        /// <summary>
        /// Не задано
        /// </summary>
        [Display("Не задано")]
        NotSet,

        /// <summary>
        /// Не оплачено
        /// </summary>
        [Display("Не оплачено")]
        NotPaid = 10,

        /// <summary>
        /// Оплачено
        /// </summary>
        [Display("Оплачено")]
        Paid = 20,

        /// <summary>
        /// Просрочено
        /// </summary>
        [Display("Просрочено")]
        Expired = 30
    }
}