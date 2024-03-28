namespace Bars.GkhGji.Regions.Nnovgorod.Report.ActCheckGji
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;

    public class ActCheckGjiDefinitionReport : GkhGji.Report.ActCheckGjiDefinitionReport
    {
        public IDomainService<InspectionAppealCits> InspectionAppealCitsDomainService { get; set; }

        protected override void FillParams(ReportParams reportParams, ActCheckDefinition definition)
        {
            var act = definition.ActCheck;
            if (act.Inspection.TypeBase == TypeBase.CitizenStatement)
            {
                var appealsBaseStatementQuery = InspectionAppealCitsDomainService.GetAll()
                    .Where(x => x.Inspection.Id == act.Inspection.Id);

                var appealsBaseStatement = appealsBaseStatementQuery
                    .Select(x => new
                    {
                        x.AppealCits.DocumentNumber,
                        x.AppealCits.DateFrom
                    })
                    .ToList();

                var appealsBaseStatementNum = string.Join(
                    ", ",
                    appealsBaseStatement
                        .Select(y => string.Format("{0} от {1}", y.DocumentNumber, y.DateFrom.ToDateTime().ToShortDateString()))
                        .Where(x => !string.IsNullOrWhiteSpace(x)));

                reportParams.SimpleReportParams["Обращения"] = appealsBaseStatementNum;
            }
        }
    }
}