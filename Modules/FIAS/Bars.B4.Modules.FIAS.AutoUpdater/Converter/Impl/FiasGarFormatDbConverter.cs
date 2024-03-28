namespace Bars.B4.Modules.FIAS.AutoUpdater.Converter.Impl
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;
    using System.Xml.Serialization;

    using Bars.B4.Config;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Logging;
    using Bars.B4.Modules.FIAS.AutoUpdater.ArchiveReader;
    using Bars.B4.Modules.FIAS.AutoUpdater.Enums;
    using Bars.B4.Modules.FIAS.AutoUpdater.GAR;
    using Bars.B4.Modules.FIAS.AutoUpdater.Utils;
    using Bars.B4.Utils;
    using Bars.Gkh.Utils;

    using Castle.Windsor;

    /// <summary>
    /// Сервис конвертации данных ФИАС из *.xml файлов
    /// </summary>
    public class FiasGarFormatDbConverter : IFiasDbConverter
    {
        private readonly object lockObject = new object();

        private List<IList> entityCollectionList;
        
        /// <summary>
        /// IoC контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }
        
        /// <summary>
        /// Менеджер логов
        /// </summary>
        public ILogManager LogManager { get; set; }

        /// <inheritdoc />
        public IEnumerable<Fias> GetFiasRecords(IFiasArchiveReader reader, bool isDelta)
        {
            GetEntityCollectionList(reader.FiasLinkedFilesDict);
            return CombineEntitiesToFiasRecord(isDelta);
        }

        /// <inheritdoc />
        public IEnumerable<FiasHouse> GetFiasHouseRecords(IFiasArchiveReader reader, bool isDelta)
        {
            GetEntityCollectionList(reader.FiasHouseLinkedFilesDict);
            return CombineEntitiesToFiasHouseRecord(isDelta);
        }

        /// <summary>
        /// Скомбинировать сущности из файлов в запись дома ФИАС
        /// </summary>
        /// <param name="isDelta">Флаг инкрементального обновления</param>
        /// <returns></returns>
        private IEnumerable<FiasHouse> CombineEntitiesToFiasHouseRecord(bool isDelta)
        {
            List<FiasHouse> result = new List<FiasHouse>();
            Dictionary<Guid, Guid> addressObjectParentGuids = null;
            var systemAddressObjectsDict = new Dictionary<long, Guid>();
        
            LogManager.Info("Обновление ФИАС: начат процесс конвертации домов");

            var houseObjects = entityCollectionList
                .OfType<List<FiasHouseObject>>()
                .FirstOrDefault()
                ?.GroupBy(x => x.OBJECTID)
                ?.Select(x =>
                {
                    var obj = x.FirstOrDefault(y => y.ISACTUAL == HOUSESHOUSEISACTUAL.Actual);

                    return obj ?? x.First(y => y.ENDDATE == x.Max(z => z.ENDDATE));
                }).ToList();

            if (houseObjects == null || houseObjects.Count == 0)
            {
                LogManager.Info($"Обновление ФИАС: обновляемых домов не обнаружено");
                return result;
            }

            var totalCount = houseObjects.Count;
            LogManager.Info($"Обновление ФИАС: общее кол-во домов: {totalCount}");

            var addressObjectDict = entityCollectionList
                .OfType<List<AddressObject>>()
                .FirstOrDefault()
                ?.GroupBy(x => x.OBJECTID)
                ?.Select(x =>
                {
                    var obj = x.FirstOrDefault(y => y.ISACTUAL == ADDRESSOBJECTSOBJECTISACTUAL.Actual);

                    return obj ?? x.First(y => y.ENDDATE == x.Max(z => z.ENDDATE));
                })
                ?.ToDictionary(x => x.OBJECTID, x => x.OBJECTGUID);

            var fiasHouseParamDict = entityCollectionList
                .OfType<List<FiasHouseParam>>()
                .FirstOrDefault()
                ?.GroupBy(x => x.OBJECTID)
                ?.Select(x => new
                {
                    x.Key,
                    Params = x.GroupBy(param => param.TYPEID)
                        .Select(parameters => parameters.First(y => y.ENDDATE == parameters.Max(z => z.ENDDATE)))
                })
                ?.ToDictionary(x => x.Key,
                    x => x.Params.ToDictionary(y => y.TYPEID.ToLong(), y => y.VALUE));

            var munHierarchyDict = entityCollectionList
                .OfType<List<MunicipalityHierarchy>>()
                .FirstOrDefault()
                ?.GroupBy(x => x.OBJECTID)
                ?.Select(x =>
                {
                    var obj = x.FirstOrDefault(y => y.ISACTIVE == HIERARCHYISACTIVE.Active);

                    return obj ?? x.First(y => y.ENDDATE == x.Max(z => z.ENDDATE));
                })
                ?.ToDictionary(x => x.OBJECTID, x => x.PARENTOBJID);
            
            if (isDelta)
            {
                var fiasHouseDomain = this.Container.ResolveDomain<FiasHouse>();
                var fiasDomaim = this.Container.ResolveDomain<Fias>();
                
                using (this.Container.Using(fiasHouseDomain, fiasDomaim))
                {
                    var addressGuids = houseObjects.Select(y => y.OBJECTGUID).ToList();
                    
                    addressObjectParentGuids = fiasHouseDomain.GetAll()
                        .Where(x => x.ActualStatus == FiasActualStatusEnum.Actual && x.HouseGuid != null &&
                            addressGuids.Contains(x.HouseGuid.Value.ToString()))
                        .ToDictionary(x => x.HouseGuid.Value, x => x.AoGuid);

                    var houseWithNullAoGuids = addressObjectParentGuids
                        .Where(x => x.Value.Equals(Guid.Empty))
                        .Select(x => x.Key.ToString())
                        .ToHashSet();
                    
                    if (houseWithNullAoGuids.Any())
                    {
                        var housesIds = houseObjects
                            .Where(x => houseWithNullAoGuids.Contains(x.OBJECTGUID))
                            .Select(x => x.OBJECTID)
                            .ToHashSet();

                        var aoIds = munHierarchyDict
                            .Where(x => housesIds.Contains(x.Key))
                            .Select(x => x.Value.ToString())
                            .ToHashSet();

                        systemAddressObjectsDict = fiasDomaim.GetAll()
                            .Where(x => aoIds.Contains(x.AOId))
                            .Select(x => new { x.AOId, x.AOGuid })
                            .ToDictionary(y => Convert.ToInt64(y.AOId), y => Guid.Parse(y.AOGuid));
                    }
                }
            }

            Parallel.ForEach(houseObjects,
                houseObject =>
                {
                    FiasStructureTypeEnum structureType = 0;
                    var record = new FiasHouse();
                    var houseGuid = new Guid(houseObject.OBJECTGUID);
                    var aoId = munHierarchyDict.Get(houseObject.OBJECTID);
                    var houseObjectParams = fiasHouseParamDict.Get(houseObject.OBJECTID);

                    Enum.TryParse(houseObject.HOUSETYPE, out structureType);
                    record.TypeRecord = FiasTypeRecordEnum.Fias;
                    record.UpdateDate = houseObject.UPDATEDATE;
                    record.StartDate = houseObject.STARTDATE;
                    record.EndDate = houseObject.ENDDATE;
                    record.HouseNum = houseObject.HOUSENUM;
                    record.BuildNum = houseObject.ADDNUM1;
                    record.StrucNum = houseObject.ADDNUM2;
                    record.HouseId = houseGuid;
                    record.HouseGuid = houseGuid;
                    record.ActualStatus = (FiasActualStatusEnum) houseObject.ISACTUAL;
                    record.StructureType = structureType;
                    record.PostalCode = houseObjectParams?.Get((int) FiasParamType.PostalCode);
                    record.Okato = houseObjectParams?.Get((int) FiasParamType.OKATO);
                    record.AoGuid = !Guid.TryParse(addressObjectDict.Get(aoId), out var parentObjectGuid) 
                        && (!addressObjectParentGuids.TryGetValue(houseGuid, out parentObjectGuid) || parentObjectGuid.Equals(Guid.Empty)) 
                            ? systemAddressObjectsDict.Get(aoId) : parentObjectGuid;

                    lock (lockObject)
                    {
                        result.Add(record);

                        var percentage = (double) result.Count / totalCount * 100;
                        var nextLoopPercentage = (double) (result.Count + 1) / totalCount * 100;
                        if (percentage % 25 > nextLoopPercentage % 25)
                        {
                            LogManager.Info($"Обработано {(int) Math.Ceiling(percentage)}% адресных объектов");
                        }
                    }
                });

            return result;
        }

        /// <summary>
        /// Скомбинировать сущности из файлов в запись адресообразующего объекта ФИАС
        /// </summary>
        /// <param name="isDelta">Флаг инкрементального обновления</param>
        private IEnumerable<Fias> CombineEntitiesToFiasRecord(bool isDelta)
        {
            dynamic addressObjectParentGuids = null;
            FiasOperationStatusEnum operStatus = 0;
            var result = new List<Fias>();
            var configProvider = this.Container.Resolve<IConfigProvider>();
            var regionCode = string.Empty;

            using (this.Container.Using(configProvider))
            {
                var config = configProvider.GetConfig().GetModuleConfig("Bars.B4.Modules.FIAS.AutoUpdater");
                regionCode = config.GetAs("RegionCode", default(string), true);

                var code = 0;
                int.TryParse(regionCode, out code);
                regionCode = code.ToString();
            }
            
            LogManager.Info("Обновление ФИАС: начат процесс конвертации адресных объектов");
            
            var addressObjectDict = entityCollectionList
                .OfType<List<AddressObject>>()
                .FirstOrDefault()
                ?.GroupBy(x => x.OBJECTID)
                ?.Select(x =>
                {
                    var obj = x.FirstOrDefault(y => y.ISACTUAL == ADDRESSOBJECTSOBJECTISACTUAL.Actual);

                    return obj ?? x.First(y => y.ENDDATE == x.Max(z => z.ENDDATE));
                })
                ?.ToDictionary(x => x.OBJECTID, x => x);

            if (addressObjectDict == null || addressObjectDict.Count == 0)
            {
                LogManager.Info($"Обновление ФИАС: обновляемых адресных объектов не обнаружено");
                return result;
            }

            var totalCount = addressObjectDict.Count;
            LogManager.Info($"Обновление ФИАС: общее кол-во адресных объектов: {addressObjectDict.Count}");

            var addressObjectParamsDict = entityCollectionList
                .OfType<List<FiasAddressObjectParam>>()
                .FirstOrDefault()
                ?.GroupBy(x => x.OBJECTID)
                ?.Select(x => new
                {
                    x.Key,
                    Params = x.GroupBy(param => param.TYPEID)
                        .Select(parameters => parameters.First(y => y.ENDDATE == parameters.Max(z => z.ENDDATE)))
                })
                ?.ToDictionary(x => x.Key,
                    x => x.Params.ToDictionary(y => y.TYPEID.ToLong(), y => y.VALUE));

            var munHierarchyDict = entityCollectionList
                .OfType<List<MunicipalityHierarchy>>()
                .FirstOrDefault()
                ?.GroupBy(x => x.OBJECTID)
                ?.Select(x =>
                {
                    var obj = x.FirstOrDefault(y => y.ISACTIVE == HIERARCHYISACTIVE.Active);

                    return obj ?? x.First(y => y.ENDDATE == x.Max(z => z.ENDDATE));
                })
                ?.ToDictionary(x => x.OBJECTID,
                    x => new
                    {
                        x.OKTMO,
                        x.PARENTOBJID
                    });

            var plainCodeDict = entityCollectionList
                .OfType<List<AdministartiveHierarchy>>()
                .FirstOrDefault()
                ?.GroupBy(x => x.OBJECTID)
                ?.Select(x =>
                {
                    var obj = x.FirstOrDefault(y => y.ISACTIVE == HIERARCHYISACTIVE.Active);

                    return obj ?? x.First(y => y.ENDDATE == x.Max(z => z.ENDDATE));
                })
                ?.ToDictionary(x => x.OBJECTID, x => x.PLANCODE);

            var fiasDomainService = this.Container.ResolveDomain<Fias>();
            using (this.Container.Using(fiasDomainService))
            {
                if (isDelta)
                {
                    var addressGuids = addressObjectDict.Values.Select(y => y.OBJECTGUID).ToList();
                    addressObjectParentGuids = fiasDomainService
                        .GetAll()
                        .Where(x => x.ActStatus == FiasActualStatusEnum.Actual && x.AOGuid != null && addressGuids.Contains(x.AOGuid))
                        .ToDictionary(x => x.AOGuid,
                            x => new
                            {
                                x.ParentGuid,
                                x.OKTMO
                            });
                }
            }

            Parallel.ForEach(addressObjectDict.Values,
                addressObject =>
                {
                    var record = new Fias();
                    var munHierarchy = munHierarchyDict?.Get(addressObject.OBJECTID);
                    var addressObjectParams = addressObjectParamsDict?.Get(addressObject.OBJECTID);

                    Enum.TryParse(addressObject.OPERTYPEID, out operStatus);
                    record.TypeRecord = FiasTypeRecordEnum.Fias;
                    record.AOId = addressObject.OBJECTID.ToString();
                    record.AOGuid = addressObject.OBJECTGUID;
                    record.FormalName = addressObject.NAME;
                    record.UpdateDate = addressObject.UPDATEDATE;
                    record.ShortName = addressObject.TYPENAME;
                    record.PrevId = addressObject.PREVID.ToString();
                    record.NextId = addressObject.NEXTID.ToString();
                    record.ActStatus = (FiasActualStatusEnum) addressObject.ISACTUAL;
                    record.StartDate = addressObject.STARTDATE;
                    record.EndDate = addressObject.ENDDATE;
                    record.OperStatus = operStatus;
                    record.AOLevel = AOLevelMatchingDict.Get(addressObject.LEVEL);
                    record.CodeRegion = addressObjectParams?.Get((int) FiasParamType.RegionCode) ?? regionCode;
                    record.OffName = addressObjectParams?.Get((int) FiasParamType.OfficialName) ?? addressObject.NAME;
                    record.PostalCode = addressObjectParams?.Get((int) FiasParamType.PostalCode);
                    record.IFNSFL = addressObjectParams?.Get((int) FiasParamType.IFNSFL);
                    record.TerrIFNSFL = addressObjectParams?.Get((int) FiasParamType.TerritoryIFNSFL);
                    record.IFNSUL = addressObjectParams?.Get((int) FiasParamType.IFNSUL);
                    record.TerrIFNSUL = addressObjectParams?.Get((int) FiasParamType.TerritoryIFNSUL);
                    record.OKATO = addressObjectParams?.Get((int) FiasParamType.OKATO);
                    record.KladrCode = addressObjectParams?.Get((int) FiasParamType.AddressCode);
                    record.ParentGuid = addressObjectDict.Get(munHierarchy?.PARENTOBJID ?? 0)?.OBJECTGUID ?? 
                        (
                            addressObjectParentGuids != null && addressObjectParentGuids.ContainsKey(record.AOGuid)
                            ? addressObjectParentGuids[record.AOGuid].ParentGuid ?? string.Empty
                            : string.Empty
                        );
                    record.OKTMO = munHierarchy?.OKTMO ?? 
                    (
                        addressObjectParentGuids != null && addressObjectParentGuids.ContainsKey(record.AOGuid)
                            ? addressObjectParentGuids[record.AOGuid].OKTMO ?? string.Empty
                            : string.Empty
                    );
                    record.KladrPlainCode = plainCodeDict.Get(addressObject.OBJECTID);

                    lock (lockObject)
                    {
                        result.Add(record);

                        var percentage = (double) result.Count / totalCount * 100;
                        var nextLoopPercentage = (double) (result.Count + 1) / totalCount * 100;
                        if (percentage % 25 > nextLoopPercentage % 25)
                        {
                            LogManager.Info($"Обработано {(int) Math.Ceiling(percentage)}% адресных объектов");
                        }
                    }
                });
            
            return result;
        }

        /// <summary>
        /// Получить список коллекций сущностей для комбинирования
        /// </summary>
        /// <param name="dict">Словарь с путями до xml файлов (ключ: значение атрибута FiasEntityNameAttribute, значение: путь к XML айлу)</param>
        private void GetEntityCollectionList(IDictionary<string, string> dict)
        {
            var typeList = new List<Type>();
            if (this.entityCollectionList == null)
            {
                this.entityCollectionList = new List<IList>();
            }
            
            dict.ForEach(x =>
                {
                    var type = FiasAttribute.GetTypesWithFiasEntityNameAttribute(Assembly.GetExecutingAssembly(), x.Key);
                    var listType = typeof(List<>).MakeGenericType(type);
                    
                    typeList.Add(listType);
                    if (this.entityCollectionList.Any(y => y.GetType() == listType))
                    {
                        return;
                    }

                    var rootName = (type.GetCustomAttributes(typeof(FiasEntityNameAttribute), true).FirstOrDefault() as FiasEntityNameAttribute)?.RootName;
                    if (string.IsNullOrEmpty(rootName))
                    {
                        throw new Exception($"Не указано свойство RootName аттрибута FiasEntityName сущности {type.Name}");
                    }

                    using (var stream = new FileStream(x.Value, FileMode.Open))
                    {
                        LogManager.Info($"Обновление ФИАС: чтение XML файла: {x.Value}");
                        var serializer = new XmlSerializer(listType, new XmlRootAttribute(rootName));
                        this.entityCollectionList.Add((IList) serializer.Deserialize(stream));
                    }
                });

            this.entityCollectionList.RemoveAll(x => !typeList.Contains(x.GetType()));
        }

        /// <summary>
        /// Словарь сопоставления кодов уровня адресообразующих объектов, пришедших из файла с FiasLevelEnum
        /// </summary>
        private Dictionary<string, FiasLevelEnum> AOLevelMatchingDict = new Dictionary<string, FiasLevelEnum>
        {
            {"1", FiasLevelEnum.Region},
            {"2", FiasLevelEnum.Raion},
            {"3", FiasLevelEnum.Raion},
            {"4", FiasLevelEnum.Place},
            {"5", FiasLevelEnum.City},
            {"6", FiasLevelEnum.Place},
            {"7", FiasLevelEnum.PlanningStruct},
            {"8", FiasLevelEnum.Street},
            {"9", FiasLevelEnum.Stead},
            {"10", FiasLevelEnum.Building},
            {"11", FiasLevelEnum.Permises},
            {"12", FiasLevelEnum.Permises},
            {"13", FiasLevelEnum.AutonomusRegion},
            {"14", FiasLevelEnum.Ctar},
            {"15", FiasLevelEnum.Extr},
            {"16", FiasLevelEnum.Sext},
            {"17", FiasLevelEnum.Permises}
        };
    }
}