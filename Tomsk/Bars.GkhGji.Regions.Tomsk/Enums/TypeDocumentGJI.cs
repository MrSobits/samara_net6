// Короче данный енум используется только для клиентской части в серверной никак неучаствует
namespace Bars.GkhGji.Regions.Tomsk.Enums
{
    using Bars.B4.Application;
    using Bars.B4.Utils;

    /// <summary>
    /// Тип документа ГЖИ для Томска
    /// </summary>
    public enum TypeDocumentGji
    {
        [Display("Приказ")]
        Disposal = 10,

        [Display("Акт проверки")]
        ActCheck = 20,

        [Display("Акт устранения нарушений")]
        ActRemoval = 30,

        [Display("Акт обследования")]
        ActSurvey = 40,

        [Display("Предписание")]
        Prescription = 50,

        [Display("Протокол")]
        Protocol = 60,

        [Display("Постановление")]
        Resolution = 70,

        /*
        [Display("Постановление прокуратуры")]
        ResolutionProsecutor = 80,
        */
        [Display("Представление")]
        Presentation = 90,

        [Display("Акт визуального обследования")] // Тип используется в Томске
        ActVisual = 100,

        [Display("Административное дело")] // Тип используется в Томске
        AdministrativeCase = 110
        /*
        ,

        [Display("Протокол МВД")]
        ProtocolMvd = 120,

        [Display("Протокол МЖК")]
        ProtocolMhc = 130
        */
    }
}