namespace Bars.GkhCalendar.Enums
{
    using Bars.B4.Utils;

    public enum ChangeAppointmentDay
    {
        [Display("Неприёмный")]
        Not = 10,

        [Display("Приёмный")]
        Yes = 20,

        [Display("Изменено время приёма")]
        Changetime = 30,
    }
}