namespace Bars.GkhCr.Report
{
    using System;
    using System.Linq;

    using Bars.B4;
    
    using B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.GkhCr.Entities;

    using Castle.Windsor;

    /// <summary>
    /// Информация по подрядчикам по выбранной программе и на заданную дату
    /// </summary>
    public class InformationByBuildersReport : BasePrintForm
    {
        // идентификатор программы КР
        private long programCrId;
        private long[] finSourceIds;
        private DateTime reportDate = DateTime.Now;

        public InformationByBuildersReport()
            : base(new ReportTemplateBinary(Properties.Resources.InformationByBuilders))
        {
        }

        public IWindsorContainer Container { get; set; }

        public override string RequiredPermission
        {
            get { return "Reports.CR.InformationByBuilders"; }
        }

        public override string Desciption
        {
            get { return "Информация по подрядчикам по выбранной программе и на заданную дату"; }
        }

        public override string GroupName
        {
            get { return "Отчеты ГЖИ"; }
        }

        public override string ParamsController
        {
            get { return "B4.controller.report.InformationByBuilders"; }
        }

        public override string Name
        {
            get { return "Информация по подрядчикам по выбранной программе и на заданную дату"; }
        }

        public override void SetUserParams(BaseParams baseParams)
        {
            programCrId = baseParams.Params["programCrId"].ToInt();

            var date = baseParams.Params.ContainsKey("dateReport")
                ? baseParams.Params["dateReport"].ToDateTime()
                : DateTime.MinValue;

            reportDate = date != DateTime.MinValue ? date : DateTime.Now.Date;

            var finSourceIdsList =
                baseParams.Params.ContainsKey("finSourceIds")
                    ? baseParams.Params["finSourceIds"].ToString()
                    : string.Empty;

            finSourceIds = !string.IsNullOrEmpty(finSourceIdsList)
                ? finSourceIdsList.Split(',').Select(id => id.ToLong()).ToArray()
                : new long[0];
        }

        public override string ReportGenerator { get; set; }

        public override void PrepareReport(ReportParams reportParams)
        {
            var serviceBuildContract = Container.Resolve<IDomainService<BuildContract>>();
            var serviceTypeWork = Container.Resolve<IDomainService<TypeWorkCr>>();
            var serviceArchiveSmr = Container.Resolve<IDomainService<ArchiveSmr>>();
            var servicePerformedWorkAct = Container.Resolve<IDomainService<PerformedWorkAct>>();
            var serviceMunicipality = Container.Resolve<IDomainService<Municipality>>();

            var muDict = serviceMunicipality.GetAll()
                .Select(x => new { x.Id, x.Name })
                .OrderBy(x => x.Name)
                .ToDictionary(x => x.Id, x => x.Name);

            muDict[-1] = string.Empty;

            var buildersByMu = serviceBuildContract.GetAll()
                .Where(x => x.ObjectCr.ProgramCr.Id == programCrId)
                .Where(x => x.Builder.Contragent != null)
                .Select(x => new
                {
                    builderName = x.Builder.Contragent.Name,
                    builderId = x.Builder.Id,
                    objectCrId = x.ObjectCr.Id,
                    muId = x.ObjectCr.RealityObject.Municipality.Id
                })
                .AsEnumerable()
                .GroupBy(x => new {x.builderId, x.muId})
                .ToDictionary(
                    x => x.Key,
                    x => new
                    {
                        builderName = x.Max(y => y.builderName),
                        roCount = x.Select(y => y.objectCrId).Distinct().Count(),
                        muId = x.Max(y => y.muId),
                        objectCrIdList = x.Select(y => y.objectCrId).Distinct().ToList()
                    });

            var objectCrIdsQuery = serviceBuildContract.GetAll()
                .Where(x => x.ObjectCr.ProgramCr.Id == programCrId)
                .Where(x => x.Builder.Contragent != null)
                .Select(x => x.ObjectCr.Id);

            var typeWorks = serviceTypeWork.GetAll()
                .Where(x => objectCrIdsQuery.Any(y => y == x.ObjectCr.Id))
                .Where(x => x.Work != null)
                .Where(x => x.FinanceSource != null)
                .WhereIf(finSourceIds.Length > 0, x => finSourceIds.Contains(x.FinanceSource.Id))
                .Select(x => new
                {
                    objectCrId = x.ObjectCr.Id,
                    typeWorkId = x.Id,
                    percentOfCompletion = x.PercentOfCompletion,
                    x.Sum,
                    x.CostSum,
                    dateStart = x.DateStartWork,
                    dateEnd = x.DateEndWork,
                    x.Work.TypeWork
                })
                .AsEnumerable()
                .GroupBy(x => x.objectCrId)
                .ToDictionary(x => x.Key, x => x.ToList());

            var archiveRecords = serviceArchiveSmr.GetAll()
                .Where(x => x.TypeWorkCr.FinanceSource != null)
                .Where(x => objectCrIdsQuery.Any(y => y == x.TypeWorkCr.ObjectCr.Id))
                .WhereIf(finSourceIds.Length > 0, x => finSourceIds.Contains(x.TypeWorkCr.FinanceSource.Id))
                .Where(x => x.ObjectCreateDate <= reportDate)
                .Select(x => new
                {
                    x.Id,
                    x.DateChangeRec,
                    typeWorkId = x.TypeWorkCr.Id,
                    percentOfCompletion = x.PercentOfCompletion,
                    x.CostSum
                })
                .AsEnumerable()
                .GroupBy(x => x.typeWorkId)
                .ToDictionary(x => x.Key, x =>
                {
                    var archiveRec =
                        x.OrderByDescending(p => p.DateChangeRec)
                            .ThenByDescending(p => p.Id)
                            .FirstOrDefault();

                    return new {archiveRec.percentOfCompletion, archiveRec.CostSum};
                });

            var objectCrDataDict = typeWorks.ToDictionary(
                x => x.Key,
                x => x.Value
                      .Select(y =>
                          {
                              var dateStart = y.dateStart.ToDateTime();
                              var dateEnd = y.dateEnd.ToDateTime();

                              var percentGraphic =
                                  dateStart != DateTime.MinValue
                                  && dateEnd != DateTime.MinValue
                                  && dateStart != dateEnd
                                  && dateStart.Date < reportDate.Date
                                      ? ((reportDate.Date - dateStart.Date).TotalDays/
                                         (dateEnd.Date - dateStart.Date).TotalDays).ToDecimal()
                                      : 0m;

                              percentGraphic = (percentGraphic > 1 ? 1 : percentGraphic) * 100;

                              var archRec = archiveRecords.ContainsKey(y.typeWorkId)
                                                ? archiveRecords[y.typeWorkId]
                                                : new { y.percentOfCompletion, y.CostSum };

                              var percentFact = archRec.percentOfCompletion; 

                              percentFact = percentFact > 100 ? 100 : percentFact;

                              return new { percentGraphic, percentFact, archRec.CostSum, y.Sum, y.TypeWork };
                          })
                      .ToList());

            var workDataByBuilder = buildersByMu.ToDictionary(
                x => x.Key,
                x =>
                    {
                        var typeworkDataList = x.Value.objectCrIdList
                            .Where(objectCrDataDict.ContainsKey)
                            .Select(y => 
                                {
                                    var tmpList = objectCrDataDict[y];
                                    var list = tmpList
                                        .Where(z => z.TypeWork == TypeWork.Work)
                                        .ToList();
                                    return new
                                    {
                                        CostSum = list.Sum(z => z.CostSum),
                                        Sum = list.Sum(z => z.Sum),
                                        percentGraphic = list.IsNotEmpty()? list.Average(z => z.percentGraphic): 0,
                                        percentFact = list.IsNotEmpty()? list.Average(z => z.percentFact): 0,
                                        limit = tmpList.Sum(z => z.Sum)
                                    };
                                })
                            .ToList();

                        var anyWork = typeworkDataList.Any();

                        return new
                        {
                            CostSum = anyWork ? typeworkDataList.Sum(y => y.CostSum) : 0,
                            Sum = anyWork ? typeworkDataList.Sum(y => y.Sum) : 0,
                            percentGraphic = anyWork ? typeworkDataList.Average(y => y.percentGraphic) : 0,
                            percentFact = anyWork ? typeworkDataList.Average(y => y.percentFact) : 0,
                            limit = anyWork ? typeworkDataList.Sum(y => y.limit) : 0
                        };
                    });

            var workActByObjectCr = servicePerformedWorkAct.GetAll()
                .Where(x => objectCrIdsQuery.Any(y => y == x.ObjectCr.Id))
                .Where(x => x.TypeWorkCr.Work != null)
                .Where(x => x.TypeWorkCr.FinanceSource != null)
                .WhereIf(finSourceIds.Length > 0, x => finSourceIds.Contains(x.TypeWorkCr.FinanceSource.Id))
                .Where(x => x.State != null && (x.State.Name == "Принято ГЖИ" || x.State.Name == "Принят ТОДК"))
                .Select(x => new
                {
                    objectCrId = x.ObjectCr.Id,
                    x.TypeWorkCr.Work.Code,
                    x.TypeWorkCr.Work.TypeWork,
                    x.Sum
                })
                .AsEnumerable()
                .GroupBy(x => x.objectCrId)
                .ToDictionary(
                    x => x.Key,
                    x => new
                    {
                        work1020 = x.Where(y => y.Code == "1020").Sum(y => y.Sum),
                        work1018 = x.Where(y => y.Code == "1018" || y.Code == "1019").Sum(y => y.Sum),
                        workSum = x.Where(y => y.TypeWork == TypeWork.Work).Sum(y => y.Sum),
                        sum = x.Sum(y => y.Sum),
                        count = x.Count()
                    });

            var actDataByBuilder = buildersByMu.ToDictionary(
                x => x.Key,
                x =>
                {
                    var actDataList = x.Value.objectCrIdList
                        .Where(workActByObjectCr.ContainsKey)
                        .Select(y => workActByObjectCr[y])
                        .ToList();

                    var anyAct = actDataList.Any();

                    return new
                    {
                        sum = anyAct ? actDataList.Sum(y => y.sum) : 0,
                        work1020 = anyAct ? actDataList.Sum(y => y.work1020) : 0,
                        work1018 = anyAct ? actDataList.Sum(y => y.work1018) : 0,
                        workSum = anyAct ? actDataList.Sum(y => y.workSum) : 0,
                        count = anyAct ? actDataList.Sum(y => y.count) : 0
                    };
                });

            var section = reportParams.ComplexReportParams.ДобавитьСекцию("section");
            reportParams.SimpleReportParams["reportDate"] = reportDate.ToShortDateString();

            foreach (var municipality in muDict)
            {
                var buildersByCurrentMu = buildersByMu.Where(x => x.Value.muId == municipality.Key).ToDictionary(x => x.Key, x => x.Value);
                if (buildersByCurrentMu.Count == 0)
                {
                    continue;
                }

                foreach (var builder in buildersByCurrentMu)
                {
                    section.ДобавитьСтроку();

                    section["municipality"] = municipality.Value;
                    section["builder"] = builder.Value.builderName;
                    section["roCount"] = builder.Value.roCount;

                    var workData = workDataByBuilder[builder.Key];

                    section["factProc"] = workData.percentFact / 100;
                    section["grProc"] = workData.percentGraphic / 100;
                    section["worksFact"] = workData.CostSum;
                    section["worksSum"] = workData.Sum;
                    section["worksProc"] = workData.Sum != 0 ? workData.CostSum / workData.Sum : 0;
                    section["houseLimit"] = workData.limit;

                    var actData = actDataByBuilder[builder.Key];

                    section["actsCount"] = actData.count;
                    section["actsSum"] = actData.sum;
                    section["actsSumSmr"] = actData.workSum;
                    section["actsSumPsd"] = actData.work1018;
                    section["actsSumTech"] = actData.work1020;
                }
            }

            reportParams.SimpleReportParams["roCountTotal"] = buildersByMu.Select(x => x.Value).Sum(x => x.roCount);
            reportParams.SimpleReportParams["factProcTotal"] = workDataByBuilder.Count != 0 ? workDataByBuilder.Select(x => x.Value).Sum(x => x.percentFact) / workDataByBuilder.Count / 100 : 0;
            reportParams.SimpleReportParams["grProcTotal"] = workDataByBuilder.Count != 0 ? workDataByBuilder.Select(x => x.Value).Sum(x => x.percentGraphic) / workDataByBuilder.Count / 100 : 0;

            var costSumTotal = workDataByBuilder.Select(x => x.Value).Sum(x => x.CostSum);
            var sumTotal = workDataByBuilder.Select(x => x.Value).Sum(x => x.Sum);

            reportParams.SimpleReportParams["worksFactTotal"] = costSumTotal;
            reportParams.SimpleReportParams["worksSumTotal"] = sumTotal;
            reportParams.SimpleReportParams["worksProcTotal"] = sumTotal != 0 ? costSumTotal / sumTotal : 0;
            reportParams.SimpleReportParams["houseLimitTotal"] = workDataByBuilder.Select(x => x.Value).Sum(x => x.limit);
            reportParams.SimpleReportParams["actsCountTotal"] = actDataByBuilder.Select(x => x.Value).Sum(x => x.count);
            reportParams.SimpleReportParams["actsSumTotal"] = actDataByBuilder.Select(x => x.Value).Sum(x => x.sum);
            reportParams.SimpleReportParams["actsSumSmrTotal"] = actDataByBuilder.Select(x => x.Value).Sum(x => x.workSum);
            reportParams.SimpleReportParams["actsSumPsdTotal"] = actDataByBuilder.Select(x => x.Value).Sum(x => x.work1018);
            reportParams.SimpleReportParams["actsSumTechTotal"] = actDataByBuilder.Select(x => x.Value).Sum(x => x.work1020);
        }
    }
}