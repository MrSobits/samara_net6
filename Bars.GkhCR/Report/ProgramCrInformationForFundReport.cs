namespace Bars.GkhCr.Report
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    
    using B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.GkhCr.Entities;

    using Castle.Windsor;

    public class ProgramCrInformationForFundReport : BasePrintForm
    {
        public IWindsorContainer Container { get; set; }

        private long programId = 0;
        private DateTime reportDate = DateTime.MinValue;
        private long[] finSourceIds;
        private long[] municipalityIds;

        private readonly List<string> column8Codes = new List<string> { "7", "8", "9", "10", "11" };
        private readonly List<string> column22Codes = new List<string> { "1018", "1019", "1020", "12", "15", "18", "19", "20", "21", "22", "23" };

        public ProgramCrInformationForFundReport()
            : base(new ReportTemplateBinary(Properties.Resources.ProgramCrInformationForFund))
        {
        }

        public override string RequiredPermission
        {
            get
            {
                return "Reports.CR.ProgramCrInformationForFund";
            }
        }

        /// <summary>
        /// Описание
        /// </summary>
        public override string Desciption
        {
            get { return "Сведения по программе КР для Фонда"; }
        }
        
        /// <summary>
        /// Группа
        /// </summary>
        public override string GroupName
        {
            get { return "Ход капремонта"; }
        }

        /// <summary>
        /// Представление с пользователскими параметрами
        /// </summary>
        public override string ParamsController
        {
            get { return "B4.controller.report.ProgramCrInformationForFund"; }
        }

        /// <summary>
        /// Наименование
        /// </summary>
        public override string Name
        {
            get { return "Сведения по программе КР для Фонда"; }
        }

        public override void SetUserParams(BaseParams baseParams)
        {
            this.programId = baseParams.Params["programCrId"].ToInt();

            this.reportDate = baseParams.Params["reportDate"].ToDateTime();

            var municipalityIdsList = baseParams.Params.ContainsKey("municipalityIds")
              ? baseParams.Params["municipalityIds"].ToString()
              : string.Empty;

            this.municipalityIds = !string.IsNullOrEmpty(municipalityIdsList) ? municipalityIdsList.Split(',').Select(id => id.ToLong()).ToArray() : new long[0];

            var finSourceIdsList = baseParams.Params.ContainsKey("finSourceIds")
             ? baseParams.Params["finSourceIds"].ToString()
             : string.Empty;

            this.finSourceIds = !string.IsNullOrEmpty(finSourceIdsList) ? finSourceIdsList.Split(',').Select(id => id.ToLong()).ToArray() : new long[0];
        }

        public override string ReportGenerator { get; set; }

        public override void PrepareReport(ReportParams reportParams)
        {
            var program = this.Container.Resolve<IDomainService<ProgramCr>>().Load(this.programId);
            var programYear = program != null ? program.Period.DateStart.Year : 0;
            
            var muList = this.Container.Resolve<IDomainService<Municipality>>().GetAll()
                .Select(x => new { x.Id, x.Name, Group = x.Group ?? string.Empty })
                .OrderBy(x => x.Group ?? x.Name)
                .ThenBy(x => x.Name)
                .ToList();

            var municipalityDict = muList.ToDictionary(x => x.Id, x => new { x.Name, Group = x.Group ?? string.Empty });

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



            var serviceTypeWorkCr =
                this.Container.Resolve<IDomainService<TypeWorkCr>>()
                    .GetAll()
                    .Where(x => x.ObjectCr.ProgramCr.Id == this.programId)
                    .WhereIf(
                        this.municipalityIds.Length > 0,
                        x => this.municipalityIds.Contains(x.ObjectCr.RealityObject.Municipality.Id))
                    .WhereIf(this.finSourceIds.Length > 0, x => this.finSourceIds.Contains(x.FinanceSource.Id));

            var objectCrDataByMu = serviceTypeWorkCr
                                .Select(x => new
                                                 {
                                                    muId = x.ObjectCr.RealityObject.Municipality.Id,
                                                    objectCrId = x.ObjectCr.Id,
                                                    x.ObjectCr.RealityObject.Address,
                                                    x.ObjectCr.RealityObject.AreaMkd,
                                                    x.ObjectCr.RealityObject.NumberLiving,
                                                    x.Sum,
                                                    x.ObjectCr.DateAcceptCrGji
                                                 })
                                .AsEnumerable()
                                .OrderBy(x => x.Address)
                                .GroupBy(x => x.muId)
                                .ToDictionary(
                                    x => x.Key,
                                    x => x.GroupBy(y => y.objectCrId)
                                            .ToDictionary(
                                                y => y.Key, 
                                                y =>
                                                    { 
                                                        var generaldataByObjCr = y.FirstOrDefault();

                                                        if (generaldataByObjCr == null)
                                                        {
                                                            return null;
                                                        }

                                                        var sum = y.Sum(z => z.Sum);

                                                        return
                                                            new 
                                                                {
                                                                    generaldataByObjCr.Address,
                                                                    generaldataByObjCr.AreaMkd,
                                                                    generaldataByObjCr.NumberLiving,
                                                                    generaldataByObjCr.DateAcceptCrGji,
                                                                    sum = sum / 1000
                                                                };
                                                    }));

            var typeWorkIdsQuery = serviceTypeWorkCr.Select(x => x.Id);

            var dataFact = programYear >= 2013
                ? this.Container.Resolve<IDomainService<ArchiveSmr>>().GetAll()
                                   .Where(x => typeWorkIdsQuery.Contains(x.TypeWorkCr.Id))
                                   .Where(x => x.DateChangeRec <= this.reportDate)
                                   .Select(x => new
                                    {
                                        x.Id,
                                        x.DateChangeRec,
                                        objectCrId = x.TypeWorkCr.ObjectCr.Id,
                                        TypeWorkCrId = x.TypeWorkCr.Id,
                                        x.TypeWorkCr.Work.Code,
                                        CostSum = x.CostSum ?? 0,
                                        VolumeOfCompletion = x.VolumeOfCompletion ?? 0
                                    })
                                    .AsEnumerable()
                                    .GroupBy(x => x.objectCrId)
                                        .ToDictionary(
                                            x => x.Key,
                                            x =>
                                            {
                                                var typeWorks = x.GroupBy(y => y.TypeWorkCrId)
                                                                    .Select(
                                                                        y =>
                                                                            {
                                                                                var typeWork = y.OrderByDescending(
                                                                                    z => z.DateChangeRec)
                                                                                    .ThenByDescending(z => z.Id)
                                                                                    .First();

                                                                                return new TypeWorkForFundProxy()
                                                                                            {
                                                                                                Code = typeWork.Code,
                                                                                                CostSum = typeWork.CostSum,
                                                                                                VolumeOfCompletion = typeWork.VolumeOfCompletion
                                                                                            };
                                                                            })
                                                                    .ToList();

                                                return GetDataDictionary(typeWorks);
                                            })
                : this.Container.Resolve<IDomainService<TypeWorkCr>>().GetAll()
                                   .Where(x => typeWorkIdsQuery.Contains(x.Id))
                                   .Select(x => new
                                    {
                                        objectCrId = x.ObjectCr.Id,
                                        TypeWorkCrId = x.Id,
                                        x.Work.Code,
                                        CostSum = x.CostSum ?? 0,
                                        VolumeOfCompletion = x.VolumeOfCompletion ?? 0
                                    })
                                    .AsEnumerable()
                                    .GroupBy(x => x.objectCrId)
                                        .ToDictionary(
                                            x => x.Key,
                                            x =>
                                            {
                                                var typeWorks = x.Select(typeWork => new TypeWorkForFundProxy
                                                                                          {
                                                                                              Code = typeWork.Code,
                                                                                              CostSum = typeWork.CostSum,
                                                                                              VolumeOfCompletion = typeWork.VolumeOfCompletion
                                                                                          })
                                                                .ToList();

                                                return GetDataDictionary(typeWorks);
                                            });


            var sectionGroup = reportParams.ComplexReportParams.ДобавитьСекцию("sectionGroup");
            var sectionGroupName = sectionGroup.ДобавитьСекцию("sectionGroupName");
            var sectionGroupTotals = sectionGroup.ДобавитьСекцию("sectionGroupTotals");
            var sectionMu = sectionGroup.ДобавитьСекцию("sectionMu");
            var sectionRo = sectionMu.ДобавитьСекцию("sectionRo");
            var sectionTotal = reportParams.ComplexReportParams.ДобавитьСекцию("sectionTotal");

            var counter = 0;
            var totals = new Dictionary<int, decimal>();
            foreach (var group in alphabeticalGroups)
            {
                sectionGroup.ДобавитьСтроку();
                var totalsGr = new Dictionary<int, decimal>();

                if (!group.Any(objectCrDataByMu.ContainsKey))
                {
                    continue;
                }

                var hasGroup = false;
                var groupName = municipalityDict[group.First()].Group;
                if (groupName != string.Empty)
                {
                    sectionGroupName.ДобавитьСтроку();
                    sectionGroupName["groupName"] = municipalityDict[group.First()].Group;
                    hasGroup = true;
                }

                foreach (var muId in group)
                {
                    if (!objectCrDataByMu.ContainsKey(muId))
                    {
                        continue;
                    }

                    var totalsMu = new Dictionary<int, decimal>();
                    totalsMu[25] = 0M;
                    sectionMu.ДобавитьСтроку();
                    sectionMu["muName"] = municipalityDict[muId].Name;

                    var crObjects = objectCrDataByMu[muId];

                    foreach (var crObject in crObjects)
                    {
                        if (crObject.Value == null)
                        {
                            continue;
                        }

                        sectionRo.ДобавитьСтроку();
                        sectionRo["column1"] = ++counter;
                        sectionRo["column3"] = crObject.Value.Address;
                        sectionRo["column4"] = crObject.Value.AreaMkd;
                        sectionRo["column5"] = crObject.Value.NumberLiving;
                        sectionRo["column6"] = crObject.Value.sum != null ? crObject.Value.sum.ToStr() : string.Empty;

                        if (crObject.Value.DateAcceptCrGji.HasValue)
                        {
                            sectionRo["column24"] = crObject.Value.DateAcceptCrGji.Value.ToShortDateString();
                            if (crObject.Value.sum != null)
                            {
                                sectionRo["column25"] = crObject.Value.sum / 1000;
                                totalsMu[25] += crObject.Value.sum / 1000 ?? 0;
                            }
                        }

                        if (dataFact.ContainsKey(crObject.Key))
                        {
                            var currentCrObjFactData = dataFact[crObject.Key];

                            this.FillSection(sectionRo, currentCrObjFactData, string.Empty);
                            this.AttachDictionaries(totalsMu, currentCrObjFactData);
                        }
                        
                    }

                    // итоги по мун.образованию
                    var crObjValuesByMu = crObjects.Select(x => x.Value).ToList();
                    sectionMu["column4Mu"] = crObjValuesByMu.Sum(x => x.AreaMkd);
                    sectionMu["column5Mu"] = crObjValuesByMu.Sum(x => x.NumberLiving);
                    sectionMu["column6Mu"] = crObjValuesByMu.Sum(x => x.sum);
                    this.FillSection(sectionMu, totalsMu, "Mu");
                    this.AttachDictionaries(totalsGr, totalsMu);
                    this.AttachDictionaries(totals, totalsMu);
                }

                // заполнение итогов по группе
                if (hasGroup)
                {
                    sectionGroupTotals.ДобавитьСтроку();
                    var crObjValuesByGroup = objectCrDataByMu.Where(x => group.Contains(x.Key)).SelectMany(x => x.Value.Select(y => y.Value)).ToList();
                    sectionGroupTotals["column4Gr"] = crObjValuesByGroup.Sum(x => x.AreaMkd);
                    sectionGroupTotals["column5Gr"] = crObjValuesByGroup.Sum(x => x.NumberLiving);
                    sectionGroupTotals["column6Gr"] = crObjValuesByGroup.Sum(x => x.sum);
                    this.FillSection(sectionGroupTotals, totalsGr, "Gr");
                }
            }

            // заполнение итогов по всем мун.образованиям
            sectionTotal.ДобавитьСтроку();
            var crObjValuesTotal = objectCrDataByMu.SelectMany(x => x.Value.Select(y => y.Value)).ToList();
            sectionTotal["column4Total"] = crObjValuesTotal.Sum(x => x.AreaMkd);
            sectionTotal["column5Total"] = crObjValuesTotal.Sum(x => x.NumberLiving);
            sectionTotal["column6Total"] = crObjValuesTotal.Sum(x => x.sum);
            this.FillSection(sectionTotal, totals, "Total");
        }

        private void FillSection(Section section, Dictionary<int, decimal> data, string postfix)
        {
            for (var i = 7; i <= 25; i++)
            {
                if (data.ContainsKey(i) && data[i] != 0)
                {
                    section[string.Format("column{0}{1}", i.ToStr(), postfix)] = data[i];
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

        private Dictionary<int, decimal> GetDataDictionary(List<TypeWorkForFundProxy> typeWorks)
        {
            var data = new Dictionary<int, decimal>();
            data[8] = typeWorks.Where(y => column8Codes.Contains(y.Code)).Sum(y => y.CostSum) / 1000;
            data[9] = typeWorks.Where(y => y.Code == "6").Sum(y => y.CostSum) / 1000;
            data[10] = typeWorks.Where(y => y.Code == "7").Sum(y => y.CostSum) / 1000;
            data[11] = typeWorks.Where(y => y.Code == "5").Sum(y => y.CostSum) / 1000;
            data[12] = typeWorks.Where(y => y.Code == "2" || y.Code == "3").Sum(y => y.CostSum) / 1000;
            data[13] = typeWorks.Where(y => y.Code == "4").Sum(y => y.CostSum) / 1000;
            data[14] = typeWorks.Where(y => y.Code == "13").Sum(y => y.VolumeOfCompletion);
            data[15] = typeWorks.Where(y => y.Code == "13").Sum(y => y.CostSum) / 1000;
            data[16] = typeWorks.Where(y => y.Code == "14").Sum(y => y.VolumeOfCompletion);
            data[17] = typeWorks.Where(y => y.Code == "14").Sum(y => y.CostSum) / 1000;
            data[18] = typeWorks.Where(y => column8Codes.Contains(y.Code)).Sum(y => y.VolumeOfCompletion);
            data[19] = typeWorks.Where(y => column8Codes.Contains(y.Code)).Sum(y => y.CostSum) / 1000;
            data[20] = typeWorks.Where(y => y.Code == "16" || y.Code == "17").Sum(y => y.VolumeOfCompletion);
            data[21] = typeWorks.Where(y => y.Code == "16" || y.Code == "17").Sum(y => y.CostSum) / 1000;
            data[22] = typeWorks.Where(y => column22Codes.Contains(y.Code)).Sum(y => y.CostSum) / 1000;
            data[7] = data[8] + data[9] + data[10] + data[11] + data[12] + data[13];
            data[23] = data[7] + data[15] + data[17] + data[19] + data[21] + data[22];

            return data;
        }
    }

    internal sealed class TypeWorkForFundProxy
    {
        public string Code;

        public decimal CostSum;

        public decimal VolumeOfCompletion;
    }
}