namespace Bars.GkhCr.Report
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    
    using B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Entities.Dicts;
    using Bars.Gkh.Enums;
    using Bars.GkhCr.Entities;

    using Castle.Windsor;

    /// <summary>
    /// Отчет по домам, по которым работы начаты фактически
    /// </summary>
    public class ActuallyStartedWorksByHousesReport : BasePrintForm
    {
        // идентификатор программы КР
        private long programCrId;
        private long[] municipalityIds;
        private long[] finSourceIds;
        private DateTime reportDate = DateTime.MinValue;

        public ActuallyStartedWorksByHousesReport()
            : base(new ReportTemplateBinary(Properties.Resources.ActuallyStartedWorksByHouses))
        {
        }

        public IWindsorContainer Container { get; set; }

        public override string RequiredPermission
        {
            get { return "Reports.CR.ActuallyStartedWorksByHouses"; }
        }

        public override string Desciption
        {
            get { return "Отчет по домам, по которым работы начаты фактически"; }
        }

        public override string GroupName
        {
            get { return "Отчеты ГЖИ"; }
        }

        public override string ParamsController
        {
            get { return "B4.controller.report.ActuallyStartedWorksByHouses"; }
        }

        public override string Name
        {
            get { return "Отчет по домам, по которым работы начаты фактически"; }
        }

        public override void SetUserParams(BaseParams baseParams)
        {
            programCrId = baseParams.Params["programCrId"].ToLong();

            var dateReportPar = baseParams.Params["reportDate"].ToDateTime();

            var dateReport = dateReportPar == DateTime.Today.Date ? DateTime.Today.AddDays(1) : dateReportPar;

            reportDate = dateReport != DateTime.MinValue ? dateReport : DateTime.Now;

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
            var workCodes = new List<string>();

            for (var i = 1; i <= 23; i++)
            {
                workCodes.Add(i.ToStr());
            }

            var program = this.Container.Resolve<IDomainService<ProgramCr>>().Load(this.programCrId);
            var programYear = program != null ? program.Period.DateStart.Year : 0;

            reportParams.SimpleReportParams["programName"] = program != null ? program.Name : string.Empty;
            reportParams.SimpleReportParams["reportDate"] = this.reportDate.ToShortDateString();

            var serviceArchiveSmr = this.Container.Resolve<IDomainService<ArchiveSmr>>();
            var serviceTypeWork = this.Container.Resolve<IDomainService<TypeWorkCr>>();
            var serviceMunicipality = this.Container.Resolve<IDomainService<Municipality>>();

            var muDict = serviceMunicipality.GetAll()
                .WhereIf(this.municipalityIds.Length > 0, x => this.municipalityIds.Contains(x.Id))
                .Select(x => new { x.Id, x.Name })
                .ToDictionary(x => x.Id, x => x.Name);

            var works = serviceTypeWork.GetAll()
                   .WhereIf(this.municipalityIds.Length > 0, x => this.municipalityIds.Contains(x.ObjectCr.RealityObject.Municipality.Id))
                   .Where(x => x.ObjectCr.ProgramCr.Id == program.Id)
                   .WhereIf(this.finSourceIds.Length > 0, x => this.finSourceIds.Contains(x.FinanceSource.Id))
                   .Select(x => new
                   {
                       muId = x.ObjectCr.RealityObject.Municipality.Id,
                       roId = x.ObjectCr.RealityObject.Id,
                       workCode = x.Work.Code,
                       typeWork = x.Work.TypeWork,
                       x.DateEndWork,
                       x.PercentOfCompletion,
                       TypeWorkCrId = x.Id
                   })
                   .AsEnumerable()
                   .GroupBy(x => x.muId)
                    .ToDictionary(
                        x => x.Key,
                        x =>
                        x.GroupBy(y => y.roId)
                        .ToDictionary(
                            y => y.Key,
                            y =>
                            y.GroupBy(r => r.workCode)
                            .ToDictionary(
                                r => r.Key,
                                r => new StartedWorkProxy
                                {
                                    AnyWorkOfThisCodeIsStarted = r.Any(t => t.PercentOfCompletion > 0 && t.PercentOfCompletion < 100),
                                    AllWorksOfThisCodeIsStarted = r.All(t => t.PercentOfCompletion > 0 && t.PercentOfCompletion < 100),
                                    typeWkIds = r.GroupBy(t => t.TypeWorkCrId).Select(z => z.Key).ToList(),
                                    allWorkInArchive = true,
                                    TypeWork = r.Select(z => z.typeWork).First()
                                })));
            
            var workDataByCodeByRoByMuId = programYear >= 2013
               ? serviceArchiveSmr.GetAll()
                .WhereIf(this.municipalityIds.Length > 0, x => this.municipalityIds.Contains(x.TypeWorkCr.ObjectCr.RealityObject.Municipality.Id))
                .Where(x => x.TypeWorkCr.ObjectCr.ProgramCr.Id == program.Id)
                .WhereIf(this.finSourceIds.Length > 0, x => this.finSourceIds.Contains(x.TypeWorkCr.FinanceSource.Id))
                .Where(x => x.DateChangeRec <= this.reportDate)
                .Select(x => new
                {
                    muId = x.TypeWorkCr.ObjectCr.RealityObject.Municipality.Id,
                    roId = x.TypeWorkCr.ObjectCr.RealityObject.Id,
                    workCode = x.TypeWorkCr.Work.Code,
                    typeWork = x.TypeWorkCr.Work.TypeWork,
                    x.TypeWorkCr.DateEndWork,
                    x.PercentOfCompletion,
                    x.Id,
                    x.DateChangeRec,
                    TypeWorkCrId = x.TypeWorkCr.Id
                })
                .AsEnumerable()
                .GroupBy(x => x.muId)
                .ToDictionary(
                    x => x.Key,
                    x =>
                    {
                        var workDataByCodeByRo = works.ContainsKey(x.Key) ? works[x.Key] : null;

                        return x.GroupBy(y => y.roId)
                                .ToDictionary(
                                    y => y.Key, y =>
                                    {
                                        var workDataByCode = workDataByCodeByRo != null
                                                             && workDataByCodeByRo.ContainsKey(y.Key)
                                                                 ? workDataByCodeByRo[y.Key]
                                                                 : null;

                                        return y.GroupBy(r => r.workCode).ToDictionary(
                                            r => r.Key,
                                            r =>
                                            {
                                                var worksOfThisCodeStartedFlag =
                                                    r.GroupBy(z => z.TypeWorkCrId).Select(
                                                        z =>
                                                        {
                                                            var archiveRec =
                                                                z.OrderByDescending(
                                                                    p => p.DateChangeRec)
                                                                 .ThenByDescending(p => p.Id)
                                                                 .FirstOrDefault();

                                                            return archiveRec != null
                                                                   && (archiveRec.PercentOfCompletion > 0 && archiveRec.PercentOfCompletion < 100);
                                                        }).ToList();

                                                var anyWorkOfThisCodeIsStarted = worksOfThisCodeStartedFlag.Any(z => z);
                                                var allWorksOfThisCodeIsStarted = worksOfThisCodeStartedFlag.All(z => z);
                                                var workIds = r.GroupBy(z => z.TypeWorkCrId).Select(z => z.Key).ToList();

                                                var allWorkInArchive = workDataByCode != null
                                                                       && workDataByCode.ContainsKey(r.Key)
                                                                       && workDataByCode[r.Key].typeWkIds.All(workIds.Contains);

                                                return new StartedWorkProxy
                                                {
                                                    AnyWorkOfThisCodeIsStarted = anyWorkOfThisCodeIsStarted,
                                                    AllWorksOfThisCodeIsStarted = allWorksOfThisCodeIsStarted,
                                                    allWorkInArchive = allWorkInArchive,
                                                    TypeWork = r.Select(z => z.typeWork).First()
                                                };
                                            });
                                    });
                    })
                        :
                works;

            var roCountByWorkByMo = workDataByCodeByRoByMuId.ToDictionary(
                x => x.Key,
                x => workCodes.ToDictionary(y => y, y => x.Value.Count(z => z.Value.ContainsKey(y) && z.Value[y].AllWorksOfThisCodeIsStarted)));

            var rowTotal2 = works.ToDictionary(
                x => x.Key, x =>
                {
                    if (workDataByCodeByRoByMuId.ContainsKey(x.Key))
                    {
                        var workDataByCodeByRo = workDataByCodeByRoByMuId[x.Key];
                        return new
                        {
                            anyWorkStarted = x.Value.Count(y => workDataByCodeByRo.ContainsKey(y.Key) && workDataByCodeByRo[y.Key].Any(z => z.Value.TypeWork == TypeWork.Work && z.Value.AllWorksOfThisCodeIsStarted)),
                            allWorksStarted = x.Value.Count(y => workDataByCodeByRo.ContainsKey(y.Key) 
                                    && y.Value.Any()
                                    && y.Value
                                        .Where(r => r.Value.TypeWork == TypeWork.Work)
                                        .All(z => workDataByCodeByRo[y.Key].ContainsKey(z.Key)
                                                && workDataByCodeByRo[y.Key][z.Key].AllWorksOfThisCodeIsStarted 
                                                && workDataByCodeByRo[y.Key][z.Key].allWorkInArchive)),
                            allStarted = x.Value
                                            .Count(y => workDataByCodeByRo.ContainsKey(y.Key)
                                                && y.Value.Any()
                                                && y.Value.All(z => workDataByCodeByRo[y.Key].ContainsKey(z.Key)
                                                    && workDataByCodeByRo[y.Key][z.Key].AllWorksOfThisCodeIsStarted
                                                    && workDataByCodeByRo[y.Key][z.Key].allWorkInArchive))

                        };
                    }

                    return new { anyWorkStarted = 0, allWorksStarted = 0, allStarted = 0 };
                });

            var section = reportParams.ComplexReportParams.ДобавитьСекцию("sectionMu");

            var dictTotals = workCodes.ToDictionary(x => "CountHousesCode" + x, x => 0);
            dictTotals["CountHouses"] = 0;
            dictTotals["CountHousesSMR"] = 0;
            dictTotals["CountHousesStartedWork"] = 0;
            dictTotals["CountHousesStartedAll"] = 0;
            
            foreach (var municipality in muDict.OrderBy(x => x.Value))
            {
                var municipalityId = municipality.Key;
                section.ДобавитьСтроку();

                section["MuName"] = municipality.Value;
                var countHouses = workDataByCodeByRoByMuId.ContainsKey(municipalityId)
                                      ? workDataByCodeByRoByMuId[municipalityId].Keys.Count()
                                      : 0;
                section["CountHouses"] = countHouses > 0 ? countHouses.ToStr() : string.Empty;
                dictTotals["CountHouses"] += countHouses;
                
                var countHousesSmr = rowTotal2.ContainsKey(municipalityId) ? rowTotal2[municipalityId].anyWorkStarted : 0;
                section["CountHousesSMR"] = countHousesSmr > 0 ? countHousesSmr.ToStr() : string.Empty;
                dictTotals["CountHousesSMR"] += countHousesSmr;

                var countHousesCompletedWork = rowTotal2.ContainsKey(municipalityId) ? rowTotal2[municipalityId].allWorksStarted : 0;
                section["CountHousesStartedWork"] = countHousesCompletedWork > 0 ? countHousesCompletedWork.ToStr() : string.Empty;
                dictTotals["CountHousesStartedWork"] += countHousesCompletedWork;

                var countHousesCompletedAll = rowTotal2.ContainsKey(municipalityId) ? rowTotal2[municipalityId].allStarted : 0;
                section["CountHousesStartedAll"] = countHousesCompletedAll > 0 ? countHousesCompletedAll.ToStr() : string.Empty;
                dictTotals["CountHousesStartedAll"] += countHousesCompletedAll;

                if (roCountByWorkByMo.ContainsKey(municipalityId))
                {
                    var municipalityData = roCountByWorkByMo[municipalityId];
                    foreach (var workCode in workCodes)
                    {
                        var count = municipalityData.ContainsKey(workCode) ? municipalityData[workCode] : 0;
                        section["CountHousesCode" + workCode] = count > 0 ? count.ToStr() : string.Empty;
                        dictTotals["CountHousesCode" + workCode] += count;
                    }
                }
                else
                {
                    foreach (var workCode in workCodes)
                    {
                        section["CountHousesCode" + workCode] = string.Empty;
                    }
                }
            }

            foreach (var total in dictTotals)
            {
                reportParams.SimpleReportParams[total.Key] = total.Value > 0 ? total.Value.ToStr() : string.Empty;
            }   
        }
    }

    class StartedWorkProxy
    {
        public bool AnyWorkOfThisCodeIsStarted;

        public bool AllWorksOfThisCodeIsStarted;

        public List<long> typeWkIds;

        public bool allWorkInArchive;

        public TypeWork TypeWork;
    }
}