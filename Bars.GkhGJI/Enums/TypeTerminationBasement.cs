namespace Bars.GkhGji.Enums
{
    using Bars.B4.Utils;

    public enum TypeTerminationBasement
    {
        [Display("Не задано")]
        NotSet = 0,

        [Display("Отсутствие события")]
        AbsenceEvent = 5,

        [Display("Малозначительность(устное замечание)")]
        Insignificance = 10,

        [Display("Отсутствие состава")]
        Absence = 20,

        [Display("Истечение срока давности")]
        Expiration = 30,

        [Display("Решение суда")]
        Court = 40
    }
}