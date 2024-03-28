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
    public class ProgrammCrRealization : BasePrintForm
    {
        public IWindsorContainer Container { get; set; }

        // идентификаторы муниципальных образований
        private List<long> municipalityIds = new List<long>();

        private List<long> financeSourceIds = new List<long>();

        private int programCrId;

        private DateTime reportDate;

        public ProgrammCrRealization()
            : base(new ReportTemplateBinary(Properties.Resources.ProgrammCrRealization))
        {
        }

        public override string RequiredPermission
        {
            get
            {
                return "Reports.CR.ProgrammCrRealization";
            }
        }

        public override string Name
        {
            get
            {
                return "Реализация программы капитального ремонта";
            }
        }

        public override string Desciption
        {
            get
            {
                return "Реализация программы капитального ремонта";
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
                return "B4.controller.report.ProgrammCrRealization";
            }
        }

        public override void SetUserParams(BaseParams baseParams)
        {
            this.programCrId = baseParams.Params.ContainsKey("programCrId") ? baseParams.Params["programCrId"].ToInt() : 0;

            this.reportDate = baseParams.Params["reportDate"].ToDateTime();

            this.municipalityIds.Clear();

            var municipalityStr = baseParams.Params["municipalityIds"].ToString();
            if (!string.IsNullOrEmpty(municipalityStr))
            {
                var mcp = municipalityStr.Split(',');
                foreach (var id in mcp)
                {
                    int mcp_id = 0;
                    if (int.TryParse(id, out mcp_id))
                    {
                        if (!this.municipalityIds.Contains(mcp_id))
                        {
                            this.municipalityIds.Add(mcp_id);
                        }
                    }
                }
            }

            this.financeSourceIds.Clear();

            var finSourcesStr = baseParams.Params["finSources"].ToString();
            if (!string.IsNullOrEmpty(finSourcesStr))
            {
                var fin = finSourcesStr.Split(',');
                foreach (var id in fin)
                {
                    int fin_id = 0;
                    if (int.TryParse(id, out fin_id))
                    {
                        if (!financeSourceIds.Contains(fin_id))
                        {
                            financeSourceIds.Add(fin_id);
                        }
                    }
                }
            }

            if (this.financeSourceIds.Count == 0)
            {
                this.financeSourceIds =
                    this.Container.Resolve<IDomainService<FinanceSource>>().GetAll().Select(x => x.Id).ToList();
            }
        }

        public override string ReportGenerator { get; set; }

        public override void PrepareReport(ReportParams reportParams)
        {
            var serviceObjectCr = this.Container.Resolve<IDomainService<ObjectCr>>();
            var serviceTypeWorkCr = this.Container.Resolve<IDomainService<TypeWorkCr>>();
            var serviceArchiveSmr = this.Container.Resolve<IDomainService<ArchiveSmr>>();
            var servFinanceSourceResource = this.Container.Resolve<IDomainService<FinanceSourceResource>>();
            var servPerformedWorkAct = this.Container.Resolve<IDomainService<PerformedWorkAct>>();
            
            // количество домов в программе КР по Мун.Образованиям
            var countByMu = serviceObjectCr.GetAll()
                .Where(x => x.ProgramCr.Id == this.programCrId)
                .WhereIf(this.municipalityIds.Count > 0, x => this.municipalityIds.Contains(x.RealityObject.Municipality.Id))
                .OrderBy(x => x.RealityObject.Municipality.Name)
                .GroupBy(x => new { x.RealityObject.Municipality.Id, x.RealityObject.Municipality.Name })
                .Select(x => new { munion = x.Key, Count = x.Count() })
                .ToDictionary(x => x.munion.Id, x => new { x.munion.Name, x.Count });
            
            // Средства источников финансирования по Mo
            var objectCrFinSourceResource = servFinanceSourceResource.GetAll()
                .Where(x => x.ObjectCr.ProgramCr.Id == this.programCrId)
                .WhereIf(this.financeSourceIds.Count > 0, x => this.financeSourceIds.Contains(x.FinanceSource.Id))
                .WhereIf(this.municipalityIds.Count > 0, x => this.municipalityIds.Contains(x.ObjectCr.RealityObject.Municipality.Id))
                .GroupBy(x => x.ObjectCr.RealityObject.Municipality.Id)
                .Select(x => new { x.Key, totalSum = x.Sum(y => y.FundResource + y.BudgetSubject + y.BudgetMu + y.OwnerResource) })
                .ToDictionary(x => x.Key, x => x.totalSum);

            // виды работы КР по объектам КР
            var typeWorkCrdata = serviceTypeWorkCr.GetAll()
                .Where(x => x.ObjectCr.ProgramCr.Id == this.programCrId)
                .WhereIf(this.municipalityIds.Count > 0, x => this.municipalityIds.Contains(x.ObjectCr.RealityObject.Municipality.Id))
                .WhereIf(this.financeSourceIds.Count > 0, x => this.financeSourceIds.Contains(x.FinanceSource.Id))
                .Select(x => new 
                {
                    MoId = x.ObjectCr.RealityObject.Municipality.Id,
                    typeWorkData = new TypeWorkCrs 
                    {
                        Id = x.Id,
                        CrId = x.ObjectCr.Id,
                        Percent = x.PercentOfCompletion ?? 0,
                        CostSum = x.CostSum,
                        isWork = x.Work.TypeWork == TypeWork.Work,
                        StartWorkDate = x.DateStartWork,
                        EndWorkDate = x.DateEndWork
                    }
                })
                .ToArray();

            var archiveSmr = serviceArchiveSmr.GetAll()
                .Where(x => x.TypeWorkCr.ObjectCr.ProgramCr.Id == this.programCrId)
                .Where(x => x.DateChangeRec <= reportDate)
                .WhereIf(this.municipalityIds.Count > 0, x => this.municipalityIds.Contains(x.TypeWorkCr.ObjectCr.RealityObject.Municipality.Id))
                .Select(x => new
                {
                    MoId = x.TypeWorkCr.ObjectCr.RealityObject.Municipality.Id,
                    x.CostSum,
                    x.PercentOfCompletion,
                    x.DateChangeRec,
                    x.Id,
                    typeWorkCrId = x.TypeWorkCr.Id
                })
                .AsEnumerable()
                .GroupBy(y => y.typeWorkCrId)
                .ToDictionary(
                    x => x.Key, 
                    y => y.OrderByDescending(z => z.DateChangeRec)
                        .ThenByDescending(z => z.Id)
                        .First());

            var workCrdata = typeWorkCrdata
                .GroupBy(x => x.MoId)
                .ToDictionary(
                    x => x.Key, 
                    x =>
                    {
                        var workList = x.Select(y =>
                            {
                                var res = y.typeWorkData;

                                var archRec = archiveSmr.ContainsKey(y.typeWorkData.Id)
                                                    ? archiveSmr[y.typeWorkData.Id]
                                                    : null;

                                if (archRec != null)
                                {
                                    res.Percent = archRec.PercentOfCompletion ?? 0;
                                    res.CostSum = archRec.CostSum;
                                }

                                return res;
                            })
                            .ToList();

                        var reportTypeWorkGrouped = workList
                            .GroupBy(y => y.CrId)
                            .ToDictionary(
                                y => y.Key,
                                y =>
                                new
                                {
                                    allIs100 = y.All(z => z.Percent == 100),
                                    allIs0 = y.All(z => z.Percent == 0)
                                });

                        // Ид объектов кр с не завершенными работами
                        var ongoingRepairsCrObjIds = reportTypeWorkGrouped.Count(y => !y.Value.allIs0 && !y.Value.allIs100);

                        // Ид объектов кр с завершенными работами
                        var completedRepairsCrObjIds = reportTypeWorkGrouped.Count(y => y.Value.allIs100);

                        var workProgressPercents = workList
                            .Where(y => y.isWork)
                            .Where(y => y.StartWorkDate.HasValue)
                            .Where(y => y.EndWorkDate.HasValue)
                            .Where(y => y.StartWorkDate != DateTime.MinValue)
                            .Where(y => y.EndWorkDate != DateTime.MinValue)
                            .Where(y => y.StartWorkDate != y.EndWorkDate)
                            .Select(y =>
                                {
                                    var percentGraphic = 0m;
                                    if (y.StartWorkDate < reportDate.Date)
                                    {
                                        percentGraphic = ((reportDate.Date - y.StartWorkDate.Value.Date).TotalDays / ((y.EndWorkDate.Value.Date - y.StartWorkDate.Value.Date).TotalDays + 1) * 100).ToDecimal();
                                    }

                                    percentGraphic = percentGraphic > 100 ? 100 : percentGraphic;

                                    var percentFact = y.Percent > 100 ? 100 : y.Percent;

                                    var gap = percentFact == 100 || percentFact > percentGraphic
                                             ? 0
                                             : (percentGraphic - percentFact).RoundDecimal(2);

                                    return new { gap, percentGraphic };
                                })
                            .ToList();
                        
                        return new
                        {
                            costSum = workList.Sum(y => y.CostSum),
                            percent = workList.Any(y => y.isWork) ? workList.Where(y => y.isWork).Average(y => y.Percent) : 0,
                            ongoingRepairsCrObjIds,
                            completedRepairsCrObjIds,
                            graphPercent = workProgressPercents.Any() ? workProgressPercents.Average(y => y.percentGraphic) : 0,
                            gapPercent = workProgressPercents.Any() ? workProgressPercents.Average(y => y.gap) : 0
                        };
                    });

            var workActs = servPerformedWorkAct.GetAll()
                .Where(x => x.ObjectCr.ProgramCr.Id == this.programCrId)
                .WhereIf(this.financeSourceIds.Count > 0, x => this.financeSourceIds.Contains(x.TypeWorkCr.FinanceSource.Id))
                .WhereIf(this.municipalityIds.Count > 0, x => this.municipalityIds.Contains(x.ObjectCr.RealityObject.Municipality.Id))
                .Where(x => x.State.Name == "Принято ГЖИ" || x.State.Name == "Принято ТОДК")
                .GroupBy(x => x.ObjectCr.RealityObject.Municipality.Id)
                .Select(x => new
                {
                    x.Key,
                    Count = x.Count(),
                    Sum = x.Sum(y => y.Sum)
                })
                .ToDictionary(x => x.Key, x => new { x.Count, x.Sum });

            int i = 0;
           
            var section = reportParams.ComplexReportParams.ДобавитьСекцию("section");
            reportParams.SimpleReportParams["Date"] = this.reportDate.ToShortDateString();

            var totals = new Dictionary<string, decimal>();

            Action<string, decimal> fillCell = (field, value) =>
            {
                section[field] = value;

                if (totals.ContainsKey(field))
                {
                    totals[field] += value;
                }
                else
                {
                    totals[field] = value;
                }
            };
            
            foreach (var munucipalityData in countByMu)
            {
                section.ДобавитьСтроку();
                var munucipalityid = munucipalityData.Key;

                var financeLimit = objectCrFinSourceResource.ContainsKey(munucipalityid) ? objectCrFinSourceResource[munucipalityid] : null;
                var sumWorkActs = workActs.ContainsKey(munucipalityid) ? workActs[munucipalityid].Sum : null;
                var actCount = workActs.ContainsKey(munucipalityid) ? workActs[munucipalityid].Count : 0;
                decimal? finishWork = null;
                
                if (workCrdata.ContainsKey(munucipalityid))
                {
                    var workCrData = workCrdata[munucipalityid];

                    finishWork = workCrData.costSum;

                    fillCell("FinishWork", finishWork ?? 0);
                    fillCell("StartWork", workCrData.ongoingRepairsCrObjIds);
                    fillCell("EndHouseCr", workCrData.completedRepairsCrObjIds);
                    fillCell("FactProgramm", workCrData.percent);
                    fillCell("GraphProgramm", workCrData.graphPercent);
                    fillCell("BacklogAtGraph", workCrData.gapPercent);
                }

                var finishWorkAtLimit = finishWork.HasValue && financeLimit.HasValue && financeLimit.Value != 0 ? (decimal?)(finishWork.Value / financeLimit.Value * 100) : null;
                var gjiActWork = sumWorkActs.HasValue && finishWork.HasValue && finishWork.Value != 0 ? (decimal?)(sumWorkActs.Value / finishWork.Value * 100) : null;
                var gjiActAtLimit = sumWorkActs.HasValue && financeLimit.HasValue && financeLimit.Value != 0 ? (decimal?)(sumWorkActs.Value / financeLimit.Value * 100) : null;

                section["Number"] = ++i;
                section["MOName"] = munucipalityData.Value.Name;

                fillCell("FinanceLimit", financeLimit ?? 0);
                fillCell("HousesCount", munucipalityData.Value.Count);
                fillCell("FinishWorkAtLimit", finishWorkAtLimit ?? 0);
                fillCell("Summ", sumWorkActs ?? 0);
                fillCell("ActCount", actCount);
                fillCell("GjiActWork", gjiActWork ?? 0);
                fillCell("GjiActAtLimit", gjiActAtLimit ?? 0);
            }

            var dividers = totals.ToDictionary(x => x.Key, x => 1);
            var divider = i > 0 ? i : 1;
            dividers["FinishWorkAtLimit"] = divider;
            dividers["GjiActWork"] = divider;
            dividers["GjiActAtLimit"] = divider;
            dividers["FactProgramm"] = divider;
            dividers["GraphProgramm"] = divider;
            dividers["BacklogAtGraph"] = divider;

            totals.ForEach(x => reportParams.SimpleReportParams["Sum" + x.Key] = x.Value / dividers[x.Key]);
        }
    }
    
    internal sealed class TypeWorkCrs
    {
        public long Id { get; set; }

        public long CrId { get; set; }

        public decimal Percent { get; set; }

        public decimal? CostSum { get; set; }

        public bool  isWork { get; set; }

        public DateTime? StartWorkDate { get; set; }

        public DateTime? EndWorkDate { get; set; }
    }
}
