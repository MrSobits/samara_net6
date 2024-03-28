namespace Bars.GisIntegration.Inspection.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Вид документа по результатам проверки
    /// </summary>
    public enum ExaminationResultDocType
    {
        [Display("Акт проверки")]
        ActCheck = 10,

        [Display("Протокол")]
        Protocol = 20
    }
}
