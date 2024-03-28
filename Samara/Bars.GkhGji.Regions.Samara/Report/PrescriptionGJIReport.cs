namespace Bars.GkhGji.Regions.Samara.Report
{
    using System.Linq;
    using System.Text;

    using Bars.B4;
    using Bars.B4.IoC;
    using B4.Modules.Reports;
    using Bars.Gkh.Domain;
    using Bars.GkhGji.Entities;

    public class PrescriptionGjiReport : GkhGji.Report.PrescriptionGjiReport
    {
        protected override void FillRegionParams(ReportParams reportParams, DocumentGji doc)
        {
            var prescription = (Prescription)doc;
            var section = reportParams.ComplexReportParams.ДобавитьСекцию("СекцияНарушенийСамара");
            var prescriptionViolDomain = Container.Resolve<IDomainService<PrescriptionViol>>();
            var violationActionsRemovGjiDomain = Container.Resolve<IDomainService<ViolationActionsRemovGji>>();

            using (Container.Using(prescriptionViolDomain, violationActionsRemovGjiDomain))
            {
                var violationsQuery = prescriptionViolDomain.GetAll()
                .Where(x => x.Document.Id == prescription.Id)
                .Select(x => x.InspectionViolation.Violation.Id);

                var actionsDict = violationActionsRemovGjiDomain.GetAll()
                    .Where(x => violationsQuery.Contains(x.ViolationGji.Id))
                    .Select(x => new { x.ActionsRemovViol.Name, x.ViolationGji.Id })
                    .AsEnumerable()
                    .GroupBy(x => x.Id)
                    .ToDictionary(x => x.Key,
                    x => x.Select(y => y.Name).Aggregate("", (p, k) => p + (!string.IsNullOrEmpty(p) ? ", " + k : k)));
                
                var violations = prescriptionViolDomain.GetAll()
                .Where(x => x.Document.Id == prescription.Id)
                .Select(x => new
                {
                    ViolationId = x.InspectionViolation.Violation.Id,
                    ViolationCodePin = x.InspectionViolation.Violation.CodePin,
                    ViolationName = x.InspectionViolation.Violation.Name,
                    x.Description,
                    x.DatePlanRemoval,
                    InspDatePlanRemoval = x.InspectionViolation.DatePlanRemoval,
                    x.InspectionViolation.Violation.PpRf170,
                    x.InspectionViolation.Violation.PpRf25,
                    x.InspectionViolation.Violation.PpRf307,
                    x.InspectionViolation.Violation.PpRf491,
                    x.InspectionViolation.Violation.OtherNormativeDocs
                })
                .AsEnumerable()
                .Select(x => new
                {
                    x.ViolationId,
                    x.ViolationCodePin,
                    x.ViolationName,
                    x.Description,
                    x.DatePlanRemoval,
                    x.InspDatePlanRemoval,
                    x.PpRf170,
                    x.PpRf25,
                    x.PpRf307,
                    x.PpRf491,
                    x.OtherNormativeDocs,
                    Action = actionsDict.ContainsKey(x.ViolationId) ? actionsDict[x.ViolationId] : string.Empty
                })
                .Distinct()
                .ToList();

                var strCodePin = new StringBuilder();

                var i = 0;
                foreach (var viol in violations)
                {
                    section.ДобавитьСтроку();
                    section["Номер1"] = ++i;
                    section["Пункт"] = viol.ViolationCodePin;
                    section["ТекстНарушения"] = viol.ViolationName;
                    section["Мероприятие"] = viol.Action;
                    section["Мероприятия"] = viol.Action;
                    section["Подробнее"] = viol.Description;
                    var strDateRemoval = viol.DatePlanRemoval ?? viol.InspDatePlanRemoval;

                    section["СрокУстранения"] = strDateRemoval;
                    section["СрокиИсполнения"] = strDateRemoval;
                    section["СрокИсполненияПредписания"] = strDateRemoval;

                    section["ПП_РФ_170"] = viol.PpRf170;
                    section["ПП_РФ_25"] = viol.PpRf25;
                    section["ПП_РФ_307"] = viol.PpRf307;
                    section["ПП_РФ_491"] = viol.PpRf491;
                    section["Прочие_норм_док"] = viol.OtherNormativeDocs;

                    if (strCodePin.Length > 0)
                    {
                        strCodePin.Append(", ");
                    }

                    strCodePin.Append(viol.ViolationCodePin.Replace("ПиН ", string.Empty));
                }

                reportParams.SimpleReportParams["ПиН"] = strCodePin;
            }
        }
    }
}