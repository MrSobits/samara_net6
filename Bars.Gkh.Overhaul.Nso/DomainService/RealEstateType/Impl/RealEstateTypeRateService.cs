namespace Bars.Gkh.Overhaul.Nso.DomainService.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using B4;

    using Bars.B4.DataAccess;
    using Bars.Gkh.ConfigSections.Overhaul.Enums;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Overhaul.Nso.ConfigSections;
    using Bars.Gkh.Overhaul.Nso.Enum;
    using Castle.Windsor;
    using Entities;
    using Gkh.DomainService.Dict.RealEstateType;
    using Gkh.Entities.Dicts;
    using Gkh.Entities.RealEstateType;
    using Gkh.Utils;
    using Overhaul.Domain;

    public class RealEstateTypeRateService : IRealEstateTypeRateService
    {
        #region Properties

        public IWindsorContainer Container { get; set; }

        public IRepository<RealEstateTypeRate> RepTypeRate { get; set; }

        public IRepository<EmergencyObject> EmergencyDomain { get; set; }

        #endregion Properties

        public IDataResult CalculateRates(BaseParams baseParams)
        {
            try
            {
                var config = Container.GetGkhConfig<OverhaulNsoConfig>();
                var periodStart = config.ProgrammPeriodStart;
                var periodEnd = config.ProgrammPeriodEnd;
                var rateCalcArea = config.RateCalcTypeArea;

                int period = (periodEnd - periodStart + 1) * 12;

                if (period <= 0)
                {
                    return new BaseDataResult(false, "Указаны неверные значения периода долгосрочной программы");
                }

                var shortPeriod = config.ShortTermProgPeriod;

                if (shortPeriod <= 0) // || shortPeriod > 5)
                {
                    return new BaseDataResult(false, "Указано неверное значение периода краткосрочной программы");
                }

                var roQuery = Container.Resolve<IRepository<RealityObject>>().GetAll()
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
                                            y.First().AreaLivingNotLivingMkd
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
                                    rate.TotalArea += ro.AreaLiving.HasValue ? ro.AreaLiving.Value : 0;
                                    break;
                                case RateCalcTypeArea.AreaLivingNotLiving:
                                    rate.TotalArea += ro.AreaLivingNotLivingMkd.HasValue
                                                          ? ro.AreaLivingNotLivingMkd.Value
                                                          : 0;
                                    break;
                                case RateCalcTypeArea.AreaMkd:
                                    rate.TotalArea += ro.AreaMkd.HasValue ? ro.AreaMkd.Value : 0;
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

                //расчет экономически обоснованного тарифа
                foreach (var typeRateProxy in result)
                {
                    typeRateProxy.Value.ReasonableRate = typeRateProxy.Value.TotalArea > 0
                        ? typeRateProxy.Value.NeedForFunding / period / typeRateProxy.Value.TotalArea
                        : 0;
                }

                UpdateRates(periodStart, shortPeriod, result);
            }
            catch (Exception e)
            {
                return new BaseDataResult(false, e.Message);
            }

            return new BaseDataResult();
        }

        private void UpdateRates(int periodStart, int periodShort, Dictionary<long, TypeRateProxy> dict)
        {
            var listRatesForSave = new List<RealEstateTypeRate>();

            var existRates = RepTypeRate.GetAll()
                .AsEnumerable()
                .GroupBy(x => x.Year)
                .ToDictionary(x => x.Key, y => y.ToDictionary(x => x.RealEstateType != null ? x.RealEstateType.Id : 0));

            //для каждого года в краткосрочной программе
            //формируем или обновляем запись показателей
            for (int year = periodStart; year < periodStart + periodShort; year++)
            {
                foreach (var item in dict)
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
                                RealEstateType = item.Key > 0 ? new RealEstateType { Id = item.Key } : null
                            };
                    }

                    rate.Year = year;

                    if (rate.SociallyAcceptableRate.HasValue)
                    {
                        rate.RateDeficit = item.Value.ReasonableRate - rate.SociallyAcceptableRate.Value;
                    }
                    
                    rate.NeedForFunding = item.Value.NeedForFunding;
                    rate.TotalArea = item.Value.TotalArea;
                    rate.ReasonableRate = item.Value.ReasonableRate;

                    listRatesForSave.Add(rate);
                }
            }

            var ratesForDelete =
                RepTypeRate.GetAll().Where(x => x.Year >= periodStart + periodShort).Select(x => x.Id).ToList();
                
            using (var tr = Container.Resolve<IDataTransaction>())
            {
                try
                {
                    ratesForDelete.ForEach(l => RepTypeRate.Delete(l));

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

            public decimal NeedForFunding { get; set; }

            public decimal ReasonableRate { get; set; }
        }

        #endregion Proxy classes
    }
}