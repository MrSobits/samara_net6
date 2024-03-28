namespace Bars.GkhCr.Report.ReportByWorkKinds
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

    public class RepairProgressByKindOfWork : BasePrintForm
    {
        public IWindsorContainer Container { get; set; }

        private long[] municipalityIds;
        private long programCrId;
        private readonly List<string> workCodes;

        public RepairProgressByKindOfWork()
            : base(new ReportTemplateBinary(Properties.Resources.RepairProgressByKindOfWork))
        {
            this.workCodes = new List<string> { "88", "141", "142", "143" };

            for (var i = 1; i <= 21; i++)
            {
                this.workCodes.Add(i.ToStr());
            }
        }

        /// <inheritdoc />
        public override string Name => "Отчет по видам работ (сравнение с программой КР по 100% работам)";

        /// <inheritdoc />
        public override string Desciption => "Отчет по видам работ (сравнение с программой КР по 100% работам)";

        /// <inheritdoc />
        public override string GroupName => "Ход капремонта";

        /// <inheritdoc />
        public override string ParamsController => "B4.controller.report.RepairProgressByKindOfWork";

        /// <inheritdoc />
        public override string RequiredPermission => "Reports.CR.RepairProgressByKindOfWork";

        /// <inheritdoc />
        public override void SetUserParams(BaseParams baseParams)
        {
            this.programCrId = baseParams.Params["programCrId"].ToInt();

            var m = baseParams.Params["municipalityIds"].ToStr();
            this.municipalityIds = !string.IsNullOrEmpty(m) ? m.Split(',').Select(x => x.ToLong()).ToArray() : new long[0];
        }

        /// <inheritdoc />
        public override string ReportGenerator { get; set; }

        /// <inheritdoc />
        public override void PrepareReport(ReportParams reportParams)
        {
            var serviceMunicipality = this.Container.Resolve<IDomainService<Municipality>>();
            var serviceTypeWorkCr = this.Container.Resolve<IDomainService<TypeWorkCr>>();
            var serviceObjectCr = this.Container.Resolve<IDomainService<ObjectCr>>();

            var muList = serviceMunicipality.GetAll()
                .WhereIf(this.municipalityIds.Length > 0, x => this.municipalityIds.Contains(x.Id))
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

            var programYear = this.Container.Resolve<IDomainService<ProgramCr>>().Load(this.programCrId);
            reportParams.SimpleReportParams["Год"] = programYear != null
                                                         ? programYear.Period.DateStart.Year.ToStr()
                                                         : string.Empty;
            
            var realtyObjectsByMuIdDict = serviceObjectCr.GetAll()
                .WhereIf(this.municipalityIds.Length > 0, x => this.municipalityIds.Contains(x.RealityObject.Municipality.Id))
                .Where(x => x.ProgramCr.Id == this.programCrId)
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


            var workData = serviceTypeWorkCr.GetAll()
                .Where(x => x.ObjectCr.ProgramCr.Id == this.programCrId)
                .WhereIf(this.municipalityIds.Length > 0, x => this.municipalityIds.Contains(x.ObjectCr.RealityObject.Municipality.Id))
                .Select(
                    x =>
                    new
                    {
                        roId = x.ObjectCr.RealityObject.Id,
                        x.Work.Code,
                        x.Sum,
                        x.Volume,
                        x.Work.TypeWork
                    })
                .AsEnumerable()
                .GroupBy(x => x.roId)
                .ToDictionary(
                    x => x.Key,
                    x =>
                    x.GroupBy(v => v.Code)
                        .ToDictionary(
                            v => v.Key,
                            v =>
                            new TypeWorkCrProxy
                            {
                                TypeWork = v.First().TypeWork,
                                Volume = v.Sum(u => u.Volume),
                                Sum = v.Sum(u => u.Sum)
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
                            if (!workData.ContainsKey(objectData.RealityObjectId))
                            {
                                continue;
                            }
                        
                            objectSection.ДобавитьСтроку();
                            objectSection["Адрес"] = objectData.Address;
                            objectSection["Этажность"] = objectData.technicalCharacteristics.Storeys;
                            objectSection["ОбщаяПлощадь"] = objectData.technicalCharacteristics.TotalArea;

                            municipalityTotals["ОбщаяПлощадь"] += objectData.technicalCharacteristics.TotalArea.HasValue ? objectData.technicalCharacteristics.TotalArea.Value : 0M;

                            objectSection["КоличествоКвартир"] = objectData.technicalCharacteristics.FlatsNum;

                            municipalityTotals["КоличествоКвартир"] += objectData.technicalCharacteristics.FlatsNum.HasValue ? objectData.technicalCharacteristics.FlatsNum.Value : 0M;

                            objectSection["КоличествоГраждан"] = objectData.technicalCharacteristics.CitizensNum;

                            municipalityTotals["КоличествоГраждан"] += objectData.technicalCharacteristics.CitizensNum.HasValue ? objectData.technicalCharacteristics.CitizensNum.Value : 0M;

                            objectSection["МатериалСтен"] = objectData.WallMaterial;
                            objectSection["МатериалКровли"] = objectData.RoofingMaterial;
                            objectSection["ТипКровли"] = objectData.technicalCharacteristics.TypeRoof.GetEnumMeta().Display;

                            var finValues = workData[objectData.RealityObjectId];
                        
                            var smrSum = finValues.Values.Where(z => z.TypeWork == TypeWork.Work).Sum(x => x.Sum);

                            objectSection["СуммаНаСМР"] = smrSum;

                            municipalityTotals["СуммаНаСМР"] += smrSum.HasValue ? smrSum.Value : 0M;

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
                            objectSection["РазработкаИЭкспертизаПроцент"] = smrSum.HasValue
                                                                            ? smrSum != 0 ? psdWorksSum * 100 / smrSum : 0
                                                                            : 0;

                            municipalityTotals["РазработкаИЭкспертиза"] += psdWorksSum ?? 0M;

                            var techControlWorksSum = finValues.ContainsKey("1020") ? finValues["1020"].Sum : 0M;

                            objectSection["Технадзор"] = techControlWorksSum;
                            objectSection["ТехнадзорПроцент"] = smrSum.HasValue
                                                                ? smrSum != 0 ? techControlWorksSum * 100 / smrSum : 0
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

            this.workCodes.ForEach(x=> dictionary[$"Сумма{x}"] = 0M);

            return dictionary;
        }
    }
}