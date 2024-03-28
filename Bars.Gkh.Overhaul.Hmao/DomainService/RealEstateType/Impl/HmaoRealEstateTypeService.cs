namespace Bars.Gkh.Overhaul.Hmao.DomainService.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Utils;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Config;
    using Bars.Gkh.ConfigSections.Overhaul.Enums;
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
    using Bars.Gkh.Overhaul.Hmao.ConfigSections;
    using Bars.Gkh.Utils;

    using Castle.Windsor;

    using Ionic.Zip;
    using Ionic.Zlib;

    /// <summary>
    /// Сервис для рабоыт с типами домов
    /// </summary>
    public class HmaoRealEstateTypeService : IHmaoRealEstateTypeService, IRealEstateTypeService
    {
        private static readonly object SyncRoot = new object();

        private Dictionary<long, RealEstateTypeProxy> realEstateTypeProxyDict;

        private Dictionary<string, ICommonParam> _dictCommonParams;

        private DynamicDictionary _appParams;

        private Dictionary<long, List<long>> _roSeDict;

        private List<RealEstateTypeStructElement> _typeStructElems;

        private List<RealEstateTypeCommonParam> _typeParams;

        private List<Tuple<long, long>> _municipalities;

        /// <summary>
        /// Настройки
        /// </summary>
        public IGkhParams GkhParams { get; set; }

        /// <summary>
        /// Контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Домен-сервис Конструктивный элемент дома
        /// </summary>
        public IDomainService<RealityObjectStructuralElement> RoSeDomain { get; set; }

        /// <summary>
        /// Репозиторий Конструктивный элемент типа дома
        /// </summary>
        public IRepository<RealEstateTypeStructElement> RealEstateTypeStructElementRepo { get; set; }

        /// <summary>
        /// Репозиторий Тип дома
        /// </summary>
        public IRepository<RealEstateType> RealEstateTypeRepo { get; set; }

        /// <summary>
        /// Репозиторий Общий параметр типа жилих домов
        /// </summary>
        public IRepository<RealEstateTypeCommonParam> RealEstateTypeCommonParamRepo { get; set; }

        /// <summary>
        /// Репозиторий Муниципальное образование типа дома
        /// </summary>
        public IRepository<RealEstateTypeMunicipality> RealEstateTypeMunicipalityDomain { get; set; }

        private MoLevel realEstTypeMoLevel { get; set; }

        /// <summary>
        /// Репозиторий Жилой дом
        /// </summary>
        public IRepository<RealityObject> roRepo { get; set; }

        /// <summary>
        /// Сервис домов в ДПКР
        /// </summary>
        public IDpkrRealityObjectService dpkrRoService { get; set; }

        /// <summary>
        /// Репозиторий Тип дома
        /// </summary>
        public IRepository<RealEstateTypeRealityObject> roTypeRepo { get; set; }

        /// <summary>
        /// Сервис тарифов дома
        /// </summary>
        public IRealityObjectTariffService RealityObjectTariffService { get; set; }

        /// <summary>
        /// Пользователь
        /// </summary>
        public IGkhUserManager UserManager { get; set; }

        /// <summary>
        /// Представлющий методы для работы с файлами
        /// </summary>
        public IFileManager FileManager { get; set; }

        /// <summary>
        /// Лог
        /// </summary>
        public IRepository<LogOperation> LogOperationRepo { get; set; }
        private Dictionary<long, RealEstateTypeProxy> RealEstateTypeProxyDict
        {
            get
            {
                return this.realEstateTypeProxyDict ?? (this.realEstateTypeProxyDict = this.RealEstateTypeRepo.GetAll()
                    .ToDictionary(
                        x => x.Id,
                        y => new RealEstateTypeProxy
                        {
                            Id = y.Id,
                            Params = new List<RealEstateParam>(),
                            StructElems = new List<StructElemExist>(),
                            MuIds = new List<long>()
                        }));
            }
        }

        /// <summary>
        /// Общие параметры
        /// </summary>
        public Dictionary<string, ICommonParam> dictCommonParams
        {
            get
            {
                return this._dictCommonParams ??
                    (this._dictCommonParams = this.Container.ResolveAll<ICommonParam>().ToDictionary(x => x.Code));
            }
        }

        /// <summary>
        /// Параметры
        /// </summary>
        public DynamicDictionary appParams
        {
            get { return this._appParams ?? (this._appParams = this.GkhParams.GetParams()); }
        }

        /// <summary>
        /// Конструктивные элементы дома
        /// </summary>
        public Dictionary<long, List<long>> RoSeDict
        {
            get
            {
                return this._roSeDict ?? (this._roSeDict = this.RoSeDomain
                    .GetAll()
                    .Where(x => x.State.StartState)
                    .Select(
                        x => new
                        {
                            RoId = x.RealityObject.Id,
                            SeId = x.StructuralElement.Id
                        })
                    .AsEnumerable()
                    .GroupBy(x => x.RoId)
                    .ToDictionary(x => x.Key, y => y.Select(z => z.SeId).ToList()));
            }
        }

        public List<RealEstateTypeStructElement> typeStructElems
        {
            get { return this._typeStructElems ?? (this._typeStructElems = this.RealEstateTypeStructElementRepo.GetAll().ToList()); }
        }

        public List<RealEstateTypeCommonParam> typeParams
        {
            get { return (this._typeParams ?? (this._typeParams = this.RealEstateTypeCommonParamRepo.GetAll().ToList())); }
        }

        public List<Tuple<long, long>> municipalities
        {
            get
            {
                return this._municipalities ?? (this._municipalities = this.RealEstateTypeMunicipalityDomain.GetAll()
                    .Select(
                        x => new
                        {
                            ReId = x.RealEstateType.Id,
                            x.Municipality.Id,
                            x.Municipality.Level
                        })
                    .AsEnumerable()
                    .Where(x => x.Level.ToMoLevel(this.Container) == this.realEstTypeMoLevel)
                    .Select(x => new Tuple<long, long>(x.ReId, x.Id)).ToList());
            }
        }

        /// <summary>
        /// Вернуть собираемость
        /// </summary>
        /// <param name="roQuery">Запрос жилых домов</param>
        /// <param name="year">Год</param>
        /// <returns>Результат</returns>
        public IDictionary<long, decimal> GetCollectionByYear(IQueryable<RealityObject> roQuery, int year)
        {
            var config = this.Container.GetGkhConfig<OverhaulHmaoConfig>();
            var rateCalcArea = config.RateCalcTypeArea;
            var typeRateDomain = this.Container.Resolve<IDomainService<RealEstateTypeRate>>();

            var roRealEstTypesDict = this.GetRealEstateTypes(roQuery);

            var typeRateByRealEstType = typeRateDomain.GetAll()
                .Where(x => x.SociallyAcceptableRate.HasValue)
                .Select(x => new { Id = (long?)x.RealEstateType.Id, x.Year, x.SociallyAcceptableRate })
                .ToList();

            return roQuery
                .Select(
                    x => new
                    {
                        x.Id,
                        x.AreaLiving,
                        x.AreaMkd,
                        x.AreaLivingNotLivingMkd
                    })
                .AsEnumerable()
                .Select(
                    x =>
                    {
                        var roTarifs = roRealEstTypesDict.ContainsKey(x.Id)
                            ? typeRateByRealEstType.Where(
                                y => roRealEstTypesDict[x.Id].Contains(y.Id.ToInt()))
                            : typeRateByRealEstType.Where(y => !y.Id.HasValue);

                        var tarif =
                            roTarifs.Where(y => y.Year <= year)
                                .OrderByDescending(y => y.Year)
                                .ThenByDescending(y => y.SociallyAcceptableRate)
                                .FirstOrDefault();
                        if (tarif != null)
                        {
                            switch (rateCalcArea)
                            {
                                case RateCalcTypeArea.AreaLiving:
                                    {
                                        return new { x.Id, Collection = tarif.SociallyAcceptableRate.ToDecimal() * x.AreaLiving.ToDecimal() };
                                    }

                                case RateCalcTypeArea.AreaLivingNotLiving:
                                    {
                                        return new { x.Id, Collection = tarif.SociallyAcceptableRate.ToDecimal() * x.AreaLivingNotLivingMkd.ToDecimal() };
                                    }
                                case RateCalcTypeArea.AreaMkd:
                                    {
                                        return new { x.Id, Collection = tarif.SociallyAcceptableRate.ToDecimal() * x.AreaMkd.ToDecimal() };
                                    }
                            }
                        }

                        return new { x.Id, Collection = 0M };
                    })
                .ToDictionary(x => x.Id, y => y.Collection.RoundDecimal(2));
        }

        /// <summary>
        /// Вернуть собираемость 
        /// </summary>
        /// <param name="roQuery">Запрос жилых домов</param>
        /// <param name="startYear">Начальный год</param>
        /// <param name="endYear">Конечный год</param>
        /// <returns>Результат</returns>
        public IDictionary<long, decimal> GetCollectionByPeriod(IQueryable<RealityObject> roQuery, int startYear, int endYear)
        {
            var config = this.Container.GetGkhConfig<OverhaulHmaoConfig>();
            var rateCalcArea = config.RateCalcTypeArea;
            var typeRateDomain = this.Container.Resolve<IDomainService<RealEstateTypeRate>>();

            var roRealEstTypesDict = this.GetRealEstateTypes(roQuery);

            var typeRateByRealEstType = typeRateDomain.GetAll()
                .Where(x => x.SociallyAcceptableRate.HasValue)
                .Select(x => new { Id = (long?)x.RealEstateType.Id, x.Year, x.SociallyAcceptableRate })
                .ToList();

            return roQuery
                .Select(
                    x => new
                    {
                        x.Id,
                        x.AreaLiving,
                        x.AreaMkd,
                        x.AreaLivingNotLivingMkd
                    })
                .AsEnumerable()
                .Select(
                    x =>
                    {
                        var roTarifs = roRealEstTypesDict.ContainsKey(x.Id)
                            ? typeRateByRealEstType.Where(
                                y => roRealEstTypesDict[x.Id].Contains(y.Id.ToInt()))
                            : typeRateByRealEstType.Where(y => !y.Id.HasValue);

                        var tarifs = new List<decimal>();

                        for (int currYear = startYear; currYear < endYear; currYear++)
                        {
                            tarifs.Add(
                                roTarifs.Where(y => y.Year <= currYear)
                                    .OrderByDescending(y => y.Year)
                                    .ThenByDescending(y => y.SociallyAcceptableRate.ToDecimal())
                                    .Select(y => y.SociallyAcceptableRate.ToDecimal())
                                    .FirstOrDefault());
                        }

                        switch (rateCalcArea)
                        {
                            case RateCalcTypeArea.AreaLiving:
                                {
                                    return new { x.Id, Collection = tarifs.Sum(y => y * x.AreaLiving.ToDecimal() * 12) };
                                }

                            case RateCalcTypeArea.AreaLivingNotLiving:
                                {
                                    return new { x.Id, Collection = tarifs.Sum(y => y * x.AreaLivingNotLivingMkd.ToDecimal() * 12) };
                                }
                            case RateCalcTypeArea.AreaMkd:
                                {
                                    return new { x.Id, Collection = tarifs.Sum(y => y * x.AreaMkd.ToDecimal() * 12) };
                                }
                        }

                        return new { x.Id, Collection = 0M };
                    })
                .ToDictionary(x => x.Id, y => y.Collection.RoundDecimal(2));
        }

        /// <summary>
        /// Вернуть список муниципалитетов
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns>Результат</returns>
        public IDataResult GetMuList(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();

            var appParams = this.GkhParams.GetParams();

            this.realEstTypeMoLevel = MoLevel.MunicipalUnion;

            if (appParams.ContainsKey("RealEstTypeMoLevel"))
            {
                this.realEstTypeMoLevel = (MoLevel)appParams["RealEstTypeMoLevel"].ToInt();
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
                .Where(x => x.Level.ToMoLevel(this.Container) == this.realEstTypeMoLevel);

            return new ListDataResult(data.ToList(), data.Count());
        }

        /// <summary>
        /// Вернуть типы домов
        /// </summary>
        /// <param name="roQuery">Запрос жилых домов</param>
        /// <returns>Результат</returns>
        public Dictionary<long, List<long>> GetRealEstateTypes(IQueryable<RealityObject> roQuery)
        {
            this.realEstTypeMoLevel = MoLevel.MunicipalUnion;

            if (this.appParams.ContainsKey("RealEstTypeMoLevel"))
            {
                this.realEstTypeMoLevel = (MoLevel)this.appParams["RealEstTypeMoLevel"].ToInt();
            }

            var roList = this.GetRoInfo(roQuery).Values.ToList();
            var typeStructElems = this.GetTypeInfo();

            var roTypeDict = new Dictionary<long, List<long>>();

            //для каждого дома получаем тип, которому он соответствует
            foreach (var ro in roList)
            {
                var reIds = this.GetTypeId(ro, typeStructElems, this.dictCommonParams);

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
                DateTime startDate = DateTime.UtcNow; // Время начала процесса. Для логирования.

                var existRoTypes = this.roTypeRepo.GetAll()
                    .Select(
                        x => new
                        {
                            RetId = x.RealEstateType.Id,
                            RoId = x.RealityObject.Id,
                            x.Id
                        })
                    .AsEnumerable()
                    .GroupBy(x => x.RoId)
                    .ToDictionary(x => x.Key, y => y.Select(x => new Tuple<long, long>(x.RetId, x.Id)).ToList());

                // Справочник названий типов домов. Для логирования.
                Dictionary<long, string> realEstateTypes = this.RealEstateTypeRepo.GetAll().ToDictionary(x => x.Id, y => y.Name);

                var roQuery = this.dpkrRoService.GetObjectsInDpkr();

                var newRoTypes = this.GetRealEstateTypes(roQuery);

                StringBuilder logBuilder = new StringBuilder();

                var listToCreate = new List<UpdateRobjectTypesItem>();

                var listToDelete = new List<long>();

                // Тарифы. Для логирования.
                var existRates = this.RealityObjectTariffService.FillRealityObjectTariffInfoDictionary(roQuery);
                var newRates = this.RealityObjectTariffService.FillRealityObjectTariffInfoDictionary(roQuery, newRoTypes);

                foreach (var roId in roQuery.Select(x => x.Id))
                {
                    var existTypes = existRoTypes.Get(roId) ?? new List<Tuple<long, long>>();
                    var newTypes = newRoTypes.Get(roId) ?? new List<long>();

                    var ro = this.roRepo.Load(roId);
                    
                    newTypes
                        .Where(x => !existTypes.Select(y => y.Item1).Contains(x))
                        .Where(x => x > 0)
                        .ForEach(
                            x =>
                            {                                
                                // Предыдущие типы дома. Через запятую, если их несколько. Для логирования.
                                string existEstTypesCommaSeparated = existTypes.Distinct().Select(y => realEstateTypes[y.Item1]).AggregateWithSeparator(", ");
                                existEstTypesCommaSeparated = !String.IsNullOrEmpty(existEstTypesCommaSeparated) ? existEstTypesCommaSeparated : "Отсутствует";

                                listToCreate.Add(                                    
                                    new UpdateRobjectTypesItem
                                    {                                        
                                        SaveRecord = new RealEstateTypeRealityObject
                                        {
                                            RealityObject = ro,
                                            RealEstateType = this.RealEstateTypeRepo.Load(x)
                                        },
                                        LogRecord = new RealEstateTypePreviewProxy
                                        {
                                            Address = ro.Address,
                                            ExistEstateTypes = existEstTypesCommaSeparated,
                                            NewEstateTypes = realEstateTypes[x],
                                            ExistRate = existRates[ro.Id]?.Tarif,
                                            NewRate = newRates[ro.Id]?.Tarif
                                        }
                                    }
                                );
                            });

                    existTypes
                        .Where(x => !newTypes.Contains(x.Item1))
                        .ForEach(x => listToDelete.Add(x.Item2));
                }

                this.Container.InTransaction(
                    () =>
                    {
                        listToCreate.ForEach(x => 
                            {
                                this.roTypeRepo.Save(x.SaveRecord);
                                logBuilder.AppendLine($"{x.LogRecord.Address};{x.LogRecord.ExistEstateTypes};{x.LogRecord.NewEstateTypes};{x.LogRecord.ExistRate};{x.LogRecord.NewRate}");
                            });
                        listToDelete.ForEach(x => this.roTypeRepo.Delete(x));
                    });

                // Записать в лог
                var logOperation = new LogOperation
                {
                    StartDate = startDate,
                    Comment = "Изменение классификации домов",
                    OperationType = Enums.LogOperationType.UpdateRoTypes,
                    EndDate = DateTime.UtcNow,
                    User = this.UserManager.GetActiveUser()
                };
                var logsZip = new ZipFile(Encoding.UTF8)
                {
                    CompressionLevel = CompressionLevel.Level3,
                    AlternateEncoding = Encoding.GetEncoding("cp866")
                };
                using (var logFile = new System.IO.MemoryStream())
                {
                    var log = Encoding.GetEncoding(1251).GetBytes(logBuilder.ToString());
                    logsZip.AddEntry($"{logOperation.OperationType.GetEnumMeta().Display}.csv", log);
                    logsZip.Save(logFile);
                    var logFileInfo = this.FileManager.SaveFile(logFile, $"{logOperation.OperationType.GetEnumMeta().Display}.zip");
                    logOperation.LogFile = logFileInfo;
                }
                this.LogOperationRepo.Save(logOperation);

                // Вернуть результат
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
        /// <returns>Результат</returns>
        public IDataResult UpdateRobjectTypesPreview(BaseParams baseParams)
        {
            var roQuery = this.dpkrRoService.GetObjectsInDpkr();

            var existRoTypes = this.roTypeRepo.GetAll()
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
                        SettlementId = (long?)x.MoSettlement.Id,
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

            var roIds = roQuery.Select(r => r.Id).ToArray();

            var roStrElemsDict = this.Container.Resolve<IDomainService<RealityObjectStructuralElement>>().GetAll()
                .Where(x => roIds.Contains(x.RealityObject.Id))
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
            var result = new Dictionary<long, RealEstateTypeProxy>(this.RealEstateTypeProxyDict);

            foreach (var typeStructElem in
                this.typeStructElems.Where(typeStructElem => result.ContainsKey(typeStructElem.RealEstateType.Id)))
            {
                result[typeStructElem.RealEstateType.Id]
                    .StructElems
                    .Add(
                        new StructElemExist
                        {
                            SeId = typeStructElem.StructuralElement.Id,
                            Exist = typeStructElem.Exists
                        });
            }

            foreach (var typeParam in this.typeParams.Where(typeParam => result.ContainsKey(typeParam.RealEstateType.Id)))
            {
                result[typeParam.RealEstateType.Id]
                    .Params
                    .Add(
                        new RealEstateParam
                        {
                            Code = typeParam.CommonParamCode,
                            Min = typeParam.Min,
                            Max = typeParam.Max
                        });
            }

            foreach (var mu in this.municipalities.Where(mu => result.ContainsKey(mu.Item1)))
            {
                result[mu.Item1].MuIds.Add(mu.Item2);
            }

            return result.Values.ToList();
        }

        private List<long> GetTypeId(RoProxy ro, IEnumerable<RealEstateTypeProxy> reTypes, Dictionary<string, ICommonParam> commonParams)
        {
            var result = new List<long>();

            foreach (var reType in reTypes)
            {
                var muId = this.realEstTypeMoLevel == MoLevel.MunicipalUnion ? ro.MuId : ro.SettlementId.ToLong();

                if (this.TestCommonParams(ro, commonParams, reType.Params) && this.TestStructElems(ro.StructElems, reType.StructElems)
                    && this.TestMunicipality(muId, reType.MuIds))
                {
                    result.Add(reType.Id);
                }
            }

            return result.Count > 0 ? result : new List<long> { 0 };
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

        /// <summary>
        /// Элементы списка обновляемых типов домов (listToCreate в методе UpdateRobjectTypes)
        /// </summary>
        private class UpdateRobjectTypesItem
        {
            /// <summary> Для сохранения </summary>
            public RealEstateTypeRealityObject SaveRecord { get; set; }

            /// <summary> Для логирования </summary>
            public RealEstateTypePreviewProxy LogRecord { get; set; }
        }
    }
}