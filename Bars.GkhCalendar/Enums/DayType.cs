namespace Bars.GkhCalendar.Enums
{
    using Bars.B4.Utils;

    public enum DayType
    {
        [Display("Рабочий")]
        Workday = 10,

        [Display("Выходной")]
        DayOff = 20,

        [Display("Праздничный")]
        Holiday = 30,

        [Display("Сегодня")]
        Today = 40
    }
}