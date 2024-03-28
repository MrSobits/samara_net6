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

    public class CompletionOfTheGraphicOnGivenDate : BasePrintForm
    {
        #region Входные параметры отчет
        private DateTime dataReport = DateTime.Now;
        private long[] municipalityIds;
        private long programCrId;
        private int timeShedule = 0;
        #endregion

        public CompletionOfTheGraphicOnGivenDate()
            : base(new ReportTemplateBinary(Properties.Resources.CompletionOfTheGraphicOnGivenDate))
        {
        }

        public IWindsorContainer Container { get; set; }

        public override string Name
        {
            get
            {
                return "Завершение работ по графику на заданную дату";
            }
        }

        public override string Desciption
        {
            get
            {
                return "Завершение работ по графику на заданную дату";
            }
        }

        public override string GroupName
        {
            get { return "Отчеты ГЖИ"; }
        }

        public override string ParamsController
        {
            get { return "B4.controller.report.CompletionOfTheGraphicOnGivenDate"; }
        }

        public override string RequiredPermission
        {
            get
            {
                return "Reports.CR.CompletionOfTheGraphicOnGivenDate";
            }
        }

        public override void SetUserParams(BaseParams baseParams)
        {
            // 0 = учитывать, 1 = не учитывать
            timeShedule = baseParams.Params["timeSchedule"].ToInt();
            dataReport = baseParams.Params["dateReport"].ToDateTime();
            var listIds = baseParams.Params["municipalityIds"].ToString();
            this.municipalityIds = !string.IsNullOrEmpty(listIds) ? listIds.Split(',').Select(x => x.ToLong()).ToArray() : new long[0];
            this.programCrId = baseParams.Params["programCr"].ToInt();
        }

        public override string ReportGenerator { get; set; }

        public override void PrepareReport(ReportParams reportParams)
        {
            var program = this.Container.Resolve<IDomainService<ProgramCr>>().GetAll()
                    .Where(x => x.Id == this.programCrId)
                    .Select(x => new { x.Id, x.Name, x.Period.DateStart.Year })
                    .FirstOrDefault();

            if (program == null) throw new Exception("Не найдена программа Капитального ремонта");

            reportParams.SimpleReportParams["programName"] = program.Name;
            reportParams.SimpleReportParams["reportDate"] = dataReport.ToShortDateString();

            var serviceObjectCr = this.Container.Resolve<IDomainService<ObjectCr>>();
            var serviceArchiveSmr = this.Container.Resolve<IDomainService<ArchiveSmr>>();
            var serviceTypeWork = this.Container.Resolve<IDomainService<TypeWorkCr>>();
            var serviceMunicipality = this.Container.Resolve<IDomainService<Municipality>>();

            var municipalityDict = serviceMunicipality.GetAll()
                .WhereIf(municipalityIds.Length > 0, x => municipalityIds.Contains(x.Id))
                .Select(x => new { x.Id, x.Name })
                .OrderBy(x => x.Name)
                .ToArray();

            var realityObjByMu = serviceObjectCr.GetAll()
                .WhereIf(municipalityIds.Length > 0, x => municipalityIds.Contains(x.RealityObject.Municipality.Id))
                .Where(x => x.ProgramCr.Id == this.programCrId)
                .Select(x => new { muId = x.RealityObject.Municipality.Id, roId = x.RealityObject.Id })
                .AsEnumerable()
                .GroupBy(x => x.muId)
                .ToDictionary(x => x.Key, x => x.Distinct(y => y.roId).Count());

            var workCodes = new List<string>();
            for (var i = 1; i <= 23; i++)
            {
                workCodes.Add(i.ToStr());
            }

            var works = serviceTypeWork.GetAll()
                   .WhereIf(this.municipalityIds.Length > 0, x => this.municipalityIds.Contains(x.ObjectCr.RealityObject.Municipality.Id))
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

            var workDataByCodeByRoByMuId = program.Year >= 2013
               ? serviceArchiveSmr.GetAll()
                .WhereIf(this.municipalityIds.Length > 0, x => this.municipalityIds.Contains(x.TypeWorkCr.ObjectCr.RealityObject.Municipality.Id))
                .Where(x => x.TypeWorkCr.ObjectCr.ProgramCr.Id == program.Id)
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
                                                            var archiveRec = z.OrderByDescending(p => p.DateChangeRec)
                                                                             .ThenByDescending(p => p.Id)
                                                                             .FirstOrDefault();

                                                            var tmpBool = (archiveRec != null) && (archiveRec.PercentOfCompletion == 100);

                                                            // 1 - Не учитывать, смотрим на те работы, у кот. процент выполнения=100 и дата завершения больше даты отчета, то есть они сделали работу с опережением
                                                            // 0 - Учитывать, считаем дома у кот. работы к моменту выгрузки отчета должны быть завершены, на процент не смотрим
                                                            var result = timeShedule == 1 ? tmpBool && (archiveRec.DateEndWork > this.dataReport) : archiveRec != null && archiveRec.DateEndWork <= this.dataReport;
                                                            return result;
                                                        }).ToList();

                                                var AnyWorkOfThisCodeIsFinished = worksOfThisCodeFinishedFlag.Any(z => z);
                                                var AllWorksOfThisCodeIsFinished = worksOfThisCodeFinishedFlag.Any() && worksOfThisCodeFinishedFlag.All(z => z);
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

            var realityObjCountByWorkByMo = workDataByCodeByRoByMuId.ToDictionary(
                x => x.Key,
                x => workCodes.ToDictionary(y => y, y => x.Value.Count(z => z.Value.ContainsKey(y) && z.Value[y].AnyWorkOfThisCodeIsFinished)));

            var rowTotals = realityObjCountByWorkByMo.ToDictionary(x => x.Key, x => x.Value.Sum(y => y.Value));

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

            var dictTotals = workCodes.ToDictionary(x => string.Format("CountHousesCode{0}Total", x), x => 0);
            dictTotals["CountHousesTotal"] = 0;
            dictTotals["SumHousesTotal"] = 0;
            dictTotals["CountHousesSMRTotal"] = 0;
            dictTotals["CountHousesCompletedWorkTotal"] = 0;

            var section = reportParams.ComplexReportParams.ДобавитьСекцию("sectionMu");
            foreach (var municipality in municipalityDict)
            {
                section.ДобавитьСтроку();

                section["MuName"] = municipality.Name;
                var countHouses = realityObjByMu.ContainsKey(municipality.Id) ? realityObjByMu[municipality.Id] : 0;
                if (countHouses > 0)
                {
                    section["CountHouses"] = countHouses;
                    dictTotals["CountHousesTotal"] += countHouses;
                }

                var sumHouses = rowTotals.ContainsKey(municipality.Id) ? rowTotals[municipality.Id] : 0;
                if (sumHouses > 0)
                {
                    section["SumHouses"] = sumHouses;
                    dictTotals["SumHousesTotal"] += sumHouses;
                }

                var countHousesSmr = rowTotal2.ContainsKey(municipality.Id) ? rowTotal2[municipality.Id].anyWorkFinished : 0;
                section["CountHousesSMR"] = countHousesSmr > 0 ? countHousesSmr.ToStr() : string.Empty;
                dictTotals["CountHousesSMRTotal"] += countHousesSmr;

                var countHousesCompletedWork = rowTotal2.ContainsKey(municipality.Id) ? rowTotal2[municipality.Id].allWorksFinished : 0;
                if (countHousesCompletedWork > 0)
                {
                    section["CountHousesCompletedWork"] = countHousesCompletedWork;
                    dictTotals["CountHousesCompletedWorkTotal"] += countHousesCompletedWork;
                }

                if (!realityObjCountByWorkByMo.ContainsKey(municipality.Id)) continue;

                var dict = realityObjCountByWorkByMo[municipality.Id];
                foreach (var workCode in workCodes.Where(dict.ContainsKey))
                {
                    var count = realityObjCountByWorkByMo[municipality.Id][workCode];
                    if (count <= 0) continue;
                    section[string.Format("CountHousesCode{0}", workCode)] = count;
                    dictTotals[string.Format("CountHousesCode{0}Total", workCode)] += count;
                }
            }

            foreach (var total in dictTotals.Where(x => x.Value > 0))
            {
                reportParams.SimpleReportParams[total.Key] = total.Value;
            }
        }
    }
}