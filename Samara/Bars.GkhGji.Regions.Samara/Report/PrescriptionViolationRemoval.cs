namespace Bars.GkhGji.Regions.Samara.Report
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    
    using B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;

    using Castle.Windsor;

    public class PrescriptionViolationRemoval : BasePrintForm
    {
        public IWindsorContainer Container { get; set; }

        private DateTime dateStart = DateTime.MinValue;
        private DateTime dateEnd = DateTime.MaxValue;
        private long municipalityId;

        public PrescriptionViolationRemoval()
            : base(new ReportTemplateBinary(Properties.Resources.PrescriptionViolationRemoval))
        {
        }

        public override string RequiredPermission
        {
            get
            {
                return "Reports.GJI.PrescriptionViolationRemoval";
            }
        }

        public override string Desciption
        {
            get
            {
                return "Устранение нарушений по предписаниям";
            }
        }

        public override string GroupName
        {
            get
            {
                return "Отчеты ГЖИ";
            }
        }

        public override string ParamsController
        {
            get
            {
                return "B4.controller.report.PrescriptionViolationRemoval";
            }
        }

        public override string Name
        {
            get
            {
                return "Устранение нарушений по предписаниям";
            }
        }

        public override void SetUserParams(BaseParams baseParams)
        {
            dateStart = baseParams.Params["dateStart"].ToDateTime();
            dateEnd = baseParams.Params["dateEnd"].ToDateTime();
            municipalityId = baseParams.Params.GetAs<long>("municipalityId", 0);
        }

        public override void PrepareReport(ReportParams reportParams)
        {
            var servicePrescriptionViol = Container.Resolve<IDomainService<PrescriptionViol>>().GetAll()
                .Where(x => x.InspectionViolation.RealityObject.Municipality.Id == municipalityId)
                .WhereIf(dateStart != DateTime.MinValue, x => x.Document.DocumentDate >= dateStart)
                .WhereIf(dateEnd != DateTime.MinValue, x => x.Document.DocumentDate <= dateEnd);

            var prescriptionAndViolationData = servicePrescriptionViol
                .Select(x => new
                {
                    DocumentId = x.Document.Id,
                    x.InspectionViolation.RealityObject.Address,
                    RealityObjectId = x.InspectionViolation.RealityObject.Id,
                    MunicipalityName = x.InspectionViolation.RealityObject.Municipality.Name,
                    x.Document.DocumentNumber,
                    x.Document.DocumentDate,
                    x.InspectionViolation.Violation.CodePin,
                    x.InspectionViolation.Violation.Name,
                    x.InspectionViolation.DatePlanRemoval,
                    x.InspectionViolation.DateFactRemoval
                })
                .AsEnumerable()
                .GroupBy(x => new
                {
                    x.DocumentId,
                    x.DocumentDate, 
                    x.DocumentNumber,
                    x.RealityObjectId, 
                    x.MunicipalityName,
                    x.Address
                })
                .Select(x => new
                {
                    PrescriptionData = x.Key, 
                    ViolationData = x.Select(y => new
                        {
                            y.CodePin,
                            y.Name,
                            y.DateFactRemoval,
                            y.DatePlanRemoval
                        })
                        .OrderBy(y => y.DatePlanRemoval)
                        .ToList()
                })
                .OrderBy(x => x.PrescriptionData.DocumentDate)
                .ThenBy(x => x.PrescriptionData.Address)
                .ToList();

            var prescriptionIdsQuery = servicePrescriptionViol.Select(x => x.Document.Id);

            var prescriptionActDict = Container.Resolve<IDomainService<DocumentGjiChildren>>().GetAll()
                .Where(x => prescriptionIdsQuery.Contains(x.Children.Id))
                .Where(x => x.Parent.TypeDocumentGji == TypeDocumentGji.ActCheck || x.Parent.TypeDocumentGji == TypeDocumentGji.ActRemoval )
                .Where(x => x.Children.TypeDocumentGji == TypeDocumentGji.Prescription)
                .Select(x => new { prescriptionId = x.Children.Id, actNumber = x.Parent.DocumentNumber })
                .AsEnumerable()
                .GroupBy(x => x.prescriptionId)
                .ToDictionary(x => x.Key, x => x.Select(y => y.actNumber).First());

            var prescriptionInspectorsDict = Container.Resolve<IDomainService<DocumentGjiInspector>>().GetAll()
                .Where(x => prescriptionIdsQuery.Contains(x.DocumentGji.Id))
                .Select(x => new { x.DocumentGji.Id, x.Inspector.Fio })
                .AsEnumerable()
                .GroupBy(x => x.Id)
                .ToDictionary(x => x.Key, x => string.Join(", ", x.Where(y => !string.IsNullOrEmpty(y.Fio)).Select(y => y.Fio)));

            var section = reportParams.ComplexReportParams.ДобавитьСекцию("section");

            Func<Dictionary<long, string>, long, string> getDictValue = (dict, key) => dict.ContainsKey(key) ? dict[key] : string.Empty;

            var i = 0;

            foreach (var data in prescriptionAndViolationData)
            {
                var actNumber = getDictValue(prescriptionActDict, data.PrescriptionData.DocumentId);
                var nextPresriptionRow = i;

                foreach (var violation in data.ViolationData)
                {
                    section.ДобавитьСтроку();

                    if (nextPresriptionRow == i)
                    {
                        section["Number"] = ++i;
                        section["DocumentNumber"] = data.PrescriptionData.DocumentNumber;
                        section["DocumentDate"] = data.PrescriptionData.DocumentDate;
                        section["MunicipalityName"] = data.PrescriptionData.MunicipalityName;
                        section["Address"] = data.PrescriptionData.Address;
                        section["Inspector"] = getDictValue(prescriptionInspectorsDict, data.PrescriptionData.DocumentId);
                    }

                    section["CodePin"] = violation.CodePin;
                    section["Description"] = violation.Name;
                    section["DatePlanRemoval"] = violation.DatePlanRemoval;
                    section["DateFactRemoval"] = violation.DateFactRemoval;
                    section["ActNumber"] = actNumber;
                }
            }
        }
    }
}