namespace Bars.Gkh.RegOperator.Imports
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading;
    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.Security;
    using Bars.B4.Modules.States;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Domain.Cache;
    using Bars.Gkh.Domain.CollectionExtensions;
    using Bars.Gkh.Domain.ParameterVersioning;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.ETL.Entities;
    using Bars.Gkh.Import;
    using Bars.Gkh.RegOperator.Domain.Interfaces;
    using Bars.Gkh.RegOperator.Domain.ParametersVersioning;
    using Bars.Gkh.RegOperator.DomainService.Period;
    using Bars.Gkh.RegOperator.DomainService.PersonalAccount;
    using Bars.Gkh.RegOperator.Entities;

    public class EtlPersonalAccountImporter : EtlBaseImporter<BasePersonalAccount>
    {
        private readonly GkhCache _cache;
        private State _basePersonalAccountDefaultState;

        public EtlPersonalAccountImporter(GkhCache cache)
        {
            _cache = cache;
        }

        public override string Code
        {
            get { return "BillingPersonalAccountImport"; }
        }

        public override IDataResult Validate(ImportQueueItem importItem)
        {
            return new BaseDataResult();
        }

        public override string FileName
        {
            get { return "kaprem"; }
        }

        public override int Order
        {
            get { return 70; }
        }

        public override int SplitCount
        {
            get { return 23; }
        }



        /*  для сохранения или обновления   */
        private readonly List<Gkh.Entities.Room> _roomItems = new List<Gkh.Entities.Room>();

        private readonly List<ChargePeriod> _chargePeriodItems = new List<ChargePeriod>();

        private readonly List<PersonalAccountPeriodSummary> _personalAccountPeriodSummaryItems = new List<PersonalAccountPeriodSummary>();

        /*  domain service  */
        public IDomainService<Gkh.Entities.Room> RoomService { get; set; }

        public IDomainService<BasePersonalAccount> AccountService { get; set; }

        public IDomainService<PersonalAccountOwner> AccountOwnerService { get; set; }

        public IDomainService<PersonalAccountPeriodSummary> PeriodSummaryService { get; set; }

        public IDomainService<RealityObjectChargeAccount> ChargeAccountService { get; set; }

        public IDomainService<RealityObjectChargeAccountOperation> ChargeAccountOperService { get; set; }

        public IDomainService<ChargePeriod> ChangePeriodService { get; set; }

        public IChargePeriodService ChargePeriodService { get; set; }

        public IStateProvider StProvider { get; set; }

        public IRepository<User> UserRepo { get; set; }

        public IRepository<RealityObject> RealityObjectRepository { get; set; }

        public IUserIdentity Identity { get; set; }

        public ISubPersonalAccountService SubPersonalAccountService { get; set; }

        private PersonalAccountOwner _testPersonalAccountOwner;

        protected string ClosedPeriodImportRight;

        /*  списки существующих  */
        private List<ChargePeriod> _changePeriods;

        private List<PersonalAccountPeriodSummary> _periodSummaries;

        private Dictionary<ChargePeriod, Dictionary<BasePersonalAccount, PersonalAccountPeriodSummary>> _periodSummaryByPeriodByAccount;

        private HashSet<PersonalAccountPeriodSummary> _periodSummaryFirstTime;

        private Dictionary<long, PersonalAccountOwner> _accountOwnerDict;

        private Dictionary<string, BasePersonalAccount> _accountDict;

        private List<Gkh.Entities.Room> _rooms;

        private Dictionary<long, Dictionary<string, Gkh.Entities.Room>> _roomDictByRoDict;

        private Dictionary<long, long> _realtyObjectMunicipality;

        private string _login;

        private readonly Dictionary<long, List<EntityLogLight>> _logsForExistingRoom = new Dictionary<long, List<EntityLogLight>>();

        private readonly Dictionary<long, List<EntityLogLight>> _logsForExistingAccount = new Dictionary<long, List<EntityLogLight>>();

        private readonly Dictionary<long, decimal> _areaShareHistory = new Dictionary<long, decimal>();

        protected override int BatchSize { get { return int.MaxValue; } } // За сохранение пачками будем отвечать сами (вариант из базового класса для нас неоптимален)

        const int RowsPerQuery = 200000;
        protected List<T> GetEntityList<T>(IDomainService<T> service) where T : IEntity
        {
            var result = new List<T>();
            var totalRowsCount = service.GetAll().Count();

            // Грузим таблицу по частям, временное решение

            for (var start = 0; start < totalRowsCount; start += RowsPerQuery)
            {
                result.AddRange(service.GetAll().Skip(start).Take(RowsPerQuery).ToArray());
            }

            return result;
        }

        protected override void InitDictionaries()
        {
            base.InitDictionaries();

            _accountOwnerDict = AccountOwnerService.GetAll().ToDictionary(x => x.Id);

            var accounts = GetEntityList(AccountService);

            _accountDict = accounts
                .GroupBy(x => x.PersonalAccountNum)
                .ToDictionary(x => x.Key, x => x.First());

            _rooms = GetEntityList(RoomService);

            _roomDictByRoDict = _rooms
                .GroupBy(x => x.RealityObject.Id)
                .ToDictionary(
                    x => x.Key,
                    x => x.GroupBy(y => y.RoomNum).ToDictionary(y => y.Key, y => y.First()));

            _periodSummaries = GetEntityList(PeriodSummaryService);

            _periodSummaryByPeriodByAccount = _periodSummaries
                .GroupBy(x => x.Period)
                .ToDictionary(
                    x => x.Key,
                    x => x.GroupBy(y => y.PersonalAccount).ToDictionary(y => y.Key, y => y.First()));

            _periodSummaryFirstTime = new HashSet<PersonalAccountPeriodSummary>();

            _changePeriods = ChangePeriodService.GetAll().ToList();

            ClosedPeriodImportRight =
                AuthService.Grant(CurrentUser, "Import.PersonalAccountClosedImport").ToString();

            var emptyPersonalAccount = new BasePersonalAccount();
            StProvider.SetDefaultState(emptyPersonalAccount);
            _basePersonalAccountDefaultState = emptyPersonalAccount.State;


            _login = UserRepo.Get(Identity.UserId).Return(u => u.Login);

            if (_login.IsEmpty())
            {
                _login = "anonymous";
            }

            _realtyObjectMunicipality = RealityObjectRepository.GetAll()
                .Where(x => x.Municipality != null)
                .Select(x => new { x.Id, muId = x.Municipality.Id })
                .ToDictionary(x => x.Id, x => x.muId);
        }

        protected Gkh.Entities.Room GetOrCreateRoom(BasePersonalAccount acc, RealityObject ro, string roomNum, int roomsCount, RoomType roomType)
        {
            if (ro == null)
            {
                return null;
            }

            if (!_roomDictByRoDict.ContainsKey(ro.Id))
            {
                _roomDictByRoDict[ro.Id] = new Dictionary<string, Gkh.Entities.Room>();
            }

            var roRooms = _roomDictByRoDict[ro.Id];

            var room = roRooms.ContainsKey(roomNum) ? roRooms[roomNum] : null;

            if (room == null)
            {
                room = new Gkh.Entities.Room
                {
                    RealityObject = ro,
                    RoomNum = roomNum,
                    RoomsCount = roomsCount,
                    Type = roomType
                };

                _roomItems.Add(room);
                _rooms.Add(room);
                roRooms[roomNum] = room;
            }
            else
            {

                room.RoomsCount = roomsCount;
                room.Type = roomType;

                _roomItems.Add(room);
            }

            return room;
        }

        public override void ProcessLine(string archiveName, string[] splits, ILogImport logger)
        {
            var ro = GetRealityObject(splits[2], splits[1]);

            if (ro == null)
            {
                var warning = string.Format("Дом не найден: addressUid = {0}, bilingId = {1}", splits[2], splits[1]);
                logger.Warn(string.Empty, warning);
                return;
            }

            var paymentCode = splits[11];


            /* лицевой счет */
            BasePersonalAccount acc = this._accountDict.ContainsKey(paymentCode) ? this._accountDict[paymentCode] : null;

            /* помещение */
            var room = GetOrCreateRoom(acc, ro,
                splits[4],                      // Appartment (RoomNum)
                splits[7].ToUncheckedInt(),     // roomsCount 
                splits[9] == "true" ? RoomType.NonLiving : RoomType.Living);      //roomType

            var ci = CultureInfo.GetCultureInfo("ru-RU");

            Thread.CurrentThread.CurrentCulture = ci;

            /* период начисления */
            var periodDate = splits[10].ToDateTime(ci);

            var period = _changePeriods.FirstOrDefault(x => x.StartDate == periodDate);

            if (period == null)
            {
                period = new ChargePeriod
                {
                    StartDate = periodDate,
                    EndDate = periodDate.AddMonths(1).AddDays(-1),
                    Name = periodDate.ToString("yyyy MMM", ci)
                };

                _changePeriods.Add(period);

                _chargePeriodItems.Add(period);
            }
            if (splits.Length > 23)
            {
                switch (splits[23])
                {
                    case "true":
                        UpdateRoomOwnershipType(room, RoomOwnershipType.Private, period.StartDate);
                        break;
                    case "false":
                        UpdateRoomOwnershipType(room, RoomOwnershipType.Municipal, period.StartDate);
                        break;
                }
            }

            /* todo ! 8  AccountState */

            if (acc != null)
            {
                var oldOpenDate = acc.OpenDate;
                var oldCloseDate = acc.CloseDate;
                var oldAreaShare = acc.AreaShare;

                _areaShareHistory[acc.Id] = acc.AreaShare;

                acc.Room = room;
                //State = splits[8]//  AccountState
                acc.Tariff = splits[18].ToDecimal();
                acc.OpenDate = splits[19].ToDateTime();
                acc.CloseDate = splits[20].ToDateTime();
                acc.Area = splits[5].ToDecimal();
                acc.LivingArea = splits[6].ToDecimal();

                acc.AreaShare = room.LivingArea != 0 ? splits[6].ToDecimal() / room.LivingArea : 0;

                var logs = this.LogEntity(acc);

                if (!_logsForExistingAccount.ContainsKey(acc.Id))
                {
                    _logsForExistingAccount[acc.Id] = new List<EntityLogLight>();
                }

                var openDatelog = logs.FirstOrDefault(x => x.ParameterName == VersionedParameters.PersonalAccountOpenDate);
                var closeDatelog = logs.FirstOrDefault(x => x.ParameterName == VersionedParameters.PersonalAccountCloseDate);
                var areaSharelog = logs.FirstOrDefault(x => x.ParameterName == VersionedParameters.AreaShare);

                if (oldOpenDate != acc.OpenDate && openDatelog != null)
                {
                    _logsForExistingAccount[acc.Id].Add(openDatelog);
                }

                if (oldCloseDate != acc.CloseDate && closeDatelog != null)
                {
                    _logsForExistingAccount[acc.Id].Add(closeDatelog);
                }

                if (oldAreaShare != acc.AreaShare && areaSharelog != null)
                {
                    _logsForExistingAccount[acc.Id].Add(areaSharelog);
                }

                logger.Info(paymentCode, string.Format("Обновлен л/с {0}", paymentCode));
                Add(acc);
                logger.CountChangedRows++;
            }
            else
            {
                acc = new BasePersonalAccount
                {
                    Room = room,
                    Tariff = splits[18].ToDecimal(),
                    OpenDate = splits[19].ToDateTime(),
                    CloseDate = splits[20].ToDateTime(),
                    PersonalAccountNum = paymentCode,
                    AccountOwner = GetTestAccountOwner(),
                    Area = splits[5].ToDecimal(),
                    LivingArea = splits[6].ToDecimal(),
                    State = _basePersonalAccountDefaultState
                };

                //State = splits[8]//  AccountState

                Add(acc);
                logger.CountAddedRows++;
                logger.Info(paymentCode, string.Format("Добавлен л/с {0}", paymentCode));

                _accountDict.Add(acc.PersonalAccountNum, acc);
            }


            if (!_periodSummaryByPeriodByAccount.ContainsKey(period))
            {
                _periodSummaryByPeriodByAccount[period] = new Dictionary<BasePersonalAccount, PersonalAccountPeriodSummary>();
            }

            var periodSummariesByAccountDict = _periodSummaryByPeriodByAccount[period];

            var periodSummary = periodSummariesByAccountDict.ContainsKey(acc) ? periodSummariesByAccountDict[acc] : null;

            if (!period.IsClosed || ClosedPeriodImportRight.ToBool())
            {
                if (periodSummary != null)
                {
                    // период уже был создан ранее

                    if (periodSummary.Id == 0 || _periodSummaryFirstTime.Contains(periodSummary))
                    {
                        // период был создан в текущем импорте
                        // либо у существующего периода значения начислений были уже перезаписаны, и по этому лс встретили еще начисления  

                        periodSummary.SaldoIn += splits[12].ToDecimal(); //  BalanceIn
                        periodSummary.SaldoOut += splits[13].ToDecimal(); // BalanceOut
                        periodSummary.ChargedByBaseTariff += splits[14].ToDecimal(); // Charge
                        // 15 ForPay не грузим
                        periodSummary.TariffPayment += splits[16].ToDecimal(); // Paid
                        periodSummary.Recalc += splits[17].ToDecimal(); // RecalculationSum
                    }
                    else
                    {
                        periodSummary.SaldoIn = splits[12].ToDecimal(); //  BalanceIn
                        periodSummary.SaldoOut = splits[13].ToDecimal(); // BalanceOut
                        periodSummary.ChargedByBaseTariff = splits[14].ToDecimal(); // Charge
                        // 15 ForPay не грузим
                        periodSummary.TariffPayment = splits[16].ToDecimal(); // Paid
                        periodSummary.Recalc = splits[17].ToDecimal(); // RecalculationSum

                        _periodSummaryFirstTime.Add(periodSummary);
                    }

                    _personalAccountPeriodSummaryItems.Add(periodSummary);
                }
                else
                {
                    periodSummary = new PersonalAccountPeriodSummary
                    {
                        PersonalAccount = acc,
                        Period = period,
                        SaldoIn = splits[12].ToDecimal(), //  BalanceIn
                        SaldoOut = splits[13].ToDecimal(), // BalanceOut
                        ChargedByBaseTariff = splits[14].ToDecimal(), // Charge
                        // 15 ForPay не грузим
                        TariffPayment = splits[16].ToDecimal(), // Paid
                        Recalc = splits[17].ToDecimal() // RecalculationSum
                    };

                    _personalAccountPeriodSummaryItems.Add(periodSummary);

                    _periodSummaries.Add(periodSummary);
                    periodSummariesByAccountDict[acc] = periodSummary;
                }
            }
            else
            {
                logger.Warn("У текущего пользователя нет права на импорт в закрытый период", string.Format("Не удалось внести информацию по лс №{0} за период {1}",
                    acc.PersonalAccountNum,
                    period.Name));
            }
            /* period summary */

        }

        private PersonalAccountOwner GetTestAccountOwner()
        {
            if (_testPersonalAccountOwner != null) return _testPersonalAccountOwner;
            _testPersonalAccountOwner = _accountOwnerDict.Values.FirstOrDefault(x => x.Name == "Тестовый");

            if (_testPersonalAccountOwner != null) return _testPersonalAccountOwner;
            _testPersonalAccountOwner = new PersonalAccountOwner
            {
                Name = "Тестовый"
            };

            AccountOwnerService.Save(_testPersonalAccountOwner);
            _accountOwnerDict.Add(_testPersonalAccountOwner.Id, _testPersonalAccountOwner);

            return _testPersonalAccountOwner;
        }

        public override Func<BasePersonalAccount, bool> Predicate(BasePersonalAccount other)
        {
            return x => true;
        }

        protected void SaveUpdateRooms()
        {
            var now = DateTime.Now;

            _roomItems.ForEach(x =>
            {
                x.ObjectEditDate = now;
                x.ObjectCreateDate = x.Id > 0 ? x.ObjectCreateDate : now;
                x.ObjectVersion = x.Id > 0 ? (x.ObjectVersion + 1) : 0;
            });

            var roomsToCreate = _roomItems.Where(x => x.Id == 0).ToList();

            // Без этого не получается сохранять Room в statelessSession, т.к. некоторые (не все!) существующие записи видятся как RoomProxy
            // Кто знает как, поправьте, пожалуйста
            var persistenceContext = Container.Resolve<ISessionProvider>().CurrentSession.GetSessionImplementation().PersistenceContext;

            var roomsToUpdate = _roomItems.Where(x => x.Id > 0).Select(x => (Gkh.Entities.Room)persistenceContext.Unproxy(x)).ToList();

            TransactionHelper.InsertInManyTransactions(Container, roomsToCreate.Union(roomsToUpdate).ToList(), 1000, false, true);

            // Создаем истории для новых записей
            var newLogs = roomsToCreate.SelectMany(this.LogEntity).ToList();

            // Создаем истории для измененных существующих записей
            newLogs.AddRange(
                _logsForExistingRoom.SelectMany(
                    x => x.Value.GroupBy(y => new { y.EntityId, y.ParameterName }).Select(y => y.Last()).ToList()));

            _roomItems.Clear();

            TransactionHelper.InsertInManyTransactions(Container, newLogs, 1000, true, true);
        }

        protected void SaveUpdateChargePeriods()
        {
            TransactionHelper.InsertInManyTransactions(Container, _chargePeriodItems);

            CorrectPeriodsMore();
        }

        protected void SaveUpdatePeriodsSummaries()
        {
            var now = DateTime.Now;

            _personalAccountPeriodSummaryItems.ForEach(x =>
            {
                x.ObjectEditDate = now;
                x.ObjectCreateDate = x.Id > 0 ? x.ObjectCreateDate : now;
                x.ObjectVersion = x.Id > 0 ? (x.ObjectVersion + 1) : 0;
            });

            TransactionHelper.InsertInManyTransactions(Container, _personalAccountPeriodSummaryItems, 1000, true, true);
        }

        protected void SaveUpdatePersonalAccounts(List<BasePersonalAccount> records)
        {
            var now = DateTime.Now;

            records.ForEach(x =>
            {
                x.ObjectEditDate = now;
                x.ObjectCreateDate = x.Id > 0 ? x.ObjectCreateDate : now;
                x.ObjectVersion = x.Id > 0 ? (x.ObjectVersion + 1) : 0;
            });

            var accountToCreate = records.Where(x => x.Id == 0).ToList();

            TransactionHelper.InsertInManyTransactions(Container, records, 1000, false, true);

            // Создаем истории для новых записей
            var newLogs = accountToCreate.SelectMany(LogEntity).ToList();

            newLogs.AddRange(_logsForExistingAccount.SelectMany(
                x => x.Value.Where(y => y.ParameterName != VersionedParameters.AreaShare)
                    .GroupBy(y => new { y.EntityId, y.ParameterName }).Select(y => y.Last()).ToList()));

            // Создаем истории для измененных существующих записей
            // Так как доля собственности в коде может меняться несколько раз, 
            // в лог будем записывать только последнее значениe, если оно отличается от исходного
            newLogs.AddRange(_logsForExistingAccount.SelectMany(
                x => x.Value.Where(y => y.ParameterName == VersionedParameters.AreaShare)
                    .GroupBy(y => new { y.EntityId, y.ParameterName })
                    .Select(y => y.Last())
                    .Where(y => !_areaShareHistory.ContainsKey(y.EntityId) || _areaShareHistory[y.EntityId] != y.PropertyValue.ToDecimal())
                    .ToList()));

            records.Clear();

            TransactionHelper.InsertInManyTransactions(Container, newLogs, 1000, true, true);
        }

        /// <summary>
        /// Площадь помещения = сумме площадей его лс, доля собственности лс = площадь лс / площадь его помещения
        /// </summary>
        /// <param name="records"></param>
        protected void CorrectAreaShare(List<BasePersonalAccount> records)
        {
            // берем только те помещения, у которых во время импорта менялись лс
            var roomIds = records.Select(x => x.Room.Id).Distinct().ToDictionary(x => x, arg => 0);

            var persistenceContext = Container.Resolve<ISessionProvider>().CurrentSession.GetSessionImplementation().PersistenceContext;

            var roomsToCorrect = _rooms.Where(x => roomIds.ContainsKey(x.Id)).ToList();

            var accountsByRoomDict = _accountDict.Values
                .GroupBy(x => x.Room)
                .ToDictionary(x => x.Key, x => x.ToList());

            foreach (var roomToCorrect in roomsToCorrect)
            {
                var room = roomToCorrect;
                // берем ВСЕ лс для помещения, в том числе те, что были еще до импорта
                var roomAccs = accountsByRoomDict.ContainsKey(room)
                    ? accountsByRoomDict[room]
                    : new List<BasePersonalAccount>();

                var newArea = roomAccs.SafeSum(x => x.Area);

                if (newArea != room.Area)
                {
                    room.Area = newArea;

                    if (room.Id > 0) // Иначе залогируем при создании
                    {
                        var log = this.LogEntity((Gkh.Entities.Room)persistenceContext.Unproxy(room)).FirstOrDefault(x => x.ParameterName == VersionedParameters.RoomArea);

                        if (log != null)
                        {
                            if (!_logsForExistingRoom.ContainsKey(room.Id))
                            {
                                _logsForExistingRoom[room.Id] = new List<EntityLogLight>();
                            }

                            _logsForExistingRoom[room.Id].Add(log);
                        }
                    }
                }

                room.LivingArea = roomAccs.SafeSum(x => x.Area);

                if (room.Area == 0) continue;

                foreach (var acc in roomAccs)
                {
                    var oldAreaShare = acc.AreaShare;
                    acc.AreaShare = Decimal.Round(acc.Area / room.Area, 2);

                    if (oldAreaShare == acc.AreaShare) continue;

                    if (acc.Id > 0) // Иначе залогируем при создании
                    {
                        var log = LogEntity(acc).FirstOrDefault(x => x.ParameterName == VersionedParameters.AreaShare);

                        if (log != null)
                        {
                            if (!_logsForExistingAccount.ContainsKey(acc.Id))
                            {
                                _logsForExistingAccount[acc.Id] = new List<EntityLogLight>();
                            }

                            _logsForExistingAccount[acc.Id].Add(log);
                        }
                    }

                    // добавляем лс в список на сохранение/изменение
                    if (!records.Contains(acc))
                    {
                        records.Add(acc);
                    }
                }

                var sum = roomAccs.SafeSum(x => x.AreaShare);

                if (sum > 1)
                {
                    var acc = roomAccs.FirstOrDefault();
                    if (acc != null)
                    {
                        acc.AreaShare = acc.AreaShare - (sum - 1);

                        if (acc.Id > 0) // Иначе залогируем при создании
                        {
                            var log = this.LogEntity(acc).FirstOrDefault(x => x.ParameterName == VersionedParameters.AreaShare);

                            if (log != null)
                            {
                                if (!_logsForExistingAccount.ContainsKey(acc.Id))
                                {
                                    _logsForExistingAccount[acc.Id] = new List<EntityLogLight>();
                                }

                                _logsForExistingAccount[acc.Id].Add(log);
                            }
                        }

                        // добавляем лс в список на сохранение/изменение
                        if (!records.Contains(acc))
                        {
                            records.Add(acc);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Просуммировать начислено и оплачено из лс в счета начислений (по периодам)
        /// </summary>
        /// <param name="roIds"></param>
        /// <param name="municipalitiesId"></param>
        protected void SaveChargeAccountOperations(List<long> roIds, List<long> municipalitiesId)
        {
            var chargeAccItems = new List<RealityObjectChargeAccount>();
            var chargeAccOperationsItems = new List<RealityObjectChargeAccountOperation>();

            var roIdsDict = roIds.Distinct().ToDictionary(x => x);

            var persAccs = _accountDict.Values.Where(x => roIdsDict.ContainsKey(x.Room.RealityObject.Id)).ToList();

            var persAccsIds = persAccs.Select(x => x.Id).Distinct().ToDictionary(x => x);

            // Их счета начислений
            var chargeAccounts = ChargeAccountService.GetAll()
                .Where(x => municipalitiesId.Contains(x.RealityObject.Municipality.Id))
                .AsEnumerable()
                .Where(x => roIdsDict.ContainsKey(x.RealityObject.Id))
                .ToList();

            var chargeAccountsIds = chargeAccounts.Select(x => x.Id).Distinct().ToDictionary(x => x);

            // Операции счета начислений
            var chargeAccOpers = ChargeAccountOperService.GetAll()
                .Where(x => municipalitiesId.Contains(x.Account.RealityObject.Municipality.Id))
                .AsEnumerable()
                .Where(x => chargeAccountsIds.ContainsKey(x.Account.Id))
                .ToList()
                .GroupBy(x => x.Account.Id)
                .ToDictionary(x => x.Key, arg => arg.ToList());

            // операции лс по дому, периоду
            var periodSummaries = PeriodSummaryService.GetAll()
                .Where(x => municipalitiesId.Contains(x.PersonalAccount.Room.RealityObject.Municipality.Id))
                .Select(x => new { ro_Id = x.PersonalAccount.Room.RealityObject.Id, PeriodSummary = x })
                .AsEnumerable()
                .Where(x => persAccsIds.ContainsKey(x.PeriodSummary.PersonalAccount.Id))
                .GroupBy(x => x.ro_Id)
                .ToDictionary(
                    x => x.Key,
                    arg => arg.GroupBy(x => x.PeriodSummary.Period)
                              .ToDictionary(y => y.Key, arg2 => arg2.Select(z => z.PeriodSummary).ToList()));

            // берем счет начислений
            foreach (var charAcc in chargeAccounts)
            {
                if (periodSummaries.ContainsKey(charAcc.RealityObject.Id))
                {
                    // операции лс по этому дому
                    var periodSummariesByRo = periodSummaries[charAcc.RealityObject.Id];

                    // по всем периодам операций лс
                    foreach (var p in periodSummariesByRo.Keys)
                    {
                        RealityObjectChargeAccountOperation chargeAccOper = null;
                        var period = p;

                        // ищем операцию за тот же период в счете начислений иначе создаем
                        if (chargeAccOpers.ContainsKey(charAcc.Id))
                        {
                            chargeAccOper = chargeAccOpers[charAcc.Id].FirstOrDefault(x => x.Period.Id == period.Id);
                        }

                        if (chargeAccOper == null)
                        {
                            chargeAccOper = new RealityObjectChargeAccountOperation
                            {
                                Account = charAcc,
                                Period = period,
                                Date = period.StartDate
                            };
                        }

                        // аггрегируем
                        chargeAccOper.ChargedTotal = periodSummariesByRo[period].SafeSum(x => x.ChargedByBaseTariff);
                        chargeAccOper.PaidTotal = periodSummariesByRo[period].SafeSum(x => x.TariffPayment);

                        chargeAccOperationsItems.Add(chargeAccOper);
                    }

                    if (chargeAccOpers.ContainsKey(charAcc.Id))
                    {
                        // Итого по счету начислений
                        charAcc.ChargeTotal = chargeAccOpers[charAcc.Id].SafeSum(x => x.ChargedTotal + x.ChargedPenalty);
                        charAcc.PaidTotal = chargeAccOpers[charAcc.Id].SafeSum(x => x.PaidTotal + x.PaidPenalty);
                    }

                    chargeAccItems.Add(charAcc);
                }
            }

            // Намеренно закрываю сессию, что orm забыл про измененные записи, полученные объектами
            Container.Resolve<ISessionProvider>().CloseCurrentSession();

            var now = DateTime.Now;

            chargeAccItems.ForEach(x =>
            {
                x.ObjectEditDate = now;
                x.ObjectCreateDate = x.Id > 0 ? x.ObjectCreateDate : now;
                x.ObjectVersion = x.Id > 0 ? (x.ObjectVersion + 1) : 0;
            });

            chargeAccOperationsItems.ForEach(x =>
            {
                x.ObjectEditDate = now;
                x.ObjectCreateDate = x.Id > 0 ? x.ObjectCreateDate : now;
                x.ObjectVersion = x.Id > 0 ? (x.ObjectVersion + 1) : 0;
            });

            TransactionHelper.InsertInManyTransactions(Container, chargeAccItems, 1000, true, true);
            TransactionHelper.InsertInManyTransactions(Container, chargeAccOperationsItems, 1000, true, true);
        }

        protected void CorrectPeriods()
        {
            var periods = _chargePeriodItems.OrderBy(x => x.StartDate).ToList();

            if (periods.Any())
            {
                foreach (var chargePeriod in periods)
                {
                    if (this._changePeriods.Any(x => x.StartDate > chargePeriod.EndDate))
                    {
                        chargePeriod.IsClosed = true;
                    }
                }

                var lastEndDate = periods.OrderByDescending(z => z.EndDate).FirstOrDefault().ReturnSafe(z => z.EndDate);

                var periodToClose =
                    _chargePeriodItems.Any(x =>
                            x.Id != 0 &&
                            !x.IsClosed &&
                            (lastEndDate == null || x.StartDate > lastEndDate));

                if (periodToClose)
                {
                    ChargePeriodService.ClosePeriod(new BaseParams(), null, CancellationToken.None);
                }
            }

        }

        protected void CorrectPeriodsMore()
        {
            var openPeriodsQuery = ChangePeriodService.GetAll().Where(x => !x.IsClosed);

            foreach (var period in openPeriodsQuery.ToList())
            {
                if (period.EndDate.HasValue && ChangePeriodService.GetAll().Count(x => x.StartDate > period.EndDate) > 0)
                {
                    period.IsClosed = true;

                    ChangePeriodService.Update(period);

                    continue;
                }

                if (ChangePeriodService.GetAll().Count(x => x.StartDate > period.StartDate) > 0)
                {
                    period.IsClosed = true;

                    if (!period.EndDate.HasValue)
                    {
                        period.EndDate = period.StartDate.AddMonths(1).AddDays(-1);
                    }

                    ChangePeriodService.Update(period);
                }
            }

            var openPeriodsCount = openPeriodsQuery.Count();

            if (openPeriodsCount == 1)
            {
                var openPeriod = openPeriodsQuery.FirstOrDefault();

                if (openPeriod != null && openPeriod.EndDate.HasValue)
                {
                    openPeriod.EndDate = null;
                    ChangePeriodService.Update(openPeriod);
                }
            }
            else if (openPeriodsCount == 0)
            {
                var youngestPeriod = ChangePeriodService.GetAll().OrderByDescending(x => x.StartDate).FirstOrDefault();

                youngestPeriod.IsClosed = false;
                youngestPeriod.EndDate = null;
                ChangePeriodService.Update(youngestPeriod);
            }
        }

        protected override void SaveRecords(List<BasePersonalAccount> records)
        {
            // Посчитать площади помещений и долю собственности лс
            CorrectAreaShare(records);

            //Закрываем периоды
            CorrectPeriods();

            var roIds = records.Select(x => x.Room.RealityObject.Id).Distinct().ToList();
            var municipalitiesId = roIds
                .Where(x => _realtyObjectMunicipality.ContainsKey(x))
                .Select(x => _realtyObjectMunicipality[x])
                .Distinct()
                .ToList();

            // Сохранить помещения
            SaveUpdateRooms();

            // Сохранить лс
            SaveUpdatePersonalAccounts(records);

            // Намеренно закрываю сессию, что orm забыл про измененные записи, полученные объектами
            Container.Resolve<ISessionProvider>().CloseCurrentSession();

            // Периоды начислений
            SaveUpdateChargePeriods();

            // Агрегации по периодам
            SaveUpdatePeriodsSummaries();

            // Начисления по дому
            SaveChargeAccountOperations(roIds, municipalitiesId);

            if (SubPersonalAccountService != null)
            {
                SubPersonalAccountService.AddSubPersonalAccount();
            }

            // Намеренно закрываю сессию, что orm забыл про измененные записи, полученные объектами
            Container.Resolve<ISessionProvider>().CloseCurrentSession();
        }

        private List<EntityLogLight> LogEntity(IEntity entity)
        {
            if (VersionedEntityHelper.IsUnderVersioning(entity))
            {
                var parameters =
                    VersionedEntityHelper.GetCreator(entity)
                        .CreateParameters()
                        .Where(x => !VersionedEntityHelper.ShouldSkip(entity, x.ParameterName))
                        .ToList();

                var hasDate = entity as IHasDateActualChange;

                var now = DateTime.Now;

                var logs = parameters.Select(x => new EntityLogLight
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
                    User = _login,
                    ObjectCreateDate = now,
                    ObjectEditDate = now
                })
                    .ToList();

                return logs;
            }

            return new List<EntityLogLight>();
        }

        private void UpdateRoomOwnershipType(Gkh.Entities.Room room, RoomOwnershipType type, DateTime factDate)
        {
            if (room.OwnershipType == type)
            {
                return;
            }

            if (room.Id == 0)
            {
                room.OwnershipType = type;
                return;
            }

            var versionedEntityService = Container.Resolve<IVersionedEntityService>();
            using (Container.Using(versionedEntityService))
            {
                var baseParams = new BaseParams { Files = new Dictionary<string, FileData>() };
                baseParams.Params["className"] = "Room";
                baseParams.Params["propertyName"] = "OwnershipType";
                baseParams.Params["value"] = type;
                baseParams.Params["entityId"] = room.Id;
                baseParams.Params["factDate"] = factDate;
                versionedEntityService.SaveParameterVersion(baseParams);
            }

        }
    }
}
