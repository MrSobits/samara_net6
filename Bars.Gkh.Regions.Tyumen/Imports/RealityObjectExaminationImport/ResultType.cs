namespace Bars.Gkh.Regions.Tyumen.Import.RealityObjectExaminationImport
{
    using Bars.B4.Utils;

    /// <summary>
    /// Тип результата добавления дома 
    /// </summary>
    public enum ResultType
    {
        [Display("")]
        None = 1,

        [Display("Да")]
        Yes = 10,

        [Display("Да, но с ошибкой")]
        YesButWithErrors = 20,

        [Display("Нет")]
        No = 50

    }
}