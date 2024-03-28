namespace Bars.Gkh.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Группа прибора учета
    /// </summary>
    public enum MeteringDeviceGroup
    {
        [Display("Приборы учета ГВС")]
        HotWater = 10,

        [Display("Приборы учета ХВС")]
        ColdWater = 20,

        [Display("Приборы учета отопления")]
        Heating = 30,

        [Display("Приборы учета электричества")]
        Electricity = 40,

        [Display("Приборы учета газоснабжения")]
        Gas = 50
    }
}