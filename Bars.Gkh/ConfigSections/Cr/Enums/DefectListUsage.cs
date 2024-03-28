namespace Bars.Gkh.ConfigSections.Cr.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Использование дефектной ведомости
    /// </summary>
    public enum DefectListUsage
    {
        /// <summary>
        /// Не использовать
        /// </summary>
        [Display("Не использовать")]
        DontUse = 0,

        /// <summary>
        /// Использовать
        /// </summary>
        [Display("Использовать")]
        Use = 10
    }
}