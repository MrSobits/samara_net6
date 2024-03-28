namespace Bars.Gkh.Regions.Tatarstan.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Вид норматива потребления
    /// </summary>
    public enum NormConsumptionType
    {
        /// <summary>
        /// Холодное водоснабжение
        /// </summary>
        [Display ("Холодное водоснабжение")]
        ColdWater = 10,

        /// <summary>
        /// Горячее водоснабжение
        /// </summary>
        [Display("Горячее водоснабжение")]
        HotWater = 20,

        /// <summary>
        /// Подогрев
        /// </summary>
        [Display("Подогрев")]
        Heating = 30,

        /// <summary>
        /// Отопление
        /// </summary>
        [Display("Отопление")]
        Firing = 40
    }
}