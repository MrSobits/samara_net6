namespace Bars.Gkh.Gis.Enum.ManOrg
{
    using B4.Utils;

    /// <summary>
    /// Коммунальный ресурс
    /// </summary>
    public enum CommunalServiceResource
    {
        /// <summary>
        /// Не задан
        /// </summary>
        [Display("Не задан")]
        Default = 0,

        /// <summary>
        /// Бытовой газ в баллонах
        /// </summary>
        [Display("Бытовой газ в баллонах")]
        GasCylinder = 1,

        /// <summary>
        /// Газ
        /// </summary>
        [Display("Газ")]
        Gas = 2,

        /// <summary>
        /// Горячая вода
        /// </summary>
        [Display("Горячая вода")]
        HotWater = 3,

        /// <summary>
        /// Сточные бытовые воды
        /// </summary>
        [Display("Сточные бытовые воды")]
        WasteWater = 4,

        /// <summary>
        /// Твердое топливо
        /// </summary>
        [Display("Твердое топливо")]
        SolidFuel = 5,

        /// <summary>
        /// Тепловая энергия
        /// </summary>
        [Display("Тепловая энергия")]
        ThermalEnergy = 6,

        /// <summary>
        /// Холодная вода
        /// </summary>
        [Display("Холодная вода")]
        ColdWater = 7,

        /// <summary>
        /// Электрическая энергия
        /// </summary>
        [Display("Электрическая энергия")]
        ElectricEnergy = 8
    }
}
