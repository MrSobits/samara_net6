namespace Bars.Gkh.RegOperator.Domain.Repository.ChargePeriod
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Repositories.ChargePeriod;

    /// <summary>
    /// Репозиторий для работы с периодами
    /// </summary>
    public class ChargePeriodRepository : IChargePeriodRepository
    {
        private readonly IDomainService<ChargePeriod> periodDomain;

        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="periodDomain">Домен-сервис <see cref="ChargePeriod"/></param>
        public ChargePeriodRepository(IDomainService<ChargePeriod> periodDomain)
        {
            this.periodDomain = periodDomain;
            this.chargePeriods = new List<ChargePeriod>();
        }

        private ChargePeriod firstPeriod;
        private ChargePeriod currentPeriod;

        private readonly List<ChargePeriod> chargePeriods;

        /// <summary>
        /// Получить первый период
        /// </summary>
        /// <param name="useCache"> Использовать кэш </param>
        public ChargePeriod GetFirstPeriod(bool useCache = true)
        {
            return !useCache
                ? this.GetFirstPeriodInternal()
                : this.firstPeriod ?? this.GetFirstPeriodInternal();
        }

        /// <summary>
        /// Получить текущий открытый период
        /// </summary>
        public ChargePeriod GetCurrentPeriod(bool useCache = true)
        {
            return !useCache
                ? this.GetCurrentPeriodInternal()
                : this.currentPeriod ?? this.GetCurrentPeriodInternal();
        }

        /// <summary>
        /// Получить последний закрытый период
        /// </summary>
        public ChargePeriod GetLastClosedPeriod()
        {
            return this.periodDomain
                .GetAll()
                .Where(x => x.IsClosed)
                .OrderByDescending(x => x.StartDate)
                .FirstOrDefault();
        }

        /// <summary>
        /// Получить период по дате
        /// </summary>
        public ChargePeriod GetPeriodByDate(DateTime date, bool checkEndPeriod = false)
        {
            var period = this.chargePeriods
                .Where(x => x.StartDate <= date)
                .SingleOrDefault(x => !x.EndDate.HasValue || date <= x.EndDate);

            if (period == null)
            {
                period = this.periodDomain.GetAll()
                    .Where(x => x.StartDate <= date)
                    .SingleOrDefault(x => !x.EndDate.HasValue || date <= x.EndDate);

                if (period != null)
                {
                    this.AddOrUpdatePeriod(period);
                }
            }

            // проверка, не вышла ли искомая дата за предполагаемый конец периода
            // но в кэш мы все равно добавим :)
            if (checkEndPeriod && period.GetEndDate() < date)
            {
                period = null;
            }

            return period;
        }

        /// <summary>
        /// Получить предыдущий период относительно нужного нам периода
        /// </summary>
        /// <param name="period"></param>
        /// <returns>Предыдущий период либо NULL</returns>
        public ChargePeriod GetPreviousPeriod(ChargePeriod period)
        {
            return this.GetPeriodByDate(period.StartDate.AddDays(-1));
        }

        /// <inheritdoc />
        public ChargePeriod Get(long id, bool useCache = true)
        {
            var period = this.chargePeriods
                .SingleOrDefault(x => x.Id == id);

            if (period.IsNull() || !useCache)
            {
                var dbPeriod = this.periodDomain.Get(id);

                if (dbPeriod.IsNotNull())
                {
                    this.AddOrUpdatePeriod(dbPeriod);
                    period = dbPeriod;
                }
            }

            return period;
        }

        /// <summary>
        /// Метод указывает, что кэш является устаревшим
        /// </summary>
        public void InvalidateCache()
        {
            this.chargePeriods.Clear();
            this.GetFirstPeriodInternal();
            this.GetCurrentPeriodInternal();
        }

        /// <summary>
        /// Инициализировать кэш
        /// </summary>
        public void InitCache()
        {
            this.InvalidateCache();
            this.periodDomain.GetAll().ForEach(this.AddOrUpdatePeriod);
        }

        /// <summary>
        /// Получить все закрытые периоды
        /// </summary>
        /// <returns></returns>
        public IQueryable<ChargePeriod> GetAllClosedPeriods()
        {
            return this.periodDomain.GetAll().Where(per => per.IsClosed);
        }

        public List<ChargePeriod> GetAll(bool useCache = true)
        {
            return useCache && this.chargePeriods.Count > 0 ? this.chargePeriods : this.periodDomain.GetAll().ToList();
        }

        private ChargePeriod GetFirstPeriodInternal()
        {
            this.firstPeriod = this.periodDomain.GetAll().OrderBy(x => x.StartDate).FirstOrDefault();
            this.AddOrUpdatePeriod(this.firstPeriod);
            return this.firstPeriod;
        }

        private ChargePeriod GetCurrentPeriodInternal()
        {
            this.currentPeriod = this.periodDomain.GetAll().SingleOrDefault(x => !x.IsClosed);
            this.AddOrUpdatePeriod(this.currentPeriod);
            return this.currentPeriod;
        }

        private void AddOrUpdatePeriod(ChargePeriod period)
        {
            var existsPeriod = this.chargePeriods.SingleOrDefault(x => x.Id == period.Id);
            if (existsPeriod.IsNotNull() && existsPeriod.Equals(period))
            {
                this.chargePeriods.Remove(existsPeriod);
            }

            this.chargePeriods.Add(period);
        }
    }
}