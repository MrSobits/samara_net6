namespace Bars.Gkh.Modules.ClaimWork.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Статус договора реструктуризации
    /// </summary>
    public enum RestructDebtDocumentState
    {
        /// <summary>
        /// Не задано
        /// </summary>
        [Display("Не задано")]
        NotSet,

        /// <summary>
        /// Действует
        /// </summary>
        [Display("Действует")]
        Active = 10,

        /// <summary>
        /// Расторгнут
        /// </summary>
        [Display("Расторгнут")]
        Terminated = 20,

        /// <summary>
        /// Отменен
        /// </summary>
        [Display("Отменен")]
        Canceled = 30
    }
}