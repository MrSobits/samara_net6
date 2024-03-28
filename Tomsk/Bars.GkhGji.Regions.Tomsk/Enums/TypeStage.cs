namespace Bars.GkhGji.Regions.Tomsk.Enums
{
    using Bars.B4.Application;
    using Bars.B4.Utils;
    using Bars.GkhGji.Contracts;

    /// <summary>
    /// Этап проверки ГЖИ для Томска
    /// </summary>
    public enum TypeStage
    {
        [Display("Приказ")]
        Disposal = 10,

        [Display("Приказ на проверку предписания")]
        DisposalPrescription = 20,

        [Display("Акт проверки")]
        ActCheck = 30,

        [Display("Акт проверки (общий)")]
        ActCheckGeneral = 40,

        [Display("Акт обследования")]
        ActSurvey = 50,

        [Display("Предписание")]
        Prescription = 60,

        [Display("Протокол")]
        Protocol = 70,

        [Display("Постановление")]
        Resolution = 80,

        [Display("Акт проверки предписания")]
        ActRemoval = 90,

        [Display("Постановление прокуратуры")]
        ResolutionProsecutor = 100,

        [Display("Представление")]
        Presentation = 110,

        [Display("Акт визуального осмотра")] // Тип используется в Томске
        ActVisual = 120,

        [Display("Административное дело")] // Тип используется в Томске
        AdministrativeCase = 130
    }
}