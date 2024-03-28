namespace Bars.Gkh.Reforma.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Тип интеграции
    /// </summary>
    public enum TypeIntegration
    {
        /// <summary>
        /// Автоматическая интеграция
        /// </summary>
        [Display("Автоматическая интеграция")]
        Automatic = 10,

        /// <summary>
        /// Выборочная интеграция
        /// </summary>
        [Display("Выборочная интеграция")]
        Selection = 20,

        /// <summary>
        /// Ручная интеграция
        /// </summary>
        [Display("Ручная интеграция")]
        Manual = 30
    }
}
