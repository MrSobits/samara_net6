using System.Linq;
using Bars.Gkh.Entities;
using Bars.Gkh.Entities.Dicts;
using Bars.Gkh.Enums;

namespace Bars.GkhCr.Report
{
    using System;
    using System.Collections.Generic;

    using Bars.GkhCr.Entities;
    
    using Bars.B4;
    using Bars.B4.Utils;
    using B4.Modules.Reports;
    

    using Castle.Windsor;

    /// <summary>
    /// Отставание выполнения по заданному виду работ
    /// </summary>
    public class LaggingPerformanceOfWork : BasePrintForm
    {
        public IWindsorContainer Container { get; set; }

        #region Параметры

        // Программа КР
        private long programCrId;

        // Множественный выбор МО
        private readonly List<long> municipalityIds = new List<long>();
        
        // Множественный выбор источников финансирования
        private readonly List<long> finSourceIds = new List<long>();

        // Множественный выбор работ из справочника Виды работ
        private readonly List<long> workTypeIds = new List<long>();
        
        // Дата отчета	Выбор даты
        private DateTime reportDate = DateTime.MinValue;

        #endregion
        
        public LaggingPerformanceOfWork()
            : base(new ReportTemplateBinary(Properties.Resources.LaggingPerformanceOfWork))
        {
            
        }
        
        public override string Name
        {
            get
            {
                return "Отставание выполнения по заданному виду работ";
            }
        }

        public override string Desciption
        {
            get 
            {
                return "Отставание выполнения по заданному виду работ";
            }
        }

        public override string GroupName
        {
            get
            {
                return "Отчеты ГЖИ";
            }
        }

        public override string ParamsController
        {
            get
            {
                return "B4.controller.report.LaggingPerformanceOfWork";
            }
        }

        public override string RequiredPermission
        {
            get
            {
                return "Reports.CR.LaggingPerformanceOfWork";
            }
        }

        public override void SetUserParams(BaseParams baseParams)
        {
            this.programCrId = baseParams.Params["programCrId"].ToLong();
            ParseIds(this.municipalityIds, baseParams.Params["municipalityIds"].ToString());
            ParseIds(this.finSourceIds, baseParams.Params["finSourceIds"].ToString());
            ParseIds(this.workTypeIds, baseParams.Params["workTypeIds"].ToString());
            this.reportDate = baseParams.Params["reportDate"].ToDateTime();
        }

        public override string ReportGenerator { get; set; }

        private static void ParseIds(List<long> list, string Ids)
        {
            list.Clear();

            if (!string.IsNullOrEmpty(Ids))
            {
                var ids = Ids.Split(',');
                foreach (var id in ids)
                {
                    long Id;
                    if (
                    long.TryParse(id, out Id))
                    {
                        if (!list.Contains(Id))
                        {
                            list.Add(Id);
                        }
                    }
                }
            }
        }

        public override void PrepareReport(ReportParams reportParams)
        {
            var programCr = this.Container.Resolve<IDomainService<ProgramCr>>().Get(this.programCrId);

            var programCrName = string.Empty;
            var programCrYear = this.reportDate.Date.Year;
            if (programCr != null)
            {
                programCrName = programCr.Name;
                programCrYear = programCr.Period.DateEnd.HasValue
                                    ? programCr.Period.DateEnd.Value.Year
                                    : this.reportDate.Date.Year;
            }

            var works = Container.Resolve<IDomainService<Work>>()
                         .GetAll()
                         .Where(x => x.TypeWork == TypeWork.Work)
                         .Where(x => workTypeIds.Contains(x.Id))
                         .Select(x => new { x.Id, x.Name, x.Code })
                         .ToList();

            var workTypesStr = string.Empty;
            foreach (var work in works)
            {
                if (string.IsNullOrEmpty(workTypesStr))
                {
                    workTypesStr += work.Name;
                }
                else
                {
                    workTypesStr += string.Format(", {0}", work.Name);
                }
            }
            reportParams.SimpleReportParams["виды работ"] = string.Empty;
            reportParams.SimpleReportParams["виды работ"] += workTypesStr;
            reportParams.SimpleReportParams["программаКР"] = string.Format("\"{0}\"", programCrName);
            
            var typeWorks = Container.Resolve<IDomainService<TypeWorkCr>>()
                            .GetAll()
                            .Where(x => x.ObjectCr.ProgramCr.Id == programCrId)
                            .WhereIf(municipalityIds.Count > 0, x => municipalityIds.Contains(x.ObjectCr.RealityObject.Municipality.Id))
                            .WhereIf(finSourceIds.Count > 0, x => finSourceIds.Contains(x.FinanceSource.Id))
                            .Where(x => workTypeIds.Contains(x.Work.Id))
                            .Select(x => new
                                    {
                                        muId = x.ObjectCr.RealityObject.Municipality.Id,
                                        realObjId = x.ObjectCr.RealityObject.Id,
                                        typeWorkId = x.Id,
                                        percentOfCompletion = x.PercentOfCompletion,
                                        dateStart = x.DateStartWork,
                                        dateEnd = x.DateEndWork
                                    })
                            .AsEnumerable()
                            .GroupBy(x => x.muId)
                            .ToDictionary(x => x.Key,
                                          x => x.GroupBy(y => y.realObjId)
                                                .ToDictionary(y => y.Key, y => y.ToList()));

            var archiveRecords = Container.Resolve<IDomainService<ArchiveSmr>>()
                            .GetAll()
                            .Where(x => x.TypeWorkCr.ObjectCr.ProgramCr.Id == programCrId)
                            .WhereIf(municipalityIds.Count > 0, x => municipalityIds.Contains(x.TypeWorkCr.ObjectCr.RealityObject.Municipality.Id))
                            .WhereIf(finSourceIds.Count > 0, x => finSourceIds.Contains(x.TypeWorkCr.FinanceSource.Id))
                            .Where(x => workTypeIds.Contains(x.TypeWorkCr.Work.Id))
                            .Where(x => x.ObjectCreateDate <= reportDate)
                            .Select(x => new
                                    {
                                        x.Id,
                                        x.DateChangeRec,
                                        typeWorkId = x.TypeWorkCr.Id,
                                        percentOfCompletion = x.PercentOfCompletion
                                    })
                            .AsEnumerable()
                            .GroupBy(x => x.typeWorkId)
                            .ToDictionary(x => x.Key, x =>
                                                        {
                                                            var archiveRec = x.OrderByDescending(p => p.DateChangeRec)
                                                                              .ThenByDescending(p => p.Id)
                                                                              .FirstOrDefault();
                                                            return (archiveRec != null)
                                                                       ? archiveRec.percentOfCompletion
                                                                       : 0;
                                                        });
            
            var laggByMuIdByRealtyId = typeWorks.ToDictionary(x => x.Key,
                    x =>
                    x.Value.ToDictionary(y => y.Key,
                        y =>
                        {
                            bool over20 = false, over40 = false, over60 = false, anyLagg = false, workFinished = true;

                            foreach (var typeWork in y.Value)
                            {
                                var dateStart = typeWork.dateStart.ToDateTime();
                                var dateEnd = typeWork.dateEnd.ToDateTime();

                                var percentGraphic = dateStart != DateTime.MinValue
                                                    && dateEnd != DateTime.MinValue
                                                    && dateStart != dateEnd
                                                    && dateStart.Date < reportDate.Date
                                                            ? ((reportDate.Date - dateStart.Date).TotalDays / ((dateEnd.Date - dateStart.Date).TotalDays + 1)).ToDecimal()
                                                            : 0m;

                                percentGraphic = percentGraphic > 1 ? 1 : percentGraphic;

                                var archRec = programCrYear >= 2013 ? (archiveRecords.ContainsKey(typeWork.typeWorkId) ? archiveRecords[typeWork.typeWorkId]: null)
                                    : typeWork.percentOfCompletion;

                                var percentFact = archRec.HasValue
                                    ? archRec.Value / 100
                                    : 0;

                                percentFact = percentFact > 1 ? 1 : percentFact;

                                var percentLagg = percentFact == 1 || percentFact > percentGraphic
                                                        ? 0
                                                        : ((percentGraphic - percentFact) * 100).RoundDecimal(2);

                                over20 |= percentLagg <= 40 && percentLagg > 20;
                                over40 |= percentLagg <= 60 && percentLagg > 40;
                                over60 |= percentLagg > 60;
                                anyLagg |= percentLagg > 0;
                                workFinished &= percentFact == 1;
                            }
                            return new { over20, over40, over60, anyLagg, workFinished };
                        })
                );
            
            var resultDict = laggByMuIdByRealtyId.ToDictionary(x => x.Key, x => new
                {
                    over20Count = x.Value.Count(y => y.Value.over20),
                    over40Count = x.Value.Count(y => y.Value.over40),
                    over60Count = x.Value.Count(y => y.Value.over60),
                    anyLaggCount = x.Value.Count(y => y.Value.anyLagg),
                    finishedCount = x.Value.Count(y => y.Value.workFinished),
                    houseCount = x.Value.Count
                });

            var municipality = this.Container.Resolve<IDomainService<Municipality>>().GetAll()
                                   .WhereIf(this.municipalityIds.Count > 0, x => this.municipalityIds.Contains(x.Id))
                                   .Select(x => new
                                       {
                                           muId = x.Id,
                                           muName = x.Name
                                       })
                                   .ToDictionary(x => x.muId, x => x.muName);

            var section = reportParams.ComplexReportParams.ДобавитьСекцию("Список");

            var counter = 0;
            foreach (var resultRec in resultDict.OrderBy(x => municipality[x.Key]))
            {
                ++counter;
                section.ДобавитьСтроку();
                section["номер"] = counter.ToStr();
                section["МО"] = municipality[resultRec.Key];
                section["КоличДомовМО"] = resultRec.Value.houseCount;
                section["Завершено"] = resultRec.Value.finishedCount;
                section["любоеОтставание"] = resultRec.Value.anyLaggCount;
                section["отставание20"] = resultRec.Value.over20Count;
                section["отставание40"] = resultRec.Value.over40Count;
                section["отставание60"] = resultRec.Value.over60Count;
            }

            reportParams.SimpleReportParams["суммаДомов"] = resultDict.Values.Sum(x => x.houseCount);
            reportParams.SimpleReportParams["суммаЗавершено"] = resultDict.Values.Sum(x => x.finishedCount);
            reportParams.SimpleReportParams["суммаЛюбоеОтст"] = resultDict.Values.Sum(x => x.anyLaggCount);
            reportParams.SimpleReportParams["сумОтставание20"] = resultDict.Values.Sum(x => x.over20Count);
            reportParams.SimpleReportParams["сумОтставание40"] = resultDict.Values.Sum(x => x.over40Count);
            reportParams.SimpleReportParams["сумОтставание60"] = resultDict.Values.Sum(x => x.over60Count);
        }
    }
}
