namespace Bars.Gkh.RegOperator.Imports.ImportRkc
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.FIAS;
    using Bars.B4.Modules.States;
    using Bars.B4.Utils;

    using Bars.Gkh.Domain.ParameterVersioning;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Import;
    using Bars.Gkh.Overhaul.Import.CommonRealtyObjectImport;
    using Bars.Gkh.RegOperator.Domain.ParametersVersioning;
    using Bars.Gkh.RegOperator.Domain.Repository;
    using Bars.Gkh.RegOperator.DomainService.PersonalAccount;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Entities.Wallet;
    using Bars.Gkh.RegOperator.Enums;
    using Bars.Gkh.RegOperator.Extenstions;
    using Bars.Gkh.RegOperator.Services.DataContracts;
    using Bars.Gkh.Repositories.ChargePeriod;
    using Bars.Gkh.Utils;

    using Castle.Windsor;
    using FastMember;
    using NHibernate;

    /// <summary>
    /// Метод для импорта лицевых счетов из PKЦ
    /// </summary>
    public class RkcImportService : IRkcImportService
    {
        #region DomainServices

        public IWindsorContainer Container { get; set; }
        public ILogImport LogImport { get; set; }
        public ISessionProvider SessionProvider { get; set; }
        public IAccountNumberService AccountNumberService { get; set; }
        public IDomainService<RealityObject> RealityObjectDomain { get; set; }
        public IDomainService<Contragent> ContragentDomain { get; set; }
        public IRepository<Fias> FiasRepository { get; set; }
        public IDomainService<FiasAddress> FiasAddressDomain { get; set; }
        public IRepository<Municipality> MunicipalityRepo { get; set; }
        public IDomainService<Room> RoomDomain { get; set; }
        public IDomainService<BasePersonalAccount> BasePersonalAccountDomain { get; set; }
        public IDomainService<PersonalAccountPeriodSummary> PersAccPeriodSummaryDomain { get; set; }
        public IChargePeriodRepository ChargePeriodRepository { get; set; }
        public IDomainService<EntityLogLight> EntityLogLightDomain { get; set; }
        public IDomainService<CashPaymentCenter> CashPaymentCenterDomain { get; set; }
        public IDomainService<CashPaymentCenterPersAcc> CashPaymentCenterPersAccDomain { get; set; }
        public IDomainService<IndividualAccountOwner> IndividualAccountOwnerDomain { get; set; }
        public IDomainService<LegalAccountOwner> LegalAccountOwnerDomain { get; set; }

        #endregion

        #region Dicts

        private long[] currentPeriodSummAccIds;
        private Dictionary<string, KeyValuePair<string, long>> fiasIdByMunicipalityNameDict;
        private Dictionary<string, List<RealtyObjectAddress>> realityObjectsByFiasGuidDict;
        private Dictionary<long, DateTime> roomAreaLastLogDict;
        private Dictionary<long, DateTime> accountOpenDateLastLogDict;
        private Dictionary<long, DateTime> accountAreaShareLastLogDict;
        private Dictionary<string, long> persAccByNumRkcInn;
        private Dictionary<string, long> rkcIdByInn;
        private Dictionary<long, string> rkcInnByPersAcc;
        private Dictionary<string, object> individualOwnersDict;
        private Dictionary<string, List<Contragent>> contragentsDict;
        private Dictionary<long, object> legalOwnersDict;
        private Dictionary<string, BasePersonalAccount> existAccount;
        private Dictionary<long, Dictionary<string, object>> realtyObjectRoomsDict;
        private Dictionary<long, Room> roomDict;
        private Dictionary<long, RealityObject> roDict;
        private Dictionary<string, Dictionary<long, object>> accountByRoomAndOwnerDict;
        private Dictionary<long, BasePersonalAccount> accountDict;
        private readonly Dictionary<Room, ProxyLogValue> roomLogToCreateDict = new Dictionary<Room, ProxyLogValue>();
        private readonly Dictionary<BasePersonalAccount, ProxyLogValue> accountAreaShareLogToCreateDict = new Dictionary<BasePersonalAccount, ProxyLogValue>();
        private readonly Dictionary<BasePersonalAccount, ProxyLogValue> accountOpenDateLogToCreateDict = new Dictionary<BasePersonalAccount, ProxyLogValue>();
        private readonly Dictionary<int, Dictionary<bool, List<string>>> logDict = new Dictionary<int, Dictionary<bool, List<string>>>();
        private readonly Dictionary<string, SyncRkcAccountResult> accResultDict = new Dictionary<string, SyncRkcAccountResult>();

        #endregion

        #region Lists to save

        private readonly HashSet<BasePersonalAccount> persAccToSave = new HashSet<BasePersonalAccount>();
        private readonly HashSet<Room> roomToSave = new HashSet<Room>();
        private readonly List<IndividualAccountOwner> indivOwnerToSave = new List<IndividualAccountOwner>();
        private readonly List<Contragent> contragentToSave = new List<Contragent>();
        private readonly List<LegalAccountOwner> legalOwnerToSave = new List<LegalAccountOwner>();
        private readonly List<EntityLogLight> entityLogLightToSave = new List<EntityLogLight>();
        private readonly List<CashPaymentCenterRealObj> rkcRealObjToSave = new List<CashPaymentCenterRealObj>();
        private readonly List<PersonalAccountPeriodSummary> periodSummariesToSave = new List<PersonalAccountPeriodSummary>();

        #endregion

        private FiasHelper fiasHelper;
        private int rowNum;
        private State accountDefaultState;
        private CultureInfo culture;
        private ChargePeriod currentPeriod;

        public ILogImport Import(ImportRkcRecord record)
        {
            if (!AccountHasDefaultState())
            {
                LogImport.Error("Ошибка", "Не создан начальный статус для сущности 'Лицевой счет(модуль Регоператора)'!");
                return LogImport;
            }

            culture = CultureInfo.CreateSpecificCulture("ru-RU");

            currentPeriod = ChargePeriodRepository.GetCurrentPeriod();

            InitDictionaries();

            rowNum = 0;
            foreach (var persAccInfo in record.PersonalAccounts)
            {
                rowNum++;

                var realObjId = GetRealObjId(persAccInfo);

                if (realObjId == 0)
                {
                    continue;
                }

                var room = GetOrCreateRoom(persAccInfo, realObjId);
                var owner = persAccInfo.AccType == "1" ? GetIndividualOwner(persAccInfo, room) : GetLegalOwner(persAccInfo);

                CreateOrUpdateAccount(persAccInfo, room, owner, realObjId, record.RkcInn);
            }

            InTransaction();

            WriteLogs();

            // Намеренно закрываем текущую сессию, иначе при каждом коммите транзакции
            // ранее измененные дома вызывают каскадирование ФИАС
            Container.Resolve<ISessionProvider>().CloseCurrentSession();

            return LogImport;
        }

        public List<SyncRkcAccountResult> GetAccountResult()
        {
            return accResultDict.Select(x => x.Value).ToList();
        }

        private void InitDictionaries()
        {
            fiasHelper = new FiasHelper(Container);

            realityObjectsByFiasGuidDict = FiasRepository.GetAll()
                .Join(
                    FiasAddressDomain.GetAll(),
                    x => x.AOGuid,
                    y => y.StreetGuidId,
                    (a, b) => new { fias = a, fiasAddress = b })
                .Join(
                    RealityObjectDomain.GetAll(),
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
                    MunicipalityId = x.realityObject.Municipality.Id
                })
                .AsEnumerable()
                .Where(x => !string.IsNullOrWhiteSpace(x.AoGuid))
                .GroupBy(x => x.AoGuid)
                .ToDictionary(x => x.Key, x => x.ToList());

            var municipalities = MunicipalityRepo.GetAll().ToArray();

            fiasIdByMunicipalityNameDict = municipalities
                .Where(x => x.Name != null)
                .Select(x => new { x.Name, x.FiasId, x.Id })
                .AsEnumerable()
                .GroupBy(x => x.Name.ToUpper().Trim())
                .ToDictionary(
                    x => x.Key,
                    x =>
                    {
                        var first = x.First();
                        return new KeyValuePair<string, long>(first.FiasId, first.Id);
                    });

            roomAreaLastLogDict = EntityLogLightDomain.GetAll()
                .Where(x => x.ClassName == typeof(Gkh.Entities.Room).Name)
                .Where(x => x.ParameterName == VersionedParameters.RoomArea)
                .Select(x => new { x.EntityId, x.Id, x.DateActualChange })
                .AsEnumerable()
                .GroupBy(x => x.EntityId)
                .ToDictionary(x => x.Key, x => x.OrderByDescending(y => y.Id).First().DateActualChange);

            accountOpenDateLastLogDict = EntityLogLightDomain.GetAll()
                .Where(x => x.ClassName == typeof(BasePersonalAccount).Name)
                .Where(x => x.ParameterName == VersionedParameters.PersonalAccountOpenDate)
                .Select(x => new { x.EntityId, x.Id, x.DateActualChange })
                .AsEnumerable()
                .GroupBy(x => x.EntityId)
                .ToDictionary(x => x.Key, x => x.OrderByDescending(y => y.Id).First().DateActualChange);

            accountAreaShareLastLogDict = EntityLogLightDomain.GetAll()
                .Where(x => x.ClassName == typeof(BasePersonalAccount).Name)
                .Where(x => x.ParameterName == VersionedParameters.AreaShare)
                .Select(x => new { x.EntityId, x.Id, x.DateActualChange })
                .AsEnumerable()
                .GroupBy(x => x.EntityId)
                .ToDictionary(x => x.Key, x => x.OrderByDescending(y => y.Id).First().DateActualChange);

            persAccByNumRkcInn = BasePersonalAccountDomain.GetAll()
                .Join(CashPaymentCenterPersAccDomain.GetAll(),
                    x => x.Id,
                    y => y.PersonalAccount.Id,
                    (a, b) => new { Acc = a, RkcPa = b })
                .Where(x => x.Acc.PersAccNumExternalSystems != null && x.Acc.PersAccNumExternalSystems != string.Empty)
                .Where(x => x.RkcPa.DateStart <= DateTime.Now && (!x.RkcPa.DateEnd.HasValue || DateTime.Now <= x.RkcPa.DateEnd))
                .Select(x => new
                {
                    x.Acc.Id,
                    x.Acc.PersAccNumExternalSystems,
                    x.RkcPa.CashPaymentCenter.Contragent.Inn
                })
                .AsEnumerable()
                .GroupBy(x => "{0}_{1}".FormatUsing(x.PersAccNumExternalSystems, x.Inn))
                .ToDictionary(x => x.Key, x => x.Select(y => y.Id).First());

            currentPeriodSummAccIds = PersAccPeriodSummaryDomain.GetAll()
                .Where(x => x.Period.Id == currentPeriod.Id)
                .Select(x => x.PersonalAccount.Id)
                .Distinct()
                .ToArray();

            rkcIdByInn = CashPaymentCenterDomain.GetAll()
                .Select(x => new
                {
                    x.Id,
                    x.Contragent.Inn
                })
                .AsEnumerable()
                .GroupBy(x => x.Inn)
                .ToDictionary(x => x.Key, y => y.Select(x => x.Id).First());

            rkcInnByPersAcc = CashPaymentCenterPersAccDomain.GetAll()
                .Where(x => x.DateStart <= DateTime.Now && (!x.DateEnd.HasValue || DateTime.Now <= x.DateEnd))
                .Select(x => new
                {
                    x.PersonalAccount.Id,
                    x.CashPaymentCenter.Contragent.Inn
                })
                .AsEnumerable()
                .GroupBy(x => x.Id)
                .ToDictionary(x => x.Key, y => y.Select(x => x.Inn).First());

            contragentsDict = ContragentDomain.GetAll()
                    .Where(x => x.Inn != null && x.Inn != string.Empty)
                    .GroupBy(x => x.Inn)
                    .ToDictionary(x => x.Key, x => x.ToList());

            legalOwnersDict = LegalAccountOwnerDomain.GetAll()
                .Select(x => new { contragentId = x.Contragent.Id, x.Id })
                .AsEnumerable()
                .GroupBy(x => x.contragentId)
                .ToDictionary(x => x.Key, x => (object)x.First().Id);

            var tmpLegalownerDict = LegalAccountOwnerDomain.GetAll()
                .Select(x => new { x.Contragent.Name, x.Contragent.Inn, x.Contragent.Kpp, x.Id })
                .AsEnumerable()
                .ToDictionary(x => x.Id, y => string.Format("{0}_{1}_{2}", y.Name, y.Inn, y.Kpp).Replace(" ", string.Empty).Trim().ToLower());

            var tmpIndivownerDict = IndividualAccountOwnerDomain.GetAll()
                .Select(x => new
                {
                    x.Id,
                    ownerSurname = x.Surname ?? string.Empty,
                    ownerFirstName = x.FirstName ?? string.Empty,
                    ownerSecondName = x.SecondName ?? string.Empty
                })
                .AsEnumerable()
                .ToDictionary(x => x.Id,
                    y => string.Format("{0}_{1}_{2}",
                        y.ownerSurname,
                        y.ownerFirstName,
                        y.ownerSecondName).Replace(" ", string.Empty).Trim().ToLower());

            existAccount = BasePersonalAccountDomain.GetAll()
                .Select(x => new { RoId = x.Room.RealityObject.Id, x.Room.RoomNum, x.AccountOwner.Id, Account = x })
                .AsEnumerable()
                .GroupBy(x =>
                    string.Format("{0}#{1}#{2}",
                        x.RoId,
                        x.RoomNum,
                        tmpIndivownerDict.Get(x.Id) ?? tmpLegalownerDict.Get(x.Id)).ToLower().Trim())
                .ToDictionary(x => x.Key, y => y.Select(x => x.Account).First());

            realtyObjectRoomsDict = RoomDomain.GetAll()
                .Select(x => new
                {
                    roId = x.RealityObject.Id,
                    x.RoomNum,
                    x.Id
                })
                .AsEnumerable()
                .GroupBy(x => x.roId)
                .ToDictionary(
                    x => x.Key,
                    x => x.GroupBy(y => y.RoomNum)
                        .ToDictionary(y => y.Key, y => (object)y.First().Id));

            roomDict = RoomDomain.GetAll().ToDictionary(x => x.Id);

            roDict = RealityObjectDomain.GetAll().ToDictionary(x => x.Id);

            accountDict = BasePersonalAccountDomain.GetAll().ToDictionary(x => x.Id);

            accountByRoomAndOwnerDict = BasePersonalAccountDomain.GetAll()
                    .Where(x => !x.State.FinalState)
                    .Select(x => new
                    {
                        accountId = x.Id,
                        roomNum = x.Room.RoomNum,
                        roId = x.Room.RealityObject.Id,
                        RoomId = x.Room.Id
                    })
                    .AsEnumerable()
                    .GroupBy(x => string.Format("{0}_{1}", x.roId, x.roomNum))
                    .ToDictionary(
                        x => x.Key,
                        x => x.GroupBy(y => y.RoomId)
                            .ToDictionary(y => y.Key, y => (object)y.First().accountId));


            var existOwners = BasePersonalAccountDomain.GetAll()
                .Select(x => new { RoomId = x.Room.Id, x.AccountOwner.Id })
                .AsEnumerable()
                .GroupBy(x => x.Id)
                .ToDictionary(x => x.Key, y => y.Select(z => z.RoomId).FirstOrDefault());

            individualOwnersDict = IndividualAccountOwnerDomain.GetAll()
                        .Select(x => new
                        {
                            x.Id,
                            ownerSurname = x.Surname ?? string.Empty,
                            ownerFirstName = x.FirstName ?? string.Empty,
                            ownerSecondName = x.SecondName ?? string.Empty
                        })
                        .AsEnumerable()
                        .GroupBy(y =>
                            string.Format("{0}_{1}_{2}_{3}",
                                existOwners.ContainsKey(y.Id) ? existOwners[y.Id].ToStr() : string.Empty,
                                y.ownerSurname.Trim(),
                                y.ownerFirstName.Trim(),
                                y.ownerSecondName.Trim()).Replace(" ", string.Empty).Trim().ToLower())
                        .ToDictionary(y => y.Key, y => (object)y.First().Id);
        }

        private long GetRealObjId(ImportRkcPersonalAccount persAccInfo)
        {
            if (string.IsNullOrEmpty(persAccInfo.Mu))
            {
				AddLog(rowNum, persAccInfo.AccNumber, "Не задано муниципальное образование", false);
                return 0;
            }

            var houseNum = persAccInfo.HouseNum + persAccInfo.Liter;
            if (string.IsNullOrEmpty(houseNum))
            {
                AddLog(rowNum, persAccInfo.AccNumber, "Не задан номер дома", false);
                return 0;
            }

            var mu = persAccInfo.Mu.Trim().ToUpper();
            if (!fiasIdByMunicipalityNameDict.ContainsKey(mu))
            {
                var errText = string.Format("В справочнике муниципальных образований не найдена запись: {0}", persAccInfo.Mu);
                AddLog(rowNum, persAccInfo.AccNumber, errText, false);
                return 0;
            }

            var municipality = fiasIdByMunicipalityNameDict[mu];
            var fiasGuid = municipality.Key;

            if (string.IsNullOrWhiteSpace(fiasGuid))
            {
                AddLog(rowNum, persAccInfo.AccNumber, "Муниципальное образование не привязано к ФИАС", false);
                return 0;
            }

            if (!fiasHelper.HasBranch(fiasGuid))
            {
                AddLog(rowNum, persAccInfo.AccNumber, "В структуре ФИАС не найдена актуальная запись для муниципального образования", false);
                return 0;
            }

            var faultReason = string.Empty;
            DynamicAddress address;

            var place = "{0} {1}".FormatUsing(persAccInfo.City, persAccInfo.TypeCity);
            var street = "{0} {1}".FormatUsing(persAccInfo.Street, persAccInfo.TypeStreet);

            if (!fiasHelper.FindInBranch(fiasGuid, place, street, ref faultReason, out address))
            {
                AddLog(rowNum, persAccInfo.AccNumber, faultReason, false);
                return 0;
            }

            // Проверяем есть ли дома на данной улице
            if (realityObjectsByFiasGuidDict.ContainsKey(address.GuidId))
            {
                // Составляем список домов на данной улице (!) с проверкой привязки МО (это не бред, после разделения одного МО многое может произойти)
                var existingRealityObjects = realityObjectsByFiasGuidDict[address.GuidId].Where(x => x.MunicipalityId == municipality.Value).ToList();

                if (existingRealityObjects.Any())
                {
                    var result = existingRealityObjects.Where(x => x.House == houseNum).ToList();

                    result = string.IsNullOrEmpty(persAccInfo.Korpus)
                                 ? result.Where(x => string.IsNullOrEmpty(x.Housing)).ToList()
                                 : result.Where(x => x.Housing == persAccInfo.Korpus).ToList();

                    result = string.IsNullOrEmpty(persAccInfo.Building)
                                 ? result.Where(x => string.IsNullOrEmpty(x.Building)).ToList()
                                 : result.Where(x => x.Building == persAccInfo.Building).ToList();

                    if (result.Count == 0)
                    {
                        AddLog(rowNum, persAccInfo.AccNumber, "В системе не найден дом, соответствующий записи", false);
                    }
                    else if (result.Count > 1)
                    {
                        AddLog(rowNum, persAccInfo.AccNumber, "В системе найдено несколько домов, соответствующих записи", false);
                    }
                    else
                    {
                        return result.First().RealityObjectId;
                    }
                }
                else
                {
                    AddLog(rowNum, persAccInfo.AccNumber, "У данного адреса другое муниципальное образование", false);
                }
            }

            return 0;
        }

        private PersonalAccountOwner GetIndividualOwner(ImportRkcPersonalAccount record, Gkh.Entities.Room room)
        {
            var key = string.Format("{0}_{1}_{2}_{3}", room.Id, record.Surname, record.Name, record.LastName).Replace(" ", string.Empty).Trim().ToLower();

            if (individualOwnersDict.ContainsKey(key))
            {
                if (individualOwnersDict[key] is long)
                {
                    return new IndividualAccountOwner { Id = (long)individualOwnersDict[key] };
                }

                if (individualOwnersDict[key] is IndividualAccountOwner)
                {
                    return (IndividualAccountOwner)individualOwnersDict[key];
                }
            }

            var owner = new IndividualAccountOwner
            {
                OwnerType = PersonalAccountOwnerType.Individual,
                Surname = record.Surname,
                FirstName = record.Name,
                SecondName = record.LastName,
                IdentityNumber = string.Empty,
                IdentitySerial = string.Empty,
                IdentityType = 0
            };


            AddLog(rowNum, record.AccNumber, "Добавлен новый владелец счета " + owner.Name);

            indivOwnerToSave.Add(owner);

            individualOwnersDict[key] = owner;

            return owner;
        }

        private PersonalAccountOwner GetLegalOwner(ImportRkcPersonalAccount record)
        {
            Contragent contragent;

            if (contragentsDict.ContainsKey(record.Inn))
            {
                var contragentList = contragentsDict[record.Inn];

                if (contragentList.Count > 1)
                {
                    AddLog(rowNum, record.AccNumber, "В системе заведены несколько контрагентов c таким ИНН", false);
                    return null;
                }

                contragent = contragentList.First();

                if (contragent.Kpp.IsNotEmpty() && contragent.Kpp != record.Kpp)
                {
                    AddLog(rowNum, record.AccNumber, "Не совпадает КПП у контрагента c таким ИНН", false);
                    return null;
                }

                contragent.Kpp = record.Kpp;

                if (contragent.Name != record.RenterName && record.RenterName.IsNotEmpty())
                {
                    contragent.Name = record.RenterName;
                    contragentToSave.Add(contragent);
                }
            }
            else
            {
                if (!Utils.VerifyInn(record.Inn, true))
                {
                    AddLog(rowNum, record.AccNumber, "Указаный ИНН не корректен", false);
                    return null;
                }

                contragent = new Contragent { Name = record.RenterName, Inn = record.Inn, Kpp = record.Kpp };
                contragentToSave.Add(contragent);
            }

            if (legalOwnersDict.ContainsKey(contragent.Id))
            {
                if (legalOwnersDict[contragent.Id] is long)
                {
                    return new LegalAccountOwner { Id = (long)legalOwnersDict[contragent.Id] };
                }

                if (legalOwnersDict[contragent.Id] is LegalAccountOwner)
                {
                    return (LegalAccountOwner)legalOwnersDict[contragent.Id];
                }
            }

            var owner = new LegalAccountOwner
            {
                OwnerType = PersonalAccountOwnerType.Legal,
                Contragent = contragent
            };

            legalOwnerToSave.Add(owner);

            legalOwnersDict[contragent.Id] = owner;

            return owner;
        }

        private BasePersonalAccount NewAccount(ImportRkcPersonalAccount record, Gkh.Entities.Room apartment, PersonalAccountOwner owner, long realObjId)
        {
            var accountKey = string.Format("{0}#{1}#{2}",
                realObjId,
                apartment.RoomNum,
                record.AccType == "1"
                    ? string.Format("{0}_{1}_{2}", record.Surname, record.Name, record.LastName)
                    : string.Format("{0}_{1}_{2}", record.RenterName, record.Inn, record.Kpp))
                .Replace(" ", string.Empty)
                .ToLower();

            if (existAccount.ContainsKey(accountKey))
            {
                return existAccount[accountKey];
            }

            var account = new BasePersonalAccount
            {
                Room = apartment,
                AccountOwner = owner,
                AreaShare = record.Share.ToDecimal(),
                OpenDate = record.AccNumberDate.ToDateTime(),
                State = accountDefaultState,
                PersAccNumExternalSystems = record.AccNumber
            };

            existAccount[accountKey] = account;

            if (string.IsNullOrWhiteSpace(account.PersonalAccountNum))
            {
                AccountNumberService.Generate(account);
            }

            AddLog(rowNum, record.AccNumber,
                string.Format("Добавлен новый лицевой счет с номером {0}; доля собственности {1}; дата открытия {2};",
                    account.PersonalAccountNum,
                    account.AreaShare.RegopRoundDecimal(2),
                    account.OpenDate.ToShortDateString()));

            return account;
        }

        private Room GetOrCreateRoom(ImportRkcPersonalAccount record, long realObjId)
        {
            Room apartment = null;
            var areaChangeDateTime = record.ChangeDateTotalArea.IsNotEmpty() ? record.ChangeDateTotalArea.ToDateTime() : DateTime.Now;
            var totalArea = record.TotalArea.ToDecimal();
            var liveArea = record.LiveArea.ToDecimal();
            var flatPlaceType = record.FlatPlaceType == "1" ? (RoomType?)RoomType.Living :
               record.FlatPlaceType == "2" ? (RoomType?)RoomType.NonLiving : null;
            var propertyType = record.PropertyType == "1" ? RoomOwnershipType.Private :
                record.PropertyType == "2" ? RoomOwnershipType.Municipal : RoomOwnershipType.NotSet;

            if (realtyObjectRoomsDict.ContainsKey(realObjId))
            {
                var realtyObjectRooms = realtyObjectRoomsDict[realObjId];

                if (realtyObjectRooms.ContainsKey(record.FlatPlaceNum))
                {
                    // Необходимо рассматривать только те записи которые в списке как Id а не как объект
                    // Если запись как объект значит она уже учтена в рачете и ее редактирвоать ненадо
                    if (realtyObjectRooms[record.FlatPlaceNum] is long)
                    {
                        apartment = roomDict[(long)realtyObjectRooms[record.FlatPlaceNum]];
                    }
                    else if (realtyObjectRooms[record.FlatPlaceNum] is Room)
                    {
                        apartment = (Room)realtyObjectRooms[record.FlatPlaceNum];
                    }

                    if (apartment == null)
                    {
                        return null;
                    }

                    if (!roomAreaLastLogDict.ContainsKey(apartment.Id))
                    {
                        roomLogToCreateDict[apartment] = new ProxyLogValue
                        {
                            DateActualChange = areaChangeDateTime,
                            Value = totalArea.ToString("G29", culture)
                        };
                    }
                    else
                    {
                        var lastActualSinceLog = roomAreaLastLogDict[apartment.Id];

                        if (apartment.Area != record.TotalArea.ToDecimal() || lastActualSinceLog != areaChangeDateTime)
                        {
                            roomLogToCreateDict[apartment] = new ProxyLogValue
                            {
                                DateActualChange = areaChangeDateTime,
                                Value = totalArea.ToString("G29", culture)
                            };
                        }

                        if (apartment.Area != record.TotalArea.ToDecimal() && lastActualSinceLog < areaChangeDateTime)
                        {
                            apartment.Area = record.TotalArea.ToDecimal();
                        }
                    }

                    if (apartment.Area != totalArea
                        || apartment.LivingArea != liveArea
                        || (flatPlaceType != null && apartment.Type != flatPlaceType)
                        || (propertyType != RoomOwnershipType.NotSet && apartment.OwnershipType != propertyType))
                    {
                        AddLog(rowNum, record.AccNumber,
                            string.Format(
                                "Изменение информации о помещении." +
                                "Старые значения: площадь {0}; жилая площадь {1}; тип помещения {2}; тип собственности {3}." +
                                "Новые значения: площадь {4}; жилая площадь {5}; тип помещения {6}; тип собственности {7}",
                                apartment.Area,
                                apartment.LivingArea,
                                apartment.Type.GetEnumMeta().Display,
                                apartment.OwnershipType.GetEnumMeta().Display,
                                totalArea > 0 ? totalArea : apartment.Area,
                                liveArea > 0 ? liveArea : apartment.LivingArea,
                                flatPlaceType != null ? flatPlaceType.Value : apartment.Type,
                                propertyType != RoomOwnershipType.NotSet ? propertyType : apartment.OwnershipType));

                        apartment.Area = totalArea > 0 ? totalArea : apartment.Area;
                        apartment.LivingArea = liveArea > 0 ? liveArea : apartment.LivingArea;
                        apartment.Type = flatPlaceType != null ? flatPlaceType.Value : apartment.Type;
                        apartment.OwnershipType = propertyType != RoomOwnershipType.NotSet ? propertyType : apartment.OwnershipType;


                        realtyObjectRooms[record.FlatPlaceNum] = apartment;
                        
                        roomToSave.Add(apartment);
                    }
                }
                else
                {
                    apartment = NewApartment(record, realObjId);
                    roomToSave.Add(apartment);
                    realtyObjectRooms[record.FlatPlaceNum] = apartment;
                }
            }
            else
            {
                apartment = NewApartment(record, realObjId);
                var realtyObjectRooms = new Dictionary<string, object>();
                roomToSave.Add(apartment);
                realtyObjectRooms[record.FlatPlaceNum] = apartment;
                realtyObjectRoomsDict[realObjId] = realtyObjectRooms;
            }

            return apartment;
        }

        private Room NewApartment(ImportRkcPersonalAccount record, long realObjId)
        {
            var room = new Room
            {
                RealityObject = roDict.Get(realObjId),
                RoomNum = record.FlatPlaceNum,
                Area = record.TotalArea.ToDecimal(),
                LivingArea = record.LiveArea.ToDecimal(),
                Type = record.FlatPlaceType == "2" ? RoomType.NonLiving : RoomType.Living,
                OwnershipType = record.PropertyType == "1" ? RoomOwnershipType.Private :
                record.PropertyType == "2" ? RoomOwnershipType.Municipal : RoomOwnershipType.NotSet
            };

            AddLog(rowNum, record.AccNumber,
                string.Format(
                    "Добавлено новое помещение по адресу {0}, кв. {1}; площадь {2}; жилая площадь {3};тип помещения {4}; форма собственности {5}",
                    room.RealityObject.Address,
                    room.RoomNum,
                    room.Area.RegopRoundDecimal(2),
                    room.LivingArea.HasValue ? room.LivingArea.Value.RegopRoundDecimal(2).ToString() : "",
                    room.Type.GetEnumMeta().Display,
                    room.OwnershipType.GetEnumMeta().Display));

            return room;
        }

        private void CreateOrUpdateAccount(ImportRkcPersonalAccount record, Gkh.Entities.Room apartment, PersonalAccountOwner owner, long realObjId, string rkcInn)
        {
            if (owner == null)
            {
                return;
            }

            BasePersonalAccount account = null;
            var shareChangeDate = record.ChangeDateShare.IsNotEmpty() ? record.ChangeDateShare.ToDateTime() : DateTime.Now;
            var share = record.Share.ToDecimal();
            var openDate = record.AccNumberDate.IsNotEmpty() ? record.AccNumberDate.ToDateTime() : DateTime.Now;
            var changeOpenDate = record.ChangeDateAccNumberDate.IsNotEmpty() ? record.ChangeDateAccNumberDate.ToDateTime() : DateTime.Now;
            var accNum = record.AccNumber;

            var key = string.Format("{0}_{1}", realObjId, apartment.RoomNum);
            if (accountByRoomAndOwnerDict.ContainsKey(key))
            {
                var accountsInRoom = accountByRoomAndOwnerDict[key];

                var rkcInnNumKey = "{0}_{1}".FormatUsing(record.AccNumber, rkcInn);

                if (persAccByNumRkcInn.ContainsKey(rkcInnNumKey) || accountsInRoom.ContainsKey(apartment.Id))
                {
                    if (persAccByNumRkcInn.ContainsKey(rkcInnNumKey))
                    {
                        account = accountDict.Get(persAccByNumRkcInn.Get(rkcInnNumKey));
                    }
                    else
                    {
                        if (accountsInRoom[apartment.Id] is long)
                        {
                            if (accountDict.ContainsKey((long)accountsInRoom[apartment.Id]))
                            {
                                account = accountDict[(long)accountsInRoom[apartment.Id]];
                            }
                        }
                        else if (accountsInRoom[apartment.Id] is BasePersonalAccount)
                        {
                            account = (BasePersonalAccount)accountsInRoom[apartment.Id];
                        }
                    }

                    if (account != null && CheckAreaShareSum(record, account, realObjId))
                    {
                        if (accountAreaShareLastLogDict.ContainsKey(account.Id))
                        {
                            var lastActualSinceLog = accountAreaShareLastLogDict[account.Id];

                            if (account.AreaShare != share
                                || lastActualSinceLog != shareChangeDate)
                            {
                                accountAreaShareLogToCreateDict[account] = new ProxyLogValue
                                {
                                    DateActualChange = shareChangeDate,
                                    Value = share.ToString("G29", culture)
                                };
                            }


                            if (account.AreaShare != share && lastActualSinceLog < shareChangeDate)
                            {
                                account.AreaShare = share;
                            }
                        }

                        if (!accountOpenDateLastLogDict.ContainsKey(account.Id))
                        {
                            accountOpenDateLogToCreateDict[account] = new ProxyLogValue
                            {
                                DateActualChange = shareChangeDate,
                                Value = openDate.ToString("dd.MM.yyyy")
                            };
                        }
                        else
                        {
                            var lastActualSinceLog = accountOpenDateLastLogDict[account.Id];

                            if (account.OpenDate != openDate
                                || lastActualSinceLog != changeOpenDate)
                            {
                                accountOpenDateLogToCreateDict[account] = new ProxyLogValue
                                {
                                    DateActualChange = shareChangeDate,
                                    Value = openDate.ToString("dd.MM.yyyy")
                                };
                            }

                            if (account.OpenDate != openDate && lastActualSinceLog < changeOpenDate)
                            {
                                account.OpenDate = openDate;
                            }
                        }

                        if (account.PersAccNumExternalSystems.IsEmpty())
                        {
                            account.PersAccNumExternalSystems = accNum;
                        }
                        else if (account.PersAccNumExternalSystems != accNum)
                        {
                            AddLog(rowNum, record.AccNumber, "Похожий лицевой счет имеет другой номер во внешних системах: " + account.PersAccNumExternalSystems);
                        }

                        account.AccountOwner = owner;
                        persAccToSave.Add(account);
                    }
                }
                else
                {
                    try
                    {
                        account = NewAccount(record, apartment, owner, realObjId);
                        account.PersAccNumExternalSystems = accNum;

                        if (CheckAreaShareSum(record, account, realObjId))
                        {
                            persAccToSave.Add(account);
                        }
                    }
                    catch (Exception e)
                    {
                        AddLog(rowNum, record.AccNumber, e.Message, false);
                    }
                }
            }
            else
            {
                try
                {
                    account = NewAccount(record, apartment, owner, realObjId);
                    account.PersAccNumExternalSystems = accNum;

                    if (CheckAreaShareSum(record, account, realObjId))
                    {
                        persAccToSave.Add(account);
                        var accountsInRoom = new Dictionary<string, object>();
                        accountsInRoom[record.FlatPlaceNum] = account;
                    }
                }
                catch (Exception e)
                {
                    AddLog(rowNum, record.AccNumber, e.Message, false);
                }
            }

            AddPersAccPeriodSummary(account);

            AddRkcRelation(account, rkcInn);
        }

        private bool CheckAreaShareSum(ImportRkcPersonalAccount persAccInfo, BasePersonalAccount persAcc, long realObjId)
        {
            var key = string.Format("{0}_{1}", realObjId, persAccInfo.FlatPlaceNum);

            if (accountByRoomAndOwnerDict.ContainsKey(key))
            {
                var accountsInRoom = accountByRoomAndOwnerDict[key];

                // проверяем суммарную долю собственности у жилища.
                var areaShareSum = persAccInfo.Share.ToDecimal();
                foreach (var roomAcc in accountsInRoom.Values)
                {
                    var areaShare = 0m;

                    if (roomAcc is long)
                    {
                        var accNum = (long)roomAcc;
                        if (accNum != persAcc.Id && accountDict.ContainsKey(accNum))
                        {
                            areaShare = accountDict[(long)roomAcc].AreaShare;
                        }
                    }
                    else if (roomAcc is BasePersonalAccount)
                    {
                        var acc = (BasePersonalAccount)roomAcc;
                        if (acc.Id != persAcc.Id)
                        {
                            areaShare = ((BasePersonalAccount)roomAcc).AreaShare;
                        }
                    }

                    areaShareSum += areaShare;
                }

                if (areaShareSum > 1)
                {
                    AddLog(rowNum, persAccInfo.AccNumber, "Суммарная доля собственности у жилища больше 1. Лицевой счет не создан!", false);
                    return false;
                }
            }

            return persAccInfo.Share.ToDecimal() <= 1;
        }

        private void WriteLogs()
        {
            foreach (var log in logDict.OrderBy(x => x.Key))
            {
                var rowNumber = string.Format("Строка {0}. Л/с с файла: {1}", log.Key, log.Value.First());

                foreach (var rowLog in log.Value)
                {
                    if (rowLog.Key)
                    {
                        LogImport.Info(rowNumber, rowLog.Value.Skip(1).AggregateWithSeparator("."));
                        LogImport.CountAddedRows++;
                    }
                    else
                    {
                        LogImport.Warn(rowNumber, rowLog.Value.Skip(1).AggregateWithSeparator("."));
                    }
                }
            }
        }

        private void AddLog(int rowNum, string accNum, string message, bool success = true)
        {
            var accResult = new SyncRkcAccountResult
            {
                AccountNumber = accNum,
                Code = success ? "00" : "02",
                Description = message
            };

            if (!accResultDict.ContainsKey(accNum))
            {
                accResultDict.Add(accNum, accResult);
            }
            else
            {
                if (accResultDict[accNum].Code == "00" && !success)
                {
                    accResultDict[accNum] = accResult;
                }
            }

            if (!logDict.ContainsKey(rowNum))
            {
                logDict[rowNum] = new Dictionary<bool, List<string>>();
            }

            var log = logDict[rowNum];

            if (!log.ContainsKey(success))
            {
                log[success] = new List<string> { accNum };
            }

            log[success].Add(message);
        }

        private bool AccountHasDefaultState()
        {
            var stateProvider = Container.Resolve<IStateProvider>();

            try
            {
                var account = new BasePersonalAccount();

                stateProvider.SetDefaultState(account);

                accountDefaultState = account.State;

                return account.State != null;
            }
            finally
            {
                Container.Release(stateProvider);
            }
        }

        private void InTransaction()
        {
            SessionProvider.CloseCurrentSession();

            var login = string.Empty;// UserRepo.Get(Identity.UserId).Return(u => u.Login);

            if (login.IsEmpty())
            {
                login = "anonymous";
            }

            PrepareEntityLogToSave(login);

            var props = typeof(BasePersonalAccount).GetProperties().Where(x => x.PropertyType == typeof(Wallet)).ToArray();
            var accessor = TypeAccessor.Create(typeof(BasePersonalAccount), true);

            try
            {
                using (var session = SessionProvider.OpenStatelessSession())
                {
                    session.SetBatchSize(1000);

                    var persistenceContext = session.GetSessionImplementation().PersistenceContext;

                    using (var tr = session.BeginTransaction())
                    {
                        foreach (var item in roomToSave)
                        {
                            if (item.Id > 0)
                            {
                                // Без этого не получается сохранять Room в statelessSession, 
                                // т.к. некоторые (не все!) существующие записи видятся как RoomProxy
                                // Кто знает как, поправьте, пожалуйста
                                var room = (Room)persistenceContext.Unproxy(item);

                                session.Update(room);
                            }
                            else
                            {
                                session.Insert(item);

                                LogEntity(item, session, login);
                            }
                        }

                        foreach (var item in indivOwnerToSave)
                        {
                            if (item.Id > 0)
                                session.Update(item);
                            else
                                session.Insert(item);
                        }

                        foreach (var item in contragentToSave)
                        {
                            if (item.Id > 0)
                                session.Update(item);
                            else
                                session.Insert(item);
                        }

                        foreach (var item in legalOwnerToSave)
                        {
                            if (item.Id > 0)
                                session.Update(item);
                            else
                                session.Insert(item);
                        }

                        foreach (var account in persAccToSave)
                        {
                            foreach (var walletProp in props)
                            {
                                var wallet = (Wallet)accessor[account, walletProp.Name];

                                if (wallet == null)
                                {
                                    wallet =
                                        (Wallet)Activator.CreateInstance(typeof(Wallet), Guid.NewGuid().ToString(), account, WalletHelper.GetWalletTypeByPropertyName(walletProp.Name));

                                    accessor[account, walletProp.Name] = wallet;
                                }

                                if (wallet.Id == 0)
                                    session.Insert(wallet);
                            }

                            if (account.Id > 0)
                                session.Update(account);
                            else
                            {
                                session.Insert(account);

                                LogEntity(account, session, login);
                            }
                        }

                        foreach (var item in periodSummariesToSave)
                        {
                            if (item.Id > 0)
                                session.Update(item);
                            else
                                session.Insert(item);
                        }

                        foreach (var item in rkcRealObjToSave)
                        {
                            if (item.Id > 0)
                                session.Update(item);
                            else
                                session.Insert(item);
                        }

                        entityLogLightToSave.ForEach(x => session.Insert(x));

                        tr.Commit();
                    }
                }
            }
            finally
            {
                roomToSave.Clear();
                indivOwnerToSave.Clear();
                legalOwnerToSave.Clear();
                persAccToSave.Clear();
            }
        }

        private void LogEntity(IEntity entity, IStatelessSession session, string login)
        {
            if (VersionedEntityHelper.IsUnderVersioning(entity))
            {
                var parameters =
                    VersionedEntityHelper.GetCreator(entity)
                        .CreateParameters()
                        .Where(x => !VersionedEntityHelper.ShouldSkip(entity, x.ParameterName))
                        .ToList();

                var hasDate = entity as IHasDateActualChange;

                var now = DateTime.UtcNow;

                parameters.ForEach(x =>
                {
                    var save = new EntityLogLight
                    {
                        EntityId = entity.Id.To<long>(),
                        ClassName = x.ClassName,
                        PropertyName = x.PropertyName,
                        PropertyValue = x.PropertyValue,
                        ParameterName = x.ParameterName,
                        DateApplied = now,
                        DateActualChange = hasDate.Return(y => y.ActualChangeDate,
                                new DateTime(now.Year, now.Month, 1, now.Hour, now.Minute, now.Second,
                                    now.Millisecond)),
                        User = login,
                        ObjectCreateDate = now,
                        ObjectEditDate = now
                    };
                    session.Insert(save);
                });
            }
        }

        private void PrepareEntityLogToSave(string login)
        {
            var emptyPersonalAccount = new BasePersonalAccount();

            if (VersionedEntityHelper.IsUnderVersioning(emptyPersonalAccount))
            {
                var parameters = VersionedEntityHelper.GetCreator(emptyPersonalAccount)
                    .CreateParameters()
                    .Where(x => !VersionedEntityHelper.ShouldSkip(emptyPersonalAccount, x.ParameterName))
                    .ToList();

                var areaShareParameter = parameters.FirstOrDefault(x => x.ParameterName == VersionedParameters.AreaShare);
                var accountOpenDateParameter = parameters.FirstOrDefault(x => x.ParameterName == VersionedParameters.PersonalAccountOpenDate);

                var now = DateTime.UtcNow;

                if (areaShareParameter != null)
                {
                    foreach (var pair in accountAreaShareLogToCreateDict)
                    {
                        var log = new EntityLogLight
                        {
                            EntityId = pair.Key.Id,
                            ClassName = areaShareParameter.ClassName,
                            PropertyName = areaShareParameter.PropertyName,
                            PropertyValue = pair.Value.Value,
                            ParameterName = areaShareParameter.ParameterName,
                            DateApplied = now,
                            DateActualChange = pair.Value.DateActualChange,
                            User = login,
                            ObjectCreateDate = now,
                            ObjectEditDate = now
                        };

                        entityLogLightToSave.Add(log);
                    }
                }

                if (accountOpenDateParameter != null)
                {
                    foreach (var pair in accountOpenDateLogToCreateDict)
                    {
                        var log = new EntityLogLight
                        {
                            EntityId = pair.Key.Id,
                            ClassName = accountOpenDateParameter.ClassName,
                            PropertyName = accountOpenDateParameter.PropertyName,
                            PropertyValue = pair.Value.Value,
                            ParameterName = accountOpenDateParameter.ParameterName,
                            DateApplied = now,
                            DateActualChange = pair.Value.DateActualChange,
                            User = login,
                            ObjectCreateDate = now,
                            ObjectEditDate = now
                        };

                        entityLogLightToSave.Add(log);
                    }
                }
            }

            var emptyRoom = new Room();

            if (VersionedEntityHelper.IsUnderVersioning(emptyRoom))
            {
                var parameters = VersionedEntityHelper.GetCreator(emptyRoom)
                    .CreateParameters()
                    .Where(x => !VersionedEntityHelper.ShouldSkip(emptyRoom, x.ParameterName))
                    .ToList();

                var roomAreaParameter = parameters.FirstOrDefault(x => x.ParameterName == VersionedParameters.RoomArea);

                var now = DateTime.UtcNow;

                if (roomAreaParameter != null)
                {
                    foreach (var pair in roomLogToCreateDict)
                    {
                        var log = new EntityLogLight
                        {
                            EntityId = pair.Key.Id,
                            ClassName = roomAreaParameter.ClassName,
                            PropertyName = roomAreaParameter.PropertyName,
                            PropertyValue = pair.Value.Value,
                            ParameterName = roomAreaParameter.ParameterName,
                            DateApplied = now,
                            DateActualChange = pair.Value.DateActualChange,
                            User = login,
                            ObjectCreateDate = now,
                            ObjectEditDate = now
                        };

                        entityLogLightToSave.Add(log);
                    }
                }
            }
        }

        private void AddPersAccPeriodSummary(BasePersonalAccount acc)
        {
            if (acc == null || currentPeriod == null || currentPeriodSummAccIds.Contains(acc.Id))
            {
                return;
            }

            var currPeriodSummary = new PersonalAccountPeriodSummary(acc, currentPeriod, 0M);

            periodSummariesToSave.Add(currPeriodSummary);
        }

        private void AddRkcRelation(BasePersonalAccount acc, string rkcInn)
        {
            if (acc == null)
            {
                return;
            }

            if (rkcInnByPersAcc.ContainsKey(acc.Id))
            {
                if (rkcInnByPersAcc[acc.Id] != rkcInn)
                {
                    AddLog(rowNum, acc.PersAccNumExternalSystems, "Лицевой счет находится под управлением другого РКЦ", false);
                }
            }
            else
            {
                if (rkcIdByInn.ContainsKey(rkcInn))
                {
                    rkcRealObjToSave.Add(new CashPaymentCenterRealObj
                    {
                        CashPaymentCenter = new CashPaymentCenter { Id = rkcIdByInn.Get(rkcInn) },
                        RealityObject = acc.Room.RealityObject,
                        DateStart = DateTime.Now
                    });
                }
                else
                {
                    AddLog(rowNum, acc.PersAccNumExternalSystems, "В системе нет РКЦ с таким ИНН", false);
                }
            }
        }

        private class ProxyLogValue
        {
            public DateTime DateActualChange { get; set; }
            public string Value { get; set; }
        }
    }
}