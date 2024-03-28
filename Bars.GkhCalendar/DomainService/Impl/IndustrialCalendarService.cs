namespace Bars.GkhCalendar.DomainService.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.GkhCalendar.Entities;
    using Bars.GkhCalendar.Enums;

    using Castle.Windsor;

    public class IndustrialCalendarService : IIndustrialCalendarService
    {
        #region Public Properties

        public IWindsorContainer Container { get; set; }

        public IDomainService<Day> DayDomain { get; set; }

        #endregion

        #region Public Methods and Operators

        public DateTime GetDateAfterWorkDays(DateTime date, uint workDaysCount)
        {
            var monthDateStart = new DateTime(date.Year, date.Month, 1);
            var tmp = 0;
            var days = new List<DateTime>();

            while (workDaysCount != tmp)
            {
                if (!this.CheckAvailabilityMonthData(monthDateStart))
                {
                    this.CreateDays(monthDateStart);
                }

                monthDateStart = new DateTime(monthDateStart.AddMonths(1).Year, monthDateStart.AddMonths(1).Month, 1);

                days =
                    this.DayDomain.GetAll()
                        .Where(x => x.DayDate >= date)
                        .Where(x => x.DayDate < monthDateStart)
                        .Where(x => x.DayType == DayType.Workday)
                        .OrderBy(x => x.DayDate)
                        .Take((int)workDaysCount)
                        .AsEnumerable()
                        .GroupBy(x => x.DayDate)
                        .Select(x => x.Select(y => y.DayDate).First())
                        .ToList();

                tmp = days.Count;
            }

            return days.LastOrDefault();
        }

        public List<Day> GetWorkDays(DateTime startDate, DateTime endDate)
        {
            if (endDate < startDate)
            {
                var tmp = endDate;
                endDate = startDate;
                startDate = tmp;
            }

            var haveMonthsList =
                this.DayDomain.GetAll()
                    .Where(x => x.DayDate >= startDate)
                    .Where(x => x.DayDate <= endDate)
                    .Select(x => new { x.DayDate.Month, x.DayDate.Year })
                    .AsEnumerable()
                    .Distinct()
                    .GroupBy(x => x.Year)
                    .ToDictionary(x => x.Key, x => x.Select(y => y.Month).Distinct().ToList());

            var tmpDay = new DateTime(startDate.Year, startDate.Month, 01);

            // добавляем в БД месяц, если нет записей по этому месяцу
            while (tmpDay <= endDate)
            {
                List<int> monthList;
                if (!haveMonthsList.TryGetValue(tmpDay.Year, out monthList) || !monthList.Contains(tmpDay.Month))
                {
                    this.CreateDays(tmpDay);
                }

                tmpDay = tmpDay.AddMonths(1);
            }

            return this.DayDomain.GetAll()
                .Where(x => x.DayDate > startDate)
                .Where(x => x.DayDate <= endDate)
                .Where(x => x.DayType == DayType.Workday)
                .ToList();
        }

        public int GetWorkDaysCount(DateTime startDate, DateTime endDate)
        {
            return this.GetWorkDays(startDate, endDate).Count;
        }

        #endregion

        #region Methods

        private bool CheckAvailabilityMonthData(DateTime monthDateStart)
        {
            return this.DayDomain.GetAll().Any(x => x.DayDate == monthDateStart);
        }

        private void CreateDays(DateTime date)
        {
            var nextMonthDate = date.AddMonths(1);

            using (var transaction = this.Container.Resolve<IDataTransaction>())
            {
                try
                {
                    for (var dt = date; dt < nextMonthDate; dt = dt.AddDays(1))
                    {
                        var isWorkDay = dt.DayOfWeek != DayOfWeek.Saturday && dt.DayOfWeek != DayOfWeek.Sunday;

                        var newObj = new Day { DayDate = dt, DayType = isWorkDay ? DayType.Workday : DayType.DayOff };

                        this.DayDomain.Save(newObj);
                    }

                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                }
            }
        }

        #endregion
    }
}