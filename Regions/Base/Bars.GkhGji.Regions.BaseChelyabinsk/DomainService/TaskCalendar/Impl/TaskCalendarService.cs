﻿namespace Bars.GkhGji.Regions.BaseChelyabinsk.DomainService
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using DomainService;
    using Entities;
    using Bars.GkhCalendar.Enums;

    using Castle.Windsor;
    using Bars.GkhGji.Entities;

    public class TaskCalendarService : ITaskCalendarService
    {
        public IWindsorContainer Container { get; set; }

        private static int GetRussianDayOfWeek(DayOfWeek dayOfWeek)
        {
            switch (dayOfWeek)
            {
                case DayOfWeek.Monday:
                    return 1;
                case DayOfWeek.Tuesday:
                    return 2;
                case DayOfWeek.Wednesday:
                    return 3;
                case DayOfWeek.Thursday:
                    return 4;
                case DayOfWeek.Friday:
                    return 5;
                case DayOfWeek.Saturday:
                    return 6;
                case DayOfWeek.Sunday:
                    return 7;
            }

            return 7;
        }

        public IDataResult GetDays(BaseParams baseParams)
        {
            var date = baseParams.Params.GetAs<DateTime?>("date");
            var direction = baseParams.Params.GetAs<int>("direction");

            var ci = new CultureInfo("ru-RU");

            var tmpDate = (date ?? new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1)).AddMonths(direction);

            if (tmpDate != DateTime.MinValue)
            {
                var tmpDays = this.GetMonthDays(tmpDate);

                var tmpResult =
                    tmpDays.OrderBy(x => x.DayDate).Select(x => new DayProxy { id = x.Id, dayOfWeek = GetRussianDayOfWeek(x.DayDate.DayOfWeek), number = x.DayDate.Day, type = x.DayType, taskCount = x.TaskCount }).ToList();

                return tmpResult.Any() ? new BaseDataResult(new { days = tmpResult, date = tmpDate, dateText = tmpDate.ToString("MMMM yyyy", ci) }) : new BaseDataResult();

                //if (tmpResult.Any())
                //{
                //    // добавление дней в список (в начало списка и конец) для количества, кратного семи
                //    var needFirstDaysCount = tmpResult.First().dayOfWeek - 1;
                //    var needLastDaysCount = 7 - tmpResult.Last().dayOfWeek;

                //    var tmpFirstDays = new List<DayProxy>();
                //    for (var i = 1; i <= needFirstDaysCount; i++)
                //    {
                //        tmpFirstDays.Add(new DayProxy { id = -1, dayOfWeek = i });
                //    }

                //    var tmpLastDays = new List<DayProxy>();
                //    for (var i = 7 - needLastDaysCount + 1; i <= 7; i++)
                //    {
                //        tmpLastDays.Add(new DayProxy { id = -1, dayOfWeek = i });
                //    }

                //    var result = new List<DayProxy>();
                //    result.AddRange(tmpFirstDays);
                //    result.AddRange(tmpResult);
                //    result.AddRange(tmpLastDays.OrderBy(x => x.dayOfWeek));

                //    return new BaseDataResult(new { days = result, date = tmpDate, dateText = tmpDate.ToString("MMMM yyyy", ci) });
                //}
            }

            return new BaseDataResult();
        }

        public IDataResult GetListDisposal(BaseParams baseParams)
        {         

            return null;
        }
        public IDataResult GetListProtocolsGji(BaseParams baseParams)
        {

            return null;
        }

        public IDataResult GetListAppeals(BaseParams baseParams)
        {

            return null;
        }

        public IDataResult GetListSOPR(BaseParams baseParams)
        {

            return null;
        }
        public IDataResult GetListResolPros(BaseParams baseParams)
        {
            return null;
        }

        public IDataResult GetListAdmonitions(BaseParams baseParams)
        {

            return null;
        }

        public IDataResult GetListPrescriptionsGji(BaseParams baseParams)
        {

            return null;
        }

        public IDataResult GetListCourtPractice(BaseParams baseParams)
        {

            return null;
        }

        private List<TaskCalendar> GetMonthDays(DateTime tmpDate)
        {
            var firstDayCurMonth = new DateTime(tmpDate.Year, tmpDate.Month, 1);
            var firstDayNextMonth = firstDayCurMonth.AddMonths(1);

            var daysCount = (firstDayNextMonth - firstDayCurMonth).Days;

            var dayList = this.GetRecords(firstDayCurMonth, firstDayNextMonth);

            if (dayList.Count == 0)
            {
                var createResult = this.CreateDays(tmpDate, daysCount);

                if (createResult.Success)
                {
                    dayList = this.GetRecords(firstDayCurMonth, firstDayNextMonth);
                }
            }
            List<TaskCalendar> listInfo = new List<TaskCalendar>();
            dayList.ForEach(day =>
            {
                var dtNow = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                if (day.DayDate < dtNow)
                {
                    day.TaskCount = 0;
                }
                else if (day.DayDate == dtNow)
                {
                    day.TaskCount = 1;
                }
                else
                {
                    day.TaskCount = 2;
                }
                listInfo.Add(day);

            });
            //добавлем информацию о проверках


            //if (dayList.Count == 0)
            //{
            //    createResult = this.CreateDays(tmpDate, daysCount, new List<int>());
            //}

            //if (dayList.Count > 0 && dayList.Count < daysCount)
            //{
            //    createResult = this.CreateDays(tmpDate, daysCount, dayList.Select(x => x.DayDate.Day).ToList());
            //}

            //if (createResult != null && !createResult.Success)
            //{
            //    return new List<Day>();
            //}

            return listInfo;
        }

        private List<TaskCalendar> GetRecords(DateTime firstDayCurMonth, DateTime firstDayNextMonth)
        {
            return
                this.Container.Resolve<IDomainService<TaskCalendar>>()
                    .GetAll()
                    .Where(x => x.DayDate >= firstDayCurMonth)
                    .Where(x => x.DayDate < firstDayNextMonth)
                    .OrderByDescending(x => x.ObjectCreateDate)
                    .AsEnumerable()
                    .GroupBy(x => x.DayDate)
                    .Select(x => x.Select(y => y).First())
                    .ToList();
        }

        /// <summary>
        /// Создает дни месяца
        /// </summary>
        private BaseDataResult CreateDays(DateTime tmpDate, int daysCount) //, List<int> daysNumberList)
        {
            var serviceDays = this.Container.Resolve<IDomainService<TaskCalendar>>();

            using (var transaction = this.Container.Resolve<IDataTransaction>())
            {
                try
                {
                    for (var i = 1; i <= daysCount; i++)
                    {
                        //if (daysNumberList.Contains(i))
                        //{
                        //    continue;
                        //}

                        var dayDate = new DateTime(tmpDate.Year, tmpDate.Month, i);
                        var isWorkDay = dayDate.DayOfWeek != DayOfWeek.Saturday && dayDate.DayOfWeek != DayOfWeek.Sunday;

                        var newObj = new TaskCalendar { DayDate = dayDate, DayType = isWorkDay ? DayType.Workday : DayType.DayOff };

                        serviceDays.Save(newObj);
                    }

                    transaction.Commit();
                    return new BaseDataResult { Success = true };
                }
                catch
                {
                    transaction.Rollback();
                    return new BaseDataResult { Success = false };
                }
            }
        }

        
    }

    internal class DayProxy
    {
        public long id;

        public int dayOfWeek;

        public int number;

        public DayType type;

        public int taskCount;
    }
}