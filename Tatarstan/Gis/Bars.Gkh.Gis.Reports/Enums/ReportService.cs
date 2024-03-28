namespace Bars.Gkh.Gis.Reports.Enums
{
    using B4.Utils;

    /// <summary>
    /// Услуги для отчетов
    /// </summary>
    public enum ReportService
    {
        /// <summary>
        /// Холодная вода
        /// </summary>
        [Display("Холодная вода")]
        ColdWater = 6,

        /// <summary>
        /// Канализация
        /// </summary>
        [Display("Канализация")]
        Sewage = 7,

        /// <summary>
        /// Отопление
        /// </summary>
        [Display("Отопление")]
        Heating = 8,

        /// <summary>
        /// Горячая вода
        /// </summary>
        [Display("Горячая вода")]
        HotWater = 9,

        /// <summary>
        /// Электроснабжение
        /// </summary>
        [Display("Электроснабжение")]
        Electricity = 25,

        /// <summary>
        /// Электроснабжение (ночное)
        /// </summary>
        [Display("Электроснабжение (ночное)")]
        ElectricityNight = 210
    }
}
