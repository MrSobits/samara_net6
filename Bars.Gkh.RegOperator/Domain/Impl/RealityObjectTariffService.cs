namespace Bars.Gkh.RegOperator.Domain.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.Gkh.Decisions.Nso.Entities;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Entities.RealEstateType;
    using Bars.Gkh.Overhaul.DomainService;
    using Bars.Gkh.Overhaul.Entities;

    using Castle.Windsor;

    using NHibernate.Linq;

    using ServiceStack.Common;

    /// <summary>
    /// Сервис тарифов домов
    /// </summary>
    public class RealityObjectTariffService : IRealityObjectTariffService
    {
        /// <summary>
        /// Контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Заполнить тарифы
        /// </summary>
        /// <returns>Результат</returns>
        public Dictionary<long, decimal> FillRealityObjectTariffDictionary()
        {
            var decProtocolDomain = this.Container.ResolveDomain<RealityObjectDecisionProtocol>();
            var monthlyFeeAmountDecDomain = this.Container.ResolveDomain<MonthlyFeeAmountDecision>();
            var realityObjectDomain = this.Container.Resolve<IRepository<RealityObject>>();
            var realEstTypeRealObjDomain = this.Container.ResolveDomain<RealEstateTypeRealityObject>();
            var paysizeRecordDomain = this.Container.ResolveDomain<PaysizeRecord>();
            var paysizeRealEstateTypeDomain = this.Container.ResolveDomain<PaysizeRealEstateType>();

            try
            {
                var protocolIds = decProtocolDomain.GetAll()
                    .OrderByDescending(x => x.ProtocolDate)
                    .Where(x => x.State.FinalState)
                    .GroupBy(x => new {RoId = x.RealityObject.Id, x.Id})
                    .Select(x => x.FirstOrDefault())
                    .ToList()
                    .Select(x => x.Id)
                    .ToHashSet();

                var monthlyFeeDecisions = monthlyFeeAmountDecDomain.GetAll()
                    .Fetch(x => x.Protocol)
                    .ThenFetch(x => x.RealityObject)
                    .ToList()
                    .Where(x => protocolIds.Contains(x.Protocol.Id))
                    .GroupBy(x => x.Protocol.RealityObject.Id)
                    .ToDictionary(x => x.Key, y => y.OrderByDescending(x => x.Protocol.ProtocolDate).First());

                var realEstTypesByRo = realEstTypeRealObjDomain.GetAll()
                    .Select(
                        x => new
                        {
                            RoId = x.RealityObject.Id,
                            RealEstTypeId = x.RealEstateType.Id,
                        })
                    .ToList()
                    .GroupBy(x => x.RoId)
                    .ToDictionary(x => x.Key, y => y.Select(x => x.RealEstTypeId).ToList());

                var paysizeRetCache = paysizeRealEstateTypeDomain.GetAll()
                    .Where(x => x.Value.HasValue)
                    .Fetch(x => x.Record)
                    .ThenFetch(x => x.Paysize)
                    .ToList()
                    .GroupBy(x => x.Record.Municipality.Id)
                    .ToDictionary(
                        x => x.Key,
                        y => y
                            .GroupBy(x => x.RealEstateType.Id)
                            .ToDictionary(x => x.Key, z => z.ToList()));

                var paysizeRecCache = paysizeRecordDomain.GetAll()
                    .Where(x => x.Value.HasValue)
                    .Fetch(x => x.Paysize)
                    .ToList()
                    .GroupBy(x => x.Municipality.Id)
                    .ToDictionary(x => x.Key, y => y.ToList());

                return realityObjectDomain.GetAll()
                    .Select(
                        x => new
                        {
                            x.Id,
                            MuId = (long?) x.Municipality.Id,
                            SettId = (long?) x.MoSettlement.Id
                        })
                    .ToList()
                    .Select(
                        x =>
                        {
                            var roTypes = realEstTypesByRo.Get(x.Id);

                            var paySizeValue = (this.GetPaysizeByType(roTypes, paysizeRetCache.Get(x.SettId.ToLong()), DateTime.Today)
                                ?? this.GetPaysizeByType(roTypes, paysizeRetCache.Get(x.MuId.ToLong()), DateTime.Today)
                                    ?? this.GetPaysizeByMu(paysizeRecCache.Get(x.SettId.ToLong()), DateTime.Today)
                                        ?? this.GetPaysizeByMu(paysizeRecCache.Get(x.MuId.ToLong()), DateTime.Today)
                                            ?? 0).ToDecimal();

                            var roDecision = monthlyFeeDecisions.Get(x.Id);
                            var roDecisionValue = paySizeValue;
                            if (roDecision != null && roDecision.Decision != null)
                            {
                                var current = roDecision.Decision
                                    .Where(y => !y.To.HasValue || y.To >= DateTime.Today)
                                    .FirstOrDefault(y => y.From <= DateTime.Today);

                                if (current != null && current.Value > roDecisionValue)
                                {
                                    roDecisionValue = current.Value;
                                }
                            }

                            return new
                            {
                                x.Id,
                                Value = roDecisionValue - paySizeValue
                            };
                        })
                    .ToDictionary(x => x.Id, y => y.Value);
            }
            finally
            {
                this.Container.Release(decProtocolDomain);
                this.Container.Release(monthlyFeeAmountDecDomain);
                this.Container.Release(realityObjectDomain);
                this.Container.Release(realEstTypeRealObjDomain);
                this.Container.Release(paysizeRecordDomain);
                this.Container.Release(paysizeRealEstateTypeDomain);
            }
        }

        /// <summary>
        /// Заполнить тарифы
        /// </summary>
        /// <param name="roQuery">Запрос жилых домов</param>
        /// <param name="newRoTypes">Новые типы домов</param>
        /// <returns>Результат</returns>
        public Dictionary<long, TarifInfoProxy> FillRealityObjectTariffInfoDictionary(
            IQueryable<RealityObject> roQuery,
            Dictionary<long, List<long>> newRoTypes = null)
        {
            var decProtocolDomain = this.Container.ResolveDomain<RealityObjectDecisionProtocol>();
            var monthlyFeeAmountDecDomain = this.Container.ResolveDomain<MonthlyFeeAmountDecision>();
            var realityObjectDomain = this.Container.Resolve<IRepository<RealityObject>>();
            var realEstTypeRealObjDomain = this.Container.ResolveDomain<RealEstateTypeRealityObject>();
            var paysizeRecordDomain = this.Container.ResolveDomain<PaysizeRecord>();
            var paysizeRealEstateTypeDomain = this.Container.ResolveDomain<PaysizeRealEstateType>();

            try
            {
                var protocolIds = decProtocolDomain.GetAll()
                    .Where(x => roQuery.Any(y => y.Id == x.RealityObject.Id))
                    .OrderByDescending(x => x.ProtocolDate)
                    .Where(x => x.State.FinalState)
                    .GroupBy(x => new {RoId = x.RealityObject.Id, x.Id})
                    .Select(x => x.FirstOrDefault())
                    .ToList()
                    .Select(x => x.Id)
                    .ToHashSet();

                var monthlyFeeDecisions = monthlyFeeAmountDecDomain.GetAll()
                    .Fetch(x => x.Protocol)
                    .ThenFetch(x => x.RealityObject)
                    .ToList()
                    .Where(x => protocolIds.Contains(x.Protocol.Id))
                    .GroupBy(x => x.Protocol.RealityObject.Id)
                    .ToDictionary(x => x.Key, y => y.OrderByDescending(x => x.Protocol.ProtocolDate).First());

                var realEstTypesByRo = newRoTypes ??
                    realEstTypeRealObjDomain.GetAll()
                        .Select(
                            x => new
                            {
                                RoId = x.RealityObject.Id,
                                RealEstTypeId = x.RealEstateType.Id,
                            })
                        .ToList()
                        .GroupBy(x => x.RoId)
                        .ToDictionary(x => x.Key, y => y.Select(x => x.RealEstTypeId).ToList());

                var paysizeRetCache = paysizeRealEstateTypeDomain.GetAll()
                    .Where(x => x.Value.HasValue)
                    .Fetch(x => x.Record)
                    .ThenFetch(x => x.Paysize)
                    .ToList()
                    .GroupBy(x => x.Record.Municipality.Id)
                    .ToDictionary(
                        x => x.Key,
                        y => y
                            .GroupBy(x => x.RealEstateType.Id)
                            .ToDictionary(x => x.Key, z => z.ToList()));

                var paysizeRecCache = paysizeRecordDomain.GetAll()
                    .Where(x => x.Value.HasValue)
                    .Fetch(x => x.Paysize)
                    .ToList()
                    .GroupBy(x => x.Municipality.Id)
                    .ToDictionary(x => x.Key, y => y.ToList());

                return roQuery
                    .Select(
                        x => new
                        {
                            x.Id,
                            MuId = (long?) x.Municipality.Id,
                            SettId = (long?) x.MoSettlement.Id
                        })
                    .ToList()
                    .Select(
                        x =>
                        {
                            var roTypes = realEstTypesByRo.Get(x.Id);

                            var tarifInfoProxy = (this.GetPaysizeInfoByType(roTypes, paysizeRetCache.Get(x.SettId.ToLong()), DateTime.Today)
                                ?? this.GetPaysizeInfoByType(roTypes, paysizeRetCache.Get(x.MuId.ToLong()), DateTime.Today)
                                    ?? this.GetPaysizeInfoByMu(paysizeRecCache.Get(x.SettId.ToLong()), DateTime.Today)
                                        ?? this.GetPaysizeInfoByMu(paysizeRecCache.Get(x.MuId.ToLong()), DateTime.Today)) ?? new TarifInfoProxy();

                            var roDecision = monthlyFeeDecisions.Get(x.Id);
                            var roDecisionValue = tarifInfoProxy.Tarif;
                            if (roDecision != null && roDecision.Decision != null)
                            {
                                var current = roDecision.Decision
                                    .Where(y => !y.To.HasValue || y.To >= DateTime.Today)
                                    .FirstOrDefault(y => y.From <= DateTime.Today);

                                if (current != null && current.Value > roDecisionValue)
                                {
                                    tarifInfoProxy.Tarif = current.Value;
                                    tarifInfoProxy.DateStart = current.From;
                                    tarifInfoProxy.DateEnd = current.To;
                                    tarifInfoProxy.IsFromDecision = true;
                                }
                            }

                            return new
                            {
                                x.Id,
                                Value = tarifInfoProxy
                            };
                        })
                    .ToDictionary(x => x.Id, y => y.Value);
            }
            finally
            {
                this.Container.Release(decProtocolDomain);
                this.Container.Release(monthlyFeeAmountDecDomain);
                this.Container.Release(realityObjectDomain);
                this.Container.Release(realEstTypeRealObjDomain);
                this.Container.Release(paysizeRecordDomain);
                this.Container.Release(paysizeRealEstateTypeDomain);
            }
        }

        private decimal? GetPaysizeByType(IEnumerable<long> roTypes, IDictionary<long, List<PaysizeRealEstateType>> dict, DateTime date)
        {
            if (dict == null || roTypes == null)
            {
                return null;
            }

            decimal? value = null;

            //получаем максимальный тариф по типу дома
            foreach (var roType in roTypes)
            {
                if (dict.ContainsKey(roType))
                {
                    if (dict[roType]
                        .Where(x => x.Record.Paysize.DateStart <= date)
                        .Any(x => !x.Record.Paysize.DateEnd.HasValue || x.Record.Paysize.DateEnd >= date))
                    {
                        value = Math.Max(
                            value ?? 0,
                            dict[roType]
                                .Where(x => x.Record.Paysize.DateStart <= date)
                                .Where(x => !x.Record.Paysize.DateEnd.HasValue || x.Record.Paysize.DateEnd >= date)
                                .Select(x => x.Value)
                                .Max() ?? 0);
                    }
                }
            }

            return value;
        }

        private decimal? GetPaysizeByMu(IEnumerable<PaysizeRecord> list, DateTime date)
        {
            if (list == null)
            {
                return null;
            }

            return list
                .OrderByDescending(x => x.Paysize.DateStart)
                .Where(x => !x.Paysize.DateEnd.HasValue || x.Paysize.DateEnd.Value >= date)
                .FirstOrDefault(x => x.Paysize.DateStart <= date)
                .Return(x => x.Value);
        }

        private TarifInfoProxy GetPaysizeInfoByType(IEnumerable<long> roTypes, IDictionary<long, List<PaysizeRealEstateType>> dict, DateTime date)
        {
            if (dict == null || roTypes == null)
            {
                return null;
            }

            TarifInfoProxy value = null;

            foreach (var roType in roTypes)
            {
                if (dict.ContainsKey(roType))
                {
                    if (dict[roType]
                        .Where(x => x.Record.Paysize.DateStart <= date)
                        .Any(x => !x.Record.Paysize.DateEnd.HasValue || x.Record.Paysize.DateEnd >= date))
                    {
                        var temp = dict[roType]
                            .Where(x => x.Record.Paysize.DateStart <= date)
                            .Where(x => !x.Record.Paysize.DateEnd.HasValue || x.Record.Paysize.DateEnd >= date)
                            .OrderByDescending(x => x.Value)
                            .Select(
                                x => new TarifInfoProxy
                                {
                                    Tarif = x.Value.ToDecimal(),
                                    DateStart = x.Record.Paysize.DateStart,
                                    DateEnd = x.Record.Paysize.DateEnd
                                })
                            .FirstOrDefault();

                        if (value == null || (temp != null && value.Tarif < temp.Tarif))
                        {
                            value = temp;
                        }
                    }
                }
            }

            return value;
        }

        private TarifInfoProxy GetPaysizeInfoByMu(IEnumerable<PaysizeRecord> list, DateTime date)
        {
            if (list == null)
            {
                return null;
            }

            return list
                .OrderByDescending(x => x.Paysize.DateStart)
                .Where(x => !x.Paysize.DateEnd.HasValue || x.Paysize.DateEnd.Value >= date)
                .Where(x => x.Paysize.DateStart <= date)
                .Select(
                    x => new TarifInfoProxy
                    {
                        Tarif = x.Value.ToDecimal(),
                        DateStart = x.Paysize.DateStart,
                        DateEnd = x.Paysize.DateEnd
                    })
                .FirstOrDefault();
        }
    }
}