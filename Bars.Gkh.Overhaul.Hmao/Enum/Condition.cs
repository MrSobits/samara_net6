namespace Bars.Gkh.Overhaul.Hmao.Enum
{
    using Bars.B4.Utils;

    /// <summary>
    /// Тип условия
    /// </summary>
    public enum Condition
    {
        /// <summary>
        /// Не задано
        /// </summary>
        [Display("Не задано")]
        NotSet = 0,

        /// <summary>
        /// Больше
        /// </summary>
        [Display("Больше")]
        Greater = 10,

        /// <summary>
        /// Равно
        /// </summary>
        [Display("Равно")]
        Equal = 20,

        /// <summary>
        /// Меньше
        /// </summary>
        [Display("Меньше")]
        Lower = 30,
    }
}