namespace Bars.Gkh.Overhaul.Hmao.Reports
{
    using System.Linq;

    using Bars.B4;
    using B4.Modules.Reports;

    using Bars.B4.Utils;
    using Bars.Gkh.Overhaul.Hmao.ConfigSections;
    using Bars.Gkh.Overhaul.Hmao.Entities;
    using Bars.Gkh.Utils;

    using Castle.Windsor;

    public class DpkrGroupedByPeriodPublish : BasePrintForm
    {
        public DpkrGroupedByPeriodPublish()
            : base(new ReportTemplateBinary(Properties.Resources.DpkrGroupedByPeriodPublish))
        {
        }

        private long[] municipalityIds;
        public IWindsorContainer Container { get; set; }

        public override string Name
        {
            get { return "Региональная программа КР с группировкой по периодам (опубликованная программа)"; }
        }

        public override string Desciption
        {
            get { return "Региональная программа КР с группировкой по периодам (опубликованная программа)"; }
        }

        public override string GroupName
        {
            get { return "Региональная программа"; }
        }

        public override string ParamsController
        {
            get { return "B4.controller.report.DpkrGroupedByPeriodPublish"; }
        }

        public override string RequiredPermission
        {
            get { return "Ovrhl.DpkrGroupedByPeriodPublish"; }
        }

        public override void SetUserParams(BaseParams baseParams)
        {
            var municipalityIdsList = baseParams.Params.GetAs("municipalityIds", string.Empty);
            municipalityIds = !string.IsNullOrEmpty(municipalityIdsList)
                ? municipalityIdsList.Split(',').Select(id => id.ToLong()).ToArray()
                : new long[0];
        }

        public override string ReportGenerator { get; set; }

        public override void PrepareReport(ReportParams reportParams)
        {
            var config = Container.GetGkhConfig<OverhaulHmaoConfig>();
            var periodStart = config.ProgrammPeriodStart;
            var periodEnd = config.ProgrammPeriodEnd;
            var shortTermProgPeriod = config.ShortTermProgPeriod;

            if (shortTermProgPeriod == 0 || periodStart == 0 || periodEnd == 0)
            {
                return;
            }

            // Раскрываем вертикальную секцию
            CreateVerticalColums(reportParams, shortTermProgPeriod, periodStart, periodEnd);

            var dpkrDomain = Container.Resolve<IDomainService<PublishedProgramRecord>>();

            var data =
                dpkrDomain.GetAll()
                    .Where(x => x.PublishedProgram.ProgramVersion.Municipality != null)
                    .Where(x => x.PublishedProgram.ProgramVersion.IsMain)
                    .Where(x => x.Stage2 != null)
                    .WhereIf(municipalityIds.Any(),
                        x => municipalityIds.Contains(x.PublishedProgram.ProgramVersion.Municipality.Id))
                    .Select(x => new
                    {
                        MuName = x.PublishedProgram.ProgramVersion.Municipality.Name,
                        RoId = x.RealityObject.Id,
                        x.RealityObject.Address,
                        x.RealityObject.DateCommissioning,
                        x.RealityObject.MaximumFloors,
                        x.RealityObject.AreaMkd,
                        x.PublishedYear,
                        x.Stage2.Stage3Version.CommonEstateObjects
                    })
                    .OrderBy(x => x.MuName)
                    .ThenBy(x => x.Address)
                    .AsEnumerable()
                    .GroupBy(x => x.MuName)
                    .ToDictionary(x => x.Key, y => y.GroupBy(x => x.RoId).ToDictionary(x => x.Key));

            var sectionMunicipality = reportParams.ComplexReportParams.ДобавитьСекцию("SectionMun");

            var ind = 1;
            foreach (var munData in data)
            {
                sectionMunicipality.ДобавитьСтроку();
                sectionMunicipality["Municipality"] = munData.Key;

                foreach (var roData in munData.Value.Values)
                {
                    var relObjData = roData.First();
                    var sectionRealObj = sectionMunicipality.ДобавитьСекцию("SectionRealObj");
                    sectionRealObj.ДобавитьСтроку();
                    sectionRealObj["Number"] = ind++;
                    sectionRealObj["MunName"] = relObjData.MuName;
                    sectionRealObj["Address"] = relObjData.Address;
                    sectionRealObj["CommissioningYear"] =
                        relObjData.DateCommissioning.HasValue
                            ? relObjData.DateCommissioning.Value.Year.ToStr()
                            : string.Empty;
                    sectionRealObj["AreaMkd"] = relObjData.AreaMkd;
                    sectionRealObj["FloorsCount"] = relObjData.MaximumFloors;

                    var tempPeriodStart = periodStart;
                    var tempPeriodEnd = periodStart + shortTermProgPeriod - 1;
                    var periodCnt = 0;
                    do
                    {
                        sectionRealObj[string.Format("Years{0}", periodCnt)] =
                            tempPeriodEnd - tempPeriodStart > 0
                                ? string.Format("{0}-{1}", tempPeriodStart, tempPeriodEnd)
                                : tempPeriodStart.ToString();

                        string[] arrString = roData
                            .Where(x => tempPeriodStart <= x.PublishedYear)
                            .Where(x => tempPeriodEnd >= x.PublishedYear)
                            .Where(x => !string.IsNullOrEmpty(x.CommonEstateObjects))
                            .Select(x => x.CommonEstateObjects)
                            .SelectMany(x => x.Split(','))
                            .Distinct()
                            .ToArray();

                        sectionRealObj[string.Format("Ceo{0}", periodCnt)] =
                            arrString.Any()
                                ? arrString.AggregateWithSeparator(", ")
                                : string.Empty;

                        periodCnt++;
                        tempPeriodStart = tempPeriodEnd + 1;

                        tempPeriodEnd =
                            tempPeriodEnd + shortTermProgPeriod > periodEnd
                                ? periodEnd
                                : tempPeriodEnd + shortTermProgPeriod;
                    } while (tempPeriodStart <= periodEnd);
                }
            }
        }

        private void CreateVerticalColums(ReportParams reportParams, int shortTermProgPeriod, int periodStart,
            int periodEnd)
        {
            var verticalSection = reportParams.ComplexReportParams.ДобавитьСекцию("SectionVert");

            var tempPeriodStart = periodStart;
            int tempPeriodEnd = periodStart + shortTermProgPeriod - 1;
            var periodCnt = 0;

            do
            {
                verticalSection.ДобавитьСтроку();
                verticalSection["Years"] = tempPeriodEnd - tempPeriodStart > 0
                    ? string.Format("{0}-{1}", tempPeriodStart, tempPeriodEnd)
                    : tempPeriodStart.ToString();
                verticalSection["Ceo"] = string.Format("$Ceo{0}$", periodCnt);

                periodCnt++;
                tempPeriodStart = tempPeriodEnd + 1;

                tempPeriodEnd =
                    tempPeriodEnd + shortTermProgPeriod > periodEnd
                        ? periodEnd
                        : tempPeriodEnd + shortTermProgPeriod;
            } while (tempPeriodStart <= periodEnd);
        }
    }
}