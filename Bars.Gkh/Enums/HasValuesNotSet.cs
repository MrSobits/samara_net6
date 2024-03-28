namespace Bars.Gkh.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Имеются/Отсутствуют/Не задано
    /// </summary>
    public enum HasValuesNotSet
    {
        /// <summary>
        /// Имеются
        /// </summary>
        [Display("Имеются")]
        Yes = 10,

        /// <summary>
        /// Отсутствуют
        /// </summary>
        [Display("Отсутствуют")]
        No = 20,

        /// <summary>
        /// Не задано
        /// </summary>
        [Display("Не задано")]
        NotSet = 30
    }
}