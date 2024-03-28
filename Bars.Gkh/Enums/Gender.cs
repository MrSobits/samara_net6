namespace Bars.Gkh.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Пол
    /// </summary>
    public enum Gender
    {
        /// <summary>
        /// Не задано
        /// </summary>
        [Display("Не задано")]
        NotSet = 0,

        /// <summary>
        /// Мужской
        /// </summary>
        [Display("Мужской")]
        Male = 10,

        /// <summary>
        /// Женский
        /// </summary>
        [Display("Женский")]
        Female = 20
    }
}