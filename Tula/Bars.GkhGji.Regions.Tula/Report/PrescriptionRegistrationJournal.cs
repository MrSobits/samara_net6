namespace Bars.GkhGji.Regions.Tula.Report
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Bars.B4;
    
    using Bars.B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.GkhGji.Entities;

    using Castle.Windsor;

    public class PrescriptionRegistrationJournal : BasePrintForm
    {
        public IWindsorContainer Container { get; set; }

        private DateTime dateStart = DateTime.MinValue;
        private DateTime dateEnd = DateTime.MaxValue;
        private long[] municipalityIds;

        public IDomainService<Prescription> PrescriptionDomain { get; set; }
        public IDomainService<InspectionGjiViolStage> InspectionGjiViolStageDomain { get; set; }

        public PrescriptionRegistrationJournal()
            : base(new ReportTemplateBinary(Properties.Resources.PrescriptionRegistrationJournal))
        {
        }

        public override string Name
        {
            get { return "Журнал регистрации предписаний"; }
        }

        public override string Desciption
        {
            get { return "Журнал регистрации предписаний"; }
        }

        public override string GroupName
        {
            get { return "Инспекторская деятельность"; }
        }

        public override string ParamsController
        {
            get { return "B4.controller.report.PrescriptionRegistrationJournal"; }
        }

        public override string RequiredPermission
        {
            get
            {
                return "GkhGji.Report.PrescriptionRegistrationJournal";
            }
        }

        public override void SetUserParams(BaseParams baseParams)
        {
            dateStart = baseParams.Params["dateStart"].ToDateTime();
            dateEnd = baseParams.Params["dateEnd"].ToDateTime();
            var municipalityIdsList = baseParams.Params.GetAs("municipalityIds", string.Empty);

            municipalityIds = !string.IsNullOrEmpty(municipalityIdsList) ? municipalityIdsList.Split(',').Select(id => id.ToLong()).ToArray() : new long[0];
        }

        public override string ReportGenerator { get; set; }

        public override void PrepareReport(ReportParams reportParams)
        {
            var prescriptionData = PrescriptionDomain.GetAll()
                .WhereIf(municipalityIds.Length > 0, x => municipalityIds.Contains(x.Contragent.Municipality.Id))
                .WhereIf(dateStart != DateTime.MinValue, x => x.DocumentDate >= dateStart)
                .WhereIf(dateEnd != DateTime.MinValue, x => x.DocumentDate <= dateEnd)
                .Select(x => new
                {
                    x.Id,
                    InspId = x.Inspection.Id,
                    MuName = x.Contragent.Municipality.Name,
                    x.DocumentNumber,
                    x.DocumentDate,
                    x.Executant.Code,
                    x.Executant.Name,
                    ContragentName = x.Contragent.Name,
                    x.Contragent.JuridicalAddress,
                    x.Contragent.AddressOutsideSubject,
                    x.PhysicalPerson
                })
                .OrderBy(x => x.MuName)
                .ToList();

            var inspIds = prescriptionData.Select(x => x.InspId).ToList();

            var inspectionGjiViolWording = InspectionGjiViolStageDomain.GetAll()
                .Where(x => inspIds.Contains(x.InspectionViolation.Inspection.Id))
                .Select(x => new
                {
                    DocId = x.Document.Id,
                    x.InspectionViolation.RealityObject.Address,
                    x.InspectionViolation.Violation.CodePin,
                    x.DatePlanRemoval
                })
                .ToList();

            var firstExecutantCodeList = new List<string> { "0", "9", "11", "8", "15", "18", "4", "2" };
            var secondExecutantCodeList = new List<string> { "1", "3", "10", "12", "13", "16", "19", "5" };
            var thirdExecutantCodeList = new List<string> { "6", "7", "14" };

            var section = reportParams.ComplexReportParams.ДобавитьСекцию("section");
            var num = 1;
            
            foreach (var prescription in prescriptionData)
            {
                section.ДобавитьСтроку();

                var perscriptionId = prescription.Id;

                section["Num"] = num++;
                section["MU"] = prescription.MuName;
                section["RegNum"] = prescription.DocumentNumber;
                section["DataCreate"] = prescription.DocumentDate;

                var violationAddress = inspectionGjiViolWording.Where(x => x.DocId == perscriptionId).Select(x => x.Address).FirstOrDefault();

                section["Address"] = violationAddress;
                
                var executantCode = prescription.Code;
                var address = prescription.JuridicalAddress.IsEmpty() ? prescription.AddressOutsideSubject : prescription.JuridicalAddress;
                if (firstExecutantCodeList.Contains(executantCode))
                {
                    section["WhoIssued"] = prescription.ContragentName + ", " + address;
                }
                if (secondExecutantCodeList.Contains(executantCode))
                {
                    section["WhoIssued"] = prescription.ContragentName + ", " + prescription.PhysicalPerson;
                }
                if (thirdExecutantCodeList.Contains(executantCode))
                {
                    section["WhoIssued"] = prescription.PhysicalPerson;
                }

                var violationCodePin = inspectionGjiViolWording.Where(x => x.DocId == perscriptionId).Select(x => x.CodePin).ToList();
                section["Viol"] = violationCodePin.Aggregate(string.Empty, (x, y) => x + (!string.IsNullOrEmpty(x) ? ", " + y : y));
                
                var term = inspectionGjiViolWording.Where(x => x.DocId == perscriptionId).Select(x => x.DatePlanRemoval).ToList();

                var fullDate = string.Empty;
                if (term.Count > 1)
                {
                    var dateViol = term.Select(x => x != null ? x.Value.ToDateTime().ToShortDateString() : string.Empty).ToList();
                    var dateViolDis = dateViol.Distinct();
                    foreach (var date in dateViolDis.Where(x => x != ""))
                    {
                        fullDate += date + ", ";
                    }
                    section["Term"] = fullDate == "" ? string.Empty : fullDate.Remove(fullDate.Length - 2, 1);
                }
                else
                {
                    var date = term.Select(x => x != null ? x.Value.ToDateTime().ToShortDateString() : string.Empty);
                    section["Term"] = date.FirstOrDefault();    
                }
                
            }
            
            reportParams.SimpleReportParams["DateStart"] = dateStart.ToDateTime().ToShortDateString();
            reportParams.SimpleReportParams["DateEnd"] = dateEnd.ToDateTime().ToShortDateString();
        }

    }
}
