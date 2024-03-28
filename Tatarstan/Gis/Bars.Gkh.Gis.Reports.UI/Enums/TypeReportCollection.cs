namespace Bars.Gkh.Gis.Reports.UI.Enums
{
    using B4.Utils;

    /// <summary>
    /// Тип отчета по начислениям субсидируемых услуг
    /// </summary>
    public enum TypeReportCollection
    {
        [Display("Все субсидируемые")]
        AllSubsidized = 10,

        [Display("Субсидируемые без газа")]
        SubsidizedWithoutGas = 20
    } 
}
