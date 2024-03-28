namespace Bars.Gkh.Gis.Enum
{
    using B4.Utils;

    /// <summary>
    /// Группы индикаторов
    /// </summary>
    public enum TypeGroupIndicators
    {
        [Display("ГВС")] 
        HotWater = 9,

        [Display("ХВС")] 
        ColdWster = 6,

        [Display("Водоотведение")] 
        OutWater = 7,

        [Display("Отопление")] 
        Heating = 8,

        [Display("Электроснабжение")] 
        Electricity = 25,

        [Display("ОДН - ГВС")]
        HotWaterOdn = 513,

        [Display("ОДН - ХВС")]
        ColdWsterOdn = 510,

        [Display("ОДН - Водоотведение")]
        OutWaterOdn = 511,

        [Display("ОДН - Отопление")]
        HeatingOdn = 512,

        [Display("ОДН - Электроснабжение")]
        ElectricityOdn = 515,

        [Display("Сводные значения без ОДН")]
        AllWithoutOdn = 40,

        [Display("Сводные значения ОДН")]
        AllWithOdn = 50,

        [Display("Прочие услуги")]
        AllOtherServices = 60
    }
}
