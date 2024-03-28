//ToDo короче если В Кэш засунут ьсущность котоаря потенциально может в себе содержать большой объекм данных
//ToDo то тогда произходят вещи каоторые тратят время просто на обновление кеша который постоянно тянет записи из этой таблицы
//ToDo если добавляете в кеш сущности тяжелые то будте готовы проверить это при больших объекмах
//Todo Иначе вы будете 5 записей грузить 30 минут. Из которых 29,5 минут уйдет только на кеш и на его постоянные обновления
//ToDo когда дебажил использовал Output и смотрел все его запросы выводиммые в консоль

namespace Bars.Gkh.RegOperator.Imports.Account
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text.RegularExpressions;

    using B4;
    using B4.DataAccess;
    using B4.Modules.FIAS;
    using B4.Modules.NHibernateChangeLog;
    using B4.Modules.States;
    using B4.Utils;

    using Bars.B4.IoC;
    using Bars.Gkh.Domain.TableLocker;

    using Decisions.Nso.Domain;
    using Decisions.Nso.Entities;
    using DomainService.PersonalAccount;
    using Gkh.Domain.Cache;
    using Gkh.Entities;
    using Gkh.Enums;
    using Gkh.Enums.Decisions;
    using Gkh.Enums.Import;
    using Gkh.ExecutionAction;
    using Import;
    using Entities;
    using Enums;
    using GkhExcel;

    using Castle.Windsor;
    using Import.Impl;

    public class OwnersImport : GkhImportBase
    {
        public static string Id = MethodBase.GetCurrentMethod().DeclaringType.FullName;

        #region Properties

        public virtual IWindsorContainer Container { get; set; }

        public ISubPersonalAccountService SubPersonalAccountService { get; set; }

        public override string Key
        {
            get { return Id; }
        }

        public override string CodeImport
        {
            get { return "OwnersImport"; }
        }

        public override string Name
        {
            get { return "Импорт собственников"; }
        }

        public override string PossibleFileExtensions
        {
            get { return "xls"; }
        }

        public override string PermissionName
        {
            get { return "Import.OwnersImport"; }
        }

        #endregion Properties

        #region private fields

        private readonly IRepository<RealityObject> _realityObjectRepository;
        private readonly IRepository<Room> _roomRepository;
        private readonly IRepository<BasePersonalAccount> _basePersonalAccountRepository;
        private readonly IRepository<IndividualAccountOwner> _individualAccountOwnerRepository;
        private readonly IRepository<LegalAccountOwner> _legalAccountOwnerRepository;
        private readonly IRepository<Contragent> _contragentRepository;
        private readonly IDomainService<State> _stateDomainService;
        private readonly IRealityObjectDecisionsService _realityObjectDecisionsService;
        private readonly IAccountNumberService _accountNumberService;
        private readonly IStateProvider _stateProvider;
        private readonly GkhCache _cache;

        protected DateTime _accountCreateDate;
        private List<Record> _records = new List<Record>();

        private readonly StatefulEntityInfo _persAccStateInfo;
        private State _defaultAccState;
        private State _accOpenState;
        private State _accActiveState;

        #region Dictionaries

        private class RealtyObjectInStreet
        {
            public long RoId { get; set; }

            public string House { get; set; }

            public string Housing { get; set; }
        }

        private readonly Dictionary<int, KeyValuePair<bool, string>> _logDict =
            new Dictionary<int, KeyValuePair<bool, string>>();

        private Dictionary<string, Dictionary<string, List<RealtyObjectInStreet>>> _realtyObjectsByAddressDict;

        private readonly Dictionary<string, int> _headersDict = new Dictionary<string, int>();

        private Dictionary<long, Dictionary<string, long>> _realtyObjectRoomsDict;

        private Dictionary<string, long> _individualOwnersDict;

        private Dictionary<long, long> _legalOwnersDict;

        private Dictionary<string, long> _accountByRoomAndOwnerDict;

        private Dictionary<string, long> _contragentsDict;

        private Dictionary<long, BasePersonalAccount> _cacheAccounts;
        private Dictionary<string, Room> _cacheRooms;

        private Dictionary<long, long> _realtyObjectMunicipalityDict = new Dictionary<long, long>();
        private Dictionary<long, Dictionary<Type, UltimateDecision>> _decisions;

        #endregion Dictionaries

        #endregion

        public OwnersImport(IRepository<Contragent> contragentRepository,
            IRepository<LegalAccountOwner> legalAccountOwnerRepository,
            IRepository<IndividualAccountOwner> individualAccountOwnerRepository,
            IRepository<BasePersonalAccount> basePersonalAccountRepository,
            IRepository<Room> roomRepository,
            IRepository<RealityObject> realityObjectRepository, IDomainService<State> stateDomainService,
            IRealityObjectDecisionsService realityObjectDecisionsService,
            IAccountNumberService accountNumberService,
            IStateProvider stateProvider,
            GkhCache cache,
            ILogImportManager logManager,
            ILogImport logImport)
        {
            _contragentRepository = contragentRepository;
            _legalAccountOwnerRepository = legalAccountOwnerRepository;
            _individualAccountOwnerRepository = individualAccountOwnerRepository;
            _basePersonalAccountRepository = basePersonalAccountRepository;
            _roomRepository = roomRepository;
            _realityObjectRepository = realityObjectRepository;
            _stateDomainService = stateDomainService;
            _realityObjectDecisionsService = realityObjectDecisionsService;
            _accountNumberService = accountNumberService;
            _stateProvider = stateProvider;
            _cache = cache;
            this.LogImportManager = logManager;
            this.LogImport = logImport;

            _persAccStateInfo = _stateProvider.GetStatefulEntityInfo(typeof (BasePersonalAccount));
        }

        protected virtual void InitDictionaries()
        {
            _cache.RegisterEntity<RealityObject>()
                .MakeDictionary(x => x.Id.ToStr());
            
            _cacheRooms = _roomRepository.GetAll()
                               .AsEnumerable()
                               .GroupBy(x => "{0}|{1}".FormatUsing(x.RealityObject.Id, x.RoomNum))
                               .ToDictionary(x => x.Key, y => y.First());
            
            var fiasService = Container.Resolve<IDomainService<Fias>>().GetAll();

            _realtyObjectsByAddressDict = fiasService
                .Join(
                    fiasService,
                    x => x.AOGuid,
                    x => x.ParentGuid,
                    (a, b) => new { parent = a, child = b })
                .Join(
                    _realityObjectRepository.GetAll(),
                    x => x.child.AOGuid,
                    y => y.FiasAddress.StreetGuidId,
                    (c, d) => new { c.parent, c.child, realityObject = d })
                .Where(x => x.parent.ActStatus == FiasActualStatusEnum.Actual)
                .Where(x => x.child.ActStatus == FiasActualStatusEnum.Actual)
                .Where(x => x.child.AOLevel == FiasLevelEnum.Street)
                .Select(x => new
                {
                    localityName = x.parent.OffName,
                    streetName = x.child.OffName,
                    x.realityObject.FiasAddress.House,
                    x.realityObject.FiasAddress.Housing,
                    RealityObjectId = x.realityObject.Id
                })
                .AsEnumerable()
                .Where(x => !string.IsNullOrWhiteSpace(x.localityName))
                .Where(x => !string.IsNullOrWhiteSpace(x.streetName))
                .GroupBy(x => x.localityName.ToLower())
                .ToDictionary(
                    x => x.Key,
                    x => x.GroupBy(y => y.streetName.ToLower())
                        .ToDictionary(
                            y => y.Key,
                            y => y.Select(z => new RealtyObjectInStreet
                            {
                                RoId = z.RealityObjectId,
                                House = z.House,
                                Housing = z.Housing
                            }).ToList()));

            _cache.RegisterEntity<Contragent>()
                .MakeDictionary(x => x.Name ?? string.Empty);
           
            _cache.RegisterEntity<LegalAccountOwner>()
                .MakeDictionary(x => x.Contragent.Id.ToString());

            _cache.RegisterEntity<IndividualAccountOwner>()
                .MakeDictionary(
                    x =>
                        "{0} {1} {2}".FormatUsing(
                            x.Surname,
                            x.FirstName,
                            x.SecondName).ToLower().Trim());

            // Внимание кеш для счетов использую в других целях
            

            _cache.Update();

            var accountService = Container.Resolve<IDomainService<BasePersonalAccount>>();

            _accountByRoomAndOwnerDict = accountService.GetAll()
                .Select(x => new
                {
                    AccountId = x.Id,
                    RoomId = x.Room.Id,
                    OwnerId = x.AccountOwner.Id
                })
                .AsEnumerable()
                .GroupBy(x => "{0}|{1}".FormatUsing(x.RoomId, x.OwnerId))
                .ToDictionary(
                    x => x.Key,
                    y => y.First().AccountId);

            _cacheAccounts = accountService.GetAll().ToDictionary(x => x.Id);
        }

        private string InitHeader(GkhExcelCell[] data)
        {
            _headersDict["LOCALITY"] = -1;
            _headersDict["STREET"] = -1;
            _headersDict["HOUSE"] = -1;
            _headersDict["HOUSING"] = -1;
            _headersDict["APARTMENT"] = -1;
            _headersDict["OWNER"] = -1;
            _headersDict["AREA"] = -1;
            _headersDict["AREASHARE"] = -1;
            _headersDict["TYPEOWNER"] = -1;

            for (var index = 0; index < data.Length; ++index)
            {
                var header = data[index].Value.ToUpper();
                if (_headersDict.ContainsKey(header))
                {
                    _headersDict[header] = index;
                }
            }

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
            this.LogImport.ImportKey = Key;
        }

        private string GetValue(GkhExcelCell[] data, string field)
        {
            var result = string.Empty;

            if (_headersDict.ContainsKey(field))
            {
                var index = _headersDict[field];
                if (data.Length > index && index > -1)
                {
                    result = data[index].Value;
                }
            }

            if (result == null) result = string.Empty;

            return result.Trim();
        }

        private IEntityCache<RealityObject> roCache { get; set; }

        private IEntityCache<IndividualAccountOwner> individualAccCache { get; set; }

        private IEntityCache<LegalAccountOwner> legalCache { get; set; }

        private IEntityCache<Contragent> contrCache { get; set; }

        public override ImportResult Import(BaseParams baseParams)
        {
            var session = Container.Resolve<ISessionProvider>().GetCurrentSession();
            NHibernateChangeLogControl.DisableAuditForSession(session);

            if (!AccountHasDefaultState())
            {
                return new ImportResult(StatusImport.CompletedWithError,
                    "Не создан начальный статус для сущности 'Лицевой счет(модуль Регоператора)'!");
            }

            CultureInfo.CreateSpecificCulture("ru-RU");

            var file = baseParams.Files["FileImport"];

            _accountCreateDate = baseParams.Params.GetAs("AccountCreateDate", DateTime.MinValue);

            if (_accountCreateDate == DateTime.MinValue)
            {
                return new ImportResult(StatusImport.CompletedWithError, "Укажите дату!");
            }

            InitLog(file.FileName);

            InitDictionaries();

            var message = ProcessData(file.Data);

            if (!message.IsEmpty())
            {
                return new ImportResult(StatusImport.CompletedWithError, message);
            }

            // Сразуже забираем то что находится в кеше
            roCache = _cache.GetCache<RealityObject>();
            individualAccCache = _cache.GetCache<IndividualAccountOwner>();
            legalCache = _cache.GetCache<LegalAccountOwner>();
            contrCache = _cache.GetCache<Contragent>();

            InTransaction(SaveData);

            if (SubPersonalAccountService != null)
            {
                SubPersonalAccountService.AddSubPersonalAccount();
            }

            WriteLogs();


            var entityLogLigthPopulateAction = Container.ResolveAll<IExecutionAction>()
                .ToList<IExecutionAction>()
                .FirstOrDefault(x => x.Code == "EntityLogLigthPopulateAction");

            if (entityLogLigthPopulateAction != null)
            {
                entityLogLigthPopulateAction.Action();
            }
        
            // Намеренно закрываем текущую сессию, иначе при каждом коммите транзакции
            // ранее измененные дома вызывают каскадирование ФИАС
            //Container.Resolve<ISessionProvider>().CloseCurrentSession();

            this.LogImportManager.Add(file, this.LogImport);
            this.LogImportManager.Save();

            _cache.Clear();
            NHibernateChangeLogControl.ReenableAuditForSession(session);

            message += this.LogImportManager.GetInfo();
            var status = this.LogImportManager.CountError > 0
                ? StatusImport.CompletedWithError
                : (this.LogImportManager.CountWarning > 0
                    ? StatusImport.CompletedWithWarning
                    : StatusImport.CompletedWithoutError);
            return new ImportResult(status, message, string.Empty, this.LogImportManager.LogFileId);
        }

        private string ProcessData(byte[] fileData)
        {
            _records.Clear();

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

                    var message = InitHeader(rows.First());

                    if (!message.IsEmpty())
                    {
                        return message;
                    }

                    for (var i = 1; i < rows.Count; ++i)
                    {
                        var record = ProcessLine(rows[i], i + 1);

                        if (record.isValidRecord)
                        {
                            _records.Add(record);
                        }
                    }

                    _records = _records
                        .GroupBy(x => new
                        {
                            x.RealtyObjectId,
                            x.Apartment,
                            x.Owner,
                            x.OwnerType,
                            x.Area,
                            x.AreaShare
                        })
                        .Select(x =>
                        {
                            var first = x.First();
                            if (x.Count() > 1)
                            {
                                var errMsg = string.Format("Дубль строки {0}", first.RowNumber);
                                x.Where(y => y != first).ForEach(y => AddLog(y.RowNumber, errMsg, false));
                            }

                            return first;
                        })
                        .ToList();

                    var dict = new Dictionary<string, decimal>();
                    var filteredRecs = new List<Record>();

                    foreach (var record in _records)
                    {
                        var key = string.Format("{0}_{1}", record.RealtyObjectId, record.Apartment);

                        if (dict.ContainsKey(key))
                        {
                            var value = dict[key];
                            var newValue = value + record.AreaShare;
                            if (newValue > 1)
                            {
                                AddLog(
                                    record.RowNumber,
                                    string.Format(
                                        "Сумма долей собственности превышает 1. {0}, {1}, {2}, {3}",
                                        record.LocalityName,
                                        record.StreetName,
                                        record.House,
                                        record.Apartment),
                                    false);
                            }
                            else
                            {
                                dict[key] = newValue;
                                filteredRecs.Add(record);
                            }
                        }
                        else
                        {
                            if (record.AreaShare >1)
                            {
                                AddLog(record.RowNumber, "Доля собственности превышает 1", false);
                            }
                            else
                            {
                                dict.Add(key, record.AreaShare);
                                filteredRecs.Add(record);
                            }
                        }
                    }
                    
                    dict.Clear();
                    _records.Clear();
                    _records = filteredRecs;
                }
            }

            return string.Empty;
        }

        /// <summary>
        /// Обработка строки импорта
        /// </summary>
        private Record ProcessLine(GkhExcelCell[] data, int rowNumber)
        {
            try
            {
                var record = new Record { isValidRecord = false, RowNumber = rowNumber };

                if (data.Length <= 1)
                {
                    return record;
                }

                record.LocalityName = GetValue(data, "LOCALITY").ToLower();

                if (record.LocalityName.IsEmpty())
                {
                    AddLog(record.RowNumber, "Не задан населенный пункт.", false);
                    return record;
                }

                record.StreetName = GetValue(data, "STREET").ToLower();

                if (record.StreetName.IsEmpty())
                {
                    AddLog(record.RowNumber, "Не задана улица.", false);
                    return record;
                }

                record.House = GetValue(data, "HOUSE");

                if (record.House.IsEmpty())
                {
                    AddLog(record.RowNumber, "Не задан номер дома.", false);
                    return record;
                }

                record.Housing = GetValue(data, "HOUSING");

                record.Apartment = GetValue(data, "APARTMENT");

                if (record.Apartment.IsEmpty())
                {
                    AddLog(record.RowNumber, "Не задан номер квартиры.", false);
                    return record;
                }

                record.Owner = GetValue(data, "OWNER");

                if (record.Owner.IsEmpty())
                {
                    AddLog(record.RowNumber, "Не задан cобственник.", false);
                    return record;
                }

                var area = GetValue(data, "AREA");

                if (area.IsEmpty())
                {
                    AddLog(record.RowNumber, "Не задана общая площадь квартиры.", false);
                    return record;
                }
                else
                {
                    decimal value;

                    if (TryGetPreisAsDecimal(area.Replace('.', ','), out value))
                    {
                        record.Area = value;
                    }
                    else
                    {
                        AddLog(record.RowNumber, "Некорректное число в поле 'AREA': " + area, false);
                        return record;
                    }


                    //if (decimal.TryParse(area.Replace('.', ','), NumberStyles.Number, _culture, out value))
                    //{
                    //    record.Area = value;
                    //}
                    //else
                    //{
                    //    AddLog(record.RowNumber, "Некорректное число в поле 'AREA': " + area, false);
                    //    return record;
                    //}
                }

                var areaShare = GetValue(data, "AREASHARE");

                if (areaShare.IsEmpty())
                {
                    AddLog(record.RowNumber, "Не задана доля собственника.", false);
                    return record;
                }
                else
                {
                    decimal value;


                    if (TryGetPreisAsDecimal(areaShare.Replace('.', ','), out value))
                    {
                        record.AreaShare = value;
                    }
                    else
                    {
                        AddLog(record.RowNumber, "Некорректное число в поле 'AREASHARE': " + areaShare, false);
                        return record;
                    }

                    //if (decimal.TryParse(areaShare.Replace('.', ','), NumberStyles.Number, _culture, out value))
                    //{
                    //    record.AreaShare = value;
                    //}
                    //else
                    //{
                    //    AddLog(record.RowNumber, "Некорректное число в поле 'AREASHARE': " + areaShare, false);
                    //    return record;
                    //}
                }

                var ownerType = GetValue(data, "TYPEOWNER");

                if (ownerType.IsEmpty())
                {
                    AddLog(record.RowNumber, "Не задан тип собственника.", false);
                    return record;
                }
                else
                {
                    switch (ownerType)
                    {
                        case "0":
                            record.OwnerType = PersonalAccountOwnerType.Individual;
                            break;

                        case "1":
                            record.OwnerType = PersonalAccountOwnerType.Legal;
                            break;

                        default:
                            AddLog(record.RowNumber, "Неизвестный тип собственника.", false);
                            return record;
                    }

                }

                if (_realtyObjectsByAddressDict.ContainsKey(record.LocalityName))
                {
                    var localityStreets = _realtyObjectsByAddressDict[record.LocalityName];

                    if (localityStreets.ContainsKey(record.StreetName))
                    {
                        var streetHouses = localityStreets[record.StreetName].Where(x => x.House == record.House).ToList();

                        if (record.Housing.IsEmpty())
                        {
                            streetHouses = streetHouses.Where(x => string.IsNullOrWhiteSpace(x.Housing)).ToList();
                        }
                        else
                        {
                            streetHouses = streetHouses.Where(x => x.Housing == record.Housing).ToList();
                        }

                        if (streetHouses.Count == 0)
                        {
                            AddLog(record.RowNumber, "Дом не найден в системе", false);
                            return record;
                        }

                        record.RealtyObjectId = streetHouses.First().RoId;
                    }
                    else
                    {
                        var errText = string.Format("В населенном пункте '{0}' на улице '{1}' не найдены дома",
                            GetValue(data, "LOCALITY"), GetValue(data, "STREET"));
                        AddLog(record.RowNumber, errText, false);
                        return record;
                    }
                }
                else
                {
                    var errText = string.Format("В населенном пункте '{0}' не найдены дома", GetValue(data, "LOCALITY"));
                    AddLog(record.RowNumber, errText, false);
                    return record;
                }

                record.isValidRecord = true;

                return record;
            }
            catch (Exception exc)
            {
                
                throw exc;
            }
            
        }

        private void SaveData()
        {
            UpdateDecisionCache(_records);
            ProcessRooms(_records);

            foreach (var record in _records)
            {
                Room apartment = _cacheRooms.Get("{0}|{1}".FormatUsing(record.RealtyObjectId, record.Apartment));

                PersonalAccountOwner owner = record.OwnerType == PersonalAccountOwnerType.Individual
                    ? CreateOrGetIndividualOwner(record)
                    : CreateOrGetLegalOwner(record);

                var keyAccOwner = "{0}|{1}".FormatUsing(apartment.Id, owner.Id);
                
                if (_accountByRoomAndOwnerDict.ContainsKey(keyAccOwner))
                {
                    UpdateAccount(record, _accountByRoomAndOwnerDict[keyAccOwner]);
                }
                else
                {
                    var newAccount = CreateAccount(record, apartment, owner);
                    _accountByRoomAndOwnerDict.Add(keyAccOwner, newAccount.Id);
                }

                AddLog(record.RowNumber, "Успешно", true);
            }
        }

        private void UpdateDecisionCache(List<Record> records)
        {
            var realtyObjects = records.Select(x => new RealityObject {Id = x.RealtyObjectId}).ToList();
            _decisions = new Dictionary<long, Dictionary<Type, UltimateDecision>>();

            var decisions1 = _realityObjectDecisionsService
                .GetActualDecisionForCollection<CrFundFormationDecision>(realtyObjects, true);

            foreach (var kv in decisions1)
            {
                _decisions.Add(kv.Key.Id, new Dictionary<Type, UltimateDecision>
                {
                    { typeof (CrFundFormationDecision), kv.Value.As<CrFundFormationDecision>() }
                });
            }

            var decisions2 = _realityObjectDecisionsService
                .GetActualDecisionForCollection<AccountOwnerDecision>(realtyObjects, true);

            foreach (var kv in decisions2)
            {
                if (_decisions.ContainsKey(kv.Key.Id))
                {
                    _decisions[kv.Key.Id].Add(typeof(AccountOwnerDecision), kv.Value.As<AccountOwnerDecision>());
                }
                else
                {
                    _decisions.Add(kv.Key.Id, new Dictionary<Type, UltimateDecision>
                    {
                        { typeof (AccountOwnerDecision), kv.Value.As<AccountOwnerDecision>() }
                    });
                }
            }
        }

        private void ProcessRooms(List<Record> records)
        {
            foreach (var record in records)
            {
                var key = "{0}|{1}".FormatUsing(record.RealtyObjectId, record.Apartment);

                Room apartment = _cacheRooms.Get(key);

                if (apartment.IsNotNull())
                {
                    apartment.Area = record.Area;
                    apartment.LivingArea = record.Area;
                    apartment.Type = RoomType.Living;
                    apartment.OwnershipType = RoomOwnershipType.Private;

                    _roomRepository.Update(apartment);
                }
                else
                {
                    apartment = CreateApartment(record);

                    _cacheRooms.Add(key, apartment);
                }
            }
        }

        private void WriteLogs()
        {
            foreach (var log in _logDict.OrderBy(x => x.Key))
            {
                var rowNumber = string.Format("Строка {0}", log.Key);

                if (log.Value.Key)
                {
                    this.LogImport.Info(rowNumber, log.Value.Value);
                    this.LogImport.CountAddedRows++;
                }
                else
                {
                    this.LogImport.Warn(rowNumber, log.Value.Value);
                }
            }
        }

        private void AddLog(int rowNum, string message, bool success)
        {
            if (_logDict.ContainsKey(rowNum))
            {
                var log = _logDict[rowNum];

                if (log.Key == success)
                {
                    _logDict[rowNum] = new KeyValuePair<bool, string>(success,
                        string.Format("{0}; {1}", log.Value, message ?? string.Empty));
                }
                else if (log.Key)
                {
                    _logDict[rowNum] = new KeyValuePair<bool, string>(success, message ?? string.Empty);
                }
            }
            else
            {
                _logDict[rowNum] = new KeyValuePair<bool, string>(success, message ?? string.Empty);
            }
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

            var fileExtentions = PossibleFileExtensions.Contains(",")
                ? PossibleFileExtensions.Split(',')
                : new[] { PossibleFileExtensions };
            if (fileExtentions.All(x => x != extention))
            {
                message = string.Format("Необходимо выбрать файл с допустимым расширением: {0}", PossibleFileExtensions);
                return false;
            }

            var tableLocker = this.Container.Resolve<ITableLocker>();
            try
            {
                if (tableLocker.CheckLocked<BasePersonalAccount>("INSERT"))
                {
                    message = TableLockedException.StandardMessage;
                    return false;
                }
            }
            finally
            {
                this.Container.Release(tableLocker);
            }

            return true;
        }

        private Room CreateApartment(Record record)
        {
            var apartment = new Room
            {
                RealityObject = roCache.GetByKey(record.RealtyObjectId.ToString()),
                RoomNum = record.Apartment,
                Area = record.Area,
                LivingArea = record.Area,
                Type = RoomType.Living,
                OwnershipType = RoomOwnershipType.Private
            };

            _roomRepository.Save(apartment);

            return apartment;
        }

        private PersonalAccountOwner CreateOrGetIndividualOwner(Record record)
        {
            IndividualAccountOwner owner;

            var ownerNameParts = record.Owner.Trim().Split(' ').Where(x => x.IsNotEmpty()).ToList();

            var surname = ownerNameParts.Count > 0 ? ownerNameParts[0] : string.Empty;
            var firstName = ownerNameParts.Count > 1 ? ownerNameParts[1] : string.Empty;
            var secondName = ownerNameParts.Count > 2 ? ownerNameParts[2] : string.Empty;
            var fullOwnerName = string.Format("{0} {1} {2}", surname, firstName, secondName).ToLower().Trim();

            owner = individualAccCache.GetByKey(fullOwnerName);

            if (owner.IsNotNull())
                return owner;

            owner = new IndividualAccountOwner
            {
                OwnerType = PersonalAccountOwnerType.Individual,
                Surname = surname,
                FirstName = firstName,
                SecondName = secondName,
                IdentityNumber = string.Empty,
                IdentitySerial = string.Empty,
                IdentityType = 0
            };

            _individualAccountOwnerRepository.Save(owner);

            individualAccCache.AddEntity(owner);

            return owner;
        }

        private PersonalAccountOwner CreateOrGetLegalOwner(Record record)
        {
            LegalAccountOwner owner;

            Contragent contragent;

            contragent = contrCache.GetByKey(record.Owner);

            if (contragent.IsNull())
            {
                contragent = new Contragent { Name = record.Owner };

                contrCache.AddEntity(contragent);
                _contragentRepository.Save(contragent);
            }

            owner = legalCache.GetByKey(contragent.Id.ToStr());


            if (owner.IsNull())
            {
                owner = new LegalAccountOwner
                {
                    OwnerType = PersonalAccountOwnerType.Legal,
                    Contragent = contragent
                };

                _legalAccountOwnerRepository.Save(owner);
                legalCache.AddEntity(owner);
            }

            return owner;
        }

        protected virtual BasePersonalAccount CreateAccount(Record record, Room apartment, PersonalAccountOwner owner)
        {
            var account = new BasePersonalAccount
            {
                Room = apartment,
                AccountOwner = owner,
                AreaShare = record.AreaShare,
                OpenDate = _accountCreateDate,
                State = GetPersonalAccountState(
                    roCache.GetByKey(apartment.RealityObject.Id.ToString(CultureInfo.InvariantCulture)))
            };

            try
            {
                _accountNumberService.Generate(account);
            }
            catch (Exception exc)
            {
                throw new Exception(string.Format("Ошибка генерации ЛС по строке {0}", record.RowNumber), exc);
            }

            _basePersonalAccountRepository.Save(account);

            // после добавления нового счета добавляю его в кеш

            if (!_cacheAccounts.ContainsKey(account.Id))
            {
                _cacheAccounts.Add(account.Id, account);
            }

            return account;
        }

        protected virtual void UpdateAccount(Record record, long accountId)
        {
            BasePersonalAccount account = null;

            if (_cacheAccounts.ContainsKey(accountId))
            {
                account = _cacheAccounts[accountId];
            }
            else
            {
                return;
            }
            
            var prevAccNum = account.PersonalAccountNum;

            if (account.AreaShare != record.AreaShare
                || account.OpenDate != _accountCreateDate
                || account.PersonalAccountNum != prevAccNum)
            {
                account.AreaShare = record.AreaShare;
                account.OpenDate = _accountCreateDate;

                _basePersonalAccountRepository.Update(account);
            }
        }

        protected State GetPersonalAccountState(RealityObject realityObject)
        {
            var state = GetDefaultPersonalAccountState();
            var nonActivePersonalAccountState = GetNonActivePersonalAccountState();
            var openPersonalAccountState = GetOpenPersonalAccountState();

            if (realityObject.ConditionHouse == ConditionHouse.Emergency
                || realityObject.TypeHouse == TypeHouse.BlockedBuilding
                || realityObject.IsNotInvolvedCr)
            {
                return nonActivePersonalAccountState;
            }

            var crFundDecision = GetDecision<CrFundFormationDecision>(realityObject);
            if (crFundDecision == null)
            {
                if (nonActivePersonalAccountState != null)
                {
                    state = nonActivePersonalAccountState;
                }
            }
            else
            {
                switch (crFundDecision.Decision)
                {
                    case CrFundFormationDecisionType.Unknown:
                        if (nonActivePersonalAccountState != null)
                        {
                            state = nonActivePersonalAccountState;
                        }

                        break;
                    case CrFundFormationDecisionType.RegOpAccount:
                        if (openPersonalAccountState != null)
                        {
                            state = openPersonalAccountState;
                        }

                        break;
                    case CrFundFormationDecisionType.SpecialAccount:
                        var accOwnerDecision = GetDecision<AccountOwnerDecision>(realityObject);
                        if (accOwnerDecision == null)
                        {
                            if (nonActivePersonalAccountState != null)
                            {
                                state = nonActivePersonalAccountState;
                            }
                        }
                        else
                        {
                            switch (accOwnerDecision.DecisionType)
                            {
                                case AccountOwnerDecisionType.Custom:
                                    if (nonActivePersonalAccountState != null)
                                    {
                                        state = nonActivePersonalAccountState;
                                    }

                                    break;
                                case AccountOwnerDecisionType.RegOp:
                                    if (openPersonalAccountState != null)
                                    {
                                        state = openPersonalAccountState;
                                    }

                                    break;
                            }
                        }
                        break;
                }
            }
            return state;
        }

        private T GetDecision<T>(RealityObject realityObject) where T : UltimateDecision
        {
            if (_decisions.ContainsKey(realityObject.Id))
            {
                UltimateDecision decision;
                if (_decisions[realityObject.Id].TryGetValue(typeof(T), out decision))
                    return decision.As<T>();
            }

            return null;
        }

        private State GetDefaultPersonalAccountState()
        {
            return _defaultAccState ?? (_defaultAccState  = _stateDomainService.FirstOrDefault(x => x.TypeId == _persAccStateInfo.TypeId && x.StartState));
        }

        private State GetOpenPersonalAccountState()
        {
            return _accOpenState ?? (_accOpenState = _stateDomainService.FirstOrDefault(x => x.TypeId == _persAccStateInfo.TypeId && x.Code == "1"));
        }

        private State GetNonActivePersonalAccountState()
        {
            return _accActiveState ?? (_accActiveState = _stateDomainService.FirstOrDefault(x => x.TypeId == _persAccStateInfo.TypeId && x.Code == "4"));
        }

        private bool AccountHasDefaultState()
        {
            var account = new BasePersonalAccount();
            _stateProvider.SetDefaultState(account);

            return account.State != null;
        }

        private void InTransaction(Action action)
        {
            var transaction = this.Container.Resolve<IDataTransaction>();
            using (this.Container.Using(transaction))
            {
                try
                {
                    action();

                    transaction.Commit();
                }
                catch (Exception exc)
                {
                    try
                    {
                        transaction.Rollback();
                    }
                    catch (TransactionRollbackException ex)
                    {
                        throw new DataAccessException(ex.Message, exc);
                    }
                    catch (Exception e)
                    {
                        throw new DataAccessException(
                            string.Format(
                                "Произошла неизвестная ошибка при откате транзакции: \r\nMessage: {0}; \r\nStackTrace:{1};",
                                e.Message,
                                e.StackTrace),
                            exc);
                    }

                    throw;
                }
            }
        }

        public bool TryGetPreisAsDecimal(string price, out decimal convertedPrice)
        {
            convertedPrice = 0.0m;

            Match match = Regex.Match(price, @"\d+(\,\d{1,4})?");

            if (match.Success)
            {
                convertedPrice = decimal.Parse(match.Value);
                return true;
            }
            
            return false;
        }
    }
}