namespace Bars.Gkh.ConfigSections.General.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Автоматическое проставление признака "Дом не участвует в программе КР"
    /// </summary>
    public enum AutoIsNotInvolved
    {
        /// <summary>
        /// Не использовать
        /// </summary>
        [Display ("Не использовать")]
         NotUse = 0,

        /// <summary>
        /// Использовать
        /// </summary>
        [Display("Использовать")]
        Use = 1
    }
}