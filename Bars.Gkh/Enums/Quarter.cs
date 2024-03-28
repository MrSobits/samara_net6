namespace Bars.Gkh.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Квартал
    /// </summary>
    public enum Quarter
    {
        /// <summary>
        /// Первый квартал
        /// </summary>
        [Display("1 квартал")]
        [Description("Первый квартал")]
        First = 10,

        /// <summary>
        /// Второй квартал
        /// </summary>
        [Display("2 квартал")]
        [Description("Второй квартал")]
        Second = 20,

        /// <summary>
        /// Третий квартал
        /// </summary>
        [Display("3 квартал")]
        [Description("Третий квартал")]
        Third = 30,

        /// <summary>
        /// Четвертый квартал
        /// </summary>
        [Display("4 квартал")]
        [Description("Четвертый квартал")]
        Fourth = 40,

        /// <summary>
        /// Весь год
        /// </summary>
        [Display("Весь год")]
        [Description("Весь год")]
        AllYear = 50
    }
}