namespace Bars.GkhCr.Enums
{
    using B4.Utils;

    /// <summary>
    /// Тип договора капитального ремонта
    /// </summary>
    public enum TypeContractBuild
    {
        [Display("Не задано")]
        NotDefined = 10,

        [Display("На ПСД")]
        Psd = 11,

        [Display("На ПСД ОКН")]
        PsdOkn = 12,

        [Display("На СК")]
        SK = 13,

        [Display("На СМР ОКН")]
        SKOKN = 14,

        [Display("На СМР")]
        Smr = 20,

        [Display("На приборы")]
        Device = 30,

        [Display("На лифты")]
        Lift = 40,

        [Display("На энергообследование")]
        EnergySurvey = 50,

        [Display("На ПД СМР")]
        PDSMR = 55,

        [Display("На ТО")]
        TO = 60
    }
}
