namespace Bars.GkhGji.Regions.Saha.Report
{
    using Bars.B4;
    
    using B4.Modules.Reports;
    using System;
    using System.Linq;
    using Bars.B4.Utils;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;

    using Castle.Windsor;

    public class ScheduledInspectionSurveysJournal : BasePrintForm
    {
        public IWindsorContainer Container { get; set; }

        public IDomainService<InspectionGjiRealityObject> InspectionGjiRealityObjectDomain { get; set; }
        public IDomainService<Disposal> DisposalDomain { get; set; }
        public IDomainService<ActCheckDefinition> ActCheckDefinitionDomain { get; set; }
        public IDomainService<ActCheck> ActCheckDomain { get; set; }
        public IDomainService<Protocol> ProtocolDomain { get; set; }
        public IDomainService<DocumentGjiInspector> DocumentGjiInspectorDomain { get; set; }
        
        private DateTime dateStart = DateTime.MinValue;
        private DateTime dateEnd = DateTime.MaxValue;
        private long[] municipalityIds;

        public ScheduledInspectionSurveysJournal()
            : base(new ReportTemplateBinary(Properties.Resources.ScheduledInspectionSurveysJournal))
        {
        }

        public override string Name
        {
            get { return "Журнал учета плановых и внеплановых проверок, инспекционных обследований"; }
        }

        public override string Desciption
        {
            get { return "Журнал учета плановых и внеплановых проверок, инспекционных обследований"; }
        }

        public override string GroupName
        {
            get { return "Инспекторская деятельность"; }
        }

        public override string ParamsController
        {
            get { return "B4.controller.report.ScheduledInspectionSurveysJournal"; }
        }

        public override string RequiredPermission
        {
            get
            {
                return "GkhGji.Report.ScheduledInspectionSurveysJournal";
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
           var inspectionGji = InspectionGjiRealityObjectDomain.GetAll()
               .WhereIf(municipalityIds.Length > 0, x => municipalityIds.Contains(x.RealityObject.Municipality.Id))
               .Select(x => new
               {
                   x.Inspection.Id,
                   MuName = x.RealityObject.Municipality.Name,
                   x.RealityObject.Address,
                   RealObjId = x.RealityObject.Id
               })
               .AsEnumerable()
               .OrderBy(x => x.MuName)
               .GroupBy(x => x.Id)
               .ToDictionary(x => x.Key, y => y.ToList());
            
            var inspectionGjiIds = inspectionGji.Select(x => x.Key).ToList();

            var disposalData = DisposalDomain.GetAll()
                .Where(x => inspectionGjiIds.Contains(x.Inspection.Id))
                .WhereIf(dateStart != DateTime.MinValue, x => x.DocumentDate >= dateStart)
                .WhereIf(dateEnd != DateTime.MinValue, x => x.DocumentDate <= dateEnd)
                .Select(x => new
                {
                    DispId = x.Id,
                    InspId = x.Inspection.Id,
                    x.DocumentNumber,
                    x.DocumentDate,
                    KindCheckName = x.KindCheck.Name,
                    x.DateStart,
                    x.DateEnd,
                    x.Inspection.PersonInspection,
                    ContragentName = x.Inspection.Contragent.Name,
                    x.Inspection.PhysicalPerson
                })
                .ToList();

            var inspIds = disposalData.Select(x => x.InspId).ToList();

            var actCheckData = ActCheckDomain.GetAll()
                .Where(x => inspIds.Contains(x.Inspection.Id))
                .Select(x => new
                {
                    ActCheckId = x.Id,
                    x.Inspection.Id,
                    x.DocumentDate,
                    x.DocumentNumber
                })
                .ToList();

            var actCheckIds = actCheckData.Select(x => x.ActCheckId).ToList();

            var actCheckDefinitionData = ActCheckDefinitionDomain.GetAll()
                .Where(x => actCheckIds.Contains(x.ActCheck.Id))
                .Select(x => new
                {
                    x.ActCheck.Inspection.Id,
                    x.DocumentNum,
                    x.DocumentDate
                })
                .ToList();

            var protocolData = ProtocolDomain.GetAll()
            .Where(x => inspectionGjiIds.Contains(x.Inspection.Id))
            .Select(x => new
            {
                x.Inspection.Id,
                x.DocumentDate,
                x.DocumentNumber
            })
            .ToList();

            var section = reportParams.ComplexReportParams.ДобавитьСекцию("section");
            var num = 1;

            var documentGjiInspector = DocumentGjiInspectorDomain.GetAll()
                .Where(x => inspIds.Contains(x.DocumentGji.Inspection.Id))
                .Select(x => new
                {
                    x.DocumentGji.Id,
                    x.Inspector.Fio
                })
                .ToList();

            foreach (var inspection in inspectionGji)
            {
                foreach (var disposal in disposalData.Where(x => x.InspId == inspection.Key))
                {
                    section.ДобавитьСтроку();

                    var inspectionId = disposal.InspId;
                    var disposalId = disposal.DispId;
                    section["Num"] = num++;
                    
                    var muName = inspectionGji.Where(x => x.Key == inspectionId)
                        .Select(y => y.Value.Select(x => x.MuName).FirstOrDefault())
                        .FirstOrDefault();
                    
                    section["MU"] = muName;
                    section["DispNum"] = disposal.DocumentNumber;
                    section["DispDate"] = disposal.DocumentDate;
                    section["SurveyType"] = disposal.KindCheckName;
                    section["Term"] = disposal.DateStart.ToDateTime().ToShortDateString() + " - " + disposal.DateEnd.ToDateTime().ToShortDateString();

                    var personInspectionType = disposal.PersonInspection;
                    if (personInspectionType == PersonInspection.PhysPerson)
                    {
                        section["Man"] = disposal.PhysicalPerson;
                    }
                    else
                    {
                        section["Man"] = disposal.ContragentName;
                    }

                    var fullAddressData = inspectionGji.Where(x => x.Key == inspectionId).ToList();
                    
                    var fullAddress = string.Empty;

                    foreach (var addressData in fullAddressData)
                    {
                        foreach (var address in addressData.Value)
                        {
                            var fullAddressByDisp = address.MuName + ", " + address.Address;
                            fullAddress += fullAddressByDisp + "; ";
                        }

                    }
                    section["Address"] = fullAddress == "" ? string.Empty : fullAddress.Remove(fullAddress.Length - 2, 1);
                    
                    var documentGjiInspectorByInspection = documentGjiInspector.Where(x => x.Id == disposalId).Select(x => x.Fio).ToList();
                    section["FIO"] = documentGjiInspectorByInspection.Aggregate(string.Empty, (x, y) => x + (!string.IsNullOrEmpty(x) ? ", " + y : y));

                    var actCheck = actCheckData.FirstOrDefault(x => x.Id == inspectionId);
                    if (actCheck != null)
                    {
                        if (actCheck.DocumentDate == null)
                        {
                            section["ActDate"] = actCheck.DocumentNumber;
                        }
                        else
                        {
                            section["ActDate"] = actCheck.DocumentDate.ToDateTime().ToShortDateString() + ", " + actCheck.DocumentNumber;
                        }

                    }

                    var actCheckDefinitionList = actCheckDefinitionData.Where(x => x.Id == inspectionId).ToList();
                    var protocolList = protocolData.Where(x => x.Id == inspectionId).ToList();

                    var fullDefinitionData = string.Empty;
                    var fullProtocolData = string.Empty;

                    if (actCheckDefinitionList.Count != 0)
                    {
                        foreach (var actCheckDefinition in actCheckDefinitionList)
                        {
                            var difinitionString = actCheckDefinition.DocumentDate == null ?
                                string.Empty + actCheckDefinition.DocumentNum :
                                actCheckDefinition.DocumentDate.ToDateTime().ToShortDateString() + "," + actCheckDefinition.DocumentNum;

                            if (difinitionString != "")
                                fullDefinitionData += difinitionString + "; ";
                        }
                        section["Result"] = fullDefinitionData == "" ? string.Empty : fullDefinitionData.Remove(fullDefinitionData.Length - 2, 1);
                        
                    }
                    else
                    {
                        foreach (var protocol in protocolList)
                        {

                            var protocolString = protocol.DocumentDate == null ?
                                string.Empty + protocol.DocumentNumber :
                                protocol.DocumentDate.ToDateTime().ToShortDateString() + "," + protocol.DocumentNumber;

                            if (protocolString != "")
                                fullProtocolData += protocolString + "; ";
                        }
                        section["Result"] = fullProtocolData == "" ? string.Empty : fullProtocolData.Remove(fullProtocolData.Length - 2, 1);
                        
                    }
                }
            }

            reportParams.SimpleReportParams["DateStart"] = dateStart.ToDateTime().ToShortDateString();
            reportParams.SimpleReportParams["DateEnd"] = dateEnd.ToDateTime().ToShortDateString();
            
        }
    }
}
