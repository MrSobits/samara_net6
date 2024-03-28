namespace Bars.Gkh.Enums
{
    using Bars.B4.Utils;

    public enum GkhStructureType
    {
        /// <summary>
        /// Не определено
        /// </summary>
        [Display("Не определено")]
        NotDefined = 0,

        /// <summary>
        /// Строение
        /// </summary>
        [Display("Строение")]
        Structure = 1,

        /// <summary>
        /// Сооружение
        /// </summary>
        [Display("Сооружение")]
        Construction = 2,

        /// <summary>
        /// Литер
        /// </summary>
        [Display("Литер")]
        Letter = 3
    }
}