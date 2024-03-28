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

    using Castle.Windsor;

    /// <summary>
    /// Реестр многоквартирных домов по видам ремонта
    /// </summary>
    internal class RegisterMkdByTypeRepair : BasePrintForm
    {
        public IWindsorContainer Container { get; set; }

        #region Fields

        private long programCrId;
        private long[] municipalityIds;
        private long[] financialSourceIds;

        sealed class WorkProxy
        {
            public decimal cost { get; set; }

            public decimal volume { get; set; }
        }

        /// <summary>
        /// список приоритетов работ при раскидывании копейки
        /// </summary>
        private List<string> priorityList = new List<string>()
            {
                "13", // Ремонт крыши
                "16", // Ремонт/Утепление фасада
                "12", // Ремонт подвального помещения
                "1",  // Ремонт внутридомовых инж. систем 
                "14", // Ремонт/замена лифтового оборудования (лифтовой шахты)
                "7",  // Установка приборов учета (узлов регулирования)
                "30", // Энергообследование
                "21", // Ремонт подъездов 
                "19", // Устройство систем противопожарной автоматики и дымоудаления
                "2101", // Замена отопительных приборов (радиаторов)
                "2102", // Замена полотенцесушителей
                "2103", // Ремонт системы противопожарной защиты многоквартирного дома
                "2105", // Установка индивидуальных тепловых пунктов (ИТП)
                "2106", // Оборудование входных групп подъездов пандусами,
                "2107", // Ремонт входных групп
                "2108", // Ремонт спусков в подвал
                "2109"  // Усиление конструкций балконов

            };

        #endregion Fields

        public RegisterMkdByTypeRepair()
            : base(new ReportTemplateBinary(Properties.Resources.RegisterMkdByTypeRepair))
        {
        }

        public override string Name
        {
            get
            {
                return "Реестр многоквартирных домов по видам ремонта";
            }
        }

        public override string Desciption
        {
            get
            {
                return "Реестр многоквартирных домов по видам ремонта";
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
                return "B4.controller.report.RegisterMkdByTypeRepair";
            }
        }

        public override string RequiredPermission
        {
            get
            {
                return "Reports.CR.RegisterMkdByTypeRepair";
            }
        }

        public override void SetUserParams(BaseParams baseParams)
        {
            this.programCrId = baseParams.Params["programCrId"].ToInt();

            var municipalityStr = baseParams.Params["municipalityIds"].ToStr();
            this.municipalityIds = string.IsNullOrEmpty(municipalityStr) ? new long[0] : municipalityStr.Split(',').Select(x => x.ToLong()).ToArray();

            var financialSourceIdStr = baseParams.Params["financialSourceIds"].ToStr();
            this.financialSourceIds = string.IsNullOrEmpty(financialSourceIdStr) ? new long[0] : financialSourceIdStr.Split(',').Select(x => x.ToLong()).ToArray();
        }

        public override string ReportGenerator { get; set; }

        public override void PrepareReport(ReportParams reportParams)
        {
            var data = this.Container.Resolve<IDomainService<TypeWorkCr>>().GetAll()
                .WhereIf(this.municipalityIds.Length > 0, x => this.municipalityIds.Contains(x.ObjectCr.RealityObject.Municipality.Id))
                .WhereIf(this.financialSourceIds.Length > 0, x => this.financialSourceIds.Contains(x.FinanceSource.Id))
                .WhereIf(this.programCrId > 0, x => this.programCrId == x.ObjectCr.ProgramCr.Id)
                .Select(x => new
                {
                    x.ObjectCr.RealityObject.Address,
                    MunicipalityName = x.ObjectCr.RealityObject.Municipality.Name,
                    cost = x.Sum,
                    WorkCode = x.Work.Code,
                    x.Work.TypeWork,
                    volume = x.Volume
                })
                .OrderBy(x => x.Address)
                .ToList();

            // Сгруппированные поля отчета 
            // Если 2+ работы сгруппированы в один столбец (например работа "a" и "b"), 
            // то необходимо в столбце написать поле "x"
            // и определить 2 записи 
            // 1. groups["a"] = "x";
            // 2. groups["b"] = "x";
            // Если 1 работа садится в один столбец, то просто пишем groups["a"] = "a"
            var groups = Enumerable.Range(1, 18).Select(x => x.ToStr()).ToDictionary(x => x, x => x);

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

            // Энергетическое обследование дома
            groups["30"] = "30";

            // Замена отопительных приборов (радиаторов)
            groups["2101"] = "2101";

            // Замена полотенцесушителей
            groups["2102"] = "2102";

            // Ремонт системы противопожарной защиты многоквартирного дома
            groups["2103"] = "2103";

            // Установка индивидуальных тепловых пунктов (ИТП)
            groups["2105"] = "2105";

            // Оборудование входных групп подъездов пандусами
            groups["2106"] = "2106";
            
            // Ремонт входных групп
            groups["2107"] = "2107";

            // Ремонт спусков в подвал
            groups["2108"] = "2108";

            // Усиление конструкций балконов
            groups["2109"] = "2109";

            var groupedByAddress = data
                .Select(x => new
                {
                    x.Address,
                    x.MunicipalityName,
                    cost = x.cost.HasValue ? x.cost.Value.RoundDecimal(2) : 0,
                    x.WorkCode,
                    x.TypeWork,
                    volume = x.volume ?? 0
                })
                .GroupBy(x => new { x.MunicipalityName, x.Address })
                .Select(x =>
                {
                    var worksSum = x.Where(y => y.TypeWork == TypeWork.Work).Sum(y => y.cost);
                    var services = x.Where(y => y.TypeWork == TypeWork.Service).Sum(y => y.cost);
                    var roundedSum = 0m;

                    var valuesByWork = x.Where(y => y.TypeWork == TypeWork.Work)
                        .Where(y => groups.ContainsKey(y.WorkCode))
                        .GroupBy(y => groups[y.WorkCode])
                        .ToDictionary(
                             y => y.Key,
                             y =>
                             {
                                 var cost = y.Sum(z => z.cost);
                                 var volume = y.Sum(z => z.volume);

                                 if (worksSum != 0)
                                 {
                                     var addToCost = Math.Floor((cost * 100 * services) / worksSum) / 100;
                                     roundedSum += addToCost;
                                     cost += addToCost;
                                 }

                                 return new WorkProxy { cost = cost, volume = volume };
                             });

                    // потерянные копейки при распределении
                    var cents = services - roundedSum;
                    if (cents > 0)
                    {
                        //в соответствии с приоритетами раскидываем копейки по работам
                        foreach (var priorityKey in priorityList)
                        {
                            if (valuesByWork.ContainsKey(priorityKey) && valuesByWork[priorityKey].cost > 0)
                            {
                                valuesByWork[priorityKey].cost += cents;
                                break;
                            }
                        }
                    }

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
            var sectionMo = reportParams.ComplexReportParams.ДобавитьСекцию("section");

            var totalCost = 0m;
            var worktotals = new Dictionary<string, WorkProxy>();

            foreach (var municipality in groupedByAddress.OrderBy(x => x.Key))
            {
                sectionMo.ДобавитьСтроку();
                sectionMo["MO"] = municipality.Key;

                var sectionHome = sectionMo.ДобавитьСекцию("sectionhome");

                var totalcostMo = 0m;
                var municipalityTotals = new Dictionary<string, WorkProxy>();

                foreach (var home in municipality.Value)
                {
                    sectionHome.ДобавитьСтроку();
                    sectionHome["num"] = pp++;
                    sectionHome["address"] = home.address;

                    sectionHome["totalcost"] = home.totalCost != 0 ? home.totalCost.ToStr() : string.Empty;
                    totalcostMo += home.totalCost;

                    foreach (var row in home.valuesByWork)
                    {
                        sectionHome["Cost" + row.Key] = row.Value.cost != 0 ? row.Value.cost.ToStr() : string.Empty;
                        sectionHome["Volume" + row.Key] = row.Value.volume != 0 ? row.Value.volume.ToStr() : string.Empty;

                        if (municipalityTotals.ContainsKey(row.Key))
                        {
                            var totals = municipalityTotals[row.Key];

                            totals.cost += row.Value.cost;
                            totals.volume += row.Value.volume;

                        }
                        else
                        {
                            municipalityTotals[row.Key] = new WorkProxy { cost = row.Value.cost, volume = row.Value.volume };
                        }
                    }
                }

                sectionMo["totalcostMO"] = totalcostMo != 0 ? totalcostMo.ToStr() : string.Empty;
                totalCost += totalcostMo;

                foreach (var totals in municipalityTotals)
                {
                    sectionMo["CostMo" + totals.Key] = totals.Value.cost != 0 ? totals.Value.cost.ToStr() : string.Empty;
                    sectionMo["VolumeMo" + totals.Key] = totals.Value.volume != 0 ? totals.Value.volume.ToStr() : string.Empty;

                    if (worktotals.ContainsKey(totals.Key))
                    {
                        var _totals = worktotals[totals.Key];

                        _totals.cost += totals.Value.cost;
                        _totals.volume += totals.Value.volume;
                    }
                    else
                    {
                        worktotals[totals.Key] = new WorkProxy { cost = totals.Value.cost, volume = totals.Value.volume };
                    }
                }
            }

            reportParams.SimpleReportParams["totalcostTotal"] = totalCost != 0 ? totalCost.ToStr() : string.Empty;

            foreach (var totals in worktotals)
            {
                reportParams.SimpleReportParams["CostTotal" + totals.Key] = totals.Value.cost != 0 ? totals.Value.cost.ToStr() : string.Empty;
                reportParams.SimpleReportParams["VolumeTotal" + totals.Key] = totals.Value.volume != 0 ? totals.Value.volume.ToStr() : string.Empty;
            }
        }
    }
}