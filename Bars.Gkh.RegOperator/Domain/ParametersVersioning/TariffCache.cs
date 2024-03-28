namespace Bars.Gkh.RegOperator.Domain.ParametersVersioning
{
    using B4.DataAccess;
    using B4.Utils;
    using B4.Utils.Annotations;
    using Bars.Gkh.Decisions.Nso.Entities;
    using Exceptions;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Entities.RealEstateType;
    using Bars.Gkh.Overhaul.Entities;
    using NHibernate.Linq;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Кэш тарифов
    /// </summary>
    public class TariffCache : ITariffCache
    {
        private readonly IRepository<RealEstateTypeRealityObject> roTypeRepository;
        private readonly IRepository<PaysizeRealEstateType> typeRepository;
        private readonly IRepository<PaysizeRecord> paysizeRepository;
        private readonly IRepository<MonthlyFeeAmountDecision> decisionRepository;
        private readonly IRepository<Room> roomRepository;

        private bool initialized;

        private Dictionary<long, PaysizeRecord[]> paysizeRecCache;
        private Dictionary<long, Dictionary<long, PaysizeRealEstateType[]>> paysizeRetCache;
        private Dictionary<long, RealEstateTypeRealityObject[]> roTypesCache;
        private Dictionary<long, Tuple<DateTime, PeriodMonthlyFee[]>[]> decisionCache;
        private Dictionary<long, Dictionary<long, long>> entranceCache;

        /// <summary>
        /// Конструктор класса
        /// </summary>
        public TariffCache(
            IRepository<RealEstateTypeRealityObject> roTypeRepository,
            IRepository<PaysizeRealEstateType> typeRepository,
            IRepository<PaysizeRecord> paysizeRepository,
            IRepository<MonthlyFeeAmountDecision> decisionRepository,
            IRepository<Room> roomRepository)
        {
            this.roTypeRepository = roTypeRepository;
            this.typeRepository = typeRepository;
            this.paysizeRepository = paysizeRepository;
            this.decisionRepository = decisionRepository;
            this.roomRepository = roomRepository;
        }

        /// <summary>
        /// Инициализация
        /// </summary>
        /// <param name="roQuery">Запрос объектов</param>
        public void Init(IQueryable<RealityObject> roQuery)
        {
            ArgumentChecker.NotNull(roQuery, "roQuery");
            var ids = roQuery.Select(x => x.Id);
            this.Init(ids);
        }

        /// <summary>
        /// Инициализация
        /// </summary>
        /// <param name="roIds">Список идентификаторов</param>
        public void Init(IEnumerable<long> roIds)
        {
            ArgumentChecker.NotNull(roIds, "roIds");
            var internalIds = roIds.Distinct().ToArray();

            this.InitInternal(internalIds);
        }

        /// <summary>
        /// Получение размеров взносов по городским поселениям
        /// </summary>
        public PaysizeRecord[] GetSettlementPaysizes(RealityObject robject)
        {
            var stlId = robject
                .Return(x => x.MoSettlement)
                .Return(x => x.Id);

            return this.paysizeRecCache.Get(stlId) ?? new PaysizeRecord[0];
        }

        /// <summary>
        /// Получение размеров взносов по муниципальным районам
        /// </summary>
        public PaysizeRecord[] GetMunicipalityPaysizes(RealityObject robject)
        {
            var muId = robject
                .Return(x => x.Municipality)
                .Return(x => x.Id);

            return this.paysizeRecCache.Get(muId) ?? new PaysizeRecord[0];
        }

        /// <summary>
        /// Получение размеров взносов по городскому поселению, сгруппированные по типу дома
        /// </summary>
        public Dictionary<long, PaysizeRealEstateType[]> GetSettlementPaysizesByType(RealityObject robject)
        {
            var stlId = robject
                .Return(x => x.MoSettlement)
                .Return(x => x.Id);

            return this.paysizeRetCache.Get(stlId) ?? new Dictionary<long, PaysizeRealEstateType[]>();
        }

        /// <summary>
        /// Получение размеров взносов по муниципальному району, сгруппированные по типу дома
        /// </summary>
        public Dictionary<long, PaysizeRealEstateType[]> GetMunicipalityPaysizesByType(RealityObject robject)
        {
            var muId = robject
                .Return(x => x.Municipality)
                .Return(x => x.Id);

            return this.paysizeRetCache.Get(muId) ?? new Dictionary<long, PaysizeRealEstateType[]>();
        }

        /// <summary>
        /// Получение типов домов
        /// </summary>
        public RealEstateTypeRealityObject[] GetRoTypes(RealityObject robject)
        {
            var roId = robject.Return(x => x.Id);

            return this.roTypesCache.Get(roId) ?? new RealEstateTypeRealityObject[0];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Tuple<DateTime, PeriodMonthlyFee[]>[] GetDecisions(RealityObject robject)
        {
            return this.decisionCache.Get(robject.Id) ?? new Tuple<DateTime, PeriodMonthlyFee[]>[0];
        }

        public Dictionary<long, long> GetEntrances(RealityObject robject)
        {
            this.ThrowIfNotInitialized();

            return this.entranceCache.Get(robject.Id) ?? new Dictionary<long, long>();
        }

        private void InitInternal(long[] roIds)
        {
            this.paysizeRecCache = this.paysizeRepository.GetAll()
                .Where(x => x.Value.HasValue)
                .Fetch(x => x.Paysize)
                .ToList()
                .GroupBy(x => x.Municipality.Id)
                .ToDictionary(x => x.Key, y => y.ToArray());

            this.paysizeRetCache = this.typeRepository.GetAll()
                .Where(x => x.Value.HasValue)
                .Fetch(x => x.Record)
                .ThenFetch(x => x.Paysize)
                .ToList()
                .GroupBy(x => x.Record.Municipality.Id)
                .ToDictionary(x => x.Key, y => y
                    .GroupBy(x => x.RealEstateType.Id)
                    .ToDictionary(x => x.Key, z => z.ToArray()));

            this.roTypesCache = this.roTypeRepository.GetAll()
                .Where(x => roIds.Contains(x.RealityObject.Id))
                .GroupBy(x => x.RealityObject.Id)
                .ToDictionary(x => x.Key, y => y.ToArray());

            this.decisionCache = this.decisionRepository.GetAll()
                .Where(x => roIds.Contains(x.Protocol.RealityObject.Id))
                .Where(x => x.Protocol.State.FinalState)
                .Select(x => new
                {
                    RoId = x.Protocol.RealityObject.Id,
                    x.Protocol.DateStart,
                    x.Decision
                })
                .AsEnumerable()
                .GroupBy(x => x.RoId)
                .ToDictionary(x => x.Key, z => z
                    .Select(x => new Tuple<DateTime, PeriodMonthlyFee[]>(x.DateStart, x.Decision.ToArray()))
                    .ToArray());

            this.entranceCache = this.roomRepository.GetAll()
                .Where(x => roIds.Contains(x.RealityObject.Id))
                .Fetch(x => x.Entrance)
                .ThenFetch(x => x.RealEstateType)
                .Where(x => x.Entrance != null)
                .Select(x => new
                {
                    RoId = x.RealityObject.Id,
                    EntranceId = x.Entrance.Id,
                    x.Entrance
                })
                .AsEnumerable()
                .GroupBy(x => x.RoId)
                .ToDictionary(x => x.Key,
                    z => z.GroupBy(x => x.EntranceId)
                        .ToDictionary(x => x.Key, x => x.First().Entrance.RealEstateType.Id));

            this.initialized = true;
        }

        private void ThrowIfNotInitialized()
        {
            if (!this.initialized)
            {
                throw new CacheNotInitializedException("Tariff cache");
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if (this.initialized)
            {
                this.paysizeRecCache.Clear();
                this.paysizeRetCache.Clear();
                this.roTypesCache.Clear();
                this.decisionCache.Clear();

                this.initialized = false;
            }
        }
    }
}