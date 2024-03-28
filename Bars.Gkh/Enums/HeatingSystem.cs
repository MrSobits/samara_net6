namespace Bars.Gkh.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Система отопления
    /// </summary>
    public enum HeatingSystem
    {
        [Display("Не задано")]
        None = 0,

        [Display("Индивидуальное")]
        Individual = 10,

        [Display("Централизованное")]
        Centralized = 20,

        [Display("Печное")]
        Stove = 30,

        [Display("Электрическое")]
        Electric = 40,

        [Display("Домовая котельная")]
        HomeBoiler = 50,

        [Display("Квартирное отопление (котел)")]
        ApartmentBoiler = 60,

        [Display("Независимое (через теплообменники), двухтрубное, вертикальное, с нижней разводкой магистралей")]
        Independent = 70
    }
}