namespace Bars.Gkh.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Типы документов агент ПИР
    /// </summary>
    public enum AgentPIRDocumentType
    {
        [Display("Заявление на выдачу судебного приказа")]
        ApplicationIssuanceCourtOrder = 10,

        [Display("Судебный приказ")]
        CourtOrder = 20,

        [Display("Отмененный судебный приказ")]
        CancelledCourtOrder = 25,

        [Display("Заявление в РОСП")]
        ApplicationROSP = 30,

        [Display("Исковое заявление")]
        StatementClaim = 40,

        [Display("Определение об отмене")]
        Cancellation = 50,

        [Display("Постановление о возбуждении исполнительного производства")]
        Resolution = 60,

        [Display("Определения об исправлении описки в решении суда")]
        ErrorFix = 70,

        [Display("Исполнительный лист")]
        SSPList = 80,

        [Display("Решение суда по исковому производству")]
        CourtDecision = 90,
    }
}
