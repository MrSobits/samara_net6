namespace Bars.Gkh.Overhaul.Hmao.Reports
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using B4.Modules.Reports;
    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Domain.CollectionExtensions;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Entities.Dicts;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Overhaul.Entities;
    using Bars.Gkh.Overhaul.Hmao.ConfigSections;
    using Bars.Gkh.Overhaul.Hmao.Entities;

    using Castle.Windsor;
    using Gkh.Utils;

    public class RegisterMkdToBeRepairedOverhaul : BasePrintForm
    {
        #region Properties

        private int startYear;

        private int endYear;

        private long[] municipalityIds;

        public IWindsorContainer Container { get; set; }
       
        public RegisterMkdToBeRepairedOverhaul()
            : base(new ReportTemplateBinary(Properties.Resources.RegisterMkdToBeRepaired))
        {
        }

        public override string Name
        {
            get
            {
                return "02_Реестр МКД, подлежащих ремонту (на основе Долгосрочной программы)";
            }
        }

        public override string Desciption
        {
            get
            {
                return "02_Реестр МКД, подлежащих ремонту (на основе Долгосрочной программы)";
            }
        }

        public override string GroupName
        {
            get
            {
                return "Формы для Фонда";
            }
        }

        public override string ParamsController
        {
            get
            {
                return "B4.controller.report.RegisterMkdToBeRepairedOverhaul";
            }
        }

        public override string RequiredPermission
        {
            get
            {
                return string.Empty;
            }
        }

        #endregion Properties

        public override void SetUserParams(BaseParams baseParams)
        {
            startYear = baseParams.Params["startYear"].ToInt();
            endYear = baseParams.Params["endYear"].ToInt();

            municipalityIds = baseParams.Params.GetAs("municipalityIds", string.Empty).ToLongArray();
        }

        public override void PrepareReport(ReportParams reportParams)
        {
            var correctionStageService = Container.Resolve<IDomainService<DpkrCorrectionStage2>>();
            var versionRecordService = Container.Resolve<IDomainService<VersionRecord>>();
            var workPriceDomain = Container.Resolve<IDomainService<WorkPrice>>();
            var roSeInProgram = Container.Resolve<IDomainService<RealityObjectStructuralElementInProgramm>>();
            var roSeService = Container.Resolve<IDomainService<RealityObjectStructuralElement>>();
            var strElWorkDomain = Container.Resolve<IDomainService<StructuralElementWork>>();

            var config = Container.GetGkhConfig<OverhaulHmaoConfig>();
            var groupByRoPeriod = config.GroupByRoPeriod;
            var periodEndYear = config.ProgrammPeriodEnd;
            
            List<DataProxy> data;
            
            if (groupByRoPeriod == 0)
            {
                data =
                    correctionStageService.GetAll()
                        .Where(x => x.Stage2.Stage3Version.ProgramVersion.IsMain)
                        .Where(x => municipalityIds.Contains(x.Stage2.Stage3Version.ProgramVersion.Municipality.Id))
                        .Where(
                            x => x.Stage2.Stage3Version.IndexNumber > 0 || x.Stage2.Stage3Version.Year < periodEndYear)
                        .Where(x => x.PlanYear <= endYear && x.PlanYear >= startYear)
                        .Select(
                            x =>
                            new DataProxy
                                {
                                    Municipality = x.RealityObject.Municipality.Name,
                                    MunicipalityId = x.RealityObject.Municipality.Id,
                                    Year = x.PlanYear,
                                    Address = x.RealityObject.Address,
                                    CeoId = x.Stage2.CommonEstateObject.Id,
                                    RoId = x.RealityObject.Id
                                })
                        .ToList();
            }
            else
            {
                var dataCorrection =
                    correctionStageService.GetAll()
                        .Where(x => x.Stage2.Stage3Version.ProgramVersion.IsMain)
                        .WhereIf(
                            municipalityIds.Any(),
                            x => municipalityIds.Contains(x.Stage2.Stage3Version.ProgramVersion.Municipality.Id))
                        .Where(
                            x => x.Stage2.Stage3Version.IndexNumber > 0 || x.Stage2.Stage3Version.Year < periodEndYear)
                        .Select(
                            x => new { x.Stage2.Stage3Version.Id, x.PlanYear, CeoId = x.Stage2.CommonEstateObject.Id })
                        .ToList()
                        .GroupBy(x => x.Id)
                        .ToDictionary(x => x.Key, y => y.Select(x => new { x.PlanYear, x.CeoId }).FirstOrDefault());

                data =
                    versionRecordService.GetAll()
                        .Where(x => x.ProgramVersion.IsMain)
                        .WhereIf(municipalityIds.Any(), x => municipalityIds.Contains(x.ProgramVersion.Municipality.Id))
                        .Where(x => correctionStageService.GetAll().Any(y => y.Stage2.Stage3Version.Id == x.Id))
                        .Select(
                            x =>
                            new
                                {
                                    x.Id,
                                    Municipality =
                                x.RealityObject.Municipality.ParentMo != null
                                    ? x.RealityObject.Municipality.ParentMo.Name
                                    : x.RealityObject.Municipality.Name,
                                    MunicipalityId =
                                x.RealityObject.Municipality.ParentMo != null
                                    ? x.RealityObject.Municipality.ParentMo.Id
                                    : x.RealityObject.Municipality.Id,
                                    x.RealityObject.Address,
                                    x.RealityObject.DateCommissioning,
                                    LastOverhaulYear = x.RealityObject.DateLastOverhaul,
                                    x.RealityObject.WallMaterial,
                                    FloorsCount = x.RealityObject.MaximumFloors,
                                    x.RealityObject.NumberEntrances,
                                    x.RealityObject.AreaMkd,
                                    x.RealityObject.AreaLivingNotLivingMkd,
                                    x.RealityObject.AreaLivingOwned,
                                    x.RealityObject.NumberLiving,
                                    x.RealityObject.CapitalGroup,
                                    x.Sum,
                                    RoId = x.RealityObject.Id
                                })
                        .ToList()
                        .Select(
                            x =>
                            new DataProxy
                                {
                                    Id = x.Id,
                                    Municipality = x.Municipality,
                                    MunicipalityId = x.MunicipalityId,
                                    Year = dataCorrection.ContainsKey(x.Id) ? dataCorrection[x.Id].PlanYear : 0,
                                    Address = x.Address,
                                    CeoId = dataCorrection.ContainsKey(x.Id) ? dataCorrection[x.Id].CeoId : 0,
                                    RoId = x.RoId
                                })
                        .Where(x => x.Year <= endYear && x.Year >= startYear)
                        .ToList();
            }

            var sectionYear = reportParams.ComplexReportParams.ДобавитьСекцию("sectionYear");
            var sectionMu = sectionYear.ДобавитьСекцию("sectionMo");
            var sectionRo = sectionMu.ДобавитьСекцию("sectionRo");
            var num = 0;
            var costCodes = new[] { 1, 2, 3, 4, 5, 6, 12, 13, 14, 16, 18 };
            var volCodes = new[] { 12, 13, 14, 16, 18 };

            var roSeInProgramDict =
                roSeInProgram.GetAll()
                    .Select(x => new { x.StructuralElement.StructuralElement.Id, RoId = x.Stage2.Stage3.RealityObject.Id, x.Stage2.Stage3.Year })
                    .ToList()
                    .GroupBy(x => string.Format("{0}_{1}", x.RoId, x.Year))
                    .ToDictionary(x => x.Key, x => x.Select(y => y.Id).ToList());

            var roSe = roSeService.GetAll()
                                .Select(
                                    x =>
                                    new RoStrElProxy
                                    {
                                        RoId = x.RealityObject.Id,
                                        SeId = x.StructuralElement.Id,
                                        Volume = x.Volume,
                                        CalculateBy = x.StructuralElement.CalculateBy,
                                        AreaLiving = x.RealityObject.AreaLiving,
                                        AreaLivingNotLivingMkd = x.RealityObject.AreaLivingNotLivingMkd,
                                        AreaMkd = x.RealityObject.AreaMkd
                                    })
                                .ToList();

            var seWorks = strElWorkDomain.GetAll()
                               .Select(
                                   x =>
                                   new
                                   {
                                       SeId = x.StructuralElement.Id,
                                       SeName = x.StructuralElement.Name,
                                       WorkName = x.Job.Work.Name,
                                       WorkCode = x.Job.Work.Code,
                                       x.Job.Work.TypeWork,
                                       JobId = x.Job.Id,
                                   })
                               .AsEnumerable()
                               .GroupBy(x => new { x.SeId, x.SeName })
                               .ToDictionary(x => x.Key, y => y.ToList());

            var workPrices =
                workPriceDomain.GetAll()
                    .Select(x => new { x.Job.Id, x.NormativeCost })
                    .AsEnumerable()
                    .GroupBy(x => x.Id)
                    .ToDictionary(x => x.Key, y => y.Select(x => x.NormativeCost).First());

            var totalCosts = new Dictionary<int, decimal>();
            var totalVvols = new Dictionary<int, decimal>();
            var tCost = 0m;
            var totalcost123456 = 0m;

            // Группируем по годам
            foreach (var yearData in data.GroupBy(x => x.Year).OrderBy(x => x.Key))
            {
                var year = yearData.Key;

                sectionYear.ДобавитьСтроку();
                sectionYear["year"] = year;
                var yearCosts = new Dictionary<int, decimal>();
                var yearVvols = new Dictionary<int, decimal>();
                var totalCostYear = 0m;
                var cost123456Year = 0m;

                foreach (var muData in yearData.GroupBy(x => x.Municipality).OrderBy(x => x.Key))
                {

                    sectionMu.ДобавитьСтроку();
                    sectionMu["moName"] = muData.Key;
                    var muCosts = new Dictionary<int, decimal>();
                    var muVvols = new Dictionary<int, decimal>();
                    var totalCostMu = 0m;
                    var cost123456Mu = 0m;

                    foreach (var roData in muData.GroupBy(x => x.RoId).OrderBy(x => x.Key))
                    {
                        sectionRo.ДобавитьСтроку();
                        sectionRo["num"] = ++num;
                        sectionRo["address"] = roData.FirstOrDefault().Return(x => x.Address);

                        #region WorkData
                        var roId = roData.Key;
                        var key = string.Format("{0}_{1}", roId, year);

                        //идентификаторы конструктивных элементов, которые связаны с этой записью 3 этапа
                        var seIds = roSeInProgramDict.ContainsKey(key) ? roSeInProgramDict[key] : new List<long>();
                        
                        var result = new List<RoWork>();

                        var roStrEls = roSe.Where(x => seIds.Contains(x.SeId) && x.RoId == roId).ToList();
                        
                        foreach (var strElWork in seWorks.Where(x => seIds.Contains(x.Key.SeId)))
                        {
                            var roStrEl = roStrEls.FirstOrDefault(x => x.SeId == strElWork.Key.SeId);
                            var calculateParam = this.GetCalculateParam(roStrEl);

                            foreach (var elWork in strElWork.Value)
                            {
                                result.Add(
                                    new RoWork
                                        {
                                            StructElement = elWork.SeName,
                                            WorkCode = elWork.WorkCode,
                                            WorkKind = elWork.WorkName,
                                            WorkType = elWork.TypeWork,
                                            Volume = roStrEl.Return(x => x.Volume),
                                            Sum =
                                                workPrices.ContainsKey(elWork.JobId)
                                                    ? calculateParam * workPrices[elWork.JobId]
                                                    : 0
                                        });
                            }
                        } 
                        #endregion

                        var costs = new Dictionary<int, decimal>();
                        var vols = new Dictionary<int, decimal>();

                        foreach (var costCode in costCodes)
                        {
                            int code = costCode;
                            var cost = result.Where(x => x.WorkCode == code.ToStr()).SafeSum(x => x.Sum);
                            costs.Add(code, cost);

                            if (muCosts.ContainsKey(code))
                            {
                                muCosts[code] += cost;
                            }
                            else
                            {
                                muCosts.Add(code, cost);
                            }

                            sectionRo["cost" + code] = cost;
                        }

                        foreach (var volCode in volCodes)
                        {
                            int code = volCode;
                            var vol = result.Where(x => x.WorkCode == code.ToStr()).SafeSum(x => x.Volume);
                            vols.Add(code, vol);

                            if (muVvols.ContainsKey(code))
                            {
                                muVvols[code] += vol;
                            }
                            else
                            {
                                muVvols.Add(code, vol);
                            }

                            sectionRo["vol" + code] = vol;
                        }

                        var totalCost = 0m;
                        var cost123456 = 0m;

                        foreach (var oneCost in costs)
                        {
                            if (new[] { 1, 2, 3, 4, 5, 6 }.Contains(oneCost.Key))
                            {
                                cost123456 += oneCost.Value;
                            }

                            totalCost += oneCost.Value;
                        }

                        totalCostMu += totalCost;
                        cost123456Mu += cost123456;

                        sectionRo["totalCost"] = totalCost;
                        sectionRo["cost123456"] = cost123456;

                    }

                    foreach (var costCode in costCodes)
                    {
                        int code = costCode;
                        decimal val = 0;

                        if (muCosts.ContainsKey(code))
                        {
                            val = muCosts[code];
                        }

                        sectionMu["costMo" + code] = val;

                        if (yearCosts.ContainsKey(code))
                        {
                            yearCosts[code] += val;
                        }
                        else
                        {
                            yearCosts.Add(code, val);
                        }
                    }

                    foreach (var volCode in volCodes)
                    {
                        int code = volCode;
                        decimal val = 0;

                        if (muVvols.ContainsKey(code))
                        {
                            val = muVvols[code];
                        }

                        sectionMu["volMo" + code] = val;

                        if (yearVvols.ContainsKey(code))
                        {
                            yearVvols[code] += val;
                        }
                        else
                        {
                            yearVvols.Add(code, val);
                        }
                    }

                    sectionMu["totalСostMo"] = totalCostMu;
                    sectionMu["costMo123456"] = cost123456Mu;

                    totalCostYear += totalCostMu;
                    cost123456Year += cost123456Mu;

                }

                foreach (var costCode in costCodes)
                {
                    int code = costCode;
                    decimal val = 0;

                    if (yearCosts.ContainsKey(code))
                    {
                        val = yearCosts[code];
                    }

                    sectionYear["costMo" + code + "Y"] = val;

                    if (totalCosts.ContainsKey(code))
                    {
                        totalCosts[code] += val;
                    }
                    else
                    {
                        totalCosts.Add(code, val);
                    }
                }

                foreach (var volCode in volCodes)
                {
                    int code = volCode;
                    decimal val = 0;

                    if (yearVvols.ContainsKey(code))
                    {
                        val = yearVvols[code];
                    }

                    sectionYear["volMo" + code + "Y"] = val;

                    if (totalVvols.ContainsKey(code))
                    {
                        totalVvols[code] += val;
                    }
                    else
                    {
                        totalVvols.Add(code, val);
                    }
                }

                sectionYear["totalCostMoY"] = totalCostYear;
                sectionYear["costMo123456Y"] = cost123456Year;

                tCost += totalCostYear;
                totalcost123456 += cost123456Year;
            }

            foreach (var costCode in costCodes)
            {
                int code = costCode;
                decimal val = 0;

                if (totalCosts.ContainsKey(code))
                {
                    val = totalCosts[code];
                }

                reportParams.SimpleReportParams["costSummary" + code] = val;
            }

            foreach (var volCode in volCodes)
            {
                int code = volCode;
                decimal val = 0;

                if (totalVvols.ContainsKey(code))
                {
                    val = totalVvols[code];
                }

                reportParams.SimpleReportParams["volSummary" + code] = val;
            }

            reportParams.SimpleReportParams["totalСostSummary"] = tCost;
            reportParams.SimpleReportParams["costSummary123456"] = totalcost123456;

        }


        #region Helpers
        private decimal GetCalculateParam(RoStrElProxy roStrEl)
        {
            if (roStrEl != null)
            {
                var strElCalculateBy = roStrEl.CalculateBy;
                switch (strElCalculateBy)
                {
                    case PriceCalculateBy.Volume:
                        return roStrEl.Volume;
                    case PriceCalculateBy.TotalArea:
                        return roStrEl.AreaMkd.HasValue ? roStrEl.AreaMkd.Value : 0;
                    case PriceCalculateBy.AreaLivingNotLivingMkd:
                        return roStrEl.AreaLivingNotLivingMkd.HasValue ? roStrEl.AreaLivingNotLivingMkd.Value : 0;
                    case PriceCalculateBy.LivingArea:
                        return roStrEl.AreaLiving.HasValue ? roStrEl.AreaLiving.Value : 0;
                }
            }

            return roStrEl.Return(x => x.Volume);
        }

        protected sealed class RoWork
        {
            public string StructElement;
            public string WorkCode;
            public string WorkKind;
            public TypeWork WorkType;
            public decimal Volume;
            public decimal Sum;
        }

        protected sealed class RoStrElProxy
        {
            public long RoId;

            public long SeId;

            public decimal Volume;

            public PriceCalculateBy CalculateBy;

            public decimal? AreaLiving;

            public decimal? AreaMkd;

            public decimal? AreaLivingNotLivingMkd;
        }

        protected sealed class DataProxy
        {
            public long Id;

            public long RoId;

            public long MunicipalityId;

            public string Municipality;

            public int Year;

            public string Address;

            public DateTime? DateCommissioning;

            public DateTime? LastOverhaulYear;

            public WallMaterial WallMaterial;

            public CapitalGroup CapitalGroup;

            public int? FloorsCount;

            public int? NumberEntrances;

            public decimal? AreaMkd;

            public decimal? AreaLivingNotLivingMkd;

            public decimal? AreaLivingOwned;

            public int? NumberLiving;

            public long CeoId;

            public decimal Sum;
        } 
        #endregion
    }
}