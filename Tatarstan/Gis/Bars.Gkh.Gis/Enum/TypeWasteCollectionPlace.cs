namespace Bars.Gkh.Gis.Enum
{
    using B4.Utils;

    /// <summary>
    /// Типы площадки вывоза мусора
    /// </summary>
    public enum TypeWasteCollectionPlace
    {
        /// <summary>
        /// Одиночный контейнер
        /// </summary>
        [Display("Одиночный контейнер")]
        SingleContainer = 0,

        /// <summary>
        /// Контейнерная площадка
        /// </summary>
        [Display("Контейнерная площадка")]
        ContainerPlatform = 1
    }
}