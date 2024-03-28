namespace Bars.Gkh.Gis.Enum
{
    using B4.Utils;

    /// <summary>
    /// Типы отходов
    /// </summary>
    public enum TypeWaste
    {
        /// <summary>
        /// Твердые
        /// </summary>
        [Display("ТБО")]
        Solid = 0,

        /// <summary>
        /// Жидкие
        /// </summary>
        [Display("ЖБО")]
        Liquid = 1
    }
}