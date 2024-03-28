namespace Bars.Gkh.RegOperator.Domain.ParametersVersioning
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using B4.DataAccess;
    using B4.Utils;

    using Bars.Gkh.Entities;
    using Bars.Gkh.Entities.Dicts;
    using Bars.Gkh.RegOperator.Enums;

    using Castle.Windsor;
    using Decisions.Nso.Entities;
    using Entities;
    using Gkh.Domain.ParameterVersioning.Proxy;
    using Gkh.Entities.RealEstateType;
    using Overhaul.Entities;

    public class TariffVersionedParameter : AbstractVersionedParameter
    {
        /// <summary>
        /// .ctor
        /// </summary>
        public TariffVersionedParameter(
            IWindsorContainer container,
            BasePersonalAccount account,
            ITariffCache tariffCache)
            : base(container)
        {
            var ro = account
                .Return(x => x.Room)
                .Return(x => x.RealityObject);

            this.roTypes = tariffCache.GetRoTypes(ro);
            this.settlTypeTariff = tariffCache.GetSettlementPaysizesByType(ro);
            this.muTypeTariff = tariffCache.GetMunicipalityPaysizesByType(ro);
            this.settlTariff = tariffCache.GetSettlementPaysizes(ro);
            this.muTariff = tariffCache.GetMunicipalityPaysizes(ro);
            this.roFees = tariffCache.GetDecisions(ro);
            this.entrances = tariffCache.GetEntrances(ro);
        }

        public override string ParameterName
        {
            get { return VersionedParameters.BaseTariff; }
            set { }
        }

        protected internal override PersistentObject GetPersistentObject()
        {
            return null;
        }

        public override IEnumerable<EntityLogRecord> GetChanges(BasePersonalAccount account, DateTime date)
        {
            return new List<EntityLogRecord>();
        }

        public override KeyValuePair<object, T> GetActualByDate<T>(BasePersonalAccount account, DateTime date, bool limitDateApplied)
        {
            TariffSource source;

            var value = this.GetEntranceSize(account, date);

            if (value.HasValue)
            {
                source = new TariffSource
                {
                    TariffSourceType = TariffSourceType.EntranceSize
                };

                return new KeyValuePair<object, T>(source, (T)(object)value.Value);
            }

            var paySizeByType = this.GetPaysizeByType(this.settlTypeTariff, date);

            value = paySizeByType?.Value;

            if (value.HasValue)
            {
                source = new TariffSource
                {
                    Municipality = paySizeByType.Record.Municipality.Name,
                    RoType = paySizeByType.RealEstateType.Name,
                    TariffSourceType = TariffSourceType.PaysizeByType
                };

                return new KeyValuePair<object, T>(source, (T)(object)value.Value);
            }

            paySizeByType = this.GetPaysizeByType(this.muTypeTariff, date);

            value = paySizeByType?.Value;

            if (value.HasValue)
            {
                source = new TariffSource
                {
                    Municipality = paySizeByType.Record.Municipality.Name,
                    RoType = paySizeByType.RealEstateType.Name,
                    TariffSourceType = TariffSourceType.PaysizeByType
                };

                return new KeyValuePair<object, T>(source, (T)(object)value.Value);
            }

            var paySizeByMu = this.GetPaysizeByMu(this.settlTariff, date);
            value = paySizeByMu?.Value;

            if (value.HasValue)
            {
                source = new TariffSource
                {
                    Municipality = paySizeByMu.Municipality.Name,
                    TariffSourceType = TariffSourceType.PaysizeByMu
                };

                return new KeyValuePair<object, T>(source, (T)(object)value.Value);
            }

            paySizeByMu = this.GetPaysizeByMu(this.muTariff, date);
            value = paySizeByMu?.Value;

            if (value.HasValue)
            {
                source = new TariffSource
                {
                    Municipality = paySizeByMu.Municipality.Name,
                    TariffSourceType = TariffSourceType.PaysizeByMu
                };

                return new KeyValuePair<object, T>(source, (T)(object)value.Value);
            }

            return new KeyValuePair<object, T>(null, default(T));
        }

        private PaysizeRealEstateType GetPaysizeByType(Dictionary<long, PaysizeRealEstateType[]> dict, DateTime date, IEnumerable<long> realEstateTypeIds = null)
        {
            PaysizeRealEstateType value = null;

            realEstateTypeIds = realEstateTypeIds ?? this.roTypes.Select(x => x.RealEstateType.Id);

            //получаем максимальный тариф по типу дома
            foreach (var retId in realEstateTypeIds)
            {
                if (dict.ContainsKey(retId))
                {
                    if (dict[retId]
                        .Where(x => x.Record.Paysize.DateStart <= date)
                        .Any(x => !x.Record.Paysize.DateEnd.HasValue || x.Record.Paysize.DateEnd >= date))
                    {
                        value = dict[retId]
                            .Where(x => x.Record.Paysize.DateStart <= date)
                            .Where(x => !x.Record.Paysize.DateEnd.HasValue || x.Record.Paysize.DateEnd >= date)
                            .OrderByDescending(x => x.Value)
                            .FirstOrDefault();
                    }
                }
            }

            return value;
        }

        private PaysizeRecord GetPaysizeByMu(IEnumerable<PaysizeRecord> list, DateTime date)
        {
            return list
                .OrderByDescending(x => x.Paysize.DateStart)
                .Where(x => !x.Paysize.DateEnd.HasValue || x.Paysize.DateEnd.Value >= date)
                .FirstOrDefault(x => x.Paysize.DateStart <= date);
        }

        /// <summary>
        /// Возвращает значение с учетом принятого решения на доме
        /// </summary>
        protected override KeyValuePair<object, T> GetActualByDateNonCached<T>(BasePersonalAccount account, DateTime date, bool limitDateApplied)
        {
            var paysize = (this.GetEntranceSize(account, date)
                    ?? this.GetPaysizeByType(this.settlTypeTariff, date)?.Value
                    ?? this.GetPaysizeByType(this.muTypeTariff, date)?.Value
                    ?? this.GetPaysizeByMu(this.settlTariff, date)?.Value
                    ?? this.GetPaysizeByMu(this.muTariff, date)?.Value
                    ?? 0).To<T>();

            var decision = this.GetProtocolSize(date).To<T>();

            if (typeof(T) == typeof(decimal))
            {
                return new KeyValuePair<object, T>(null, Math.Max(paysize.To<decimal>(), decision.To<decimal>()).To<T>());
            }

            if (typeof(T) == typeof(int))
            {
                return new KeyValuePair<object, T>(null, Math.Max(paysize.To<int>(), decision.To<int>()).To<T>());
            }

            throw new InvalidOperationException();
        }

        public override KeyValuePair<object, T> GetLastChangedByDate<T>(BasePersonalAccount account, IPeriod period, DateTime date)
        {
            return default(KeyValuePair<object, T>);
        }

        private decimal? GetEntranceSize(BasePersonalAccount account, DateTime date)
        {
            var eId = account.Room.Entrance.Return(x => x.Id);

            if (!this.entrances.ContainsKey(eId))
            {
                return null;
            }

            var array = new[] { this.entrances[eId] };

            return this.GetPaysizeByType(this.settlTypeTariff, date, array)?.Value
                   ?? this.GetPaysizeByType(this.muTypeTariff, date, array)?.Value;
        }

        private decimal? GetProtocolSize(DateTime date)
        {
            var periodMonthlyFees = this.roFees
                .Where(x => x.Item1 <= date)
                .Select(x => x.Item2)
                .FirstOrDefault();

            decimal? size = null;

            if (periodMonthlyFees != null && periodMonthlyFees.Any())
            {
                size = periodMonthlyFees
                    .Where(x => x.From <= date)
                    .Where(x => !x.To.HasValue || x.To >= date)
                    .OrderByDescending(x => x.From)
                    .Select(x => x.Value)
                    .FirstOrDefault();
            }

            return size;
        }

        private readonly RealEstateTypeRealityObject[] roTypes;
        private readonly Dictionary<long, PaysizeRealEstateType[]> settlTypeTariff;
        private readonly Dictionary<long, PaysizeRealEstateType[]> muTypeTariff;
        private readonly PaysizeRecord[] muTariff;
        private readonly PaysizeRecord[] settlTariff;
        private readonly Tuple<DateTime, PeriodMonthlyFee[]>[] roFees;
        private readonly Dictionary<long, long> entrances;
    }

    /// <summary>
    /// Информация о тарифе и откуда он взялся
    /// </summary>
    public class TariffSource
    {
        public string Municipality { get; set; }

        public string RoType { get; set; }

        public string ProtocolNum { get; set; }

        public DateTime? ProtocolDate { get; set; }

        public TariffSourceType TariffSourceType { get; set; }
    }
}