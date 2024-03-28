namespace Bars.GkhCr.Report
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    
    using B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Enums;
    using Bars.GkhCr.Entities;
    using Bars.GkhCr.Enums;
    using Bars.GkhCr.Report.PlannedAllocationOfWorks;

    using Castle.Windsor;

    public class PlannedAllocationOfWorksReport : BasePrintForm
    {
        private readonly Dictionary<string, decimal> totals = new Dictionary<string, decimal>();
        private Dictionary<string, Record> recordsDict = new Dictionary<string, Record>();
        private int programCrId;

        public PlannedAllocationOfWorksReport()
            : base(new ReportTemplateBinary(Properties.Resources.PlannedAllocationOfWorks))
        {
        }

        public IWindsorContainer Container { get; set; }

        public override string RequiredPermission
        {
            get
            {
                return "Reports.CR.PlannedAllocationOfWorks";
            }
        }

        public override string Name
        {
            get { return "Отчет Плановое распределение выполнения работ по месяцам года по МО РТ"; }
        }

        public override string Desciption
        {
            get { return "Отчет Плановое распределение выполнения работ по месяцам года по МО РТ"; }
        }

        public override string GroupName
        {
            get { return "Отчеты ГЖИ"; }
        }

        public override string ParamsController
        {
            get { return "B4.controller.report.PlannedAllocationOfWorks"; }
        }

        public override void SetUserParams(BaseParams baseParams)
        {
            programCrId = baseParams.Params["programCrId"].ToInt();
        }

        public override string ReportGenerator { get; set; }

        public override void PrepareReport(ReportParams reportParams)
        {
            FillRecords();

            var section = reportParams.ComplexReportParams.ДобавитьСекцию("СекцияДанные");

            var isData = false;
            foreach (var rec in recordsDict.OrderBy(x => x.Key))
            {
                section.ДобавитьСтроку();

                section["Район"] = rec.Key;
                FillSection(section, "СМР", rec.Value.Smr);
                FillSection(section, "МесяцСумма", rec.Value.SumTotal);
                FillSection(section, "Сумма", rec.Value.SumTotal + rec.Value.SumDict[13]);

                section["ПроцентСумма"] = rec.Value.PercentTotal;
                section["Процент"] = rec.Value.PercentTotal + rec.Value.PercentDict[13];

                for (var month = 1; month <= 13; month++)
                {
                    FillSection(section, string.Format("Месяц{0}", month), rec.Value.SumDict[month]);
                    section[string.Format("Процент{0}", month)] = rec.Value.PercentDict[month];

                    if (month < 13)
                    {
                        section["МесяцНар" + month] = rec.Value.CumulativeSum[month];
                        section["ПроцентНар" + month] = rec.Value.CumulativePercent[month];
                    }
                }

                isData = true;
            }

            if (isData)
            {
                ProcessTotal();
            }

            foreach (var поле in totals)
            {
                reportParams.SimpleReportParams[поле.Key] = поле.Value;
            }

            var programm = Container.Resolve<IDomainService<ProgramCr>>().GetAll().Where(x => x.Id == programCrId).Select(x => x.Name).FirstOrDefault();
            if (programm != null)
            {
                reportParams.SimpleReportParams["Программа"] = programm;
            }
        }

        /// <summary>
        /// ЗаполнитьСекцию
        /// </summary>
        /// <param name="section"></param>
        /// <param name="field"></param>
        /// <param name="value"></param>
        private void FillSection(Section section, string field, decimal value)
        {
            if (value != decimal.Zero)
            {
                section[field] = value;
            }

            FillTotal(string.Format("{0}Итого", field), value);
        }

        /// <summary>
        /// Заполнить итоги
        /// </summary>
        /// <param name="field"></param>
        /// <param name="value"></param>
        private void FillTotal(string field, decimal value)
        {
            if (!totals.ContainsKey(field))
            {
                totals.Add(field, decimal.Zero);
            }

            totals[field] += value;
        }

        /// <summary>
        /// Обработка итогов
        /// </summary>
        private void ProcessTotal()
        {
            var divider = totals["СМРИтого"] == decimal.Zero ? 1 : totals["СМРИтого"];
            for (var month = 1; month <= 13; month++)
            {
                FillTotal(string.Format("Процент{0}Итого", month), decimal.Divide(totals[string.Format("Месяц{0}Итого", month)], divider));
                if (month < 13)
                {
                    FillTotal("ПроцентСуммаИтого", totals[string.Format("Процент{0}Итого", month)]);
                }
            }

            FillTotal("МесяцНар1Итого", totals["Месяц1Итого"]);
            FillTotal("ПроцентНар1Итого", totals["Процент1Итого"]);

            for (var month = 2; month < 13; month++)
            {
                FillTotal("МесяцНар" + month + "Итого", totals["МесяцНар" + (month - 1) + "Итого"] + totals["Месяц" + month + "Итого"]);
                FillTotal("ПроцентНар" + month + "Итого", totals["ПроцентНар" + (month - 1) + "Итого"] + totals["Процент" + month + "Итого"]);
            }

            FillTotal("ПроцентИтого", totals["ПроцентСуммаИтого"] + totals["Процент13Итого"]);
        }

        /// <summary>
        /// Метод для получения строк отчета
        /// </summary>
        private void FillRecords()
        {
            var serviceTypeWorkCr = Container.Resolve<IDomainService<TypeWorkCr>>();

            recordsDict = serviceTypeWorkCr.GetAll()
                             .Where(
                                 x =>
                                 x.ObjectCr.ProgramCr.Id == programCrId
                                 && x.FinanceSource.TypeFinance != TypeFinance.Other && x.Work.TypeWork == TypeWork.Work)
                             .Select(x => new { Municipality = x.ObjectCr.RealityObject.Municipality.Name, x.Sum })
                             .AsEnumerable()
                             .GroupBy(x => x.Municipality)
                             .ToDictionary(x => x.Key, x => new Record(x.Key, x.Sum(y => y.Sum)));

            var recordsSumDict =
                serviceTypeWorkCr.GetAll()
                                 .Where(
                                     x =>
                                     x.ObjectCr.ProgramCr.Id == programCrId
                                     && x.FinanceSource.TypeFinance != TypeFinance.Other
                                     && x.Work.TypeWork == TypeWork.Work)
                                 .Select(
                                     x =>
                                     new
                                         {
                                             Municipality = x.ObjectCr.RealityObject.Municipality.Name,
                                             x.DateEndWork,
                                             x.Sum
                                         })
                                 .OrderBy(x => x.Municipality)
                                 .ThenBy(x => x.DateEndWork)
                                 .AsEnumerable()
                                 .GroupBy(x => x.Municipality + x.DateEndWork)
                                 .ToDictionary(
                                     x => x.Key,
                                     x => new
                                              {
                                                  Municipality = x.Select(y => y.Municipality).First(),
                                                      DateEndWork = x.Select(y => y.DateEndWork).First(),
                                                      Sum = x.Sum(y => y.Sum),
                                                  });

            foreach (var rec in recordsSumDict.Values)
            {
                var municipality = rec.Municipality;
                var date = rec.DateEndWork;
                var month = !date.HasValue || date == DateTime.MinValue ? 13 : date.Value.Month;
                var sum = rec.Sum.HasValue ? rec.Sum.Value : 0M;

                if (recordsDict.ContainsKey(municipality))
                {
                    recordsDict[municipality].AddSum(month, sum);
                }
            }

            foreach (var rec in recordsDict.Values)
            {
                rec.ProcessData();
            }
        }

    }
}
