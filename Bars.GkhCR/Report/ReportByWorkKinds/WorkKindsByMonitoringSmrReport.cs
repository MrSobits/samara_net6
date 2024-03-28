namespace Bars.GkhCr.Report
{
    using B4.Modules.Reports;
    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.GkhCr.Entities;
    using Bars.GkhCr.Enums;
    using Bars.GkhCr.Report.ReportByWorkKinds;
    using Castle.Windsor;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.IoC;

    /// <summary>
    /// Отчет по видам работ по мониторингу СМР
    /// </summary>
    public class WorkKindsByMonitoringSmrReport : BasePrintForm
    {
        /// <summary>
        /// Контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }

        private long programmCrId;

        private DateTime reportDate;

        private List<long> municipalityListId;

        private readonly List<string> workCodes;

        /// <summary>
        /// Конструктор
        /// </summary>
        public WorkKindsByMonitoringSmrReport()
            : base(new ReportTemplateBinary(Properties.Resources.RepairProgressByKindOfWork))
        {
            this.workCodes = new List<string> { "88", "141", "142", "143" };

            for (var i = 1; i <= 21; i++)
            {
                this.workCodes.Add(i.ToStr());
            }
        }

        /// <summary>
        /// Название
        /// </summary>
        public override string Name => "Отчет по видам работ по мониторингу СМР (по 100% работам)";

        /// <summary>
        /// Описание
        /// </summary>
        public override string Desciption => "Отчет по видам работ по мониторингу СМР (по 100% работам)";

        /// <summary>
        /// Название группы
        /// </summary>
        public override string GroupName => "Ход капремонта";

        /// <summary>
        /// Контроллер параметров для генерации отчёта
        /// </summary>
        public override string ParamsController => "B4.controller.report.WorkKindsByMonitoringSmr";

        /// <summary>
        /// Разрешение
        /// </summary>
        public override string RequiredPermission => "Reports.CR.WorkKindsByMonitoringSmrReport";

        /// <summary>
        /// Установка пользовательских параметров
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        public override void SetUserParams(BaseParams baseParams)
        {
            var municipalitysString = baseParams.Params["municipalityIds"].ToString();
            this.municipalityListId = !string.IsNullOrEmpty(municipalitysString)
                                     ? municipalitysString.Split(',').Select(x => x.ToLong()).ToList()
                                     : new List<long>();
            this.reportDate = baseParams.Params["reportDate"].ToDateTime();
            this.programmCrId = baseParams.Params["programCrId"].ToInt();
        }

        /// <summary>
        /// Генератор отчета
        /// </summary>
        public override string ReportGenerator { get; set; }

        /// <summary>
        /// Подготовка отчета
        /// </summary>
        /// <param name="reportParams">Параметры отчета</param>
        public override void PrepareReport(ReportParams reportParams)
        {
            var serviceObjectCr = this.Container.Resolve<IDomainService<ObjectCr>>();
            var serviceArchiveSmr = this.Container.Resolve<IDomainService<ArchiveSmr>>();
            var serviceTypeWork = this.Container.Resolve<IDomainService<TypeWorkCr>>();
            var serviceMunicipality = this.Container.Resolve<IDomainService<Municipality>>();
            var serviceProgramCr = this.Container.Resolve<IDomainService<ProgramCr>>();

            using (this.Container.Using(serviceObjectCr, serviceArchiveSmr, serviceTypeWork, serviceMunicipality, serviceProgramCr))
            {
                var muList = serviceMunicipality.GetAll()
                .WhereIf(this.municipalityListId.Count > 0, x => this.municipalityListId.Contains(x.Id))
                .Select(x => new { x.Id, x.Name, x.Group })
                .OrderBy(x => x.Group ?? x.Name)
                .ThenBy(x => x.Name)
                .ToList();

                var muDictionary = muList.ToDictionary(x => x.Id, x => new { x.Name, x.Group });

                var alphabeticalGroups = new List<List<long>>();

                var lastGroup = "extraordinaryString";

                foreach (var municipality in muList)
                {
                    if (municipality.Group != lastGroup)
                    {
                        lastGroup = municipality.Group;
                        alphabeticalGroups.Add(new List<long>());
                    }

                    if (alphabeticalGroups.Any())
                    {
                        alphabeticalGroups.Last().Add(municipality.Id);
                    }
                }

                var program = serviceProgramCr.Load(this.programmCrId);
                var programYear = program?.Period.DateStart.Year ?? 0;
                reportParams.SimpleReportParams["Год"] = programYear > 0 ? programYear.ToStr() : string.Empty;

                var realtyObjectsByMuIdDict =
                    serviceObjectCr.GetAll()
                    .WhereIf(this.municipalityListId.Count > 0, x => this.municipalityListId.Contains(x.RealityObject.Municipality.Id))
                    .Where(x => x.ProgramCr.Id == this.programmCrId)
                    .Select(x => new
                    {
                        muId = x.RealityObject.Municipality.Id,
                        RealityObjectId = x.RealityObject.Id,
                        x.RealityObject.Address,
                        RealityObjectFloors = x.RealityObject.MaximumFloors,
                        x.RealityObject.AreaMkd,
                        x.RealityObject.NumberApartments,
                        x.RealityObject.NumberLiving,
                        WallMaterial = x.RealityObject.WallMaterial.Name,
                        RoofingMaterial = x.RealityObject.RoofingMaterial.Name,
                        x.RealityObject.TypeRoof
                    })
                    .OrderBy(x => x.Address)
                    .AsEnumerable()
                    .Select(x => new
                    {
                        x.muId,
                        x.RealityObjectId,
                        x.Address,
                        x.WallMaterial,
                        x.RoofingMaterial,
                        technicalCharacteristics = new TechnicalCharacteristics
                        {
                            TypeRoof = x.TypeRoof,
                            CitizensNum = x.NumberLiving,
                            FlatsNum = x.NumberApartments,
                            Storeys = x.RealityObjectFloors,
                            TotalArea = x.AreaMkd
                        }
                    })
                    .GroupBy(x => x.muId)
                    .ToDictionary(x => x.Key, x => x.ToList());

                var workDataByCodeByRoIdDict = programYear >= 2013
                    ? serviceArchiveSmr.GetAll()
                        .WhereIf(this.municipalityListId.Count > 0, x => this.municipalityListId.Contains(x.TypeWorkCr.ObjectCr.RealityObject.Municipality.Id))
                        .Where(x => x.TypeWorkCr.ObjectCr.ProgramCr.Id == this.programmCrId)
                        .Where(x => x.TypeWorkCr.IsActive)
                        .Where(x => x.TypeWorkCr.ObjectCr.ProgramCr.TypeProgramStateCr == TypeProgramStateCr.Active)
                        .Where(x => x.DateChangeRec <= this.reportDate)
                        .Select(x => new
                        {
                            roId = x.TypeWorkCr.ObjectCr.RealityObject.Id,
                            TypeWorkCrId = x.TypeWorkCr.Id,
                            workCode = x.TypeWorkCr.Work.Code,
                            x.CostSum,
                            x.VolumeOfCompletion,
                            x.DateChangeRec,
                            x.Id
                        })
                        .AsEnumerable()
                        .GroupBy(x => x.roId)
                        .ToDictionary(
                            x => x.Key,
                            x =>
                            x.GroupBy(y => y.workCode)
                            .ToDictionary(
                                y => y.Key,
                                y =>
                                {
                                    var typeWorks = y.GroupBy(z => z.TypeWorkCrId)
                                    .Select(z => z.OrderByDescending(p => p.DateChangeRec).ThenByDescending(p => p.Id).First())
                                    .ToList();

                                    return new TypeWorkCrProxy
                                    {
                                        Sum = typeWorks.Sum(t => t.CostSum),
                                        Volume = typeWorks.Sum(t => t.VolumeOfCompletion)
                                    };
                                }))
                    : serviceTypeWork.GetAll()
                        .WhereIf(this.municipalityListId.Count > 0, x => this.municipalityListId.Contains(x.ObjectCr.RealityObject.Municipality.Id))
                        .Where(x => x.ObjectCr.ProgramCr.Id == this.programmCrId)
                        .Where(x => x.IsActive)
                        .Where(x => x.ObjectCr.ProgramCr.TypeProgramStateCr == TypeProgramStateCr.Active)
                        .Select(x => new
                        {
                            roId = x.ObjectCr.RealityObject.Id,
                            TypeWorkCrId = x.Id,
                            workCode = x.Work.Code,
                            x.CostSum,
                            x.VolumeOfCompletion,
                            x.Id
                        })
                        .AsEnumerable()
                        .GroupBy(x => x.roId)
                        .ToDictionary(
                            x => x.Key,
                            x =>
                            x.GroupBy(y => y.workCode)
                            .ToDictionary(
                                y => y.Key,
                                y => new TypeWorkCrProxy
                                {
                                    Sum = y.Sum(t => t.CostSum),
                                    Volume = y.Sum(t => t.VolumeOfCompletion)
                                }));

                var psdCodes = new List<string> { "1018", "1019" };

                var mainSection = reportParams.ComplexReportParams.ДобавитьСекцию("СекцияГлавная");
                var groupSection = mainSection.ДобавитьСекцию("СекцияГруппа");
                var groupTotalsSection = mainSection.ДобавитьСекцию("СекцияИтогоПоГруппе");
                var regionSection = mainSection.ДобавитьСекцию("СекцияРайоны");
                var regionTotalsSection = regionSection.ДобавитьСекцию("СекцияИтогоПоРайону");
                var objectSection = regionSection.ДобавитьСекцию("СекцияОбъекты");
                var totalsSection = reportParams.ComplexReportParams.ДобавитьСекцию("СекцияИтоги");

                var totals = this.GetTotalsDictionary();

                foreach (var group in alphabeticalGroups)
                {
                    mainSection.ДобавитьСтроку();
                    if (!string.IsNullOrEmpty(muDictionary[group.First()].Group))
                    {
                        groupSection.ДобавитьСтроку();
                        groupSection["НаименованиеГруппы"] = muDictionary[group.First()].Group;
                    }

                    var groupMuTotals = this.GetTotalsDictionary();

                    foreach (var municipalityId in group)
                    {
                        regionSection.ДобавитьСтроку();
                        regionSection["Район"] = muDictionary[municipalityId].Name ?? string.Empty;

                        var municipalityTotals = this.GetTotalsDictionary();

                        if (realtyObjectsByMuIdDict.ContainsKey(municipalityId))
                        {
                            foreach (var objectData in realtyObjectsByMuIdDict[municipalityId])
                            {
                                objectSection.ДобавитьСтроку();
                                objectSection["Адрес"] = objectData.Address;
                                objectSection["Этажность"] = objectData.technicalCharacteristics.Storeys;
                                objectSection["ОбщаяПлощадь"] = objectData.technicalCharacteristics.TotalArea;

                                municipalityTotals["ОбщаяПлощадь"] += objectData.technicalCharacteristics.TotalArea ?? 0M;

                                objectSection["КоличествоКвартир"] = objectData.technicalCharacteristics.FlatsNum;

                                municipalityTotals["КоличествоКвартир"] += objectData.technicalCharacteristics.FlatsNum ?? 0M;

                                objectSection["КоличествоГраждан"] = objectData.technicalCharacteristics.CitizensNum;

                                municipalityTotals["КоличествоГраждан"] += objectData.technicalCharacteristics.CitizensNum ?? 0M;

                                objectSection["МатериалСтен"] = objectData.WallMaterial;
                                objectSection["МатериалКровли"] = objectData.RoofingMaterial;
                                objectSection["ТипКровли"] = objectData.technicalCharacteristics.TypeRoof.GetEnumMeta().Display;

                                if (!workDataByCodeByRoIdDict.ContainsKey(objectData.RealityObjectId))
                                {
                                    continue;
                                }

                                var finValues = workDataByCodeByRoIdDict[objectData.RealityObjectId];

                                var smrSum = finValues.Values.Sum(x => x.Sum);

                                objectSection["СуммаНаСМР"] = smrSum;

                                municipalityTotals["СуммаНаСМР"] += smrSum ?? 0M;

                                foreach (var workCode in this.workCodes)
                                {
                                    if (finValues.ContainsKey(workCode))
                                    {
                                        objectSection[$"Объем{workCode}"] = finValues[workCode].Volume;
                                        objectSection[$"Сумма{workCode}"] = finValues[workCode].Sum;

                                        municipalityTotals[$"Сумма{workCode}"] += finValues[workCode].Sum.HasValue ? finValues[workCode].Sum.Value : 0M;

                                        objectSection[$"Процент{workCode}"] = RepairNorm.GetInfo(workCode, finValues, objectData.technicalCharacteristics);
                                    }
                                }

                                var psdWorksSum = finValues.Where(x => psdCodes.Contains(x.Key)).Sum(x => x.Value.Sum);

                                objectSection["РазработкаИЭкспертиза"] = psdWorksSum;
                                objectSection["РазработкаИЭкспертизаПроцент"] = smrSum.HasValue && smrSum != 0
                                                                                ? psdWorksSum * 100 / smrSum
                                                                                : 0;

                                municipalityTotals["РазработкаИЭкспертиза"] += psdWorksSum ?? 0M;

                                var techControlWorksSum = finValues.ContainsKey("1020") ? finValues["1020"].Sum : 0M;

                                objectSection["Технадзор"] = techControlWorksSum;
                                objectSection["ТехнадзорПроцент"] = smrSum.HasValue && smrSum != 0
                                                                    ? techControlWorksSum * 100 / smrSum
                                                                    : 0;

                                municipalityTotals["Технадзор"] += techControlWorksSum ?? 0M;
                            }
                        }

                        regionTotalsSection.ДобавитьСтроку();
                        this.FillTotalsSection(regionTotalsSection, municipalityTotals);
                        this.AddToSummary(groupMuTotals, municipalityTotals);
                    }

                    if (!string.IsNullOrEmpty(muDictionary[group.Last()].Group))
                    {
                        groupTotalsSection.ДобавитьСтроку();
                        this.FillTotalsSection(groupTotalsSection, groupMuTotals);
                    }

                    this.AddToSummary(totals, groupMuTotals);
                }

                totalsSection.ДобавитьСтроку();
                this.FillTotalsSection(totalsSection, totals);
            }
        }

        private void FillTotalsSection(Section section, Dictionary<string, decimal> finValues)
        {
            foreach (var key in finValues.Keys)
            {
                section[key] = finValues[key];
            }
        }

        private void AddToSummary(IDictionary<string, decimal> summary, IDictionary<string, decimal> addable)
        {
            foreach (var kvPair in addable)
            {
                if (summary.ContainsKey(kvPair.Key))
                {
                    summary[kvPair.Key] += kvPair.Value;
                }
                else
                {
                    summary[kvPair.Key] = kvPair.Value;
                }
            }
        }

        private Dictionary<string, decimal> GetTotalsDictionary()
        {
            var dictionary = new Dictionary<string, decimal>
            {
                ["ОбщаяПлощадь"] = 0M,
                ["КоличествоКвартир"] = 0M,
                ["КоличествоГраждан"] = 0M,
                ["РазработкаИЭкспертиза"] = 0M,
                ["Технадзор"] = 0M,
                ["СуммаНаСМР"] = 0M
            };

            this.workCodes.ForEach(code => dictionary[$"Сумма{code}"] = 0M);

            return dictionary;
        }
    }
}