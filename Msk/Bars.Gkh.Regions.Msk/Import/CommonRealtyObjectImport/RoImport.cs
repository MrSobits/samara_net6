namespace Bars.Gkh.Regions.Msk.Import.CommonRealtyObjectImport
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
    using Bars.B4.IoC;
    using Bars.B4.Modules.FIAS;
    using Bars.B4.Modules.States;
    using Bars.B4.Utils;
    using Bars.Gkh.Config;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Entities.CommonEstateObject;
    using Bars.Gkh.Entities.Dicts;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Enums.Import;
    using Bars.Gkh.Import;
    using Bars.Gkh.Overhaul.Entities;
    using Bars.Gkh.Overhaul.Import.CommonRealtyObjectImport;
    using Bars.GkhExcel;
    using Bars.B4.Logging;
    using Bars.B4.Modules.Tasks.Common.Service;
    using Bars.Gkh.Utils;

    using Castle.Windsor;
    using Gkh.Entities.CommonEstateObject;
    using Gkh.Entities.Dicts;
    using Gkh.Import.Impl;

    using NHibernate.Type;

    public class RoImport : GkhImportBase
    {
        public static string Id = MethodBase.GetCurrentMethod().DeclaringType.FullName;

        #region Properties

        public virtual IWindsorContainer Container { get; set; }

        public override string Key
        {
            get { return Id; }
        }

        public override string CodeImport
        {
            get { return "CommonRealtyObjectImport"; }
        }

        public override string Name
        {
            get { return "Импорт жилых домов (универсальный)"; }
        }

        public override string PossibleFileExtensions
        {
            get { return "xls"; }
        }

        public override string PermissionName
        {
            get { return "Import.CommonRealtyObjectImport.View"; }
        }

        public ISessionProvider SessionProvider { get; set; }

        public IFiasRepository FiasRepository { get; set; }

        public IRepository<RealityObject> RealityObjectRepository { get; set; }

        public IRepository<FiasAddress> FiasAddressRepository { get; set; }

        public IRepository<StructuralElement> StructuralElementRepository { get; set; }

        public IRepository<RealityObjectStructuralElement> RealityObjectStructuralElementRepository { get; set; }

        public ILogImportManager LogManager { get; set; }

        #endregion Properties

        #region Dictionaries

        private Dictionary<int, Log> logDict;

        private Dictionary<long, Municipality> municipalitiesDict;

        private Dictionary<string, List<RealtyObjectAddress>> realityObjectsByFiasGuidDict;

        private Dictionary<string, KeyValuePair<string, long>> fiasIdByMunicipalityNameDict;

        private Dictionary<string, long> structuralElementDict;

        private Dictionary<long, string> structuralElementNameDict;

        private Dictionary<string, CapitalGroup> capitalGroupDict;

        private Dictionary<string, RealEstateType> realEstateTypeDict;

        private Dictionary<long, Dictionary<long, long>> realtyObjectStructuralElementsDict;

        private readonly Dictionary<string, IDinamicAddress> dynamicAddressDictByMixedGuid = new Dictionary<string, IDinamicAddress>();

        private readonly Dictionary<long, Record> foundRealtyObjects = new Dictionary<long, Record>();

        private readonly Dictionary<string, int> headersDict = new Dictionary<string, int>();

        private readonly Dictionary<string, StructElementHeader> structElementHeadersDict = new Dictionary<string, StructElementHeader>();

        private HashSet<long> knownRobjectIds; 

        #endregion Dictionaries

        private List<Record> realtyObjectsToCreate = new List<Record>();

        private ILogImport logImport;

        private bool createObjects;
        private bool allowEmptyStreets;

        private bool overrideExistRecords;
        private bool useAdminOkrug;

        private FiasHelper fiasHelper;

        private State realityObjectDefaultState;

        private State realityObjectStructuralElementDefaultState;

        private void InitDefaultStates()
        {
            var statePrivder = Container.Resolve<IStateProvider>();

            using (Container.Using(statePrivder))
            {
                var emptyRealityObject = new RealityObject();

                statePrivder.SetDefaultState(emptyRealityObject);

                realityObjectDefaultState = emptyRealityObject.State;

                var emptyRealityObjectStructuralElement = new RealityObjectStructuralElement();

                statePrivder.SetDefaultState(emptyRealityObjectStructuralElement);

                realityObjectStructuralElementDefaultState = emptyRealityObjectStructuralElement.State;

                logDict = new Dictionary<int, Log>();
            }
        }

        public ImportResult Import(BaseParams baseParams)
        {
            var file = baseParams.Files["FileImport"];

            createObjects = baseParams.Params.GetAs("createObjectsOnImport", false);
            allowEmptyStreets = baseParams.Params.GetAs("allowEmptyStreets", false);
            overrideExistRecords = baseParams.Params.GetAs("overridExistRecsOnImport", false);
            useAdminOkrug = Container.Resolve<IGkhParams>().GetParams().GetAs<bool>("UseAdminOkrug");

            InitDefaultStates();

            InitLog(file.FileName);

            InitDictionaries();

            var message = ProcessData(file.Data);

            if (!string.IsNullOrEmpty(message))
            {
                return new ImportResult(StatusImport.CompletedWithError, message);
            }

            message = SaveData();

            WriteLogs();

            // Намеренно закрываем текущую сессию, иначе при каждом коммите транзакции
            // ранее измененные дома вызывают каскадирование ФИАС
            Container.Resolve<ISessionProvider>().CloseCurrentSession();

            LogManager.Add(file, logImport);
            LogManager.Save();

            message += LogManager.GetInfo();
            var status = LogManager.CountError > 0 ? StatusImport.CompletedWithError : (LogManager.CountWarning > 0 ? StatusImport.CompletedWithWarning : StatusImport.CompletedWithoutError);
            return new ImportResult(status, message, string.Empty, LogManager.LogFileId);
        }

        public bool Validate(BaseParams baseParams, out string message)
        {
            message = null;

            var extention = baseParams.Files["FileImport"].Extention;

            var fileExtentions = PossibleFileExtensions.Contains(",") ? PossibleFileExtensions.Split(',') : new[] { PossibleFileExtensions };
            if (fileExtentions.All(x => x != extention))
            {
                message = string.Format("Необходимо выбрать файл с допустимым расширением: {0}", PossibleFileExtensions);
                return false;
            }

            return true;
        }

        private void InitDictionaries()
        {
            fiasHelper = new FiasHelper(Container);

            var fiasRepo = Container.Resolve<IRepository<Fias>>();
            var capitalGroupDomain = Container.ResolveDomain<CapitalGroup>();
            var realEstateTypeDomain = Container.ResolveDomain<RealEstateType>();
            var muDomain = Container.ResolveDomain<Municipality>();

            try
            {
                realityObjectsByFiasGuidDict = fiasRepo.GetAll()
                    .Join(FiasAddressRepository.GetAll(),
                        x => x.AOGuid,
                        y => y.StreetGuidId,
                        (a, b) => new { fias = a, fiasAddress = b })
                    .Join(RealityObjectRepository.GetAll(),
                        x => x.fiasAddress.Id,
                        y => y.FiasAddress.Id,
                        (c, d) => new { c.fias, c.fiasAddress, realityObject = d })
                    .Where(x => x.fias.ActStatus == FiasActualStatusEnum.Actual)
                    .Select(x => new RealtyObjectAddress
                    {
                        AoGuid = x.fias.AOGuid,
                        Id = x.fiasAddress.Id,
                        House = x.fiasAddress.House,
                        Housing = x.fiasAddress.Housing,
                        Building = x.fiasAddress.Building,
                        RealityObjectId = x.realityObject.Id,
                        MunicipalityId = x.realityObject.Municipality.Id,
                        Liter = x.fiasAddress.Letter
                    })
                    .AsEnumerable()
                    .Where(x => !string.IsNullOrWhiteSpace(x.AoGuid))
                    .GroupBy(x => x.AoGuid)
                    .ToDictionary(x => x.Key, x => x.ToList());

                capitalGroupDict = capitalGroupDomain.GetAll()
                    .Where(x => x.Code != null && x.Code != "")
                    .ToList()
                    .GroupBy(x => x.Name)
                    .ToDictionary(x => x.Key, y => y.First());


                realEstateTypeDict = realEstateTypeDomain.GetAll()
                    .Where(x => x.Code != null && x.Code != "")
                    .ToList()
                    .GroupBy(x => x.Code)
                    .ToDictionary(x => x.Key, y => y.First());

                var municipalities = muDomain.GetAll().ToArray();

                municipalitiesDict = municipalities.ToDictionary(x => x.Id);

                fiasIdByMunicipalityNameDict = municipalities
                    .Where(x => Name != null)
                    .Select(x => new { x.Name, x.FiasId, x.Id })
                    .AsEnumerable()
                    .GroupBy(x => x.Name.ToUpper())
                    .ToDictionary(
                        x => x.Key,
                        x =>
                        {
                            var first = x.First();
                            return new KeyValuePair<string, long>(first.FiasId, first.Id);
                        });

                knownRobjectIds = RealityObjectRepository.GetAll().Select(x => x.Id).ToHashSet();
            }
            finally
            {
                Container.Release(fiasRepo);
                Container.Release(capitalGroupDomain);
                Container.Release(realEstateTypeDomain);
                Container.Release(muDomain);
            }
        }

        private string InitHeader(GkhExcelCell[] data)
        {
            headersDict["ID"] = -1;
            headersDict["ADDRESS"] = -1;
            headersDict["OKR"] = -1;
            headersDict["MU"] = -1;
            headersDict["KINDCITY"] = -1;
            headersDict["CITY"] = -1;
            headersDict["KINDSTREET"] = -1;
            headersDict["STREET"] = -1;
            headersDict["KLADRSTREETKOD"] = -1;
            headersDict["KOD"] = -1;
            headersDict["HOUSENUM"] = -1;
            headersDict["LITER"] = -1;
            headersDict["KORPUS"] = -1;
            headersDict["BUILDING"] = -1;
            headersDict["HOUSINGVIEW"] = -1;
            headersDict["HOUSECONDITION"] = -1;
            headersDict["STARTUPDAY"] = -1;
            headersDict["BUILTYEAR"] = -1;
            headersDict["TOTALAREA"] = -1;
            headersDict["LIVAREA"] = -1;
            headersDict["MINFLOORS"] = -1;
            headersDict["MAXFLOORS"] = -1;
            headersDict["ENTRANCESNUM"] = -1;
            headersDict["RESIDENTSNUM"] = -1;
            headersDict["APARTLIVAREA"] = -1;
            headersDict["TOTALDEPRECIATION"] = -1;
            headersDict["PRIVDATE"] = -1;
            headersDict["PRIVATEPROPERTYAREA"] = -1;
            headersDict["MUNICIPALPROPERTYAREA"] = -1;

            headersDict["GROUPOFCAP"] = -1;
            headersDict["FLATCOUNT"] = -1;
            headersDict["ISNOTINVOLVEDCR"] = -1;
            headersDict["HOUSETYPE"] = -1;
            headersDict["LIVNOTLIVAREA"] = -1;

            for (var index = 0; index < data.Length; ++index)
            {
                var header = data[index].Value.ToUpper();
                if (headersDict.ContainsKey(header))
                {
                    headersDict[header] = index;
                }
                else if (header.Contains('_'))
                {
                    var arr = header.Split('_');

                    if (arr.Length > 1)
                    {
                        var code = arr[1];

                        var structElementHeader = new StructElementHeader();

                        if (structElementHeadersDict.ContainsKey(code))
                        {
                            structElementHeader = structElementHeadersDict[code];
                        }
                        else
                        {
                            structElementHeadersDict[code] = structElementHeader;
                        }

                        switch (arr[0].ToUpper())
                        {
                            case "VOLUME":
                                structElementHeader.Volume = index;
                                break;

                            case "WEARPERCENT":
                                structElementHeader.Wearout = index;
                                break;

                            case "INSTOROVERHLYEAR":
                                structElementHeader.LastOverhaulYear = index;
                                break;
                        }
                    }
                }
            }

            var structElemList = StructuralElementRepository.GetAll()
                .Where(x => x.Code != null)
                .Select(x => new { x.Code, x.Id, x.Name, grName = x.Group.Name, coeName = x.Group.CommonEstateObject.Name })
                .AsEnumerable()
                .Where(x => structElementHeadersDict.Keys.Contains(x.Code))
                .ToArray();

            var undefinedStructElCodes = structElementHeadersDict.Keys.Where(x => !structElemList.Select(y => y.Code).Contains(x)).ToList();

            if (undefinedStructElCodes.Any())
            {
                return "Следующие коды конструктивных элементов из файла отсутствуют в системе: " + string.Join(", ", undefinedStructElCodes);
            }

            var duplicateStructElByCode = structElemList.GroupBy(x => x.Code).Where(x => x.Count() > 1).ToList();
            if (duplicateStructElByCode.Any())
            {
                var dubles = string.Empty;
                duplicateStructElByCode.ForEach(x => dubles += " <br/>" + x.Key + ": " + string.Join(", ", x.Select(y => string.Format("{0} - {1} - {2}", y.coeName, y.grName, y.Name))));

                return "Коды конструктивных элементов системы не уникальны:" + dubles;
            }

            // Словарь конструктивных характеристик по Коду
            structuralElementDict = structElemList.ToDictionary(x => x.Code, x => x.Id);

            // Словарь наименований конструктивных характеристик
            structuralElementNameDict = structElemList.ToDictionary(x => x.Id, x => x.Name);

            // Словарь конструктивных характеристик жилых домов
            realtyObjectStructuralElementsDict = RealityObjectStructuralElementRepository.GetAll()
                .Where(x => x.RealityObject != null)
                .Where(x => x.StructuralElement != null)
                .Select(x => new
                {
                    roId = x.RealityObject.Id,
                    seId = x.StructuralElement.Id,
                    roStrElemId = x.Id
                })
                .AsEnumerable()
                .Where(x => structuralElementNameDict.Keys.Contains(x.seId))
                .GroupBy(x => x.roId)
                .ToDictionary(
                    x => x.Key,
                    x => x.GroupBy(y => y.seId)
                          .ToDictionary(
                            y => y.Key,
                            y => y.First().roStrElemId));

            return string.Empty;
        }

        public void InitLog(string fileName)
        {
            LogManager = Container.Resolve<ILogImportManager>();
            if (LogManager == null)
            {
                throw new Exception("Не найдена реализация интерфейса ILogImportManager");
            }

            LogManager.FileNameWithoutExtention = fileName;
            LogManager.UploadDate = DateTime.Now;

            logImport = Container.ResolveAll<ILogImport>().FirstOrDefault(x => x.Key == MainLogImportInfo.Key);
            if (logImport == null)
            {
                throw new Exception("Не найдена реализация интерфейса ILogImport");
            }

            logImport.SetFileName(fileName);
            logImport.ImportKey = Key;
        }

        #region Fias Helpers

        private FiasAddress CreateFiasAddress(DynamicAddress address, string house, string housing, string building)
        {
            if (address == null)
            {
                return null;
            }

            var addressName = new StringBuilder(address.AddressName.TrimEnd().TrimEnd(','));

            if (!string.IsNullOrEmpty(house))
            {
                addressName.Append(", д. ");
                addressName.Append(house);

                if (!string.IsNullOrEmpty(housing))
                {
                    addressName.Append(", корп. ");
                    addressName.Append(housing);
                }

                if (!string.IsNullOrEmpty(building))
                {
                    addressName.Append(", секц. ");
                    addressName.Append(building);
                }
            }

            var fiasAddress = new FiasAddress
            {
                AddressGuid = address.AddressGuid,
                AddressName = addressName.ToString(),
                PostCode = address.PostCode,
                StreetGuidId = address.GuidId,
                StreetName = address.Name,
                StreetCode = address.Code,
                House = house,
                Housing = housing,
                Building = building,

                PlaceAddressName = (string.IsNullOrEmpty(address.Name) ? address.AddressName : address.AddressName.Replace(address.Name, string.Empty)).Trim().Trim(','),
                PlaceGuidId = address.ParentGuidId,
                PlaceName = address.ParentName,
                PlaceCode = address.PlaceCode
            };

            return fiasAddress;
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

            if (dynamicAddressDictByMixedGuid.ContainsKey(mo.FiasId))
            {
                dinamicAddress = dynamicAddressDictByMixedGuid[mo.FiasId];
            }
            else
            {
                dinamicAddress = FiasRepository.GetDinamicAddress(mo.FiasId);
                dynamicAddressDictByMixedGuid[mo.FiasId] = dinamicAddress;
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

        #endregion Fias Helpers

        private string ProcessData(byte[] fileData)
        {
            Action<Record> createRealtyObject = record =>
            {
                if (createObjects)
                {
                    record.FiasAddress = CreateFiasAddress(record.Address, record.House, record.Housing, record.Building);

                    realtyObjectsToCreate.Add(record);
                }
                else
                {
                    var text = string.Format(
                            "Не удалось найти дом, соответствующий адресу {0} или KLADRSTREETKOD: {1}; NUM: {2}, KORPUS: {3}",
                            record.LocalityName + " " + record.StreetName,
                            record.CodeKladrStreet,
                            record.House,
                            record.Housing);

                    AddLog(record.RowNumber, text, ResultType.No);
                }
            };

            using (var excel = Container.Resolve<IGkhExcelProvider>("ExcelEngineProvider"))
            {
                if (excel == null)
                {
                    throw new Exception("Не найдена реализация интерфейса IGkhExcelProvider");
                }

                using (var memoryStreamFile = new MemoryStream(fileData))
                {
                    memoryStreamFile.Seek(0, SeekOrigin.Begin);

                    excel.Open(memoryStreamFile);

                    var rows = excel.GetRows(0, 0);

                    var message = InitHeader(rows.First());

                    if (!string.IsNullOrEmpty(message))
                    {
                        return message;
                    }

                    for (var i = 1; i < rows.Count; ++i)
                    {
                        var record = ProcessLine(rows[i], i + 1);

                        if (!record.isValidRecord)
                        {
                            continue;
                        }

                        if (record.RoId > 0)
                        {
                            foundRealtyObjects[record.RoId] = record;

                            if (!overrideExistRecords)
                            {
                                AddLog(record.RowNumber, "По данному дому уже есть данные. Данные не будут загружены.");
                            }

                            continue;
                        }

                        // Проверяем есть ли дома на данной улице
                        //if (!realityObjectsByFiasGuidDict.ContainsKey(record.Address.GuidId))
                        var guid = record.Address.GuidId == null && allowEmptyStreets ? record.Address.ParentGuidId : record.Address.GuidId;
                        if (!realityObjectsByFiasGuidDict.ContainsKey(guid))
                        {
                            createRealtyObject(record);
                        }
                        else
                        {
                            // Составляем список домов на данной улице (!) с проверкой привязки МО (это не бред, после разделения одного МО многое может произойти)
                            var existingRealityObjects = realityObjectsByFiasGuidDict[guid].Where(x => x.MunicipalityId == record.MunicipalityId).ToList();

                            if (!existingRealityObjects.Any())
                            {
                                createRealtyObject(record);
                            }
                            else
                            {
                                var result = existingRealityObjects.Where(x => x.House == record.House).ToList();

                                result = string.IsNullOrEmpty(record.Housing)
                                    ? result.Where(x => string.IsNullOrEmpty(x.Housing)).ToList()
                                    : result.Where(x => x.Housing == record.Housing).ToList();

                                result = string.IsNullOrEmpty(record.Building)
                                    ? result.Where(x => string.IsNullOrEmpty(x.Building)).ToList()
                                    : result.Where(x => x.Building == record.Building).ToList();

                                result = string.IsNullOrEmpty(record.Liter)
                                    ? result.Where(x => string.IsNullOrEmpty(x.Liter)).ToList()
                                    : result.Where(x => x.Liter == record.Liter).ToList();

                                if (result.Count == 0)
                                {
                                    createRealtyObject(record);
                                }
                                else if (result.Count > 1)
                                {
                                    AddLog(record.RowNumber, "В системе найдено несколько домов, соответствующих записи.", ResultType.No);
                                }
                                else
                                {
                                    // Найден ровно 1 дом
                                    // Если в файле есть дублирующиеся дома, загружать последний номер (т.е. перезапишем существующую запись словаря)
                                    foundRealtyObjects[result.First().RealityObjectId] = record;

                                    if (!overrideExistRecords)
                                    {
                                        AddLog(record.RowNumber, "По данному дому уже есть данные. Данные не будут загружены.");
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return string.Empty;
        }

        /// <summary>
        /// Обработка строки импорта
        /// </summary>
        private Record ProcessLine(GkhExcelCell[] data, int rowNumber)
        {
            var record = new Record { isValidRecord = false, RowNumber = rowNumber };

            if (data.Length <= 1)
            {
                return record;
            }

            var robjectId = GetValue(data, "ID").ToLong();
            if (robjectId > 0 && knownRobjectIds.Contains(robjectId))
            {
                record.RoId = robjectId;
            }

            if (record.RoId == 0)
            {
                var municipalityName = GetValue(data, "MU").Trim().ToUpper();
                if (string.IsNullOrEmpty(municipalityName))
                {
                    AddLog(record.RowNumber, "Не задано муниципальное образование.", ResultType.No);
                    return record;
                }

                record.LocalityName = Simplified(GetValue(data, "CITY") + " " + GetValue(data, "KINDCITY"));
                record.StreetName = Simplified(GetValue(data, "STREET") + " " + GetValue(data, "KINDSTREET"));
                record.CodeKladrStreet = GetValue(data, "KLADRSTREETKOD");
                record.House = Simplified(GetValue(data, "HOUSENUM"));
                record.Liter = Simplified(GetValue(data, "LITER"));
                record.GkhCode = Simplified(GetValue(data, "KOD"));

                if (string.IsNullOrEmpty(record.House))
                {
                    AddLog(record.RowNumber, "Не задан номер дома.", ResultType.No);
                    return record;
                }

                if (!fiasIdByMunicipalityNameDict.ContainsKey(municipalityName))
                {
                    var errText = string.Format("В справочнике муниципальных образований не найдена запись: {0}", GetValue(data, "MU").Trim());
                    AddLog(record.RowNumber, errText, ResultType.No);
                    return record;
                }

                var municipality = fiasIdByMunicipalityNameDict[municipalityName];
                var fiasGuid = municipality.Key;

                if (string.IsNullOrWhiteSpace(fiasGuid))
                {
                    AddLog(record.RowNumber, "Муниципальное образование не привязано к ФИАС", ResultType.No);
                    return record;
                }

                if (!fiasHelper.HasBranch(fiasGuid))
                {
                    AddLog(record.RowNumber, "В структуре ФИАС не найдена актуальная запись для муниципального образования.", ResultType.No);
                    return record;
                }

                var faultReason = string.Empty;
                DynamicAddress address;

                if (RecordHasValidCodeKladrStreet(record))
                {
                    if (!fiasHelper.FindInBranchByKladr(fiasGuid, record.CodeKladrStreet, ref faultReason, out address))
                    {
                        AddLog(record.RowNumber, faultReason, ResultType.DoesNotMeetFias);
                        return record;
                    }
                }
                else
                {
                    if (!fiasHelper.FindInBranch(fiasGuid, record.LocalityName, record.StreetName, ref faultReason, out address, allowEmptyStreets))
                    {
                        AddLog(record.RowNumber, faultReason, ResultType.DoesNotMeetFias);
                        return record;
                    }
                }

                record.District = GetValue(data, "OKR");
                if (useAdminOkrug && string.IsNullOrEmpty(record.District))
                {
                    AddLog(record.RowNumber, string.Format("Не заполнен административный округ. Адрес: {0}", address.AddressName), ResultType.No);
                    return record;
                }

                if (string.IsNullOrEmpty(record.GkhCode))
                {
                    AddLog(record.RowNumber, string.Format("Отсутствует код дома. Адрес: {0}", address.AddressName), ResultType.YesButWithErrors);
                }

                if (string.IsNullOrEmpty(record.CodeKladrStreet))
                {
                    AddLog(record.RowNumber, string.Format("Отсутствует код дома КЛАДР. Адрес: {0}", address.AddressName), ResultType.YesButWithErrors);
                }

                record.Address = address;
                record.MunicipalityId = municipality.Value;
                record.Housing = Simplified(GetValue(data, "KORPUS"));
                record.Building = Simplified(GetValue(data, "BUILDING"));
            }

            record.StrAddress = GetValue(data, "ADDRESS");
            record.TypeHouse = GetValue(data, "HOUSINGVIEW");
            record.DateCommissioning = GetValue(data, "STARTUPDAY");
            record.BuildYear = GetValue(data, "BUILTYEAR");
            record.AreaMkd = GetValue(data, "TOTALAREA");
            record.AreaLivingNotLivingMkd = GetValue(data, "LIVNOTLIVAREA");
            record.AreaLiving = GetValue(data, "LIVAREA");
            record.Floors = GetValue(data, "MINFLOORS");
            record.MaximumFloors = GetValue(data, "MAXFLOORS");
            record.NumberEntrances = GetValue(data, "ENTRANCESNUM");
            record.CapitalGroup = GetValue(data, "GROUPOFCAP");
            record.NumberLiving = GetValue(data, "RESIDENTSNUM");
            record.NumberApartments = GetValue(data, "FLATCOUNT");
            record.PhysicalWear = GetValue(data, "TOTALDEPRECIATION");
            record.PrivatizationDateFirstApartment = GetValue(data, "PRIVDATE");
            record.AreaOwned = GetValue(data, "PRIVATEPROPERTYAREA");
            record.AreaMunicipalOwned = GetValue(data, "MUNICIPALPROPERTYAREA");
            record.ConditionHouse = GetValue(data, "HOUSECONDITION");
            record.IsNotInvolvedCr = GetValue(data, "ISNOTINVOLVEDCR");
            record.RealEstTypeCode = GetValue(data, "HOUSETYPE");
            record.GkhCode = GetValue(data, "KOD");

            record.StructElementDict = GetStructElements(data);

            record.isValidRecord = true;

            return record;
        }

        private bool RecordHasValidCodeKladrStreet(Record record)
        {
            if (string.IsNullOrWhiteSpace(record.CodeKladrStreet))
            {
                record.CodeKladrStreet = null;
                return false;
            }

            var codeLength = record.CodeKladrStreet.Length;

            if (codeLength < 15)
            {
                record.CodeKladrStreet = null;
                return false;
            }

            if (codeLength > 17)
            {
                record.CodeKladrStreet = record.CodeKladrStreet.Substring(0, 17);
            }
            else if (codeLength < 17)
            {
                record.CodeKladrStreet = record.CodeKladrStreet + (codeLength == 15 ? "00" : "0");
            }

            return fiasHelper.HasValidStreetKladrCode(record.CodeKladrStreet);
        }

        private string GetValue(GkhExcelCell[] data, string field)
        {
            var result = string.Empty;

            if (headersDict.ContainsKey(field))
            {
                var index = headersDict[field];
                if (data.Length > index && index > -1)
                {
                    result = data[index].Value;
                }
            }

            return result.Trim();
        }

        private Dictionary<string, StructElementRecord> GetStructElements(GkhExcelCell[] data)
        {
            var result = new Dictionary<string, StructElementRecord>();

            foreach (var header in structElementHeadersDict)
            {
                var structElement = new StructElementRecord();

                if (data.Length > header.Value.LastOverhaulYear)
                {
                    var value = data[header.Value.LastOverhaulYear].Value;

                    if (!string.IsNullOrWhiteSpace(value))
                    {
                        structElement.LastOverhaulYear = value;
                        structElement.IsValid = true;
                    }
                }

                if (data.Length > header.Value.Volume)
                {
                    var value = data[header.Value.Volume].Value;

                    if (!string.IsNullOrWhiteSpace(value))
                    {
                        structElement.Volume = value;
                        structElement.IsValid = true;
                    }
                }

                if (data.Length > header.Value.Wearout)
                {
                    var value = data[header.Value.Wearout].Value;

                    if (!string.IsNullOrWhiteSpace(value))
                    {
                        structElement.Wearout = value;
                        structElement.IsValid = true;
                    }
                }

                if (structElement.IsValid)
                {
                    result[header.Key] = structElement;
                }
            }

            return result;
        }

        private void Apply(RealityObject realityObject, Record record)
        {
            if (realityObject.State == null)
            {
                realityObject.State = realityObjectDefaultState;
            }

            Action<string> warnLog = text => AddLog(record.RowNumber, text, ResultType.YesButWithErrors);

            if (!string.IsNullOrEmpty(record.TypeHouse))
            {
                switch (record.TypeHouse.ToUpper())
                {
                    case "БЛОКИРОВАННОЙ ЗАСТРОЙКИ":
                        realityObject.TypeHouse = TypeHouse.BlockedBuilding;
                        break;

                    case "ЧАСТНЫЙ ДОМ":
                        realityObject.TypeHouse = TypeHouse.Individual;
                        break;

                    case "МКД": //МНОГОКВАРТИРНЫЙ
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

            if (!string.IsNullOrEmpty(record.IsNotInvolvedCr))
            {
                switch (record.IsNotInvolvedCr.ToUpper())
                {
                    case "НЕТ":
                        realityObject.IsNotInvolvedCr = true;
                        break;
                    case "ДА":
                        realityObject.IsNotInvolvedCr = false;
                        break;
                }
            }

            var conditionHouse = ConditionHouse.Serviceable;
            if (!string.IsNullOrEmpty(record.ConditionHouse))
            {
                switch (record.ConditionHouse.ToUpper())
                {
                    case "ИСПРАВНЫЙ":
                        conditionHouse = ConditionHouse.Serviceable;
                        break;

                    case "АВАРИЙНЫЙ":
                        conditionHouse = ConditionHouse.Emergency;
                        break;

                    case "ВЕТХИЙ":
                        conditionHouse = ConditionHouse.Dilapidated;
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
            realityObject.ConditionHouse = conditionHouse;

            if (!string.IsNullOrEmpty(record.DateCommissioning))
            {
                if (record.DateCommissioning.Trim().Length == 4)
                {
                    var value = record.DateCommissioning.ToInt();
                    if (value > 0)
                    {
                        realityObject.DateCommissioning = new DateTime(value, 1, 1);
                    }
                    else
                    {
                        warnLog("Дата сдачи в эксплуатацию");
                    }
                }
                else
                {
                    DateTime value;

                    if (DateTime.TryParse(record.DateCommissioning, out value))
                    {
                        realityObject.DateCommissioning = value;
                    }
                    else
                    {
                        warnLog("Дата сдачи в эксплуатацию");
                    }
                }
            }
            else
            {
                warnLog("Дата сдачи в эксплуатацию");
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

            if (!string.IsNullOrEmpty(record.AreaLivingNotLivingMkd))
            {
                realityObject.AreaLivingNotLivingMkd = record.AreaLivingNotLivingMkd.ToDecimal();
            }
            else
            {
                warnLog("Общая площадь жилых и нежилых помещений");
            }

            if (!string.IsNullOrEmpty(record.AreaLiving))
            {
                realityObject.AreaLiving = record.AreaLiving.ToDecimal();
            }
            else
            {
                warnLog("Жилая площадь");
            }

            if (!string.IsNullOrEmpty(record.GkhCode))
            {
                realityObject.GkhCode = record.GkhCode;
            }
            else
            {
                warnLog("Код дома");
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

            if (!string.IsNullOrEmpty(record.CapitalGroup))
            {
                if (capitalGroupDict.ContainsKey(record.CapitalGroup))
                {
                    realityObject.CapitalGroup = capitalGroupDict[record.CapitalGroup];
                }
                else
                {
                    warnLog("Отсутствует указанная группа капитальности в справочнике");
                }
            }
            else
            {
                realityObject.CapitalGroup = null;
                warnLog("Группа капитальности");
            }

            if (!string.IsNullOrEmpty(record.RealEstTypeCode))
            {
                if (realEstateTypeDict.ContainsKey(record.RealEstTypeCode))
                {
                    realityObject.RealEstateType = realEstateTypeDict[record.RealEstTypeCode];
                }
                else
                {
                    warnLog("Отсутствует тип дома с таким кодом");
                }
            }

            if (!string.IsNullOrEmpty(record.NumberLiving))
            {
                realityObject.NumberLiving = record.NumberLiving.ToInt();
            }
            else
            {
                warnLog("Количество проживающих");
            }

            if (!string.IsNullOrEmpty(record.NumberApartments))
            {
                realityObject.NumberApartments = record.NumberApartments.ToInt();
            }
            else
            {
                warnLog("Количество квартир");
            }

            if (!string.IsNullOrEmpty(record.PhysicalWear))
            {
                realityObject.PhysicalWear = record.PhysicalWear.ToInt();
            }
            else
            {
                warnLog("Износ МКД");
            }

            if (!string.IsNullOrEmpty(record.AreaOwned))
            {
                realityObject.AreaOwned = record.AreaOwned.Replace(".", ",").ToDecimal();
            }
            else
            {
                warnLog("Площадь в частной собственности");
            }

            if (!string.IsNullOrEmpty(record.AreaMunicipalOwned))
            {
                realityObject.AreaMunicipalOwned = record.AreaMunicipalOwned.Replace(".", ",").ToDecimal();
            }
            else
            {
                warnLog("Площадь в муниципальной собственности");
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
                        realityObject.HasPrivatizedFlats = false;
                    }
                }
                else
                {
                    DateTime value;

                    if (DateTime.TryParse(record.PrivatizationDateFirstApartment, out value))
                    {
                        realityObject.PrivatizationDateFirstApartment = value;
                        realityObject.HasPrivatizedFlats = true;
                    }
                    else
                    {
                        warnLog("Дата приватизации первого жилого помещения");
                        realityObject.HasPrivatizedFlats = false;
                    }
                }
            }
            else
            {
                warnLog("Дата приватизации первого жилого помещения");
                realityObject.HasPrivatizedFlats = false;
            }

            if (!string.IsNullOrEmpty(record.Liter))
            {
                if (realityObject.FiasAddress != null)
                {
                    realityObject.FiasAddress.Letter = record.Liter;
                    realityObject.Address = CreateAddressField(realityObject.FiasAddress);

                    realityObject.FiasAddress.AddressName = string.IsNullOrEmpty(realityObject.FiasAddress.PlaceAddressName)
                        ? realityObject.Address
                        : realityObject.FiasAddress.PlaceAddressName + ", " + realityObject.Address;
                }
            }

            if (!string.IsNullOrEmpty(record.GkhCode))
            {
                realityObject.UnomCode = record.GkhCode;
            }
            else
            {
                warnLog("Код дома");
            }

            if (!string.IsNullOrEmpty(record.District))
            {
                realityObject.District = record.District;
            }
        }


        private string SaveData()
        {
            var message = string.Empty;

            {
                try
                {
                    if (overrideExistRecords)
                    {
                        SaveDataOverrideExist(foundRealtyObjects);
                    }

                    if (createObjects && realtyObjectsToCreate.Any())
                    {
                        SaveDataCreateObjects(realtyObjectsToCreate);
                    }
                }
                catch (Exception exc)
                {
                    try
                    {
                        logImport.IsImported = false;
                        Container.Resolve<ILogManager>().Error("Импорт", exc);
                        message = "Произошла неизвестная ошибка. Обратитесь к администратору";
                        logImport.Error(Name, "Произошла неизвестная ошибка. Обратитесь к администратору.");
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message, exc);
                    }
                }
            }

            return message;
        }

        /// <summary>
        /// Сохранить, перезаписав существующие данные
        /// </summary>        
        private void SaveDataOverrideExist(Dictionary<long, Record> realityRecordObjects)
        {
            var realityRecordDict = realityRecordObjects
                .Where(x => x.Value != null)
                .ToDictionary(x => x.Key, x => x.Value);

            //получаем список Домов для сохранения
            var realityObjectsDict = RealityObjectRepository
                .GetAll()
                .Where(x => realityRecordDict.Keys.Contains(x.Id))
                .ToDictionary(x => x.Id, x => x);

            //получаем список структурных элементов
            var structuralElementsList = StructuralElementRepository.GetAll().ToList();

            var сonstructiveElementsList = new List<RealityObjectStructuralElement>();
            foreach (var realityRecord in realityRecordDict)
            {
                if (realityObjectsDict.ContainsKey(realityRecord.Key))
                {
                    var realityObject = realityObjectsDict[realityRecord.Key];

                    Apply(realityObject, realityRecord.Value);
                    AddLog(
                        realityRecord.Value.RowNumber,
                        string.Format("Будет обновлен дом по адресу: {0} (оригинальный адрес: {1})", realityObject.Address, realityRecord.Value.StrAddress),
                        ResultType.AlreadyExists);

                    //получаем список Конструктивных элементов дома для сохранения
                    var constructiveElements = GetConstructiveElements(
                        realityObject,
                        realityRecord.Value,
                        structuralElementsList,
                        false);

                    сonstructiveElementsList.AddRange(constructiveElements);
                }
            }

            //Сохраняем            
            SaveObjects(realityObjectsDict.Values.ToList(), сonstructiveElementsList);
        }

        /// <summary>
        /// Сохранить новые записи
        /// </summary>        
        private void SaveDataCreateObjects(List<Record> realityObjects)
        {
            // Если в файле есть дублирующиеся дома, загружать последний.
            realityObjects = realityObjects
                .GroupBy(x => new
                {
                    GuidID = x.Address.GuidId == null && allowEmptyStreets
                      ? x.Address.ParentGuidId : x.Address.GuidId,
                    x.House,
                    x.Housing,
                    x.Building,
                    x.Liter
                })
                .Select(x =>
                {
                    var objects = x.ToList();
                    var last = objects.Last();
                    if (objects.Count > 1)
                    {
                        objects.RemoveAt(objects.Count - 1);
                        var errMessage = string.Format("Дубль строки {0}", last.RowNumber);
                        objects.ForEach(y => AddLog(y.RowNumber, errMessage, ResultType.No));
                    }
                    return last;
                })
                .ToList();

            //получаем список структурных элементов
            var structuralElementsList = StructuralElementRepository.GetAll().ToList();

            //Списки объектов для сохранения
            var fiasAddressSaveList = new List<FiasAddress>();
            var roSaveList = new List<RealityObject>();
            var structuralElementsSaveList = new List<RealityObjectStructuralElement>();

            foreach (var realtyObjectToCreate in realityObjects.Where(x => x != null))
            {
                var objectToCreate = realtyObjectToCreate;

                if (objectToCreate.FiasAddress == null)
                {
                    AddLog(objectToCreate.RowNumber, "Объект не создан. В ФИАС не удалось найти адреc, соответствующий записи", ResultType.DoesNotMeetFias);

                    continue;
                }

                if (!ValidateEntity(objectToCreate.FiasAddress, objectToCreate.RowNumber))
                {
                    continue;
                }

                if (!municipalitiesDict.ContainsKey(objectToCreate.MunicipalityId))
                {
                    AddLog(objectToCreate.RowNumber, "Не удалось определить муниципальное образование", ResultType.No);

                    continue;
                }

                var municipality = municipalitiesDict[objectToCreate.MunicipalityId];

                //Адрес для сохранения
                fiasAddressSaveList.Add(objectToCreate.FiasAddress);

                var realityObj = new RealityObject
                {
                    FiasAddress = objectToCreate.FiasAddress,
                    GkhCode = objectToCreate.GkhCode,
                    Municipality = municipality,
                    Address = GetAddressForMunicipality(municipality, objectToCreate.FiasAddress)
                };

                Apply(realityObj, objectToCreate);

                if (!ValidateEntity(municipality, objectToCreate.RowNumber))
                {
                    continue;
                }
                if (!ValidateEntity(realityObj, objectToCreate.RowNumber))
                {
                    continue;
                }

                //Дом для сохранения
                roSaveList.Add(realityObj);

                //Конструктивный элемент дома для сохранения
                var constructiveElements = GetConstructiveElements(realityObj, objectToCreate, structuralElementsList);
                structuralElementsSaveList.AddRange(constructiveElements);

                AddLog(objectToCreate.RowNumber, string.Format("Будет создан дом с адресом: {0} (оригинальный адрес: {1})", realityObj.Address, objectToCreate.StrAddress), ResultType.Yes);
            }

            //сохраняем            
            SaveObjects(roSaveList, structuralElementsSaveList, fiasAddressSaveList);
        }

        /// <summary>
        /// Сохранить объекты через сессию
        /// </summary>
        /// <param name="roUpdate">Список домов</param>
        /// <param name="roStructuralElementsUpdate">Список структурных элементов дома</param>
        /// <param name="fiasAddressesList">Список адресов</param>
        private void SaveObjects(
            List<RealityObject> roUpdate,
            List<RealityObjectStructuralElement> roStructuralElementsUpdate,
            List<FiasAddress> fiasAddressesList = null)
        {
            var session = Container.Resolve<ISessionProvider>().OpenStatelessSession();
            try
            {
                //Вспомогательное действие
                Action<BaseEntity> action = x =>
                {
                    if (x.Id > 0)
                    {
                        session.Update(x);
                    }
                    else
                    {
                        session.Insert(x);
                    }
                };

                using (var transaction = session.BeginTransaction())
                {
                    try
                    {
                        //адреса
                        if (fiasAddressesList != null)
                        {
                            fiasAddressesList.ForEach(action);
                        }

                        //дома
                        roUpdate.ForEach(action);

                        //структурные элементы дома
                        roStructuralElementsUpdate.ForEach(action);

                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
            finally
            {
                Container.Resolve<ISessionProvider>().CloseCurrentSession();
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }

        private List<RealityObjectStructuralElement> GetConstructiveElements(
            RealityObject realityObject,
            Record record,
            List<StructuralElement> structuralElementsList,
            bool realtyObjectIsNew = true)
        {
            var structElementRecordDict = record.StructElementDict
                .Where(x => !string.IsNullOrWhiteSpace(x.Key))
                .ToDictionary(x => x.Key, x => x.Value);

            var roStructuralElementsDict = realtyObjectStructuralElementsDict.ContainsKey(realityObject.Id)
                ? realtyObjectStructuralElementsDict[realityObject.Id]
                : new Dictionary<long, long>();

            var roStructuralElementList = new List<RealityObjectStructuralElement>();

            if (!realtyObjectIsNew)
            {
                roStructuralElementList =
                    RealityObjectStructuralElementRepository
                        .GetAll()
                        .Where(x => roStructuralElementsDict.Values.Contains(x.Id))
                        .ToList();

            }

            var result = new List<RealityObjectStructuralElement>();
            foreach (var structElement in structElementRecordDict)
            {
                var structuralElementId = structuralElementDict[structElement.Key];

                RealityObjectStructuralElement realityObjectStructuralElement;
                if (!realtyObjectIsNew && roStructuralElementsDict.ContainsKey(structuralElementId))
                {
                    realityObjectStructuralElement =
                        roStructuralElementList.First(x => x.Id == roStructuralElementsDict[structuralElementId]);
                }
                else
                {
                    realityObjectStructuralElement = new RealityObjectStructuralElement
                    {
                        RealityObject = realityObject,
                        StructuralElement =
                            structuralElementsList.First(x => x.Id == structuralElementId),
                        Name = structuralElementNameDict[structuralElementId]
                    };
                }

                if (!string.IsNullOrEmpty(structElement.Value.Volume))
                {
                    realityObjectStructuralElement.Volume = structElement.Value.Volume.ToDecimal();
                }

                if (!string.IsNullOrEmpty(structElement.Value.LastOverhaulYear))
                {
                    var value = structElement.Value.LastOverhaulYear.ToInt();

                    if (value > 0)
                    {
                        realityObjectStructuralElement.LastOverhaulYear = value;
                    }
                }

                if (!string.IsNullOrEmpty(structElement.Value.Wearout))
                {
                    decimal value;
                    var str = structElement.Value.Wearout.Replace(
                        CultureInfo.InvariantCulture.NumberFormat.CurrencyDecimalSeparator,
                        CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator);

                    if (decimal.TryParse(str, out value))
                    {
                        realityObjectStructuralElement.Wearout = value;
                    }
                }

                if (realityObjectStructuralElement.State == null)
                {
                    realityObjectStructuralElement.State = realityObjectStructuralElementDefaultState;
                }

                result.Add(realityObjectStructuralElement);
            }
            return result;
        }

        private void AddLog(int rowNumber, string text, ResultType type = ResultType.None, bool anyChange = false)
        {
            var id = rowNumber;
            var log = new Log();
            if (logDict.ContainsKey(id))
            {
                log = logDict[id];
            }
            else
            {
                logDict[id] = log;
            }


            if (type == ResultType.No || type == ResultType.DoesNotMeetFias)
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

            log.AnyChange |= anyChange;
        }

        private void WriteLogs()
        {
            foreach (var log in logDict.OrderBy(x => x.Key))
            {
                var logValue = log.Value;

                var text = string.IsNullOrEmpty(log.Value.Text.Trim(' ').Trim(','))
                               ? string.Empty
                               : "Место ошибки: " + log.Value.Text.Trim(' ').Trim(',');

                switch (logValue.ResultType)
                {
                    case ResultType.AlreadyExists:
                        logImport.CountChangedRows++;
                        text = "Данные о доме обновлены. " + text;

                        break;

                    case ResultType.DoesNotMeetFias:
                        logImport.CountWarning++;
                        break;

                    case ResultType.No:
                        logImport.CountWarning++;
                        break;

                    case ResultType.Yes:
                        logImport.CountAddedRows++;
                        break;

                    case ResultType.YesButWithErrors:
                        logImport.CountAddedRows++;
                        logImport.CountWarning++;
                        break;
                }

                var resultText = string.Format(
                    "Строка: {0}; Дом добавлен: {1} ",
                    log.Key,
                    logValue.ResultType.GetEnumMeta().Display);

                logImport.Info(resultText, text);
            }
        }

        /// <summary>
        /// Валидация сущности согласно маппингу
        /// </summary>
        /// <typeparam name="T">Тип сущности</typeparam>
        /// <param name="entity">Объект</param>
        private bool ValidateEntity<T>(T entity, int rowNumber)
        {
            var classMeta = SessionProvider.GetCurrentSession()
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
                        AddLog(rowNumber, string.Format("Свойство \"{0}\" не может принимать пустые значения; ", propName), ResultType.No);
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

                            AddLog(rowNumber, message, ResultType.No);
                            return false;
                        }
                    }
                }

                ++i;
            }

            return true;
        }

        /// <summary>
        /// Returns a string that has space removed from the start and the end, and that has each sequence of internal space replaced with a single space.
        /// </summary>
        /// <param name="initialString"></param>
        /// <returns></returns>
        private static string Simplified(string initialString)
        {
            if (string.IsNullOrEmpty(initialString))
            {
                return initialString;
            }

            var trimmed = initialString.Trim();

            if (!trimmed.Contains(" "))
            {
                return trimmed;
            }

            var result = string.Join(" ", trimmed.Split(' ').Where(x => !string.IsNullOrWhiteSpace(x)));

            return result;
        }

        /// <summary>
        /// Собирает адрес (Улица, дом, литера, корпус, секция, квартира)
        /// </summary>
        /// <param name="fiasAddress">
        /// ФИАС адрес
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        private string CreateAddressField(FiasAddress fiasAddress)
        {
            var addressParts = new List<string>();

            if (!string.IsNullOrEmpty(fiasAddress.StreetName))
            {
                addressParts.Add(fiasAddress.StreetName);
            }

            if (!string.IsNullOrEmpty(fiasAddress.House))
            {
                addressParts.Add(string.Format("д. {0}", fiasAddress.House));
            }

            if (!string.IsNullOrEmpty(fiasAddress.Letter))
            {
                addressParts.Add(string.Format("лит. {0}", fiasAddress.Letter));
            }

            if (!string.IsNullOrEmpty(fiasAddress.Housing))
            {
                addressParts.Add(string.Format("корп. {0}", fiasAddress.Housing));
            }

            if (!string.IsNullOrEmpty(fiasAddress.Building))
            {
                addressParts.Add(string.Format("секц. {0}", fiasAddress.Building));
            }

            if (!string.IsNullOrEmpty(fiasAddress.Flat))
            {
                addressParts.Add(string.Format("кв. {0}", fiasAddress.Flat));
            }

            return string.Join(", ", addressParts);
        }

        public ImportResult ImportWithProgress(BaseParams @params, IProgressIndicator indicator)
        {
            return Import(@params);
        }
    }
}