namespace Bars.GkhGji.Regions.Saha.Report
{
    using System;
    using System.Linq;
    using Bars.B4;
    
    using B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.GkhGji.Entities;

    using Castle.Windsor;

    public class AppealCitsJurnalAccounting : BasePrintForm
    {
        public IWindsorContainer Container { get; set; }

        private DateTime dateStart = DateTime.MinValue;
        private DateTime dateEnd = DateTime.MaxValue;
        private long[] municipalityIds;

        public AppealCitsJurnalAccounting()
            : base(new ReportTemplateBinary(Properties.Resources.AppealCitsJurnalAccounting))
        {
        }
        
        public override string Name
        {
            get { return "Журнал учета обращений"; }
        }

        public override string Desciption
        {
            get { return "Журнал учета обращений"; }
        }

        public override string GroupName
        {
            get { return "Обращения ГЖИ"; }
        }

        public override string ParamsController
        {
            get { return "B4.controller.report.AppealCitsJurnalAccounting"; }
        }

        public override string RequiredPermission
        {
            get
            {
                return "GkhGji.Report.AppealCitsJurnalAccounting";
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
            var appealCitsRealityObjectIds = Container.Resolve<IDomainService<AppealCitsRealityObject>>().GetAll()
                .WhereIf(municipalityIds.Length > 0, x => municipalityIds.Contains(x.RealityObject.Municipality.Id))
                .WhereIf(dateStart != DateTime.MinValue, x => x.AppealCits.DateFrom >= dateStart)
                .WhereIf(dateEnd != DateTime.MinValue, x => x.AppealCits.DateFrom <= dateEnd)
                .Select(x => new
                {
                    x.AppealCits.DateFrom,
                    DocNum = x.AppealCits.DocumentNumber,
                    MuName = x.RealityObject.Municipality.Name,
                    x.AppealCits.CorrespondentAddress,
                    x.AppealCits.Correspondent,
                    x.AppealCits.Description,
                    SuretyFio = x.AppealCits.Surety.Fio,
                    ResolveName = x.AppealCits.SuretyResolve.Name,
                    ExecunantFio = x.AppealCits.Executant.Fio
                })
                .AsEnumerable()
                .Distinct(x => x.DocNum)
                .OrderBy(x => x.MuName)
                .ToList();
            
            var section = reportParams.ComplexReportParams.ДобавитьСекцию("section");
            var num = 1;

            foreach (var appeal in appealCitsRealityObjectIds)
            {
                section.ДобавитьСтроку();

                section["Num"] = num++;
                section["DateIn"] = appeal.DateFrom;
                section["RegNum"] = appeal.DocNum;
                section["MU"] = appeal.MuName;
                section["Address"] = appeal.CorrespondentAddress;
                section["Corespondent"] = appeal.Correspondent;
                section["Summary"] = appeal.Description;
                section["Surety"] = appeal.SuretyFio;
                section["Resol"] = appeal.ResolveName;
                section["Executor"] = appeal.ExecunantFio;
            }

            reportParams.SimpleReportParams["DateStart"] = dateStart.ToShortDateString();
            reportParams.SimpleReportParams["DateEnd"] = dateEnd.ToShortDateString();
            reportParams.SimpleReportParams["CurrentDate"] = DateTime.Today.ToShortDateString();
        }
    }
}
