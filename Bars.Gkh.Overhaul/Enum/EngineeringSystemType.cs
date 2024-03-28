namespace Bars.Gkh.Overhaul.Enum
{
    using Bars.B4.Utils;

    /// <summary>
    /// Тип инженерной системы
    /// </summary>
    public enum EngineeringSystemType
    {
        /// <summary>
        /// Не задано
        /// </summary>
        [Display("Не задано")]
        Default = 0,

        /// <summary>
        /// Центральная
        /// </summary>
        [Display("Центральная")]
        Central = 10,

        /// <summary>
        /// Автономная
        /// </summary>
        [Display("Автономная")]
        Autonomy = 20,

        /// <summary>
        /// Комбинированная
        /// </summary>
        [Display("Комбинированная")]
        Combined = 30
    }
}