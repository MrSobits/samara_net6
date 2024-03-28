namespace Bars.GkhCr.Report
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

    public class HousesCompletedWorkFact : BasePrintForm
    {
        public IWindsorContainer Container { get; set; }

        #region Входные параметры отчет
        private DateTime dataReport = DateTime.Now;
        private List<long> listMunicipalityId = null;
        private long programCrId;
        #endregion


        public HousesCompletedWorkFact()
            : base(new ReportTemplateBinary(Properties.Resources.HousesCompletedWorkFact))
        {
        }

        public override string Name
        {
            get
            {
                return "Отчет по домам, по которым работы завершены фактически";
            }
        }

        public override string Desciption
        {
            get
            {
                return "Отчет по домам, по которым работы завершены фактически";
            }
        }

        public override string GroupName
        {
            get { return "Отчеты ГЖИ"; }
        }

        public override string ParamsController
        {
            get { return "B4.controller.report.HousesCompletedWorkFact"; }
        }

        public override string RequiredPermission
        {
            get
            {
                return "Reports.CR.HousesCompletedWorkFact";
            }
        }

        public override void SetUserParams(B4.BaseParams baseParams)
        {
            dataReport = baseParams.Params["dateReport"].ToDateTime();
            var listIds = baseParams.Params["municipalityIds"].ToString();
            this.listMunicipalityId = !string.IsNullOrEmpty(listIds) ? listIds.Split(',').Select(x => x.ToLong()).ToList() : new List<long>();
            this.programCrId = baseParams.Params["programCr"].ToLong();
        }

        public override string ReportGenerator { get; set; }

        public override void PrepareReport(ReportParams reportParams)
        {
            var workCodes = new List<string>();

            for (var i = 1; i <= 31; i++)
            {
                workCodes.Add(i.ToStr());
            }

            var program = this.Container.Resolve<IDomainService<ProgramCr>>().Load(this.programCrId);
            var programYear = program != null ? program.Period.DateStart.Year : 0;

            reportParams.SimpleReportParams["programName"] = program.Name;
            reportParams.SimpleReportParams["reportDate"] = dataReport.ToShortDateString();

            var serviceObjectCr = this.Container.Resolve<IDomainService<ObjectCr>>();
            var serviceArchiveSmr = this.Container.Resolve<IDomainService<ArchiveSmr>>();
            var serviceTypeWork = this.Container.Resolve<IDomainService<TypeWorkCr>>();
            var serviceMunicipality = this.Container.Resolve<IDomainService<Municipality>>();

            var muDict = serviceMunicipality.GetAll()
                .WhereIf(this.listMunicipalityId.Count > 0, x => this.listMunicipalityId.Contains(x.Id))
                .Select(x => new { x.Id, x.Name })
                .ToDictionary(x => x.Id, x => x.Name);

            var roByMu = serviceObjectCr.GetAll()
                .WhereIf(this.listMunicipalityId.Count > 0, x => this.listMunicipalityId.Contains(x.RealityObject.Municipality.Id))
                .Where(x => x.ProgramCr.Id == this.programCrId)
                .Select(x => new { muId = x.RealityObject.Municipality.Id, roId = x.RealityObject.Id })
                .AsEnumerable()
                .GroupBy(x => x.muId)
                .ToDictionary(x => x.Key, x => x.Distinct(y => y.roId).Count());

            var works = serviceTypeWork.GetAll()
                   .WhereIf(this.listMunicipalityId.Count > 0, x => this.listMunicipalityId.Contains(x.ObjectCr.RealityObject.Municipality.Id))
                   .Where(x => x.ObjectCr.ProgramCr.Id == program.Id)
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
                                r => new WorkProxy
                                {
                                    AnyWorkOfThisCodeIsFinished = r.Any(t => t.PercentOfCompletion == 100),
                                    AllWorksOfThisCodeIsFinished = r.All(t => t.PercentOfCompletion == 100),
                                    typeWkIds = r.GroupBy(t => t.TypeWorkCrId).Select(z => z.Key).ToList(),
                                    allWorkInArchive = true,
                                    TypeWork = r.Select(z => z.typeWork).First()
                                })));

            var workDataByCodeByRoByMuId = programYear >= 2013
               ? serviceArchiveSmr.GetAll()
                .WhereIf(this.listMunicipalityId.Count > 0, x => this.listMunicipalityId.Contains(x.TypeWorkCr.ObjectCr.RealityObject.Municipality.Id))
                .Where(x => x.TypeWorkCr.ObjectCr.ProgramCr.Id == program.Id)
                .Where(x => x.DateChangeRec <= this.dataReport)
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
                                                            var worksOfThisCodeFinishedFlag =
                                                                r.GroupBy(z => z.TypeWorkCrId).Select(
                                                                    z =>
                                                                        {
                                                                            var archiveRec =
                                                                                z.OrderByDescending(
                                                                                    p => p.DateChangeRec)
                                                                                 .ThenByDescending(p => p.Id)
                                                                                 .FirstOrDefault();

                                                                            return archiveRec != null
                                                                                   && archiveRec.PercentOfCompletion
                                                                                   == 100;
                                                                        }).ToList();

                                                            var AnyWorkOfThisCodeIsFinished = worksOfThisCodeFinishedFlag.Any(z => z);
                                                            var AllWorksOfThisCodeIsFinished = worksOfThisCodeFinishedFlag.All(z => z);
                                                            var workIds = r.GroupBy(z => z.TypeWorkCrId).Select(z => z.Key).ToList();

                                                            var allWorkInArchive = workDataByCode != null 
                                                                                   && workDataByCode.ContainsKey(r.Key)
                                                                                   && workDataByCode[r.Key].typeWkIds.All(workIds.Contains);


                                                            return new WorkProxy
                                                                       {
                                                                           AnyWorkOfThisCodeIsFinished = AnyWorkOfThisCodeIsFinished,
                                                                           AllWorksOfThisCodeIsFinished = AllWorksOfThisCodeIsFinished,
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
                x => workCodes.ToDictionary(y => y, y => x.Value.Count(z => z.Value.ContainsKey(y) && z.Value[y].AnyWorkOfThisCodeIsFinished)));

            var rowTotals = roCountByWorkByMo.ToDictionary(x => x.Key, x => x.Value.Sum(y => y.Value));

            var rowTotal2 = works.ToDictionary(
                x => x.Key, x =>
                    {
                        if (workDataByCodeByRoByMuId.ContainsKey(x.Key))
                        {
                            var workDataByCodeByRo = workDataByCodeByRoByMuId[x.Key];
                            return new
                            {
                                anyWorkFinished = x.Value.Count(y => workDataByCodeByRo.ContainsKey(y.Key) && workDataByCodeByRo[y.Key].Any(z => z.Value.TypeWork == TypeWork.Work && z.Value.AnyWorkOfThisCodeIsFinished)),
                                allWorksFinished = x.Value.Count(y => workDataByCodeByRo.ContainsKey(y.Key) 
                                    && y.Value.Any()
                                    && y.Value
                                        .Where(r => r.Value.TypeWork == TypeWork.Work)
                                        .All(z => workDataByCodeByRo[y.Key].ContainsKey(z.Key) 
                                                && workDataByCodeByRo[y.Key][z.Key].AllWorksOfThisCodeIsFinished 
                                                && workDataByCodeByRo[y.Key][z.Key].allWorkInArchive)),
                                allFinished = x.Value
                                    .Count(y => workDataByCodeByRo.ContainsKey(y.Key) 
                                        && y.Value.Any() 
                                        && y.Value.All(z => workDataByCodeByRo[y.Key].ContainsKey(z.Key) 
                                            && workDataByCodeByRo[y.Key][z.Key].AllWorksOfThisCodeIsFinished 
                                            && workDataByCodeByRo[y.Key][z.Key].allWorkInArchive))
                            };
                        }
                        
                        return new { anyWorkFinished = 0, allWorksFinished = 0, allFinished = 0 };
                    });

            var section = reportParams.ComplexReportParams.ДобавитьСекцию("sectionMu");

            var dictTotals = workCodes.ToDictionary(x => "CountHousesCode" + x, x => 0);
            dictTotals["CountHouses"] = 0;
            dictTotals["SumHouses"] = 0;
            dictTotals["CountHousesSMR"] = 0;
            dictTotals["CountHousesCompletedWork"] = 0;
            dictTotals["CountHousesCompletedAll"] = 0;

            foreach (var municipality in muDict.OrderBy(x => x.Value))
            {
                var municipalityId = municipality.Key;
                section.ДобавитьСтроку();

                section["MuName"] = municipality.Value;
                var countHouses = roByMu.ContainsKey(municipalityId) ? roByMu[municipalityId] : 0;
                section["CountHouses"] = countHouses > 0 ? countHouses.ToStr() : string.Empty; 
                dictTotals["CountHouses"] += countHouses; 

                var sumHouses = rowTotals.ContainsKey(municipalityId) ? rowTotals[municipalityId] : 0;
                section["SumHouses"] = sumHouses > 0 ? sumHouses.ToStr() : string.Empty; 
                dictTotals["SumHouses"] += sumHouses;

                var countHousesSmr = rowTotal2.ContainsKey(municipalityId) ? rowTotal2[municipalityId].anyWorkFinished : 0;
                section["CountHousesSMR"] = countHousesSmr > 0 ? countHousesSmr.ToStr() : string.Empty; 
                dictTotals["CountHousesSMR"] += countHousesSmr;

                var countHousesCompletedWork = rowTotal2.ContainsKey(municipalityId) ? rowTotal2[municipalityId].allWorksFinished : 0;
                section["CountHousesCompletedWork"] = countHousesCompletedWork > 0 ? countHousesCompletedWork.ToStr() : string.Empty; 
                dictTotals["CountHousesCompletedWork"] += countHousesCompletedWork;

                var countHousesCompletedAll = rowTotal2.ContainsKey(municipalityId) ? rowTotal2[municipalityId].allFinished : 0;
                section["CountHousesCompletedAll"] = countHousesCompletedAll > 0 ? countHousesCompletedAll.ToStr() : string.Empty; 
                dictTotals["CountHousesCompletedAll"] += countHousesCompletedAll;

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
                reportParams.SimpleReportParams[total.Key] = total.Value > 0 ? total.Value.ToStr() : string.Empty ;
            }
        }
    }

    class WorkProxy
    {
        public bool AnyWorkOfThisCodeIsFinished;

        public bool AllWorksOfThisCodeIsFinished;

        public List<long> typeWkIds;

        public bool allWorkInArchive;

        public TypeWork TypeWork;
    }
}