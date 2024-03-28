namespace Bars.Gkh.Overhaul.Hmao.Enum
{
    using Bars.B4.Utils;

    /// <summary>
    /// Статус документа ДПКР
    /// </summary>
    public enum DpkrDocumentState
    {
        /// <summary>
        /// Действует
        /// </summary>
        [Display("Действует")]
        Active = 10,

        /// <summary>
        /// Аннулирован
        /// </summary>
        [Display("Аннулирован")]
        Cancelled = 20,

        /// <summary>
        /// Черновик
        /// </summary>
        [Display("Черновик")]
        Draft = 30
    }
}
