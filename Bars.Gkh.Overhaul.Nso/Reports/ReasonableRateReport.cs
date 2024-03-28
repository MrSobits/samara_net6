namespace Bars.Gkh.Overhaul.Nso.Reports
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    
    using B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Config;
    using Bars.Gkh.ConfigSections.Overhaul.Enums;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Domain.CollectionExtensions;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Overhaul.Nso.ConfigSections;
    using Bars.Gkh.Overhaul.Nso.Entities;

    using Castle.Windsor;
    using Gkh.Utils;

    public class ReasonableRateReport : BasePrintForm
    {
        public ReasonableRateReport()
            : base(new ReportTemplateBinary(Properties.Resources.ReasonableRate))
        {
        }

        public IWindsorContainer Container { get; set; }

        public IDomainService<RealityObjectStructuralElementInProgrammStage3> DpkrDomain { get; set; }

        public IRepository<Municipality> MunicipalityDomain { get; set; }

        public IGkhParams GkhParams { get; set; }

        public override string Name
        {
            get { return "Экономически обоснованный тариф взносов на КР"; }
        }

        public override string Desciption
        {
            get { return "Экономически обоснованный тариф взносов на КР"; }
        }

        public override string GroupName
        {
            get { return "Региональная программа"; }
        }

        public override string ParamsController
        {
            get { return "B4.controller.report.ReasonableRate"; }
        }

        public override string RequiredPermission
        {
            get { return "Reports.GkhOverhaul.ReasonableRate"; }
        }

        private long[] municipalityIds = new long[0];

        private int startYear;

        public override void SetUserParams(BaseParams baseParams)
        {
            municipalityIds = baseParams.Params.GetAs<string>("muIds").ToLongArray();

            startYear = baseParams.Params.GetAs("startYear", 0);
        }

        public override string ReportGenerator { get; set; }

        public override void PrepareReport(ReportParams reportParams)
        {
            var config = Container.GetGkhConfig<OverhaulNsoConfig>();
            var endYear = config.ProgrammPeriodEnd;
            var rateCalcArea = config.RateCalcTypeArea;

            reportParams.SimpleReportParams["StartYear"] = startYear;
            reportParams.SimpleReportParams["EndYear"] = endYear;

            var dpkrYears = endYear - startYear + 1;
            var appParams = GkhParams.GetParams();

            var moLevel = appParams.ContainsKey("MoLevel") && !string.IsNullOrEmpty(appParams["MoLevel"].To<string>())
                ? appParams["MoLevel"].To<MoLevel>()
                : MoLevel.MunicipalUnion;

            var muNameById = MunicipalityDomain
                .GetAll()
                .Select(x => new
                {
                    x.Id,
                    x.Name
                })
                .ToDictionary(x => x.Id, y => y.Name);

            var dpkrInfo = DpkrDomain.GetAll()
                .Where(x => x.Year >= startYear)
                .Where(x => x.Year <= endYear)
                .WhereIf(municipalityIds.Length > 0 && moLevel == MoLevel.MunicipalUnion,
                    x => municipalityIds.Contains(x.RealityObject.Municipality.Id))
                .WhereIf(municipalityIds.Length > 0 && moLevel == MoLevel.Settlement,
                    x => municipalityIds.Contains(x.RealityObject.MoSettlement.Id))
                .OrderBy(x => x.RealityObject.Municipality.Name)
                .ThenBy(x => x.RealityObject.MoSettlement.Name)
                .Select(x => new
                {

                    RoId = x.RealityObject.Id,
                    MuId = x.RealityObject.Municipality.Id,
                    SettlementId = (long?)x.RealityObject.MoSettlement.Id,
                    x.Sum,
                    x.RealityObject.AreaMkd,
                    x.RealityObject.AreaLiving,
                    x.RealityObject.AreaLivingNotLivingMkd,
                })
                .AsEnumerable()
                .GroupBy(x => x.RoId)
                .Select(x =>
                {
                    var ro = x.First();

                    return new
                    {
                        RoId = x.Key,
                        ro.MuId,
                        ro.SettlementId,
                        Sum = x.SafeSum(y => y.Sum),
                        Area = rateCalcArea == RateCalcTypeArea.AreaLiving ? ro.AreaLiving.ToDecimal() :
                               rateCalcArea == RateCalcTypeArea.AreaLivingNotLiving ? ro.AreaLivingNotLivingMkd.ToDecimal() :    
                               rateCalcArea == RateCalcTypeArea.AreaMkd ? ro.AreaMkd.ToDecimal() : 0M
                    };
                })
                .ToArray();

            var dpkrInfoByMu = dpkrInfo
                .GroupBy(x => x.MuId)
                .ToDictionary(
                    x => x.Key, 
                    y => 
                    {
                        var sum = y.SafeSum(x => x.Sum);
                        var area = y.SafeSum(x => x.Area);

                        return new
                        {
                            Sum = sum,
                            Area = area,
                            Tariff = area > 0 && dpkrYears != 0 ? sum / area / (12 * dpkrYears) : 0
                        };
                    });

            var dpkrInfoBySettlement = dpkrInfo
                .GroupBy(x => x.MuId)
                .ToDictionary(
                    x => x.Key, 
                    y => y.Where(x => x.SettlementId.HasValue)
                          .GroupBy(x => x.SettlementId.Value)
                          .ToDictionary(
                            x => x.Key, 
                            z => 
                            {
                                var sum = z.SafeSum(x => x.Sum);
                                var area = z.SafeSum(x => x.Area);

                                return new
                                {
                                    Sum = sum,
                                    Area = area,
                                    Tariff = area > 0 && dpkrYears != 0 ? sum / area / (12 * dpkrYears) : 0
                                };
                            }));

            var sectionMu = reportParams.ComplexReportParams.ДобавитьСекцию("sectionMu");

            foreach (var infoByMu in dpkrInfoByMu)
            {
                sectionMu.ДобавитьСтроку();
                sectionMu["Municipality"] = muNameById.Get(infoByMu.Key);
                sectionMu["MuSum"] = infoByMu.Value.Sum;
                sectionMu["MuArea"] = infoByMu.Value.Area;
                sectionMu["MuTarif"] = infoByMu.Value.Tariff;

                var sectionSettlement = sectionMu.ДобавитьСекцию("sectionSettlement");

                if (moLevel == MoLevel.MunicipalUnion)
                {
                    sectionSettlement.ДобавитьСтроку();
                    sectionSettlement["Settlement"] = muNameById.Get(infoByMu.Key);
                    sectionSettlement["SettlementSum"] = infoByMu.Value.Sum;
                    sectionSettlement["SettlementArea"] = infoByMu.Value.Area;
                    sectionSettlement["SettlementTarif"] = infoByMu.Value.Tariff;
                    continue;
                }

                var infoBySettlement = dpkrInfoBySettlement.Get(infoByMu.Key);

                if (infoBySettlement == null)
                {
                    continue;
                }

                foreach (var info in infoBySettlement)
                {
                    sectionSettlement.ДобавитьСтроку();
                    sectionSettlement["Settlement"] = muNameById.Get(info.Key);
                    sectionSettlement["SettlementSum"] = info.Value.Sum;
                    sectionSettlement["SettlementArea"] = info.Value.Area;
                    sectionSettlement["SettlementTarif"] = info.Value.Tariff;
                }
            }

            var totalSum = dpkrInfoByMu.Values.SafeSum(x => x.Sum);
            var totalArea = dpkrInfoByMu.Values.SafeSum(x => x.Area);

            reportParams.SimpleReportParams["TotalSum"] = dpkrInfoByMu.Values.SafeSum(x => x.Sum);
            reportParams.SimpleReportParams["TotalArea"] = dpkrInfoByMu.Values.SafeSum(x => x.Area);
            reportParams.SimpleReportParams["TotalTarif"] = totalArea != 0 && dpkrYears != 0 ? totalSum / totalArea / (12 * dpkrYears) : 0;
        }
    }
}