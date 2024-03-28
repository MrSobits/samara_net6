namespace Bars.Gkh.ConfigSections.Overhaul.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Статус документа
    /// </summary>
    public enum DocumentState
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
        Cancelled = 20
    }
}