namespace Bars.Gkh.Overhaul.Hmao.Reports
{
    using System.Linq;

    using Bars.B4;
    using B4.Modules.Reports;

    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Overhaul.Hmao.ConfigSections;
    using Bars.Gkh.Overhaul.Hmao.DomainService;
    using Bars.Gkh.Overhaul.Hmao.Entities;

    using Castle.Windsor;
    using Gkh.Utils;

    public class HousesHaveNotCollectSumReport : BasePrintForm
    {
        #region Dependency injection members

        public IDomainService<RealityObject> RealityObjectDomain { get; set; }

        public IDomainService<DpkrCorrectionStage2> DpkrCorrectionDomain { get; set; }

        public IHmaoRealEstateTypeService RealEstateService { get; set; }

        #endregion

        public HousesHaveNotCollectSumReport()
            : base(new ReportTemplateBinary(Properties.Resources.HousesHaveNotCollectSum))
        {

        }

        public IWindsorContainer Container { get; set; }

        public override string Name
        {
            get
            {
                return "Отчет о собираемости средств граждан на КР за период ДПКР";
            }
        }

        public override string Desciption
        {
            get
            {
                return "Отчет о собираемости средств граждан на КР за период ДПКР";
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
                return "B4.controller.report.HousesHaveNotCollectSum";
            }
        }

        public override string RequiredPermission
        {
            get
            {
                return "Reports.GkhOverhaul.HousesHaveNotCollectSum";
            }
        }

        private long[] municipalityIds;

        public override void SetUserParams(BaseParams baseParams)
        {
            var strMunicpalIds = baseParams.Params.GetAs("municipalityIds", string.Empty);

            municipalityIds = !string.IsNullOrEmpty(strMunicpalIds)
                ? strMunicpalIds.Split(',').Select(x => x.ToLong()).ToArray()
                : new long[0];
        }

        public override string ReportGenerator { get; set; }

        public override void PrepareReport(ReportParams reportParams)
        {
            var config = Container.GetGkhConfig<OverhaulHmaoConfig>();
            var periodStartYear = config.ProgrammPeriodStart;
            var periodEndYear = config.ProgrammPeriodEnd;

            var sumByRoList = DpkrCorrectionDomain.GetAll()
                .Where(x => x.Stage2.Stage3Version.ProgramVersion.IsMain)
                .WhereIf(municipalityIds.Length > 0 , x => municipalityIds.Contains(x.Stage2.Stage3Version.ProgramVersion.Municipality.Id))
                .Where( x => x.Stage2.Stage3Version.IndexNumber > 0 || x.Stage2.Stage3Version.Year < periodEndYear)
                .Select(x => new
                {
                    Municipality = x.RealityObject.Municipality.Name,
                    x.RealityObject.Address,
                    RealObjId = x.RealityObject.Id,
                    x.Stage2.Sum
                })
                .OrderBy(x => x.Municipality)
                .ThenBy(x => x.Address)
                .AsEnumerable()
                .GroupBy(x => x.RealObjId)
                .Select(y => new
                            {
                                y.Key,
                                y.First().Municipality,
                                y.First().Address,
                                Sum =y.Sum(x => x.Sum)
                            })
                .ToList();

            var roQuery = RealityObjectDomain.GetAll()
                              .WhereIf(municipalityIds.Length > 0, x => municipalityIds.Contains(x.Municipality.Id));

            var collectionByRoDict = RealEstateService.GetCollectionByPeriod(roQuery, periodStartYear, periodEndYear);

            var sectionRealObj = reportParams.ComplexReportParams.ДобавитьСекцию("sectionRealObj");

            var number = 1;
            foreach (var roData in sumByRoList)
            {
                sectionRealObj.ДобавитьСтроку();

                sectionRealObj["Number"] = number++;
                sectionRealObj["Municipality"] = roData.Municipality;
                sectionRealObj["Address"] = roData.Address;
                sectionRealObj["Sum"] = roData.Sum;
                sectionRealObj["Collection"] = collectionByRoDict.ContainsKey(roData.Key) ? collectionByRoDict[roData.Key] : 0;
            }
        }
    }
}