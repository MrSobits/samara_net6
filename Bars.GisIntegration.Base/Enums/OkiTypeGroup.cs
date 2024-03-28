namespace Bars.GisIntegration.Base.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Типы групп объектов Ком. услуг.
    /// </summary>
    public enum OkiTypeGroup
    {

        [Display("Объект водоснабжения")]
        WaterSupply = 1,
        [Display("Объект теплоснабжения")]
        HeatSupply = 2,
        [Display("Объект водоотведения")]
        Sewerage = 3,
        [Display("Объект газоснабжения")]
        GasSupply = 4,
        [Display("Объект электроснабжения")]
        ElectricPowerSupply = 5
    }
}
