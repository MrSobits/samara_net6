namespace Bars.Gkh.Overhaul.Import.CommonRealtyObjectImport
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
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Enums.Import;
    using Bars.Gkh.Import;
    using Bars.Gkh.Overhaul.Entities;
    using Bars.Gkh.Utils;
    using Bars.GkhExcel;
    using Castle.Windsor;
    using Gkh.Entities.CommonEstateObject;
    using Gkh.Entities.Dicts;
    using Gkh.Import.Impl;

    using Microsoft.Extensions.Logging;

    using NHibernate.Type;

    public class RoImport : GkhImportBase
    {
        public static string Id = MethodBase.GetCurrentMethod().DeclaringType.FullName;

        #region Properties

        public virtual IWindsorContainer Container { get; set; }

        public override string Key => RoImport.Id;

        public override string CodeImport => "CommonRealtyObjectImport";

        public override string Name => "Импорт жилых домов (универсальный)";

        public override string PossibleFileExtensions => "xls";

        public override string PermissionName => "Import.CommonRealtyObjectImport.View";

        public ISessionProvider SessionProvider { get; set; }

        public IFiasRepository FiasRepository { get; set; }

        public IRepository<RealityObject> RealityObjectRepository { get; set; }

        public IRepository<FiasAddress> FiasAddressRepository { get; set; }

        public IRepository<StructuralElement> StructuralElementRepository { get; set; }

        public IRepository<RealityObjectStructuralElement> RealityObjectStructuralElementRepository { get; set; }

        public IRepository<Fias> fiasRepo { get; set; }

        #endregion Properties

        #region Dictionaries

        private readonly Dictionary<int, Log> logDict = new Dictionary<int, Log>();

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

        #endregion Dictionaries

        private List<Record> realtyObjectsToCreate = new List<Record>();

        private bool createObjects;
        private bool allowEmptyStreets;

        private bool overrideExistRecords;

        private FiasHelper fiasHelper;

        private State realityObjectDefaultState;

        private State realityObjectStructuralElementDefaultState;

        public RoImport(ILogImportManager logImportManager, ILogImport logImport)
        {
            this.LogImportManager = logImportManager;
            this.LogImport = logImport;
        }

        private void InitDefaultStates()
        {
            var statePrivder = this.Container.Resolve<IStateProvider>();

            using (this.Container.Using(statePrivder))
            {
                var emptyRealityObject = new RealityObject();

                statePrivder.SetDefaultState(emptyRealityObject);

                this.realityObjectDefaultState = emptyRealityObject.State;

                var emptyRealityObjectStructuralElement = new RealityObjectStructuralElement();

                statePrivder.SetDefaultState(emptyRealityObjectStructuralElement);

                this.realityObjectStructuralElementDefaultState = emptyRealityObjectStructuralElement.State;
            }
        }

        public override ImportResult Import(BaseParams baseParams)
        {
            var file = baseParams.Files["FileImport"];

            this.createObjects = baseParams.Params.GetAs("createObjectsOnImport", false);
            this.allowEmptyStreets = baseParams.Params.GetAs("allowEmptyStreets", false);
            this.overrideExistRecords = baseParams.Params.GetAs("overridExistRecsOnImport", false);

            this.InitDefaultStates();

            this.InitLog(file.FileName);

            this.InitDictionaries();

            var message = this.ProcessData(file.Data);

            if (!string.IsNullOrEmpty(message))
            {
                return new ImportResult(StatusImport.CompletedWithError, message);
            }

            message = this.SaveData();

            this.WriteLogs();

            // Намеренно закрываем текущую сессию, иначе при каждом коммите транзакции
            // ранее измененные дома вызывают каскадирование ФИАС
            this.Container.Resolve<ISessionProvider>().CloseCurrentSession();

            this.LogImportManager.Add(file, this.LogImport);
            this.LogImportManager.Save();

            message += this.LogImportManager.GetInfo();
            var status = this.LogImportManager.CountError > 0 ? StatusImport.CompletedWithError : (this.LogImportManager.CountWarning > 0 ? StatusImport.CompletedWithWarning : StatusImport.CompletedWithoutError);
            return new ImportResult(status, message, string.Empty, this.LogImportManager.LogFileId);
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

            var fileExtentions = this.PossibleFileExtensions.Contains(",") ? this.PossibleFileExtensions.Split(',') : new[] {this.PossibleFileExtensions };
            if (fileExtentions.All(x => x != extention))
            {
                message = string.Format("Необходимо выбрать файл с допустимым расширением: {0}", this.PossibleFileExtensions);
                return false;
            }

            return true;
        }

        private void InitDictionaries()
        {
            this.fiasHelper = new FiasHelper(this.Container);

            var capitalGroupDomain = this.Container.ResolveDomain<CapitalGroup>();
            var realEstateTypeDomain = this.Container.ResolveDomain<RealEstateType>();
            var muDomain = this.Container.ResolveDomain<Municipality>();

            try
            {
                var fiasAddressList = this.FiasAddressRepository.GetAll()
                    .Select(x => new
                    {
                        x.Id,
                        x.House,
                        x.Housing,
                        x.Building,
                        x.Letter,
                        x.StreetGuidId
                    })
                    .ToList();

                var roList = this.RealityObjectRepository.GetAll()
                    .Select(x => new
                    {
                        FiasAddressId = x.FiasAddress.Id,
                        x.Id,
                        MunicipalityId = x.Municipality.Id
                    })
                    .ToList();

                this.realityObjectsByFiasGuidDict = this.fiasRepo.GetAll()
                    .Where(x => x.ActStatus == FiasActualStatusEnum.Actual)
                    .WhereNotEmptyString(x => x.AOGuid)
                    .Select(x => x.AOGuid)
                    .ToList()
                    .Join(fiasAddressList,
                        x => x,
                        y => y.StreetGuidId,
                        (a, b) => new { aoGuid = a, fiasAddress = b })
                    .Join(roList,
                        x => x.fiasAddress.Id,
                        y => y.FiasAddressId,
                        (c, d) => new { c.aoGuid, c.fiasAddress, realityObject = d })
                    .Select(x => new RealtyObjectAddress
                    {
                        AoGuid = x.aoGuid,
                        Id = x.fiasAddress.Id,
                        House = x.fiasAddress.House,
                        Housing = x.fiasAddress.Housing,
                        Building = x.fiasAddress.Building,
                        RealityObjectId = x.realityObject.Id,
                        MunicipalityId = x.realityObject.MunicipalityId,
                        Liter = x.fiasAddress.Letter
                    })
                    .GroupBy(x => x.AoGuid)
                    .ToDictionary(x => x.Key, x => x.ToList());

                this.capitalGroupDict = capitalGroupDomain.GetAll()
                    .Where(x => x.Code != null && x.Code != "")
                    .ToList()
                    .GroupBy(x => x.Name)
                    .ToDictionary(x => x.Key, y => y.First());

                this.realEstateTypeDict = realEstateTypeDomain.GetAll()
                    .Where(x => x.Code != null && x.Code != "")
                    .ToList()
                    .GroupBy(x => x.Code)
                    .ToDictionary(x => x.Key, y => y.First());

                var municipalities = muDomain.GetAll().ToArray();

                this.municipalitiesDict = municipalities.ToDictionary(x => x.Id);

                this.fiasIdByMunicipalityNameDict = municipalities
                    .Where(x => this.Name != null)
                    .Select(x => new {x.Name, x.FiasId, x.Id})
                    .AsEnumerable()
                    .GroupBy(x => x.Name.ToUpper())
                    .ToDictionary(
                        x => x.Key,
                        x =>
                        {
                            var first = x.First();
                            return new KeyValuePair<string, long>(first.FiasId, first.Id);
                        });
            }
            finally
            {
                this.Container.Release(capitalGroupDomain);
                this.Container.Release(realEstateTypeDomain);
                this.Container.Release(muDomain);
            }
        }

        private string InitHeader(GkhExcelCell[] data)
        {
            this.headersDict["MU"] = -1;
            this.headersDict["KINDCITY"] = -1;
            this.headersDict["CITY"] = -1;
            this.headersDict["KINDSTREET"] = -1;
            this.headersDict["STREET"] = -1;
            this.headersDict["KLADRSTREETKOD"] = -1;
            this.headersDict["HOUSENUM"] = -1;
            this.headersDict["LITER"] = -1;
            this.headersDict["KORPUS"] = -1;
            this.headersDict["BUILDING"] = -1;
            this.headersDict["HOUSINGVIEW"] = -1;
            this.headersDict["STARTUPDAY"] = -1;
            this.headersDict["BUILTYEAR"] = -1;
            this.headersDict["TOTALAREA"] = -1;
            this.headersDict["LIVNOTLIVAREA"] = -1;
            this.headersDict["LIVAREA"] = -1;
            this.headersDict["MINFLOORS"] = -1;
            this.headersDict["MAXFLOORS"] = -1;
            this.headersDict["ENTRANCESNUM"] = -1;
            this.headersDict["GROUPOFCAP"] = -1;
            this.headersDict["FLATCOUNT"] = -1;
            this.headersDict["RESIDENTSNUM"] = -1;
            this.headersDict["APARTLIVAREA"] = -1;
            this.headersDict["TOTALDEPRECIATION"] = -1;
            this.headersDict["PRIVDATE"] = -1;
            this.headersDict["PRIVATEPROPERTYAREA"] = -1;
            this.headersDict["MUNICIPALPROPERTYAREA"] = -1;
            this.headersDict["HOUSECONDITION"] = -1;
            this.headersDict["ISNOTINVOLVEDCR"] = -1;
            this.headersDict["KOD"] = -1;
            this.headersDict["HOUSETYPE"] = -1;

            for (var index = 0; index < data.Length; ++index)
            {
                var header = data[index].Value.ToUpper();
                if (this.headersDict.ContainsKey(header))
                {
                    this.headersDict[header] = index;
                }
                else if (header.Contains('_'))
                {
                    var arr = header.Split('_');

                    if (arr.Length > 1)
                    {
                        var code = arr[1];

                        var structElementHeader = new StructElementHeader();

                        if (this.structElementHeadersDict.ContainsKey(code))
                        {
                            structElementHeader = this.structElementHeadersDict[code];
                        }
                        else
                        {
                            this.structElementHeadersDict[code] = structElementHeader;
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

            var structElemList = this.StructuralElementRepository.GetAll()
                .Where(x => x.Code != null)
                .Select(x => new { x.Code, x.Id, x.Name, grName = x.Group.Name, coeName = x.Group.CommonEstateObject.Name })
                .AsEnumerable()
                .Where(x => this.structElementHeadersDict.Keys.Contains(x.Code))
                .ToArray();

            var undefinedStructElCodes = this.structElementHeadersDict.Keys.Where(x => !structElemList.Select(y => y.Code).Contains(x)).ToList();

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
            this.structuralElementDict = structElemList.ToDictionary(x => x.Code, x => x.Id);

            // Словарь наименований конструктивных характеристик
            this.structuralElementNameDict = structElemList.ToDictionary(x => x.Id, x => x.Name);

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
                .Where(x => this.structuralElementNameDict.Keys.Contains(x.seId))
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
            this.LogImport.ImportKey = this.Key;
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

                PlaceAddressName = (string.IsNullOrEmpty(address.Name) ?  address.AddressName : address.AddressName.Replace(address.Name, string.Empty)).Trim().Trim(','),
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

            if (this.dynamicAddressDictByMixedGuid.ContainsKey(mo.FiasId))
            {
                dinamicAddress = this.dynamicAddressDictByMixedGuid[mo.FiasId];
            }
            else
            {
                dinamicAddress = this.FiasRepository.GetDinamicAddress(mo.FiasId);
                this.dynamicAddressDictByMixedGuid[mo.FiasId] = dinamicAddress;
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
                if (this.createObjects)
                {
                    record.FiasAddress = this.CreateFiasAddress(record.Address, record.House, record.Housing, record.Building);

                    this.realtyObjectsToCreate.Add(record);
                }
                else
                {
                    var text = string.Format(
                            "Не удалось найти дом, соответствующий адресу {0} или KLADR_KOD_STREET: {1}; NUM: {2}, KORPUS: {3}",
                            record.LocalityName + " " + record.StreetName,
                            record.CodeKladrStreet,
                            record.House,
                            record.Housing);

                    this.AddLog(record.RowNumber, text, ResultType.No);
                }
            };

            var excel = this.Container.Resolve<IGkhExcelProvider>("ExcelEngineProvider");
            using (this.Container.Using(excel))
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

                    var message = this.InitHeader(rows.First());

                    if (!string.IsNullOrEmpty(message))
                    {
                        return message;
                    }

                    for (var i = 1; i < rows.Count; ++i)
                    {
                        var record = this.ProcessLine(rows[i], i + 1);

                        if (!record.isValidRecord)
                        {
                            continue;
                        }

                        // Проверяем есть ли дома на данной улице
                        //if (!realityObjectsByFiasGuidDict.ContainsKey(record.Address.GuidId))
                        var guid = record.Address.GuidId == null && this.allowEmptyStreets ? record.Address.ParentGuidId : record.Address.GuidId;
                        if (!this.realityObjectsByFiasGuidDict.ContainsKey(guid))
                        {
                            createRealtyObject(record);
                        }
                        else
                        {
                            // Составляем список домов на данной улице (!) с проверкой привязки МО (это не бред, после разделения одного МО многое может произойти)
                            var existingRealityObjects = this.realityObjectsByFiasGuidDict[guid].Where(x => x.MunicipalityId == record.MunicipalityId).ToList();

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
                                    this.AddLog(record.RowNumber, "В системе найдено несколько домов, соответствующих записи.", ResultType.No);
                                }
                                else
                                {
                                    // Найден ровно 1 дом
                                    // Если в файле есть дублирующиеся дома, загружать последний номер (т.е. перезапишем существующую запись словаря)
                                    this.foundRealtyObjects[result.First().RealityObjectId] = record;

                                    if (!this.overrideExistRecords)
                                    {
                                        this.AddLog(record.RowNumber, "По данному дому уже есть данные. Данные не будут загружены.");
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

            var municipalityName = this.GetValue(data, "MU").Trim().ToUpper();
            if (string.IsNullOrEmpty(municipalityName))
            {
                this.AddLog(record.RowNumber, "Не задано муниципальное образование.", ResultType.No);
                return record;
            }

            record.LocalityName = RoImport.Simplified(this.GetValue(data, "CITY") + " " + this.GetValue(data, "KINDCITY"));
            record.StreetName = RoImport.Simplified(this.GetValue(data, "STREET") + " " + this.GetValue(data, "KINDSTREET"));
            record.CodeKladrStreet = this.GetValue(data, "KLADRSTREETKOD");
            record.House = RoImport.Simplified(this.GetValue(data, "HOUSENUM"));
            record.Liter = RoImport.Simplified(this.GetValue(data, "LITER"));
            record.GkhCode = RoImport.Simplified(this.GetValue(data, "KOD"));

            if (string.IsNullOrEmpty(record.House))
            {
                this.AddLog(record.RowNumber, "Не задан номер дома.", ResultType.No);
                return record;
            }

            if (!this.fiasIdByMunicipalityNameDict.ContainsKey(municipalityName))
            {
                var errText = string.Format("В справочнике муниципальных образований не найдена запись: {0}", this.GetValue(data, "MU").Trim());
                this.AddLog(record.RowNumber, errText, ResultType.No);
                return record;
            }

            var municipality = this.fiasIdByMunicipalityNameDict[municipalityName];
            var fiasGuid = municipality.Key;

            if (string.IsNullOrWhiteSpace(fiasGuid))
            {
                this.AddLog(record.RowNumber, "Муниципальное образование не привязано к ФИАС", ResultType.No);
                return record;
            }

            if (!this.fiasHelper.HasBranch(fiasGuid))
            {
                this.AddLog(record.RowNumber, "В структуре ФИАС не найдена актуальная запись для муниципального образования.", ResultType.No);
                return record;
            }

            var faultReason = string.Empty;
            DynamicAddress address;

            if (this.RecordHasValidCodeKladrStreet(record))
            {
                if (!this.fiasHelper.FindInBranchByKladr(fiasGuid, record.CodeKladrStreet, ref faultReason, out address))
                {
                    this.AddLog(record.RowNumber, faultReason, ResultType.DoesNotMeetFias);
                    return record;
                }
            }
            else
            {
                if (!this.fiasHelper.FindInBranch(fiasGuid, record.LocalityName, record.StreetName, ref faultReason, out address, this.allowEmptyStreets))
                {
                    this.AddLog(record.RowNumber, faultReason, ResultType.DoesNotMeetFias);
                    return record;
                }
            }

            record.Address = address;
            record.MunicipalityId = municipality.Value;

            record.Housing = RoImport.Simplified(this.GetValue(data, "KORPUS"));
            record.Building = RoImport.Simplified(this.GetValue(data, "BUILDING"));
            record.TypeHouse = GetValue(data, "HOUSINGVIEW");
            record.DateCommissioning = this.GetValue(data, "STARTUPDAY");
            record.BuildYear = this.GetValue(data, "BUILTYEAR");
            record.AreaMkd = this.GetValue(data, "TOTALAREA");
            record.AreaLivingNotLivingMkd = this.GetValue(data, "LIVNOTLIVAREA");
            record.AreaLiving = this.GetValue(data, "LIVAREA");
            record.Floors = this.GetValue(data, "MINFLOORS");
            record.MaximumFloors = this.GetValue(data, "MAXFLOORS");
            record.NumberEntrances = this.GetValue(data, "ENTRANCESNUM");
            record.CapitalGroup = this.GetValue(data, "GROUPOFCAP");
            record.NumberLiving = this.GetValue(data, "RESIDENTSNUM");
            record.NumberApartments = this.GetValue(data, "FLATCOUNT");
            record.PhysicalWear = this.GetValue(data, "TOTALDEPRECIATION");
            record.PrivatizationDateFirstApartment = this.GetValue(data, "PRIVDATE");
            record.AreaOwned = this.GetValue(data, "PRIVATEPROPERTYAREA");
            record.AreaMunicipalOwned = this.GetValue(data, "MUNICIPALPROPERTYAREA");
            record.ConditionHouse = this.GetValue(data, "HOUSECONDITION");
            record.IsNotInvolvedCr = this.GetValue(data, "ISNOTINVOLVEDCR");
            record.RealEstTypeCode = this.GetValue(data, "HOUSETYPE");

            record.StructElementDict = this.GetStructElements(data);

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

            return this.fiasHelper.HasValidStreetKladrCode(record.CodeKladrStreet);
        }

        private string GetValue(GkhExcelCell[] data, string field)
        {
            var result = string.Empty;

            if (this.headersDict.ContainsKey(field))
            {
                var index = this.headersDict[field];
                if (data.Length > index && index > -1)
                {
                    result = data[index].Value ?? string.Empty;
                }
            }

            return result.Trim();
        }

        private Dictionary<string, StructElementRecord> GetStructElements(GkhExcelCell[] data)
        {
            var result = new Dictionary<string, StructElementRecord>();

            foreach (var header in this.structElementHeadersDict)
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
                realityObject.State = this.realityObjectDefaultState;
            }

            Action<string> warnLog = text => this.AddLog(record.RowNumber, text, ResultType.YesButWithErrors);

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
                if (this.capitalGroupDict.ContainsKey(record.CapitalGroup))
                {
                    realityObject.CapitalGroup = this.capitalGroupDict[record.CapitalGroup];
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
                if (this.realEstateTypeDict.ContainsKey(record.RealEstTypeCode))
                {
                    realityObject.RealEstateType = this.realEstateTypeDict[record.RealEstTypeCode];
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
                    realityObject.Address = this.CreateAddressField(realityObject.FiasAddress);

                    realityObject.FiasAddress.AddressName = string.IsNullOrEmpty(realityObject.FiasAddress.PlaceAddressName)
                        ? realityObject.Address
                        : realityObject.FiasAddress.PlaceAddressName + ", " + realityObject.Address;
                }
            }
        }

        private string SaveData()
        {
            var message = string.Empty;

            using (var transaction = this.Container.Resolve<IDataTransaction>())
            {
                try
                {
                    if (this.overrideExistRecords)
                    {
                        foreach (var foundRealtyObject in this.foundRealtyObjects.Where(x => x.Value != null))
                        {
                            var realityObj = this.RealityObjectRepository.Load(foundRealtyObject.Key);
                            this.Apply(realityObj, foundRealtyObject.Value);

                            this.RealityObjectRepository.Update(realityObj);
                            this.SaveConstructiveElements(realityObj, foundRealtyObject.Value, false);

                            this.AddLog(foundRealtyObject.Value.RowNumber, string.Empty, ResultType.AlreadyExists);
                        }

                        this.LogImport.CountChangedRows = this.foundRealtyObjects.Count(x => x.Value != null);
                    }

                    if (this.createObjects && this.realtyObjectsToCreate.Any())
                    {
                        // Если в файле есть дублирующиеся дома, загружать последний.
                        this.realtyObjectsToCreate = this.realtyObjectsToCreate
                            .GroupBy(x => new
                                              {
                                                  GuidID = x.Address.GuidId == null && this.allowEmptyStreets 
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

                                    objects.ForEach(y => this.AddLog(y.RowNumber, errMessage, ResultType.No));
                                }

                                return last;
                            })
                            .ToList();

                        foreach (var realtyObjectToCreate in this.realtyObjectsToCreate.Where(x => x != null))
                        {
                            var objectToCreate = realtyObjectToCreate;

                            if (objectToCreate.FiasAddress == null)
                            {
                                this.AddLog(objectToCreate.RowNumber, "Объект не создан. В ФИАС не удалось найти адреc, соответствующий записи", ResultType.DoesNotMeetFias);

                                continue;
                            }

                            if (!this.ValidateEntity(objectToCreate.FiasAddress, objectToCreate.RowNumber))
                            {
                                continue;
                            }

                            if (!this.municipalitiesDict.ContainsKey(objectToCreate.MunicipalityId))
                            {
                                this.AddLog(objectToCreate.RowNumber, "Не удалось определить муниципальное образование", ResultType.No);

                                continue;
                            }

                            var municipality = this.municipalitiesDict[objectToCreate.MunicipalityId];

                            this.FiasAddressRepository.Save(objectToCreate.FiasAddress);

                            var realityObj = new RealityObject
                            {
                                FiasAddress = objectToCreate.FiasAddress,
                                GkhCode = objectToCreate.GkhCode,
                                Municipality = municipality,
                                Address = this.GetAddressForMunicipality(municipality, objectToCreate.FiasAddress)
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

                            this.RealityObjectRepository.Save(realityObj);
                            this.SaveConstructiveElements(realityObj, objectToCreate);

                            this.AddLog(objectToCreate.RowNumber, string.Empty, ResultType.Yes);
                        }

                        this.LogImport.CountAddedRows = this.realtyObjectsToCreate.Count(x => x != null);
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

        private void SaveConstructiveElements(RealityObject realityObject, Record record, bool realtyObjectIsNew = true)
        {
            var roStructuralElementsDict = this.realtyObjectStructuralElementsDict.ContainsKey(realityObject.Id)
                       ? this.realtyObjectStructuralElementsDict[realityObject.Id]
                       : new Dictionary<long, long>();

            foreach (var structElement in record.StructElementDict.Where(x => !string.IsNullOrWhiteSpace(x.Key)))
            {
                var structuralElementId = this.structuralElementDict[structElement.Key];

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
                    realityObjectStructuralElement.State = this.realityObjectStructuralElementDefaultState;
                }

                this.RealityObjectStructuralElementRepository.Save(realityObjectStructuralElement);
            }
        }

        private void AddLog(int rowNumber, string text, ResultType type = ResultType.None, bool anyChange = false)
        {
            var id = rowNumber;
            var log = new Log();
            if (this.logDict.ContainsKey(id))
            {
                log = this.logDict[id];
            }
            else
            {
                this.logDict[id] = log;
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
            foreach (var log in this.logDict.OrderBy(x => x.Key))
            {
                var logValue = log.Value;

                var text = string.IsNullOrEmpty(log.Value.Text.Trim(' ').Trim(','))
                               ? string.Empty
                               : "Место ошибки: " + log.Value.Text.Trim(' ').Trim(',');

                switch (logValue.ResultType)
                {
                    case ResultType.AlreadyExists:
                        this.LogImport.CountChangedRows++;
                        text = "Данные о доме обновлены. " + text;

                        break;

                    case ResultType.DoesNotMeetFias:
                        this.LogImport.CountWarning++;
                        break;

                    case ResultType.No:
                        this.LogImport.CountWarning++;
                        break;

                    case ResultType.Yes:
                        this.LogImport.CountAddedRows++;
                        break;

                    case ResultType.YesButWithErrors:
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
        /// <param name="rowNumber">номер строки</param>
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
                        this.AddLog(rowNumber, string.Format("Свойство \"{0}\" не может принимать пустые значения; ", propName), ResultType.No);
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

                            this.AddLog(rowNumber, message, ResultType.No);
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
    }
}