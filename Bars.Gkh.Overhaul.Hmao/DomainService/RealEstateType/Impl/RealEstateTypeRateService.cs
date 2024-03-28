using Bars.B4.Utils;

namespace Bars.Gkh.Overhaul.Hmao.DomainService.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using B4;

    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.Gkh.ConfigSections.Overhaul.Enums;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Overhaul.Hmao.ConfigSections;

    using Castle.Windsor;

    using Entities;

    using Gkh.DomainService.Dict.RealEstateType;
    using Gkh.Entities.Dicts;
    using Gkh.Entities.RealEstateType;
    using Gkh.Utils;

    using ResultType = System.Collections.Generic.Dictionary<int, System.Collections.Generic.Dictionary<long, RealEstateTypeRateService.TypeRateProxy>>;

    /// <summary>
    /// Сервис для расчета тарифа по типам домов
    /// </summary>
    public class RealEstateTypeRateService : IRealEstateTypeRateService
    {
        #region Properties

        /// <summary>
        /// IoC
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Тариф по типам домов
        /// </summary>
        public IRepository<RealEstateTypeRate> RepTypeRate { get; set; }

        /// <summary>
        /// Аварийность жилого дома
        /// </summary>
        public IRepository<EmergencyObject> EmergencyDomain { get; set; }

        /// <summary>
        /// Жилой дом
        /// </summary>
        public IRepository<RealityObject> RealityObjectRepository { get; set; }

        /// <summary>
        /// КЭ дома в ДПКР
        /// </summary>
        public IDomainService<RealityObjectStructuralElementInProgrammStage3> Stage3Domain { get; set; }

        /// <summary>
        /// Сервис для работы с типами домов
        /// </summary>
        public IRealEstateTypeService RealEstateTypeService { get; set; }

        #endregion Properties

        /// <inheritdoc />
        public IDataResult CalculateRates(BaseParams baseParams)
        {
            var config = this.Container.GetGkhConfig<OverhaulHmaoConfig>();

            var periodStart = config.ProgrammPeriodStart;
            var periodEnd = config.ProgrammPeriodEnd;
            var rateCalcArea = config.RateCalcTypeArea;
            var usePeriod = config.UsePeriodInRateCalc;

            var period = 12;    // число месяцев в году

            if (usePeriod)
            {
                period *= periodEnd - periodStart + 1;
            }

            if (period <= 0)
            {
                return BaseDataResult.Error("Указаны неверные значения периода долгосрочной программы");
            }

            var shortPeriod = config.ShortTermProgPeriod;

            if (shortPeriod <= 0 || shortPeriod > 100)
            {
                return BaseDataResult.Error("Указано неверное значение периода краткосрочной программы");
            }

            var roQuery = this.RealityObjectRepository.GetAll()
                .Where(x => x.TypeHouse != TypeHouse.Individual && x.TypeHouse != TypeHouse.NotSet)
                .Where(x => x.ConditionHouse == ConditionHouse.Serviceable || x.ConditionHouse == ConditionHouse.Dilapidated)
                .Where(x => this.Stage3Domain.GetAll().Any(y => y.RealityObject.Id == x.Id))
                .Where(x => !this.EmergencyDomain.GetAll().Any(e => e.RealityObject.Id == x.Id)
                    || this.EmergencyDomain.GetAll()
                        .Where(e => e.ConditionHouse == ConditionHouse.Serviceable || e.ConditionHouse == ConditionHouse.Dilapidated)
                        .Any(e => e.RealityObject.Id == x.Id))
                .Where(x => !x.IsNotInvolvedCr);

            var roDict = this.RealEstateTypeService.GetRealEstateTypes(roQuery);

            var roSums = this.Stage3Domain.GetAll()
                .Select(x => new
                {
                    RoId = x.RealityObject.Id,
                    x.RealityObject.AreaMkd,
                    x.RealityObject.AreaLivingNotLivingMkd,
                    x.RealityObject.AreaLiving,
                    x.Sum,
                    x.Year
                })
                .AsEnumerable()
                .GroupBy(x => x.Year)
                .ToDictionary(x => x.Key,
                    y => y.GroupBy(z => z.RoId)
                        .ToDictionary(k => k.Key,
                            v => new RateSumsProxy
                            {
                                Sum = v.Sum(s => s.Sum),
                                AreaMkd = v.First().AreaMkd.GetValueOrDefault(),
                                AreaLiving = v.First().AreaLiving.GetValueOrDefault(),
                                AreaLivingNotLivingMkd = v.First().AreaLivingNotLivingMkd.GetValueOrDefault()
                            }));

            var result = this.InitResultDict(periodStart, shortPeriod);

            foreach (var yearResult in result)
            {
                foreach (var roRealEstInfo in roDict)
                {
                    foreach (var reId in roRealEstInfo.Value)
                    {
                        var rate = yearResult.Value[reId];

                        rate.Id = reId;

                        if (roSums.ContainsKey(yearResult.Key) && roSums[yearResult.Key].ContainsKey(roRealEstInfo.Key))
                        {
                            var ro = roSums[yearResult.Key][roRealEstInfo.Key];

                            rate.NeedForFunding += ro.Sum;

                            switch (rateCalcArea)
                            {
                            case RateCalcTypeArea.AreaLiving:
                                rate.TotalArea += ro.AreaLiving;
                                break;
                            case RateCalcTypeArea.AreaLivingNotLiving:
                                rate.TotalArea += ro.AreaLivingNotLivingMkd;
                                break;
                            case RateCalcTypeArea.AreaMkd:
                                rate.TotalArea += ro.AreaMkd;
                                break;
                            }
                        }
                    }
                }

                // расчет экономически обоснованного тарифа
                foreach (var typeRateProxy in yearResult.Value)
                {
                    typeRateProxy.Value.ReasonableRate = typeRateProxy.Value.TotalArea > 0
                        ? typeRateProxy.Value.NeedForFunding / period / typeRateProxy.Value.TotalArea
                        : 0;
                }
            }

            this.UpdateRates(periodStart, shortPeriod, result);

            return new BaseDataResult();
        }

        private void UpdateRates(int periodStart, int shortPeriod, ResultType dict)
        {
            var listRatesForSave = new List<RealEstateTypeRate>();

            var existRates = this.RepTypeRate.GetAll()
                .AsEnumerable()
                .GroupBy(x => x.Year)
                .ToDictionary(x => x.Key, y => y.ToDictionary(x => x.RealEstateType?.Id ?? 0));

            // для каждого года в краткосрочной программе
            // формируем или обновляем запись показателей
            for (int year = periodStart; year < periodStart + shortPeriod; year++)
            {
                foreach (var item in dict[year])
                {
                    RealEstateTypeRate rate;

                    if (existRates.ContainsKey(year) && existRates[year].ContainsKey(item.Key))
                    {
                        rate = existRates[year][item.Key];
                    }
                    else
                    {
                        rate =
                            new RealEstateTypeRate
                            {
                                RealEstateType = item.Key > 0 ? new RealEstateType {Id = item.Key} : null
                            };
                    }

                    rate.Year = year;

                    rate.RateDeficit = item.Value.ReasonableRate - rate.SociallyAcceptableRate.ToDecimal();

                    rate.NeedForFunding = item.Value.NeedForFunding;
                    rate.TotalArea = item.Value.TotalArea;
                    rate.ReasonableRate = item.Value.ReasonableRate;

                    listRatesForSave.Add(rate);
                }
            }

            using (var tr = this.Container.Resolve<IDataTransaction>())
            {
                try
                {
                    listRatesForSave.ForEach(this.RepTypeRate.Save);

                    tr.Commit();
                }
                catch (Exception)
                {
                    tr.Rollback();
                    throw;
                }
            }
        }

        private ResultType InitResultDict(int periodStart, int shortPeriod)
        {
            var result = new ResultType();
            var realEstateTypeDomain = this.Container.Resolve<IDomainService<RealEstateType>>();

            using (this.Container.Using(realEstateTypeDomain))
            {
                for (int year = periodStart; year < periodStart + shortPeriod; year++)
                {
                    var subDict = realEstateTypeDomain.GetAll()
                        .Select(x => x.Id)
                        .ToDictionary(x => x, x => new TypeRateProxy { Id = x });

                    subDict.Add(0, new TypeRateProxy { Id = 0 });

                    result.Add(year, subDict);
                }

                return result;
            }
        }

        #region Proxy classes

        internal class TypeRateProxy
        {
            public long Id { get; set; }

            public decimal TotalArea { get; set; }

            public decimal NeedForFunding { get; set; }

            public decimal ReasonableRate { get; set; }

            public int Year { get; set; }
        }

        private class RateSumsProxy
        {
            public decimal Sum { get; set; }

            public decimal AreaMkd { get; set; }

            public decimal AreaLiving { get; set; }

            public decimal AreaLivingNotLivingMkd { get; set; }
        }

        #endregion Proxy classes
    }
}