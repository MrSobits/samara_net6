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
    using Bars.GkhCr.Report.Helper;

    public class RegisterMkdToBeRepaired : BasePrintForm
    {
        #region Properties

        protected long programCrId;

        protected long[] municipalityIds;

        protected long[] financialSourceIds;

        sealed class WorkProxy
        {
            /// <summary>
            /// Стоимость работ
            /// </summary>
            public decimal Cost { get; set; }

            /// <summary>
            /// Объем работ
            /// </summary>
            public decimal Volume { get; set; }
        }

        /// <summary>
        /// Касс-прокси для работ КР
        /// </summary>
        protected class RelatyObjectWorkDataProxy
        {
            /// <summary>
            /// Адрес дома
            /// </summary>
            public string Address { get; set; }
            /// <summary>
            /// Наименование МР
            /// </summary>
            public string MunicipalityName { get; set; }
            /// <summary>
            /// Сумма (плановая)
            /// </summary>
            public decimal? Cost { get; set; }
            /// <summary>
            /// Код работы
            /// </summary>
            public string WorkCode { get; set; }
            /// <summary>
            /// Тип работ
            /// </summary>
            public TypeWork TypeWork { get; set; }
            /// <summary>
            /// Объем работ (плановый)
            /// </summary>
            public decimal? Volume { get; set; }
        }


        /// <summary>
        /// Конструктор для инициализации генератора отчёта
        /// </summary>
        public RegisterMkdToBeRepaired()
            : base(new ReportTemplateBinary(Properties.Resources.RegisterMkdToBeRepaired))
        {
        }

        /// <summary>
        /// Имя отчёта
        /// </summary>
        public override string Name
        {
            get 
            {
                return "02_Реестр МКД, подлежащих ремонту";
            }
        }

        /// <summary>
        /// Описание отчёта
        /// </summary>
        public override string Desciption
        {
            get
            {
                return "02_Реестр МКД, подлежащих ремонту";
            }
        }

        /// <summary>
        /// Имя группы отчёта
        /// </summary>
        public override string GroupName
        {
            get
            {
                return "Формы для Фонда";
            }
        }

        /// <summary>
        /// Контроллер формы отчёта
        /// </summary>
        public override string ParamsController
        {
            get
            {
                return "B4.controller.report.RegisterMkdToBeRepaired";
            }
        }

        /// <summary>
        /// Необходимые привелегии
        /// </summary>
        public override string RequiredPermission
        {
            get
            {
                return "Reports.CR.RegisterMkdToBeRepaired";
            }
        }

        public override string ReportGenerator { get; set; }

        /// <summary>
        /// Сервис Виды работ КР
        /// </summary>
        public IDomainService<TypeWorkCr> TypeWorkCrDomain { get; set; }

        #endregion Properties

        /// <summary>
        /// Установка базовых параметров отчёта
        /// </summary>
        /// <param name="baseParams"></param>
        public override void SetUserParams(BaseParams baseParams)
        {
            this.programCrId = baseParams.Params["programCrId"].ToLong();

            var municipalityStr = baseParams.Params["municipalityIds"].ToStr();
            this.municipalityIds = string.IsNullOrEmpty(municipalityStr) ? new long[0] : municipalityStr.Split(',').Select(x => x.ToLong()).ToArray();

            var financialSourceIdStr = baseParams.Params["financialSourceIds"].ToStr();
            this.financialSourceIds = string.IsNullOrEmpty(financialSourceIdStr) ? new long[0] : financialSourceIdStr.Split(',').Select(x => x.ToLong()).ToArray();
        }

        /// <summary>
        /// Приготовить отчёт
        /// </summary>
        /// <param name="reportParams"></param>
        public override void PrepareReport(ReportParams reportParams)
        {
            var data = GetMkdToBeRepaired();

            // Сгруппированные поля отчета 
            // Если 2+ работы сгруппированы в один столбец (например работа "a" и "b"), 
            // то необходимо в столбце написать поле "x"
            // и определить 2 записи 
            // 1. groups["a"] = "x";
            // 2. groups["b"] = "x";
            // Если 1 работа садится в один столбец, то просто пишем groups["a"] = "a"
            var groups = Enumerable.Range(1, 19).Select(x => x.ToStr()).ToDictionary(x => x, x => x);

            // Группирвка всех работ по "Ремонт внутридомовых инж. систем" : { "1", "2", "3", "4", "5", "6" };
            groups["2"] = "1";
            groups["3"] = "1";
            groups["4"] = "1";
            groups["5"] = "1";
            groups["6"] = "1";

            // Группирвка всех работ по "Установка приборов учета (узлов регулирования)" : { "7", "8", "9", "10", "11", "29" };
            groups["8"] = "7";
            groups["9"] = "7";
            groups["10"] = "7";
            groups["11"] = "7";
            groups["29"] = "7";

            // Ремонт/замена лифтового оборудования + Ремонт лифтовой шахты
            groups["15"] = "14";

            // Ремонт + Утепление фасада
            groups["17"] = "16";

            groups["21"] = "21";
            groups["30"] = "30";

            var groupedByAddress = data
                .Select(x => new
                {
                    x.Address,
                    x.MunicipalityName,
                    Cost = x.Cost.HasValue ? x.Cost.Value.RoundDecimal(2) : 0,
                    x.WorkCode,
                    x.TypeWork,
                    Volume = x.Volume ?? 0
                })
                .GroupBy(x => new { x.MunicipalityName, x.Address })
                .Select(x =>
                 {
                    var works = x.Where(y => y.TypeWork == TypeWork.Work)
                            .Where(y => groups.ContainsKey(y.WorkCode))
                            .ToList();

                    var worksSum = works.Sum(y => y.Cost);

                    var services = x.Where(y => y.TypeWork == TypeWork.Service).Sum(y => y.Cost);

                    var distributedDict = x
                       .Select(y => new SumDistributorTypeWorkProxy
                       {
                           WorkCode = y.WorkCode,
                           TypeWork = y.TypeWork,
                           Sum = y.Cost
                       })
                       .ToList()
                       .GetDistrubutedList(groups.Keys.ToList())
                       .GroupBy(y => groups[y.WorkCode])
                       .ToDictionary(y => y.Key, y => y.Sum(z => z.Sum));

                    var valuesByWork = x
                        .Where(y => y.TypeWork == TypeWork.Work)
                        .Where(y => groups.ContainsKey(y.WorkCode))
                        .GroupBy(y => groups[y.WorkCode])
                        .ToDictionary(
                             y => y.Key,
                             y => new WorkProxy
                             {
                                 Cost = distributedDict.ContainsKey(y.Key) ? distributedDict[y.Key] : 0,
                                 Volume = y.Sum(z => z.Volume)
                             });

                    return new
                    {
                        municipality = x.Key.MunicipalityName,
                        address = x.Key.Address,
                        totalCost = worksSum + services,
                        valuesByWork
                    };
                })
                .GroupBy(x => x.municipality)
                .ToDictionary(x => x.Key, x => x.ToList());

            var pp = 1;
            var sectionMo = reportParams.ComplexReportParams.ДобавитьСекцию("sectionMo");

            var totalCost = 0m;
            var worktotals = new Dictionary<string, WorkProxy>();

            foreach (var municipality in groupedByAddress.OrderBy(x => x.Key))
            {
                sectionMo.ДобавитьСтроку();
                sectionMo["moName"] = municipality.Key;

                var sectionHome = sectionMo.ДобавитьСекцию("sectionRo");

                var totalcostMo = 0m;
                var municipalityTotals = new Dictionary<string, WorkProxy>();

                foreach (var home in municipality.Value.OrderBy(x => x.address))
                {
                    sectionHome.ДобавитьСтроку();
                    sectionHome["num"] = pp++;
                    sectionHome["address"] = home.address;

                    sectionHome["totalCost"] = home.totalCost != 0 ? home.totalCost.ToStr() : string.Empty;
                    totalcostMo += home.totalCost;

                    foreach (var row in home.valuesByWork)
                    {
                        sectionHome["cost" + row.Key] = row.Value.Cost != 0 ? row.Value.Cost.ToStr() : string.Empty;
                        sectionHome["vol" + row.Key] = row.Value.Volume != 0 ? row.Value.Volume.ToStr() : string.Empty;

                        if (municipalityTotals.ContainsKey(row.Key))
                        {
                            var totals = municipalityTotals[row.Key];

                            totals.Cost += row.Value.Cost;
                            totals.Volume += row.Value.Volume;

                        }
                        else
                        {
                            municipalityTotals[row.Key] = new WorkProxy { Cost = row.Value.Cost, Volume = row.Value.Volume };
                        }
                    }
                }

                sectionMo["totalСostMo"] = totalcostMo != 0 ? totalcostMo.ToStr() : string.Empty;
                totalCost += totalcostMo;

                foreach (var totals in municipalityTotals)
                {
                    sectionMo["costMo" + totals.Key] = totals.Value.Cost != 0 ? totals.Value.Cost.ToStr() : string.Empty;
                    sectionMo["volMo" + totals.Key] = totals.Value.Volume != 0 ? totals.Value.Volume.ToStr() : string.Empty;

                    if (worktotals.ContainsKey(totals.Key))
                    {
                        var _totals = worktotals[totals.Key];

                        _totals.Cost += totals.Value.Cost;
                        _totals.Volume += totals.Value.Volume;
                    }
                    else
                    {
                        worktotals[totals.Key] = new WorkProxy { Cost = totals.Value.Cost, Volume = totals.Value.Volume };
                    }
                }
            }

            reportParams.SimpleReportParams["totalСostSummary"] = totalCost != 0 ? totalCost.ToStr() : string.Empty;

            foreach (var totals in worktotals)
            {
                reportParams.SimpleReportParams["costSummary" + totals.Key] = totals.Value.Cost != 0 ? totals.Value.Cost.ToStr() : string.Empty;
                reportParams.SimpleReportParams["volSummary" + totals.Key] = totals.Value.Volume != 0 ? totals.Value.Volume.ToStr() : string.Empty;
            }
        }

        /// <summary>
        /// Получить список работ. Переопределяемый метод.
        /// </summary>
        /// <returns></returns>
        protected virtual List<RelatyObjectWorkDataProxy> GetMkdToBeRepaired()
        {
            return this.TypeWorkCrDomain.GetAll()
                .WhereIf(this.municipalityIds.Length > 0, x => this.municipalityIds.Contains(x.ObjectCr.RealityObject.MoSettlement.Id))
                .WhereIf(this.financialSourceIds.Length > 0, x => this.financialSourceIds.Contains(x.FinanceSource.Id))
                .WhereIf(this.programCrId > 0, x => this.programCrId == x.ObjectCr.ProgramCr.Id)
                .Select(x => new RelatyObjectWorkDataProxy
                {
                    Address = x.ObjectCr.RealityObject.Address,
                    MunicipalityName = x.ObjectCr.RealityObject.MoSettlement.Name,
                    Cost = x.Sum,
                    WorkCode = x.Work.Code,
                    TypeWork = x.Work.TypeWork,
                    Volume = x.Volume
                })
                .OrderBy(x => x.Address)
                .ToList();
        }
    }
}