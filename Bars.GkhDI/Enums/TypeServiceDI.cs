namespace Bars.GkhDi.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Тип услуги
    /// </summary>
    public enum TypeServiceDi
    {
        /// <summary>
        /// Отопление
        /// </summary>
        [Display("Отопление")]
        Heating = 10,

        /// <summary>
        /// Электричество
        /// </summary>
        [Display("Электричество")]
        Electrical = 20,

        /// <summary>
        /// Газ
        /// </summary>
        [Display("Газ")]
        Gas = 30,

        /// <summary>
        /// Горячее водоснабжение
        /// </summary>
        [Display("Горячее водоснабжение")]
        HotWater = 40,

        /// <summary>
        /// Холодное водоснабжение
        /// </summary>
        [Display("Холодное водоснабжение")]
        ColdWater = 50,

        /// <summary>
        /// Водоотведение
        /// </summary>
        [Display("Водоотведение")]
        Wastewater = 60,

        /// <summary>
        /// Тепловая энергия для нужд отопления
        /// </summary>
        [Display("Тепловая энергия для нужд отопления")]
        ThermalEnergyForHeating = 61,

        /// <summary>
        /// Тепловая энергия для нужд горячего водоснабжения
        /// </summary>
        [Display("Тепловая энергия для нужд горячего водоснабжения")]
        ThermalEnergyForNeedsOfHotWater = 62,

        /// <summary>
        /// Прочие ресурсы (услуги)
        /// </summary>
        [Display("Прочие ресурсы (услуги)")]
        OtherSource = 65,

        /// <summary>
        /// Итого по всем
        /// </summary>
        [Display("Итого по всем")]
        Summury = 70
    }
}
