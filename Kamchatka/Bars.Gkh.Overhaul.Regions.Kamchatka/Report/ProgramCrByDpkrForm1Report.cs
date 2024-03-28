namespace Bars.Gkh.Overhaul.Regions.Kamchatka.Report
{
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
    using Bars.Gkh.Overhaul.Regions.Kamchatka.Properties;
    using Bars.Gkh.Utils;

    using Castle.Windsor;

    public class ProgramCrByDpkrForm1Report : BasePrintForm
    {
        private readonly IDomainService<PublishedProgramRecord> _publishedProgRecDomain;
        private readonly IRepository<Municipality> _municipalityDomain;
        private readonly IDomainService<RealityObject> _roDomain;
        private readonly IGkhParams _gkhParams;
        private readonly IWindsorContainer _container;
        private readonly IDomainService<RealityObjectStructuralElement> _roSeDomain;

        private int startYear;
        private int endYear;
        private long[] municipalityIds;
        private MoLevel moLevel;

        public ProgramCrByDpkrForm1Report(
            IDomainService<PublishedProgramRecord> publishedProgRecDomain,
            IRepository<Municipality> municipalityDomain,
            IDomainService<RealityObject> roDomain,
            IGkhParams gkhParams,
            IWindsorContainer container,
            IDomainService<RealityObjectStructuralElement> roSeDomain)
            : base(new ReportTemplateBinary(Resources.ProgramCrByDpkrForm1))
        {
            _publishedProgRecDomain = publishedProgRecDomain;
            _municipalityDomain = municipalityDomain;
            _roDomain = roDomain;
            _gkhParams = gkhParams;
            _container = container;
            _roSeDomain = roSeDomain;
        }

        public override string Name
        {
            get { return "Форма 1. Перечень многоквартирных домов, включенных в краткосрочный план КР (Камчатка)"; }
        }

        public override string Desciption
        {
            get { return "Форма 1. Перечень многоквартирных домов, включенных в краткосрочный план КР (Камчатка)"; }
        }

        public override string GroupName
        {
            get { return "Региональная программа"; }
        }

        public override string ParamsController
        {
            get { return "B4.controller.report.ProgramCrByDpkrForm1"; }
        }

        public override string RequiredPermission
        {
            get { return "Reports.GkhOverhaul.ProgramCrByDpkrForm1"; }
        }

        public override void SetUserParams(BaseParams baseParams)
        {
            startYear = baseParams.Params.GetAs<int>("startYear");
            endYear = baseParams.Params.GetAs<int>("endYear");
            var moIds = baseParams.Params.GetAs<string>("municipalityIds").ToLongArray();

            var appParams = _gkhParams.GetParams();
            moLevel = appParams.ContainsKey("MoLevel")
                ? appParams["MoLevel"].To<MoLevel>()
                : MoLevel.MunicipalUnion;

            municipalityIds = _municipalityDomain.GetAll()
                    .WhereIf(moIds.Any(), x => moIds.Contains(x.Id))
                    .Select(x => new
                    {
                        x.Id,
                        x.Level
                    })
                    .AsEnumerable()
                    .Where(x => x.Level.ToMoLevel(_container) == moLevel)
                    .Select(x => x.Id)
                    .ToArray();
        }

        public override void PrepareReport(ReportParams reportParams)
        {
            reportParams.SimpleReportParams["YearStart"] = startYear;
            reportParams.SimpleReportParams["YearEnd"] = endYear;

            if (!municipalityIds.Any())
            {
                reportParams.SimpleReportParams["Municipality"] = "по всем МО";
            }
            else
            {
                var mus = _municipalityDomain.GetAll()
                    .WhereIf(municipalityIds.Any(), x => municipalityIds.Contains(x.Id))
                    .Select(x => x.Name).ToArray();
                reportParams.SimpleReportParams["Municipality"] = string.Join(",", mus);
            }

            var municipalities = _municipalityDomain.GetAll()
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
                    Level = x.Level.ToMoLevel(_container)
                })
                .ToDictionary(x => x.Id);

            var realObjs = _roDomain.GetAll()
                .OrderBy(x => x.Address)
                .WhereIf(municipalityIds.Length > 0, x => municipalityIds.Contains(x.Municipality.Id)
                                                          || municipalityIds.Contains(x.MoSettlement.Id))

                .Select(x => new
                {
                    x.Id,
                    x.Address,
                    x.BuildYear,
                    Area = x.AreaMkd.HasValue ? x.AreaMkd.Value : 0m,
                    x.MaximumFloors,
                    x.NumberEntrances,
                    x.NumberLiving
                })
                .ToDictionary(x => x.Id);

            var publProgData =
                _publishedProgRecDomain.GetAll()
                    .WhereIf(
                        municipalityIds.Any(),
                        x => municipalityIds.Contains(x.PublishedProgram.ProgramVersion.Municipality.Id))
                    .Where(x => x.PublishedProgram.ProgramVersion.IsMain)
                    .Where(x => x.PublishedYear >= startYear && x.PublishedYear <= endYear)
                    .OrderBy(x => x.Stage2.Stage3Version.RealityObject.Municipality.Name)
                    .ThenBy(x => x.Stage2.Stage3Version.RealityObject.MoSettlement.Name)
                    .ThenBy(x => x.Stage2.Stage3Version.RealityObject.Address)
                    .Select(x => new
                    {
                        MuId = x.Stage2.Stage3Version.RealityObject.Municipality.Id,
                        SettlId = x.Stage2.Stage3Version.RealityObject.MoSettlement.Id,
                        RoId = x.Stage2.Stage3Version.RealityObject.Id,
                        x.Stage2.Stage3Version.Sum,
                        x.Stage2.Stage3Version.RealityObject.AreaLiving,
                        x.Stage2.Stage3Version.RealityObject.AreaLivingNotLivingMkd
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.MuId)
                    .ToDictionary(x => x.Key,
                        y => y.GroupBy(x => x.SettlId)
                            .ToDictionary(x => x.Key, z => z.GroupBy(w => new
                                                                              {
                                                                                  w.RoId, w.AreaLiving,w.AreaLivingNotLivingMkd
                                                                              })
                                .ToDictionary(w => w.Key, l => l.SafeSum(x => x.Sum))));

            var roSes =
                _roSeDomain.GetAll()
                    .WhereIf(
                        municipalityIds.Length > 0,
                        x =>
                        municipalityIds.Contains(x.RealityObject.Municipality.Id)
                        || municipalityIds.Contains(x.RealityObject.MoSettlement.Id))

                    .Select(x => new
                                     {
                                         roId = x.RealityObject.Id,
                                         x.StructuralElement.Name,
                                         x.StructuralElement.Group.CommonEstateObject.Code,
                                         x.LastOverhaulYear
                                     })
                    .AsEnumerable()
                    .GroupBy(x => x.roId)
                    .ToDictionary(x => x.Key, y => new
                                                       {
                                                           Names = y.Where(x => x.Code == "2").Select(x => x.Name).AggregateWithSeparator(", "),
                                                           LastOverhaulYear = y.Select(x => x.LastOverhaulYear).OrderByDescending(x => x).FirstOrDefault()
                                                       });


            var sectionMu = reportParams.ComplexReportParams.ДобавитьСекцию("sectionMu");
            var number = 1;
            var totalMkdArea = 0m;
            var totalRoomsArea = 0m;
            var totalRoomsLivingArea = 0m;
            var totalCrCost = 0m;

            foreach (var dataByMu in publProgData)
            {
                sectionMu.ДобавитьСтроку();

                sectionMu["Number"] = number;
                sectionMu["MunicipalityName"] = municipalities.Get(dataByMu.Key).Return(x => x.Name);

                var sectionSettlement = sectionMu.ДобавитьСекцию("sectionSettlement");
                var settlNum = 1;
                var totalMkdAreaMr = 0m;
                var totalRoomsAreaMr = 0m;
                var totalRoomsLivingAreaMr = 0m;
                var totalCrCostMr = 0m;

                foreach (var dataBySettl in dataByMu.Value)
                {
                    sectionSettlement.ДобавитьСтроку();

                    sectionSettlement["SettNumber"] = "{0}.{1}".FormatUsing(number, settlNum);
                    
                    sectionSettlement["SettlementName"] = municipalities.Get(dataBySettl.Key).Return(x => x.Name);

                    var sectionRo = sectionSettlement.ДобавитьСекцию("sectionRo");
                    var roNum = 1;
                    var totalMkdAreaMo = 0m;
                    var totalRoomsAreaMo = 0m;
                    var totalRoomsLivingAreaMo = 0m;
                    var totalCrCostMo = 0m;

                    foreach (var dataByRo in dataBySettl.Value)
                    {
                        sectionRo.ДобавитьСтроку();

                        sectionRo["RoNumber"] = "{0}.{1}.{2}".FormatUsing(number, settlNum, roNum);
                        roNum++;

                        sectionRo["Address"] = realObjs.Get(dataByRo.Key.RoId).Return(x => x.Address);
                        sectionRo["BuildYear"] = realObjs.Get(dataByRo.Key.RoId).Return(x => x.BuildYear);
                        sectionRo["MkdArea"] = realObjs.Get(dataByRo.Key.RoId).Return(x => x.Area);
                        sectionRo["MaximumFloors"] = realObjs.Get(dataByRo.Key.RoId).Return(x => x.MaximumFloors);
                        sectionRo["NumberEntrances"] = realObjs.Get(dataByRo.Key.RoId).Return(x => x.NumberEntrances);
                        sectionRo["NumberLiving"] = realObjs.Get(dataByRo.Key.RoId).Return(x => x.NumberLiving);

                        var roseInfo = roSes.Get(dataByRo.Key.RoId);

                        if (roseInfo != null)
                        {
                            sectionRo["LastCrYear"] = roseInfo.LastOverhaulYear;
                            sectionRo["Fasad"] = roseInfo.Names;
                        }
                        else
                        {
                            sectionRo["LastCrYear"] = string.Empty;
                            sectionRo["Fasad"] = string.Empty;
                        }

                        var area = dataByRo.Key.AreaLivingNotLivingMkd.HasValue ? dataByRo.Key.AreaLivingNotLivingMkd.Value : 0m;
                        sectionRo["RoomsArea"] = area;
                        sectionRo["RoomsLivingArea"] = dataByRo.Key.AreaLiving.HasValue ? dataByRo.Key.AreaLiving.Value : 0m;
                        sectionRo["CrCost"] = dataByRo.Value;

                        totalMkdAreaMo += sectionRo["MkdArea"].ToDecimal();
                        totalRoomsAreaMo += sectionRo["RoomsArea"].ToDecimal();
                        totalRoomsLivingAreaMo += sectionRo["RoomsLivingArea"].ToDecimal();
                        totalCrCostMo += sectionRo["CrCost"].ToDecimal();

                        if (area != 0)
                        {
                            sectionRo["SpecificCost"] = dataByRo.Value / area;
                            sectionRo["LimitCost"] = dataByRo.Value / area;
                        }
                        else
                        {
                            sectionRo["SpecificCost"] = 0;
                            sectionRo["LimitCost"] = 0;
                        }
                    }

                    sectionSettlement["TotalMkdAreaMO"] = totalMkdAreaMo;
                    totalMkdAreaMr += sectionSettlement["TotalMkdAreaMO"].ToDecimal();
                    sectionSettlement["TotalRoomsAreaMO"] = totalRoomsAreaMo;
                    totalRoomsAreaMr += sectionSettlement["TotalRoomsAreaMO"].ToDecimal();
                    sectionSettlement["TotalRoomsLivingAreaMO"] = totalRoomsLivingAreaMo;
                    totalRoomsLivingAreaMr += sectionSettlement["TotalRoomsLivingAreaMO"].ToDecimal();
                    sectionSettlement["TotalCrCostMO"] = totalCrCostMo;
                    totalCrCostMr += sectionSettlement["TotalCrCostMO"].ToDecimal();

                    if (totalRoomsAreaMo != 0)
                    {
                        sectionSettlement["TotalSpecificCostMO"] = totalCrCostMo / totalRoomsAreaMo;
                        sectionSettlement["TotalLimitCostMO"] = totalCrCostMo / totalRoomsAreaMo;
                    }
                    else
                    {
                        sectionSettlement["TotalSpecificCostMO"] = 0;
                        sectionSettlement["TotalLimitCostMO"] = 0;
                    }

                    settlNum++;
                }

                sectionMu["TotalMkdAreaMR"] = totalMkdAreaMr;
                totalMkdArea += sectionMu["TotalMkdAreaMR"].ToDecimal();
                sectionMu["TotalRoomsAreaMR"] = totalRoomsAreaMr;
                totalRoomsArea += sectionMu["TotalRoomsAreaMR"].ToDecimal();
                sectionMu["TotalRoomsLivingAreaMR"] = totalRoomsLivingAreaMr;
                totalRoomsLivingArea += sectionMu["TotalRoomsLivingAreaMR"].ToDecimal();
                sectionMu["TotalCrCostMR"] = totalCrCostMr;
                totalCrCost += sectionMu["TotalCrCostMR"].ToDecimal();

                if (totalRoomsAreaMr != 0)
                {
                    sectionMu["TotalSpecificCostMR"] = totalCrCostMr / totalRoomsAreaMr;
                    sectionMu["TotalLimitCostMR"] = totalCrCostMr / totalRoomsAreaMr;
                }
                else
                {
                    sectionMu["TotalSpecificCostMR"] = 0;
                    sectionMu["TotalLimitCostMR"] = 0;
                }

                number++;
            }

            reportParams.SimpleReportParams["TotalMkdArea"] = totalMkdArea;
            reportParams.SimpleReportParams["TotalRoomsArea"] = totalRoomsArea;
            reportParams.SimpleReportParams["TotalRoomsLivingArea"] = totalRoomsLivingArea;
            reportParams.SimpleReportParams["TotalCrCost"] = totalCrCost;

            if (totalRoomsArea != 0)
            {
                reportParams.SimpleReportParams["TotalSpecificCost"] = totalCrCost / totalRoomsArea;
                reportParams.SimpleReportParams["TotalLimitCost"] = totalCrCost / totalRoomsArea;
            }
            else
            {
                reportParams.SimpleReportParams["TotalSpecificCost"] = 0;
                reportParams.SimpleReportParams["TotalLimitCost"] = 0;
            }
        }
    }
}