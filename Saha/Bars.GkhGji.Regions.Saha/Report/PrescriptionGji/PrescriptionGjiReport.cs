namespace Bars.GkhGji.Regions.Saha.Report.PrescriptionGji
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using B4.Modules.Reports;
    using Bars.Gkh.Report;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Regions.Saha.Entities;

    public class PrescriptionGjiReport : GkhGji.Report.PrescriptionGjiReport
    {
        public override bool PrintingAllowed
        {
            get
            {
                return false;
            }
        }
        
        public override List<TemplateInfo> GetTemplateInfo()
        {
            return new List<TemplateInfo>()
                       {
                           new TemplateInfo()
                               {
                                   Name = "PrescriptionGJI",
                                   Code = "BlockGJI_ExecutiveDocPrescription_5001",
                                   Description = "Типы обследования все кроме 5014, 5016",
                                   Template = Properties.Resources.BlockGJI_ExecutiveDocPrescription_5001
                               },
                               new TemplateInfo()
                               {
                                   Name = "PrescriptionGJI",
                                   Code = "BlockGJI_ExecutiveDocPrescription_5003",
                                   Description = "Типы обследования  5014, 5016",
                                   Template = Properties.Resources.BlockGJI_ExecutiveDocPrescription_5003
                               }
                       };
        }

        protected override void GetTemplateCode(List<TypeSurveyGji> typeSurveys)
        {
            CodeTemplate = "BlockGJI_ExecutiveDocPrescription_5001";

            if (typeSurveys.Any(x => x.Code == "5014" || x.Code == "5016"))
            {
                CodeTemplate = "BlockGJI_ExecutiveDocPrescription_5003";
            }
        }

        protected override void FillRegionParams(ReportParams reportParams, DocumentGji doc)
        {
            var prescription = (Prescription)doc;
            var sectionHH = reportParams.ComplexReportParams.ДобавитьСекцию("СекцияНарушенийНН");
            var sectionHH1 = reportParams.ComplexReportParams.ДобавитьСекцию("СекцияНарушенийНН1");

            var servicePrescriptionViol = Container.Resolve<IDomainService<PrescriptionViol>>().GetAll();
            var queryViolations = servicePrescriptionViol.Where(x => x.Document.Id == prescription.Id);
            var violations = queryViolations
                .Select(x => new
                                {
                                    inspViolId = x.InspectionViolation.Id,
                                    x.Action,
                                    datePlanremoval = x.InspectionViolation.DatePlanRemoval,
                                    codePin = x.InspectionViolation.Violation.CodePin
                                })
                .ToList();

            var queryActInspViol = queryViolations.Select(x => x.InspectionViolation.Id);
            var dictInspViolWording = Container.Resolve<IDomainService<InspectionGjiViolWording>>().GetAll()
                                                .Where(x => queryActInspViol.Contains(x.InspectionViolation.Id))
                                                .Select(x => new { x.Wording, inspViolsId = x.InspectionViolation.Id })
                                                .ToArray()
                                                .GroupBy(x => x.inspViolsId)
                                                .ToDictionary(x => x.Key, v => string.Join("; ", v.Select(y => y.Wording)));

            var i = 1;
            foreach (var item in violations)
            {
                sectionHH.ДобавитьСтроку();
                sectionHH1.ДобавитьСтроку();

                sectionHH["Номер1"] = i++;
                sectionHH["ФормулировкаНарушения"] = dictInspViolWording.ContainsKey(item.inspViolId)
                                  ? dictInspViolWording[item.inspViolId]
                                  : string.Empty;
                sectionHH["Мероприятия"] = item.Action;
                var strDateRemoval = item.datePlanremoval.HasValue
                                                ? item.datePlanremoval.Value.ToShortDateString()
                                                : string.Empty;
                sectionHH["СрокУстранения"] = strDateRemoval;

                sectionHH1["Номер1"] = i++;
                sectionHH1["ФормулировкаНарушения"] = dictInspViolWording.ContainsKey(item.inspViolId)
                                  ? dictInspViolWording[item.inspViolId]
                                  : string.Empty;
                sectionHH1["Пункт"] = item.codePin;
            }

            reportParams.SimpleReportParams["ФормулировкаНарушения"] = string.Join("; ", dictInspViolWording.Values);
        }
    }
}