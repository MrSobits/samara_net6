namespace Bars.Gkh.Overhaul.Regions.Kamchatka.Report
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using B4.Modules.Reports;

    using Bars.B4.Utils;
    using Bars.Gkh.Config;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Domain.CollectionExtensions;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Overhaul.Entities;
    using Bars.Gkh.Overhaul.Hmao.Entities;
    using Bars.Gkh.Utils;

    using Castle.Windsor;

    public class ProgramCrByDpkrForm2Report : BasePrintForm
    {
        public ProgramCrByDpkrForm2Report()
            : base(new ReportTemplateBinary(Properties.Resources.ProgramCrByDpkrForm2))
        {
        }

        public IDomainService<PublishedProgramRecord> PublishedProgRecDomain { get; set; }
        public IRepository<Municipality> MunicipalityDomain { get; set; }
        public IDomainService<RealityObject> RealObjDomain { get; set; }
        public IDomainService<RealityObjectStructuralElement> RoSeDomain { get; set; }
        public IGkhParams GkhParams { get; set; }

        private long[] municipalityIds; 
        private int startYear;  //Начало периода. Выбор года из периода ДПКР
        private int endYear;    //Окончание периода. Выбор года из периода ДПКР
        private MoLevel moLevel;  

        public IWindsorContainer Container { get; set; }

        public override string Name
        {
            get { return "Форма 2. Реестр многоквартирных домов по видам ремонта (Камчатка)"; }
        }

        public override string Desciption
        {
            get { return "Форма 2. Реестр многоквартирных домов по видам ремонта (Камчатка)"; }
        }

        public override string GroupName
        {
            get { return "Региональная программа"; }
        }

        public override string ParamsController
        {
            get
            {
                return "B4.controller.report.ProgramCrByDpkrForm2";
            }
        }

        public override string RequiredPermission
        {
            get
            {
                return "Reports.GkhOverhaul.ProgramCrByDpkrForm2";
            }
        }

        public override void SetUserParams(BaseParams baseParams)
        {
            var moIds = baseParams.Params.GetAs<string>("municipalityIds").ToLongArray();
            startYear = baseParams.Params.GetAs<int>("startYear");
            endYear = baseParams.Params.GetAs<int>("endYear");

            var appParams = GkhParams.GetParams();
            moLevel = appParams.ContainsKey("MoLevel")
                ? appParams["MoLevel"].To<MoLevel>()
                : MoLevel.MunicipalUnion;

            municipalityIds = MunicipalityDomain.GetAll()
                    .WhereIf(moIds.Any(), x => moIds.Contains(x.Id))
                    .Select(x => new
                    {
                        x.Id,
                        x.Level
                    })
                    .AsEnumerable()
                    .Where(x => x.Level.ToMoLevel(Container) == moLevel)
                    .Select(x => x.Id)
                    .ToArray();
        }

        public override void PrepareReport(ReportParams reportParams)
        {
            var engeneerSystemCeoCodes = new List<string> { "4", "5", "6", "7", "8" };
            var neededVolumeCeoCode = new List<string> { "1", "2", "3" };

            var municipalities = MunicipalityDomain.GetAll()
                .Select(x => new
                {
                    x.Id,
                    x.Name,
                    x.Level
                })
                .AsEnumerable()
                .Select(x => new
                {
                    x.Id,
                    x.Name,
                    Level = x.Level.ToMoLevel(Container)
                })
                .ToDictionary(x => x.Id);

            var realObjs = RealObjDomain.GetAll()
                .WhereIf(municipalityIds.Length > 0, x => municipalityIds.Contains(x.Municipality.Id)
                                                          || municipalityIds.Contains(x.MoSettlement.Id))
                .Select(x => new
                {
                    x.Id,
                    x.Address,
                    x.NumberLifts
                })
                .ToDictionary(x => x.Id);

            var query =
                PublishedProgRecDomain.GetAll()
                    .Where(x => x.Stage2.Stage3Version.RealityObject.MoSettlement != null)
                    .WhereIf(
                        municipalityIds.Length > 0,
                        x => municipalityIds.Contains(x.PublishedProgram.ProgramVersion.Municipality.Id))
                    .Where(x => x.PublishedProgram.ProgramVersion.IsMain)
                    .Where(x => x.PublishedYear >= startYear && x.PublishedYear <= endYear);

            var publishProgData = query
                .OrderBy(x => x.Stage2.Stage3Version.RealityObject.Municipality.Name)
                .ThenBy(x => x.Stage2.Stage3Version.RealityObject.MoSettlement.Name)
                .ThenBy(x => x.Stage2.Stage3Version.RealityObject.Address)
                .Select(x => new
                {
                    MuId = x.Stage2.Stage3Version.RealityObject.Municipality.Id,
                    SettlId = x.Stage2.Stage3Version.RealityObject.MoSettlement.Id,
                    RoId = x.Stage2.Stage3Version.RealityObject.Id,
                    x.PublishedYear,
                    x.Stage2.Sum,
                    CeoCode = x.Stage2.CommonEstateObject.Code
                })
                .AsEnumerable();

            var publishProgInfo = publishProgData
                .GroupBy(x => x.MuId)
                .ToDictionary(x => x.Key,
                    y => y.GroupBy(x => x.SettlId)
                            .ToDictionary(x => x.Key,
                                w => w.GroupBy(x => x.RoId)
                                      .ToDictionary(x => x.Key,
                                         z => z.GroupBy(x => x.CeoCode).ToDictionary(x => x.Key, k => k.SafeSum(x => x.Sum)))));


            var publishProgInfoByMu = publishProgData
                .GroupBy(x => x.MuId)
                .ToDictionary(x => x.Key,
                    y => y.GroupBy(x => x.CeoCode).ToDictionary(x => x.Key, k => k.SafeSum(x => x.Sum)));


            var publishProgInfoBySettl = publishProgData
                .GroupBy(x => x.SettlId)
                            .ToDictionary(x => x.Key,
                                w => w.GroupBy(x => x.CeoCode).ToDictionary(x => x.Key, k => k.SafeSum(x => x.Sum)));

            var publishProgTotalInfo = publishProgData
                .GroupBy(x => x.CeoCode).ToDictionary(x => x.Key, k => k.SafeSum(x => x.Sum));

            var roSeInfoData = RoSeDomain.GetAll()
                .WhereIf(municipalityIds.Length > 0, x => municipalityIds.Contains(x.RealityObject.Municipality.Id) || municipalityIds.Contains(x.RealityObject.MoSettlement.Id))
                .Where(x => neededVolumeCeoCode.Contains(x.StructuralElement.Group.CommonEstateObject.Code))
                .Where(x => query.Any(y => y.Stage2.Stage3Version.RealityObject.Id == x.RealityObject.Id))
                .Select(x => new
                {
                    RoId = x.RealityObject.Id,
                    MuId = x.RealityObject.Municipality.Id,
                    SettlId = x.RealityObject.MoSettlement.Id,
                    x.StructuralElement.Group.CommonEstateObject.Code,
                    x.Volume
                })
                .AsEnumerable();


            var roSeInfoByRo = roSeInfoData                
                .GroupBy(x => x.RoId)
                .ToDictionary(x => x.Key,
                              y => y.GroupBy(x => x.Code).ToDictionary(x => x.Key, z => z.SafeSum(x => x.Volume)));

            reportParams.SimpleReportParams["PeriodStart"] = startYear;
            reportParams.SimpleReportParams["PeriodEnd"] = endYear;
            reportParams.SimpleReportParams["Municipality"] = municipalityIds.Length == municipalities.Values.Count(x => x.Level == moLevel) ? "всем МО"
                : municipalities.Values.Where(x => x.Level == moLevel).WhereIf(municipalityIds.Length > 0, x => municipalityIds.Contains(x.Id)).Select(x => x.Name).AggregateWithSeparator(", ");
            reportParams.SimpleReportParams["TotalofTotalSum"] = publishProgTotalInfo.SafeSum(x => x.Value);
            reportParams.SimpleReportParams["TotalEngSysSum"] = publishProgTotalInfo.Where(x => engeneerSystemCeoCodes.Contains(x.Key)).SafeSum(x => x.Value);
            reportParams.SimpleReportParams["TotalCeo7Sum"] = publishProgTotalInfo.Get("7");
            reportParams.SimpleReportParams["TotalCeo5Sum"] = publishProgTotalInfo.Get("5");
            reportParams.SimpleReportParams["TotalCeo4Sum"] = publishProgTotalInfo.Get("4");
            reportParams.SimpleReportParams["TotalCeo6Sum"] = publishProgTotalInfo.Get("6");
            reportParams.SimpleReportParams["TotalCeo8Sum"] = publishProgTotalInfo.Get("8");

            reportParams.SimpleReportParams["TotalCeo1Sum"] = publishProgTotalInfo.Get("1");
            reportParams.SimpleReportParams["TotalCeo2Sum"] = publishProgTotalInfo.Get("2");
            reportParams.SimpleReportParams["TotalCeo3Sum"] = publishProgTotalInfo.Get("3");
            var sectionMu = reportParams.ComplexReportParams.ДобавитьСекцию("sectionMu");

            var muNumber = 1;
            var totalNumberLift = 0;

            var ceo1VolumeTot = 0m;
            var ceo2VolumeTot = 0m;
            var ceo3VolumeTot = 0m;

            foreach (var dataByMu in publishProgInfo)
            {
                sectionMu.ДобавитьСтроку();

                var tempPublishProgInfoByMu = publishProgInfoByMu.Get(dataByMu.Key) ?? new Dictionary<string, decimal>();

                sectionMu["Number"] = muNumber;
                sectionMu["Municipality"] = municipalities.Get(dataByMu.Key).Return(x => x.Name);
                sectionMu["TotalSumMR"] = tempPublishProgInfoByMu.SafeSum(x => x.Value);
                sectionMu["EngSysSumMR"] = tempPublishProgInfoByMu.Where(x => engeneerSystemCeoCodes.Contains(x.Key)).SafeSum(x => x.Value);
                sectionMu["Ceo7SumMR"] = tempPublishProgInfoByMu.Get("7");
                sectionMu["Ceo5SumMR"] = tempPublishProgInfoByMu.Get("5");
                sectionMu["Ceo4SumMR"] = tempPublishProgInfoByMu.Get("4");
                sectionMu["Ceo6SumMR"] = tempPublishProgInfoByMu.Get("6");
                sectionMu["Ceo8SumMR"] = tempPublishProgInfoByMu.Get("8");

                sectionMu["Ceo1SumMR"] = tempPublishProgInfoByMu.Get("1");
                sectionMu["Ceo2SumMR"] = tempPublishProgInfoByMu.Get("2");
                sectionMu["Ceo3SumMR"] = tempPublishProgInfoByMu.Get("3");

                var sectionSettlement = sectionMu.ДобавитьСекцию("sectionSettlement");

                var ceo1VolumeMr = 0m;
                var ceo2VolumeMr = 0m;
                var ceo3VolumeMr = 0m;

                var settlNum = 1;
                var muNumberLift = 0;
                foreach (var dataBySettl in dataByMu.Value)
                {
                    sectionSettlement.ДобавитьСтроку();

                    var tempPublishProgInfoBySettl = publishProgInfoBySettl.Get(dataBySettl.Key) ?? new Dictionary<string, decimal>();

                    sectionSettlement["SettNumber"] = "{0}.{1}".FormatUsing(muNumber, settlNum);
                    settlNum++;

                    sectionSettlement["Settlement"] = municipalities.Get(dataBySettl.Key).Return(x => x.Name);
                    sectionSettlement["TotalSumMO"] = tempPublishProgInfoBySettl.SafeSum(x => x.Value);
                    sectionSettlement["EngSysSumMO"] = tempPublishProgInfoBySettl.Where(x => engeneerSystemCeoCodes.Contains(x.Key)).SafeSum(x => x.Value);
                    sectionSettlement["Ceo7SumMO"] = tempPublishProgInfoBySettl.Get("7");
                    sectionSettlement["Ceo5SumMO"] = tempPublishProgInfoBySettl.Get("5");
                    sectionSettlement["Ceo4SumMO"] = tempPublishProgInfoBySettl.Get("4");
                    sectionSettlement["Ceo6SumMO"] = tempPublishProgInfoBySettl.Get("6");
                    sectionSettlement["Ceo8SumMO"] = tempPublishProgInfoBySettl.Get("8");

                    sectionSettlement["Ceo1SumMO"] = tempPublishProgInfoBySettl.Get("1");
                    sectionSettlement["Ceo2SumMO"] = tempPublishProgInfoBySettl.Get("2");
                    sectionSettlement["Ceo3SumMO"] = tempPublishProgInfoBySettl.Get("3");

                    var ceo1VolumeMo = 0m;
                    var ceo2VolumeMo = 0m;
                    var ceo3VolumeMo = 0m;

                    var settlNumberLift = 0;
                    var roNumber = 1;
                    var sectionRo = sectionSettlement.ДобавитьСекцию("sectionRo");
                    foreach (var dataByRo in dataBySettl.Value)
                    {
                        sectionRo.ДобавитьСтроку();

                        var tempRoSeInfoByRo = roSeInfoByRo.Get(dataByRo.Key) ?? new Dictionary<string, decimal>();

                        sectionRo["RoNumber"] = "{0}.{1}.{2}".FormatUsing(muNumber, settlNum, roNumber++);
                        sectionRo["Address"] = realObjs.Get(dataByRo.Key).Return(x => x.Address);
                        sectionRo["TotalSum"] = dataByRo.Value.SafeSum(x => x.Value);
                        sectionRo["EngSysSum"] = dataByRo.Value.Where(x => engeneerSystemCeoCodes.Contains(x.Key)).SafeSum(x => x.Value);

                        var numberLifts = realObjs.Get(dataByRo.Key).Return(x => x.NumberLifts).ToInt();
                        settlNumberLift += numberLifts;

                        sectionRo["LiftCnt"] = numberLifts;
                        sectionRo["Ceo1Sum"] = 0;
                        sectionRo["Ceo2Sum"] = 0;
                        sectionRo["Ceo3Sum"] = 0;
                        sectionRo["Ceo4Sum"] = 0;
                        sectionRo["Ceo5Sum"] = 0;
                        sectionRo["Ceo6Sum"] = 0;
                        sectionRo["Ceo7Sum"] = 0;
                        sectionRo["Ceo8Sum"] = 0;

                        sectionRo["Ceo1Volume"] = 0;
                        sectionRo["Ceo2Volume"] = 0;
                        sectionRo["Ceo3Volume"] = 0;

                        foreach (var dataByCode in dataByRo.Value)
                        {
                            sectionRo["Ceo{0}Sum".FormatUsing(dataByCode.Key)] = dataByCode.Value;
                        }

                        if (sectionRo["Ceo1Sum"].ToDecimal() != 0)
                        {
                            sectionRo["Ceo1Volume"] = tempRoSeInfoByRo.Get("1");
                        }
                        if (sectionRo["Ceo2Sum"].ToDecimal() != 0)
                        {
                            sectionRo["Ceo2Volume"] = tempRoSeInfoByRo.Get("2");
                        }
                        if (sectionRo["Ceo3Sum"].ToDecimal() != 0)
                        {
                            sectionRo["Ceo3Volume"] = tempRoSeInfoByRo.Get("3");
                        }

                        ceo1VolumeMo += sectionRo["Ceo1Volume"].ToDecimal();
                        ceo2VolumeMo += sectionRo["Ceo2Volume"].ToDecimal();
                        ceo3VolumeMo += sectionRo["Ceo3Volume"].ToDecimal();
                    }

                    sectionSettlement["LiftCntMO"] = settlNumberLift;
                    muNumberLift += settlNumberLift;

                    sectionSettlement["Ceo1VolumeMO"] = ceo1VolumeMo;
                    sectionSettlement["Ceo2VolumeMO"] = ceo2VolumeMo;
                    sectionSettlement["Ceo3VolumeMO"] = ceo3VolumeMo;

                    ceo1VolumeMr += ceo1VolumeMo;
                    ceo2VolumeMr += ceo2VolumeMo;
                    ceo3VolumeMr += ceo3VolumeMo;
                }

                sectionMu["LiftCntMR"] = muNumberLift;
                totalNumberLift += muNumberLift;
                muNumber++;

                sectionMu["Ceo1VolumeMR"] = ceo1VolumeMr;
                sectionMu["Ceo2VolumeMR"] = ceo2VolumeMr;
                sectionMu["Ceo3VolumeMR"] = ceo3VolumeMr;

                ceo1VolumeTot += ceo1VolumeMr;
                ceo2VolumeTot += ceo2VolumeMr;
                ceo3VolumeTot += ceo3VolumeMr;
            }

            reportParams.SimpleReportParams["TotalLiftCnt"] = totalNumberLift;

            reportParams.SimpleReportParams["TotalCeo1Volume"] = ceo1VolumeTot;
            reportParams.SimpleReportParams["TotalCeo2Volume"] = ceo2VolumeTot;
            reportParams.SimpleReportParams["TotalCeo3Volume"] = ceo3VolumeTot;
        }
    }
}