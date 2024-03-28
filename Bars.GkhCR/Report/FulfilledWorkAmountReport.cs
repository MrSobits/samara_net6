namespace Bars.GkhCr.Report
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    
    using B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.GkhCr.Entities;

    using Castle.Windsor;

    public class FulfilledWorkAmountReport : BasePrintForm
    {
        public IWindsorContainer Container { get; set; }

        private List<long> municipalityListId = new List<long>();
        private int programCrId;
        private List<long> finSourceIds = new List<long>();
        private int renovType;

        public FulfilledWorkAmountReport()
            : base(new ReportTemplateBinary(Properties.Resources.FulfilledWorkAmount))
        {
        }

        public override string RequiredPermission
        {
            get
            {
                return "Reports.CR.FulfilledWorkAmountReport";
            }
        }

        public override string Name
        {
            get { return "Выполненные объемы работ по МО"; }
        }

        public override string Desciption
        {
            get { return "Выполненные объемы работ по МО"; }
        }

        public override string GroupName
        {
            get { return "Отчеты ГЖИ"; }
        }

        public override string ParamsController
        {
            get { return "B4.controller.report.FulfilledWorkAmount"; }
        }

        private void ParseIds(List<long> list, string Ids)
        {
            list.Clear();

            if (!string.IsNullOrEmpty(Ids))
            {
                var ids = Ids.Split(',');
                foreach (var id in ids)
                {
                    long Id = 0;
                    if (long.TryParse(id, out Id))
                    {
                        if (!list.Contains(Id))
                        {
                            list.Add(Id);
                        }
                    }
                }
            }
        }

        public override void SetUserParams(BaseParams baseParams)
        {
            programCrId = baseParams.Params["programCr"].ToInt();
            renovType = baseParams.Params["renovType"].ToInt();

            this.ParseIds(municipalityListId, baseParams.Params["municipalityIds"].ToString());
            this.ParseIds(finSourceIds, baseParams.Params["finSource"].ToString());
        }

        public override string ReportGenerator { get; set; }

        private class TypeWorkProxy
        {
            public long MuId;
            public long FinSourceId;
            public string WorkCode;
            public decimal? Sum;
            public decimal? CostSum;
            public TypeWork TypeWork;
        }

        public override void PrepareReport(ReportParams reportParams)
        {
            var psdWorks = new List<string> { "1018", "1019" };
            var tehWorks = new List<string> { "1020" };

            var programCrDomainService = Container.Resolve<IDomainService<ProgramCr>>();
            var municipalityDomainService = Container.Resolve<IDomainService<Municipality>>();
            var finSourceDomainService = Container.Resolve<IDomainService<FinanceSource>>();

            var dateEnd = programCrDomainService.GetAll().Where(x => x.Id == programCrId).Select(x => x.Period.DateEnd).FirstOrDefault();

            reportParams.SimpleReportParams["Год"] = dateEnd.HasValue ? dateEnd.Value.Year.ToString(): string.Empty;
            reportParams.SimpleReportParams["ТекущаяДата"] = DateTime.Now.ToShortDateString();

            var section = reportParams.ComplexReportParams.ДобавитьСекцию("Секция");

            var columns = new List<string>
                {
                    "ОбъемыРабот",
                    "ЛимитСМР",
                    "ПСДРиЭ",
                    "ЛимитПСДРиЭ",
                    "РаботыПоТехнадзору",
                    "ЛимитПоТехнадзору",
                    "ОбъемРаботИУслуг",
                    "ЛимитФинансирования"
                };

            var typeWorkProxies = this.GetWorks();

            var dictTypeWorkCr = typeWorkProxies
                .GroupBy(x => x.MuId)
                .ToDictionary(
                    mu => mu.Key,
                    y => y.GroupBy(x => x.FinSourceId)
                          .ToDictionary(
                              fs => fs.Key, 
                              z => z.GroupBy(x => x.WorkCode)
                                    .ToDictionary(wk => wk.Key, v => v.ToList())));

            int number = 0;

            //справочник id - наименование муниципальных образований
            var dictMunicipality = municipalityDomainService.GetAll()
                .WhereIf(this.municipalityListId.Count > 0, x => this.municipalityListId.Contains(x.Id))
                .Select(x => new { x.Id, x.Name })
                .AsEnumerable()
                .GroupBy(x => x.Id)
                .ToDictionary(x => x.Key, y => y.Select(x => x.Name).FirstOrDefault());

            //справочник id - наименование источников финансирования
            var dictFinSources = finSourceDomainService.GetAll()
                .WhereIf(this.finSourceIds.Count > 0, x => this.finSourceIds.Contains(x.Id))
                .Select(x => new { x.Id, x.Name })
                .AsEnumerable()
                .GroupBy(x => x.Id)
                .ToDictionary(x => x.Key, y => y.Select(x => x.Name).FirstOrDefault());

            var dictGrandTotals = columns.ToDictionary(x => x, y => 0m);

            var subSection = section.ДобавитьСекцию("Подсекция");

            foreach (var mu in dictMunicipality.OrderBy(x => x.Value))
            {
                if (!dictTypeWorkCr.ContainsKey(mu.Key))
                {
                    continue;
                }

                section.ДобавитьСтроку();
                section["РайонМо"] = mu.Value;

                var dictMuTotals = columns.ToDictionary(x => x, y => 0m);

                foreach (var fs in dictFinSources)
                {
                    subSection.ДобавитьСтроку();
                    subSection["НН"] = ++number;
                    subSection["Район"] = mu.Value;
                    subSection["Источник"] = fs.Value;

                    var currentWorks = new Dictionary<string, List<TypeWorkProxy>>();

                    if (dictTypeWorkCr.ContainsKey(mu.Key) && dictTypeWorkCr[mu.Key].ContainsKey(fs.Key))
                    {
                        currentWorks = dictTypeWorkCr[mu.Key][fs.Key];
                    }

                    var sum1 = currentWorks
                        .Sum(x => x.Value.Where(y => y.TypeWork == TypeWork.Work).Sum(y => y.CostSum ?? 0));

                    dictMuTotals["ОбъемыРабот"] += sum1;
                    subSection["ОбъемыРабот"] = sum1;

                    var sum2 = currentWorks
                        .Sum(x => x.Value.Where(y => y.TypeWork == TypeWork.Work).Sum(y => y.Sum ?? 0));

                    dictMuTotals["ЛимитСМР"] += sum2;
                    subSection["ЛимитСМР"] = sum2;

                    var sum3 = currentWorks
                        .Where(x => psdWorks.Contains(x.Key))
                        .Sum(x => x.Value.Sum(y => y.CostSum ?? 0));

                    dictMuTotals["ПСДРиЭ"] += sum3;
                    subSection["ПСДРиЭ"] = sum3;

                    var sum4 = currentWorks
                        .Where(x => psdWorks.Contains(x.Key))
                        .Sum(x => x.Value.Sum(y => y.Sum ?? 0));

                    dictMuTotals["ЛимитПСДРиЭ"] += sum4;
                    subSection["ЛимитПСДРиЭ"] = sum4;

                    var sum5 = currentWorks
                        .Where(x => tehWorks.Contains(x.Key))
                        .Sum(x => x.Value.Sum(y => y.CostSum ?? 0));

                    dictMuTotals["РаботыПоТехнадзору"] += sum5;
                    subSection["РаботыПоТехнадзору"] = sum5;

                    var sum6 = currentWorks
                        .Where(x => tehWorks.Contains(x.Key))
                        .Sum(x => x.Value.Sum(y => y.Sum ?? 0));

                    dictMuTotals["ЛимитПоТехнадзору"] += sum6;
                    subSection["ЛимитПоТехнадзору"] = sum6;

                    var sum7 = currentWorks
                        .Sum(x => x.Value.Sum(y => y.CostSum ?? 0));

                    dictMuTotals["ОбъемРаботИУслуг"] += sum7;
                    subSection["ОбъемРаботИУслуг"] = sum7;

                    var sum8 = currentWorks
                        .Sum(x => x.Value.Sum(y => y.Sum ?? 0));

                    dictMuTotals["ЛимитФинансирования"] += sum8;
                    subSection["ЛимитФинансирования"] = sum8;
                }

                foreach (var total in dictMuTotals)
                {
                    section[total.Key + "Мо"] = total.Value.ToDecimal();
                    dictGrandTotals[total.Key] += total.Value.ToDecimal();
                }
            }

            foreach (var total in dictGrandTotals)
            {
                reportParams.SimpleReportParams[total.Key + "Итого"] = total.Value.ToDecimal();
            }
        }

        private List<TypeWorkProxy> GetWorks()
        {
            var typeWorkCrDomainService = Container.Resolve<IDomainService<TypeWorkCr>>();

            //фильтр кодов работ для вида ремонта "приборы учета" и "комплексный"
            var workCodesFirst = new List<string> { "7", "8", "9", "10", "11" };

            //фильтр кодов работ для вида ремонта "комплексный (основной)"
            var workCodesSecond = new List<string> { "7", "8", "9", "10", "11", "12" };

            var typeWorkProxies = new List<TypeWorkProxy>();

            switch (renovType)
            {
                // Вид ремонта не задан, берем все работы
                case 0:
                    typeWorkProxies = typeWorkCrDomainService.GetAll()
                        .Where(x => x.ObjectCr.ProgramCr.Id == programCrId)
                        .WhereIf(this.municipalityListId.Count > 0, x => this.municipalityListId.Contains(x.ObjectCr.RealityObject.Municipality.Id))
                        .WhereIf(this.finSourceIds.Count > 0, x => this.finSourceIds.Contains(x.FinanceSource.Id))
                        .Select(x => new TypeWorkProxy
                        {
                            MuId = x.ObjectCr.RealityObject.Municipality.Id,
                            FinSourceId = x.FinanceSource.Id,
                            WorkCode = x.Work.Code,
                            Sum = x.Sum,
                            CostSum = x.CostSum,
                            TypeWork = x.Work.TypeWork
                        })
                        .ToList();
                    break;

                // во вкладке виды работ есть работы только с кодом 7,8,9,10,11
                case 1:
                    {
                        var objectCrIds =
                            typeWorkCrDomainService.GetAll()
                                .Where(x => x.ObjectCr.ProgramCr.Id == programCrId && x.Work.TypeWork != TypeWork.Service)
                                .WhereIf(this.municipalityListId.Count > 0, x => this.municipalityListId.Contains(x.ObjectCr.RealityObject.Municipality.Id))
                                .WhereIf(this.finSourceIds.Count > 0, x => this.finSourceIds.Contains(x.FinanceSource.Id))
                                .Select(x => new { x.ObjectCr.Id, x.Work.Code })
                                .AsEnumerable()
                                .GroupBy(x => x.Id)
                                .ToDictionary(x => x.Key, y => y.Select(x => x.Code).ToList())
                                    .Where(x => x.Value.All(workCodesFirst.Contains))
                                    .Select(x => x.Key)
                                    .ToList();

                        var count = objectCrIds.Count;

                        for (int i = 0; i < count; i += 1000)
                        {
                            var tempIds = objectCrIds.Skip(i).Take(count - i < 1000 ? count - i : 1000).ToList();

                            typeWorkProxies.AddRange(typeWorkCrDomainService.GetAll()
                                .WhereIf(this.finSourceIds.Count > 0, x => this.finSourceIds.Contains(x.FinanceSource.Id))
                                .Where(x => tempIds.Contains(x.ObjectCr.Id))
                                .Select(x => new TypeWorkProxy
                                {
                                    MuId = x.ObjectCr.RealityObject.Municipality.Id,
                                    FinSourceId = x.FinanceSource.Id,
                                    WorkCode = x.Work.Code,
                                    Sum = x.Sum,
                                    CostSum = x.CostSum
                                }));
                        }
                    }

                    break;

                // во вкладке виды работ есть работы не только с кодом 7,8,9,10,11
                case 2:
                    {
                        var objectCrIds =
                            typeWorkCrDomainService.GetAll()
                                .Where(x => x.ObjectCr.ProgramCr.Id == programCrId && x.Work.TypeWork != TypeWork.Service)
                                .WhereIf(this.municipalityListId.Count > 0, x => this.municipalityListId.Contains(x.ObjectCr.RealityObject.Municipality.Id))
                                .WhereIf(this.finSourceIds.Count > 0, x => this.finSourceIds.Contains(x.FinanceSource.Id))
                                .Select(x => new { x.ObjectCr.Id, x.Work.Code })
                                .AsEnumerable()
                                .GroupBy(x => x.Id)
                                .ToDictionary(x => x.Key, y => y.Select(x => x.Code).ToList())
                                    .Where(x => x.Value.Any(y => !workCodesFirst.Contains(y)))
                                    .Select(x => x.Key)
                                    .ToList();

                        var count = objectCrIds.Count;

                        for (int i = 0; i < count; i += 1000)
                        {
                            var tempIds = objectCrIds.Skip(i).Take(count - i < 1000 ? count - i : 1000).ToList();

                            typeWorkProxies.AddRange(typeWorkCrDomainService.GetAll()
                                .WhereIf(this.finSourceIds.Count > 0, x => this.finSourceIds.Contains(x.FinanceSource.Id))
                                .Where(x => tempIds.Contains(x.ObjectCr.Id))
                                .Select(x => new TypeWorkProxy
                                {
                                    MuId = x.ObjectCr.RealityObject.Municipality.Id,
                                    FinSourceId = x.FinanceSource.Id,
                                    WorkCode = x.Work.Code,
                                    Sum = x.Sum,
                                    CostSum = x.CostSum
                                }));
                        }
                    }

                    break;

                //во вкладке виды работ есть работы не только с кодом 7,8,9,10,11,12
                case 3:
                    {
                        //получаем объекты кр, у которых во вкладке виды работ по источнику финансирования "Финансирование 185 ФЗ (по доп. программе)" 
                        //есть работы с кодами не 7,8,9,10,11,12
                        //в этом случае берем все работы
                        var objCrIds =
                            typeWorkCrDomainService.GetAll()
                                .Where(x => x.ObjectCr.ProgramCr.Id == programCrId && x.FinanceSource.Code == "3" && x.Work.TypeWork != TypeWork.Service)
                                .WhereIf(this.municipalityListId.Count > 0, x => this.municipalityListId.Contains(x.ObjectCr.RealityObject.Municipality.Id))
                                .Select(x => new { x.ObjectCr.Id, x.Work.Code })
                                .AsEnumerable()
                                .GroupBy(x => x.Id)
                                .ToDictionary(x => x.Key, y => y.Select(x => x.Code).AsEnumerable())
                                    .Where(x => !x.Value.All(workCodesSecond.Contains))
                                    .Select(x => x.Key)
                                    .ToList();

                        var count = objCrIds.Count;

                        for (int i = 0; i < count; i += 1000)
                        {
                            var tempIds = objCrIds.Skip(i).Take(count - i < 1000 ? count - i : 1000).ToList();

                            typeWorkProxies.AddRange(typeWorkCrDomainService.GetAll()
                                .WhereIf(this.finSourceIds.Count > 0, x => this.finSourceIds.Contains(x.FinanceSource.Id))
                                .Where(x => tempIds.Contains(x.ObjectCr.Id))
                                .Select(x => new TypeWorkProxy
                                {
                                    CostSum = x.CostSum,
                                    Sum = x.Sum,
                                    MuId = x.ObjectCr.RealityObject.Municipality.Id,
                                    FinSourceId = x.FinanceSource.Id,
                                    WorkCode = x.Work.Code
                                }));
                        }

                        //теперь берем те объекты кр, у которых по источнику финансирования "Финансирование 185 ФЗ (по доп. программе)" 
                        //есть работы только с кодами 7,8,9,10,11,12
                        //в этом случае берем только работы по другим источникам финансирования и с другими кодами
                        objCrIds =
                            typeWorkCrDomainService.GetAll()
                                .Where(x => x.ObjectCr.ProgramCr.Id == programCrId && x.FinanceSource.Code == "3" && x.Work.TypeWork != TypeWork.Service)
                                .WhereIf(this.municipalityListId.Count > 0, x => this.municipalityListId.Contains(x.ObjectCr.RealityObject.Municipality.Id))
                                .Select(x => new { x.ObjectCr.Id, x.Work.Code })
                                .AsEnumerable()
                                .GroupBy(x => x.Id)
                                .ToDictionary(x => x.Key, y => y.Select(x => x.Code).AsEnumerable())
                                    .Where(x => x.Value.All(workCodesSecond.Contains))
                                    .Select(x => x.Key)
                                    .ToList();

                        count = objCrIds.Count;

                        for (int i = 0; i < count; i += 1000)
                        {
                            var tempIds = objCrIds.Skip(i).Take(count - i < 1000 ? count - i : 1000).ToList();

                            typeWorkProxies.AddRange(typeWorkCrDomainService.GetAll()
                                .Where(x => tempIds.Contains(x.ObjectCr.Id) && x.FinanceSource.Code != "3" && !workCodesSecond.Contains(x.Work.Code))
                                .WhereIf(this.finSourceIds.Count > 0, x => this.finSourceIds.Contains(x.FinanceSource.Id))
                                .Select(x => new TypeWorkProxy
                                {
                                    MuId = x.ObjectCr.RealityObject.Municipality.Id,
                                    FinSourceId = x.FinanceSource.Id,
                                    WorkCode = x.Work.Code,
                                    Sum = x.Sum,
                                    CostSum = x.CostSum
                                }));
                        }
                    }

                    break;
            }

            return typeWorkProxies;
        }
    }
}