using Bars.B4.Utils;

namespace Bars.B4.Modules.FIAS.Enums
{
    public enum FiasEstimateStatusEnum: byte
    {
        /// <summary>
        /// Не определено
        /// </summary>
        [Display("")]
        [Description("Не определено")]
        NotDefined = 0,

        /// <summary>
        /// Владение
        /// </summary>
        [Display("владение")]
        [Description("Владение")]
        Ownership = 1,

        /// <summary>
        /// Дом
        /// </summary>
        [Display("дом")]
        [Description("Дом")]
        House = 2,

        /// <summary>
        /// Домовладение
        /// </summary>
        [Display("домовладение")]
        [Description("Домовладение")]
        HouseOwnership = 3,

        /// <summary>
        /// Гараж
        /// </summary>
        [Display("гараж")]
        [Description("Гараж")]
        Garage = 4,

        /// <summary>
        /// Здание
        /// </summary>
        [Display("здание")]
        [Description("Здание")]
        Building = 5,

        /// <summary>
        /// Шахта
        /// </summary>
        [Display("шахта")]
        [Description("Шахта")]
        Mine = 6,
    }
}
