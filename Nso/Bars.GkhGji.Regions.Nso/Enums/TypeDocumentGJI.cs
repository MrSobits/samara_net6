namespace Bars.GkhGji.Regions.Nso.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Тип документа ГЖИ для ЯНАО
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

        [Display("Постановление прокуратуры")]
        ResolutionProsecutor = 80,

        [Display("Представление")]
		Presentation = 90,

		[Display("Протокол по ст.19.7 КоАП РФ")] // Тип используется в НСО
		Protocol197 = 140
    }
}