namespace Bars.GkhCr.Report
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using Bars.B4;
    
    using Bars.B4.Modules.States;
    using B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.GkhCr.Entities;

    using Castle.Windsor;

    /// <summary>
    /// Отчет по домам, по которым присутствует отставание по графику выполнения работ
    /// </summary>
    public class CountRealObjByBacklogWork : BasePrintForm
    {
        #region Параметры
        public IWindsorContainer Container { get; set; }
        private List<long> municipalityIds = new List<long>();
        private List<long> financeSourceIds = new List<long>();
        private long programCrId;
        private DateTime reportDate;

        public CountRealObjByBacklogWork()
            : base(new ReportTemplateBinary(Properties.Resources.CountRealObjByBacklogWork))
        {
        }

        public override string Name
        {
            get { return "Отчет по домам, по которым присутствует отставание по графику выполнения работ"; }
        }

        public override string Desciption
        {
            get { return "Отчет по домам, по которым присутствует отставание по графику выполнения работ"; }
        }

        public override string GroupName
        {
            get { return "Отчеты ГЖИ"; }
        }

        public override string ParamsController
        {
            get { return "B4.controller.report.CountRealObjByBacklogWork"; }
        }

        public override string RequiredPermission
        {
            get
            {
                return "Reports.CR.CountRealObjByBacklogWork";
            }
        }

        private List<long> ParseIds(string ids)
        {
            if (!string.IsNullOrEmpty(ids))
            {
                return ids.Split(',').Select(x => x.ToLong()).ToList();
            }

            return new List<long>();
        }
        #endregion

        public override void SetUserParams(BaseParams baseParams)
        {
            programCrId = baseParams.Params["programCrId"].ToLong();

            // добавляем +1 день к дате отчета (учитываем по дату отчета включительно)
            reportDate = baseParams.Params["reportDate"].ToDateTime().AddDays(1);  

            municipalityIds = baseParams.Params.ContainsKey("municipalityIds")
                                  ? ParseIds(baseParams.Params["municipalityIds"].ToStr())
                                  : new List<long>();

            financeSourceIds = baseParams.Params.ContainsKey("finSourceIds")
                                 ? ParseIds(baseParams.Params["finSourceIds"].ToStr())
                                 : new List<long>();
        }

        public override string ReportGenerator { get; set; }

        public override void PrepareReport(ReportParams reportParams)
        {
            var programCr = this.Container.Resolve<IDomainService<ProgramCr>>().Get(this.programCrId);
            
            var programDate = reportDate.Date.Year;
            if (programCr != null)
            {
                programDate = programCr.Period.DateEnd.HasValue
                                    ? programCr.Period.DateEnd.Value.Year
                                    : this.reportDate.Date.Year;
            }

            var typeWorks = Container.Resolve<IDomainService<TypeWorkCr>>().GetAll()
                .Where(x => x.ObjectCr.ProgramCr.Id == programCrId && x.Work.TypeWork == TypeWork.Work)
                .WhereIf(municipalityIds.Count > 0, x => municipalityIds.Contains(x.ObjectCr.RealityObject.Municipality.Id))
                .WhereIf(financeSourceIds.Count > 0, x => financeSourceIds.Contains(x.FinanceSource.Id))
                .Select(x => new
                {
                    muId = x.ObjectCr.RealityObject.Municipality.Id,
                    muName = x.ObjectCr.RealityObject.Municipality.Name,
                    realObjId = x.ObjectCr.RealityObject.Id,
                    typeWorkId = x.Id,
                    workCode = x.Work.Code,
                    percentOfCompletion = x.PercentOfCompletion,
                    dateStart = x.DateStartWork ?? DateTime.MinValue,
                    dateEnd = x.DateEndWork ?? DateTime.MinValue
                })
                .AsEnumerable()
                .OrderBy(x => x.muName)
                .GroupBy(x => x.muId)
                .ToDictionary(
                    x => x.Key,
                    x => x.GroupBy(y => y.workCode)
                          .ToDictionary(y => y.Key, y => y.ToList()));

            var archiveRecords = Container.Resolve<IDomainService<ArchiveSmr>>().GetAll()
                .Where(x => x.TypeWorkCr.ObjectCr.ProgramCr.Id == programCrId && x.TypeWorkCr.Work.TypeWork == TypeWork.Work)
                .WhereIf(municipalityIds.Count > 0, x => municipalityIds.Contains(x.TypeWorkCr.ObjectCr.RealityObject.Municipality.Id))
                .WhereIf(financeSourceIds.Count > 0, x => financeSourceIds.Contains(x.TypeWorkCr.FinanceSource.Id))
                .Where(x => x.ObjectCreateDate <= reportDate)
                .Select(x => new
                {
                    x.Id,
                    x.DateChangeRec,
                    typeWorkId = x.TypeWorkCr.Id,
                    workCode = x.TypeWorkCr.Work.Code,
                    percentOfCompletion = x.PercentOfCompletion
                })
                .AsEnumerable()
                .GroupBy(x => x.typeWorkId)
                .ToDictionary(
                    x => x.Key, 
                    x =>
                    {
                        var archiveRec = x.OrderByDescending(p => p.DateChangeRec)
                                            .ThenByDescending(p => p.Id)
                                            .FirstOrDefault();
                        return (archiveRec != null)
                                    ? archiveRec.percentOfCompletion
                                    : 0;
                    });

            var laggByMuIdByRealtyId = typeWorks
                .ToDictionary(
                    x => x.Key,
                    x => x.Value.ToDictionary(
                        y => y.Key,
                        y =>
                        {
                            var robjWithLagList = new List<long>();
                            var robjAnyWorkNoLagList = new List<long>();
                            var robjList = new List<long>();

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

                                var archRec = programDate >= 2013 ? (archiveRecords.ContainsKey(typeWork.typeWorkId) ? archiveRecords[typeWork.typeWorkId] : null)
                                    : typeWork.percentOfCompletion;

                                var percentFact = archRec.HasValue
                                    ? archRec.Value / 100
                                    : 0;

                                percentFact = percentFact > 1 ? 1 : percentFact;

                                var percentLagg = percentFact == 1 || percentFact > percentGraphic
                                                        ? 0
                                                        : ((percentGraphic - percentFact) * 100).RoundDecimal(2);

                                if (percentLagg > 0.01.ToDecimal())
                                {
                                    // дома с отставанием по работам
                                    robjWithLagList.Add(typeWork.realObjId);
                                }
                                else
                                {
                                    // столбец Кол-во домов, у ктр есть хотя бы одна работа без оставания
                                    robjAnyWorkNoLagList.Add(typeWork.realObjId);
                                }
                                
                                // все дома по программе КР
                                robjList.Add(typeWork.realObjId);
                            }

                            return new { robjWithLagList, robjList, robjAnyWorkNoLagList };
                        }));

            // столбец Кол-во домов
            var resultAllHouses = laggByMuIdByRealtyId.ToDictionary(
                x => x.Key, x => x.Value.SelectMany(y => y.Value.robjList).Distinct().Count());

            // столбцы 1 - 23
            var robjectsCountByWorkCodeByMuDict = laggByMuIdByRealtyId.ToDictionary(
                x => x.Key, x => x.Value.ToDictionary(y => y.Key, y => y.Value.robjWithLagList.Distinct().Count()));
            
            // столбец Итого Домов
            var resultHousesDict = laggByMuIdByRealtyId.ToDictionary(
                x => x.Key, x => x.Value.SelectMany(y => y.Value.robjWithLagList).Distinct().Count());

            // Кол-во домов с кодом вида работы 1-23
            var resulttmp = laggByMuIdByRealtyId.ToDictionary(
                x => x.Key, x => x.Value.SelectMany(y => y.Value.robjList).Distinct().Count());

            // столбец Кол-во домов, у ктр есть хотя бы одна работа без оставания
            var resultHouses = laggByMuIdByRealtyId.ToDictionary(
                x => x.Key, x => x.Value.SelectMany(y => y.Value.robjAnyWorkNoLagList).Distinct().Count());
            
            var municipalityDict = this.Container.Resolve<IDomainService<Municipality>>().GetAll()
                .WhereIf(this.municipalityIds.Count > 0, x => this.municipalityIds.Contains(x.Id))
                .Select(x => new
                {
                    muId = x.Id,
                    muName = x.Name
                })
                .ToDictionary(x => x.muId, x => x.muName);

            var section = reportParams.ComplexReportParams.ДобавитьСекцию("Секция");
            reportParams.SimpleReportParams["ДатаОтчета"] = reportDate.AddDays(-1).ToShortDateString();
            reportParams.SimpleReportParams["ПрограммаКР"] = programCr.Name;
            var totalWorkCount = new int[23];
            var total = new int[3];
            var countHouse = 0;

            foreach (var municipality in laggByMuIdByRealtyId)
            {
                section.ДобавитьСтроку();
                section["Municipality"] = municipalityDict[municipality.Key];
                section["countHouse"] = resultAllHouses[municipality.Key];
                countHouse += resultAllHouses[municipality.Key];

                var uniqueRobjectsByWorkCode = robjectsCountByWorkCodeByMuDict[municipality.Key];

                // столбец Итого работ
                var muTotalWorkCount = 0;

                for (var i = 0; i < 23; ++i)
                {
                    var workCode = (i + 1).ToStr();
                    if (uniqueRobjectsByWorkCode.ContainsKey(workCode))
                    {
                        var uniqueRobjectsCount = uniqueRobjectsByWorkCode[workCode];
                        section["type" + (i + 1)] = uniqueRobjectsCount > 0 ? uniqueRobjectsCount.ToStr() : string.Empty;
                        totalWorkCount[i] += uniqueRobjectsCount;
                        muTotalWorkCount += uniqueRobjectsCount;  
                    }
                }

                section["Total1"] = muTotalWorkCount > 0 ? muTotalWorkCount.ToStr() : string.Empty;
                total[0] += muTotalWorkCount;
                section["Total2"] = resultHousesDict[municipality.Key] > 0 ? resultHousesDict[municipality.Key].ToStr() : string.Empty;
                total[1] += resultHousesDict[municipality.Key];
                section["Total3"] = resulttmp[municipality.Key] - resultHouses[municipality.Key] > 0 ? (resulttmp[municipality.Key] - resultHouses[municipality.Key]).ToStr() : string.Empty;
                total[2] += resulttmp[municipality.Key] - resultHouses[municipality.Key];
            }

            reportParams.SimpleReportParams["TotalCountHouse"] = countHouse;
            for (var i = 0; i < 3; ++i)
            {
                reportParams.SimpleReportParams["SubTotal" + (i + 1)] = total[i] > 0 ? total[i].ToStr() : string.Empty;
            }
            
            for (var i = 0; i < 23; ++i)
            {
                reportParams.SimpleReportParams["TotalType" + (i + 1)] = totalWorkCount[i] > 0 ? totalWorkCount[i].ToStr() : string.Empty;
            }
        }
    }
}