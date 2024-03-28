namespace Bars.Gkh.Import
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Modules.FIAS;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Enums.Import;
    using Bars.Gkh.Import.FundRealtyObjects;

    using Castle.Windsor;
    using Impl;
    using Ionic.Zip;

    using Microsoft.Extensions.Logging;

    using NHibernate.Type;

    public class FundRealtyObjectsImport : GkhImportBase
    {
        public static string Id = MethodBase.GetCurrentMethod().DeclaringType.FullName;

        public ISessionProvider SessionProvider { get; set; }

        public virtual IWindsorContainer Container { get; set; }

        private readonly Dictionary<string, int> _headersDict = new Dictionary<string, int>();

        private readonly Dictionary<long, FundRealtyObjectRecord> _foundRealtyObjects = new Dictionary<long, FundRealtyObjectRecord>();

        private List<FundRealtyObjectRecord> _realtyObjectsToCreate = new List<FundRealtyObjectRecord>();

        private Dictionary<string, long> _capitalGroup = new Dictionary<string, long>();

        private Dictionary<string, long> _typeProject = new Dictionary<string, long>();

        private Dictionary<string, List<FiasAddressRecord>> _fiasAdressesByKladrCodeDict;

        private Dictionary<string, string> _aoGuidByKladrCodeMap;

        private Dictionary<string, IDinamicAddress> _dynamicAddressDict = new Dictionary<string, IDinamicAddress>();

        private Dictionary<string, Municipality> _municipalitiesDict = new Dictionary<string, Municipality>();

        private Dictionary<long, List<RealtyObjectLandProxy>> _roLandDict;

        private Dictionary<string, ExtraDataProxy> _extraDataByFederalNumDict = new Dictionary<string, ExtraDataProxy>();

        public IFiasRepository fiasRepository { get; set; }

        public IRepository<RealityObject> RealityObjectRepository { get; set; }

        public IRepository<RealityObjectLand> RealityObjectLandRepository { get; set; }

        public IRepository<FiasAddress> FiasAddressRepository { get; set; }
        
        private bool createObjects = false;

        public override string Key
        {
            get { return Id; }
        }

        public override string CodeImport
        {
            get { return "FundRealtyObjectsImport"; }
        }

        public override string Name
        {
            get { return "Импорт домов"; }
        }

        public override string PossibleFileExtensions
        {
            get { return "zip"; }
        }

        public override string PermissionName
        {
            get { return "Import.FundRealtyObjectsImport.View"; }
        }

        private static string importHeader = "RASP_PART_0";

        public FundRealtyObjectsImport(ILogImportManager logImportManager, ILogImport logImport)
        {
            this.LogImportManager = logImportManager;
            this.LogImport = logImport;
        }

        public override ImportResult Import(BaseParams baseParams)
        {
            var fileData = baseParams.Files["FileImport"];
            var replaceData = baseParams.Params.GetAs("replaceDataOnImport", false);

            this.createObjects = baseParams.Params.GetAs("createObjectsOnImport", false);

            InitLog(fileData.FileName);

            InitDictionaries();

            var message = string.Empty;

            bool successfullyUnzipped;

            using (var memoryStream = GetFileMemoryStream(fileData.Data, "RASP_PART_0", "csv", out successfullyUnzipped))
            {
                if (!successfullyUnzipped)
                {
                    LogImport.Warn(importHeader, "Не удалось распаковать файл RASP_PART_0 из архива");
                }
                else
                {
                    memoryStream.Seek(0, SeekOrigin.Begin);

                    ProcessData(memoryStream);
                }
            }

            ExtraDataImport(fileData);

            if (successfullyUnzipped)
            {
                message = SaveData();
            }

            // Намеренно закрываем текущую сессию, иначе при каждом коммите транзакции
            // ранее измененные дома вызывают каскадирование ФИАС
            Container.Resolve<ISessionProvider>().CloseCurrentSession();

            var part0Added = LogImport.CountAddedRows;
            var part0Changed = LogImport.CountChangedRows;
            var part0Warning = LogImport.CountWarning;

            ImportTechPassport(fileData, replaceData);

            LogImport.Info(string.Empty, string.Format("Общее количество добавленых домов: {0}", part0Added));
            LogImport.Info(string.Empty, string.Format("Общее количество обновленных домов: {0}", part0Changed));
            LogImport.Info(string.Empty, string.Format("Общее количество предупреждений по домам: {0}", part0Warning));

            LogImport.Info(string.Empty, string.Format("Общее количество добавленых записей в технический паспорт: {0}", LogImport.CountAddedRows - part0Added));
            LogImport.Info(string.Empty, string.Format("Общее количество обновленных записей технического паспорта: {0}", LogImport.CountChangedRows - part0Changed));
            LogImport.Info(string.Empty, string.Format("Общее количество предупреждений по записям технического паспорта: {0}", LogImport.CountWarning - part0Warning));

            LogImportManager.Add(fileData, LogImport);
            LogImportManager.Save();

            message += LogImportManager.GetInfo();
            var status = LogImportManager.CountError > 0 ? StatusImport.CompletedWithError : (LogImportManager.CountWarning > 0 ? StatusImport.CompletedWithWarning : StatusImport.CompletedWithoutError);
            return new ImportResult(status, message, string.Empty, LogImportManager.LogFileId);
        }

        private void InitDictionaries()
        {
            _capitalGroup = Container.Resolve<IDomainService<CapitalGroup>>().GetAll()
                .Where(x => x.Name != null)
                .Select(x => new { x.Id, x.Name })
                .AsEnumerable()
                .GroupBy(x => x.Name)
                .ToDictionary(x => x.Key, x => x.First().Id);

            _typeProject = Container.Resolve<IDomainService<TypeProject>>().GetAll()
                .Where(x => x.Name != null)
                .Select(x => new { x.Id, x.Name })
                .AsEnumerable()
                .GroupBy(x => x.Name)
                .ToDictionary(x => x.Key, x => x.First().Id);

            // Словарь всех существующих адресов по коду КЛАДР
            _fiasAdressesByKladrCodeDict = Container.Resolve<IDomainService<Fias>>().GetAll()
                .Join(
                    Container.Resolve<IDomainService<FiasAddress>>().GetAll(),
                    x => x.AOGuid,
                    y => y.StreetGuidId,
                    (a, b) => new { fias = a, fiasAddress = b }
                )
                .Where(x => x.fias.ActStatus == FiasActualStatusEnum.Actual)
                .Where(x => x.fias.KladrCode != null && x.fias.KladrCode != "")
                .Select(x => new FiasAddressRecord
                    {
                        KladrCode = x.fias.KladrCode, 
                        AoGuid = x.fias.AOGuid,
                        Id = x.fiasAddress.Id, 
                        House = x.fiasAddress.House, 
                        Housing = x.fiasAddress.Housing
                    })
                .AsEnumerable()
                .GroupBy(x => x.KladrCode)
                .ToDictionary(x => x.Key, x => x.ToList());

            // Словарь aoGiod по коду КЛАДР
            _aoGuidByKladrCodeMap = Container.Resolve<IDomainService<Fias>>().GetAll()
                .Where(x => x.ActStatus == FiasActualStatusEnum.Actual)
                .Where(x => x.KladrCode != null && x.KladrCode != "")
                .Select(x => new { x.KladrCode, x.AOGuid })
                .AsEnumerable()
                .GroupBy(x => x.KladrCode)
                .ToDictionary(x => x.Key, x => x.First().AOGuid);

            _municipalitiesDict = Container.Resolve<IDomainService<Municipality>>().GetAll()
                .Where(x => x.FiasId != null)
                .AsEnumerable()
                .GroupBy(x => x.FiasId)
                .ToDictionary(x => x.Key, x => x.First());

            _roLandDict = RealityObjectLandRepository.GetAll()
                .Where(x => x.RealityObject != null)
                .Select(x => new
                    {
                        roId = x.RealityObject.Id,
                        x.Id,
                        x.CadastrNumber,
                        x.DateLastRegistration,
                        x.DocumentName
                    })
                .AsEnumerable()
                .GroupBy(x => x.roId)
                .ToDictionary(
                    x => x.Key,
                    x => x.Select(y => new RealtyObjectLandProxy
                            {
                                Id = y.Id,
                                CadastrNumber = y.CadastrNumber,
                                DocumentName = y.DocumentName,
                                DateLastRegistration = y.DateLastRegistration
                            })
                          .ToList());
        }

        /// <summary>
        /// Обработка файла импорта
        /// </summary>
        private void ProcessData(MemoryStream memoryStreamFile)
        {
            var realtyObjects = RealityObjectRepository.GetAll()
                .Select(x => new { x.Id, FiasAddressId = x.FiasAddress.Id })
                .AsEnumerable()
                .GroupBy(x => x.FiasAddressId)
                .ToDictionary(x => x.Key, x => x.Select(y => y.Id).ToList());

            using (var reader = new StreamReader(memoryStreamFile, Encoding.GetEncoding(1251)))
            {
                var headers = reader.ReadLine().ToStr().Split(';').Select(x => x.Trim('"')).ToArray();

                var headerLength = headers.Length;

                if (headerLength <= 1)
                {
                    throw new Exception("Неверный файл загрузки");
                }

                for (var index = 0; index < headerLength; ++index)
                {
                    _headersDict[headers[index]] = index;
                }

                string line;

                while ((line = reader.ReadLine()) != null)
                {
                    var record = ProcessLine(line);
                    if (!record.isValidRecord)
                    {
                        continue;
                    }

                    var codeKladr = record.CodeKladrStreet;

                    if (_fiasAdressesByKladrCodeDict.ContainsKey(codeKladr))
                    {
                        var fiasAdresses = _fiasAdressesByKladrCodeDict[codeKladr];
                        var result = fiasAdresses;

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
                            if (createObjects)
                            {
                                var aoGuid = fiasAdresses.First().AoGuid;

                                record.FiasAddress = CreateAddressByStreetAoGuid(aoGuid, record.House, record.Housing);

                                _realtyObjectsToCreate.Add(record);
                            }
                            else
                            {
                                LogImport.Warn(importHeader, string.Format("ID_MKD: {0}. Не удалось найти адреc, соответствующий KLADR_KOD_STREET: {1}, NUM: {2}, KORPUS: {3}",
                                    record.FederalNumber,
                                    record.CodeKladrStreet,
                                    record.House,
                                    record.Housing));
                            }
                        }
                        else
                        {
                            var realtyObjectIdsByAddress = result.Where(x => realtyObjects.ContainsKey(x.Id)).SelectMany(x => realtyObjects[x.Id]).ToList();

                            if (realtyObjectIdsByAddress.Count == 0)
                            {
                                // дом не найден в системе
                                if (createObjects)
                                {
                                    record.FiasAddressId = result.First().Id;
                                    _realtyObjectsToCreate.Add(record);
                                }
                                else
                                {
                                    LogImport.Warn(importHeader, string.Format("ID_MKD: {0}. Не удалось найти дом, соответствующий KLADR_KOD_STREET: {1}, NUM: {2}, KORPUS: {3}",
                                    record.FederalNumber,
                                    record.CodeKladrStreet,
                                    record.House,
                                    record.Housing));
                                }
                                continue;
                            }

                            if (realtyObjectIdsByAddress.Count > 1)
                            {
                                LogImport.Warn(importHeader, string.Format("ID_MKD: {0}. По адресу KLADR_KOD_STREET: {1}, NUM: {2}, KORPUS: {3} - найдено {4} объектов.",
                                    record.FederalNumber,
                                    record.CodeKladrStreet,
                                    record.House,
                                    record.Housing,
                                    result.Count));
                                continue;
                            }

                            var realtyObjectId = realtyObjectIdsByAddress.First();

                            // Если в файле есть дублирующиеся дома, загружать последний номер.
                            _foundRealtyObjects[realtyObjectId] = record;
                        }
                    }
                    else
                    {
                        if (createObjects && _aoGuidByKladrCodeMap.ContainsKey(codeKladr))
                        {
                            var aoGuid = _aoGuidByKladrCodeMap[codeKladr];

                            record.FiasAddress = CreateAddressByStreetAoGuid(aoGuid, record.House, record.Housing);

                            _realtyObjectsToCreate.Add(record);
                        }
                        else
                        {
                            LogImport.Warn(importHeader, string.Format("ID_MKD: {0}. В системе не найдена улица, соответствующая коду КЛАДР {1}",
                                record.FederalNumber,
                                record.CodeKladrStreet));
                        }
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
        private FundRealtyObjectRecord ProcessLine(string line)
        {
            var record = new FundRealtyObjectRecord { isValidRecord = false };
            var data = line.ToStr().Split(';').Select(x => x.Trim('"')).ToArray();
            if (data.Length <= 1)
            {
                return record;
            }

            record.CodeKladrStreet = GetValue(data, "KLADR_KOD_STREET");
            record.House = GetValue(data, "NUM").Trim();
            record.Housing = GetValue(data, "KORPUS").Trim();
            record.FederalNumber = GetValue(data, "ID_MKD");

            if (string.IsNullOrEmpty(record.CodeKladrStreet) || record.CodeKladrStreet.Length != 17)
            {
                LogImport.Warn(importHeader, string.Format("ID_MKD: {0}. Неверный код КЛАДР улицы: \"{1}\"", record.FederalNumber, record.CodeKladrStreet));
                return record;
            }

            if (string.IsNullOrEmpty(record.House))
            {
                LogImport.Warn(importHeader, string.Format("ID_MKD: {0}. Не задан номер дома", record.FederalNumber));
                return record;
            }

            record.NumberLiving = GetValue(data, "COUNT_PEOPLE");
            record.CapitalGroup = GetValue(data, "GRUPPA");
            record.TypeProject = GetValue(data, "SER");
            record.AreaMkd = GetValue(data, "SQV");
            record.AreaLiving = GetValue(data, "SQV_GIL");
            record.AreaLivingOwned = GetValue(data, "SQV_GR");
            record.DateCommissioning = GetValue(data, "YEAR");
            record.PhysicalWear = GetValue(data, "DET_DEGREE");
            record.CadastreNumber = GetValue(data, "NUM_TERRA");
            record.DateDemolition = GetValue(data, "DATA_SNOS");
            record.AreaLivingNotLivingMkd = GetValue(data, "SQV_ROOMS");
            record.AreaNotLivingFunctional = GetValue(data, "SQV_NOT_DW");
            record.AreaMunicipalOwned = GetValue(data, "SQV_MUN");
            record.AreaGovernmentOwned = GetValue(data, "SQV_GOS");
            record.CadastrDate = GetValue(data, "KADASTR_DATE");
            record.CadastrDocName = GetValue(data, "KADASTR_NAME_DOC");
            record.TypeHouse = GetValue(data, "TYPE_HOME");
            record.BuildYear = GetValue(data, "YEAR_BUY");

            record.isValidRecord = true;

            return record;
        }

        /// <summary>
        /// Метод получения из массива строк data значения, соответвующего полю field
        /// </summary>
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
        /// Метод переноса свойств из записи фонда, в RealityObject
        /// </summary>
        private bool Apply(RealityObject realityObject, FundRealtyObjectRecord record)
        {
            var anyChange = realityObject.FederalNum != record.FederalNumber;
            
            realityObject.FederalNum = record.FederalNumber;

            if (realityObject.NumberLiving == null && !string.IsNullOrEmpty(record.NumberLiving))
            {
                realityObject.NumberLiving = record.NumberLiving.ToInt();
                anyChange = true;
            }

            if (realityObject.CapitalGroup == null && !string.IsNullOrEmpty(record.CapitalGroup))
            {
                if (_capitalGroup.ContainsKey(record.CapitalGroup))
                {
                    realityObject.CapitalGroup = new CapitalGroup { Id = _capitalGroup[record.CapitalGroup] };
                    anyChange = true;
                }
            }

            if (realityObject.TypeProject == null && !string.IsNullOrEmpty(record.TypeProject))
            {
                if (_typeProject.ContainsKey(record.TypeProject))
                {
                    realityObject.TypeProject = new TypeProject { Id = _typeProject[record.TypeProject] };
                    anyChange = true;
                }
            }

            if (realityObject.AreaMkd == null && !string.IsNullOrEmpty(record.AreaMkd))
            {
                realityObject.AreaMkd = record.AreaMkd.ToDecimal();
                anyChange = true;
            }

            if (realityObject.AreaLiving == null && !string.IsNullOrEmpty(record.AreaLiving))
            {
                realityObject.AreaLiving = record.AreaLiving.ToDecimal();
                anyChange = true;
            }

            if (realityObject.AreaLivingOwned == null && !string.IsNullOrEmpty(record.AreaLivingOwned))
            {
                realityObject.AreaLivingOwned = record.AreaLivingOwned.ToDecimal();
                anyChange = true;
            }

            if (realityObject.DateCommissioning == null && !string.IsNullOrEmpty(record.DateCommissioning))
            {
                var value = record.DateCommissioning.ToInt();
                if (value > 0)
                {
                    realityObject.DateCommissioning = new DateTime(value, 1, 1);
                    anyChange = true;
                }
            }

            if (realityObject.PhysicalWear == null && !string.IsNullOrEmpty(record.PhysicalWear))
            {
                realityObject.PhysicalWear = record.PhysicalWear.ToDecimal();
                anyChange = true;
            }

            if (string.IsNullOrEmpty(realityObject.CadastreNumber) && !string.IsNullOrEmpty(record.CadastreNumber))
            {
                realityObject.CadastreNumber = record.CadastreNumber;
                anyChange = true;
            }

            if (realityObject.BuildYear == null && !string.IsNullOrEmpty(record.BuildYear))
            {
                int value;
                if (int.TryParse(record.BuildYear, out value))
                {
                    realityObject.BuildYear = value;
                    anyChange = true;    
                }
            }

            if (realityObject.TypeHouse == TypeHouse.NotSet && !string.IsNullOrEmpty(record.TypeHouse))
            {
                switch (record.TypeHouse.ToUpper())
                {
                    case "БЛОКИРОВАННЫЙ": realityObject.TypeHouse = TypeHouse.BlockedBuilding;
                        anyChange = true;
                        break;

                    case "ИНДИВИДУАЛЬНЫЙ ДОМ": realityObject.TypeHouse = TypeHouse.Individual;
                        anyChange = true;
                        break;

                    case "МНОГОКВАРТИРНЫЙ ДОМ": realityObject.TypeHouse = TypeHouse.ManyApartments;
                        anyChange = true;
                        break;
                }
            }

            if (realityObject.ConditionHouse == 0)
            {
                realityObject.ConditionHouse = ConditionHouse.Serviceable;
                anyChange = true;
            }

            if (realityObject.DateDemolition == null && !string.IsNullOrEmpty(record.DateDemolition))
            {
                if (record.DateDemolition.Trim().Length == 4)
                {
                    var value = record.DateDemolition.ToInt();
                    if (value > 0)
                    {
                        realityObject.DateDemolition = new DateTime(value, 1, 1);
                        anyChange = true;
                    }
                }
                else
                {
                    realityObject.DateDemolition = record.DateDemolition.ToDateTime();
                    anyChange = true;
                }
            }

            if (realityObject.AreaLivingNotLivingMkd == null && !string.IsNullOrEmpty(record.AreaLivingNotLivingMkd))
            {
                realityObject.AreaLivingNotLivingMkd = record.AreaLivingNotLivingMkd.ToDecimal();
                anyChange = true;
            }

            if (realityObject.AreaNotLivingFunctional == null && !string.IsNullOrEmpty(record.AreaNotLivingFunctional))
            {
                realityObject.AreaNotLivingFunctional = record.AreaNotLivingFunctional.ToDecimal();
                anyChange = true;
            }

            if (realityObject.AreaMunicipalOwned == null && !string.IsNullOrEmpty(record.AreaMunicipalOwned))
            {
                realityObject.AreaMunicipalOwned = record.AreaMunicipalOwned.ToDecimal();
                anyChange = true;
            }

            if (realityObject.AreaGovernmentOwned == null && !string.IsNullOrEmpty(record.AreaGovernmentOwned))
            {
                realityObject.AreaGovernmentOwned = record.AreaGovernmentOwned.ToDecimal();
                anyChange = true;
            }

            if (_extraDataByFederalNumDict.ContainsKey(record.FederalNumber))
            {
                var extraData = _extraDataByFederalNumDict[record.FederalNumber];

                if (realityObject.MaximumFloors == null && extraData.MaximumFloors.HasValue)
                {
                    realityObject.MaximumFloors = extraData.MaximumFloors;
                    anyChange = true;
                }

                if (realityObject.NumberEntrances == null && extraData.NumberEntrances.HasValue)
                {
                    realityObject.NumberEntrances = extraData.NumberEntrances;
                    anyChange = true;
                }

                if (realityObject.Floors == null && extraData.Floors.HasValue)
                {
                    realityObject.Floors = extraData.Floors;
                    anyChange = true;
                }

                if (realityObject.TypeRoof == 0 && extraData.TypeRoof.HasValue)
                {
                    realityObject.TypeRoof = extraData.TypeRoof.Value;
                    anyChange = true;
                }

                if (realityObject.HeatingSystem == 0 && extraData.HeatingSystem.HasValue)
                {
                    realityObject.HeatingSystem = extraData.HeatingSystem.Value;
                    anyChange = true;
                }

                if (realityObject.NumberApartments == null)
                {
                    switch (realityObject.TypeHouse)
                    {
                        case TypeHouse.ManyApartments:
                            realityObject.NumberApartments = extraData.NumberApartmentsMkd;
                            anyChange = true;
                            break;

                        case TypeHouse.SocialBehavior:
                            realityObject.NumberApartments = extraData.NumberApartmentsHostel;
                            anyChange = true;
                            break;
                    }
                }
            }

            return anyChange;
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

        private string SaveData()
        {
            var message = string.Empty;

            LogImport.CountAddedRows = 0;
            LogImport.CountChangedRows = 0;

            using (var transaction = Container.Resolve<IDataTransaction>())
            {
                try
                {
                    foreach (var foundRealtyObject in _foundRealtyObjects.Where(x => x.Value != null))
                    {
                        var realityObj = RealityObjectRepository.Load(foundRealtyObject.Key);
                        var anyChange = Apply(realityObj, foundRealtyObject.Value);
                        if (anyChange)
                        {
                            RealityObjectRepository.Update(realityObj);
                        }

                        anyChange |= SaveRealityObjectLand(realityObj, foundRealtyObject.Value, true);

                        if (anyChange)
                        {
                            LogImport.Info(importHeader, string.Format("ID_MKD: {0}, Адрес: {1}. Дом обновлен", realityObj.FederalNum, realityObj.Address));
                            LogImport.CountChangedRows++;
                        }
                        else
                        {
                            LogImport.Info(importHeader, string.Format("ID_MKD: {0}, Адрес: {1}. Актуальный дом", realityObj.FederalNum, realityObj.Address));
                        }
                    }

                    if (createObjects)
                    {
                        // Если в файле есть дублирующиеся дома, загружать последний.
                        _realtyObjectsToCreate = _realtyObjectsToCreate
                            .GroupBy(x => new { x.CodeKladrStreet, x.House, x.Housing })
                            .Select(x => x.Select(y => y).Last())
                            .ToList();

                        foreach (var realtyObjectToCreate in _realtyObjectsToCreate.Where(x => x != null))
                        {
                            var msg = string.Empty;

                            var objectToCreate = realtyObjectToCreate;

                            if (objectToCreate.FiasAddressId == null && objectToCreate.FiasAddress == null)
                            {
                                LogImport.Warn(
                                    importHeader,
                                    string.Format(
                                        "ID_MKD: {0}. Объект не создан. В ФИАС не удалось найти адреc, соответствующий KLADR_KOD_STREET: {1}, NUM: {2}, KORPUS: {3}",
                                        objectToCreate.FederalNumber,
                                        objectToCreate.CodeKladrStreet,
                                        objectToCreate.House,
                                        objectToCreate.Housing));

                                continue;
                            }

                            FiasAddress fiasAddress;
                            if (objectToCreate.FiasAddressId.HasValue)
                            {
                                fiasAddress = FiasAddressRepository.Load(objectToCreate.FiasAddressId.Value);
                            }
                            else
                            {
                                if (this.ValidateEntity(objectToCreate.FiasAddress, ref msg))
                                {
                                    FiasAddressRepository.Save(objectToCreate.FiasAddress);
                                }
                                fiasAddress = objectToCreate.FiasAddress;
                            }

                            var municipality = GetMunicipality(fiasAddress);

                            if (municipality == null)
                            {
                                LogImport.Warn(importHeader, string.Format("ID_MKD: {0}. Объект не создан. Не удалось определить муниципальное образование", objectToCreate.FederalNumber));

                                continue;
                            }
 
                            var realityObj = new RealityObject
                                {
                                    FiasAddress = fiasAddress,
                                    Municipality = municipality,
                                    Address = GetAddressForMunicipality(municipality, fiasAddress)
                                };

                            Apply(realityObj, objectToCreate);

                            this.ValidateEntity(municipality, ref msg);
                            this.ValidateEntity(realityObj, ref msg);

                            if (!msg.IsEmpty())
                            {
                                LogImport.Warn(importHeader, string.Format("ID_MKD: {0} Дополнительная информация: {1}", objectToCreate.FederalNumber, msg));
                                continue;
                            }

                            RealityObjectRepository.Save(realityObj);
                            SaveRealityObjectLand(realityObj, objectToCreate);
                            LogImport.Info(importHeader, string.Format("ID_MKD: {0}, Адрес: {1}. Дом создан", realityObj.FederalNum, realityObj.Address));
                            LogImport.CountAddedRows++;
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
                        LogImport.Error(importHeader, "Произошла неизвестная ошибка. Обратитесь к администратору.");

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

        private bool SaveRealityObjectLand(RealityObject realityObject, FundRealtyObjectRecord record, bool checkForExistance = false)
        {
            var anyChange = false;

            if (string.IsNullOrEmpty(record.CadastreNumber))
            {
                return anyChange;
            }

            bool needToCreate = !checkForExistance;

            DateTime? cadastrDate = null;
            if (record.CadastrDate.Trim().Length == 4)
            {
                var value = record.CadastrDate.ToInt();
                if (value > 0)
                {
                    cadastrDate = new DateTime(value, 1, 1);
                }
            }
            else
            {
                DateTime date;
                if (DateTime.TryParse(record.CadastrDate, out date))
                {
                    cadastrDate = date;
                }
            }

            if (checkForExistance)
            {
                if (_roLandDict.ContainsKey(realityObject.Id) && _roLandDict[realityObject.Id].Any(x => x.CadastrNumber == record.CadastreNumber))
                {
                    var existinfRealtyObjectLand = _roLandDict[realityObject.Id].First(x => x.CadastrNumber == record.CadastreNumber);

                    var needToUpdate = false;
                    if (string.IsNullOrEmpty(existinfRealtyObjectLand.DocumentName) && !string.IsNullOrEmpty(record.CadastrDocName))
                    {
                        existinfRealtyObjectLand.DocumentName = record.CadastrDocName;
                        needToUpdate = true;
                    }

                    if (existinfRealtyObjectLand.DateLastRegistration == null && cadastrDate != null)
                    {
                        existinfRealtyObjectLand.DateLastRegistration = cadastrDate;
                        needToUpdate = true;
                    }

                    if (needToUpdate)
                    {
                        var realtyObjectLand = RealityObjectLandRepository.Load(existinfRealtyObjectLand.Id);

                        realtyObjectLand.DateLastRegistration = existinfRealtyObjectLand.DateLastRegistration;
                        realtyObjectLand.DocumentName = existinfRealtyObjectLand.DocumentName;

                        RealityObjectLandRepository.Update(realtyObjectLand);
                    }

                    anyChange = needToUpdate;
                }
                else
                {
                    needToCreate = true;
                    anyChange = true;
                }
            }

            if (needToCreate)
            {
                var realtyObjectLand = new RealityObjectLand
                    {
                        RealityObject = realityObject,
                        CadastrNumber = record.CadastreNumber,
                        DocumentName = record.CadastrDocName,
                        DateLastRegistration = cadastrDate
                    };

                RealityObjectLandRepository.Save(realtyObjectLand);
            }

            return anyChange;
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
            if (LogImportManager == null)
            {
                throw new Exception("Не найдена реализация интерфейса ILogImportManager");
            }

            LogImportManager.FileNameWithoutExtention = fileName;
            LogImportManager.UploadDate = DateTime.Now;

            if (LogImport == null)
            {
                throw new Exception("Не найдена реализация интерфейса ILogImport");
            }

            LogImport.SetFileName(fileName);
            LogImport.ImportKey = Key;
        }

        /// <summary>
        /// Валидация сущности согласно маппингу
        /// </summary>
        /// <typeparam name="T">Тип сущности</typeparam>
        /// <param name="entity">Объект</param>
        /// <param name="message">Сообщение</param>
        private bool ValidateEntity<T>(T entity, ref string message)
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
                        message += string.Format("Свойство \"{0}\" не может принимать пустые значения; ", propName);
                        return false;
                    }
                }

                if (st != null)
                {
                    if (st.SqlType.LengthDefined)
                    {
                        if (val != null && val.ToStr().Length > st.SqlType.Length)
                        {
                            message += string.Format("Значение свойства \"{0}\" не может быть длиннее {1}; ",
                                propName,
                                st.SqlType.Length);
                            return false;
                        }
                    }
                }

                ++i;
            }

            return true;
        }

        private void ImportTechPassport(FileData zipFile, bool replaceData)
        {
            var techPassportImport = Container.ResolveAll<ITechPassportImport>();

            if (techPassportImport.Any())
            {
                techPassportImport.First().Import(zipFile, LogImport, replaceData);
            }
        }

        private MemoryStream GetFileMemoryStream(byte[] data, string filePrefix, string fileExtension, out bool success)
        {
            var result = new MemoryStream();
            success = false;

            using (var zipFile = ZipFile.Read(new MemoryStream(data)))
            {
                var zipEntry = zipFile.FirstOrDefault(x => x.FileName.Contains(filePrefix) && x.FileName.EndsWith(fileExtension));
                if (zipEntry != null)
                {
                    zipEntry.Extract(result);
                    result.Seek(0, SeekOrigin.Begin);
                    success = true;
                }
            }

            return result;
        }

        private void ExtraDataImport(FileData zipFile)
        {
            var extraDataImporters = Container.ResolveAll<IExtraDataImport>();

            foreach (var extraDataImporter in extraDataImporters)
            {
                bool successfullyUnzipped;
                using (var memoryStream = GetFileMemoryStream(zipFile.Data, extraDataImporter.Code, "csv", out successfullyUnzipped))
                {
                    if (!successfullyUnzipped)
                    {
                        LogImport.Warn(extraDataImporter.Code, string.Format("Не удалось распаковать файл {0} из архива", extraDataImporter.Code));
                    }
                    else
                    {
                        memoryStream.Seek(0, SeekOrigin.Begin);

                        extraDataImporter.Import(memoryStream, _extraDataByFederalNumDict);
                    }
                }
            }
        }
    }
}
