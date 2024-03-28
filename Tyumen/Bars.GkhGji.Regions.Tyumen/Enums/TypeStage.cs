namespace Bars.GkhGji.Regions.Tyumen.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Этап проверки ГЖИ
    /// </summary>
    public enum TypeStage
    {
        /// <summary>
        /// Приказ
        /// </summary>
        [Display("Приказ")]
        Disposal = 10,

        /// <summary>
        /// Приказ на проверку предписания
        /// </summary>
        [Display("Приказ на проверку предписания")]
        DisposalPrescription = 20,

        /// <summary>
        /// Акт проверки
        /// </summary>
        [Display("Акт проверки")]
        ActCheck = 30,

        /// <summary>
        /// Акт проверки (общий)
        /// </summary>
        [Display("Акт проверки (общий)")]
        ActCheckGeneral = 40,

        /// <summary>
        /// Акт обследования
        /// </summary>
        [Display("Акт обследования")]
        ActSurvey = 50,

        /// <summary>
        /// Предписание
        /// </summary>
        [Display("Предписание")]
        Prescription = 60,

        /// <summary>
        /// Протокол
        /// </summary>
        [Display("Протокол")]
        Protocol = 70,

        /// <summary>
        /// Постановление
        /// </summary>
        [Display("Постановление")]
        Resolution = 80,

        /// <summary>
        /// Акт проверки предписания
        /// </summary>
        [Display("Акт проверки предписания")]
        ActRemoval = 90,

        /// <summary>
        /// Постановление прокуратуры
        /// </summary>
        [Display("Постановление прокуратуры")]
        ResolutionProsecutor = 100,

        /// <summary>
        /// Представление
        /// </summary>
        [Display("Представление")]
        Presentation = 110
    }
}