namespace Bars.Gkh.Overhaul.Regions.Saha.DomainService.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.Gkh.ConfigSections.Overhaul.Enums;
    using Bars.Gkh.DomainService.Dict.RealEstateType;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Entities.Dicts;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Overhaul.Hmao.ConfigSections;
    using Bars.Gkh.Overhaul.Hmao.DomainService;
    using Bars.Gkh.Overhaul.Hmao.Entities;
    using Bars.Gkh.Overhaul.Regions.Saha.Entities.RealEstateType;
    using Bars.Gkh.Utils;

    using Castle.Windsor;

    public class SahaRealEstateTypeRateService : IRealEstateTypeRateService
    {
        public IWindsorContainer Container { get; set; }
        public IRepository<SahaRealEstateTypeRate> RepTypeRate { get; set; }
        public IRepository<EmergencyObject> EmergencyDomain { get; set; }

        public IDataResult CalculateRates(BaseParams baseParams)
        {
            var config = Container.GetGkhConfig<OverhaulHmaoConfig>();
            var periodStart = config.ProgrammPeriodStart;
            var periodEnd = config.ProgrammPeriodEnd;
            var rateCalcArea = config.RateCalcTypeArea;

            var period = (periodEnd - periodStart + 1) * 12;

            if (period <= 0)
            {
                return new BaseDataResult(false, "Указаны неверные значения периода долгосрочной программы");
            }

            var shortPeriod = config.ShortTermProgPeriod;

            if (shortPeriod <= 0 || shortPeriod > 100)
            {
                return new BaseDataResult(false, "Указано неверное значение периода краткосрочной программы");
            }

            var stage3Domain = Container.Resolve<IDomainService<RealityObjectStructuralElementInProgrammStage3>>();

            var roQuery = Container.Resolve<IRepository<RealityObject>>().GetAll()
                .Where(x => stage3Domain.GetAll().Any(y => y.RealityObject.Id == x.Id))
                .Where(x => x.TypeHouse != TypeHouse.Individual && x.TypeHouse != TypeHouse.NotSet)
                .Where(x => x.ConditionHouse == ConditionHouse.Serviceable || x.ConditionHouse == ConditionHouse.Dilapidated)
                .Where(x => !EmergencyDomain.GetAll().Any(e => e.RealityObject.Id == x.Id)
                    || EmergencyDomain.GetAll()
                        .Where(e => e.ConditionHouse == ConditionHouse.Serviceable || e.ConditionHouse == ConditionHouse.Dilapidated)
                        .Any(e => e.RealityObject.Id == x.Id))
                .Where(x => !x.IsNotInvolvedCr);

            var roDict = Container.Resolve<IRealEstateTypeService>().GetRealEstateTypes(roQuery);

            var roSums = Container.Resolve<IDomainService<RealityObjectStructuralElementInProgrammStage3>>().GetAll()
                .Select(x => new
                {
                    RoId = x.RealityObject.Id,
                    x.RealityObject.AreaMkd,
                    x.RealityObject.AreaLivingNotLivingMkd,
                    x.RealityObject.AreaLiving,
                    x.Sum
                })
                .AsEnumerable()
                .GroupBy(x => x.RoId)
                .ToDictionary(x => x.Key, y => new
                {
                    Sum = y.Sum(x => x.Sum),
                    y.First().AreaMkd,
                    y.First().AreaLiving,
                    y.First().AreaLivingNotLivingMkd,
                    AreaNotLivingMkd = y.First().AreaLivingNotLivingMkd != null 
                        ? y.First().AreaLivingNotLivingMkd.Value - (y.First().AreaLiving ?? 0) 
                        : 0
                });

            var result = Container.Resolve<IDomainService<RealEstateType>>().GetAll()
                .Select(x => x.Id)
                .ToDictionary(x => x, x => new TypeRateProxy { Id = x });

            result.Add(0, new TypeRateProxy { Id = 0 });
            var roTypeDict = new Dictionary<long, List<long>>();

            foreach (var roRealEstInfo in roDict)
            {
                foreach (var reId in roRealEstInfo.Value)
                {
                    var rate = result[reId];

                    rate.Id = reId;

                    if (roSums.ContainsKey(roRealEstInfo.Key))
                    {
                        var ro = roSums[roRealEstInfo.Key];

                        rate.NeedForFunding += ro.Sum;

                        switch (rateCalcArea)
                        {
                            case RateCalcTypeArea.AreaLiving:
                                rate.TotalArea += ro.AreaLiving ?? 0;
                                rate.TotalNotLivingArea += ro.AreaNotLivingMkd;
                                rate.TotalLivingNotLivingArea += ro.AreaLivingNotLivingMkd ?? 0;
                                break;
                            case RateCalcTypeArea.AreaLivingNotLiving:
                                rate.TotalArea += ro.AreaLivingNotLivingMkd ?? 0;
                                break;
                            case RateCalcTypeArea.AreaMkd:
                                rate.TotalArea += ro.AreaMkd ?? 0;
                                break;
                        }
                    }

                    if (reId > 0)
                    {
                        if (!roTypeDict.ContainsKey(reId))
                        {
                            roTypeDict.Add(reId, new List<long>());
                        }

                        roTypeDict[reId].Add(roRealEstInfo.Key);
                    }
                }
            }

            // расчет экономически обоснованного тарифа
            if (rateCalcArea == RateCalcTypeArea.AreaLiving)
            {
                foreach (var typeRateProxy in result)
                {
                    var reasonableRateLivingNotLivingArea = typeRateProxy.Value.TotalLivingNotLivingArea > 0
                                                                      ? typeRateProxy.Value.NeedForFunding / period
                                                                        / typeRateProxy.Value.TotalLivingNotLivingArea
                                                                      : 0;
                    typeRateProxy.Value.ReasonableRateNotLivingArea = typeRateProxy.Value.TotalLivingNotLivingArea > 0
                        ? (reasonableRateLivingNotLivingArea / typeRateProxy.Value.TotalLivingNotLivingArea) * typeRateProxy.Value.TotalNotLivingArea
                        : 0;

                    typeRateProxy.Value.ReasonableRate = typeRateProxy.Value.TotalArea > 0
                        ? (reasonableRateLivingNotLivingArea / typeRateProxy.Value.TotalLivingNotLivingArea) * typeRateProxy.Value.TotalArea
                        : 0;
                }
            }
            else
            {
                foreach (var typeRateProxy in result)
                {
                    typeRateProxy.Value.ReasonableRate = typeRateProxy.Value.TotalArea > 0
                        ? typeRateProxy.Value.NeedForFunding / period / typeRateProxy.Value.TotalArea
                        : 0;
                }
            }

            UpdateRates(periodStart, shortPeriod, result);

            return new BaseDataResult();
        }

        private void UpdateRates(int periodStart, int periodShort, Dictionary<long, TypeRateProxy> dict)
        {
            var listRatesForSave = new List<SahaRealEstateTypeRate>();

            var existRates = RepTypeRate.GetAll()
                .AsEnumerable()
                .GroupBy(x => x.Year)
                .ToDictionary(x => x.Key, y => y.ToDictionary(x => x.RealEstateType != null ? x.RealEstateType.Id : 0));

            // для каждого года в краткосрочной программе
            // формируем или обновляем запись показателей
            for (var year = periodStart; year < periodStart + periodShort; year++)
            {
                foreach (var item in dict)
                {
                    SahaRealEstateTypeRate rate;

                    if (existRates.ContainsKey(year) && existRates[year].ContainsKey(item.Key))
                    {
                        rate = existRates[year][item.Key];
                    }
                    else
                    {
                        rate =
                            new SahaRealEstateTypeRate
                            {
                                RealEstateType = item.Key > 0 ? new RealEstateType { Id = item.Key } : null
                            };
                    }

                    rate.Year = year;

                    rate.RateDeficit = item.Value.ReasonableRate - rate.SociallyAcceptableRate.ToDecimal();
                    rate.RateDeficitNotLivingArea = item.Value.ReasonableRateNotLivingArea
                                                    - rate.SociallyAcceptableRateNotLivingArea.ToDecimal();
                    rate.NeedForFunding = item.Value.NeedForFunding;
                    rate.TotalArea = item.Value.TotalArea;
                    rate.TotalNotLivingArea = item.Value.TotalNotLivingArea;
                    rate.ReasonableRate = item.Value.ReasonableRate;
                    rate.ReasonableRateNotLivingArea = item.Value.ReasonableRateNotLivingArea;

                    listRatesForSave.Add(rate);
                }
            }

            using (var tr = Container.Resolve<IDataTransaction>())
            {
                try
                {
                    listRatesForSave.ForEach(RepTypeRate.Save);

                    tr.Commit();
                }
                catch (Exception)
                {
                    tr.Rollback();
                    throw;
                }
            }
        }

        #region Proxy classes

        private class TypeRateProxy
        {
            public long Id { get; set; }
            public decimal TotalArea { get; set; }
            public decimal TotalNotLivingArea { get; set; }
            public decimal TotalLivingNotLivingArea { get; set; }
            public decimal NeedForFunding { get; set; }
            public decimal ReasonableRate { get; set; }
            public decimal ReasonableRateNotLivingArea { get; set; }
        }

        #endregion Proxy classes
    }
}