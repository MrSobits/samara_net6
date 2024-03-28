namespace Bars.GkhGji.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Источник поступления
    /// </summary>
    public enum SSTUSource
    {
        /// <summary>
        /// Напрямую от заявителя
        /// </summary>
        [Display("Напрямую от заявителя")]
        Direct = 10,

        /// <summary>
        /// Переадресованы из иного органа
        /// </summary>
        [Display("Переадресованы из иного органа")]
        SSTU = 20,

    }
}