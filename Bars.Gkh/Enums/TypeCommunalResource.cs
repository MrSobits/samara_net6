namespace Bars.Gkh.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Вид коммунального ресурса
    /// </summary>
    public enum TypeCommunalResource
    {

        /// <summary>
        /// Не определено
        /// </summary>
        [Display("Не определено")]
        Undefined = 0,

        /// <summary>
        /// Бытовой газ в баллонах
        /// </summary>
        [Display("Бытовой газ в баллонах")]
        HomeGasInBulbs = 10,

        /// <summary>
        /// Газ
        /// </summary>
        [Display("Газ")]
        Gas = 20,

        /// <summary>
        /// Горячая вода
        /// </summary>
        [Display("Горячая вода")]
        HotWater = 30,

        /// <summary>
        /// Сточные бытовые воды
        /// </summary>
        [Display("Сточные бытовые воды")]
        WasteHomeWaters = 40,

        /// <summary>
        /// Твердое топливо
        /// </summary>
        [Display("Твердое топливо")]
        SolidFuel = 50,

        /// <summary>
        /// Тепловая энергия
        /// </summary>
        [Display("Тепловая энергия")]
        ThermalEnergy = 60,

        /// <summary>
        /// Холодная вода
        /// </summary>
        [Display("Холодная вода")]
        ColdWater = 70,

        /// <summary>
        /// Электрическая энергия
        /// </summary>
        [Display("Электрическая энергия")]
        ElectricalEnergy = 80
    }
}
