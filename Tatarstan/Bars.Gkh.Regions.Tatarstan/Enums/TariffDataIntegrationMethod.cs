using Bars.B4.Utils;

namespace Bars.Gkh.Regions.Tatarstan.Enums
{
    /// <summary>
    /// Методы интеграции данных по тарифам
    /// </summary>
    public enum TariffDataIntegrationMethod
    {
        /// <summary>
        /// Запрос списка объектов жилищного фонда
        /// </summary>
        [Display("Запрос списка объектов жилищного фонда")]
        RequestHousingFacilities = 1,

        /// <summary>
        /// Запрос списка помещений
        /// </summary>
        [Display("Запрос списка помещений")]
        RequestPremises = 2,

        /// <summary>
        /// Запрос сведений по тарифам
        /// </summary>
        [Display("Запрос сведений по тарифам")]
        RequestInformationTariffs = 3
    }
}
