namespace Bars.Gkh.RegOperator.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using B4.DataAccess;
    using B4.Utils;
    using Gkh.Entities;
    using Overhaul.Entities;
    using Repositories;

    /// <summary>
    /// 
    /// </summary>
    public class EntranceTariffRepository : IEntranceTariffRepository
    {
        private readonly IRepository<RealityObject> _roRepo;
        private readonly IRepository<Entrance> _entranceRepo;
        private readonly IRepository<PaysizeRealEstateType> _paysizeRetRepo;

        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="roRepo"></param>
        /// <param name="paysizeRetRepo"></param>
        /// <param name="entranceRepo"></param>
        public EntranceTariffRepository(IRepository<RealityObject> roRepo, IRepository<PaysizeRealEstateType> paysizeRetRepo, IRepository<Entrance> entranceRepo)
        {
            _roRepo = roRepo;
            _paysizeRetRepo = paysizeRetRepo;
            _entranceRepo = entranceRepo;
        }

        /// <summary>
        /// Получить тарифы для всех подъездов дома
        /// </summary>
        public Dictionary<long, decimal> GetRobjectTariff(long roId, DateTime date)
        {
            var entrances = _entranceRepo.GetAll()
                .Where(x => x.RealityObject.Id == roId)
                .ToArray();

            if (entrances.IsEmpty())
            {
                return new Dictionary<long, decimal>();
            }

            var stlAndMuId = _roRepo.GetAll()
                .Where(x => x.Id == roId)
                .Select(x => new
                {
                    MuId = x.Municipality.Id,
                    StlId = (long?)x.MoSettlement.Id
                })
                .First();

            var retIds = entrances
                .Where(x => x.RealEstateType != null)
                .Select(x => x.RealEstateType.Id)
                .ToArray();

            var stlRets = GetMunicipalityTypes(stlAndMuId.StlId.GetValueOrDefault(), date, retIds);
            var muRets = GetMunicipalityTypes(stlAndMuId.MuId, date, retIds);

            var result = new Dictionary<long, decimal>();

            foreach (var entrance in entrances)
            {
                var ret = stlRets.FirstOrDefault(x => x.RealEstateType.Id == entrance.RealEstateType.Id)
                    ?? muRets.FirstOrDefault(x => x.RealEstateType.Id == entrance.RealEstateType.Id);
                if (ret != null)
                {
                    result.Add(entrance.Id, ret.Value.GetValueOrDefault());
                }
            }

            return result;
        }

        /// <summary>
        /// Получить тариф для указанного дома и указанного типа дома
        /// </summary>
        public decimal GetRetTariff(long roId, long retId, DateTime date)
        {
            var stlAndMuId =
                _roRepo.GetAll()
                       .Where(x => x.Id == roId)
                       .Select(x => new { MuId = x.Municipality.Id, StlId = (long?)x.MoSettlement.Id })
                       .First();

            return
                (GetMunicipalityTypes(stlAndMuId.StlId.GetValueOrDefault(), date, new[] { retId }).FirstOrDefault()
                 ?? GetMunicipalityTypes(stlAndMuId.MuId, date, new[] { retId }).FirstOrDefault()).Return(
                     x => x.Value.GetValueOrDefault());
        }

        private PaysizeRealEstateType[] GetMunicipalityTypes(long muId, DateTime date, long[] retIds)
        {
            return _paysizeRetRepo.GetAll()
                .Where(x => x.Record.Paysize.DateStart <= date)
                .Where(x => !x.Record.Paysize.DateEnd.HasValue || x.Record.Paysize.DateEnd >= date)
                .Where(x => x.Record.Municipality.Id == muId)
                .Where(x => retIds.Contains(x.RealEstateType.Id))
                .Where(x => x.Value.HasValue && x.Value.Value > 0)
                .ToArray();
        }
    }
}