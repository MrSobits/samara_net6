namespace Bars.GkhGji.DomainService
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.DomainService.BaseParams;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Entities.BoilerRooms;

    using Castle.Windsor;

    public class HeatInputService : IHeatInputService
    {
        /// <summary>
        /// Windsor-контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }

        private readonly IDomainService<HeatingPeriod> heatingPeriodDomain;
        private readonly IDomainService<UnactivePeriod> unacktivePeriodDomain;
        private readonly IDomainService<HeatInputPeriod> heatInputPeriodDomain;
        private readonly IDomainService<HeatInputInformation> heatInputInfoDomain;

        /// <summary>
        /// Подготовка к работе в зимних условиях
        /// </summary>
        private readonly IDomainService<WorkWinterCondition> workWinterCondDomain;
        private readonly IDomainService<BoilerRoom> boilerRoomDomain;

        public HeatInputService(
            IDomainService<HeatingPeriod> heatingPeriodDomain,
            IDomainService<UnactivePeriod> unacktivePeriodDomain,
            IDomainService<HeatInputPeriod> heatInputPeriodDomain,
            IDomainService<HeatInputInformation> heatInputInfoDomain,
            IDomainService<WorkWinterCondition> workWinterCondDomain,
            IDomainService<BoilerRoom> boilerRoomDomain)
        {
            this.heatingPeriodDomain = heatingPeriodDomain;
            this.unacktivePeriodDomain = unacktivePeriodDomain;
            this.heatInputPeriodDomain = heatInputPeriodDomain;
            this.heatInputInfoDomain = heatInputInfoDomain;
            this.workWinterCondDomain = workWinterCondDomain;
            this.boilerRoomDomain = boilerRoomDomain;
        }

        public IDataResult SaveHeatInputInfo(BaseParams baseParams)
        {
            try
            {
                var modifiedRecords = baseParams.Params.GetAs<HeatInputInformation[]>("records");
                var modifiedRecordsIds = modifiedRecords.Select(x => x.Id).ToArray();

                var heatInputInfoRecs = this.heatInputInfoDomain.GetAll()
                    .Where(x => modifiedRecordsIds.Contains(x.Id))
                    .ToList();

                foreach (var heatInputInfoRec in heatInputInfoRecs)
                {
                    var modifiedRecord = modifiedRecords.FirstOrDefault(x => x.Id == heatInputInfoRec.Id);
                    if (modifiedRecord != null)
                    {
                        heatInputInfoRec.Count = modifiedRecord.Count;
                        heatInputInfoRec.CentralHeating = modifiedRecord.CentralHeating;
                        heatInputInfoRec.IndividualHeating = modifiedRecord.IndividualHeating;
                        heatInputInfoRec.Percent = modifiedRecord.Count != 0
                            ? decimal.Round(((heatInputInfoRec.CentralHeating + heatInputInfoRec.IndividualHeating) / (decimal)heatInputInfoRec.Count) * 100, 5)
                            : 0;
                        heatInputInfoRec.NoHeating = heatInputInfoRec.Count
                                                     - heatInputInfoRec.CentralHeating
                                                     - heatInputInfoRec.IndividualHeating;

                        this.heatInputInfoDomain.Update(heatInputInfoRec);
                    }
                }

                return new BaseDataResult();
            }
            catch (Exception e)
            {
                return new BaseDataResult { Success = false, Message = e.Message };
            }
        }

        public IDataResult SaveWorkWinterInfo(BaseParams baseParams)
        {
            try
            {
                var modifiedRecords = baseParams.Params.GetAs<WorkWinterCondition[]>("records");
                var modifiedRecordsIds = modifiedRecords.Select(x => x.Id).ToArray();

                var workWinterCondRecs = this.workWinterCondDomain.GetAll()
                    .Where(x => modifiedRecordsIds.Contains(x.Id))
                    .ToList();

                foreach (var workWinterCondRec in workWinterCondRecs)
                {
                    var modifiedRecord = modifiedRecords.FirstOrDefault(x => x.Id == workWinterCondRec.Id);
                    if (modifiedRecord != null)
                    {
                        workWinterCondRec.Total = modifiedRecord.Total;
                        workWinterCondRec.PreparationTask = modifiedRecord.PreparationTask;
                        workWinterCondRec.PreparedForWork = modifiedRecord.PreparedForWork;
                        workWinterCondRec.FinishedWorks = modifiedRecord.FinishedWorks;

                        if (modifiedRecord.PreparationTask == 0)
                        {
                            workWinterCondRec.Percent = 0;
                        }
                        else
                        {
                            workWinterCondRec.Percent = modifiedRecord.PreparedForWork / modifiedRecord.PreparationTask;
                        }

                        this.workWinterCondDomain.Update(workWinterCondRec);
                    }
                }

                return new BaseDataResult();
            }
            catch (Exception e)
            {
                return new BaseDataResult { Success = false, Message = e.Message };
            }
        }

        /// <summary>
        /// Копирование данных из периода 
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns>Результат выполнения запроса</returns>
        public IDataResult CopyPeriodWorkWinterCondition(BaseParams baseParams)
        {
            var copyMonth = baseParams.Params.GetAs<int>("copymonth");
            var copyYear = baseParams.Params.GetAs<int>("copyyear");
            var heatInputPeriodId = baseParams.Params.GetAs<int>("hipId");
            var municipalityId = baseParams.Params.GetAs<string>("municipality");

            using (var transaction = this.Container.Resolve<IDataTransaction>())
            {
                try
                {
                    var workWintersCopyTo = this.workWinterCondDomain.GetAll()
                        .Where(x => x.HeatInputPeriod.Id == heatInputPeriodId)
                        .ToList();

                    var heatInputPeriodCopyFromId = this.heatInputPeriodDomain.GetAll()
                        .Where(x => Convert.ToInt32(x.Month) == copyMonth)
                        .Where(x => x.Year == copyYear)
                        .Where(x => x.Municipality.Name == municipalityId)
                        .Select(x => x.Id).FirstOrDefault();

                    if (heatInputPeriodCopyFromId == 0)
                    {
                        throw new Exception("Не найден период!");
                    }

                    var workWintersCopyFrom = this.workWinterCondDomain.GetAll()
                        .Where(x => x.HeatInputPeriod.Id == heatInputPeriodCopyFromId)
                        .Select(x => new { x.WorkInWinterMark.RowNumber, x.Total, x.PreparationTask, x.PreparedForWork, x.FinishedWorks, x.Percent })
                        .AsEnumerable()
                        .GroupBy(x => x.RowNumber)
                        .ToDictionary(
                            x => x.Key,
                            x =>
                                x.Select(
                                    y =>
                                        new
                                        {
                                            y.Total,
                                            y.PreparationTask,
                                            y.PreparedForWork,
                                            y.FinishedWorks,
                                            y.Percent
                                        }).First());

                    foreach (var workWinterCopyin in workWintersCopyTo)
                    {
                        workWinterCopyin.Total = workWintersCopyFrom[workWinterCopyin.WorkInWinterMark.RowNumber].Total;
                        workWinterCopyin.PreparationTask = workWintersCopyFrom[workWinterCopyin.WorkInWinterMark.RowNumber].PreparationTask;
                        workWinterCopyin.PreparedForWork = workWintersCopyFrom[workWinterCopyin.WorkInWinterMark.RowNumber].PreparedForWork;
                        workWinterCopyin.FinishedWorks = workWintersCopyFrom[workWinterCopyin.WorkInWinterMark.RowNumber].FinishedWorks;
                        workWinterCopyin.Percent = workWintersCopyFrom[workWinterCopyin.WorkInWinterMark.RowNumber].Percent;

                        this.workWinterCondDomain.Update(workWinterCopyin);
                    }

                    transaction.Commit();

                }
                catch (Exception ex)
                {
                    transaction.Rollback();

                    return BaseDataResult.Error(ex.Message);
                }
            }
            return new BaseDataResult();
        }


#warning  Бойлерная Надо переделалть чтото непонятное тут происходит
        public IDataResult GetBoilerInfo(BaseParams baseParams)
        {
            var loadParams = this.GetLoadParam(baseParams);
            var periodId = loadParams.Filter.GetAs("hipId", 0L);

            var heatInputPeriod = this.heatInputPeriodDomain.Get(periodId);

            var list = new List<BoilerInfo>();

            if (heatInputPeriod != null)
            {
                var date = new DateTime(heatInputPeriod.Year, heatInputPeriod.Month, 1);
                var periodStart = date;
                var periodEnd = date.AddMonths(1).AddDays(-1);
                var municipalityId = heatInputPeriod.Municipality.Id;

                var boilerRoomsCount = this.boilerRoomDomain.GetAll().Count(x => x.Municipality.Id == municipalityId);

                // Считаем количество НЕАКТИВНЫХ котельных в данном МО в данном месяце. 
                // Котельная считается НЕАКТИВНОЙ, если она была не активна хотя бы один день в месяце за который вводятся данные
                var unactiveCount = this.unacktivePeriodDomain.GetAll()
                    .Where(x => x.BoilerRoom.Municipality.Id == municipalityId)
                    .Where(x => x.Start == null || x.Start.Value.Date <= periodEnd.Date)
                    .Where(x => x.End == null || x.End.Value.Date >= periodStart.Date)
                    .Select(x => x.BoilerRoom.Id)
                    .Distinct()
                    .Count();

                // Считаем количество ЗАПУЩЕННЫХ котельных в данном МО в данном месяце. 
                // Котельная считается ЗАПУЩЕННОЙ, если она была запущена во все дни в течении месяца за который вводят данные.
                // Логика расчета : 
                // 1. берем все периоды запуска, которые пересекаются с нашим периодом
                // 2. Если каждый день нашего месяца содержится в вышеуказанных периодах, то считаем, что в данном месяце котельная запущена
                var startedCount = this.heatingPeriodDomain.GetAll()
                    .Where(x => x.BoilerRoom.Municipality.Id == municipalityId)
                    // 1. берем все периоды запуска, которые пересекаются с нашим периодом
                    .Where(x => x.Start.Date <= periodEnd.Date)
                    .Where(x => x.End == null || x.End.Value.Date >= periodStart.Date)
                    .Select(x => new
                    {
                        x.BoilerRoom.Id,
                        x.Start,
                        x.End
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.Id)
                    .Select(x =>
                        {
                            var periods = x.Select(y => new
                            {
                                startDay = (periodStart < y.Start ? periodStart : y.Start).Day,
                                endDay = (y.End == null ? periodEnd : (periodEnd < y.End.Value ? periodEnd : y.End.Value)).Day
                            })
                            .ToList();

                            // 2. Если каждый день нашего месяца содержится в вышеуказанных периодах, то считаем, что в данном месяце котельная запущена
                            var started = Enumerable.Range(1, periodEnd.Day).All(y => periods.Any(z => z.startDay <= y && z.endDay >= y));

                            return new { x.Key, started };
                        })
                    .Count(x => x.started);

                var boilerInfo = new BoilerInfo
                {
                    Title = "Котельные",
                    Count = Math.Max(boilerRoomsCount - unactiveCount, 0),
                    Started = startedCount,
                };

                boilerInfo.Percent = boilerInfo.Count != 0
                    ? decimal.Round((boilerInfo.Started / boilerInfo.Count) * 100, 2)
                    : 0;

                boilerInfo.NotStarted = Math.Max(boilerInfo.Count - boilerInfo.Started, 0);

                list.Add(boilerInfo);
            }

            return new ListDataResult(list, list.Count());
        }

        protected virtual LoadParam GetLoadParam(BaseParams baseParams)
        {
            return baseParams.Params.Read<LoadParam>().Execute(Converter.ToLoadParam);
        }

        public class BoilerInfo
        {
            public string Title { get; set; }

            public int Count { get; set; }

            public int Started { get; set; }

            public decimal Percent { get; set; }

            public int NotStarted { get; set; }
        }
    }
}