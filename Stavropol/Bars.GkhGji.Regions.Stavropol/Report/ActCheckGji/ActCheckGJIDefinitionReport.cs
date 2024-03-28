namespace Bars.GkhGji.Regions.Stavropol.Report
{
    using B4.Modules.Reports;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Report;

    public class StavropolActCheckGjiDefinitionReport : ActCheckGjiDefinitionReport
    {
        protected override void FillParams(ReportParams reportParams, ActCheckDefinition definition)
        {
            reportParams.SimpleReportParams["НомерОпределения"] = definition.DocumentNumber != null
                ? definition.DocumentNumber.ToString()
                : definition.DocumentNum;
        }
    }
}