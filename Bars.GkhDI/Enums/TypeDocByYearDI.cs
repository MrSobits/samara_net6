namespace Bars.GkhDi.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Тип документа по годам
    /// </summary>
    public enum TypeDocByYearDi
    {
        [Display("Сметы доходов и расходов")]
        EstimateIncome = 10,

        [Display("Заключение ревизионной комиссии")]
        ConclusionRevisory = 20,

        [Display("Отчет о выполнении сметы доходов и расходов ")]
        ReportEstimateIncome = 30,

        [Display("Протоколы общих собраний")]
        OwnersMeetingProtocol = 40
    }
}