namespace Bars.Gkh.Overhaul.Hmao.Reports
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using B4.Modules.Reports;

    using Bars.B4.Utils;
    using Bars.Gkh.ConfigSections.Overhaul.Enums;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Domain.CollectionExtensions;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Entities.Dicts;
    using Bars.Gkh.Overhaul.Entities;
    using Bars.Gkh.Overhaul.Hmao.ConfigSections;
    using Bars.Gkh.Overhaul.Hmao.Entities;

    using Castle.Windsor;
    using Enums;
    using Gkh.Utils;

    public class ListByManyApartmentsHousesOverhaul : BasePrintForm
    {
        #region Свойства

        private int startYear;
        private int endYear;
        private long[] municipalityIds;
        public IWindsorContainer Container { get; set; }

        public ListByManyApartmentsHousesOverhaul()
            : base(new ReportTemplateBinary(Properties.Resources.ListByManyApartmentsHousesOverhaul))
        {
        }

        public override string RequiredPermission
        {
            get { return string.Empty; }
        }

        public override string Name
        {
            get { return "Перечень многоквартирных домов (на основе Долгосрочной программы)"; }
        }

        public override string Desciption
        {
            get { return "Перечень многоквартирных домов (на основе Долгосрочной программы)"; }
        }

        public override string GroupName
        {
            get { return "Формы для фонда"; }
        }

        public override string ParamsController
        {
            get { return "B4.controller.report.ListByManyApartmentsHousesOverhaul"; }
        }

        protected sealed class DataProxy
        {
            public long Id;
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

        protected virtual decimal GetPartialCost(DataProxy data)
        {
            return (data.AreaMkd.HasValue && data.AreaMkd.Value != 0M ? data.Sum / data.AreaMkd.Value : data.Sum).RoundDecimal(2);
        }

        protected virtual decimal GetPartialCostTotal(List<DataProxy> data)
        {
            var area = data.SafeSum(x => x.AreaMkd.HasValue ? x.AreaMkd.Value : 0);
            var sum = data.SafeSum(x => x.Sum);

            return (area != 0M ? sum / area : sum).RoundDecimal(2);
        }

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
            var subsidyRecordVersionService = Container.Resolve<IDomainService<SubsidyRecordVersion>>();
            var workPriceDomain = Container.Resolve<IDomainService<WorkPrice>>();
            var structElWorkDomain = Container.Resolve<IDomainService<StructuralElementWork>>();

            var ceoWorks = structElWorkDomain.GetAll()
                .Select(y => new
                {
                    CeoId = y.StructuralElement != null && y.StructuralElement.Group != null
                    && y.StructuralElement.Group.CommonEstateObject != null ? y.StructuralElement.Group.CommonEstateObject.Id : 0,
                    WorkId = y.Job != null ? y.Job.Id : 0,
                    CalculateBy = y.StructuralElement != null ? y.StructuralElement.CalculateBy : PriceCalculateBy.Volume
                })
                                 .ToList()
                .GroupBy(x => x.CeoId)
                .ToDictionary(x => x.Key, y => y.ToList());

            var workPrices =
                workPriceDomain.GetAll()
                    .Select(
                        x => new
                        {
                            WorkId = x.Job.Id,
                            x.Year,
                            x.SquareMeterCost,
                            x.NormativeCost,
                            MuId = x.Municipality != null ? x.Municipality.Id : 0,
                            CapitalGroupId = x.CapitalGroup != null ? x.CapitalGroup.Id : 0
                        })
                    .ToList()
                    .GroupBy(x => string.Format("{0}_{1}_{2}", x.WorkId, x.Year, x.MuId))
                    .ToDictionary(x => x.Key, x => x);

            var config = Container.GetGkhConfig<OverhaulHmaoConfig>();
            var groupByRoPeriod = config.GroupByRoPeriod;
            var periodEndYear = config.ProgrammPeriodEnd;
            var periodStartYear = config.ProgrammPeriodStart;
            var workPriceCalcYear = config.WorkPriceCalcYear;
            var workPriceDetermineType = config.WorkPriceDetermineType;

            List<DataProxy> data;

            var subsidy =
                subsidyRecordVersionService.GetAll()
                    .WhereIf(municipalityIds.Any(), x => municipalityIds.Contains(x.Version.Municipality.Id))
                    .Select(
                        x =>
                        new
                        {
                            x.Id,
                            MuId = x.Version.Municipality.Id,
                            x.SubsidyYear,
                            x.BudgetRegion,
                            x.BudgetMunicipality,
                            x.BudgetFcr,
                            x.CorrectionFinance
                        })
                    .ToList()
                    .ToDictionary(x => string.Format("{0}_{1}-{2}", x.MuId, x.SubsidyYear, x.Id), x => x);

            if (groupByRoPeriod == 0)
            {
                data =
                    correctionStageService.GetAll()
                        .Where(x => x.Stage2.Stage3Version.ProgramVersion.IsMain)
                        .Where(x => municipalityIds.Contains(x.Stage2.Stage3Version.ProgramVersion.Municipality.Id))
                        .Where(
                            x => x.Stage2.Stage3Version.IndexNumber > 0 || x.Stage2.Stage3Version.Year < periodEndYear)
                        .Where(x => x.PlanYear <= endYear && x.PlanYear >= startYear)
                        .Select(x => new DataProxy
                        {
                            Municipality = x.RealityObject.Municipality.Name,
                            MunicipalityId = x.RealityObject.Municipality.Id,
                            Year = x.PlanYear,
                            Address = x.RealityObject.Address,
                            DateCommissioning = x.RealityObject.DateCommissioning,
                            LastOverhaulYear = x.RealityObject.DateLastOverhaul,
                            WallMaterial = x.RealityObject.WallMaterial,
                            FloorsCount = x.RealityObject.MaximumFloors,
                            NumberEntrances = x.RealityObject.NumberEntrances,
                            AreaMkd = x.RealityObject.AreaMkd,
                            AreaLivingNotLivingMkd = x.RealityObject.AreaLivingNotLivingMkd,
                            AreaLivingOwned = x.RealityObject.AreaLivingOwned,
                            NumberLiving = x.RealityObject.NumberLiving,
                            Sum = x.Stage2.Sum,
                            CapitalGroup = x.RealityObject.CapitalGroup,
                            CeoId = x.Stage2.CommonEstateObject.Id
                        })
                        .ToList();
            }
            else
            {
                var dataCorrection =
                    correctionStageService.GetAll()
                        .Where(x => x.Stage2.Stage3Version.ProgramVersion.IsMain)
                        .WhereIf(municipalityIds.Any(), x => municipalityIds.Contains(x.Stage2.Stage3Version.ProgramVersion.Municipality.Id))
                        .Where(x => x.Stage2.Stage3Version.IndexNumber > 0 || x.Stage2.Stage3Version.Year < periodEndYear)
                        .Select(x => new { x.Stage2.Stage3Version.Id, x.PlanYear, CeoId = x.Stage2.CommonEstateObject.Id })
                        .ToList()
                        .GroupBy(x => x.Id)
                        .ToDictionary(x => x.Key, y => y.Select(x => new { x.PlanYear, x.CeoId }).FirstOrDefault());

                data =
                    versionRecordService.GetAll()
                        .Where(x => x.ProgramVersion.IsMain)
                        .WhereIf(municipalityIds.Any(), x => municipalityIds.Contains(x.ProgramVersion.Municipality.Id))
                        .Where(x => correctionStageService.GetAll().Any(y => y.Stage2.Stage3Version.Id == x.Id))
                        .Select(x => new
                        {
                            x.Id,
                            Municipality = x.RealityObject.Municipality.ParentMo != null ? x.RealityObject.Municipality.ParentMo.Name : x.RealityObject.Municipality.Name,
                            MunicipalityId = x.RealityObject.Municipality.ParentMo != null ? x.RealityObject.Municipality.ParentMo.Id : x.RealityObject.Municipality.Id,
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
                            x.Sum
                        })
                        .ToList()
                        .Select(x => new DataProxy
                        {
                            Id = x.Id,
                            Municipality = x.Municipality,
                            MunicipalityId = x.MunicipalityId,
                            Year = dataCorrection.ContainsKey(x.Id) ? dataCorrection[x.Id].PlanYear : 0,
                            Address = x.Address,
                            DateCommissioning = x.DateCommissioning,
                            LastOverhaulYear = x.LastOverhaulYear,
                            WallMaterial = x.WallMaterial,
                            FloorsCount = x.FloorsCount,
                            NumberEntrances = x.NumberEntrances,
                            AreaMkd = x.AreaMkd,
                            AreaLivingNotLivingMkd = x.AreaLivingNotLivingMkd,
                            AreaLivingOwned = x.AreaLivingOwned,
                            NumberLiving = x.NumberLiving,
                            Sum = x.Sum,
                            CapitalGroup = x.CapitalGroup,
                            CeoId = dataCorrection.ContainsKey(x.Id) ? dataCorrection[x.Id].CeoId : 0,
                        })
                        .Where(x => x.Year <= endYear && x.Year >= startYear)
                        .ToList();
            }

            #region Вывод Данных

            var yearSection = reportParams.ComplexReportParams.ДобавитьСекцию("year");
            var muSection = yearSection.ДобавитьСекцию("municipality");
            var section = muSection.ДобавитьСекцию("section");
            var num = 0;
            var allAreaMkd = 0M;
            var allAreaLivNotLivMkd = 0M;
            var allAreaLivOwned = 0M;
            var allCountPerson = 0;
            var allSum = 0M;
            var allFundResource = 0M;
            var allBudgetSubject = 0M;
            var allBudgetMu = 0M;
            var allOwnerResource = 0M;
            var allLimCost = 0M;

            var subsidyKeys = subsidy.Keys;

            foreach (var dataProxy in data.GroupBy(x => x.Year).OrderBy(x => x.Key))
            {
                yearSection.ДобавитьСтроку();
                yearSection["year"] = dataProxy.Key;

                var yearAreaMkd = 0M;
                var yearAreaLivNotLivMkd = 0M;
                var yearAreaLivOwned = 0M;
                var yearCountPerson = 0;
                var yearSum = 0M;
                var yearFundResource = 0M;
                var yearBudgetSubject = 0M;
                var yearBudgetMu = 0M;
                var yearOwnerResource = 0M;
                var yearLimCost = 0M;

                foreach (var proxy in dataProxy.GroupBy(x => x.Municipality).OrderBy(x => x.Key))
                {
                    muSection.ДобавитьСтроку();
                    muSection["МунОбр"] = proxy.Key;
                    var muAreaMkd = 0M;
                    var muAreaLivNotLivMkd = 0M;
                    var muAreaLivOwned = 0M;
                    var muCountPerson = 0;
                    var muSum = 0M;
                    var muFundResource = 0M;
                    var muBudgetSubject = 0M;
                    var muBudgetMu = 0M;
                    var muOwnerResource = 0M;
                    var muLimCost = 0M;
                    var muId = proxy.FirstOrDefault().Return(x => x.MunicipalityId);
                    var subsidyKey = string.Format(
                        "{0}_{1}",
                        muId,
                        dataProxy.Key);
                    
                    var subsidyListIds = new List<string>();

                    var keyReal = string.Empty;
                    foreach (var key in subsidyKeys)
                    {
                        var index = key.IndexOf("-", StringComparison.Ordinal);
                        var id = key.Remove(index);

                        subsidyListIds.Add(id);

                        if (subsidyListIds.Any() && subsidyKey == id)
                        {
                            keyReal = key;
                            break;
                        }
                    }
                    
                    var subsidyRec = subsidyListIds.Contains(subsidyKey) ? subsidy[keyReal] : null;

                    foreach (var ro in proxy)
                    {
                        ++num;
                        section.ДобавитьСтроку();
                        section["номер"] = num;
                        section["адресМкд"] = ro.Address;
                        section["датаВвода"] = ro.DateCommissioning.HasValue ? ro.DateCommissioning.Value.Return(x => x.Year.ToStr()) : string.Empty;
                        section["датаКонца"] = ro.LastOverhaulYear.HasValue ? ro.LastOverhaulYear.Value.Return(x => x.Year.ToStr()) : string.Empty;
                        section["материалСтен"] = ro.Return(x => x.WallMaterial).Return(x => x.Name);
                        section["колЭтажей"] = ro.FloorsCount.HasValue ? ro.FloorsCount.Value.ToStr() : string.Empty;
                        section["колПодъездов"] = ro.NumberEntrances.HasValue ? ro.NumberEntrances.Value.ToStr() : string.Empty;
                        section["ОбщПлощВсего"] = ro.AreaMkd.HasValue ? ro.AreaMkd.Value.ToStr() : string.Empty;
                        muAreaMkd += ro.AreaMkd.HasValue ? ro.AreaMkd.Value : 0;

                        section["ОбщПлощПомещВсего"] = ro.AreaLivingNotLivingMkd.HasValue ? ro.AreaLivingNotLivingMkd.Value.ToStr() : string.Empty;
                        muAreaLivNotLivMkd += ro.AreaLivingNotLivingMkd.HasValue ? ro.AreaLivingNotLivingMkd.Value : 0;

                        section["ОбщПлощЖил"] = ro.AreaLivingOwned.HasValue ? ro.AreaLivingOwned.Value.ToStr() : string.Empty;
                        muAreaLivOwned += ro.AreaLivingOwned.HasValue ? ro.AreaLivingOwned.Value : 0;

                        section["КолЖител"] = ro.NumberLiving.HasValue ? ro.NumberLiving.Value.ToStr() : string.Empty;
                        muCountPerson += ro.NumberLiving.HasValue ? ro.NumberLiving.Value : 0;

                        section["ИтогоСтоим"] = ro.Sum;
                        muSum += ro.Sum;

                        section["УделСтоим"] = GetPartialCost(ro);

                        if (ceoWorks.ContainsKey(ro.CeoId))
                        {
                            var predSum = 0m;
                            var works = ceoWorks[ro.CeoId];
                            foreach (var work in works)
                            {
                                var year = workPriceCalcYear == WorkPriceCalcYear.First
                                               ? periodStartYear
                                               : dataProxy.Key;

                                var key = string.Format("{0}_{1}_{2}", work.WorkId, year, muId);

                                if (workPrices.ContainsKey(key))
                                {
                                    var workPrice = workPrices[key];
                                    if (workPriceDetermineType == WorkPriceDetermineType.WithCapitalGroup)
                                    {
                                        var wp =
                                            workPrice.FirstOrDefault(
                                                x => x.CapitalGroupId == ro.CapitalGroup.Return(y => y.Id));
                                        if (wp != null)
                                        {
                                            predSum += work.CalculateBy == PriceCalculateBy.Volume
                                                           ? wp.NormativeCost
                                                           : wp.SquareMeterCost.HasValue ? wp.SquareMeterCost.Value : 0m;
                                        }
                                    }
                                    else
                                    {
                                        var wp = workPrice.FirstOrDefault();
                                        if (wp != null)
                                        {
                                            predSum += work.CalculateBy == PriceCalculateBy.Volume
                                                           ? wp.NormativeCost
                                                           : wp.SquareMeterCost.HasValue ? wp.SquareMeterCost.Value : 0m;
                                        }
                                    }
                                }
                            }

                            section["ПредСтоим"] = predSum;
                            muLimCost += predSum;
                        }

                        var sumFund = 0M;
                        var sumReg = 0M;
                        var sumMu = 0M;
                        var sumTsj = 0M;
                        if (subsidyRec != null && subsidyRec.CorrectionFinance != 0)
                        {
                            sumFund = subsidyRec.BudgetFcr / subsidyRec.CorrectionFinance * ro.Sum;
                            section["СтоимФонд"] = sumFund;

                            sumReg = subsidyRec.BudgetRegion / subsidyRec.CorrectionFinance * ro.Sum;
                            section["СтоимФедБюдж"] = sumReg;

                            sumMu = subsidyRec.BudgetMunicipality / subsidyRec.CorrectionFinance * ro.Sum;
                            section["СтоимМестБюдж"] = sumMu;

                            sumTsj = ro.Sum - (sumFund + sumMu + sumReg);
                            section["СтоимТСЖ"] = sumTsj;
                        }
                        muFundResource += sumFund;
                        muBudgetSubject += sumReg;
                        muBudgetMu += sumMu;
                        muOwnerResource += sumTsj;
                    }

                    muSection["ИтОбщПлщВс"] = muAreaMkd;
                    yearAreaMkd += muAreaMkd;

                    muSection["ИтОбщПлщПомщВс"] = muAreaLivNotLivMkd;
                    yearAreaLivNotLivMkd += muAreaLivNotLivMkd;

                    muSection["ИтОбщПлщЖил"] = muAreaLivOwned;
                    yearAreaLivOwned += muAreaLivOwned;

                    muSection["ИтКолЖит"] = muCountPerson;
                    yearCountPerson += muCountPerson;

                    muSection["ПолнИтСтм"] = muSum;
                    yearSum += muSum;

                    muSection["ИтСтмФонд"] = muFundResource;
                    yearFundResource += muFundResource;

                    muSection["ИтСтмФедБдж"] = muBudgetSubject;
                    yearBudgetSubject += muBudgetSubject;

                    muSection["ИтСтмМстБдж"] = muBudgetMu;
                    yearBudgetMu += muBudgetMu;

                    muSection["ИтСтмТСЖ"] = muOwnerResource;
                    yearOwnerResource += muOwnerResource;

                    muSection["ИтУдСтм"] = GetPartialCostTotal(proxy.ToList());

                    muSection["ИтПрдСтм"] = muLimCost;
                    yearLimCost += muLimCost;
                }

                yearSection["ИтОбщПлщВсГод"] = yearAreaMkd;
                allAreaMkd += yearAreaMkd;

                yearSection["ИтОбщПлщПомщВсГод"] = yearAreaLivNotLivMkd;
                allAreaLivNotLivMkd += yearAreaLivNotLivMkd;

                yearSection["ИтОбщПлщЖилГод"] = yearAreaLivOwned;
                allAreaLivOwned += yearAreaLivOwned;

                yearSection["ИтКолЖитГод"] = yearCountPerson;
                allCountPerson += yearCountPerson;

                yearSection["ПолнИтСтмГод"] = yearSum;
                allSum += yearSum;

                yearSection["ИтСтмФондГод"] = yearFundResource;
                allFundResource += yearFundResource;

                yearSection["ИтСтмФедБджГод"] = yearBudgetSubject;
                allBudgetSubject += yearBudgetSubject;

                yearSection["ИтСтмМстБджГод"] = yearBudgetMu;
                allBudgetMu += yearBudgetMu;

                yearSection["ИтСтмТСЖГод"] = yearOwnerResource;
                allOwnerResource += yearOwnerResource;

                yearSection["ИтУдСтмГод"] = GetPartialCostTotal(dataProxy.ToList());

                yearSection["ИтПрдСтмГод"] = yearLimCost;
                allLimCost += yearLimCost;

            }

            reportParams.SimpleReportParams["ВсОбщПлщВс"] = allAreaMkd;
            reportParams.SimpleReportParams["ВсОбщПлщПмщ"] = allAreaLivNotLivMkd;
            reportParams.SimpleReportParams["ВсОбщПлщЖил"] = allAreaLivOwned;
            reportParams.SimpleReportParams["ВсКолЖит"] = allCountPerson;
            reportParams.SimpleReportParams["ВсИтСтм"] = allSum;
            reportParams.SimpleReportParams["ВсСтмФонд"] = allFundResource;
            reportParams.SimpleReportParams["ВсСтмФедБдж"] = allBudgetSubject;
            reportParams.SimpleReportParams["ВсСтмМстБдж"] = allBudgetMu;
            reportParams.SimpleReportParams["ВсСтмТСЖ"] = allOwnerResource;
            reportParams.SimpleReportParams["ВсУдСтм"] = 0;
            reportParams.SimpleReportParams["ВсПрдСтм"] = allLimCost;
            #endregion
        }
    }
}