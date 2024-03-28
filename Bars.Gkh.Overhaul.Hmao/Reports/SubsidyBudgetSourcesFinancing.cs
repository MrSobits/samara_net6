namespace Bars.Gkh.Overhaul.Hmao.Reports
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using B4.Modules.Reports;

    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Overhaul.Hmao.ConfigSections;
    using Bars.Gkh.Overhaul.Hmao.Entities;

    using Castle.Windsor;
    using Gkh.Utils;

    public class SubsidyBudgetSourcesFinancing : BasePrintForm
    {
        public IRepository<Municipality> MunicipalityDomain { get; set; }

        public IDomainService<SubsidyRecordVersion> SubsidyRecordVersionDomain { get; set; }

        public SubsidyBudgetSourcesFinancing()
            : base(new ReportTemplateBinary(Properties.Resources.SubsidyBudgetSourcesFinancing))
        {

        }

        public IWindsorContainer Container { get; set; }

        public override string Name
        {
            get
            {
                return "Итоговый бюджет на КР по разрезам финансирования";
            }
        }

        public override string Desciption
        {
            get
            {
                return "Итоговый бюджет на КР по разрезам финансирования";
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
                return "B4.controller.report.SubsidyBudgetSrcFinanc";
            }
        }

        public override string RequiredPermission
        {
            get
            {
                return "Ovrhl.SubsidyBudgetSrcFinanc";
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
            // Для каждого вида резерва финансирвоания сохздаем колоку с годом
            // просто нехотел придумывать километровые переменные для кждого бюджета
            // 1 - Средства собстственников
            // 2 - Региональный бюджет
            // 3 - Бюджет муниципальных образований
            // 4 - Средства ГК ФСР ЖКХ
            // 5 - Средства иных источников
            // 6 - Итоговый бюджет на капремонт

            var config = Container.GetGkhConfig<OverhaulHmaoConfig>();
            var minYear = config.ProgrammPeriodStart;
            var maxYear = config.ProgrammPeriodEnd;

            if (minYear == 0 || maxYear == 0 || minYear > maxYear)
            {
                throw new ReportParamsException("Проверьте правильность параметров расчета дпкр");
            }

            // Раскрываем вертикальную секцию
            CreateVerticalColums(reportParams, minYear, maxYear);

            var muList = MunicipalityDomain.GetAll()
                          .WhereIf(municipalityIds.Any(), x => municipalityIds.Contains(x.Id))
                          .OrderBy(x => x.Name).Select(x => new { x.Id, x.Name }).ToList();

            var dataList =
                SubsidyRecordVersionDomain.GetAll()
                    .Where(x => x.SubsidyRecord.Municiaplity != null)
                    .WhereIf(municipalityIds.Any(), x => municipalityIds.Contains(x.SubsidyRecord.Municiaplity.Id))
                    .Where(x => x.Version.IsMain)
                    .Select(x => new
                    {
                        year = x.SubsidyRecord.SubsidyYear,
                        muId = x.SubsidyRecord.Municiaplity.Id,
                        x.BudgetCr,
                        x.SubsidyRecord.OwnerSumForCr,
                        x.SubsidyRecord.BudgetRegion,
                        x.SubsidyRecord.BudgetOtherSource,
                        x.SubsidyRecord.BudgetMunicipality,
                        x.SubsidyRecord.BudgetFcr
                    })
                    .AsEnumerable();

            //Формирую словарь чтобы по МО и году получит ьзначение
            var dictMu = dataList.GroupBy(x => x.muId)
                .ToDictionary(
                    x => x.Key,
                    y => y.GroupBy(x => x.year).ToDictionary(x => x.Key, z => z.First()));

            var section = reportParams.ComplexReportParams.ДобавитьСекцию("SectionRow");

            int currentYear = minYear;
            var num = 1;
            foreach (var mu in muList)
            {
                section.ДобавитьСтроку();
                section["num"] = num;
                section["mu"] = mu.Name;

                currentYear = minYear;

                while (currentYear <= maxYear)
                {
                    section[string.Format("value_1_{0}", currentYear)] = 0;
                    section[string.Format("value_2_{0}", currentYear)] = 0;
                    section[string.Format("value_3_{0}", currentYear)] = 0;
                    section[string.Format("value_4_{0}", currentYear)] = 0;
                    section[string.Format("value_5_{0}", currentYear)] = 0;
                    section[string.Format("value_6_{0}", currentYear)] = 0;

                    if (dictMu.ContainsKey(mu.Id))
                    {
                        if (dictMu[mu.Id].ContainsKey(currentYear))
                        {
                            var rec = dictMu[mu.Id][currentYear];
                            section[string.Format("value_1_{0}", currentYear)] = rec.OwnerSumForCr;
                            section[string.Format("value_2_{0}", currentYear)] = rec.BudgetRegion;
                            section[string.Format("value_3_{0}", currentYear)] = rec.BudgetMunicipality;
                            section[string.Format("value_4_{0}", currentYear)] = rec.BudgetFcr;
                            section[string.Format("value_5_{0}", currentYear)] = rec.BudgetOtherSource;
                            section[string.Format("value_6_{0}", currentYear)] = rec.BudgetCr;
                        }
                    }

                    currentYear++;
                }
                
                num++;
            }

            // Формирую словарь чтобы по году сразу получит ьсумму
            var dictYear = dataList.GroupBy(x => x.year)
                               .ToDictionary(
                                x => x.Key,
                                y => y.ToList());

            // Прохожу еще раз чтобы заполнить итоговые суммы
            currentYear = minYear;
            while (currentYear <= maxYear)
            {
                reportParams.SimpleReportParams[string.Format("sum_1_{0}", currentYear)] = 0;
                reportParams.SimpleReportParams[string.Format("sum_2_{0}", currentYear)] = 0;
                reportParams.SimpleReportParams[string.Format("sum_3_{0}", currentYear)] = 0;
                reportParams.SimpleReportParams[string.Format("sum_4_{0}", currentYear)] = 0;
                reportParams.SimpleReportParams[string.Format("sum_5_{0}", currentYear)] = 0;
                reportParams.SimpleReportParams[string.Format("sum_5_{0}", currentYear)] = 0;

                if (dictYear.ContainsKey(currentYear))
                {
                    var recs = dictYear[currentYear];
                    reportParams.SimpleReportParams[string.Format("sum_1_{0}", currentYear)] = recs.Sum(x => x.OwnerSumForCr);
                    reportParams.SimpleReportParams[string.Format("sum_2_{0}", currentYear)] = recs.Sum(x => x.BudgetRegion);
                    reportParams.SimpleReportParams[string.Format("sum_3_{0}", currentYear)] = recs.Sum(x => x.BudgetMunicipality);
                    reportParams.SimpleReportParams[string.Format("sum_4_{0}", currentYear)] = recs.Sum(x => x.BudgetFcr);
                    reportParams.SimpleReportParams[string.Format("sum_5_{0}", currentYear)] = recs.Sum(x => x.BudgetOtherSource);
                    reportParams.SimpleReportParams[string.Format("sum_5_{0}", currentYear)] = recs.Sum(x => x.BudgetCr);
                }

                currentYear++;
            }
        }

        // заполнение вертикальной секции
        public void CreateVerticalColums(ReportParams reportParams, int minYear, int maxYear)
        {
            var verticalSection = reportParams.ComplexReportParams.ДобавитьСекцию("SectionYears");

            int currentYear = minYear;

            while (currentYear <= maxYear)
            {
                verticalSection.ДобавитьСтроку();
                verticalSection["year"] = string.Format("{0} год, руб.", currentYear);
                
                verticalSection["value_1"] = string.Format("$value_1_{0}$", currentYear);
                verticalSection["value_2"] = string.Format("$value_2_{0}$", currentYear);
                verticalSection["value_3"] = string.Format("$value_3_{0}$", currentYear);
                verticalSection["value_4"] = string.Format("$value_4_{0}$", currentYear);
                verticalSection["value_5"] = string.Format("$value_5_{0}$", currentYear);
                verticalSection["value_6"] = string.Format("$value_6_{0}$", currentYear);

                verticalSection["sum_1"] = string.Format("$sum_1_{0}$", currentYear);
                verticalSection["sum_2"] = string.Format("$sum_2_{0}$", currentYear);
                verticalSection["sum_3"] = string.Format("$sum_3_{0}$", currentYear);
                verticalSection["sum_4"] = string.Format("$sum_4_{0}$", currentYear); 
                verticalSection["sum_5"] = string.Format("$sum_5_{0}$", currentYear);
                verticalSection["sum_6"] = string.Format("$sum_6_{0}$", currentYear);

                currentYear++;
            }
        }
    }
}