namespace Bars.Gkh.Overhaul.Hmao.Reports
{
    using System.Linq;

    using Bars.B4;
    using B4.Modules.Reports;

    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Overhaul.Hmao.Entities;
    using Castle.Windsor;
    using Bars.B4.DataAccess;
    using Bars.Gkh.Overhaul.Hmao.ConfigSections;

    using Gkh.Utils;

    public class SubsidyBudget : BasePrintForm
    {
        public IRepository<Municipality> MunicipalityDomain { get; set; }

        public IDomainService<SubsidyRecordVersion> SubsidyRecordVersionDomain { get; set; }

        public SubsidyBudget()
            : base(new ReportTemplateBinary(Properties.Resources.SubsidyBudget))
        {

        }

        public IWindsorContainer Container { get; set; }

        public override string Name
        {
            get
            {
                return "Итоговый бюджет на КР";
            }
        }

        public override string Desciption
        {
            get
            {
                return "Итоговый бюджет на КР";
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
                return "B4.controller.report.SubsidyBudget";
            }
        }

        public override string RequiredPermission
        {
            get
            {
                return "Ovrhl.SubsidyBudget";
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
            /*
             * SectionRow
             * num
             * mu
            */

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
                .OrderBy(x => x.Name).Select(x => new {x.Id, x.Name}).ToList();

            var dataList =
                SubsidyRecordVersionDomain.GetAll()
                    .Where(x => x.SubsidyRecord.Municiaplity != null)
                    .WhereIf(municipalityIds.Any(), x => municipalityIds.Contains(x.SubsidyRecord.Municiaplity.Id))
                    .Where(x => x.Version.IsMain)
                    .Select(x => new
                    {
                        year = x.SubsidyRecord.SubsidyYear,
                        muId = x.SubsidyRecord.Municiaplity.Id,
                        x.BudgetCr
                    })
                    .AsEnumerable();

            //Формирую словарь чтобы по МО и году получит ьзначение
            var dictMu = dataList.GroupBy(x => x.muId)
                .ToDictionary(x => x.Key,
                    y => y.GroupBy(x => x.year).ToDictionary(x => x.Key, z => z.Select(x => x.BudgetCr).First()));

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
                    section[string.Format("value_{0}", currentYear)] = 0;

                    if (dictMu.ContainsKey(mu.Id))
                    {
                        if (dictMu[mu.Id].ContainsKey(currentYear))
                        {
                            section[string.Format("value_{0}", currentYear)] = dictMu[mu.Id][currentYear];
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
                                y => y.Sum( x => x.BudgetCr));

            // Прохожу еще раз чтобы заполнить итоговые суммы
            currentYear = minYear;
            while (currentYear <= maxYear)
            {
                reportParams.SimpleReportParams[string.Format("sum_{0}", currentYear)] = 0;

                if (dictYear.ContainsKey(currentYear))
                {
                    reportParams.SimpleReportParams[string.Format("sum_{0}", currentYear)] = dictYear[currentYear];
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
                verticalSection["value"] = string.Format("$value_{0}$", currentYear);
                verticalSection["sum"] = string.Format("$sum_{0}$", currentYear);

                currentYear++;
            }
        }
    }
}