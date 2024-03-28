namespace Bars.Gkh.Gis.Enum.ManOrg
{
    using B4.Utils;

    /// <summary>
    /// Наименование коммунальной услуги
    /// </summary>
    public enum CommunalServiceName
    {
        /// <summary>
        /// Не задано
        /// </summary>
        [Display("Не задано")]
        Default = 0,

        /// <summary>
        /// Водоотведение
        /// </summary>
        [Display("Водоотведение")]
        WasteWater = 1,

        /// <summary>
        /// Газоснабжение
        /// </summary>
        [Display("Газоснабжение")]
        GasSupply = 2,

        /// <summary>
        /// Горячее водоснабжение
        /// </summary>
        [Display("Горячее водоснабжение")]
        HotWaterSupply = 3,

        /// <summary>
        /// Отопление
        /// </summary>
        [Display("Отопление")]
        Heating = 4,

        /// <summary>
        /// Холодное водоснабжение
        /// </summary>
        [Display("Холодное водоснабжение")]
        ColdWaterSupply = 5,

        /// <summary>
        /// Электроснабжение
        /// </summary>
        [Display("Электроснабжение")]
        ElectricPowerSupply = 6
    }
}
