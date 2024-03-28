namespace Bars.GkhGji.Regions.Zabaykalye.Report
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Bars.B4;
    
    using Bars.B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.GkhGji.Entities;
    using Castle.Windsor;

    public class AdministrativeOffensesJurnalReport : BasePrintForm
    {
        public IWindsorContainer Container { get; set; }

        private DateTime dateStart = DateTime.MinValue;
        private DateTime dateEnd = DateTime.MaxValue;
        private long[] municipalityIds;

        public IDomainService<Protocol> ProtocolDomain { get; set; }
        public IDomainService<ProtocolArticleLaw> ProtocolArticleLawDomain { get; set; }
        public IDomainService<DocumentGjiInspector> DocumentGjiInspectorDomain { get; set; }
        public IDomainService<InspectionGjiViolStage> InspectionGjiViolStageDomain { get; set; }

        public AdministrativeOffensesJurnalReport()
            : base(new ReportTemplateBinary(Properties.Resources.AdministrativeOffensesJurnalReport))
        {
        }
        
        public override string Name
        {
            get { return "Журнал регистрации протоколов об административных правонарушениях"; }
        }

        public override string Desciption
        {
            get { return "Журнал регистрации протоколов об административных правонарушениях"; }
        }

        public override string GroupName
        {
            get { return "Инспекторская деятельность"; }
        }

        public override string ParamsController
        {
            get { return "B4.controller.report.AdministrativeOffensesJurnalReport"; }
        }

        public override string RequiredPermission
        {
            get
            {
                return "GkhGji.Report.AdministrativeOffensesJurnalReport";
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
            var data = ProtocolDomain.GetAll()
                .WhereIf(municipalityIds.Length > 0, x => municipalityIds.Contains(x.Contragent.Municipality.Id))
                .WhereIf(dateStart != DateTime.MinValue, x => x.DocumentDate >= dateStart)
                .WhereIf(dateEnd != DateTime.MinValue, x => x.DocumentDate <= dateEnd)
                .Select(x => new
                {
                    inspId = x.Inspection.Id,
                    protocolId = x.Id,
                    MuName = x.Contragent.Municipality.Name,
                    x.DocumentNumber,
                    x.DocumentDate,
                    ContagentName = x.Contragent.Name,
                    x.Contragent.JuridicalAddress,
                    x.Contragent.AddressOutsideSubject,
                    x.Executant.Code,
                    x.PhysicalPerson
                })
                .OrderBy(x => x.MuName)
                .ToList();

            var protocolIds = data.Select(x => x.protocolId).ToList();

            var protocolArticleLaw = ProtocolArticleLawDomain.GetAll()
                .Where(x => protocolIds.Contains(x.Protocol.Id))
                .Select(x => new
                {
                    x.Protocol.Id,
                    x.ArticleLaw.Name
                })
                .ToList();

            var documentGjiInspector = DocumentGjiInspectorDomain.GetAll()
                .Where(x => protocolIds.Contains(x.DocumentGji.Id))
                .Select(x => new
                {
                    x.DocumentGji.Id,
                    x.Inspector.Fio
                })
                .ToList();

            var inspectionGjiViolStage = InspectionGjiViolStageDomain.GetAll()
                .Select(x => new
                {
                    DocId = x.Document.Id,
                    x.InspectionViolation.RealityObject.Address,
                    x.InspectionViolation.Violation.CodePin
                })
                .ToList();
            
            var firstExecutantCodeList = new List<string> { "0", "9", "11", "8", "15", "18", "4", "2" };
            var secondExecutantCodeList = new List<string> { "1", "3", "10", "12", "13", "16", "19", "5" };
            var thirdExecutantCodeList = new List<string> { "6", "7", "14" };

            var section = reportParams.ComplexReportParams.ДобавитьСекцию("section");
            
            var num = 1;

            foreach (var protocol in data)
            {
                section.ДобавитьСтроку();

                var protocolId = protocol.protocolId;
                var executCod = protocol.Code;

                section["Num"] = num++;
                section["MU"] = protocol.MuName;
                section["ProtNum"] = protocol.DocumentNumber;
                section["CreateDate"] = protocol.DocumentDate;

                var documentGjiInspectorData = documentGjiInspector.Where(x => x.Id == protocolId).Select(x => x.Fio).ToList();
                if (documentGjiInspectorData.Count > 1)
                {
                    section["FIO"] = documentGjiInspectorData.Aggregate(string.Empty, (x, y) => x + (!string.IsNullOrEmpty(x) ? ", " + y : y)); 
                }
                else
                {
                    section["FIO"] = documentGjiInspectorData.FirstOrDefault(); 
                }
                
                if (firstExecutantCodeList.Contains(executCod))
                {
                    var address = protocol.JuridicalAddress.IsEmpty() ? protocol.AddressOutsideSubject : protocol.JuridicalAddress;
                    section["Contragent"] = protocol.ContagentName + ", " + address;
                }
                if (secondExecutantCodeList.Contains(executCod))
                {
                    section["Contragent"] = protocol.ContagentName + ", " + protocol.PhysicalPerson;
                }
                if (thirdExecutantCodeList.Contains(executCod))
                {
                    section["Contragent"] = protocol.PhysicalPerson;
                }

                var addressViol = inspectionGjiViolStage.Where(x => x.DocId == protocolId).Select(x => x.Address).FirstOrDefault();
                section["Address"] = addressViol;

                var viol = inspectionGjiViolStage.Where(x => x.DocId == protocolId).Select(x => x.CodePin).ToList();
                if (viol.Count > 1)
                {
                    section["Violation"] = viol.Aggregate(string.Empty, (x, y) => x + (!string.IsNullOrEmpty(x) ? ", " + y : y));
                }
                else
                {
                    section["Violation"] = viol.FirstOrDefault();
                }
                
                var articleLawData = protocolArticleLaw.Where(x => x.Id == protocolId).Select(x => x.Name).ToList();
                if (articleLawData.Count > 1)
                {
                    section["Article"] = articleLawData.Aggregate(string.Empty, (x, y) => x + (!string.IsNullOrEmpty(x) ? ", " + y : y));
                }
                else
                {
                    section["Article"] = articleLawData.FirstOrDefault();
                }
                
            }

            reportParams.SimpleReportParams["DateStart"] = dateStart.ToDateTime().ToShortDateString();
            reportParams.SimpleReportParams["DateEnd"] = dateEnd.ToDateTime().ToShortDateString();
        }
    }
}
