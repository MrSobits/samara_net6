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
    /// Еженедельный отчет по кап ремонту
    /// </summary>
    public class WeeklyAboutCrProgress : BasePrintForm
    {
        public IWindsorContainer Container { get; set; }

        // идентификаторы муниципальных образований
        private List<long> municipalityIds = new List<long>();
        private List<long> financeSourceIds = new List<long>();
        private long programCrId;

        private DateTime reportDate;

        public WeeklyAboutCrProgress()
            : base(new ReportTemplateBinary(Properties.Resources.WeeklyAboutCrProgress))
        {
        }

        public override string RequiredPermission
        {
            get
            {
                return "Reports.CR.WeeklyAboutCrProgress";
            }
        }

        public override string Name
        {
            get { return "Еженедельный отчет"; }
        }

        public override string Desciption
        {
            get { return "Еженедельный отчет"; }
        }

        public override string GroupName
        {
            get { return "Отчеты кап.ремонта"; }
        }

        public override string ParamsController
        {
            get { return "B4.controller.report.WeeklyAboutCrProgress"; }
        }

        public override void SetUserParams(BaseParams baseParams)
        {
            this.programCrId = baseParams.Params["programCrId"].ToInt();

            this.reportDate = baseParams.Params["reportDate"].ToDateTime();

            var municipalityStr = baseParams.Params.GetAs("municipalityIds", string.Empty);
            this.municipalityIds = string.IsNullOrEmpty(municipalityStr)
                                       ? new List<long>()
                                       : municipalityStr.Split(',').Select(x => x.ToLong()).ToList();
 
            var finSourcesStr = baseParams.Params["finSources"].ToString();

            this.financeSourceIds = string.IsNullOrEmpty(finSourcesStr)
                                       ? new List<long>()
                                       : finSourcesStr.Split(',').Select(x => x.ToLong()).ToList();
        }

        public override string ReportGenerator { get; set; }

        public override void PrepareReport(ReportParams reportParams)
        {
            var objectCrData = this.Container.Resolve<IDomainService<ObjectCr>>().GetAll()
                .Where(x => x.ProgramCr.Id == this.programCrId)
                .WhereIf(this.municipalityIds.Count > 0, x => this.municipalityIds.Contains(x.RealityObject.Municipality.Id))
                .Select(x => new
                {
                    objectCrId = x.Id,
                    municipalityName = x.RealityObject.Municipality.Name,
                    municipalityId = x.RealityObject.Municipality.Id,
                    x.RealityObject.Id,
                    numberLiving = x.RealityObject.NumberLiving ?? 0,
                    areaMkd = x.RealityObject.AreaMkd ?? 0
                })
            .ToList();

            // запрос на работы по программе кап.ремонта
            var typeWorkQuery = this.Container.Resolve<IDomainService<TypeWorkCr>>().GetAll()
                .Where(x => x.ObjectCr.ProgramCr.Id == this.programCrId)
                .WhereIf(this.municipalityIds.Count > 0, x => this.municipalityIds.Contains(x.ObjectCr.RealityObject.Municipality.Id))
                .WhereIf(this.financeSourceIds.Count > 0, x => this.financeSourceIds.Contains(x.FinanceSource.Id))
                .Where(x => x.Work.TypeWork == TypeWork.Work)
                .Where(x => x.Sum != 0m);

            // id работ
            var typeWorkCrIds = typeWorkQuery.Select(x => x.Id);

            // виды работы кап.ремонта из Архива на дату отчета
            var archiveSmrRec = this.Container.Resolve<IDomainService<ArchiveSmr>>().GetAll()
                .Where(x => typeWorkCrIds.Contains(x.TypeWorkCr.Id))
                .Where(x => x.DateChangeRec <= this.reportDate)
                .Select(x => new
                {
                    x.Id,
                    x.DateChangeRec,
                    objectCrId = x.TypeWorkCr.ObjectCr.Id,
                    TypeWorkId = x.TypeWorkCr.Id,
                    CompletionPercent = x.PercentOfCompletion ?? 0
                })
                .AsEnumerable()
                .GroupBy(x => x.objectCrId)
                .ToDictionary(
                    x => x.Key,
                    x => x.GroupBy(y => y.TypeWorkId)
                          .ToDictionary(
                              y => y.Key, 
                              y => y.OrderByDescending(z => z.DateChangeRec)
                                    .ThenByDescending(z => z.Id)
                                    .First().CompletionPercent));

            // виды работ кап.ремонта по объектам КР
            var objectCrTypeWorkData = typeWorkQuery
                .Select(x => new
                {
                    typeWorkId = x.Id,
                    objectCrId = x.ObjectCr.Id,
                    Percent = x.PercentOfCompletion ?? 0
                })
                .AsEnumerable()
                .GroupBy(x => x.objectCrId)
                .ToDictionary(
                    x => x.Key, 
                    x =>
                    {
                        if (archiveSmrRec != null && archiveSmrRec.ContainsKey(x.Key))
                        {
                            var objectCrArchiveRecs = archiveSmrRec[x.Key];
                            return x.ToDictionary(y => y.typeWorkId, y => objectCrArchiveRecs.ContainsKey(y.typeWorkId) ? objectCrArchiveRecs[y.typeWorkId] : y.Percent);
                        }
                        else
                        {
                            return x.ToDictionary(y => y.typeWorkId, y => y.Percent);
                        }
                    });
            
            var objectCrState = objectCrTypeWorkData
                .ToDictionary(
                    x => x.Key, 
                    x => new
                        {
                            allIs100 = x.Value.All(y => y.Value == 100), 
                            allIs0 = x.Value.All(y => y.Value == 0)
                        });

            // id объектов кр с не завершенными работами
            var ongoingRepairsCrObjIds = objectCrState.Where(x => !x.Value.allIs0 && !x.Value.allIs100).ToDictionary(x => x.Key);

            // id объектов кр с завершенными работами
            var completedRepairsCrObjIds = objectCrState.Where(x => x.Value.allIs100).ToDictionary(x => x.Key);
            
            // данные по актам работ
            var workActs = this.Container.Resolve<IDomainService<PerformedWorkAct>>().GetAll()
                .Where(x => typeWorkCrIds.Contains(x.TypeWorkCr.Id))
                .Where(x => x.DateFrom <= this.reportDate)
                .Select(x => new
                {
                    objectCrId = x.ObjectCr.Id, 
                    sum = x.Sum
                })
                .AsEnumerable()
                .GroupBy(x => x.objectCrId)
                .ToDictionary(x => x.Key, x => x.Sum(y => y.sum ?? 0));

            var data = objectCrData.GroupBy(x => new { x.municipalityId, x.municipalityName })
                .ToDictionary(
                    x => x.Key, 
                    x =>
                        {
                            // кол-во домов КР, где ведутся работы
                            var ongoingRepairsObjectsCount = x.Count(y => ongoingRepairsCrObjIds.ContainsKey(y.objectCrId));

                            // данные об объекте КР с выполненными работами
                            var repairCompleteObjectsData = x.Where(y => completedRepairsCrObjIds.ContainsKey(y.objectCrId))
                                .Select(y => new
                                {
                                    y.areaMkd,
                                    y.numberLiving,
                                    workActsSum = workActs.ContainsKey(y.objectCrId) ? workActs[y.objectCrId] : 0
                                })
                                .ToList();

                            return new
                            {
                                ongoingRepairsObjectsCount, 
                                objectCrCount = x.Count(),
                                repairCompleteObjectsCount = repairCompleteObjectsData.Count,
                                repairCompleteObjectsArea = repairCompleteObjectsData.Sum(z => z.areaMkd),
                                repairCompleteObjectsNumberLiving = repairCompleteObjectsData.Sum(z => z.numberLiving),
                                repairCompleteObjectsWorkActsSum = repairCompleteObjectsData.Sum(z => z.workActsSum),
                            };
                        });
            
            var section = reportParams.ComplexReportParams.ДобавитьСекцию("section");

            var i = 0;

            Func<decimal, string> printVal = x => x > 0 ? x.ToStr() : string.Empty;

            foreach (var municipalityData in data.OrderBy(x => x.Key.municipalityName))
            {
                section.ДобавитьСтроку();
                section["Номер1"] = ++i;
                section["МуниципальноеОбразование"] = municipalityData.Key.municipalityName;
                section["КолвоДомовПоПрограмме"] = printVal(municipalityData.Value.objectCrCount);
                section["КолвоДомовВедутсяРаботы"] = printVal(municipalityData.Value.ongoingRepairsObjectsCount);
                section["КолвоДомовЗавершены"] = printVal(municipalityData.Value.repairCompleteObjectsCount);
                section["ПлощадьДомовЗавершены"] = printVal(municipalityData.Value.repairCompleteObjectsArea);
                section["ЧислоЖителейЗавершены"] = printVal(municipalityData.Value.repairCompleteObjectsNumberLiving);
                section["ОбъемСредствЗавершены"] = printVal(municipalityData.Value.repairCompleteObjectsWorkActsSum);
            }

            reportParams.SimpleReportParams["ИтогоКолвоДомовПоПрограмме"] = printVal(objectCrData.Count);
            reportParams.SimpleReportParams["ИтогоКолвоДомовВедутсяРаботы"] = printVal(data.Sum(x => x.Value.ongoingRepairsObjectsCount));
            reportParams.SimpleReportParams["ИтогоКолвоДомовЗавершены"] = printVal(data.Sum(x => x.Value.repairCompleteObjectsCount));
            reportParams.SimpleReportParams["ИтогоПлощадьДомовЗавершены"] = printVal(data.Sum(x => x.Value.repairCompleteObjectsArea));
            reportParams.SimpleReportParams["ИтогоЧислоЖителейЗавершены"] = printVal(data.Sum(x => x.Value.repairCompleteObjectsNumberLiving));
            reportParams.SimpleReportParams["ИтогоОбъемСредствЗавершены"] = printVal(data.Sum(x => x.Value.repairCompleteObjectsWorkActsSum));
        }
    }
}