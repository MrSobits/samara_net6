namespace Bars.GisIntegration.Base.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Тип дома
    /// </summary>
    public enum HouseType
    {
        /// <summary>
        /// Многоквартирный
        /// </summary>
        [Display("Многоквартирный")]
        Apartment = 10,

        /// <summary>
        /// Жилой
        /// </summary>
        [Display("Жилой")]
        Living = 20,

        /// <summary>
        /// ЖД блокированной застройки
        /// </summary>
        [Display("ЖД блокированной застройки")]
        Blocks = 30
    }
}