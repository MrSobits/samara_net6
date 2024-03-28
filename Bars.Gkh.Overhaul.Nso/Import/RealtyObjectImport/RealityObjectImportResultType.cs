namespace Bars.Gkh.Overhaul.Nso.Import
{
    using B4.Utils;

    /// <summary>
    /// Тип результата добавления дома 
    /// </summary>
    public enum RealityObjectImportResultType
    {
        [Display("")]
        None = 1,

        [Display("Да")]
        Yes = 10,

        [Display("Да, но с ошибкой")]
        YesButWithErrors = 20,

        [Display("Имелся в системе")]
        AlreadyExists = 30,

        [Display("Не соответствует ФИАС")]
        DoesNotMeetFias = 40,

        [Display("Нет")]
        No = 50

    }
}