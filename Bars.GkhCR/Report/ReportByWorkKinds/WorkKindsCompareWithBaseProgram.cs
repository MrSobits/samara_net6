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

    public class WorkKindsCompareWithBaseProgram : BasePrintForm
    {
        public IWindsorContainer Container { get; set; }

        private long programmCrId;

        private long additionalProgrammCrId;

        private List<long> municipalityListId;

        private List<long> finSourceListId;

        private DateTime reportDate;

        private int rowNum;

        private readonly List<string> workCodes = new List<string> { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", 
            "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23","88","141","142","143", "1018", "1019", "1020" };

        private readonly List<string> workCodesNotCmp = new List<string> { "1018", "1019", "1020" };

        private readonly Dictionary<string, string> aggregatedWorkCodes;

        public WorkKindsCompareWithBaseProgram()
            : base(new ReportTemplateBinary(Properties.Resources.WorkKindsCompareWithBaseProgram))
        {
            aggregatedWorkCodes = workCodes.ToDictionary(x => x, x => x);
            aggregatedWorkCodes["1019"] = "1018";
        }

        public override string Name
        {
            get
            {
                return "Отчет по видам работ (сверка с основной программой)";
            }
        }       

        public override string Desciption
        {
            get
            {
                return "Отчет по видам работ (сверка с основной программой)";
            }
        }

        public override string GroupName
        {
            get
            {
                return "Ход капремонта";
            }
        }

        public override string ParamsController
        {
            get
            {
                return "B4.controller.report.CompareWithBaseProgramm";
            }
        }

        public override string RequiredPermission
        {
            get
            {
                return "Reports.CR.WorkKindsCompareWithBaseProgram";
            }
        }

        public override void SetUserParams(BaseParams baseParams)
        {
            var municipalityIdsString = baseParams.Params["municipalityIds"].ToString();
            municipalityListId = string.IsNullOrEmpty(municipalityIdsString)
                                     ? new List<long>()
                                     : municipalityIdsString.Split(',').Select(x => x.ToLong()).ToList();
            
            programmCrId = baseParams.Params["programCrId"].ToInt();
            additionalProgrammCrId = baseParams.Params["programCrAdditionalId"].ToInt();
            reportDate = baseParams.Params["reportDate"].ToDateTime();
            
            var finSourceIdsString = baseParams.Params["financialSourceIds"].ToString();
            finSourceListId = string.IsNullOrEmpty(finSourceIdsString)
                                  ? new List<long>()
                                  : finSourceIdsString.Split(',').Select(x => x.ToLong()).ToList();
        }

        public override string ReportGenerator { get; set; }

        public override void PrepareReport(ReportParams reportParams)
        {
            var serviceMunicipality = Container.Resolve<IDomainService<Municipality>>();
            var serviceFinanceSource = Container.Resolve<IDomainService<FinanceSource>>();
            var serviceProgramCr = Container.Resolve<IDomainService<ProgramCr>>();

            var sectionMain = reportParams.ComplexReportParams.ДобавитьСекцию("sectionMain");
            var sectionGroup = sectionMain.ДобавитьСекцию("sectionGroup");
            var sectionMunicipality = sectionMain.ДобавитьСекцию("sectionMunicipality");
            var sectionObject = sectionMunicipality.ДобавитьСекцию("sectionObject");
            var sectionGroupSummary = sectionMain.ДобавитьСекцию("sectionGroupSummary");
            var sectionSummary = reportParams.ComplexReportParams.ДобавитьСекцию("sectionSummary");

            reportParams.SimpleReportParams["Period"] = serviceProgramCr.Load(additionalProgrammCrId).Period.Name;
            
            var finSourceDict = serviceFinanceSource.GetAll()
                .WhereIf(this.finSourceListId.Count > 0, x => this.finSourceListId.Contains(x.Id))
                .ToDictionary(x => x.Id, x => x.Name);
            
            var muList = serviceMunicipality.GetAll()
                .WhereIf(this.municipalityListId.Count > 0, x => this.municipalityListId.Contains(x.Id))
                .Select(x => new { x.Id, x.Name, x.Group })
                .OrderBy(x => x.Group ?? x.Name)
                .ThenBy(x => x.Name)
                .ToList();

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

            var muNameAndGroupByMuIdDict = muList.ToDictionary(y => y.Id, y => new { y.Name, y.Group });

            var realtyObjectsSubset = GetRealtyObjectSubset();
            var realtyObjectsByMuIdDict = GetRealtyObjectData(realtyObjectsSubset);

            var crData = this.GetCrData(realtyObjectsSubset);

            var summary = new Dictionary<string, decimal>();

            rowNum = 0;

            foreach (var municipalityGroup in alphabeticalGroups.Where(municipalityGroup => municipalityGroup.Any()))
            {
                sectionMain.ДобавитьСтроку();

                var groupName = muNameAndGroupByMuIdDict[municipalityGroup.First()].Group;

                if (!string.IsNullOrEmpty(groupName))
                {
                    sectionGroup.ДобавитьСтроку();
                    sectionGroup["GroupName"] = groupName;
                }
                
                var groupSummary = new Dictionary<string, decimal>();

                foreach (var municipality in municipalityGroup.Where(realtyObjectsByMuIdDict.ContainsKey))
                {
                    sectionMunicipality.ДобавитьСтроку();
                    sectionMunicipality["municipalityName"] = muNameAndGroupByMuIdDict[municipality].Name;

                    var municipalitySummary = new Dictionary<string, decimal>();

                    foreach (var realtyObject in realtyObjectsByMuIdDict[municipality].Where(realtyObject => crData.ContainsKey(realtyObject.id)))
                    {
                        foreach (var dataByFinSource in crData[realtyObject.id])
                        {
                            sectionObject.ДобавитьСтроку();

                            var rowValues = this.FillRow(sectionObject, realtyObject, finSourceDict[dataByFinSource.Key], dataByFinSource.Value);

                            this.AddToSummary(municipalitySummary, rowValues);
                        }
                    }

                    this.FillSummary(sectionMunicipality, municipalitySummary);

                    this.AddToSummary(groupSummary, municipalitySummary);
                }
                
                // Итоги группы
                if (!string.IsNullOrEmpty(groupName))
                {
                    sectionGroupSummary.ДобавитьСтроку();
                    this.FillSummary(sectionGroupSummary, groupSummary);
                }

                this.AddToSummary(summary, groupSummary);
            }

            // Общие итоги
            sectionSummary.ДобавитьСтроку();
            FillSummary(sectionSummary, summary);
        }
        
        private IQueryable<long> GetRealtyObjectSubset()
        {
            var serviceTypeWorkCr = Container.Resolve<IDomainService<TypeWorkCr>>();

            var realtyObjectSubquery = serviceTypeWorkCr.GetAll()
                .Where(x => x.ObjectCr.ProgramCr.Id == this.additionalProgrammCrId)
                .WhereIf(this.municipalityListId.Count > 0, x => this.municipalityListId.Contains(x.ObjectCr.RealityObject.Municipality.Id))
                .WhereIf(this.finSourceListId.Count > 0, x => this.finSourceListId.Contains(x.FinanceSource.Id))
                .Select(x => x.ObjectCr.RealityObject.Id);

            return realtyObjectSubquery;
        }

        private Dictionary<long, List<RealtyObjectProxy>> GetRealtyObjectData(IQueryable<long> realtyObjectsSubset)
        {
            var serviceRealityObject = Container.Resolve<IDomainService<RealityObject>>();
            
            var realtyObjectsByMuIdDict = serviceRealityObject.GetAll()
                .Where(x => realtyObjectsSubset.Contains(x.Id))
                .Select(x => new
                {
                    muId = x.Municipality.Id,
                    RealityObjectId = x.Id,
                    x.Address,
                    RealityObjectFloors = x.MaximumFloors,
                    x.AreaMkd,
                    x.NumberApartments,
                    x.NumberLiving,
                    WallMaterial = x.WallMaterial.Name,
                    RoofingMaterial = x.RoofingMaterial.Name,
                    x.TypeRoof
                })
                .OrderBy(x => x.Address)
                .AsEnumerable()
                .Select(x => new RealtyObjectProxy
                {
                    municipalityId = x.muId,
                    id = x.RealityObjectId,
                    address = x.Address,
                    wallMaterial = x.WallMaterial,
                    roofingMaterial = x.RoofingMaterial,
                    technicalCharacteristics = new TechnicalCharacteristics
                    {
                        TypeRoof = x.TypeRoof,
                        CitizensNum = x.NumberLiving,
                        FlatsNum = x.NumberApartments,
                        Storeys = x.RealityObjectFloors,
                        TotalArea = x.AreaMkd
                    }
                })
                .GroupBy(x => x.municipalityId)
                .ToDictionary(x => x.Key, x => x.ToList());

            return realtyObjectsByMuIdDict;
        }

        private Dictionary<long, Dictionary<long, Dictionary<string, TypeWorkCrInfo>>> GetCrData(IQueryable<long> realtyObjectsSubset)
        {
            var serviceTypeWorkCr = Container.Resolve<IDomainService<TypeWorkCr>>();
            var servicePerformedWorkAct = Container.Resolve<IDomainService<PerformedWorkAct>>();
            
           // Данные по программе 2 - для вывода в отчет
            var workDataByCodeByFinSrcByRoDict = serviceTypeWorkCr.GetAll()
                .Where(x => realtyObjectsSubset.Contains(x.ObjectCr.RealityObject.Id))
                .Where(x => x.ObjectCr.ProgramCr.Id == additionalProgrammCrId)
                .WhereIf(this.finSourceListId.Count > 0, x => this.finSourceListId.Contains(x.FinanceSource.Id))
                .Select(x => new
                {
                    roId = x.ObjectCr.RealityObject.Id,
                    financeSourceId = x.FinanceSource.Id,
                    x.Work.Code,
                    x.Work.TypeWork,
                    x.Sum,
                    x.Volume
                })
                .AsEnumerable()
                .GroupBy(x => x.roId)
                .ToDictionary(
                    x => x.Key,
                    x =>
                    x.GroupBy(z => z.financeSourceId)
                        .ToDictionary(
                            w => w.Key,
                            w => w.GroupBy(v => aggregatedWorkCodes.ContainsKey(v.Code) ? aggregatedWorkCodes[v.Code] : "-1")
                                .ToDictionary(v => v.Key, v => new TypeWorkCrProxy
                                                                   {
                                                                       Volume = v.Sum(p => p.Volume), 
                                                                       Sum = v.Sum(p => v.Key == "-1" ? (p.TypeWork == TypeWork.Work ? p.Sum : 0) : p.Sum)
                                                                       /*
                                                                        * Не считаем сумму по услугам, которые не входят в нами определенный список работ, чтобы была верна сумма СМР
                                                                        */
                                                                   })));


            // Данные по программе 1 - лимиты
            var limits = serviceTypeWorkCr.GetAll()
                .Where(x => realtyObjectsSubset.Contains(x.ObjectCr.RealityObject.Id))
                .Where(x => workCodes.Contains(x.Work.Code))
                .Where(x => x.ObjectCr.ProgramCr.Id == programmCrId)
                .WhereIf(this.finSourceListId.Count > 0, x => this.finSourceListId.Contains(x.FinanceSource.Id))
                .Select(x => new
                {
                    roId = x.ObjectCr.RealityObject.Id,
                    financeSourceId = x.FinanceSource.Id,
                    x.Work.Code,
                    x.Sum
                })
                .AsEnumerable()
                .GroupBy(x => x.roId)
                .ToDictionary(
                    x => x.Key,
                    x =>
                    x.GroupBy(z => z.financeSourceId)
                        .ToDictionary(
                            w => w.Key,
                            w => w.GroupBy(v => aggregatedWorkCodes[v.Code])
                                .ToDictionary(v => v.Key, v => v.Sum(p => p.Sum))));

            // Данные по актам по программе 2
            var performedWorkActsDict = servicePerformedWorkAct.GetAll()
                .Where(x => realtyObjectsSubset.Contains(x.TypeWorkCr.ObjectCr.RealityObject.Id))
                .Where(x => workCodes.Contains(x.TypeWorkCr.Work.Code))
                .WhereIf(this.finSourceListId.Count > 0, x => this.finSourceListId.Contains(x.TypeWorkCr.FinanceSource.Id))
                .Where(x => x.TypeWorkCr.ObjectCr.ProgramCr.Id == additionalProgrammCrId)
                .Select(x => new
                {
                    roId = x.ObjectCr.RealityObject.Id,
                    financeSourceId = x.TypeWorkCr.FinanceSource.Id,
                    x.TypeWorkCr.Work.Code,
                    x.Sum,
                    x.Volume
                })
                .AsEnumerable()
                .GroupBy(x => x.roId)
                .ToDictionary(
                    x => x.Key,
                    x =>
                    x.GroupBy(z => z.financeSourceId)
                        .ToDictionary(
                            w => w.Key,
                            w => w.GroupBy(v => aggregatedWorkCodes[v.Code])
                                .ToDictionary(v => v.Key, v => v.Sum(p => p.Sum))));

            var crData = new Dictionary<long, Dictionary<long, Dictionary<string,TypeWorkCrInfo>>>();

            foreach (var roId in workDataByCodeByFinSrcByRoDict.Keys)
            {
                crData[roId] = new Dictionary<long, Dictionary<string, TypeWorkCrInfo>>();

                foreach (var workDataByCode in workDataByCodeByFinSrcByRoDict[roId])
                {
                    var finSourceId = workDataByCode.Key;
                    crData[roId][finSourceId] = new Dictionary<string, TypeWorkCrInfo>();

                    foreach (var workData in workDataByCode.Value)
                    {
                        var workCode = workData.Key;
                        var data = new TypeWorkCrInfo { Sum = workData.Value.Sum, Volume = workData.Value.Volume };

                        if (limits.ContainsKey(roId) 
                            && limits[roId].ContainsKey(finSourceId) 
                            && limits[roId][finSourceId].ContainsKey(workCode))
                        {
                            data.Limit = limits[roId][finSourceId][workCode];
                        }

                        if (performedWorkActsDict.ContainsKey(roId) 
                            && performedWorkActsDict[roId].ContainsKey(finSourceId)
                            && performedWorkActsDict[roId][finSourceId].ContainsKey(workCode))
                        {
                            data.ActSum = performedWorkActsDict[roId][finSourceId][workCode];
                        }
                            
                        crData[roId][finSourceId][workCode] = data;
                    }
                }
            }
            
            return crData;
        }

        private decimal ProcessData(Dictionary<string, TypeWorkCrInfo> typeWorkCrInfoDict, TechnicalCharacteristics technicalCharacteristics)
        {
            var typeWorkCrProxyDict = typeWorkCrInfoDict
                .ToDictionary(x => x.Key, x => new TypeWorkCrProxy { Volume = x.Value.Volume, Sum = x.Value.Sum });

            var smrSum = 0M;

            foreach (var typeWorkCrInfo in typeWorkCrInfoDict.Where(x => !workCodesNotCmp.Contains(x.Key)))
            {
                var info = typeWorkCrInfo.Value;
                info.AdditionalProperty = RepairNorm.GetInfo(typeWorkCrInfo.Key, typeWorkCrProxyDict, technicalCharacteristics, out info.HasError);
                info.HasActDiff = info.Sum.HasValue ? (info.ActSum > info.Sum) : (info.ActSum.HasValue && info.ActSum.Value > 0);
                info.HasLimitDiff = info.Limit.HasValue ? (info.Sum > info.Limit) : (info.Sum.HasValue && info.Sum.Value > 0);
                smrSum += info.Sum.HasValue ? info.Sum.Value : 0;
            }

            if (smrSum > 0)
            {
                foreach (var typeWork in this.workCodesNotCmp.Where(typeWorkCrInfoDict.ContainsKey).Select(code => typeWorkCrInfoDict[code]).Where(work => work.Sum.HasValue))
                {
                    typeWork.AdditionalProperty = (typeWork.Sum.Value / smrSum * 100).ToString();
                }
            }

            return smrSum;
        }

        private Dictionary<string, decimal> FillRow(Section section, RealtyObjectProxy realtyObject, string finSource, Dictionary<string, TypeWorkCrInfo> crData)
        {
            var result = new Dictionary<string, decimal>();

            section["num"] = ++rowNum;
            section["address"] = realtyObject.address;
            section["Storeys"] = realtyObject.technicalCharacteristics.Storeys;
            section["TotalArea"] = realtyObject.technicalCharacteristics.TotalArea;
            section["FlatsNum"] = realtyObject.technicalCharacteristics.FlatsNum;
            section["CitizensNum"] = realtyObject.technicalCharacteristics.CitizensNum;
            section["wallMaterial"] = realtyObject.wallMaterial;
            section["roofingMaterial"] = realtyObject.roofingMaterial;
            section["TypeRoof"] = realtyObject.technicalCharacteristics.TypeRoof.GetEnumMeta().Display;
            section["finSource"] = finSource;

            result["TotalArea"] = realtyObject.technicalCharacteristics.TotalArea.HasValue ? realtyObject.technicalCharacteristics.TotalArea.Value : 0;
            result["FlatsNum"] = realtyObject.technicalCharacteristics.FlatsNum.HasValue ? realtyObject.technicalCharacteristics.FlatsNum.Value : 0;
            result["CitizensNum"] = realtyObject.technicalCharacteristics.CitizensNum.HasValue ? realtyObject.technicalCharacteristics.CitizensNum.Value : 0;

            var sum = this.ProcessData(crData, realtyObject.technicalCharacteristics);

            section["sumSmr"] = sum;
            result["sumSmr"] = sum;

            var aggregatedCodes = new List<string>();

            foreach (var work in crData.Where(x => aggregatedWorkCodes.ContainsValue(x.Key)))
            {
                section["Sum" + work.Key] = work.Value.Sum;
                section["Volume" + work.Key] = work.Value.Volume;
                section["Info" + work.Key] = work.Value.AdditionalProperty;
                section["ActDiff" + work.Key] = work.Value.HasActDiff ? "1" : string.Empty;

                if (!workCodesNotCmp.Contains(work.Key))
                {
                    section["LimitDiff" + work.Key] = work.Value.HasError;
                }

                if (work.Value.Sum.HasValue)
                {
                    result[work.Key] = work.Value.Sum.Value;
                }
                
                aggregatedCodes.Add(work.Key); 
            }

            foreach (var work in aggregatedWorkCodes)
            {
                if (!aggregatedCodes.Contains(work.Key) && !workCodesNotCmp.Contains(work.Key))
                {
                    section["LimitDiff" + work.Key] = false;
                }
            }

            var validWorksData = crData.Where(x => workCodes.Contains(x.Key) && !workCodesNotCmp.Contains(x.Key)).ToList();

            if (validWorksData.Any())
            {
                section["AnyError"] = validWorksData.Any(x => x.Value.HasError) ? "1" : string.Empty;
                section["AnyLimitDiff"] = validWorksData.Any(x => x.Value.HasLimitDiff) ? "1" : string.Empty;
                section["AnyActDiff"] = validWorksData.Any(x => x.Value.HasActDiff) ? "1" : string.Empty;
            }
            else
            {
                section["AnyError"] = string.Empty;
                section["AnyLimitDiff"] = string.Empty;
                section["AnyActDiff"] = string.Empty;
            }
            
            return result;
        }

        private void FillSummary(Section section, IDictionary<string, decimal> summary)
        {
            foreach (var kvPair in summary)
            {
                section["Total" + kvPair.Key] = kvPair.Value;
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
    }

    internal class RealtyObjectProxy
    {
        public long municipalityId;

        public long id;

        public string address;

        public string wallMaterial;

        public string roofingMaterial;

        public TechnicalCharacteristics technicalCharacteristics;
    }
}