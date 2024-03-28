namespace Bars.Gkh.Overhaul.Domain.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using B4;
    using B4.Utils;
    using B4.Utils.Annotations;
    using Entities;
    using Gkh.Entities;
    using Gkh.Entities.RealEstateType;
    using NHibernate.Linq;

    public class PaysizeRepository : IPaysizeRepository
    {
        private readonly IDomainService<PaysizeRecord> _paysizeDomain;
        private readonly IDomainService<PaysizeRealEstateType> _paysizeRetDomain;
        private readonly IDomainService<RealEstateTypeRealityObject> _roTypeDomain;

        public PaysizeRepository(
            IDomainService<PaysizeRecord> paysizeDomain,
            IDomainService<PaysizeRealEstateType> paysizeRetDomain,
            IDomainService<RealEstateTypeRealityObject> roTypeDomain)
        {
            _paysizeDomain = paysizeDomain;
            _paysizeRetDomain = paysizeRetDomain;
            _roTypeDomain = roTypeDomain;
        }

        public decimal GetRoPaysize(RealityObject robject)
        {
            return GetRoPaysize(robject, DateTime.Today);
        }

        public virtual decimal GetRoPaysize(RealityObject robject, DateTime date)
        {
            ArgumentChecker.NotNull(robject, "robject");

            return (GetRoPaysizeByDecision(robject, date)
                    ?? GetRoPaysizeByType(robject, date)
                    ?? GetRoPaysizeByMu(robject, date)
                    ?? 0).ToDecimal();
        }

        public decimal? GetRoPaysizeByType(RealityObject robject, DateTime date)
        {
            InitCache();

            var roId = robject.Return(x => x.Id);
            var muId = robject.Return(x => x.Municipality).Return(x => x.Id);
            var stlId = robject.Return(x => x.MoSettlement).Return(x => x.Id);

            return GetPaysizeByType(_roTypesCache.Get(roId), _paysizeRetCache.Get(stlId), date)
                   ?? GetPaysizeByType(_roTypesCache.Get(roId), _paysizeRetCache.Get(muId), date);
        }

        public decimal? GetRoPaysizeByMu(RealityObject robject, DateTime date)
        {
            InitCache();

            var muId = robject.Return(x => x.Municipality).Return(x => x.Id);
            var stlId = robject.Return(x => x.MoSettlement).Return(x => x.Id);

            return GetPaysizeByMu(_paysizeRecCache.Get(stlId), date) ?? GetPaysizeByMu(_paysizeRecCache.Get(muId), date);
        }

        public virtual decimal? GetRoPaysizeByDecision(RealityObject robject, DateTime date)
        {
            return null;
        }

        protected decimal? GetPaysizeByType(IEnumerable<long> roTypes, IDictionary<long, IEnumerable<PaysizeRealEstateType>> dict, DateTime date)
        {
            if (dict == null || roTypes == null)
            {
                return null;
            }

            decimal? value = null;

            Func<PaysizeRealEstateType, bool> condition = x =>
                x.Record.Paysize.DateStart <= date
                && (!x.Record.Paysize.DateEnd.HasValue || x.Record.Paysize.DateEnd >= date);

            // получаем максимальный тариф по типу дома
            foreach (var roType in roTypes.Where(dict.ContainsKey))
            {
                if (dict[roType].Any(condition))
                {
                    value = Math.Max(value ?? 0,
                        dict[roType]
                            .Where(condition)
                            .Select(x => x.Value)
                            .Max() ?? 0);
                }
            }

            return value;
        }

        protected decimal? GetPaysizeByMu(IEnumerable<PaysizeRecord> list, DateTime date)
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

        protected virtual void InitCache()
        {
            if (_paysizeRecCache == null)
            {
                _paysizeRecCache = _paysizeDomain.GetAll()
                    .Where(x => x.Value.HasValue)
                    .Fetch(x => x.Paysize)
                    .ToList()
                    .GroupBy(x => x.Municipality.Id)
                    .ToDictionary(x => x.Key, z => z.AsEnumerable());
            }

            if (_paysizeRetCache == null)
            {
                _paysizeRetCache = _paysizeRetDomain.GetAll()
                    .Where(x => x.Value.HasValue)
                    .Fetch(x => x.Record)
                    .ThenFetch(x => x.Paysize)
                    .ToList()
                    .GroupBy(x => x.Record.Municipality.Id)
                    .ToDictionary(x => x.Key, y => y
                        .GroupBy(x => x.RealEstateType.Id)
                        .ToDictionary(x => x.Key, z => z.AsEnumerable()));
            }

            if (_roTypesCache == null)
            {
                _roTypesCache = _roTypeDomain.GetAll()
                    .Select(x => new {RoId = x.RealityObject.Id, RetId = x.RealEstateType.Id})
                    .ToList()
                    .GroupBy(x => x.RoId)
                    .ToDictionary(x => x.Key, y => y.Select(x => x.RetId));
            }
        }

        private Dictionary<long, IEnumerable<PaysizeRecord>> _paysizeRecCache;
        private Dictionary<long, Dictionary<long, IEnumerable<PaysizeRealEstateType>>> _paysizeRetCache;
        private Dictionary<long, IEnumerable<long>> _roTypesCache;
    }
}