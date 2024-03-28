namespace Bars.Gkh.Overhaul.Tat.Import
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using B4;
    using B4.DataAccess;
   using Microsoft.Extensions.Logging;
    using B4.Modules.FIAS;
    using B4.Utils;

    using Bars.B4.IoC;

    using Castle.Windsor;
    using Enums;
    using Enums.Import;
    using Gkh.Entities;
    using Gkh.Entities.CommonEstateObject;
    using Gkh.Import;
    using Gkh.Import.Impl;

    using Microsoft.Extensions.Logging;

    using Overhaul.Entities;

    public class RealtyObjectImport : GkhImportBase
    {
        public static string Id = "NsoRealtyObjectImport";

        public virtual IWindsorContainer Container { get; set; }

        private readonly Dictionary<string, Log> logDict = new Dictionary<string, Log>();

        private readonly Dictionary<long, RealtyObjectRecord> _foundRealtyObjects = new Dictionary<long, RealtyObjectRecord>();
        
        private List<RealtyObjectRecord> _realtyObjectsToCreate = new List<RealtyObjectRecord>();
        
        private Dictionary<string, List<FiasAddressRecord>> _fiasAdressesByGuidDict;

        private readonly Dictionary<string, int> _headersDict = new Dictionary<string, int>();

        private readonly Dictionary<string, IDinamicAddress> _dynamicAddressDict = new Dictionary<string, IDinamicAddress>();

        private Dictionary<string, Municipality> _municipalitiesDict = new Dictionary<string, Municipality>();

        private Dictionary<string, string> _fiasIdByMunicipalityNameDict = new Dictionary<string, string>();

        private Dictionary<string, Dictionary<string, Dictionary<string, string>>> fiasDict;

        private Dictionary<string, Dictionary<string, long>> structuralElementDict;

        private Dictionary<long, Dictionary<long, long>> realtyObjectStructuralElementsDict;

        private Dictionary<long, string> structuralElementNameDict;

        private IFiasRepository fiasRepository;

        private IDomainService<StructuralElement> serviceStructuralElement;

        private IDomainService<RealityObjectStructuralElement> serviceRealityObjectStructuralElement;

        public override string Key
        {
            get { return Id; }
        }

        public override string CodeImport
        {
            get { return "NsoRealtyObjectImport"; }
        }

        public override string Name
        {
            get { return "Импорт жилых домов для Новосибирска"; }
        }

        public override string PossibleFileExtensions
        {
            get { return "csv"; }
        }

        public override string PermissionName
        {
            get { return "Import.TatRealtyObjectImport.View"; }
        }

        public RealtyObjectImport(ILogImportManager logImportManager, ILogImport logImport)
        {
            this.LogImportManager = logImportManager;
            this.LogImport = logImport;
        }

        public override ImportResult Import(BaseParams baseParams)
        {
            var fileData = baseParams.Files["FileImport"];
            
            fiasRepository = Container.Resolve<IFiasRepository>();
            serviceStructuralElement = Container.Resolve<IDomainService<StructuralElement>>();
            serviceRealityObjectStructuralElement = Container.Resolve<IDomainService<RealityObjectStructuralElement>>();

            InitLog(fileData.FileName);

            InitDictionaries();

            ProcessData(fileData.Data);

            var message = SaveData();

            WriteLogs();
            LogImportManager.Add(fileData, LogImport);
            LogImportManager.Save();

            message += LogImportManager.GetInfo();
            var status = LogImportManager.CountError > 0 ? StatusImport.CompletedWithError : (LogImportManager.CountWarning > 0 ? StatusImport.CompletedWithWarning : StatusImport.CompletedWithoutError);
            return new ImportResult(status, message, string.Empty, LogImportManager.LogFileId);
        }

        private void InitHeaders()
        {
            _headersDict["ExternalId"] = 0;
            _headersDict["MunicipalityName"] = 4;
            _headersDict["LocalityName"] = 5;
            _headersDict["StreetName"] = 6;
            _headersDict["House"] = 7;
            _headersDict["Housing"] = 8;
            _headersDict["TypeHouse"] = 15;
            _headersDict["BuildYear"] = 17;
            _headersDict["AreaMkd"] = 21;
            _headersDict["Floors"] = 22;
            _headersDict["MaximumFloors"] = 23;
            _headersDict["NumberEntrances"] = 24;
            _headersDict["NumberApartments"] = 25;
            _headersDict["NumberLiving"] = 26;
            _headersDict["AreaLiving"] = 28;
            _headersDict["AreaLivingOwned"] = 29;
            _headersDict["PrivatizationDateFirstApartment"] = 34;
            
            _headersDict["FacadeArea"] = 35;
            _headersDict["RoofArea"] = 36;
            _headersDict["BasementArea"] = 37;
            _headersDict["FacadeLastRepairYear"] = 55;
            _headersDict["RoofLastRepairYear"] = 56;
            _headersDict["BasementLastRepairYear"] = 57;
            _headersDict["LiftLastRepairYear"] = 58;
            _headersDict["ElectricityLastRepairYear"] = 59;
            _headersDict["HeatSystemLastRepairYear"] = 60;
            _headersDict["ColdWaterLastRepairYear"] = 61;
            _headersDict["HotWaterLastRepairYear"] = 62;
            _headersDict["SewerageLastRepairYear"] = 63;
            _headersDict["GasLastRepairYear"] = 64;

            _headersDict["NumberLifts"] = 70;
            _headersDict["ConditionHouse"] = 71;

            _headersDict["FacadeType"] = 72;
            _headersDict["RoofType"] = 73;
            _headersDict["BasementType"] = 74;
            _headersDict["HeatsystemType"] = 75;
            _headersDict["WaterSystemType"] = 76;
            _headersDict["SewerageType"] = 77;
            _headersDict["GasSystemType"] = 78;
            _headersDict["ElectricitySystemType"] = 79;
            _headersDict["LiftType"] = 80;

            _headersDict["ColdWaterPipeLength"] = 40;
            _headersDict["HotWaterPipeLength"] = 41;
        }

        private void InitDictionaries()
        {
            InitHeaders();
            // Словарь всех существующих адресов по коду КЛАДР
            _fiasAdressesByGuidDict = Container.Resolve<IDomainService<Fias>>().GetAll()
                .Join(
                    Container.Resolve<IDomainService<FiasAddress>>().GetAll(),
                    x => x.AOGuid,
                    y => y.StreetGuidId,
                    (a, b) => new { fias = a, fiasAddress = b })
                .Where(x => x.fias.ActStatus == FiasActualStatusEnum.Actual)
                .Select(x => new FiasAddressRecord
                {
                    AoGuid = x.fias.AOGuid,
                    Id = x.fiasAddress.Id,
                    House = x.fiasAddress.House,
                    Housing = x.fiasAddress.Housing
                })
                .AsEnumerable()
                .GroupBy(x => x.AoGuid)
                .ToDictionary(x => x.Key, x => x.ToList());

            var fiasService = Container.Resolve<IDomainService<Fias>>().GetAll();

            // Словарь ФИАС
            fiasDict = fiasService
                .Join(
                    fiasService,
                    x => x.AOGuid,
                    x => x.ParentGuid,
                    (a, b) => new { parent = a, child = b })
                .Where(x => x.parent.ActStatus == FiasActualStatusEnum.Actual)
                .Where(x => x.child.ActStatus == FiasActualStatusEnum.Actual)
                .Where(x => x.child.AOLevel == FiasLevelEnum.Street)
                .Select(x => new
                    {
                        municipalityGuid = x.parent.ParentGuid,
                        parentName = x.parent.OffName,
                        parentShortName = x.parent.ShortName,
                        childGuid = x.child.AOGuid,
                        childName = x.child.OffName,
                        childShortName = x.child.ShortName
                    })
                .AsEnumerable()
                .GroupBy(x => x.parentName.ToUpper() + " " + x.parentShortName.ToUpper())
                .ToDictionary(
                    x => x.Key,
                    x => x.GroupBy(y => y.municipalityGuid)
                        .ToDictionary(
                            y => y.Key,
                            y => y.GroupBy(z => z.childName.ToUpper() + " " + z.childShortName.ToUpper())
                                    .ToDictionary(
                                        z => z.Key, 
                                        z => z.First().childGuid)));

            _municipalitiesDict = Container.Resolve<IDomainService<Municipality>>().GetAll()
                .AsEnumerable()
                .GroupBy(x => x.FiasId)
                .ToDictionary(x => x.Key, x => x.First());

            this._fiasIdByMunicipalityNameDict = Container.Resolve<IDomainService<Municipality>>().GetAll()
                .Select(x => new { x.Name, x.FiasId })
                .AsEnumerable()
                .GroupBy(x => x.Name.ToUpper())
                .ToDictionary(x => x.Key, x => x.First().FiasId);
            
            var structElemList = serviceStructuralElement.GetAll()
                .Select(x => new
                    {
                        seCode = x.Code,
                        seId = x.Id,
                        seName = x.Name,
                        coeCode = x.Group.CommonEstateObject.Code
                    })
                .ToArray();

            // Словарь конструктивных характеристик over ООИ
            structuralElementDict = structElemList
                .GroupBy(x => x.coeCode)
                .ToDictionary(
                    x => x.Key,
                    x => x.GroupBy(z => z.seCode).ToDictionary(z => z.Key, z => z.First().seId));


            // Словарь наименований конструктивных характеристик
            structuralElementNameDict = structElemList.ToDictionary(x => x.seId, x => x.seName);

            // Словарь конструктивных характеристик жилых домов
            realtyObjectStructuralElementsDict = serviceRealityObjectStructuralElement.GetAll()
                .Select(x => new
                    {
                        roId = x.RealityObject.Id, 
                        seId = x.StructuralElement.Id, 
                        roStrElemId = x.Id
                    })
                .AsEnumerable()
                .GroupBy(x => x.roId)
                .ToDictionary(
                    x => x.Key, 
                    x => x.GroupBy(y => y.seId)
                          .ToDictionary(
                            y => y.Key, 
                            y => y.First().roStrElemId));
        }

        /// <summary>
        /// Обработка файла импорта
        /// </summary>
        private void ProcessData(byte[] fileData)
        {
            var servRealityObject = Container.Resolve<IDomainService<RealityObject>>();
            var realtyObjects = servRealityObject.GetAll()
                .Select(x => new { x.Id, FiasAddressId = x.FiasAddress.Id })
                .AsEnumerable()
                .GroupBy(x => x.FiasAddressId)
                .ToDictionary(x => x.Key, x => x.Select(y => y.Id).ToList());

            using (var memoryStreamFile = new MemoryStream(fileData))
            {
                memoryStreamFile.Seek(0, SeekOrigin.Begin);
                var reader = new StreamReader(memoryStreamFile, Encoding.GetEncoding(1251));

                string line;

                while ((line = reader.ReadLine()) != null)
                {
                    var record = ProcessLine(line);
                    if (!record.isValidRecord)
                    {
                        continue;
                    }
                   
                    if (_fiasAdressesByGuidDict.ContainsKey(record.StreetAoGuid))
                    {
                        var result = _fiasAdressesByGuidDict[record.StreetAoGuid];

                        if (!string.IsNullOrEmpty(record.House) && !string.IsNullOrEmpty(record.Housing))
                        {
                            result = result.Where(x => x.House == record.House && x.Housing == record.Housing).ToList();
                        }
                        else if (!string.IsNullOrEmpty(record.House) && string.IsNullOrEmpty(record.Housing))
                        {
                            result = result.Where(x => x.House == record.House && string.IsNullOrEmpty(x.Housing)).ToList();
                        }

                        if (result.Count == 0)
                        {
                            // адреса дома не найден => соответственно не может быть и дома

                            record.FiasAddress = CreateAddressByStreetAoGuid(record.StreetAoGuid, record.House, record.Housing);

                            _realtyObjectsToCreate.Add(record);
                        }
                        else
                        {
                            var realtyObjectIdsByAddress = result.Where(x => realtyObjects.ContainsKey(x.Id)).SelectMany(x => realtyObjects[x.Id]).ToList();

                            if (realtyObjectIdsByAddress.Count == 0)
                            {
                                // дом не найден в системе
                                record.FiasAddressId = result.First().Id;
                                _realtyObjectsToCreate.Add(record);
                                continue;
                            }

                            if (realtyObjectIdsByAddress.Count > 1)
                            {
                                AddLog(record.ExternalId, "В системе найдено несколько домов, соответсвующих записи.", RealityObjectImportResultType.No);
                                continue;
                            }

                            var realtyObjectId = realtyObjectIdsByAddress.First();

                            // Если в файле есть дублирующиеся дома, загружать последний номер.
                            _foundRealtyObjects[realtyObjectId] = record;
                        }
                    }
                    else
                    {
                        record.FiasAddress = CreateAddressByStreetAoGuid(record.StreetAoGuid, record.House, record.Housing);

                        _realtyObjectsToCreate.Add(record);
                    }
                }
            }
        }

        /// <summary>
        /// Создание FiasAddress на основе aoGuid улицы, номера и корпуса дома
        /// Важно! Корректно работает только для уровня улиц
        /// </summary>
        private FiasAddress CreateAddressByStreetAoGuid(string aoGuid, string house, string housing)
        {
            IDinamicAddress dynamicAddress;
            if (_dynamicAddressDict.ContainsKey(aoGuid))
            {
                dynamicAddress = _dynamicAddressDict[aoGuid];
            }
            else
            {
                dynamicAddress = fiasRepository.GetDinamicAddress(aoGuid);
                _dynamicAddressDict[aoGuid] = dynamicAddress;
            }

            var addressName = new StringBuilder(dynamicAddress.AddressName);

            if (!string.IsNullOrEmpty(house))
            {
                addressName.Append(", д. ");
                addressName.Append(house);

                if (!string.IsNullOrEmpty(housing))
                {
                    addressName.Append(", корп. ");
                    addressName.Append(housing);
                }
            }

            var fiasAddress = new FiasAddress
            {
                AddressGuid = dynamicAddress.AddressGuid,
                AddressName = addressName.ToString(),
                PostCode = dynamicAddress.PostCode,
                StreetGuidId = dynamicAddress.GuidId,
                StreetName = dynamicAddress.Name,
                StreetCode = dynamicAddress.Code,
                House = house,
                Housing = housing,

                // Поля ниже коррекны только если входной параметр aoGuid улицы
                PlaceAddressName = dynamicAddress.AddressName.Replace(dynamicAddress.Name, string.Empty).Trim(' ').Trim(','),
                PlaceGuidId = dynamicAddress.ParentGuidId,
                PlaceName = dynamicAddress.ParentName
            };

            return fiasAddress;
        }

        /// <summary>
        /// Обработка строки импорта
        /// </summary>
        private RealtyObjectRecord ProcessLine(string line)
        {
            var record = new RealtyObjectRecord { isValidRecord = false };
            var data = line.ToStr().Split(';').Select(x => x.Trim('"')).ToArray();
            if (data.Length <= 1)
            {
                return record;
            }

            record.ExternalId = GetValue(data, "ExternalId");
            record.LocalityName = GetValue(data, "LocalityName");
            record.StreetName = GetValue(data, "StreetName");
            record.House = GetValue(data, "House").Trim();
            
            if (string.IsNullOrEmpty(record.LocalityName))
            {
                AddLog(record.ExternalId, "Не задан населеный пункт.", RealityObjectImportResultType.DoesNotMeetFias);
                return record;
            }

            if (string.IsNullOrEmpty(record.StreetName))
            {
                AddLog(record.ExternalId, "Не задана улица.", RealityObjectImportResultType.DoesNotMeetFias);
                return record;
            }

            if (string.IsNullOrEmpty(record.House))
            {
                AddLog(record.ExternalId, "Не задан номер дома.", RealityObjectImportResultType.No);
                return record;
            }

            if (!fiasDict.ContainsKey(record.LocalityName.ToUpper()))
            {
                AddLog(record.ExternalId, "Не удалось найти соответсвующий населенный пункт в системе.", RealityObjectImportResultType.DoesNotMeetFias);
                return record;
            }

            var localityFiasDict = fiasDict[record.LocalityName.ToUpper()];

            var localityStreets = localityFiasDict.First().Value;
            
            if (localityFiasDict.Count > 1)
            {
                var municipalityName = GetValue(data, "MunicipalityName").Trim().ToUpper();

                if (_fiasIdByMunicipalityNameDict.ContainsKey(municipalityName))
                {
                    var municipalityGuid = _fiasIdByMunicipalityNameDict[municipalityName];
                    var localities = localityFiasDict.Where(x => x.Key == municipalityGuid).Select(x => x.Value).ToList();

                    if (localities.Count == 1)
                    {
                        localityStreets = localities.First();
                    }
                    else if (localities.Count < 1)
                    {
                        AddLog(record.ExternalId, "В системе найдено несколько населенных пунктов, соответсвующих записи. "
                                                  + "Но в указанном муниципальном образовании данный населенный пункт не найден", RealityObjectImportResultType.No);
                        return record;
                    }
                    else if (localities.Count > 1)
                    {
                        AddLog(record.ExternalId, "В системе найдено несколько населенных пунктов, соответсвующих записи с заданным муниципальным образованием.", RealityObjectImportResultType.No);
                        return record;
                    }
                }
                else
                {
                    AddLog(record.ExternalId, "В системе найдено несколько населенных пунктов, соответсвующих записи. "
                                              + "Но в справочнике муниципальных образований не найдена соответствующая запись.", RealityObjectImportResultType.No);
                    return record;
                }
            }

            if (!localityStreets.ContainsKey(record.StreetName.ToUpper()))
            {
                AddLog(record.ExternalId, "Не удалось найти соответсвующую улицу населенного пункта в системе.", RealityObjectImportResultType.DoesNotMeetFias);
                return record;
            }

            record.StreetAoGuid = localityStreets[record.StreetName.ToUpper()];  
            
            record.Housing = GetValue(data, "Housing");
            record.TypeHouse = GetValue(data, "TypeHouse");
            record.BuildYear = GetValue(data, "BuildYear");
            record.AreaMkd = GetValue(data, "AreaMkd");
            record.Floors = GetValue(data, "Floors");
            record.MaximumFloors = GetValue(data, "MaximumFloors");
            record.NumberEntrances = GetValue(data, "NumberEntrances");
            record.NumberApartments = GetValue(data, "NumberApartments");
            record.NumberLiving = GetValue(data, "NumberLiving");
            record.AreaLiving = GetValue(data, "AreaLiving");
            record.AreaLivingOwned = GetValue(data, "AreaLivingOwned");
            record.PrivatizationDateFirstApartment = GetValue(data, "PrivatizationDateFirstApartment");
            record.FacadeArea = GetValue(data, "FacadeArea");
            record.RoofArea = GetValue(data, "RoofArea");
            record.BasementArea = GetValue(data, "BasementArea");
            record.FacadeLastRepairYear = GetValue(data, "FacadeLastRepairYear");
            record.RoofLastRepairYear = GetValue(data, "RoofLastRepairYear");
            record.BasementLastRepairYear = GetValue(data, "BasementLastRepairYear");
            record.LiftLastRepairYear = GetValue(data, "LiftLastRepairYear");
            record.ElectricityLastRepairYear = GetValue(data, "ElectricityLastRepairYear");
            record.HeatSystemLastRepairYear = GetValue(data, "HeatSystemLastRepairYear");
            record.ColdWaterLastRepairYear = GetValue(data, "ColdWaterLastRepairYear");
            record.HotWaterLastRepairYear = GetValue(data, "HotWaterLastRepairYear");
            record.SewerageLastRepairYear = GetValue(data, "SewerageLastRepairYear");
            record.GasLastRepairYear = GetValue(data, "GasLastRepairYear");
            record.NumberLifts = GetValue(data, "NumberLifts");
            record.ConditionHouse = GetValue(data, "ConditionHouse");
            record.FacadeType = GetValue(data, "FacadeType");
            record.RoofType = GetValue(data, "RoofType");
            record.BasementType = GetValue(data, "BasementType");
            record.HeatsystemType = GetValue(data, "HeatsystemType");
            record.WaterSystemType = GetValue(data, "WaterSystemType");
            record.SewerageType = GetValue(data, "SewerageType");
            record.GasSystemType = GetValue(data, "GasSystemType");
            record.ElectricitySystemType = GetValue(data, "ElectricitySystemType");
            record.LiftType = GetValue(data, "LiftType");
            record.ColdWaterPipeLength = GetValue(data, "ColdWaterPipeLength");
            record.HotWaterPipeLength = GetValue(data, "HotWaterPipeLength");

            record.isValidRecord = true;

            return record;
        }

        private string GetValue(string[] data, string field)
        {
            var result = string.Empty;

            if (_headersDict.ContainsKey(field))
            {
                var index = _headersDict[field];
                if (data.Length > index)
                {
                    result = data[index];
                }
            }

            return result;
        }

        /// <summary>
        /// Метод переноса свойств из записи файла, в RealityObject
        /// </summary>
        private void Apply(RealityObject realityObject, RealtyObjectRecord record)
        {
            Action<string> warnLog = text => AddLog(record.ExternalId, text, RealityObjectImportResultType.YesButWithErrors); 

            if (string.IsNullOrEmpty(realityObject.ExternalId))
            {
                realityObject.ExternalId = record.ExternalId;
            }

            if (!string.IsNullOrEmpty(record.TypeHouse))
            {
                switch (record.TypeHouse.ToUpper())
                {
                    case "БЛОКИРОВАННОЙ ЗАСТРОЙКИ": 
                        realityObject.TypeHouse = TypeHouse.BlockedBuilding;
                        break;

                    case "ИНДИВИДУАЛЬНЫЙ":
                        realityObject.TypeHouse = TypeHouse.Individual;
                        break;

                    case "МНОГОКВАРТИРНЫЙ":
                        realityObject.TypeHouse = TypeHouse.ManyApartments;
                        break;

                    case "ОБЩЕЖИТИЕ":
                        realityObject.TypeHouse = TypeHouse.SocialBehavior;
                        break;

                    default:
                        warnLog("Тип дома");
                        break;
                }
            }
            else
            {
                warnLog("Тип дома");
            }

            if (!string.IsNullOrEmpty(record.ConditionHouse))
            {
                switch (record.ConditionHouse.ToUpper())
                {
                    case "ИСПРАВНЫЙ":
                        realityObject.ConditionHouse = ConditionHouse.Serviceable;
                        break;

                    case "ВЕТХИЙ":
                        realityObject.ConditionHouse = ConditionHouse.Dilapidated;
                        break;

                    case "АВАРИЙНЫЙ":
                        realityObject.ConditionHouse = ConditionHouse.Emergency;
                        break;

                    case "СНЕСЕН":
                        realityObject.ConditionHouse = ConditionHouse.Razed;
                        break;

                    default:
                        warnLog("Состояние дома");
                        break;
                }
            }
            else
            {
                warnLog("Состояние дома");
            }

            if (!string.IsNullOrEmpty(record.BuildYear))
            {
                realityObject.BuildYear = record.BuildYear.ToInt();
            }
            else
            {
                warnLog("Год постройки");
            }

            if (!string.IsNullOrEmpty(record.AreaMkd))
            {
                realityObject.AreaMkd = record.AreaMkd.ToDecimal();
            }
            else
            {
                warnLog("Общая площадь");
            }

            if (!string.IsNullOrEmpty(record.Floors))
            {
                realityObject.Floors = record.Floors.ToInt();
            }
            else
            {
                warnLog("Минимальная этажность");
            }

            if (!string.IsNullOrEmpty(record.MaximumFloors))
            {
                realityObject.MaximumFloors = record.MaximumFloors.ToInt();
            }
            else
            {
                warnLog("Максимальная этажность");
            }

            if (!string.IsNullOrEmpty(record.NumberEntrances))
            {
                realityObject.NumberEntrances = record.NumberEntrances.ToInt();
            }
            else
            {
                warnLog("Количество подъездов");
            }

            if (!string.IsNullOrEmpty(record.NumberApartments))
            {
                realityObject.NumberApartments = record.NumberApartments.ToInt();
            }
            else
            {
                warnLog("Количество квартир");
            }

            if (!string.IsNullOrEmpty(record.NumberLiving))
            {
                realityObject.NumberLiving = record.NumberLiving.ToInt();
            }
            else
            {
                warnLog("Количество проживающих");
            }

            if (!string.IsNullOrEmpty(record.AreaLiving))
            {
                realityObject.AreaLiving = record.AreaLiving.ToDecimal();
            }
            else
            {
                warnLog("Жилая площадь");
            }

            if (!string.IsNullOrEmpty(record.AreaLivingOwned))
            {
                realityObject.AreaLivingOwned = record.AreaLivingOwned.ToDecimal();
            }
            else
            {
                warnLog("Жилая площадь в собственности граждан");
            }

            if (!string.IsNullOrEmpty(record.PrivatizationDateFirstApartment))
            {
                if (record.PrivatizationDateFirstApartment.Trim().Length == 4)
                {
                    var value = record.PrivatizationDateFirstApartment.ToInt();
                    if (value > 0)
                    {
                        realityObject.PrivatizationDateFirstApartment = new DateTime(value, 1, 1);
                    }
                    else
                    {
                        warnLog("Дата приватизации первого жилого помещения");
                    }
                }
                else
                {
                    realityObject.PrivatizationDateFirstApartment = record.PrivatizationDateFirstApartment.ToDateTime();
                }
            }
            else
            {
                warnLog("Дата приватизации первого жилого помещения");
            }

            if (!string.IsNullOrEmpty(record.NumberLifts))
            {
                realityObject.NumberLifts = record.NumberLifts.ToInt();
            }
            else
            {
                warnLog("Количество лифтов");
            }
        }

        private Municipality GetMunicipality(FiasAddress address)
        {
            if (address == null || string.IsNullOrEmpty(address.AddressGuid))
            {
                return null;
            }

            var guidMass = address.AddressGuid.Split('#');

            Municipality result = null;

            foreach (var s in guidMass)
            {
                var t = s.Split('_');

                Guid g;

                Guid.TryParse(t[1], out g);

                if (g != Guid.Empty)
                {
                    var guid = g.ToString();
                    if (_municipalitiesDict.ContainsKey(guid))
                    {
                        result = _municipalitiesDict[guid];
                    }
                }
            }

            return result;
        }

        private string GetAddressForMunicipality(Municipality mo, FiasAddress address)
        {
            if (address == null || mo == null)
            {
                return string.Empty;
            }

            var result = address.AddressName ?? string.Empty;

            if (string.IsNullOrEmpty(result) && string.IsNullOrEmpty(mo.FiasId))
            {
                return string.Empty;
            }

            IDinamicAddress dinamicAddress;

            if (_dynamicAddressDict.ContainsKey(mo.FiasId))
            {
                dinamicAddress = _dynamicAddressDict[mo.FiasId];
            }
            else
            {
                dinamicAddress = fiasRepository.GetDinamicAddress(mo.FiasId);
                _dynamicAddressDict[mo.FiasId] = dinamicAddress;
            }

            if (dinamicAddress == null)
            {
                return string.Empty;
            }

            if (result.StartsWith(dinamicAddress.AddressName))
            {
                result = result.Replace(dinamicAddress.AddressName, string.Empty).Trim();
            }

            if (result.StartsWith(","))
            {
                result = result.Substring(1).Trim();
            }

            return result;
        }

        private bool UpdateRobjStructElem(string coeCode, string seCode, RealityObject realityObject,
            Dictionary<long, long> roStructuralElementsDict, bool realtyObjectIsNew, string lastOverhaulYear = null, string volume = null)
        {
            if (!structuralElementDict.ContainsKey(coeCode) || string.IsNullOrEmpty(seCode))
            {
                return false;
            }

            var coeStructuralElements = structuralElementDict[coeCode];
            
            if (!coeStructuralElements.ContainsKey(seCode))
            {
                return false;
            }

            var structuralElementId = coeStructuralElements[seCode];

            RealityObjectStructuralElement realityObjectStructuralElement;

            if (!realtyObjectIsNew && roStructuralElementsDict.ContainsKey(structuralElementId))
            {
                realityObjectStructuralElement = serviceRealityObjectStructuralElement.Load(roStructuralElementsDict[structuralElementId]);
            }
            else
            {
                realityObjectStructuralElement = new RealityObjectStructuralElement
                    {
                        RealityObject = realityObject,
                        StructuralElement = serviceStructuralElement.Load(structuralElementId),
                        Name = structuralElementNameDict[structuralElementId]
                    };
            }

            if (!string.IsNullOrEmpty(volume))
            {
                realityObjectStructuralElement.Volume = volume.ToDecimal();
            }

            if (!string.IsNullOrEmpty(lastOverhaulYear))
            {
                var value = lastOverhaulYear.ToInt();

                if (value > 0)
                {
                    realityObjectStructuralElement.LastOverhaulYear = value;
                }
            }
            else if (realityObject.BuildYear.HasValue)
            {
                realityObjectStructuralElement.LastOverhaulYear = realityObject.BuildYear.Value;
            }

            serviceRealityObjectStructuralElement.Save(realityObjectStructuralElement);

            return true;
        }

        private void SaveConstructiveElements(RealityObject realityObject, RealtyObjectRecord record, bool realtyObjectIsNew = true)
        {
            var roStructuralElementsDict = realtyObjectStructuralElementsDict.ContainsKey(realityObject.Id)
                       ? realtyObjectStructuralElementsDict[realityObject.Id]
                       : new Dictionary<long, long>();

            #region Фасад

            if (!string.IsNullOrEmpty(record.FacadeType))
            {
                var seFacadeType = string.Empty;
                switch (record.FacadeType.Trim())
                {
                    case "1": seFacadeType = "1"; break;
                    case "2": seFacadeType = "2"; break;
                    case "3": seFacadeType = "3"; break;
                    case "4": seFacadeType = "3/1"; break;
                    case "5": seFacadeType = "4"; break;
                    case "6": seFacadeType = "5"; break;
                    case "7": seFacadeType = "6"; break;
                    case "8": seFacadeType = "7"; break;
                    case "9": seFacadeType = "8"; break;
                    case "10": seFacadeType = "9"; break;
                    case "11": seFacadeType = "10"; break;
                    case "12": seFacadeType = "11"; break;
                }

                if (!UpdateRobjStructElem("3", seFacadeType, realityObject, roStructuralElementsDict, realtyObjectIsNew, record.FacadeLastRepairYear, record.FacadeArea))
                { 
                    AddLog(record.ExternalId, "Фасад", RealityObjectImportResultType.YesButWithErrors);
                }
            }

            #endregion Фасад

            #region Крыша

            if (!string.IsNullOrEmpty(record.RoofType))
            {
                var seRoofType = string.Empty;
                switch (record.RoofType.Trim())
                {
                    case "1": seRoofType = "1"; break;
                    case "2": seRoofType = "1/1"; break;
                    case "3": seRoofType = "2"; break;
                    case "4": seRoofType = "3"; break;
                    case "5": seRoofType = "3/1"; break;
                    case "6": seRoofType = "3/2"; break;
                    case "7": seRoofType = "4"; break;
                    case "8": seRoofType = "5"; break;
                    case "9": seRoofType = "6"; break;
                    case "10": seRoofType = "7"; break;
                    case "11": seRoofType = "71"; break;
                }

                if (!UpdateRobjStructElem("2", seRoofType, realityObject, roStructuralElementsDict, realtyObjectIsNew, record.RoofLastRepairYear, record.RoofArea))
                {
                    AddLog(record.ExternalId, "Крыша", RealityObjectImportResultType.YesButWithErrors);
                }
            }

            #endregion Крыша

            #region Подвал

            if (!string.IsNullOrEmpty(record.BasementType))
            {
                if (!UpdateRobjStructElem("1", record.BasementType, realityObject, roStructuralElementsDict, realtyObjectIsNew, record.BasementLastRepairYear, record.BasementArea))
                { 
                    AddLog(record.ExternalId, "Подвал", RealityObjectImportResultType.YesButWithErrors);
                }
            }

            #endregion Подвал

            #region Система отопления

            if (!string.IsNullOrEmpty(record.HeatsystemType))
            {
                if (!UpdateRobjStructElem("4", record.HeatsystemType, realityObject, roStructuralElementsDict, realtyObjectIsNew, record.HeatSystemLastRepairYear, record.AreaMkd))
                {
                    AddLog(record.ExternalId, "Инженерная система отопления", RealityObjectImportResultType.YesButWithErrors);
                }
            }

            #endregion Система отопления

            #region Системы холодного и горячего водоснабжения

            var waterSystemLastRepairYear = string.Empty;

            if (!string.IsNullOrEmpty(record.HotWaterLastRepairYear))
            {
                waterSystemLastRepairYear = record.HotWaterLastRepairYear;
            }

            if (!string.IsNullOrEmpty(record.ColdWaterLastRepairYear))
            {
                waterSystemLastRepairYear = record.ColdWaterLastRepairYear;
            }

            if (!string.IsNullOrEmpty(record.WaterSystemType))
            {
                if (!UpdateRobjStructElem("5", record.WaterSystemType, realityObject, roStructuralElementsDict, realtyObjectIsNew, waterSystemLastRepairYear, record.AreaMkd))
                {
                    AddLog(record.ExternalId, "Инженерная система горячего и холодного водоснабжения", RealityObjectImportResultType.YesButWithErrors);
                }
            }

            #endregion Системы холодного и горячего водоснабжения

            #region Система водотведения

            if (!string.IsNullOrEmpty(record.SewerageType))
            {
                if (!UpdateRobjStructElem("6", record.SewerageType, realityObject, roStructuralElementsDict, realtyObjectIsNew, record.SewerageLastRepairYear, record.AreaMkd))
                {
                    AddLog(record.ExternalId, "Инженерная система канализования и водоотведения", RealityObjectImportResultType.YesButWithErrors);
                }
            }

            #endregion Система водотведения

            #region Система газоснабжения

            if (!string.IsNullOrEmpty(record.GasSystemType))
            {
                if (!UpdateRobjStructElem("7", record.GasSystemType, realityObject, roStructuralElementsDict, realtyObjectIsNew, record.GasLastRepairYear, record.AreaMkd))
                {
                    AddLog(record.ExternalId, "Система газоснабжения", RealityObjectImportResultType.YesButWithErrors);
                }
            }

            #endregion Система газоснабжения

            #region Система электроснабжения

            if (!string.IsNullOrEmpty(record.ElectricitySystemType))
            {
                if (!UpdateRobjStructElem("8", record.ElectricitySystemType, realityObject, roStructuralElementsDict, realtyObjectIsNew, record.ElectricityLastRepairYear, record.AreaMkd))
                {
                    AddLog(record.ExternalId, "Система электроснабжения", RealityObjectImportResultType.YesButWithErrors);
                }
            }

            #endregion Система электроснабжения

            #region Система лифтового оборудования

            if (!string.IsNullOrEmpty(record.LiftType))
            {
                if (!UpdateRobjStructElem("9", record.LiftType, realityObject, roStructuralElementsDict, realtyObjectIsNew, record.LiftLastRepairYear, record.NumberLifts))
                {
                    AddLog(record.ExternalId, "Лифтовое оборудование", RealityObjectImportResultType.YesButWithErrors);
                }
            }

            #endregion Система лифтового оборудования
        }

        private string SaveData()
        {
            var message = string.Empty;

            var repRealityObject = Container.Resolve<IRepository<RealityObject>>();
            var servFiasAddress = Container.Resolve<IDomainService<FiasAddress>>();

            LogImport.CountAddedRows = 0;
            LogImport.CountChangedRows = 0;

            using (var transaction = Container.Resolve<IDataTransaction>())
            {
                try
                {
                    foreach (var foundRealtyObject in _foundRealtyObjects.Where(x => x.Value != null))
                    {
                        var realityObj = repRealityObject.Load(foundRealtyObject.Key);
                        Apply(realityObj, foundRealtyObject.Value);
                        repRealityObject.Update(realityObj);
                        SaveConstructiveElements(realityObj, foundRealtyObject.Value, false);
                        AddLog(foundRealtyObject.Value.ExternalId, string.Empty, RealityObjectImportResultType.AlreadyExists);
                    }

                    if (_realtyObjectsToCreate.Any())
                    {
                        // Если в файле есть дублирующиеся дома, загружать последний.
                        _realtyObjectsToCreate = _realtyObjectsToCreate
                            .GroupBy(x => new { x.StreetAoGuid, x.House, x.Housing })
                            .Select(x => x.Select(y => y).Last())
                            .ToList();

                        foreach (var realtyObjectToCreate in _realtyObjectsToCreate.Where(x => x != null))
                        {
                            var objectToCreate = realtyObjectToCreate;

                            if (objectToCreate.FiasAddressId == null && objectToCreate.FiasAddress == null)
                            {
                                AddLog(
                                    objectToCreate.ExternalId,
                                    "Объект не создан. В ФИАС не удалось найти адреc, соответствующий записи", 
                                    RealityObjectImportResultType.DoesNotMeetFias);
                                continue;
                            }

                            FiasAddress fiasAddress;
                            if (objectToCreate.FiasAddressId.HasValue)
                            {
                                fiasAddress = servFiasAddress.Load(objectToCreate.FiasAddressId.Value);
                            }
                            else
                            {
                                servFiasAddress.Save(objectToCreate.FiasAddress);
                                fiasAddress = objectToCreate.FiasAddress;
                            }

                            var municipality = GetMunicipality(fiasAddress);

                            if (municipality == null)
                            {
                                AddLog(objectToCreate.ExternalId, "Не удалось определить муниципальное образование", RealityObjectImportResultType.No);
                                continue;
                            }

                            var realityObj = new RealityObject
                            {
                                FiasAddress = fiasAddress,
                                Municipality = municipality,
                                Address = GetAddressForMunicipality(municipality, fiasAddress)
                            };

                            Apply(realityObj, objectToCreate);
                            repRealityObject.Save(realityObj);
                            SaveConstructiveElements(realityObj, objectToCreate);
                            AddLog(objectToCreate.ExternalId, string.Empty, RealityObjectImportResultType.Yes);
                        }
                    }

                    transaction.Commit();
                }
                catch (Exception exc)
                {
                    try
                    {
                        LogImport.IsImported = false;
                        Container.Resolve<ILogger>().LogError(exc, "Импорт");
                        message = "Произошла неизвестная ошибка. Обратитесь к администратору";
                        LogImport.Error(Name, "Произошла неизвестная ошибка. Обратитесь к администратору.");

                        transaction.Rollback();
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message, exc);
                    }
                }
            }

            return message;
        }

        public override bool Validate(BaseParams baseParams, out string message)
        {
            message = null;

            if (!baseParams.Files.ContainsKey("FileImport"))
            {
                message = "Не выбран файл для импорта";
                return false;
            }

            var extention = baseParams.Files["FileImport"].Extention;

            var fileExtentions = PossibleFileExtensions.Contains(",") ? PossibleFileExtensions.Split(',') : new[] { PossibleFileExtensions };
            if (fileExtentions.All(x => x != extention))
            {
                message = string.Format("Необходимо выбрать файл с допустимым расширением: {0}", PossibleFileExtensions);
                return false;
            }

            return true;
        }

        public void InitLog(string fileName)
        {
            if (this.LogImportManager == null)
            {
                throw new Exception("Не найдена реализация интерфейса ILogImportManager");
            }

            this.LogImportManager.FileNameWithoutExtention = fileName;
            this.LogImportManager.UploadDate = DateTime.Now;

            if (this.LogImport == null)
            {
                throw new Exception("Не найдена реализация интерфейса ILogImport");
            }

            this.LogImport.SetFileName(fileName);
            this.LogImport.ImportKey = Key;
        }

        private void AddLog(string id, string text, RealityObjectImportResultType type = RealityObjectImportResultType.None)
        {
            var log = new Log();
            if (logDict.ContainsKey(id))
            {
                log = logDict[id];
            }
            else
            {
                logDict[id] = log;
            }


            if (type == RealityObjectImportResultType.No || type == RealityObjectImportResultType.DoesNotMeetFias)
            {
                log.Text = text;
            }
            else
            {
                log.Text = log.Text + ", " + text;
            }

            if (log.ResultType < type)
            {
                log.ResultType = type;
            }
        }

        private void WriteLogs()
        {
            foreach (var log in logDict)
            {
                var logValue = log.Value;

                var text = string.IsNullOrEmpty(log.Value.Text)
                               ? string.Empty
                               : "Место ошибки: " + log.Value.Text.Trim(' ').Trim(',');
                
                switch (logValue.ResultType)
                {
                    case RealityObjectImportResultType.AlreadyExists:
                        LogImport.CountChangedRows++;
                        break;

                    case RealityObjectImportResultType.DoesNotMeetFias:
                        LogImport.CountError++;
                        break;

                    case RealityObjectImportResultType.No:
                        LogImport.CountError++;
                        break;

                    case RealityObjectImportResultType.Yes:
                        LogImport.CountAddedRows++;
                        break;

                    case RealityObjectImportResultType.YesButWithErrors:
                        LogImport.CountAddedRows++;
                        LogImport.CountWarning++;
                        break;
                }

                var resultText = string.Format(
                    "ID: {0}; Дом добавлен: {1} ",
                    log.Key,
                    logValue.ResultType.GetEnumMeta().Display);

                LogImport.Info(resultText, text);
            }
        }
    }
}