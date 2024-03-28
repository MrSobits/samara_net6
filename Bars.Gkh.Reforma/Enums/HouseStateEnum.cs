namespace Bars.Gkh.Reforma.Enums
{
    using Bars.B4.Utils;

    public enum HouseStateEnum {
        /// <summary>
        /// Исправный
        /// </summary>
        [Display("Исправный")]
        normal = 1,

        /// <summary>
        /// Требующий капитального ремонта
        /// </summary>
        [Display("Требующий капитального ремонта")]
        warning = 2,

        /// <summary>
        /// Аварийный
        /// </summary>
        [Display("Аварийный")]
        alarm = 3,

        /// <summary>
        /// Нет данных
        /// </summary>
        [Display("Нет данных")]
        noinfo = 4
    }
}