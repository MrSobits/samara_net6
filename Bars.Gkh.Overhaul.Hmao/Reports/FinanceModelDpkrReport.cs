namespace Bars.Gkh.Overhaul.Hmao.Reports
{
    using System.Linq;
    using System;
    using Bars.Gkh.Domain.CollectionExtensions;
    using Bars.Gkh.Entities;
    using Bars.B4;
    using B4.Modules.Reports;

    using Bars.B4.Utils;
    using Bars.Gkh.Overhaul.Domain;
    using Bars.Gkh.Overhaul.Hmao.ConfigSections;
    using Bars.Gkh.Overhaul.Hmao.Entities;
    using Castle.Windsor;
    using Gkh.Entities.RealEstateType;
    using Gkh.Utils;
    using Overhaul.Entities;

    public class FinanceModelDpkrReport : BasePrintForm
    {
        #region Dependency injection members

        public IDomainService<RealEstateTypeMunicipality> RealEstateTypeMuDomain { get; set; }

        public IDomainService<RealEstateTypeRate> RealEstateTypeRateDomain { get; set; }

        public IDomainService<VersionRecord> VersionRecordDomain { get; set; }

        public IDomainService<SubsidyRecordVersion> SubsidyRecordVersionDomain { get; set; }

        public IDomainService<StructuralElementWork> StructElWorkDomain { get; set; }

        public IDomainService<VersionRecordStage1> VersionStage1Domain { get; set; }

        public IDomainService<Municipality> MunicipalityDomain { get; set; }

        public IDomainService<DpkrCorrectionStage2> DpkrCorrectionDomain { get; set; }

        public IDomainService<PublishedProgramRecord> PublishedProgramDomain { get; set; }
        #endregion

        public FinanceModelDpkrReport()
            : base(new ReportTemplateBinary(Properties.Resources.FinanceModelDpkr))
        {

        }

        public IWindsorContainer Container { get; set; }

        public override string Name
        {
            get
            {
                return "Финансовая модель формирования региональной программы по МО";
            }
        }

        public override string Desciption
        {
            get
            {
                return "Финансовая модель формирования региональной программы по МО";
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
                return "B4.controller.report.FinanceModelDpkr";
            }
        }

        public override string RequiredPermission
        {
            get
            {
                return "Reports.GkhOverhaul.FinanceModelDpkr";
            }
        }

        private long municipalityId;

        private int groupPeriod;

        public override void SetUserParams(BaseParams baseParams)
        {
            municipalityId = baseParams.Params.GetAs<long>("municipalityId");

            groupPeriod = baseParams.Params.GetAs("groupPeriod", 6);
        }

        public override string ReportGenerator { get; set; }

        public override void PrepareReport(ReportParams reportParams)
        {
            var config = Container.GetGkhConfig<OverhaulHmaoConfig>();
            var minYear = config.ProgrammPeriodStart;
            var maxYear = config.ProgrammPeriodEnd;

            if (minYear == 0 || maxYear == 0 || minYear > maxYear)
            {
                throw new ReportParamsException("Проверьте правильность параметров расчета дпкр");
            }

            if ((maxYear - minYear + 1) % groupPeriod == 1) 
            {
                throw new ReportParamsException("Неверно задан период группировки. Период ДПКР должен быть кратен выбранному количеству лет.");
            }

            CreateVerticalColums(reportParams, minYear, maxYear);

            var worksDict = StructElWorkDomain.GetAll()
                    .Select(x => new
                    {
                        structId = x.StructuralElement.Id,
                        WorkId = x.Job.Work.Id
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.structId)
                    .ToDictionary(x => x.Key, y => y.Count());

            var realEstType = RealEstateTypeMuDomain.GetAll().Where(x => x.Municipality.Id == municipalityId).Select(x => x.RealEstateType).FirstOrDefault();

            var rates = RealEstateTypeRateDomain.GetAll()
                .Where(x => x.Year >= minYear && x.Year <= maxYear)
                .Where(x => x.RealEstateType == realEstType)
                .Select(x => new
                {
                    x.Year,
                    x.SociallyAcceptableRate
                })
                .AsEnumerable()
                .GroupBy(x => x.Year)
                .ToDictionary(x => x.Key, y => y.Select(x => x.SociallyAcceptableRate).FirstOrDefault());

            var realObjsInfoByYear = DpkrCorrectionDomain.GetAll()
                 .Where(x => x.PlanYear >= minYear && x.PlanYear <= maxYear && x.RealityObject.Municipality.Id == municipalityId)
                 .Select(x => new
                 {
                     x.PlanYear,
                     x.RealityObject.Id
                 })
                 .AsEnumerable()
                 .Distinct()
                 .GroupBy(x => x.PlanYear)
                 .ToDictionary(x => x.Key, y => y.Count());

            var realObjsArea = VersionRecordDomain.GetAll()
                .Where(x => x.ProgramVersion.IsMain && x.ProgramVersion.Municipality.Id == municipalityId)
                .Where(x => PublishedProgramDomain.GetAll().Any(y => y.Stage2.Stage3Version.Id == x.Id))
                .Select(x => new
                {
                    x.RealityObject.Id,
                    x.RealityObject.AreaLivingNotLivingMkd
                })
                .AsEnumerable()
                .Distinct()
                .SafeSum(x => x.AreaLivingNotLivingMkd.ToDecimal());

            realObjsArea = (realObjsArea/1000).RoundDecimal(2);

            var subsidyInfo = SubsidyRecordVersionDomain.GetAll()
                .Where(x => x.SubsidyYear >= minYear && x.SubsidyYear <= maxYear)
                .Where(x => x.Version.Municipality.Id == municipalityId && x.Version.IsMain)
                .Select(x => new
                {
                    x.Id,
                    x.SubsidyYear,
                    x.BudgetFcr,
                    x.PlanOwnerPercent,
                    x.NotReduceSizePercent,
                    x.BudgetCr,
                    x.CorrectionFinance
                })
                .AsEnumerable()
                .Select(x => new
                {
                    x.Id,
                    x.SubsidyYear,
                    BudgetFcr = (x.BudgetFcr / 1000).RoundDecimal(2),
                    x.PlanOwnerPercent,
                    x.NotReduceSizePercent,
                    BudgetCr = (x.BudgetCr / 1000).RoundDecimal(2),
                    CorrectionFinance = (x.CorrectionFinance / 1000).RoundDecimal(2),
                })
                .ToDictionary(x => x.SubsidyYear);

            var workCountByStage2 = VersionStage1Domain.GetAll()
                .Where(x => DpkrCorrectionDomain.GetAll().Any(y => y.Stage2.Id == x.Stage2Version.Id))
                .Where(x => x.Year >= minYear && x.Year <= maxYear
                    && x.StructuralElement.RealityObject.Municipality.Id == municipalityId)
                .Select(x => new
                {
                    x.Stage2Version.Id,
                    SeId = x.StructuralElement.StructuralElement.Id
                })
                .AsEnumerable()
                .Select(x => new
                {
                    x.Id,
                    WorkCount = worksDict.ContainsKey(x.SeId) ? worksDict[x.SeId] : 0,
                })
                .GroupBy(x => x.Id)
                .ToDictionary(x => x.Key, y => y.Sum(x => x.WorkCount));

            var workCountByCorrection = DpkrCorrectionDomain.GetAll()
                .Where(x => x.PlanYear >= minYear && x.PlanYear <= maxYear
                    && x.RealityObject.Municipality.Id == municipalityId)
                .Select(x => new
                {
                    St2Id = x.Stage2.Id,
                    x.PlanYear
                })
                .AsEnumerable()
                .Select(x => new
                {
                    x.PlanYear,
                    WorkCount = workCountByStage2.ContainsKey(x.St2Id) ? workCountByStage2[x.St2Id] : 0
                })
                .GroupBy(x => x.PlanYear)
                .ToDictionary(x => x.Key, y => y.Sum(x => x.WorkCount));

            reportParams.SimpleReportParams["ReportDate"] = DateTime.Now.ToShortDateString();
            reportParams.SimpleReportParams["Municipality"] = MunicipalityDomain.GetAll().Where(x => x.Id == municipalityId).Select(x => x.Name).FirstOrDefault();
            reportParams.SimpleReportParams["DpkrRoCount"] = DpkrCorrectionDomain.GetAll()
                .Where(x => x.PlanYear >= minYear && x.PlanYear <= maxYear && x.RealityObject.Municipality.Id == municipalityId)
                .Select(x => x.RealityObject.Id).Distinct().Count();

            var periodCnt = 0;
            var periodRoCount = 0;
            var periodWorkCount = 0;
            var periodCorrectionFinance = 0M;
            for (var year = minYear; year <= maxYear; year++)
            {
                if (periodCnt % groupPeriod == 0)
                {
                    periodRoCount = 0;
                    periodWorkCount = 0;
                    periodCorrectionFinance = 0M;
                    for (int i = year; i < year + groupPeriod; i++)
                    {
                        periodCorrectionFinance += subsidyInfo.ContainsKey(i) ? subsidyInfo[i].CorrectionFinance : 0;
                        periodWorkCount += workCountByCorrection.ContainsKey(i) ? workCountByCorrection[i] : 0;
                        periodRoCount += realObjsInfoByYear.ContainsKey(i) ? realObjsInfoByYear[i] : 0;
                    }
                }

                var isSubsidyInfoExist = subsidyInfo.ContainsKey(year);

                reportParams.SimpleReportParams[GetVariable("Tarif", year)] = rates.ContainsKey(year) ? rates[year] : null;
                reportParams.SimpleReportParams[GetVariable("AreaLivingNotLivingMkd", year)] = realObjsArea;
                reportParams.SimpleReportParams[GetVariable("OwnerCollection", year)] = isSubsidyInfoExist ? subsidyInfo[year].PlanOwnerPercent : 0;
                reportParams.SimpleReportParams[GetVariable("NotReduceSizePercent", year)] = isSubsidyInfoExist ? subsidyInfo[year].NotReduceSizePercent : 0;
                reportParams.SimpleReportParams[GetVariable("BudgetCr", year)] = isSubsidyInfoExist ? subsidyInfo[year].BudgetCr : 0;
                reportParams.SimpleReportParams[GetVariable("BudgetFcr", year)] = isSubsidyInfoExist ? subsidyInfo[year].BudgetFcr : 0;
                reportParams.SimpleReportParams[GetVariable("CorrectionFinance", year)] = isSubsidyInfoExist ? subsidyInfo[year].CorrectionFinance : 0;
                reportParams.SimpleReportParams[GetVariable("CorrectionFinancePeriod", year)] = periodCorrectionFinance;
                reportParams.SimpleReportParams[GetVariable("RoCount", year)] = realObjsInfoByYear.ContainsKey(year) ? realObjsInfoByYear[year] : 0;
                reportParams.SimpleReportParams[GetVariable("RoCountPeriod", year)] = periodRoCount;
                reportParams.SimpleReportParams[GetVariable("WorkCount", year)] = workCountByCorrection.ContainsKey(year) ? workCountByCorrection[year] : 0;
                reportParams.SimpleReportParams[GetVariable("WorkCountPeriod", year)] = periodWorkCount;

                periodCnt++;
            }
        }

        private string GetVariable(string commonVariable, int year)
        {
            return string.Format("{0}{1}", commonVariable, year);
        }

        // заполнение вертикальной секции
        private void CreateVerticalColums(ReportParams reportParams, int minYear, int maxYear)
        {
            var verticalSectionPeriod = reportParams.ComplexReportParams.ДобавитьСекцию("vertSectionPeriod");
            var verticalSection = new Section();
            var periodCnt = 0;

            for (var i = minYear; i <= maxYear; i++)
            {
                if (periodCnt % groupPeriod == 0)
                {
                    verticalSectionPeriod.ДобавитьСтроку();
                    verticalSection = verticalSectionPeriod.ДобавитьСекцию("vertSection");
                }

                CreateColumns(verticalSection, i);

                periodCnt++;
            }

        }

        private void CreateColumns(Section verticalSection, int? year)
        {
            verticalSection.ДобавитьСтроку();
            verticalSection["Year"] = year;
            verticalSection["Tarif"] = string.Format("$Tarif{0}$", year);
            verticalSection["AreaLivingNotLivingMkd"] = string.Format("$AreaLivingNotLivingMkd{0}$", year);
            verticalSection["OwnerCollection"] = string.Format("$OwnerCollection{0}$", year);
            verticalSection["NotReduceSizePercent"] = string.Format("$NotReduceSizePercent{0}$", year);
            verticalSection["BudgetCr"] = string.Format("$BudgetCr{0}$", year);
            verticalSection["BudgetFcr"] = string.Format("$BudgetFcr{0}$", year);
            verticalSection["CorrectionFinance"] = string.Format("$CorrectionFinance{0}$", year);
            verticalSection["CorrectionFinancePeriod"] = string.Format("$CorrectionFinancePeriod{0}$", year);
            verticalSection["RoCount"] = string.Format("$RoCount{0}$", year);
            verticalSection["RoCountPeriod"] = string.Format("$RoCountPeriod{0}$", year);
            verticalSection["WorkCount"] = string.Format("$WorkCount{0}$", year);
            verticalSection["WorkCountPeriod"] = string.Format("$WorkCountPeriod{0}$", year);
        }
    }
}