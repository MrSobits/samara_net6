namespace Bars.Gkh.Overhaul.Regions.Kamchatka.Import.KamchatkaRealtyObject
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.FIAS;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Entities.Dicts;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Enums.Import;
    using Bars.Gkh.Import;
    using Bars.Gkh.Overhaul.Entities;
    
    using Castle.Windsor;
    using Gkh.Entities.CommonEstateObject;
    using Gkh.Import.Impl;

    using Microsoft.Extensions.Logging;

    using NHibernate.Type;

    public class RealtyObjectImport : GkhImportBase
    {
        public static string Id = MethodBase.GetCurrentMethod().DeclaringType.FullName;

        public virtual IWindsorContainer Container { get; set; }

        public ISessionProvider SessionProvider { get; set; }

        public IFiasRepository IFiasRepository { get; set; }

        public IRepository<StructuralElement> StructuralElementRepository { get; set; }

        public IRepository<RealityObjectStructuralElement> RealityObjectStructuralElementRepository { get; set; }

        public IRepository<WallMaterial> WallMaterialRepository { get; set; }

        public IRepository<RoofingMaterial> RoofingMaterialRepository { get; set; }

        public IRepository<CapitalGroup> CapitalGroupRepository { get; set; }

        public IRepository<FiasAddress> FiasAddressRepository { get; set; }


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

        private Dictionary<string, WallMaterial> wallMaterialDict;

        private Dictionary<string, RoofingMaterial> roofingMaterialDict;

        private Dictionary<string, CapitalGroup> capitalGroupDict;

        public override string Key
        {
            get { return Id; }
        }

        public override string CodeImport
        {
            get { return "KamchatkaRealtyObjectImport"; }
        }

        public override string Name
        {
            get { return "Импорт жилых домов для Камчатки"; }
        }

        public override string PossibleFileExtensions
        {
            get { return "csv"; }
        }

        public override string PermissionName
        {
            get { return "Import.KamchatkaRealtyObjectImport.View"; }
        }

        public RealtyObjectImport(ILogImportManager logImportManager, ILogImport logImport)
        {
            this.LogImportManager = logImportManager;
            this.LogImport = logImport;
        }

        public override ImportResult Import(BaseParams baseParams)
        {
            var fileData = baseParams.Files["FileImport"];

            this.InitLog(fileData.FileName);

            this.InitDictionaries();

            this.ProcessData(fileData.Data);

            var message = this.SaveData();

            this.WriteLogs();

            // Намеренно закрываем текущую сессию, иначе при каждом коммите транзакции
            // ранее измененные дома вызывают каскадирование ФИАС
            Container.Resolve<ISessionProvider>().CloseCurrentSession();

            this.LogImportManager.Add(fileData, this.LogImport);
            this.LogImportManager.Save();

            message += this.LogImportManager.GetInfo();
            var status = this.LogImportManager.CountError > 0 ? StatusImport.CompletedWithError : (this.LogImportManager.CountWarning > 0 ? StatusImport.CompletedWithWarning : StatusImport.CompletedWithoutError);
            return new ImportResult(status, message, string.Empty, this.LogImportManager.LogFileId);
        }

        private void InitHeaders()
        {
            this._headersDict["MunicipalityName"] = 5;
            this._headersDict["LocalityName"] = 6;
            this._headersDict["StreetName"] = 7;
            this._headersDict["House"] = 8;
            this._headersDict["Housing"] = 9;
            this._headersDict["TypeHouse"] = 16;
            this._headersDict["BuildYear"] = 18;
            this._headersDict["WallMaterial"] = 19;
            this._headersDict["RoofingMaterial"] = 85;
            this._headersDict["CapitalGroup"] = 84;
            this._headersDict["AreaMkd"] = 22;
            this._headersDict["Floors"] = 23;
            this._headersDict["MaximumFloors"] = 24;
            this._headersDict["NumberEntrances"] = 25;
            this._headersDict["NumberApartments"] = 26;
            this._headersDict["NumberLiving"] = 27;
            this._headersDict["AreaLiving"] = 29;
            this._headersDict["AreaLivingOwned"] = 30;
            this._headersDict["PrivatizationDateFirstApartment"] = 35;
            
            this._headersDict["FacadeArea"] = 36;
            this._headersDict["RoofArea"] = 37;

            this._headersDict["FoundationLastRepairYear"] = 53;
            this._headersDict["FacadeLastRepairYear"] = 54;
            this._headersDict["RoofLastRepairYear"] = 55;
            this._headersDict["ElectricityLastRepairYear"] = 56;
            this._headersDict["HeatSystemLastRepairYear"] = 57;
            this._headersDict["ColdWaterLastRepairYear"] = 58;
            this._headersDict["HotWaterLastRepairYear"] = 59;
            this._headersDict["SewerageLastRepairYear"] = 60;
            
            this._headersDict["ConditionHouse"] = 66;
            this._headersDict["FacadeType"] = 67;
            this._headersDict["RoofType"] = 68;
            this._headersDict["HeatsystemType"] = 69;
            this._headersDict["SewerageType"] = 70;
            this._headersDict["ElectricitySystemType"] = 71;

            this._headersDict["FoundationArea"] = 72;
            this._headersDict["FoundationWearout"] = 73;
            this._headersDict["FoundationType"] = 74;

            this._headersDict["RoofWearout"] = 75;
            this._headersDict["FacadeWearout"] = 76;
            this._headersDict["ColdWaterSystemWearout"] = 77;
            this._headersDict["HotWaterSystemWearout"] = 78;
            
            this._headersDict["ColdWaterSystemType"] = 79;
            this._headersDict["HotWaterSystemType"] = 80;

            this._headersDict["SewerageWearout"] = 81;
            this._headersDict["HeatsystemWearout"] = 82;
            this._headersDict["ElectricityWearout"] = 83;

            this._headersDict["ElectricNetworksLength"] = 38;
            this._headersDict["HeatSystemPipeLength"] = 39;
            this._headersDict["ColdWaterPipeLength"] = 40;
            this._headersDict["HotWaterPipeLength"] = 41;
            this._headersDict["SeweragePipeLength"] = 42;

            this._headersDict["ElectroDeviceCount"] = 48;
            this._headersDict["HeatDeviceCount"] = 49;
            this._headersDict["ColdDeviceCount"] = 50;
            this._headersDict["HotDeviceCount"] = 51;
        }

        private void InitDictionaries()
        {
            this.InitHeaders();
            // Словарь всех существующих адресов по коду КЛАДР
            this._fiasAdressesByGuidDict = this.Container.Resolve<IDomainService<Fias>>().GetAll()
                .Join(
                    this.Container.Resolve<IDomainService<FiasAddress>>().GetAll(),
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
                .Where(x => x.AoGuid != null)
                .GroupBy(x => x.AoGuid)
                .ToDictionary(x => x.Key, x => x.ToList());

            var fiasService = this.Container.Resolve<IDomainService<Fias>>().GetAll();

            // Словарь ФИАС
            this.fiasDict = fiasService
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
                .Where(x => x.municipalityGuid != null)
                .Where(x => x.parentName != null)
                .Where(x => x.parentShortName != null)
                .Where(x => x.childName != null)
                .Where(x => x.childShortName != null)
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

            this._municipalitiesDict = this.Container.Resolve<IDomainService<Municipality>>().GetAll()
                .Where(x => x.FiasId != null)
                .AsEnumerable()
                .GroupBy(x => x.FiasId)
                .ToDictionary(x => x.Key, x => x.First());

            this._fiasIdByMunicipalityNameDict = this.Container.Resolve<IDomainService<Municipality>>().GetAll()
                .Where(x => Name != null)
                .Select(x => new { x.Name, x.FiasId })
                .AsEnumerable()
                .GroupBy(x => x.Name.ToUpper())
                .ToDictionary(x => x.Key, x => x.First().FiasId);
            
            var structElemList = this.StructuralElementRepository.GetAll()
                .Select(x => new
                    {
                        seCode = x.Code,
                        seId = x.Id,
                        seName = x.Name,
                        coeCode = x.Group.CommonEstateObject.Code
                    })
                .ToArray();

            // Словарь конструктивных характеристик over ООИ
            this.structuralElementDict = structElemList
                .Where(x => x.coeCode != null)
                .Where(x => x.seCode != null)
                .GroupBy(x => x.coeCode)
                .ToDictionary(
                    x => x.Key,
                    x => x.GroupBy(z => z.seCode).ToDictionary(z => z.Key, z => z.First().seId));


            // Словарь наименований конструктивных характеристик
            this.structuralElementNameDict = structElemList.ToDictionary(x => x.seId, x => x.seName);

            // Словарь конструктивных характеристик жилых домов
            this.realtyObjectStructuralElementsDict = this.RealityObjectStructuralElementRepository.GetAll()
                .Where(x => x.RealityObject != null)
                .Where(x => x.StructuralElement != null)
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

            // Словарь материалов стен
            wallMaterialDict = WallMaterialRepository.GetAll()
                .Where(x => x.Name != null)
                .AsEnumerable()
                .GroupBy(x => x.Name.ToUpper())
                .ToDictionary(x => x.Key, x => x.First());

            // Словарь материалов кровли
            roofingMaterialDict = RoofingMaterialRepository.GetAll()
                .Where(x => x.Name != null)
                .AsEnumerable()
                .GroupBy(x => x.Name.ToUpper())
                .ToDictionary(x => x.Key, x => x.First());

            // Словарь группы капитальности
            capitalGroupDict = CapitalGroupRepository.GetAll()
                .Where(x => x.Name != null)
                .AsEnumerable()
                .GroupBy(x => x.Name.ToUpper())
                .ToDictionary(x => x.Key, x => x.First());
        }

        /// <summary>
        /// Обработка файла импорта
        /// </summary>
        private void ProcessData(byte[] fileData)
        {
            var servRealityObject = this.Container.Resolve<IDomainService<RealityObject>>();
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
                var rowNumberer = 0;

                while ((line = reader.ReadLine()) != null)
                {
                    var record = this.ProcessLine(line, ++rowNumberer);
                    if (!record.isValidRecord)
                    {
                        continue;
                    }
                   
                    if (this._fiasAdressesByGuidDict.ContainsKey(record.StreetAoGuid))
                    {
                        var result = this._fiasAdressesByGuidDict[record.StreetAoGuid];

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

                            record.FiasAddress = this.CreateAddressByStreetAoGuid(record.StreetAoGuid, record.House, record.Housing);

                            this._realtyObjectsToCreate.Add(record);
                        }
                        else
                        {
                            var realtyObjectIdsByAddress = result.Where(x => realtyObjects.ContainsKey(x.Id)).SelectMany(x => realtyObjects[x.Id]).ToList();

                            if (realtyObjectIdsByAddress.Count == 0)
                            {
                                // дом не найден в системе
                                record.FiasAddressId = result.First().Id;
                                this._realtyObjectsToCreate.Add(record);
                                continue;
                            }

                            if (realtyObjectIdsByAddress.Count > 1)
                            {
                                this.AddLog(record.RowNumber, "В системе найдено несколько домов, соответсвующих записи.", RealityObjectImportResultType.No);
                                continue;
                            }

                            var realtyObjectId = realtyObjectIdsByAddress.First();

                            // Если в файле есть дублирующиеся дома, загружать последний номер.
                            this._foundRealtyObjects[realtyObjectId] = record;
                        }
                    }
                    else
                    {
                        record.FiasAddress = this.CreateAddressByStreetAoGuid(record.StreetAoGuid, record.House, record.Housing);

                        this._realtyObjectsToCreate.Add(record);
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
            if (this._dynamicAddressDict.ContainsKey(aoGuid))
            {
                dynamicAddress = this._dynamicAddressDict[aoGuid];
            }
            else
            {
                dynamicAddress = this.IFiasRepository.GetDinamicAddress(aoGuid);
                this._dynamicAddressDict[aoGuid] = dynamicAddress;
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
        private RealtyObjectRecord ProcessLine(string line, int rowNumber)
        {
            var record = new RealtyObjectRecord { isValidRecord = false, RowNumber = rowNumber};
            var data = line.ToStr().Split(';').Select(x => x.Trim('"')).ToArray();
            if (data.Length <= 1)
            {
                return record;
            }

            record.LocalityName = this.GetValue(data, "LocalityName");
            record.StreetName = this.GetValue(data, "StreetName");
            record.House = this.GetValue(data, "House").Trim();
            
            if (string.IsNullOrEmpty(record.LocalityName))
            {
                this.AddLog(record.RowNumber, "Не задан населеный пункт.", RealityObjectImportResultType.DoesNotMeetFias);
                return record;
            }

            if (string.IsNullOrEmpty(record.StreetName))
            {
                this.AddLog(record.RowNumber, "Не задана улица.", RealityObjectImportResultType.DoesNotMeetFias);
                return record;
            }

            if (string.IsNullOrEmpty(record.House))
            {
                this.AddLog(record.RowNumber, "Не задан номер дома.", RealityObjectImportResultType.No);
                return record;
            }

            if (!this.fiasDict.ContainsKey(record.LocalityName.ToUpper()))
            {
                this.AddLog(record.RowNumber, "Не удалось найти соответсвующий населенный пункт в системе.", RealityObjectImportResultType.DoesNotMeetFias);
                return record;
            }

            var localityFiasDict = this.fiasDict[record.LocalityName.ToUpper()];

            var localityStreets = localityFiasDict.First().Value;
            
            if (localityFiasDict.Count > 1)
            {
                var municipalityName = this.GetValue(data, "MunicipalityName").Trim().ToUpper();

                if (this._fiasIdByMunicipalityNameDict.ContainsKey(municipalityName))
                {
                    var municipalityGuid = this._fiasIdByMunicipalityNameDict[municipalityName];
                    var localities = localityFiasDict.Where(x => x.Key == municipalityGuid).Select(x => x.Value).ToList();

                    if (localities.Count == 1)
                    {
                        localityStreets = localities.First();
                    }
                    else if (localities.Count < 1)
                    {
                        this.AddLog(record.RowNumber, "В системе найдено несколько населенных пунктов, соответсвующих записи. "
                                                  + "Но в указанном муниципальном образовании данный населенный пункт не найден", RealityObjectImportResultType.No);
                        return record;
                    }
                    else if (localities.Count > 1)
                    {
                        this.AddLog(record.RowNumber, "В системе найдено несколько населенных пунктов, соответсвующих записи с заданным муниципальным образованием.", RealityObjectImportResultType.No);
                        return record;
                    }
                }
                else
                {
                    this.AddLog(record.RowNumber, "В системе найдено несколько населенных пунктов, соответсвующих записи. "
                                              + "Но в справочнике муниципальных образований не найдена соответствующая запись.", RealityObjectImportResultType.No);
                    return record;
                }
            }

            if (!localityStreets.ContainsKey(record.StreetName.ToUpper()))
            {
                this.AddLog(record.RowNumber, "Не удалось найти соответсвующую улицу населенного пункта в системе.", RealityObjectImportResultType.DoesNotMeetFias);
                return record;
            }

            record.StreetAoGuid = localityStreets[record.StreetName.ToUpper()];  
            
            record.Housing = this.GetValue(data, "Housing");
            record.TypeHouse = this.GetValue(data, "TypeHouse");
            record.BuildYear = this.GetValue(data, "BuildYear");
            record.AreaMkd = this.GetValue(data, "AreaMkd");
            record.Floors = this.GetValue(data, "Floors");
            record.MaximumFloors = this.GetValue(data, "MaximumFloors");
            record.NumberEntrances = this.GetValue(data, "NumberEntrances");
            record.NumberApartments = this.GetValue(data, "NumberApartments");
            record.NumberLiving = this.GetValue(data, "NumberLiving");
            record.WallMaterial = this.GetValue(data, "WallMaterial");
            record.RoofingMaterial = this.GetValue(data, "RoofingMaterial");
            record.CapitalGroup = this.GetValue(data, "CapitalGroup");
            record.AreaLiving = this.GetValue(data, "AreaLiving");
            record.AreaLivingOwned = this.GetValue(data, "AreaLivingOwned");
            record.PrivatizationDateFirstApartment = this.GetValue(data, "PrivatizationDateFirstApartment");
            record.FacadeArea = this.GetValue(data, "FacadeArea");
            record.RoofArea = this.GetValue(data, "RoofArea");
            record.FoundationArea = this.GetValue(data, "FoundationArea");
            record.FacadeLastRepairYear = this.GetValue(data, "FacadeLastRepairYear");
            record.RoofLastRepairYear = this.GetValue(data, "RoofLastRepairYear");
            record.ElectricityLastRepairYear = this.GetValue(data, "ElectricityLastRepairYear");
            record.HeatSystemLastRepairYear = this.GetValue(data, "HeatSystemLastRepairYear");
            record.ColdWaterLastRepairYear = this.GetValue(data, "ColdWaterLastRepairYear");
            record.HotWaterLastRepairYear = this.GetValue(data, "HotWaterLastRepairYear");
            record.SewerageLastRepairYear = this.GetValue(data, "SewerageLastRepairYear");
            record.ConditionHouse = this.GetValue(data, "ConditionHouse");
            record.FacadeType = this.GetValue(data, "FacadeType");
            record.RoofType = this.GetValue(data, "RoofType");
            record.FoundationType = this.GetValue(data, "FoundationType");
            record.HeatsystemType = this.GetValue(data, "HeatsystemType");
            record.SewerageType = this.GetValue(data, "SewerageType");
            record.ElectricitySystemType = this.GetValue(data, "ElectricitySystemType");

            record.FoundationArea = this.GetValue(data, "FoundationArea");
            record.FoundationWearout = this.GetValue(data, "FoundationWearout");
            record.FoundationLastRepairYear = this.GetValue(data, "FoundationLastRepairYear");
            record.RoofWearout = this.GetValue(data, "RoofWearout");
            record.FacadeWearout = this.GetValue(data, "FacadeWearout");
            record.ColdWaterSystemWearout = this.GetValue(data, "ColdWaterSystemWearout");
            record.HotWaterSystemWearout = this.GetValue(data, "HotWaterSystemWearout");
            record.ColdWaterSystemType = this.GetValue(data, "ColdWaterSystemType");
            record.HotWaterSystemType = this.GetValue(data, "HotWaterSystemType");
            record.SewerageWearout = this.GetValue(data, "SewerageWearout");
            record.HeatsystemWearout = this.GetValue(data, "HeatsystemWearout");
            record.ElectricityWearout = this.GetValue(data, "ElectricityWearout");

            record.ElectricNetworksLength = this.GetValue(data, "ElectricNetworksLength");
            record.HeatSystemPipeLength = this.GetValue(data, "HeatSystemPipeLength");
            record.ColdWaterPipeLength = this.GetValue(data, "ColdWaterPipeLength");
            record.HotWaterPipeLength = this.GetValue(data, "HotWaterPipeLength");
            record.SeweragePipeLength = this.GetValue(data, "SeweragePipeLength");

            record.ElectroDeviceCount = this.GetValue(data, "ElectroDeviceCount");
            record.HeatDeviceCount = this.GetValue(data, "HeatDeviceCount");
            record.ColdDeviceCount = this.GetValue(data, "ColdDeviceCount");
            record.HotDeviceCount = this.GetValue(data, "HotDeviceCount");
            
            record.isValidRecord = true;

            return record;
        }

        private string GetValue(string[] data, string field)
        {
            var result = string.Empty;

            if (this._headersDict.ContainsKey(field))
            {
                var index = this._headersDict[field];
                if (data.Length > index)
                {
                    result = data[index] != null ? data[index].Trim(): string.Empty;
                }
            }

            return result;
        }

        /// <summary>
        /// Метод переноса свойств из записи файла, в RealityObject
        /// </summary>
        private void Apply(RealityObject realityObject, RealtyObjectRecord record)
        {
            Action<string> warnLog = text => this.AddLog(record.RowNumber, text, RealityObjectImportResultType.YesButWithErrors); 

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
                        realityObject.HasPrivatizedFlats = true;
                    }
                    else
                    {
                        warnLog("Дата приватизации первого жилого помещения");
                    }
                }
                else
                {
                    realityObject.PrivatizationDateFirstApartment = record.PrivatizationDateFirstApartment.ToDateTime();
                    realityObject.HasPrivatizedFlats = true;
                }
            }
            else
            {
                warnLog("Дата приватизации первого жилого помещения");
            }

            if (!string.IsNullOrEmpty(record.WallMaterial) && wallMaterialDict.ContainsKey(record.WallMaterial.ToUpper()))
            {
                realityObject.WallMaterial = wallMaterialDict[record.WallMaterial.ToUpper()];
            }
            else
            {
                warnLog("Материалы стен");
            }

            if (!string.IsNullOrEmpty(record.RoofingMaterial) && roofingMaterialDict.ContainsKey(record.RoofingMaterial.ToUpper()))
            {
                realityObject.RoofingMaterial = roofingMaterialDict[record.RoofingMaterial.ToUpper()];
            }
            else
            {
                warnLog("Материалы крыши");
            }

            if (!string.IsNullOrEmpty(record.CapitalGroup) && capitalGroupDict.ContainsKey(record.CapitalGroup.ToUpper()))
            {
                realityObject.CapitalGroup = capitalGroupDict[record.CapitalGroup.ToUpper()];
            }
            else
            {
                warnLog("Группа капитальности");
            }

            if (!string.IsNullOrEmpty(record.HeatsystemType))
            {
                switch (record.HeatsystemType.ToUpper())
                {
                    case "ЦЕНТРАЛИЗОВАННОЕ":
                        realityObject.HeatingSystem = HeatingSystem.Centralized;
                        break;

                    case "ИНДИВИДУАЛЬНОЕ":
                        realityObject.HeatingSystem = HeatingSystem.Individual;
                        break;

                    default:
                        warnLog("Система отопление");
                        break;
                }
            }
            else
            {
                warnLog("Система отопление");
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
                    if (this._municipalitiesDict.ContainsKey(guid))
                    {
                        result = this._municipalitiesDict[guid];
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

            if (this._dynamicAddressDict.ContainsKey(mo.FiasId))
            {
                dinamicAddress = this._dynamicAddressDict[mo.FiasId];
            }
            else
            {
                dinamicAddress = this.IFiasRepository.GetDinamicAddress(mo.FiasId);
                this._dynamicAddressDict[mo.FiasId] = dinamicAddress;
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
            Dictionary<long, long> roStructuralElementsDict, bool realtyObjectIsNew, string lastOverhaulYear = null, string volume = null, string wearout = null)
        {
            if (!this.structuralElementDict.ContainsKey(coeCode) || string.IsNullOrEmpty(seCode))
            {
                return false;
            }

            var coeStructuralElements = this.structuralElementDict[coeCode];
            
            if (!coeStructuralElements.ContainsKey(seCode))
            {
                return false;
            }

            var structuralElementId = coeStructuralElements[seCode];

            RealityObjectStructuralElement realityObjectStructuralElement;

            if (!realtyObjectIsNew && roStructuralElementsDict.ContainsKey(structuralElementId))
            {
                realityObjectStructuralElement = this.RealityObjectStructuralElementRepository.Load(roStructuralElementsDict[structuralElementId]);
            }
            else
            {
                realityObjectStructuralElement = new RealityObjectStructuralElement
                    {
                        RealityObject = realityObject,
                        StructuralElement = this.StructuralElementRepository.Load(structuralElementId),
                        Name = this.structuralElementNameDict[structuralElementId]
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

            if (!string.IsNullOrEmpty(wearout))
            {
                decimal value;
                var str = wearout.Replace(
                    CultureInfo.InvariantCulture.NumberFormat.CurrencyDecimalSeparator,
                    CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator);

                if (decimal.TryParse(str, out value))
                {
                    realityObjectStructuralElement.Wearout = value;
                }
            }

            this.RealityObjectStructuralElementRepository.Save(realityObjectStructuralElement);

            return true;
        }

        private void SaveConstructiveElements(RealityObject realityObject, RealtyObjectRecord record, bool realtyObjectIsNew = true)
        {
            var roStructuralElementsDict = this.realtyObjectStructuralElementsDict.ContainsKey(realityObject.Id)
                       ? this.realtyObjectStructuralElementsDict[realityObject.Id]
                       : new Dictionary<long, long>();

            Func<string, string, string, string, string, bool> updateStructElement =
                (ceoCode, seCode, lastRepairYear, volume, wearout) =>
                    {
                        if (seCode == "0")
                        {
                            return true;
                        }

                        if (string.IsNullOrWhiteSpace(volume) || volume == "0")
                        {
                            return true;
                        }

                        return this.UpdateRobjStructElem(
                            ceoCode,
                            seCode.Trim(),
                            realityObject,
                            roStructuralElementsDict,
                            realtyObjectIsNew,
                            lastRepairYear,
                            volume,
                            wearout);
                    };

            Func<string, string, string, bool> updateMeterDeviceStructElement = (ceoCode, seCode, volume) =>
                {
                    var lastRepairYear = realityObject.BuildYear < 2007 ? "2007" : realityObject.BuildYear.ToStr();
                    return updateStructElement(ceoCode, seCode, lastRepairYear, volume, "10");
                };

            #region Фасад

            if (!string.IsNullOrEmpty(record.FacadeType))
            {
                if (!updateStructElement("2", record.FacadeType, record.FacadeLastRepairYear, record.FacadeArea, record.FacadeWearout))
                { 
                    this.AddLog(record.RowNumber, "Фасад", RealityObjectImportResultType.YesButWithErrors);
                }
            }

            #endregion Фасад

            #region Крыша

            if (!string.IsNullOrEmpty(record.RoofType))
            {
                if (!updateStructElement("1", record.RoofType, record.RoofLastRepairYear, record.RoofArea, record.RoofWearout))
                {
                    this.AddLog(record.RowNumber, "Крыша", RealityObjectImportResultType.YesButWithErrors);
                }
            }

            #endregion Крыша

            #region Система отопления

            if (!string.IsNullOrEmpty(record.HeatsystemType) && record.HeatsystemType == "1")
            {
                if (!updateStructElement("7", "1", record.HeatSystemLastRepairYear, record.HeatSystemPipeLength, record.HeatsystemWearout))
                {
                    this.AddLog(record.RowNumber, "Инженерная система отопления", RealityObjectImportResultType.YesButWithErrors);
                }
            }

            if (!string.IsNullOrEmpty(record.HeatDeviceCount))
            {
                if (!updateMeterDeviceStructElement("7", "2", record.HeatDeviceCount))
                {
                    this.AddLog(record.RowNumber, "ОДПУ теплоснабжения", RealityObjectImportResultType.YesButWithErrors);
                }
            }

            #endregion Система отопления

            #region Система горячего водоснабжения

            if (!string.IsNullOrEmpty(record.HotWaterSystemType))
            {
                if (record.HotWaterSystemType == "1")
                {
                    if (!updateStructElement("4", "1", record.HotWaterLastRepairYear, record.HotWaterPipeLength, record.HotWaterSystemWearout))
                    {
                        this.AddLog(record.RowNumber, "Инженерная система горячего водоснабжения", RealityObjectImportResultType.YesButWithErrors);
                    }
                }
                else if (record.HotWaterSystemType == "2")
                {
                    if (!this.UpdateRobjStructElem("4", "2", realityObject, roStructuralElementsDict, realtyObjectIsNew, record.HotWaterLastRepairYear, string.Empty, record.HotWaterSystemWearout))
                    {
                         this.AddLog(record.RowNumber, "Инженерная система горячего водоснабжения", RealityObjectImportResultType.YesButWithErrors);
                    }
                }
            }

            if (!string.IsNullOrEmpty(record.HotDeviceCount))
            {
                if (!updateMeterDeviceStructElement("4", "3", record.HotDeviceCount))
                {
                    this.AddLog(record.RowNumber, "ОДПУ горячего водоснабжения", RealityObjectImportResultType.YesButWithErrors);
                }
            }

            #endregion Система горячего водоснабжения

            #region Система холодного водоснабжения

            if (!string.IsNullOrEmpty(record.ColdWaterSystemType))
            {
                if (record.ColdWaterSystemType == "1")
                {
                    if (!updateStructElement("5", "1", record.ColdWaterLastRepairYear, record.ColdWaterPipeLength, record.ColdWaterSystemWearout))
                    {
                        this.AddLog(record.RowNumber, "Инженерная система холодного водоснабжения", RealityObjectImportResultType.YesButWithErrors);
                    }
                }
                else if (record.ColdWaterSystemType == "2")
                {
                    if (!this.UpdateRobjStructElem("5", "2", realityObject, roStructuralElementsDict, realtyObjectIsNew, record.ColdWaterLastRepairYear, string.Empty, record.ColdWaterSystemWearout))
                    {
                        this.AddLog(record.RowNumber, "Инженерная система холодного водоснабжения", RealityObjectImportResultType.YesButWithErrors);
                    }
                }
            }

            if (!string.IsNullOrEmpty(record.ColdDeviceCount))
            {
                if (!updateMeterDeviceStructElement("5", "3", record.ColdDeviceCount))
                {
                    this.AddLog(record.RowNumber, "ОДПУ холодного водоснабжения", RealityObjectImportResultType.YesButWithErrors);
                }
            }

            #endregion Система холодного водоснабжения

            #region Система водотведения

            if (!string.IsNullOrEmpty(record.SewerageType) && (record.SewerageType == "1" || record.SewerageType == "2"))
            {
                if (!updateStructElement("6", record.SewerageType, record.SewerageLastRepairYear, record.SeweragePipeLength, record.SewerageWearout))
                {
                    this.AddLog(record.RowNumber, "Инженерная система канализования и водоотведения", RealityObjectImportResultType.YesButWithErrors);
                }
            }

            #endregion Система водотведения

            #region Система электроснабжения

            if (!string.IsNullOrEmpty(record.ElectricitySystemType) && record.ElectricitySystemType == "1")
            {
                if (!updateStructElement("8", record.ElectricitySystemType, record.ElectricityLastRepairYear, record.ElectricNetworksLength, record.ElectricityWearout))
                {
                    this.AddLog(record.RowNumber, "Система электроснабжения", RealityObjectImportResultType.YesButWithErrors);
                }
            }

            if (!string.IsNullOrEmpty(record.ElectroDeviceCount))
            {
                if (!updateMeterDeviceStructElement("8", "2", record.ElectroDeviceCount))
                {
                    this.AddLog(record.RowNumber, "ОДПУ электроснабжения", RealityObjectImportResultType.YesButWithErrors);
                }
            }
            
            #endregion Система электроснабжения
            
            #region Фундамент

            if (!string.IsNullOrEmpty(record.FoundationType))
            {
                if (!updateStructElement("3", record.FoundationType, record.FacadeLastRepairYear, record.FoundationArea, record.FoundationWearout))
                {
                    this.AddLog(record.RowNumber, "Фундамент", RealityObjectImportResultType.YesButWithErrors);
                }
            }

            #endregion Фундамент
        }

        private string SaveData()
        {
            var message = string.Empty;

            var repRealityObject = this.Container.Resolve<IRepository<RealityObject>>();
            

            this.LogImport.CountAddedRows = 0;
            this.LogImport.CountChangedRows = 0;

            using (var transaction = this.Container.Resolve<IDataTransaction>())
            {
                try
                {
                    foreach (var foundRealtyObject in this._foundRealtyObjects.Where(x => x.Value != null))
                    {
                        var realityObj = repRealityObject.Load(foundRealtyObject.Key);
                        this.Apply(realityObj, foundRealtyObject.Value);
                        repRealityObject.Update(realityObj);
                        this.SaveConstructiveElements(realityObj, foundRealtyObject.Value, false);
                        this.AddLog(foundRealtyObject.Value.RowNumber, string.Empty, RealityObjectImportResultType.AlreadyExists);
                    }

                    if (this._realtyObjectsToCreate.Any())
                    {
                        // Если в файле есть дублирующиеся дома, загружать последний.
                        this._realtyObjectsToCreate = this._realtyObjectsToCreate
                            .GroupBy(x => new { x.StreetAoGuid, x.House, x.Housing })
                            .Select(x => x.Select(y => y).Last())
                            .ToList();

                        foreach (var realtyObjectToCreate in this._realtyObjectsToCreate.Where(x => x != null))
                        {
                            var objectToCreate = realtyObjectToCreate;

                            if (objectToCreate.FiasAddressId == null && objectToCreate.FiasAddress == null)
                            {
                                this.AddLog(
                                    objectToCreate.RowNumber,
                                    "Объект не создан. В ФИАС не удалось найти адреc, соответствующий записи", 
                                    RealityObjectImportResultType.DoesNotMeetFias);
                                continue;
                            }

                            FiasAddress fiasAddress;
                            if (objectToCreate.FiasAddressId.HasValue)
                            {
                                fiasAddress = FiasAddressRepository.Load(objectToCreate.FiasAddressId.Value);
                            }
                            else
                            {
                                if (!this.ValidateEntity(objectToCreate.FiasAddress, objectToCreate.RowNumber))
                                {
                                    continue;
                                }

                                FiasAddressRepository.Save(objectToCreate.FiasAddress);
                                fiasAddress = objectToCreate.FiasAddress;
                            }

                            var municipality = this.GetMunicipality(fiasAddress);

                            if (municipality == null)
                            {
                                this.AddLog(objectToCreate.RowNumber, "Не удалось определить муниципальное образование", RealityObjectImportResultType.No);
                                continue;
                            }

                            var realityObj = new RealityObject
                            {
                                FiasAddress = fiasAddress,
                                Municipality = municipality,
                                Address = this.GetAddressForMunicipality(municipality, fiasAddress)
                            };

                            this.Apply(realityObj, objectToCreate);

                            if (!this.ValidateEntity(municipality, objectToCreate.RowNumber))
                            {
                                continue;
                            }

                            if (!this.ValidateEntity(realityObj, objectToCreate.RowNumber))
                            {
                                continue;
                            }
                            
                            repRealityObject.Save(realityObj);
                            this.SaveConstructiveElements(realityObj, objectToCreate);
                            this.AddLog(objectToCreate.RowNumber, string.Empty, RealityObjectImportResultType.Yes);
                        }
                    }

                    transaction.Commit();
                }
                catch (Exception exc)
                {
                    try
                    {
                        this.LogImport.IsImported = false;
                        this.Container.Resolve<ILogger>().LogError(exc, "Импорт");
                        message = "Произошла неизвестная ошибка. Обратитесь к администратору";
                        this.LogImport.Error(this.Name, "Произошла неизвестная ошибка. Обратитесь к администратору.");

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

            var fileExtentions = this.PossibleFileExtensions.Contains(",") ? this.PossibleFileExtensions.Split(',') : new[] { this.PossibleFileExtensions };
            if (fileExtentions.All(x => x != extention))
            {
                message = string.Format("Необходимо выбрать файл с допустимым расширением: {0}", this.PossibleFileExtensions);
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

        private void AddLog(int rowNumber, string text, RealityObjectImportResultType type = RealityObjectImportResultType.None)
        {
            var id = rowNumber.ToStr();
            var log = new Log();
            if (this.logDict.ContainsKey(id))
            {
                log = this.logDict[id];
            }
            else
            {
                this.logDict[id] = log;
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
            foreach (var log in this.logDict)
            {
                var logValue = log.Value;

                var text = string.IsNullOrEmpty(log.Value.Text.Trim(' ').Trim(','))
                               ? string.Empty
                               : "Место ошибки: " + log.Value.Text.Trim(' ').Trim(',');
                
                switch (logValue.ResultType)
                {
                    case RealityObjectImportResultType.AlreadyExists:
                        this.LogImport.CountChangedRows++;
                        break;

                    case RealityObjectImportResultType.DoesNotMeetFias:
                        this.LogImport.CountError++;
                        break;

                    case RealityObjectImportResultType.No:
                        this.LogImport.CountError++;
                        break;

                    case RealityObjectImportResultType.Yes:
                        this.LogImport.CountAddedRows++;
                        break;

                    case RealityObjectImportResultType.YesButWithErrors:
                        this.LogImport.CountAddedRows++;
                        this.LogImport.CountWarning++;
                        break;
                }

                var resultText = string.Format(
                    "Строка: {0}; Дом добавлен: {1} ",
                    log.Key,
                    logValue.ResultType.GetEnumMeta().Display);

                this.LogImport.Info(resultText, text);
            }
        }


        /// <summary>
        /// Валидация сущности согласно маппингу
        /// </summary>
        /// <typeparam name="T">Тип сущности</typeparam>
        /// <param name="entity">Объект</param>
        /// <param name="message">Сообщение</param>
        private bool ValidateEntity<T>(T entity, int rowNumber)
        {
            var classMeta = this.SessionProvider.GetCurrentSession()
                .SessionFactory.GetClassMetadata(typeof(T));

            var i = 0;
            foreach (var propName in classMeta.PropertyNames)
            {
                var propType = classMeta.GetPropertyType(propName);
                var st = propType as StringType;

                var nullable = classMeta.PropertyNullability[i];

                var val = typeof(T).GetProperty(propName)
                    .GetValue(entity, new object[0]);

                if (!nullable)
                {
                    if (val == null)
                    {
                        AddLog(rowNumber, string.Format("Свойство \"{0}\" не может принимать пустые значения; ", propName), RealityObjectImportResultType.No);
                        return false;
                    }
                }

                if (st != null)
                {
                    if (st.SqlType.LengthDefined)
                    {
                        if (val != null && val.ToStr().Length > st.SqlType.Length)
                        {
                            var message = string.Format("Значение свойства \"{0}\" не может быть длиннее {1}; ",
                                propName,
                                st.SqlType.Length);

                            AddLog(rowNumber, message, RealityObjectImportResultType.No);
                            return false;
                        }
                    }
                }

                ++i;
            }

            return true;
        }
    }
}