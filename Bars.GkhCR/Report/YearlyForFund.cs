namespace Bars.GkhCr.Report
{
	using B4.Modules.Reports;
	using Bars.B4;
	using Bars.B4.Utils;
	using Bars.Gkh.Enums;
	using Bars.GkhCr.Entities;
	using Bars.GkhCr.Localizers;
	using Bars.GkhCr.Report.Helper;
	using System.Collections.Generic;
	using System.Linq;

    public class YearlyForFund : BasePrintForm
    {
        public YearlyForFund(ReportTemplateBinary template = null)
            : base(template ?? new ReportTemplateBinary(Properties.Resources.YearlyForFund))
        {
            this.InitCodes();
        }

        #region Properties

        private int programId;

        private List<long> finSourceIds = new List<long>();

        private int assemblyTo = 10;

        private List<long> municipalityIds = new List<long>();

        protected List<string> allVolsCodes;

        protected Dictionary<string, string> workGroups;

        public IDomainService<ProtocolCr> ProtocolCrDomain { get; set; }

        public IDomainService<TypeWorkCr> TypeWorkCrDomain { get; set; }

        public override string RequiredPermission
        {
            get
            {
                return "Reports.CR.YearlyForFund";
            }
        }

        /// <summary>
        /// Описание
        /// </summary>
        public override string Desciption
        {
            get { return "Ежеквартальный и годовой отчеты для Фонда"; }
        }

        /// <summary>
        /// Группа
        /// </summary>
        public override string GroupName
        {
            get { return "Отчеты кап.ремонта"; }
        }

        /// <summary>
        /// Представление с пользователскими параметрами
        /// </summary>
        public override string ParamsController
        {
            get { return "B4.controller.report.YearlyForFund"; }
        }

        /// <summary>
        /// Наименование
        /// </summary>
        public override string Name
        {
            get { return "Ежеквартальный и годовой отчеты для Фонда"; }
        }

        #endregion Properties

        protected virtual void InitCodes()
        {
            workGroups = Enumerable.Range(1, 6).Union(Enumerable.Range(12, 11 /*12 + 11 = 23*/)).Select(x => x.ToStr()).ToDictionary(x => x, x => x);
            allVolsCodes = Enumerable.Range(12, 11 /*12 + 11 = 23*/).Select(x => x.ToStr()).ToList();
        }

        public override void SetUserParams(BaseParams baseParams)
        {
            //10=по ходу работ, 20=по плану
            assemblyTo = baseParams.Params["assemblyTo"].ToInt();

            programId = baseParams.Params["programCrId"].ToInt();

            baseParams.Params["reportDate"].ToDateTime();

            var municipalityIdsList = baseParams.Params.GetAs("municipalityIds", string.Empty);
            municipalityIds = !string.IsNullOrEmpty(municipalityIdsList)
                ? municipalityIdsList.Split(',').Select(id => id.ToLong()).ToList()
                : new List<long>();

            var finSourcesStr = baseParams.Params.GetAs("finSources", string.Empty);
            finSourceIds = !string.IsNullOrEmpty(finSourcesStr)
                ? finSourcesStr.Split(',').Select(id => id.ToLong()).ToList()
                : new List<long>();
        }

        private void AddToDict(Dictionary<string, decimal?> dict, string key, decimal? value)
        {
            if (dict.ContainsKey(key))
                dict[key] += value;
            else
                dict.Add(key, value);
        }

        public override void PrepareReport(ReportParams reportParams)
        {
            var data = TypeWorkCrDomain.GetAll()
                .WhereIf(finSourceIds.Count > 0, x => finSourceIds.Contains(x.FinanceSource.Id))
                .WhereIf(this.municipalityIds.Count > 0, x => this.municipalityIds.Contains(x.ObjectCr.RealityObject.Municipality.Id))

                .Where(x => x.ObjectCr.ProgramCr.Id == programId)
                .Select(x => new
                {
                    ObjectCrId = x.ObjectCr.Id,
                    DateAccept = x.ObjectCr.DateAcceptCrGji,
                    City = x.ObjectCr.RealityObject.FiasAddress.PlaceName,
                    Street = x.ObjectCr.RealityObject.FiasAddress.StreetName,
                    House = x.ObjectCr.RealityObject.FiasAddress.House,
                    Building = x.ObjectCr.RealityObject.FiasAddress.Housing,
                    Municipality = x.ObjectCr.RealityObject.Municipality.Name,
                    Group = x.ObjectCr.RealityObject.Municipality.Group,
                    WorkCode = x.Work.Code,
                    TypeWork = ((TypeWork?)x.Work.TypeWork) ?? 0,
                    Sum = x.Sum,
                    CostSum = x.CostSum,
                    Volume = x.Volume,
                    VolumeOfCompletion = x.VolumeOfCompletion
                })
                .AsEnumerable()
                .GroupBy(x => string.IsNullOrWhiteSpace(x.Group) ? string.Empty : x.Group)
                .ToDictionary(
                    x => x.Key,
                    x => x.GroupBy(y => y.Municipality)
                          .ToDictionary(
                            y => y.Key,
                            y => y.GroupBy(z => new
                                    {
                                        z.ObjectCrId,
                                        z.DateAccept,
                                        z.City,
                                        z.Street,
                                        z.House,
                                        z.Building
                                    })
                                  .ToDictionary(
                                      z => z.Key,
                                      z => z.Select(v => new
                                              {
                                                  v.TypeWork,
                                                  v.WorkCode,
                                                  Sum = ((assemblyTo == 20 ? v.Sum : v.CostSum) ?? 0).RoundDecimal(2),
                                                  Volume = assemblyTo == 20 ? v.Volume : v.VolumeOfCompletion
                                              })
                                            .ToList())));

            var protocolsDict = ProtocolCrDomain.GetAll()
                .WhereIf(this.municipalityIds.Count > 0, x => this.municipalityIds.Contains(x.ObjectCr.RealityObject.Municipality.Id))
                .Where(x => x.ObjectCr.ProgramCr.Id == programId)
                .Where(x => x.TypeDocumentCr.Key == TypeDocumentCrLocalizer.ProtocolCompleteCrKey)
                .Select(x => new { ObjectCrId = x.ObjectCr.Id, x.DateFrom })
                .AsEnumerable()
                .GroupBy(x => x.ObjectCrId)
                .ToDictionary(x => x.Key, x => x.First().DateFrom);

            var sectionGroup = reportParams.ComplexReportParams.ДобавитьСекцию("СекцияГруппа");
            var sectionRaion = sectionGroup.ДобавитьСекцию("СекцияРайон");
            var sectionObject = sectionRaion.ДобавитьСекцию("СекцияОбъект");
            var sectionTotal = sectionRaion.ДобавитьСекцию("СекцияИтоги");
            
            var totalsTotal = new Dictionary<string, decimal?>();
            foreach (var group in data.OrderBy(x => x.Key))
            {
                sectionGroup.ДобавитьСтроку();
                sectionGroup["Group"] = group.Key;
                var totalsRaion = new Dictionary<string, decimal?>();
                var totalsGroup = new Dictionary<string, decimal?>();

                foreach (var municipality in group.Value.OrderBy(x => x.Key))
                {
                    sectionRaion.ДобавитьСтроку();
                    sectionRaion["Raion"] = municipality.Key;
                    
                    var houseNumber = 1;
                    var houses = municipality.Value
                        .OrderBy(x => x.Key.City)
                        .ThenBy(x => x.Key.Street)
                        .ThenBy(x => x.Key.House)
                        .ThenBy(x => x.Key.Building);

                    foreach (var house in houses)
                    {
                        sectionObject.ДобавитьСтроку();
                        decimal? sumEngSys = 0;
                        decimal? totalSum = 0;

                        // распределение суммы услуг по работам
                        var distributedDict = house.Value
                            .Select(y => new SumDistributorTypeWorkProxy
                            {
                                WorkCode = y.WorkCode,
                                TypeWork = y.TypeWork,
                                Sum = y.Sum
                            })
                            .ToList()
                            .GetDistrubutedList(workGroups.Keys.ToList())
                            .GroupBy(y => workGroups[y.WorkCode])
                            .ToDictionary(y => y.Key, y => y.Sum(z => z.Sum));

                        var valuesByWork = house.Value
                            .Where(y => y.TypeWork == TypeWork.Work)
                            .Where(y => workGroups.ContainsKey(y.WorkCode))
                            .GroupBy(y => workGroups[y.WorkCode])
                            .ToDictionary(
                                 y => y.Key,
                                 y => new 
                                 {
                                     costSum = distributedDict.ContainsKey(y.Key) ? distributedDict[y.Key] : 0,
                                     volume = y.Sum(z => z.Volume)
                                 });

                        foreach (var workProxy in valuesByWork)
                        {
                            sectionObject["sum" + workProxy.Key] = workProxy.Value.costSum;
                            totalSum += workProxy.Value.costSum;
                            AddToDict(totalsRaion, "sum" + workProxy.Key, workProxy.Value.costSum);
                        }

                        for (var i = 1; i <= 6; i++)
                        {
                            sumEngSys += valuesByWork.ContainsKey(i.ToString()) ? valuesByWork[i.ToString()].costSum : 0;
                        }
                        
                        // объемы по работам
                        foreach (var workCode in allVolsCodes)
                        {
                            var sum = valuesByWork.ContainsKey(workCode) ? valuesByWork[workCode].volume : 0;
                            sectionObject["volume" + workCode] = sum;
                            AddToDict(totalsRaion, "volume" + workCode, sum);
                        }

                        AddToDict(totalsRaion, "sumEngSys", sumEngSys);
                        AddToDict(totalsRaion, "totalSum", totalSum);

                        sectionObject["Number"] = houseNumber++;
                        sectionObject["City"] = house.Key.City;
                        sectionObject["Street"] = house.Key.Street;
                        sectionObject["HouseNumber"] = house.Key.House;
                        sectionObject["Building"] = house.Key.Building;
                        sectionObject["sumEngSys"] = sumEngSys;
                        sectionObject["totalSum"] = totalSum;
                        sectionObject["actAcceptDate"] = house.Key.DateAccept;
                        sectionObject["actDate"] = protocolsDict.ContainsKey(house.Key.ObjectCrId) ? protocolsDict[house.Key.ObjectCrId].ToString() : string.Empty;
                    }

                    sectionTotal.ДобавитьСтроку();
                    sectionTotal["totals"] = "Итоги по району";
                    foreach (var total in totalsRaion)
                    {
                        sectionTotal[total.Key] = total.Value;
                        AddToDict(totalsGroup, total.Key, total.Value);
                        AddToDict(totalsTotal, total.Key, total.Value);
                    }

                    totalsRaion.Clear();
                }

                // Итоги по группе, если была группа
                if (!string.IsNullOrWhiteSpace(group.Key))
                {
                    sectionTotal.ДобавитьСтроку();
                    sectionTotal["totals"] = "Итоги по группе";
                    foreach (var total in totalsGroup)
                    {
                        sectionTotal[total.Key] = total.Value;
                    }

                    totalsGroup.Clear();
                }
            }

            // Общие итоги
            sectionTotal.ДобавитьСтроку();
            sectionTotal["totals"] = "ИТОГО";
            foreach (var total in totalsTotal)
            {
                sectionTotal[total.Key] = total.Value;
            }
        }

        public override string ReportGenerator { get; set; }
    }
}