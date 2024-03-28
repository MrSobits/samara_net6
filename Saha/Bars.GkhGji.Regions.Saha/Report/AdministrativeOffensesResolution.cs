namespace Bars.GkhGji.Regions.Saha.Report
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    
    using B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.GkhGji.Entities;

    using Castle.Windsor;

    public class AdministrativeOffensesResolution : BasePrintForm
    {
        public IWindsorContainer Container { get; set; }

        private DateTime dateStart = DateTime.MinValue;
        private DateTime dateEnd = DateTime.MaxValue;
        private long[] municipalityIds;


        public AdministrativeOffensesResolution()
            : base(new ReportTemplateBinary(Properties.Resources.AdministrativeOffensesResolution))
        {
        }
        
        public override string Name
        {
            get { return "Журнал регистрации постановлений по делам об административных правонарушениях"; }
        }

        public override string Desciption
        {
            get { return "Журнал регистрации постановлений по делам об административных правонарушениях"; }
        }

        public override string GroupName
        {
            get { return "Инспекторская деятельность"; }
        }

        public override string ParamsController
        {
            get { return "B4.controller.report.AdministrativeOffensesResolution"; }
        }

        public override string RequiredPermission
        {
            get
            {
                return "GkhGji.Report.AdministrativeOffensesResolution";
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
            var resolutionData = Container.Resolve<IDomainService<Resolution>>().GetAll()
                .WhereIf(municipalityIds.Length > 0, x => municipalityIds.Contains(x.Municipality.Id))
                .WhereIf(dateStart != DateTime.MinValue, x => x.DocumentDate >= dateStart)
                .WhereIf(dateEnd != DateTime.MinValue, x => x.DocumentDate <= dateEnd)
                .Select(x => new
                {
                    InspId = x.Inspection.Id,
                    MuName = x.Contragent.Municipality.Name,
                    x.DocumentNumber,
                    x.DocumentDate,
                    x.Official.Fio,
                    ExecutantCode = x.Executant.Code,
                    SanctionCode = x.Sanction.Code,
                    SanctionName = x.Sanction.Name,
                    x.PenaltyAmount,
                    ContragentName = x.Contragent.Name,
                    JurAddress = x.Contragent.JuridicalAddress,
                    x.Contragent.AddressOutsideSubject,
                    x.PhysicalPerson

                })
                .OrderBy(x => x.MuName)
                .ToList();
            
            var inspectionGjiViolWording = Container.Resolve<IDomainService<InspectionGjiViol>>().GetAll()
                .Select(x => new
                {
                    x.RealityObject.Address,
                    x.Inspection.Id
                })
                .ToList();

            var section = reportParams.ComplexReportParams.ДобавитьСекцию("section");

            var num = 1;

            var firstExecutantCodeList = new List<string> { "0", "9", "11", "8", "15", "18", "4", "2" };
            var secondExecutantCodeList = new List<string> { "1", "3", "10", "12", "13", "16", "19", "5" };
            var thirdExecutantCodeList = new List<string> { "6", "7", "14" };
            
            foreach (var resolution in resolutionData)
            {
                section.ДобавитьСтроку();

                section["Num"] = num++;
                section["MU"] = resolution.MuName;
                section["RegNum"] = resolution.DocumentNumber;
                section["DateCreate"] = resolution.DocumentDate;
                section["InspFio"] = resolution.Fio;

                var executantCode = resolution.ExecutantCode;
                var address = resolution.JurAddress.IsEmpty() ? resolution.AddressOutsideSubject : resolution.JurAddress;
                if (firstExecutantCodeList.Contains(executantCode))
                {
                    section["Man"] = resolution.ContragentName + ", " + address;  
                }
                if (secondExecutantCodeList.Contains(executantCode))
                {
                    section["Man"] = resolution.ContragentName + ", " + resolution.PhysicalPerson;
                }
                if (thirdExecutantCodeList.Contains(executantCode))
                {
                    section["Man"] = resolution.PhysicalPerson;
                }

                var inspectionId = resolution.InspId;
                var addressViol = inspectionGjiViolWording.Where(x => x.Id == inspectionId).Select(x => x.Address).FirstOrDefault();
                section["Address"] = addressViol;
                
                var sanctionCode = resolution.SanctionCode;

                var penalty = Math.Round(resolution.PenaltyAmount.ToDecimal(), 0);
                section["Result"] = sanctionCode == "1" ? resolution.SanctionName + ", " + penalty : string.Empty;    
                
                
            }

            reportParams.SimpleReportParams["DateStart"] = dateStart.ToDateTime().ToShortDateString();
            reportParams.SimpleReportParams["DateEnd"] = dateEnd.ToDateTime().ToShortDateString();
        }
    }
}
