namespace Bars.GkhCr.Report
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using Bars.B4;
    
    using Bars.B4.Modules.States;
    using B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.GkhCr.Entities;
    using Castle.Windsor;

    /// <summary>
    /// Ежеквартальный и годовой отчеты для Фонда (расширенный)
    /// </summary>
    public class QuartAndAnnualRepByFundExt : BasePrintForm
    {
        #region Свойства
        public IWindsorContainer Container { get; set; }
        private long programCrId;
        private long[] financeSourceIds;
        private long[] municipalityIds;
        private DateTime reportDate;
        private int assemblyTo = 10;
        private List<string> column10Codes = new List<string> { "7", "8", "9", "10", "11", "29" };
        private List<string> allWorkCodes = new List<string> { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "13", "14", "12", "17", "16", "15", "18", "29" };

        /// <summary>
        /// словарь приоритетов работ при раскидывании копейки, лол
        /// </summary>
        private Dictionary<int, string> priorityDict = new Dictionary<int, string>
        {
            {1, "13"},
            {2, "16"},
            {3, "17"},
            {4, "12"},
            {5, "2"},
            {6, "4"},
            {7, "1"},
            {8, "3"},
            {9, "6"},
            {10, "5"},
            {11, "14"},
            {12, "15"},
            {13, "8"},
            {14, "9"},
            {15, "7"},
            {16, "10"},
            {17, "11"},
            {18, "29"},
            {19, "30"}
        };

        public QuartAndAnnualRepByFundExt()
            : base(new ReportTemplateBinary(Properties.Resources.QuartAndAnnualRepByFundExt))
        {
        }

        public override string Name
        {
            get
            {
                return "Ежеквартальный и годовой отчеты для Фонда (расширенный)";
            }
        }

        public override string Desciption
        {
            get
            {
                return "Ежеквартальный и годовой отчеты для Фонда (расширенный)";
            }
        }

        public override string GroupName
        {
            get
            {
                return "Формы программы";
            }
        }

        public override string ParamsController
        {
            get
            {
                return "B4.controller.report.QuartAndAnnualRepByFundExt";
            }
        }

        public override string RequiredPermission
        {
            get
            {
                return "Reports.CR.QuartAndAnnualRepByFundExt";
            }
        }
        #endregion

        public override void SetUserParams(BaseParams baseParams)
        {
            this.programCrId = baseParams.Params["programCrId"].ToInt();

            var financeSourceIdsList = baseParams.Params.GetAs("finSources", string.Empty);
            this.financeSourceIds = !string.IsNullOrEmpty(financeSourceIdsList) ? financeSourceIdsList.Split(',').Select(id => id.ToLong()).ToArray() : new long[0];

            var municipalityIdsList = baseParams.Params.GetAs("municipalityIds", string.Empty);
            this.municipalityIds = !string.IsNullOrEmpty(municipalityIdsList) ? municipalityIdsList.Split(',').Select(id => id.ToLong()).ToArray() : new long[0];

            this.reportDate = baseParams.Params["reportDate"].ToDateTime();

            // 10 - по плану (по умолчанию); 20 - по ходу работ
            this.assemblyTo = baseParams.Params["assemblyTo"].ToInt();
        }

        public override void PrepareReport(ReportParams reportParams)
        {
            var program = this.Container.Resolve<IDomainService<ProgramCr>>().Load(this.programCrId);
            var programYear = program != null ? program.Period.DateStart.Year : 0;
            var serviceMunicipality = this.Container.Resolve<IDomainService<Municipality>>();

            var muList = serviceMunicipality.GetAll()
                .WhereIf(this.municipalityIds.Length > 0, x => this.municipalityIds.Contains(x.Id))
                .Select(x => new { x.Id, x.Name, Group = x.Group ?? string.Empty })
                .OrderBy(x => x.Group)
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

            var serviceTypeWorkCr = Container.Resolve<IDomainService<TypeWorkCr>>().GetAll()
                .Where(x => x.ObjectCr.ProgramCr.Id == programCrId)
                .WhereIf(municipalityIds.Length > 0, x => municipalityIds.Contains(x.ObjectCr.RealityObject.Municipality.Id))
                .WhereIf(financeSourceIds.Length > 0, x => financeSourceIds.Contains(x.FinanceSource.Id));

            var objectCrDataByMu = serviceTypeWorkCr
                .Select(x => new
                {
                    muId = x.ObjectCr.RealityObject.Municipality.Id,
                    objectCrId = x.ObjectCr.Id,
                    x.ObjectCr.RealityObject.Address,
                    x.ObjectCr.DateAcceptCrGji
                })
                .AsEnumerable()
                .OrderBy(x => x.Address)
                .GroupBy(x => x.muId)
                .ToDictionary(x => x.Key,
                    x => x.GroupBy(y => y.objectCrId)
                        .ToDictionary(y => y.Key,
                            y =>
                            {
                                var generaldataByObjCr = y.FirstOrDefault();
                                if (generaldataByObjCr == null)
                                {
                                    return null;
                                }

                                return
                                    new
                                    {
                                        generaldataByObjCr.Address,
                                        generaldataByObjCr.DateAcceptCrGji
                                    };
                            }));

            var typeWorkIdsQuery = serviceTypeWorkCr.Select(x => x.Id);

            Dictionary<long, Dictionary<int, decimal>> dataFact;

            if (programYear >= 2013 && assemblyTo == 20)
            {
                dataFact = this.Container.Resolve<IDomainService<ArchiveSmr>>().GetAll()
                    .Where(x => typeWorkIdsQuery.Contains(x.TypeWorkCr.Id))
                    .Where(x => x.DateChangeRec <= this.reportDate)
                    .Select(x => new
                    {
                        x.Id,
                        x.DateChangeRec,
                        objectCrId = x.TypeWorkCr.ObjectCr.Id,
                        TypeWorkCrId = x.TypeWorkCr.Id,
                        x.TypeWorkCr.Work.TypeWork,
                        x.TypeWorkCr.Work.Code,
                        CostSum = x.CostSum ?? 0,
                        VolumeOfCompletion = x.VolumeOfCompletion ?? 0
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.objectCrId)
                    .ToDictionary(x => x.Key,
                        x =>
                        {
                            var typeWorks = x.GroupBy(y => y.TypeWorkCrId)
                                .Select(y =>
                                {
                                    var typeWork = y.OrderByDescending(
                                        z => z.DateChangeRec)
                                        .ThenByDescending(z => z.Id)
                                        .First();

                                    return new TypeWorkForFundProxy
                                    {
                                        Code = typeWork.Code,
                                        CostSum = typeWork.CostSum,
                                        VolumeOfCompletion = typeWork.VolumeOfCompletion,
                                        type = typeWork.TypeWork
                                    };
                                })
                                .ToList();

                            var sumServices = x.Where(y => y.TypeWork == TypeWork.Service).Sum(y => y.CostSum);

                            return GetDataDictionary(typeWorks, sumServices);
                        });
            }
            else if (assemblyTo == 20)
            {
                dataFact = this.Container.Resolve<IDomainService<TypeWorkCr>>().GetAll()
                    .Where(x => typeWorkIdsQuery.Contains(x.Id))
                    .Select(x => new
                    {
                        objectCrId = x.ObjectCr.Id,
                        TypeWorkCrId = x.Id,
                        x.Work.TypeWork,
                        x.Work.Code,
                        CostSum = x.CostSum ?? 0,
                        VolumeOfCompletion = x.VolumeOfCompletion ?? 0
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.objectCrId)
                    .ToDictionary(x => x.Key,
                        x =>
                        {
                            var typeWorks = x.GroupBy(y => y.Code)
                                .Select(typeWork => new TypeWorkForFundProxy
                                {
                                    Code = typeWork.Key,
                                    CostSum = typeWork.Sum(z => z.CostSum),
                                    VolumeOfCompletion = typeWork.Sum(z => z.VolumeOfCompletion),
                                    type = typeWork.Select(z => z.TypeWork).FirstOrDefault()
                                })
                                .ToList();

                            var sumServices = x.Where(y => y.TypeWork == TypeWork.Service).Sum(y => y.CostSum);

                            return GetDataDictionary(typeWorks, sumServices);
                        });
            }
            else
            {
                dataFact = this.Container.Resolve<IDomainService<TypeWorkCr>>().GetAll()
                    .Where(x => typeWorkIdsQuery.Contains(x.Id))
                    .Select(x => new
                    {
                        objectCrId = x.ObjectCr.Id,
                        TypeWorkCrId = x.Id,
                        x.Work.TypeWork,
                        x.Work.Code,
                        plantSum = x.Sum ?? 0,
                        planVolume = x.Volume ?? 0
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.objectCrId)
                    .ToDictionary(x => x.Key,
                        x =>
                        {
                            var typeWorks = x.GroupBy(y => y.Code)
                                .Select(typeWork => new TypeWorkForFundProxy
                                {
                                    Code = typeWork.Key,
                                    CostSum = typeWork.Sum(z => z.plantSum),
                                    VolumeOfCompletion = typeWork.Sum(z => z.planVolume),
                                    type = typeWork.Select(z => z.TypeWork).FirstOrDefault()
                                })
                                .ToList();

                            var sumServices = x.Where(y => y.TypeWork == TypeWork.Service).Sum(y => y.plantSum);

                            return GetDataDictionary(typeWorks, sumServices);
                        });
            }

            var groupSection = reportParams.ComplexReportParams.ДобавитьСекцию("sectionGroup");
            var groupNameSection = groupSection.ДобавитьСекцию("sectionGroupName");
            var sectionMu = groupSection.ДобавитьСекцию("sectionMu");
            var section = sectionMu.ДобавитьСекцию("section");
            var sectionTotal = reportParams.ComplexReportParams.ДобавитьСекцию("sectionTotal");

            var counter = 0;
            var totals = new Dictionary<int, decimal>();
            foreach (var group in alphabeticalGroups)
            {
                groupSection.ДобавитьСтроку();
                var totalsGr = new Dictionary<int, decimal>();
                var firstMu = muDictionary[group.First()];

                var hasGroup = false;
                if (firstMu.Group != string.Empty)
                {
                    groupNameSection.ДобавитьСтроку();
                    groupNameSection["groupname"] = firstMu.Group;
                    hasGroup = true;
                }

                foreach (var muId in group)
                {
                    if (objectCrDataByMu.ContainsKey(muId))
                    {
                        sectionMu.ДобавитьСтроку();
                        sectionMu["МунОбр"] = muDictionary[muId].Name;

                        var totalsMu = new Dictionary<int, decimal>();

                        var crObjects = objectCrDataByMu[muId];

                        foreach (var crObject in crObjects)
                        {
                            section.ДобавитьСтроку();
                            section["column1"] = ++counter;
                            section["column2"] = crObject.Value.Address;

                            if (dataFact.ContainsKey(crObject.Key))
                            {
                                var currentCrObjFactData = dataFact[crObject.Key];

                                this.FillSection(section, currentCrObjFactData, string.Empty);
                                this.AttachDictionaries(totalsMu, currentCrObjFactData);
                            }
                        }

                        // итоги по мун.образованию
                        this.FillSection(sectionMu, totalsMu, "Mu");
                        this.AttachDictionaries(totalsGr, totalsMu);
                        this.AttachDictionaries(totals, totalsMu);
                    }
                }

                // заполнение итогов по группе
                if (hasGroup)
                {
                    this.FillSection(groupNameSection, totalsGr, "Gr");
                }
            }

            // заполнение итогов по всем мун.образованиям
            sectionTotal.ДобавитьСтроку();
            this.FillSection(sectionTotal, totals, "Tot");
        }

        private void FillSection(Section section, Dictionary<int, decimal> data, string postfix)
        {
            for (var i = 3; i <= 23; i++)
            {
                if (data.ContainsKey(i) && data[i] != 0)
                {
                    section[string.Format("column{0}{1}", postfix, i.ToStr())] = data[i];
                }
            }
        }

        private void AttachDictionaries(Dictionary<int, decimal> totals, Dictionary<int, decimal> addable)
        {
            foreach (var key in addable.Keys)
            {
                if (totals.ContainsKey(key))
                {
                    totals[key] += addable[key];
                }
                else
                {
                    totals[key] = addable[key];
                }
            }
        }

        private Dictionary<int, decimal> GetDataDictionary(List<TypeWorkForFundProxy> typeWorks, decimal sumServices)
        {
            var sums = new Dictionary<string, decimal>();
            decimal allWorkSum = 0;

            // по всем работам
            foreach (var work in typeWorks.Where(x => x.type == TypeWork.Work)) 
            {
                sums.Add(work.Code, work.CostSum);
                allWorkSum += work.CostSum;
            }

            if (sumServices > 0)
            {
                if (allWorkSum <= 0)
                {
                    var firstWorkCode = typeWorks.Where(x => x.type == TypeWork.Work).Select(x => x.Code).FirstOrDefault(x => x != null);
                    if (firstWorkCode != null && sums.ContainsKey(firstWorkCode) && sums[firstWorkCode] > 0)
                    {
                        sums[firstWorkCode] += sumServices;
                    }
                }
                else
                {
                    var dopSums = sums.Where(x => x.Key != "20").ToDictionary(x => x.Key, y => (y.Value * sumServices / allWorkSum));
                    var tmpSum = dopSums.Sum(x => x.Value);
                    var err = Math.Round(sumServices - tmpSum, 2);

                    if (err > 0)
                    {
                        foreach (var prior in priorityDict.OrderBy(x => x.Key))
                        {
                            if (sums.ContainsKey(prior.Value) && sums[prior.Value] > 0)
                            {
                                sums[prior.Value] += err;
                                break;
                            }
                        }
                    }

                    foreach (var sum in dopSums)
                    {
                        sums[sum.Key] += sum.Value;
                    }
                }
            }

            var data = new Dictionary<int, decimal>();
            data[4] = sums.ContainsKey("6") ? sums["6"] : 0M;
            data[5] = sums.ContainsKey("1") ? sums["1"] : 0M;
            data[6] = sums.ContainsKey("5") ? sums["5"] : 0M;

            data[7] = sums.ContainsKey("3") ? sums["3"] : 0M;
            data[8] = sums.ContainsKey("2") ? sums["2"] : 0M;
            data[9] = sums.ContainsKey("4") ? sums["4"] : 0M;

            data[10] = this.column10Codes.Sum(code => sums.ContainsKey(code) ? sums[code] : 0M);

            data[3] = data[4] + data[5] + data[6] + data[7] + data[8] + data[9] + data[10];

            data[11] = typeWorks.Where(y => y.Code == "13").Sum(y => y.VolumeOfCompletion);
            data[12] = sums.ContainsKey("13") ? sums["13"] : 0M;

            var liftWorksList = new List<string> { "14", "15" };
            data[13] = typeWorks.Where(y => y.Code == "14" || y.Code == "15").Sum(y => y.VolumeOfCompletion);
            data[14] = liftWorksList.Sum(code => sums.ContainsKey(code) ? sums[code] : 0M);

            data[15] = typeWorks.Where(y => y.Code == "12").Sum(y => y.VolumeOfCompletion);
            data[16] = sums.ContainsKey("12") ? sums["12"] : 0M;

            var fasadWorksList = new List<string> { "16", "17" };
            data[17] = typeWorks.Where(y => y.Code == "16" || y.Code == "17").Sum(y => y.VolumeOfCompletion);
            data[18] = fasadWorksList.Sum(code => sums.ContainsKey(code) ? sums[code] : 0M);

            data[19] = typeWorks.Where(y => y.Code == "18").Sum(y => y.VolumeOfCompletion);
            data[20] = sums.ContainsKey("18") ? sums["18"] : 0M;

            data[21] = typeWorks.Where(y => y.Code == "30").Sum(y => y.VolumeOfCompletion);
            data[22] = typeWorks.Where(y => y.Code == "30").Sum(y => y.CostSum);

            data[23] = typeWorks.Sum(y => y.CostSum);

            return data;
        }

        private class TypeWorkForFundProxy
        {
            public string Code;

            public decimal CostSum;

            public decimal VolumeOfCompletion;

            public TypeWork type;
        }

        public override string ReportGenerator
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }
    }
}