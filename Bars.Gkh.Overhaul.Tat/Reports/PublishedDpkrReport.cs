namespace Bars.Gkh.Overhaul.Tat.Reports
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;    
    using Bars.B4.Modules.Reports;
    using Bars.B4.Utils;
    using Castle.Windsor;

    public class PublishedDpkrReport : BasePrintForm
    {
        public PublishedDpkrReport()
            : base(new ReportTemplateBinary(Properties.Resources.PublishedDpkr))
        {

        }

        public IWindsorContainer Container { get; set; }

        public override string Name
        {
            get
            {
                return "Отчет по опубликованию ДПКР";
            }
        }

        public override string Desciption
        {
            get
            {
                return "Отчет по опубликованию ДПКР";
            }
        }

        public override string GroupName
        {
            get
            {
                return "Долгосрочная программа";
            }
        }

        public override string ParamsController
        {
            get
            {
                return "B4.controller.report.PublishedDpkr";
            }
        }

        public override string RequiredPermission
        {
            get
            {
                return "Ovrhl.PublishedDpkr";
            }
        }

        private List<long> municipalityIds;

        public override void SetUserParams(BaseParams baseParams)
        {
            var strMunicpalIds = baseParams.Params.GetAs("municipalityIds", string.Empty);

            municipalityIds = !string.IsNullOrEmpty(strMunicpalIds)
                ? strMunicpalIds.Split(',').Select(x => x.ToLong()).ToList()
                : new List<long>();
        }

        public override string ReportGenerator { get; set; }

        public override void PrepareReport(ReportParams reportParams)
        {
            //ToDo Этот отчет вообещ в татарстане ненужен пока код закомментировал
            /*
            var records =
                Container.Resolve<IDomainService<DpkrCorrectionStage2>>().GetAll()
                    .Where(x => x.Stage2.Stage3Version.ProgramVersion.IsMain)
                    .WhereIf(municipalityIds.Count > 0, x => municipalityIds.Contains(x.RealityObject.Municipality.Id))
                    .Select(x => new
                    {
                        CeoId = x.Stage2.CommonEstateObject.Id,
                        CeoName = x.Stage2.CommonEstateObject.Name,
                        MuName = x.RealityObject.Municipality.Name,
                        x.RealityObject.Address,
                        x.RealityObject.DateCommissioning,
                        x.Stage2.Stage3Version.StoredCriteria,
                        x.RealityObject.DateLastOverhaul,
                        x.PublicationYear
                    })
                    .OrderBy(x => x.MuName)
                    .ThenBy(x => x.Address)
                    .ThenBy(x => x.PublicationYear)
                    .ToArray();

            if(!records.Any())
                return;

            //словарь идентификатор оои - строка наименований работ
            var dictCeoJob =
                Container.Resolve<IDomainService<StructuralElementWork>>().GetAll()
                    .GroupBy(x => x.StructuralElement.Group.CommonEstateObject.Id)
                    .ToDictionary(x => x.Key,
                        y => y.Select(x => x.Job.Name)
                            .AsEnumerable()
                            .Aggregate((result, add) =>
                            {
                                if (!string.IsNullOrWhiteSpace(add))
                                {
                                    if (!string.IsNullOrWhiteSpace(result))
                                    {
                                        result += ", ";
                                    }

                                    result += add;
                                }

                                return result;
                            }));

            var dpkrParamsService = Container.Resolve<IDpkrParamsService>();

            var parameters = dpkrParamsService.GetParams();

            var minYear = parameters.ContainsKey("ProgrammPeriodStart") ? parameters["ProgrammPeriodStart"].ToInt() : 0;
            var maxYear = parameters.ContainsKey("ProgrammPeriodEnd") ? parameters["ProgrammPeriodEnd"].ToInt() : 0;


            reportParams.SimpleReportParams["period"] = string.Format("{0}-{1}", minYear, maxYear) ;

            var sect = reportParams.ComplexReportParams.ДобавитьСекцию("Section");
            var i = 1;

            foreach (var stage2 in records)
            {
                var works = dictCeoJob.ContainsKey(stage2.CeoId)
                    ? dictCeoJob[stage2.CeoId]
                    : null;

                sect.ДобавитьСтроку();

                sect["row"] = i++;
                sect["Mu"] = stage2.MuName;
                sect["Address"] = stage2.Address;
                sect["StartDate"] = stage2.DateCommissioning.HasValue
                    ? stage2.DateCommissioning.Value.ToShortDateString()
                    : null;

                sect["OOI"] = stage2.CeoName;
                sect["Wear"] = stage2.StoredCriteria.Where(x => x.Criterion == "StructuralElementWearout").Select(x => x.Value).FirstOrDefault();
                sect["TypeWork"] = works;
                sect["Year"] = stage2.PublicationYear;
                var lastOverhaulYear = stage2.StoredCriteria.Where(x => x.Criterion == "LastOverhaulYearParam").Select(x => x.Value).FirstOrDefault();
                sect["DateKr"] = lastOverhaulYear.ToInt() > 0 ? lastOverhaulYear : string.Empty;
            }
            */
        }
    }
}