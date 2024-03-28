namespace Bars.B4.Modules.Analytics.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Способ создания.
    /// </summary>
    public enum OwnerType
    {
        /// <summary>
        /// Пользовательский
        /// </summary>
        [Display("Пользовательский")]
        User = 1,

        /// <summary>
        /// Системный
        /// </summary>
        [Display("Системный")]
        System = 2
    }
}
