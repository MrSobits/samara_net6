namespace Bars.Gkh.Overhaul.Nso.DomainService.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.Gkh.Config;
    using Bars.Gkh.Domain;
    using Bars.Gkh.DomainService.Dict.RealEstateType;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Entities.Dicts;
    using Bars.Gkh.Entities.RealEstateType;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Overhaul.CommonParams;
    using Bars.Gkh.Overhaul.Domain;
    using Bars.Gkh.Overhaul.DomainService;
    using Bars.Gkh.Overhaul.Entities;
    using Bars.Gkh.Utils;

    using Castle.Windsor;

    /// <summary>
    /// Сервис для рабоыт с типами домов
    /// </summary>
    public class NsoRealEstateTypeService : INsoRealEstateTypeService, IRealEstateTypeService
    {
        private static readonly object SyncRoot = new object();

        /// <summary>
        /// Настройки
        /// </summary>
        public IGkhParams GkhParams { get; set; }
        
        /// <summary>
        /// Контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }

        private MoLevel RealEstTypeMoLevel { get; set; }

        /// <summary>
        /// Сервис домов в ДПКР
        /// </summary>
        public IDpkrRealityObjectService DpkrRoService { get; set; }

        /// <summary>
        /// Репозиторий Тип дома
        /// </summary>
        public IRepository<RealEstateTypeRealityObject> RoTypeRepo { get; set; }

        /// <summary>
        /// Репозиторий Тип дома
        /// </summary>
        public IRepository<RealEstateType> RealEstateTypeRepo { get; set; }

        /// <summary>
        /// Сервис тарифов дома
        /// </summary>
        public IRealityObjectTariffService RealityObjectTariffService { get; set; }

        /// <summary>
        /// Вернуть список муниципалитетов
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns>Результат</returns>
        public IDataResult GetMuList(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();

            var appParams = this.GkhParams.GetParams();

            this.RealEstTypeMoLevel = MoLevel.MunicipalUnion;

            if (appParams.ContainsKey("RealEstTypeMoLevel"))
            {
                this.RealEstTypeMoLevel = (MoLevel) appParams["RealEstTypeMoLevel"].ToInt();
            }

            var data = this.Container.Resolve<IRepository<Municipality>>().GetAll()
                .OrderBy(x => x.Name)
                .Select(
                    x => new
                    {
                        x.Id,
                        x.Level,
                        x.Name
                    })
                .Filter(loadParams, this.Container)
                .AsEnumerable()
                .Where(x => x.Level.ToMoLevel(this.Container) == this.RealEstTypeMoLevel);

            return new ListDataResult(data.ToList(), data.Count());
        }

        /// <summary>
        /// Вернуть типы домов
        /// </summary>
        /// <param name="roQuery">Запрос жилых домов</param>
        /// <returns>Результат</returns>
        public Dictionary<long, List<long>> GetRealEstateTypes(IQueryable<RealityObject> roQuery)
        {
            var appParams = this.GkhParams.GetParams();

            this.RealEstTypeMoLevel = MoLevel.MunicipalUnion;

            if (appParams.ContainsKey("RealEstTypeMoLevel"))
            {
                this.RealEstTypeMoLevel = (MoLevel) appParams["RealEstTypeMoLevel"].ToInt();
            }

            var roList = this.GetRoInfo(roQuery).Values.ToList();
            var typeStructElems = this.GetTypeInfo();
            var dictCommonParams = this.Container.ResolveAll<ICommonParam>()
                .ToDictionary(x => x.Code);

            var roTypeDict = new Dictionary<long, List<long>>();

            //для каждого дома получаем тип, которому он соответствует
            foreach (var ro in roList)
            {
                var reIds = this.GetTypeId(ro, typeStructElems, dictCommonParams);

                roTypeDict.Add(ro.Id, reIds);
            }

            return roTypeDict;
        }

        /// <summary>
        /// Обновить типы домов
        /// </summary>
        public IDataResult UpdateRobjectTypes()
        {
            if (!Monitor.TryEnter(SyncRoot))
            {
                return new BaseDataResult(false, "Действие в данный момент выполняется");
            }

            try
            {
                var roRepo = this.Container.ResolveRepository<RealityObject>();
                var dpkrRoService = this.Container.Resolve<IDpkrRealityObjectService>();
                var roTypeRepo = this.Container.ResolveRepository<RealEstateTypeRealityObject>();
                var retRepo = this.Container.ResolveRepository<RealEstateType>();

                var existRoTypes = roTypeRepo.GetAll()
                    .Select(
                        x => new
                        {
                            x.Id,
                            RetId = x.RealEstateType.Id,
                            RoId = x.RealityObject.Id
                        })
                    .AsEnumerable()
                    .GroupBy(x => x.RoId)
                    .ToDictionary(x => x.Key, y => y.Select(x => new Tuple<long, long>(x.RetId, x.Id)).ToList());

                var roQuery = dpkrRoService.GetObjectsInDpkr();

                var newRoTypes = this.GetRealEstateTypes(roQuery);

                var listToCreate = new List<RealEstateTypeRealityObject>();

                var listToDelete = new List<long>();

                foreach (var roId in roQuery.Select(x => x.Id))
                {
                    var existTypes = existRoTypes.Get(roId) ?? new List<Tuple<long, long>>();
                    var newTypes = newRoTypes.Get(roId) ?? new List<long>();

                    var ro = roRepo.Load(roId);

                    newTypes
                        .Where(x => !existTypes.Select(y => y.Item1).Contains(x))
                        .Where(x => x > 0)
                        .ForEach(
                            x => listToCreate.Add(
                                new RealEstateTypeRealityObject
                                {
                                    RealityObject = ro,
                                    RealEstateType = retRepo.Load(x)
                                }));

                    existTypes
                        .Where(x => !newTypes.Contains(x.Item1))
                        .ForEach(x => listToDelete.Add(x.Item2));
                }

                this.Container.InTransaction(
                    () =>
                    {
                        listToCreate.ForEach(roTypeRepo.Save);
                        listToDelete.ForEach(x => roTypeRepo.Delete(x));
                    });

                return new BaseDataResult();
            }
            finally
            {
                Monitor.Exit(SyncRoot);
            }
            
        }

        /// <summary>
        /// Предпросмотр результатов обновления типов домов
        /// </summary>
        /// <returns></returns>
        public IDataResult UpdateRobjectTypesPreview(BaseParams baseParams)
        {
            var roQuery = this.DpkrRoService.GetObjectsInDpkr();

            var existRoTypes = this.RoTypeRepo.GetAll()
                .Select(
                    x => new
                    {
                        x.RealEstateType.Name,
                        RoId = x.RealityObject.Id,
                    })
                .AsEnumerable()
                .GroupBy(x => x.RoId)
                .ToDictionary(x => x.Key, y => y.Select(x => x.Name).AggregateWithSeparator(", "));

            var newTypes = this.GetRealEstateTypes(roQuery);

            var typeDict =
                this.RealEstateTypeRepo.GetAll().
                    Select(
                        x => new
                        {
                            x.Id,
                            x.Name
                        })
                    .ToDictionary(x => x.Id, y => y.Name);

            var newTypesNames = newTypes
                .Where(x => !x.Value.Contains(0))
                .Select(
                    x => new
                    {
                        x.Key,
                        Values = x.Value.Select(y => typeDict.Get(y))
                    })
                .ToDictionary(x => x.Key, x => string.Join(", ", x.Values));

            var existRates = this.RealityObjectTariffService.FillRealityObjectTariffInfoDictionary(roQuery);
            var newRates = this.RealityObjectTariffService.FillRealityObjectTariffInfoDictionary(roQuery, newTypes);

            var result = roQuery
                .AsEnumerable()
                .Select(
                    x => new RealEstateTypePreviewProxy
                    {
                        Address = x.Address,
                        ExistEstateTypes = existRoTypes.Get(x.Id) ?? "Отсутствует",
                        NewEstateTypes = newTypesNames.Get(x.Id) ?? "Отсутствует",
                        ExistRate = existRates.Get(x.Id)?.Tarif,
                        NewRate = newRates.Get(x.Id)?.Tarif
                    })
                .Where(x => x.NewEstateTypes != x.ExistEstateTypes)
                .ToList();

            var totalCount = result.Count;

            return new ListDataResult(result, totalCount);
        }

        private Dictionary<long, RoProxy> GetRoInfo(IQueryable<RealityObject> roQuery)
        {
            var roDict = roQuery
                .Select(
                    x => new RoProxy
                    {
                        Id = x.Id,
                        MuId = x.Municipality.Id,
                        SettlementId = (long?) x.MoSettlement.Id,
                        AreaLiving = x.AreaLiving,
                        AreaLivingNotLivingMkd = x.AreaLivingNotLivingMkd,
                        AreaMkd = x.AreaMkd,
                        AreaOwned = x.AreaOwned,
                        BuildYear = x.BuildYear,
                        PrivatizationDateFirstApartment = x.PrivatizationDateFirstApartment,
                        MaximumFloors = x.MaximumFloors,
                        Floors = x.Floors,
                        NumberLifts = x.NumberLifts,
                        NumberEntrances = x.NumberEntrances,
                        PhysicalWear = x.PhysicalWear
                    })
                .AsEnumerable()
                .Distinct(x => x.Id)
                .ToDictionary(x => x.Id);

            var roStrElemsDict = this.Container.Resolve<IDomainService<RealityObjectStructuralElement>>().GetAll()
                .Where(x => roQuery.Any(y => y.Id == x.RealityObject.Id))
                .Where(x => x.State.StartState)
                .Select(
                    x => new
                    {
                        RoId = x.RealityObject.Id,
                        SeId = x.StructuralElement.Id
                    })
                .AsEnumerable()
                .Distinct();

            foreach (var roStrElem in roStrElemsDict)
            {
                if (roDict.ContainsKey(roStrElem.RoId))
                {
                    roDict[roStrElem.RoId].StructElems.Add(roStrElem.SeId);
                }
            }

            return roDict;
        }

        private List<RealEstateTypeProxy> GetTypeInfo()
        {
            var result = this.Container.ResolveRepository<RealEstateType>().GetAll()
                .ToDictionary(
                    x => x.Id,
                    y => new RealEstateTypeProxy
                    {
                        Id = y.Id,
                        Params = new List<RealEstateParam>(),
                        StructElems = new List<StructElemExist>(),
                        MuIds = new List<long>()
                    });

            //получаем какие конструктивные элементы должны быть/не быть у дома для соответствия типу
            var typeStructElems = this.Container.ResolveRepository<RealEstateTypeStructElement>().GetAll()
                .Select(
                    x => new
                    {
                        ReId = x.RealEstateType.Id,
                        SeId = x.StructuralElement.Id,
                        x.Exists
                    })
                .ToList();

            foreach (var typeStructElem in typeStructElems)
            {
                if (result.ContainsKey(typeStructElem.ReId))
                {
                    result[typeStructElem.ReId]
                        .StructElems
                        .Add(
                            new StructElemExist
                            {
                                SeId = typeStructElem.SeId,
                                Exist = typeStructElem.Exists
                            });
                }
            }

            //получаем какие параметры нужны для соответствия дома типу
            var typeParams = this.Container.ResolveRepository<RealEstateTypeCommonParam>().GetAll()
                .Select(
                    x => new
                    {
                        ReId = x.RealEstateType.Id,
                        Code = x.CommonParamCode,
                        x.Max,
                        x.Min
                    })
                .ToList();

            foreach (var typeParam in typeParams)
            {
                if (result.ContainsKey(typeParam.ReId))
                {
                    result[typeParam.ReId]
                        .Params
                        .Add(
                            new RealEstateParam
                            {
                                Code = typeParam.Code,
                                Min = typeParam.Min,
                                Max = typeParam.Max
                            });
                }
            }

            //получаем муниципальные образования типа
            var municipalities = this.Container.ResolveRepository<RealEstateTypeMunicipality>().GetAll()
                .Select(
                    x => new
                    {
                        ReId = x.RealEstateType.Id,
                        x.Municipality.Id,
                        x.Municipality.Level
                    })
                .AsEnumerable()
                .Where(x => x.Level.ToMoLevel(this.Container) == this.RealEstTypeMoLevel);

            foreach (var mu in municipalities)
            {
                if (result.ContainsKey(mu.ReId))
                {
                    result[mu.ReId].MuIds.Add(mu.Id);
                }
            }

            return result.Values.ToList();
        }

        private List<long> GetTypeId(RoProxy ro, IEnumerable<RealEstateTypeProxy> reTypes, Dictionary<string, ICommonParam> commonParams)
        {
            var result = new List<long>();

            foreach (var reType in reTypes)
            {
                var muId = this.RealEstTypeMoLevel == MoLevel.MunicipalUnion ? ro.MuId : ro.SettlementId.ToLong();

                if (this.TestMunicipality(muId, reType.MuIds)
                    && this.TestCommonParams(ro, commonParams, reType.Params)
                    && this.TestStructElems(ro.StructElems, reType.StructElems))
                {
                    result.Add(reType.Id);
                }
            }

            return result.Count > 0 ? result : new List<long> {0};
        }

        private bool TestStructElems(List<long> roStrElems, List<StructElemExist> typeStrElems)
        {
            if (typeStrElems.Count == 0)
            {
                return true;
            }

            if (roStrElems.Count == 0)
            {
                return !typeStrElems.Any(x => x.Exist);
            }

            return typeStrElems.Where(x => x.Exist).Select(x => x.SeId).All(roStrElems.Contains)
                && !roStrElems.Any(typeStrElems.Where(x => !x.Exist).Select(x => x.SeId).Contains);
        }

        private bool TestCommonParams(RoProxy roProxy, Dictionary<string, ICommonParam> commonParams, IEnumerable<RealEstateParam> typeParams)
        {
            foreach (var param in typeParams)
            {
                var commonParam = commonParams[param.Code];

                var ro = new RealityObject
                {
                    AreaLivingNotLivingMkd = roProxy.AreaLivingNotLivingMkd,
                    AreaMkd = roProxy.AreaMkd,
                    AreaOwned = roProxy.AreaOwned,
                    BuildYear = roProxy.BuildYear,
                    PrivatizationDateFirstApartment = roProxy.PrivatizationDateFirstApartment,
                    MaximumFloors = roProxy.MaximumFloors,
                    Floors = roProxy.Floors,
                    NumberEntrances = roProxy.NumberEntrances,
                    NumberLifts = roProxy.NumberLifts,
                    PhysicalWear = roProxy.PhysicalWear
                };

                if (!commonParam.Validate(ro, param.Min, param.Max))
                {
                    return false;
                }
            }

            return true;
        }

        private bool TestMunicipality(long muId, List<long> retMuIds)
        {
            if (retMuIds.Count == 0)
            {
                return true;
            }

            return muId > 0 && retMuIds.Contains(muId);
        }

        private class RoProxy
        {
            public RoProxy()
            {
                this.StructElems = new List<long>();
            }

            public long Id { get; set; }

            public long MuId { get; set; }

            public long? SettlementId { get; set; }

            public decimal? AreaLiving { get; set; }

            public decimal? AreaLivingNotLivingMkd { get; set; }

            public decimal? AreaMkd { get; set; }

            public decimal? AreaOwned { get; set; }

            public int? BuildYear { get; set; }

            public DateTime? PrivatizationDateFirstApartment { get; set; }

            public int? MaximumFloors { get; set; }

            public int? Floors { get; set; }

            public int? NumberLifts { get; set; }

            public int? NumberEntrances { get; set; }

            public decimal? PhysicalWear { get; set; }

            public List<long> StructElems { get; private set; }
        }

        private class RealEstateTypeProxy
        {
            public long Id { get; set; }

            public List<StructElemExist> StructElems { get; set; }

            public List<RealEstateParam> Params { get; set; }

            public List<long> MuIds { get; set; }
        }

        private class StructElemExist
        {
            public long SeId { get; set; }

            public bool Exist { get; set; }
        }

        private class RealEstateParam
        {
            public string Code { get; set; }

            public string Min { get; set; }

            public string Max { get; set; }
        }

        private class RealEstateTypePreviewProxy
        {
            public string Address { get; set; }

            public string ExistEstateTypes { get; set; }

            public string NewEstateTypes { get; set; }

            public decimal? ExistRate { get; set; }

            public decimal? NewRate { get; set; }
        }
    }
}