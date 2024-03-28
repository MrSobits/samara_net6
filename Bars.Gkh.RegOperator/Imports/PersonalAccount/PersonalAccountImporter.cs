namespace Bars.Gkh.RegOperator.Imports
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Threading;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Microsoft.Extensions.Logging;
    using Bars.B4.Modules.Security;
    using Bars.B4.Modules.States;
    using Bars.B4.Modules.Tasks.Common.Utils;
    using Bars.B4.Utils;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Domain.CollectionExtensions;
    using Bars.Gkh.Domain.ParameterVersioning;
    using Bars.Gkh.DomainService.AddressMatching;
    using Bars.Gkh.Dto;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Import;
    using Bars.Gkh.RegOperator.Domain.ParametersVersioning;
    using Bars.Gkh.RegOperator.Domain.ValueObjects;
    using Bars.Gkh.RegOperator.DomainModelServices.PersonalAccount;
    using Bars.Gkh.RegOperator.DomainService.PersonalAccount;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Entities.Refactor;
    using Bars.Gkh.RegOperator.Entities.ValueObjects;
    using Bars.Gkh.RegOperator.Entities.Wallet;
    using Bars.Gkh.RegOperator.Enums;
    using Bars.Gkh.RegOperator.Extenstions;
    using Bars.Gkh.Utils;

    using FastMember;

    using NHibernate;
    using NHibernate.Linq;
    using DomainModelServices;

    /// <summary>
    /// Импортер ЛС
    /// </summary>
    public class PersonalAccountImporter : BaseBillingImporter<BasePersonalAccount>
    {
        #region Константы и переменные
        // гуид, который будет использоваться как SourceGuid при данном импорте
        private const string BillingImportGuid = "93AAE3D8-B5A8-40F2-94FC-6DB9337732DD";
        private bool closedPeriodImportRight;

        /*  для сохранения или обновления   */
        private List<EntityLogLight> logLights = new List<EntityLogLight>();
        private readonly List<Gkh.Entities.Room> roomItems = new List<Gkh.Entities.Room>();
        private readonly List<ChargePeriod> chargePeriodItems = new List<ChargePeriod>();
        private readonly List<PersonalAccountPeriodSummary> personalAccountPeriodSummaryItems = new List<PersonalAccountPeriodSummary>();
        private readonly List<Transfer> transfers = new List<Transfer>();
        private readonly List<MoneyOperation> moneyOperations = new List<MoneyOperation>();
        private readonly List<NotMatchedAddressInfo> addressesForMatching = new List<NotMatchedAddressInfo>();

        /*  списки существующих  */
        private State basePersonalAccountDefaultState;
        private Dictionary<string, State> accountStates;
        private List<ChargePeriod> chargePeriods;
        private List<PersonalAccountPeriodSummary> periodSummaries;
        private Dictionary<ChargePeriod, Dictionary<BasePersonalAccount, PersonalAccountPeriodSummary>> periodSummaryByPeriodByAccount;
        private Dictionary<long, PersonalAccountOwner> accountOwnerDict;
        private Dictionary<string, BasePersonalAccount> accountDict;
        private List<Gkh.Entities.Room> rooms;
        private Dictionary<long, long> realtyObjectMunicipality;
        private Dictionary<long, IEnumerable<PersAccAreasInfo>> persAccsByRoomDict;
        private Dictionary<long, Wallet> persAccBaseWalletDict;
        private Dictionary<long, Wallet> persAccPenaltyWalletDict;
        private readonly Stopwatch sw;
        private readonly Dictionary<long, List<EntityLogLight>> logsForExistingRoom = new Dictionary<long, List<EntityLogLight>>();
        private readonly Dictionary<long, List<EntityLogLight>> logsForExistingAccount = new Dictionary<long, List<EntityLogLight>>();
        private readonly Dictionary<long, decimal> areaShareHistory = new Dictionary<long, decimal>();
        private int rowNumber;
        private int controlNumber;
        private readonly List<string[]> allSplits;
        private List<BaseParams> versions;
        private PersonalAccountOwner testPersonalAccountOwner;

        private const string RecruitmentCode = "15";
        private const string OverhaulCode269 = "269";
        private const string OverhaulCode356 = "356";
        private const string OverhaulCode4440 = "4440";
        private const string OverhaulPenaltyCode = "506";
        private const string RecruitmentPenaltyCode = "507";
        private const string ContributionsPenaltyCode = "4441";

        private const string StartSalt = "unum";
        private const string EndSalt = "libertatem";
        #endregion

        private readonly string[] overhaulCodeGroup = new[]
        {
            PersonalAccountImporter.OverhaulCode269,
            PersonalAccountImporter.OverhaulCode356,
            PersonalAccountImporter.OverhaulCode4440
        };

        private readonly string[] penaltyCodeGroup = new[]
        {
            PersonalAccountImporter.OverhaulPenaltyCode,
            PersonalAccountImporter.ContributionsPenaltyCode,
            PersonalAccountImporter.RecruitmentPenaltyCode
        };

        /// <summary>
        /// ctor
        /// </summary>
        public PersonalAccountImporter()
        {
            this.sw = Stopwatch.StartNew();
            this.rowNumber = 0;
            this.controlNumber = 1;
            this.allSplits = new List<string[]>();
        }

        /// <summary>
        /// Название файла
        /// </summary>
        public override string FileName => "kaprem";

        /// <summary>
        /// Приоритет
        /// </summary>
        public override int Order => 70;

        /// <summary>
        /// Количество столбцов
        /// </summary>
        public override int SplitCount => 23;

        #region Сервисы
        
        /// <summary>
        /// Домен-сервис <see cref="Gkh.Entities.Room"/>
        /// </summary>
        public IDomainService<Gkh.Entities.Room> RoomService { get; set; }

        /// <summary>
        /// Домен-сервис <see cref="BasePersonalAccount"/>
        /// </summary>
        public IDomainService<BasePersonalAccount> AccountService { get; set; }

        /// <summary>
        /// Домен-сервис <see cref="PersonalAccountOwner"/>
        /// </summary>
        public IDomainService<PersonalAccountOwner> AccountOwnerService { get; set; }

        /// <summary>
        /// Домен-сервис <see cref="PersonalAccountPeriodSummary"/>
        /// </summary>
        public IDomainService<PersonalAccountPeriodSummary> PeriodSummaryService { get; set; }

        /// <summary>
        /// Домен-сервис <see cref="RealityObjectChargeAccount"/>
        /// </summary>
        public IDomainService<RealityObjectChargeAccount> ChargeAccountService { get; set; }

        /// <summary>
        /// Домен-сервис <see cref="RealityObjectChargeAccountOperation"/>
        /// </summary>
        public IDomainService<RealityObjectChargeAccountOperation> ChargeAccountOperService { get; set; }

        /// <summary>
        /// Домен-сервис <see cref="ChargePeriod"/>
        /// </summary>
        public IDomainService<ChargePeriod> ChangePeriodService { get; set; }

        /// <summary>
        /// Домен-сервис <see cref="State"/>
        /// </summary>
        public IDomainService<State> StateDomainService { get; set; }

        /// <summary>
        /// Домен-сервис <see cref="Wallet"/>
        /// </summary>
        public IDomainService<Wallet> WalletDomain { get; set; }

        /// <summary>
        /// Сервис импортированных адресов
        /// </summary>
        public IImportedAddressService AddressService { get; set; }

        /// <summary>
        /// Провайдер статусов
        /// </summary>
        public IStateProvider StateProvider { get; set; }

        /// <summary>
        /// Менеджер логов
        /// </summary>
        public ILogger LogManager { get; set; }

        /// <summary>
        /// Репозиторий <see cref="RealityObject"/>
        /// </summary>
        public IRepository<RealityObject> RealityObjectRepository { get; set; }

        /// <summary>
        /// Сервис ЛС
        /// </summary>
        public ISubPersonalAccountService SubPersonalAccountService { get; set; }

        /// <summary>
        /// Провайдер сессии
        /// </summary>
        public ISessionProvider SessionProvider { get; set; }

        /// <summary>
        /// Менеджер пользователей
        /// </summary>
        public IGkhUserManager UserManager { get; set; }

        #endregion

        private TypeAccessor Accessor { get; set; }

        private PropertyInfo[] Props { get; set; }

        /// <inheritdoc />
        protected override ValidateResult Validate(StreamReader streamReader)
        {
            var controlCode = streamReader.ReadLine();

            var origin = streamReader.ReadLine();

            // Возвращаем на вторую строку.
            if (streamReader.BaseStream.CanSeek)
            {
                streamReader.DiscardBufferedData();
                streamReader.BaseStream.Seek(0, SeekOrigin.Begin);
                streamReader.ReadLine();
            }

            string errorMessage = null;

            if (!this.CheckHash(origin, controlCode))
            {
                errorMessage = "Некорректная контрольная строка";
            }

            var dateLoadStr = streamReader.ReadLine().Split('|')[10];
            if (!DateTime.TryParse(dateLoadStr, out var dateLoad))
            {
                errorMessage = "Не удается прочитать дату выгрузки";
            }

            // Проверка корректности даты загрузки
            var cDate = DateTime.Now.Date;
            var conditionDate = cDate.Month == 1
                ? dateLoad.Year >= cDate.Date.Year - 1 && dateLoad <= cDate
                : dateLoad.Year == cDate.Year && dateLoad <= cDate;

            if (!conditionDate)
            {
                errorMessage = "Импорт лицевых счетов разрешен с указанием периода текущего года";
            }

            return new ValidateResult(errorMessage == null) { Message = errorMessage };
        }

        /// <summary>
        /// Проверка контрольной суммы файла
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="controlCode"></param>
        /// <returns></returns>
        private bool CheckHash(string origin, string controlCode)
        {
            var source = $"{PersonalAccountImporter.StartSalt}{origin}{PersonalAccountImporter.EndSalt}";

            var hash = MD5.GetHashString(source);

            return hash.Equals(controlCode);
        }

        /// <summary>
        /// Обработать строку
        /// </summary>
        public override void ProcessLine(string archiveName, string[] splits)
        {
            this.allSplits.Add(splits);
        }

        /// <summary>
        /// Предикат
        /// </summary>
        public override Func<BasePersonalAccount, bool> Predicate(BasePersonalAccount other)
        {
            return x => true;
        }

        /// <summary>
        /// Сохранить записи
        /// </summary>
        protected override void SaveRecords(List<BasePersonalAccount> records)
        {
            this.SessionProvider.GetCurrentSession().FlushMode = FlushMode.Never;

            var persAccNumbers = this.allSplits.Select(x => x[11]).ToList();

            var periodDates = this.allSplits.Select(x => x[10].ToDateTime()).ToList();

            this.FillDictionaries(persAccNumbers, periodDates);

            foreach (var splits in this.allSplits)
            {
                this.ProcessLine(splits);
            }

            this.Indicator.Indicate(null, 90, "Сохранение данных");

            // Посчитать площади помещений и долю собственности лс
            this.CorrectAreaShare(records);

            this.Indicator.Indicate(null, 91, "Сохранение данных");

            // Закрываем периоды
            this.CorrectPeriods();

            this.Indicator.Indicate(null, 92, "Сохранение данных");

            var roIds = records.Select(x => x.Room.RealityObject.Id).Distinct().ToList();
            var municipalitiesId = roIds
                .Where(x => this.realtyObjectMunicipality.ContainsKey(x))
                .Select(x => this.realtyObjectMunicipality[x])
                .Distinct()
                .ToList();

            // Сохранить помещения
            this.SaveUpdateRooms();

            this.Indicator.Indicate(null, 93, "Сохранение данных");

            var session = this.SessionProvider.GetCurrentSession();
            session.Clear();
            session.FlushMode = FlushMode.Commit;

            this.SaveImportedAddressForMatching();

            // Сохранить лс
            this.SaveUpdatePersonalAccounts(records);

            this.Indicator.Indicate(null, 94, "Сохранение данных");

            // Периоды начислений
            this.SaveUpdateChargePeriods();

            this.Indicator.Indicate(null, 95, "Сохранение данных");

            // Агрегации по периодам
            this.SaveUpdatePeriodsSummaries();

            this.Indicator.Indicate(null, 96, "Сохранение данных");

            // Начисления по дому
            this.SaveChargeAccountOperations(roIds, municipalitiesId);

            this.Indicator.Indicate(null, 97, "Сохранение данных");

            // Сохранить операции
            TransactionHelper.InsertInManyTransactions(this.Container, this.moneyOperations, 1000, true, true);

            this.Indicator.Indicate(null, 98, "Сохранение данных");

            // Сохранить трансферы
            TransactionHelper.InsertInManyTransactions(this.Container, this.transfers, 1000, true, true);

            this.Indicator.Indicate(null, 99, "Сохранение данных");

            this.LogManager.LogInformation("{0}. SubPersonalAccountService", this.sw.Elapsed);

            this.SubPersonalAccountService?.AddSubPersonalAccount();

            this.LogManager.LogInformation("{0}. SubPersonalAccountService end", this.sw.Elapsed);

            this.LogManager.LogInformation("{0}. IVersionedEntityService", this.sw.Elapsed);

            TransactionHelper.InsertInManyTransactions(this.Container, this.logLights, 1000, true, true);

            this.LogManager.LogInformation("{0}. IVersionedEntityService end", this.sw.Elapsed);

            // Намеренно закрываю сессию, что orm забыл про измененные записи, полученные объектами
            this.SessionProvider.CloseCurrentSession();
            this.LogManager.LogInformation("{0}. this.SessionProvider.CloseCurrentSession()();", this.sw.Elapsed);
        }

        private void FillDictionaries(
            IEnumerable<string> persAccNumbers,
            List<DateTime> periodDates)
        {
            if (this.UserIdentity != null)
            {
                var authService = this.Container.Resolve<IAuthorizationService>();
                this.closedPeriodImportRight = authService.Grant(this.UserIdentity, "Import.PersonalAccountClosedImport");
            }

            var emptyPersonalAccount = new BasePersonalAccount();
            this.StateProvider.SetDefaultState(emptyPersonalAccount);
            this.basePersonalAccountDefaultState = emptyPersonalAccount.State;

            var typeId = this.basePersonalAccountDefaultState.Return(y => y.TypeId, string.Empty);

            // Создаем словарь 
            this.accountStates = this.StateDomainService.GetAll()
                .Where(x => x.TypeId == typeId)
                .ToList()
                .GroupBy(x => x.Name)
                .ToDictionary(x => x.Key.ToLower(), x => x.FirstOrDefault());

            var accounts = new List<BasePersonalAccount>();

            this.Indicator.Indicate(null, 5, "Подготовка данных");

            persAccNumbers.Section(1000).ForEach(persAccNums =>
                    accounts.AddRange(
                        this.AccountService.GetAll()
                            .Fetch(acc => acc.Room)
                            .ThenFetch(x => x.RealityObject)
                            .Fetch(x => x.AccountOwner)
                            .Fetch(x => x.BaseTariffWallet)
                            .Fetch(x => x.DecisionTariffWallet)
                            .Fetch(x => x.RentWallet)
                            .Fetch(x => x.PenaltyWallet)
                            .Fetch(x => x.SocialSupportWallet)
                            .Fetch(x => x.PreviosWorkPaymentWallet)
                            .Fetch(x => x.AccumulatedFundWallet)
                            .Where(x => persAccNums.Contains(x.PersonalAccountNum))
                            .ToList()));

            var accountIds = accounts.Select(x => x.Id).ToList();

            this.accountOwnerDict = accounts.Select(x => x.AccountOwner)
                .GroupBy(x => x.Id)
                .ToDictionary(x => x.Key, x => x.First());

            this.accountDict = accounts
                .GroupBy(x => x.PersonalAccountNum)
                .ToDictionary(x => x.Key, x => x.First());

            this.persAccBaseWalletDict = accounts
                .Select(
                    x => new
                    {
                        x.Id,
                        x.BaseTariffWallet
                    })
                .GroupBy(x => x.Id)
                .Select(x => x.First())
                .ToDictionary(x => x.Id, x => x.BaseTariffWallet);

            this.persAccPenaltyWalletDict = accounts
                .Select(
                    x => new
                    {
                        x.Id,
                        x.PenaltyWallet
                    })
                .GroupBy(x => x.Id)
                .Select(x => x.First())
                .ToDictionary(x => x.Id, x => x.PenaltyWallet);

            this.rooms = accounts.Select(x => x.Room).Distinct().ToList();

            var roomsIds = this.rooms.Select(x => x.Id).ToList();
            var openAccState = this.accountStates.Get("открыт");

            var persAccsByRoomList = new List<BasePersonalAccount>();

            roomsIds.Section(1000).ForEach(ids => 
                persAccsByRoomList.AddRange(this.AccountService.GetAll()
                    .Where(x => ids.Contains(x.Room.Id))
                    .Where(x => x.State == null || x.State == openAccState)
                    .ToList()));

            this.persAccsByRoomDict = persAccsByRoomList
                .Select(
                    x => new
                    {
                        RoomId = x.Room.Id,
                        x.Id,
                        x.Area,
                        x.LivingArea,
                        x.AreaShare,
                        x.State
                    })
                .ToList()
                .GroupBy(x => x.RoomId)
                .ToDictionary(
                    x => x.Key,
                    y => y.Select(
                        x => new PersAccAreasInfo
                        {
                            Id = x.Id,
                            Area = x.Area,
                            LivingArea = x.LivingArea,
                            AreaShare = x.AreaShare,
                            State = x.State
                        }));

            periodDates = periodDates.Distinct().ToList();
            this.chargePeriods = this.ChangePeriodService.GetAll().SplitWhere(x => periodDates.Contains(x.StartDate)).ToList();
            var periodIds = this.chargePeriods.Select(x => x.Id).ToList();

            this.periodSummaries = new List<PersonalAccountPeriodSummary>();

            accountIds.Section(1000).ForEach(accs => this.periodSummaries.AddRange(
                    this.PeriodSummaryService.GetAll()
                        .Fetch(x => x.Period)
                        .Where(x => periodIds.Contains(x.Period.Id))
                        .Where(x => accs.Contains(x.PersonalAccount.Id))
                        .ToList()));

            this.periodSummaryByPeriodByAccount = this.periodSummaries
                .GroupBy(x => x.Period)
                .ToDictionary(
                    x => x.Key,
                    x => x.GroupBy(y => y.PersonalAccount).ToDictionary(y => y.Key, y => y.First()));

            this.realtyObjectMunicipality = this.RealityObjectRepository.GetAll()
                .Where(x => x.Municipality != null)
                .Select(x => new { x.Id, muId = x.Municipality.Id })
                .ToDictionary(x => x.Id, x => x.muId);

            this.Props = typeof(BasePersonalAccount).GetProperties().Where(x => x.PropertyType == typeof(Wallet)).ToArray();
            this.Accessor = TypeAccessor.Create(typeof(BasePersonalAccount), true);

            this.Indicator.Indicate(null, 20, "Данные подготовлены");
        }

        private void ProcessLine(string[] splits)
        {
            this.LogManager.LogInformation("{0}. Обработка строки: {1}", this.sw.Elapsed, ++this.rowNumber);

            var countPerPercent = this.allSplits.Count / 70;

            if (this.rowNumber > countPerPercent * this.controlNumber)
            {
                this.controlNumber++;
                var percent = ((float)this.rowNumber * 89) / this.allSplits.Count;
                this.Indicator.Indicate(
                    null,
                    (ushort)(percent > 89 ? 89 : percent),
                    "Обработка строки {0} из {1}".FormatUsing(this.rowNumber, this.allSplits.Count));
            }

            bool isWrongErcBinding = false;
            var ro = this.GetRealityObject(splits[2], out isWrongErcBinding, splits[1]);

            if (ro == null)
            {
                string warning;
                
                if (isWrongErcBinding)
                {
                    warning =
                        $"Лицевой счет с billingId = {splits[1]} и начисления по нему не были загружены. Необходимо перепривязать код ЕРЦ, просьба обратиться в техническую поддержку ГИС МЖФ РТ";
                }
                else
                {
                    warning =
                        $"Дом не найден: addressUid = {splits[2]}, bilingId = {splits[1]}, {splits[24]}, {splits[25]}, {splits[26]}, д. {splits[27]}, кв. {splits[4]}";
                    
                    //сохраняем адреса для будущего матчинга
                    this.AddAddressForMatching(splits);
                }
                
                this.Logger.Warn("Строка " + this.rowNumber, warning);
                return;
            }

            var persAccNum = splits[11];

            /* лицевой счет */
            BasePersonalAccount acc;
            this.accountDict.TryGetValue(persAccNum, out acc);

            /* помещение */
            var room = this.GetOrCreateRoom(
                ro,
                splits[4], //Appartment
                splits[7].ToUncheckedInt(), // RoomsCount
                splits[9] == "true" ? RoomType.NonLiving : RoomType.Living, // NonLiving
                splits[5].ToDecimal(), // ApartmentArea
                splits[6].ToDecimal() // LivingArea
                );

            var cultureInfo = CultureInfo.GetCultureInfo("ru-RU");

            Thread.CurrentThread.CurrentCulture = cultureInfo;

            /* период начисления */
            var periodDate = splits[10].ToDateTime(cultureInfo);

            if (periodDate > new DateTime(DateTime.Now.Year, DateTime.Now.Month , 1))
            {
                this.Logger.Warn("Строка " + this.rowNumber, "Лицевой счет и начисления по нему не загружены, так как указан некорректный период начисления");
                return;
            }

            var period = this.chargePeriods.FirstOrDefault(x => x.StartDate == periodDate);

            if (period == null)
            {
                period = new ChargePeriod(
                    periodDate.ToString("yyyy MMM", cultureInfo),
                    periodDate,
                    periodDate.AddMonths(1).AddDays(-1));

                this.chargePeriods.Add(period);
                this.chargePeriodItems.Add(period);
            }
            if (splits.Length > 23)
            {
                switch (splits[23])
                {
                    case "true":
                        this.UpdateRoomOwnershipType(room, RoomOwnershipType.Private, period.StartDate);
                        break;
                    case "false":
                        this.UpdateRoomOwnershipType(room, RoomOwnershipType.Municipal, period.StartDate);
                        break;
                }
            }

            var accountState = this.accountStates.Get(splits[8].ToLower());

            if (acc != null)
            {
                var oldOpenDate = acc.OpenDate;
                var oldCloseDate = acc.CloseDate;
                var oldAreaShare = acc.AreaShare;

                this.areaShareHistory[acc.Id] = acc.AreaShare;

                acc.Room = room;
                acc.Tariff = splits[18].ToDecimal();
                acc.OpenDate = splits[19].ToDateTime();
                acc.SetCloseDate(splits[20].ToDateTime(), false);
                acc.Area = splits[5].ToDecimal();
                acc.LivingArea = splits[6].ToDecimal();

                if (accountState != null)
                {
                    acc.State = accountState;
                }

                acc.AreaShare = room.LivingArea.HasValue && room.LivingArea.Value != 0 ? splits[6].ToDecimal() / room.LivingArea.Value : 0;

                var logs = this.LogEntity(acc);
                List<EntityLogLight> logsforExistingAccount;
                if (!this.logsForExistingAccount.TryGetValue(acc.Id, out logsforExistingAccount))
                {
                    logsforExistingAccount = this.logsForExistingAccount[acc.Id] = new List<EntityLogLight>();
                }

                var openDatelog = logs.FirstOrDefault(x => x.ParameterName == VersionedParameters.PersonalAccountOpenDate);
                var closeDatelog = logs.FirstOrDefault(x => x.ParameterName == VersionedParameters.PersonalAccountCloseDate);
                var areaSharelog = logs.FirstOrDefault(x => x.ParameterName == VersionedParameters.AreaShare);

                if (oldOpenDate != acc.OpenDate && openDatelog != null)
                {
                    logsforExistingAccount.Add(openDatelog);
                }

                if (oldCloseDate != acc.CloseDate && closeDatelog != null)
                {
                    logsforExistingAccount.Add(closeDatelog);
                }

                if (oldAreaShare != acc.AreaShare && areaSharelog != null)
                {
                    logsforExistingAccount.Add(areaSharelog);
                }

                this.LogManager.LogInformation(
                    "Строка " + this.rowNumber,
                    $"Обновлен л/с {persAccNum}, {splits[24]}, {splits[25]}, {splits[26]}, д. {splits[27]}, кв. {splits[4]}");
                this.Add(acc);
                this.Logger.CountChangedRows++;
            }
            else
            {
                acc = new BasePersonalAccount
                {
                    Room = room,
                    Tariff = splits[18].ToDecimal(),
                    OpenDate = splits[19].ToDateTime(),
                    PersonalAccountNum = persAccNum,
                    AccountOwner = this.GetTestAccountOwner(),
                    Area = splits[5].ToDecimal(),
                    LivingArea = splits[6].ToDecimal(),
                    State = accountState ?? this.basePersonalAccountDefaultState
                };

                acc.SetCloseDate(splits[20].ToDateTime(), false);

                this.Add(acc);
                this.Logger.CountAddedRows++;
                this.LogManager.LogInformation(
                    "Строка " + this.rowNumber,
                    $"Добавлен л/с {persAccNum}, {splits[24]}, {splits[25]}, {splits[26]}, д. {splits[27]}, кв. {splits[4]}");

                this.accountDict.Add(acc.PersonalAccountNum, acc);
            }

            switch (acc.ServiceType)
            {
                case PersAccServiceType.NotSelected:
                    if (this.overhaulCodeGroup.Contains(splits[22]))
                    {
                        acc.ServiceType = PersAccServiceType.Overhaul;
                    }
                    else if (splits[22] == PersonalAccountImporter.RecruitmentCode)
                    {
                        acc.ServiceType = PersAccServiceType.Recruitment;
                    }
                    break;
                case PersAccServiceType.Overhaul:
                    if (splits[22] == PersonalAccountImporter.RecruitmentCode)
                    {
                        acc.ServiceType = PersAccServiceType.OverhaulRecruitment;
                    }
                    break;
                case PersAccServiceType.Recruitment:
                    if (this.overhaulCodeGroup.Contains(splits[22]))
                    {
                        acc.ServiceType = PersAccServiceType.OverhaulRecruitment;
                    }
                    break;
                case PersAccServiceType.OverhaulRecruitment:
                    if (this.overhaulCodeGroup.Contains(splits[22]))
                    {
                        acc.ServiceType = PersAccServiceType.Overhaul;
                    }
                    else if (splits[22] == PersonalAccountImporter.RecruitmentCode)
                    {
                        acc.ServiceType = PersAccServiceType.Recruitment;
                    }
                    break;
            }

            Dictionary<BasePersonalAccount, PersonalAccountPeriodSummary> periodSummariesByAccountDict;
            if (!this.periodSummaryByPeriodByAccount.TryGetValue(period, out periodSummariesByAccountDict))
            {
                periodSummariesByAccountDict =
                    this.periodSummaryByPeriodByAccount[period] = new Dictionary<BasePersonalAccount, PersonalAccountPeriodSummary>();
            }

            PersonalAccountPeriodSummary periodSummary;
            periodSummariesByAccountDict.TryGetValue(acc, out periodSummary);

            if (!period.IsClosed || this.closedPeriodImportRight)
            {
                if (periodSummary != null)
                {
                    // обнуляю значения уже существующих PersonalAccountPeriodSummaries перед импортом
                    if (!this.personalAccountPeriodSummaryItems.Contains(periodSummary))
                    {
                        periodSummary.SaldoIn = 0M;
                        periodSummary.SaldoOut = 0M;
                        periodSummary.ChargedByBaseTariff = 0M;
                        periodSummary.ChargeTariff = 0M;
                        periodSummary.TariffPayment = 0M;
                        periodSummary.RecalcByBaseTariff = 0M;
                        periodSummary.OverhaulPayment = 0M;
                        periodSummary.RecruitmentPayment = 0M;
                        periodSummary.Penalty = 0M;
                        periodSummary.PenaltyPayment = 0M;
                        periodSummary.RecalcByPenalty = 0M;
                    }

                    this.FillPeriodSummary(splits, periodSummary);

                    this.personalAccountPeriodSummaryItems.Add(periodSummary);
                }
                else
                {
                    periodSummary = new PersonalAccountPeriodSummary
                    {
                        PersonalAccount = acc,
                        Period = period
                    };

                    this.FillPeriodSummary(splits, periodSummary);

                    this.personalAccountPeriodSummaryItems.Add(periodSummary);

                    this.periodSummaries.Add(periodSummary);

                    periodSummariesByAccountDict[acc] = periodSummary;
                }

                if (acc.Id == 0)
                {
                    foreach (var walletProp in this.Props)
                    {
                        var wallet = (Wallet)Activator.CreateInstance(typeof(Wallet), Guid.NewGuid().ToString(), acc, WalletHelper.GetWalletTypeByPropertyName(walletProp.Name));

                        this.Accessor[acc, walletProp.Name] = wallet;
                    }
                }

                if (this.penaltyCodeGroup.Contains(splits[22]))
                {
                    this.CreatePenaltyTransfers(splits, acc, period);
                }
                else
                {
                    this.CreateBaseTransfers(splits, acc, period);
                }
            }
            else
            {
                this.Logger.Warn(
                    "У текущего пользователя нет права на импорт в закрытый период",
                    $"Не удалось внести информацию по лс №{acc.PersonalAccountNum} за период {period.Name}");
            }
            /* period summary */
            this.LogManager.LogInformation("{0}. Обработка строки завершена: {1}", this.sw.Elapsed, this.rowNumber);
        }

        private void CreateBaseTransfers(string[] splits, BasePersonalAccount acc, ChargePeriod period)
        {
            var baseWallet = acc.Id > 0 ? this.persAccBaseWalletDict[acc.Id] : acc.BaseTariffWallet;

            // создаем операцию и трансфер начисления
            if (splits[14].ToDecimal() != 0M)
            {
                var operationCharge = new MoneyOperation(PersonalAccountImporter.BillingImportGuid, period)
                {
                    Reason = "Импорт ЛС (Начисление)",
                    OperationDate = DateTime.Now,
                    Amount = splits[14].ToDecimal()
                };

                var baseTariffChargeTransfer = baseWallet.TakeMoney(
                    TransferBuilder.Create(
                        acc,
                        new MoneyStream(
                            this.GetChargeSource(operationCharge.OriginatorGuid),
                            operationCharge,
                            operationCharge.OperationDate,
                            operationCharge.Amount)
                        {
                            Description = "Начислено по базовому тарифу"
                        }));

                this.moneyOperations.Add(operationCharge);
                this.transfers.Add(baseTariffChargeTransfer);
            }

            // создаем операцию и трансфер оплаты
            if (splits[16].ToDecimal() != 0M)
            {
                var operationPayment = new MoneyOperation(PersonalAccountImporter.BillingImportGuid, period)
                {
                    Reason = "Импорт ЛС (Оплата)",
                    OperationDate = DateTime.Now,
                    Amount = splits[16].ToDecimal()
                };

                var baseTariffPaymentTransfer = baseWallet.StoreMoney(
                    TransferBuilder.Create(
                        acc,
                        new MoneyStream(
                            operationPayment.OriginatorGuid,
                            operationPayment,
                            operationPayment.OperationDate,
                            operationPayment.Amount)
                        {
                            Description = "Оплата по базовому тарифу"
                        }));

                this.moneyOperations.Add(operationPayment);
                this.transfers.Add(baseTariffPaymentTransfer);
            }

            // создаем операцию и трансфер перерасчета
            if (splits[17].ToDecimal() != 0M)
            {
                var operationRecalc = new MoneyOperation(PersonalAccountImporter.BillingImportGuid, period)
                {
                    Reason = "Импорт ЛС (Перерасчет)",
                    OperationDate = DateTime.Now,
                    Amount = splits[17].ToDecimal()
                };

                var baseTariffRecalcTransfer = baseWallet.TakeMoney(
                    TransferBuilder.Create(
                        acc,
                        new MoneyStream(
                            this.GetChargeSource(operationRecalc.OriginatorGuid),
                            operationRecalc,
                            operationRecalc.OperationDate,
                            operationRecalc.Amount)
                        {
                            Description = "Перерасчет"
                        }));

                this.moneyOperations.Add(operationRecalc);
                this.transfers.Add(baseTariffRecalcTransfer);
            }
        }

        private void CreatePenaltyTransfers(string[] splits, BasePersonalAccount acc, ChargePeriod period)
        {
            var penaltyWallet = acc.Id > 0 ? this.persAccPenaltyWalletDict[acc.Id] : acc.PenaltyWallet;

            // создаем операцию и трансфер начисления
            if (splits[14].ToDecimal() != 0M)
            {
                var operationCharge = new MoneyOperation(PersonalAccountImporter.BillingImportGuid, period)
                {
                    Reason = "Импорт ЛС (Начисление пени)",
                    OperationDate = DateTime.Now,
                    Amount = splits[14].ToDecimal()
                };

                var penaltyChargeTransfer = penaltyWallet.TakeMoney(
                    TransferBuilder.Create(
                        acc,
                        new MoneyStream(
                            this.GetChargeSource(operationCharge.OriginatorGuid),
                            operationCharge,
                            operationCharge.OperationDate,
                            operationCharge.Amount)
                        {
                            Description = "Начисление пени"
                        }));

                this.moneyOperations.Add(operationCharge);
                this.transfers.Add(penaltyChargeTransfer);
            }

            // создаем операцию и трансфер оплаты
            if (splits[16].ToDecimal() != 0M)
            {
                var operationPayment = new MoneyOperation(PersonalAccountImporter.BillingImportGuid, period)
                {
                    Reason = "Импорт ЛС (Оплата пени)",
                    OperationDate = DateTime.Now,
                    Amount = splits[16].ToDecimal()
                };

                var penaltyPaymentTransfer = penaltyWallet.StoreMoney(
                    TransferBuilder.Create(
                        acc,
                        new MoneyStream(
                            operationPayment.OriginatorGuid,
                            operationPayment,
                            operationPayment.OperationDate,
                            operationPayment.Amount)
                        {
                            Description = "Оплата пени"
                        }));

                this.moneyOperations.Add(operationPayment);
                this.transfers.Add(penaltyPaymentTransfer);
            }

            // создаем операцию и трансфер перерасчета
            if (splits[17].ToDecimal() != 0M)
            {
                var operationRecalc = new MoneyOperation(PersonalAccountImporter.BillingImportGuid, period)
                {
                    Reason = "Импорт ЛС (Перерасчет пени)",
                    OperationDate = DateTime.Now,
                    Amount = splits[17].ToDecimal()
                };

                var penaltyRecalcTransfer = penaltyWallet.TakeMoney(
                    TransferBuilder.Create(
                        acc,
                        new MoneyStream(
                            this.GetChargeSource(operationRecalc.OriginatorGuid),
                            operationRecalc,
                            operationRecalc.OperationDate,
                            operationRecalc.Amount)
                        {
                            Description = "Перерасчет пени"
                        }));

                this.moneyOperations.Add(operationRecalc);
                this.transfers.Add(penaltyRecalcTransfer);
            }
        }

        private void FillPeriodSummary(string[] splits, PersonalAccountPeriodSummary periodSummary)
        {
            periodSummary.SaldoIn += splits[12].ToDecimal(); // BalanceIn

            if (this.penaltyCodeGroup.Contains(splits[22]))
            {
                periodSummary.PenaltyDebt += splits[12].ToDecimal(); // BalanceIn
                periodSummary.Penalty += splits[14].ToDecimal(); // ChargeByBaseTariff
                periodSummary.PenaltyPayment += splits[16].ToDecimal(); // Paid
                periodSummary.RecalcByPenalty += splits[17].ToDecimal(); // RecalculationSum
                periodSummary.SaldoOut += splits[13].ToDecimal(); // SaldoOut
            }
            else
            {
                periodSummary.BaseTariffDebt += splits[12].ToDecimal(); // BalanceIn
                periodSummary.SaldoOut += splits[13].ToDecimal(); // BalanceOut
                periodSummary.ChargedByBaseTariff += splits[14].ToDecimal(); // ChargeByBaseTariff
                periodSummary.ChargeTariff += splits[14].ToDecimal(); // ChargeTariff
                periodSummary.TariffPayment += splits[16].ToDecimal(); // Paid
                periodSummary.RecalcByBaseTariff += splits[17].ToDecimal(); // RecalculationSum

                if (this.overhaulCodeGroup.Contains(splits[22]))
                {
                    periodSummary.OverhaulPayment += splits[16].ToDecimal(); // OverhaulPayment
                }
                else if (splits[22] == PersonalAccountImporter.RecruitmentCode)
                {
                    periodSummary.RecruitmentPayment += splits[16].ToDecimal(); // RecruitmentPayment
                }
            }
        }

        private void AddAddressForMatching(string[] splits)
        {
            var notMatchedAddressInfo = new NotMatchedAddressInfo
            {
                BillingId = splits[1],
                City = splits[25],
                Street = splits[26],
                House = splits[27]
            };
            if (this.addressesForMatching.All(x => x != notMatchedAddressInfo))
            {
                this.addressesForMatching.Add(notMatchedAddressInfo);
            }
        }

        private void SaveImportedAddressForMatching()
        {
            this.AddressService.Save(this.addressesForMatching, this.ArchiveName, "Импорт из ПП Коммунальные платежи");
        }

        /// <summary>
        /// Получить или создать помещение
        /// </summary>
        protected Gkh.Entities.Room GetOrCreateRoom(
            RealityObject ro,
            string roomNum,
            int roomsCount,
            RoomType roomType,
            decimal area,
            decimal livingArea)
        {
            if (ro == null)
            {
                return null;
            }

            var room = this.RoomService.GetAll()
                .FirstOrDefault(x =>
                    x.RealityObject.Id == ro.Id
                    && x.RoomNum == roomNum
                    && x.Type == roomType);

            if (room == null)
            {
                room = new Gkh.Entities.Room
                {
                    RealityObject = ro,
                    RoomNum = roomNum,
                    RoomsCount = roomsCount,
                    Type = roomType,
                    Area = area,
                    LivingArea = livingArea
                };

                this.rooms.Add(room);
            }
            else
            {
                room.RoomsCount = roomsCount;
                room.Type = roomType;
                room.Area = area;
                room.LivingArea = livingArea;
            }

            this.roomItems.Add(room);

            return room;
        }

        private PersonalAccountOwner GetTestAccountOwner()
        {
            if (this.testPersonalAccountOwner != null)
            {
                return this.testPersonalAccountOwner;
            }
            this.testPersonalAccountOwner = this.accountOwnerDict.Values.FirstOrDefault(x => x.Name == "Тестовый");

            if (this.testPersonalAccountOwner != null)
            {
                return this.testPersonalAccountOwner;
            }
            this.testPersonalAccountOwner = new PersonalAccountOwner("Тестовый");

            // быдло-решение
            TransactionHelper.InsertInManyTransactions(this.Container, new[] { this.testPersonalAccountOwner }, 1000, true, true);

            this.accountOwnerDict.Add(this.testPersonalAccountOwner.Id, this.testPersonalAccountOwner);

            return this.testPersonalAccountOwner;
        }

        /// <summary>
        /// Площадь помещения = сумме площадей его лс, доля собственности лс = площадь лс / площадь его помещения
        /// </summary>
        protected void CorrectAreaShare(List<BasePersonalAccount> records)
        {
            // берем только те помещения, у которых во время импорта менялись лс
            var roomIds = records.Select(x => x.Room.Id).Distinct().ToDictionary(x => x, arg => 0);

            var persistenceContext = this.SessionProvider.CurrentSession.GetSessionImplementation().PersistenceContext;

            var roomsToCorrect = this.rooms.Where(x => x.Id != 0 && roomIds.ContainsKey(x.Id)).ToList();

            var accountsByRoomDict = this.accountDict.Values
                .GroupBy(x => x.Room)
                .ToDictionary(x => x.Key, x => x.ToList());

            foreach (var roomToCorrect in roomsToCorrect)
            {
                var room = roomToCorrect;

                // берем ВСЕ лс для помещения, в том числе те, что были еще до импорта
                var roomAccs = accountsByRoomDict.ContainsKey(room)
                    ? accountsByRoomDict[room].Where(x => x.State == null || x.State.Code == "1").ToList()
                    : new List<BasePersonalAccount>();

                var newArea = this.persAccsByRoomDict.ContainsKey(room.Id) ? this.persAccsByRoomDict[room.Id].SafeSum(x => x.Area) : 0M;
                var newLivingArea = room.LivingArea;

                foreach (var acc in roomAccs)
                {
                    var oldAccInfo = this.persAccsByRoomDict.ContainsKey(room.Id)
                        ? this.persAccsByRoomDict[room.Id].FirstOrDefault(x => x.Id == acc.Id)
                        : null;
                    newArea = newArea - (oldAccInfo?.Area ?? 0M) + acc.Area;
                    newLivingArea = newLivingArea - (oldAccInfo?.LivingArea ?? 0M) + acc.LivingArea;
                }

                room.Area = newArea;
                room.LivingArea = newLivingArea;

                if (room.Id > 0) // Иначе залогируем при создании
                {
                    var log =
                        this.LogEntity((Gkh.Entities.Room)persistenceContext.Unproxy(room))
                            .FirstOrDefault(x => x.ParameterName == VersionedParameters.RoomArea);

                    if (log != null)
                    {
                        if (!this.logsForExistingRoom.ContainsKey(room.Id))
                        {
                            this.logsForExistingRoom[room.Id] = new List<EntityLogLight>();
                        }

                        this.logsForExistingRoom[room.Id].Add(log);
                    }
                }

                if (room.Area == 0)
                {
                    continue;
                }

                foreach (var acc in roomAccs)
                {
                    var oldAreaShare = acc.AreaShare;
                    acc.AreaShare = decimal.Round(acc.Area / room.Area, 2);

                    if (oldAreaShare == acc.AreaShare)
                    {
                        continue;
                    }

                    if (acc.Id > 0) // Иначе залогируем при создании
                    {
                        var log = this.LogEntity(acc).FirstOrDefault(x => x.ParameterName == VersionedParameters.AreaShare);

                        if (log != null)
                        {
                            if (!this.logsForExistingAccount.ContainsKey(acc.Id))
                            {
                                this.logsForExistingAccount[acc.Id] = new List<EntityLogLight>();
                            }

                            this.logsForExistingAccount[acc.Id].Add(log);
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
                                if (!this.logsForExistingAccount.ContainsKey(acc.Id))
                                {
                                    this.logsForExistingAccount[acc.Id] = new List<EntityLogLight>();
                                }

                                this.logsForExistingAccount[acc.Id].Add(log);
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
        /// Закрыть периоды
        /// </summary>
        protected void CorrectPeriods()
        {
            var periods = this.chargePeriodItems.OrderBy(x => x.StartDate).ToList();

            if (!periods.Any())
            {
                return;
            }

            foreach (var chargePeriod in periods)
            {
                if (this.chargePeriods.Any(x => x.StartDate > chargePeriod.EndDate))
                {
                    chargePeriod.IsClosed = true;
                }
            }
        }

        /// <summary>
        /// Сохранить или обновить помещения
        /// </summary>
        protected void SaveUpdateRooms()
        {
            this.LogManager.LogInformation("{0}. Сохранение помещений", this.sw.Elapsed);

            var now = DateTime.Now;

            this.roomItems.ForEach(
                x =>
                {
                    x.ObjectEditDate = now;
                    x.ObjectCreateDate = x.Id > 0 ? x.ObjectCreateDate : now;
                    x.ObjectVersion = x.Id > 0 ? (x.ObjectVersion + 1) : 0;
                });

            var roomsToCreate = this.roomItems.Where(x => x.Id == 0).ToList();

            TransactionHelper.InsertInManyTransactions(this.Container, this.roomItems, 1000, false, true);

            // Создаем истории для новых записей
            var newLogs = roomsToCreate.SelectMany(this.LogEntity).ToList();

            // Создаем истории для измененных существующих записей
            newLogs.AddRange(
                this.logsForExistingRoom.SelectMany(
                    x => x.Value.GroupBy(y => new { y.EntityId, y.ParameterName }).Select(y => y.Last()).ToList()));

            this.roomItems.Clear();

            TransactionHelper.InsertInManyTransactions(this.Container, newLogs, 1000, true, true);

            this.LogManager.LogInformation("{0}. Сохранение помещений завершено", this.sw.Elapsed);
        }

        /// <summary>
        /// Сохранить или обновить ЛС
        /// </summary>
        protected void SaveUpdatePersonalAccounts(List<BasePersonalAccount> records)
        {
            this.LogManager.LogInformation("{0}. Сохранение ЛС", this.sw.Elapsed);

            var now = DateTime.Now;

            records.ForEach(
                x =>
                {
                    x.ObjectEditDate = now;
                    x.ObjectCreateDate = x.Id > 0 ? x.ObjectCreateDate : now;
                    x.ObjectVersion = x.Id > 0 ? (x.ObjectVersion + 1) : 0;
                });

            var accountToCreate = records.Where(x => x.Id == 0).ToList();

            TransactionHelper.InsertInManyTransactions(this.Container, records.Select(x => x.BaseTariffWallet), 1000, false, true);
            TransactionHelper.InsertInManyTransactions(this.Container, accountToCreate.Select(x => x.DecisionTariffWallet), 1000, false, true);
            TransactionHelper.InsertInManyTransactions(this.Container, accountToCreate.Select(x => x.PenaltyWallet), 1000, false, true);
            TransactionHelper.InsertInManyTransactions(this.Container, accountToCreate.Select(x => x.RentWallet), 1000, false, true);
            TransactionHelper.InsertInManyTransactions(this.Container, accountToCreate.Select(x => x.SocialSupportWallet), 1000, false, true);
            TransactionHelper.InsertInManyTransactions(this.Container, accountToCreate.Select(x => x.PreviosWorkPaymentWallet), 1000, false, true);
            TransactionHelper.InsertInManyTransactions(this.Container, accountToCreate.Select(x => x.AccumulatedFundWallet), 1000, false, true);
            TransactionHelper.InsertInManyTransactions(this.Container, accountToCreate.Select(x => x.RestructAmicableAgreementWallet), 1000, false, true);

            TransactionHelper.InsertInManyTransactions(this.Container, records, 1000, false, true);

            // Создаем истории для новых записей
            var newLogs = accountToCreate.SelectMany(this.LogEntity).ToList();

            newLogs.AddRange(
                this.logsForExistingAccount.SelectMany(
                    x =>
                        x.Value.Where(y => y.ParameterName != VersionedParameters.AreaShare)
                            .GroupBy(
                                y => new
                                {
                                    y.EntityId,
                                    y.ParameterName
                                })
                            .Select(y => y.Last())
                            .ToList()));

            // Создаем истории для измененных существующих записей
            // Так как доля собственности в коде может меняться несколько раз, 
            // в лог будем записывать только последнее значениe, если оно отличается от исходного
            newLogs.AddRange(
                this.logsForExistingAccount.SelectMany(
                    x =>
                        x.Value.Where(y => y.ParameterName == VersionedParameters.AreaShare)
                            .GroupBy(
                                y => new
                                {
                                    y.EntityId,
                                    y.ParameterName
                                })
                            .Select(y => y.Last())
                            .Where(
                                y => !this.areaShareHistory.ContainsKey(y.EntityId) || this.areaShareHistory[y.EntityId] != y.PropertyValue.ToDecimal())
                            .ToList()));

            records.Clear();

            TransactionHelper.InsertInManyTransactions(this.Container, newLogs, 1000, true, true);

            this.LogManager.LogInformation("{0}. Сохранение ЛС завершено", this.sw.Elapsed);
        }

        /// <summary>
        /// Сохранить или обновить периоды начислений
        /// </summary>
        protected void SaveUpdateChargePeriods()
        {
            TransactionHelper.InsertInManyTransactions(this.Container, this.chargePeriodItems, useStatelessSession: true);

            // создаём несозданные партиции на основании периодов
            foreach (var sql in PersonalAccountImportHelper.GetSqlCommandsNeedToExecute(this.SessionProvider.GetCurrentSession()))
            {
                this.SessionProvider.GetCurrentSession().CreateSQLQuery(sql).ExecuteUpdate();
            }

            this.CorrectPeriodsMore();
        }

        /// <summary>
        /// Закрыть периоды
        /// </summary>
        protected void CorrectPeriodsMore()
        {
            var openPeriodsQuery = this.ChangePeriodService.GetAll().Where(x => !x.IsClosed);

            foreach (var period in openPeriodsQuery.ToList())
            {
                if (period.EndDate.HasValue && this.ChangePeriodService.GetAll().Count(x => x.StartDate > period.EndDate) > 0)
                {
                    period.IsClosed = true;

                    this.ChangePeriodService.Update(period);

                    continue;
                }

                if (this.ChangePeriodService.GetAll().Count(x => x.StartDate > period.StartDate) <= 0)
                {
                    continue;
                }

                period.IsClosed = true;

                if (!period.EndDate.HasValue)
                {
                    period.EndDate = period.StartDate.AddMonths(1).AddDays(-1);
                }

                this.ChangePeriodService.Update(period);
            }

            switch (openPeriodsQuery.Count())
            {
                case 1:
                    {
                        var openPeriod = openPeriodsQuery.FirstOrDefault();

                        if (openPeriod?.EndDate != null)
                        {
                            openPeriod.EndDate = null;
                            this.ChangePeriodService.Update(openPeriod);
                        }
                    }
                    break;
                case 0:
                    {
                        var youngestPeriod = this.ChangePeriodService.GetAll().OrderByDescending(x => x.StartDate).FirstOrDefault();

                        youngestPeriod.IsClosed = false;
                        youngestPeriod.EndDate = null;
                        this.ChangePeriodService.Update(youngestPeriod);
                    }
                    break;
            }
        }

        /// <summary>
        /// Сохранить или обновить информацию по ЛС
        /// </summary>
        protected void SaveUpdatePeriodsSummaries()
        {
            this.LogManager.LogInformation("{0}. Сохранение информации по ЛС за период", this.sw.Elapsed);

            var now = DateTime.Now;

            this.personalAccountPeriodSummaryItems.ForEach(
                x =>
                {
                    x.ObjectEditDate = now;
                    x.ObjectCreateDate = x.Id > 0 ? x.ObjectCreateDate : now;
                    x.ObjectVersion = x.Id > 0 ? (x.ObjectVersion + 1) : 0;
                });

            TransactionHelper.InsertInManyTransactions(this.Container, this.personalAccountPeriodSummaryItems, 1000, true, true);
            this.LogManager.LogInformation("{0}. Сохранение информации по ЛС за период завершено", this.sw.Elapsed);
        }

        /// <summary>
        /// Просуммировать начислено и оплачено из лс в счета начислений (по периодам)
        /// </summary>
        protected void SaveChargeAccountOperations(List<long> roIds, List<long> municipalitiesId)
        {
            this.LogManager.LogInformation("{0}. Сохранение информации информации по счетам начислений", this.sw.Elapsed);
            this.LogManager.LogInformation("{0}. Сохранение информации информации по счетам начислений заврешено", this.sw.Elapsed);

            var chargeAccItems = new List<RealityObjectChargeAccount>();
            var chargeAccOperationsItems = new List<RealityObjectChargeAccountOperation>();

            var roIdsDict = roIds.Distinct().ToDictionary(x => x);

            var persAccs = this.accountDict.Values.Where(x => roIdsDict.ContainsKey(x.Room.RealityObject.Id)).ToList();

            var persAccsIds = persAccs.Select(x => x.Id).Distinct().ToDictionary(x => x);

            // Их счета начислений
            var chargeAccounts = this.ChargeAccountService.GetAll()
                .Where(x => municipalitiesId.Contains(x.RealityObject.Municipality.Id))
                .AsEnumerable()
                .Where(x => roIdsDict.ContainsKey(x.RealityObject.Id))
                .ToList();

            var periods = this.chargePeriods.Select(x => x.Id).ToList();

            var chargeAccountsIds = chargeAccounts.Select(x => x.Id).Distinct().ToDictionary(x => x);

            this.LogManager.LogInformation("{0}. Их счета начислений", this.sw.Elapsed);

            // Операции счета начислений
            var chargeAccOpers = this.ChargeAccountOperService.GetAll()
                .Where(x => municipalitiesId.Contains(x.Account.RealityObject.Municipality.Id) && periods.Contains(x.Period.Id))
                .AsEnumerable()
                .Where(x => chargeAccountsIds.ContainsKey(x.Account.Id))
                .ToList()
                .GroupBy(x => x.Account.Id)
                .ToDictionary(x => x.Key, arg => arg.ToList());

            this.LogManager.LogInformation("{0}. Операции счета начислений", this.sw.Elapsed);

            // операции лс по дому, периоду
            var periodSummaries = this.PeriodSummaryService.GetAll()
                .Where(x => municipalitiesId.Contains(x.PersonalAccount.Room.RealityObject.Municipality.Id) && roIds.Contains(x.PersonalAccount.Room.RealityObject.Id) && periods.Contains(x.Period.Id))
                .Select(x => new { ro_Id = x.PersonalAccount.Room.RealityObject.Id, PeriodSummary = x })
                .AsEnumerable()
                .GroupBy(x => x.ro_Id)
                .ToDictionary(
                    x => x.Key,
                    arg => arg.GroupBy(x => x.PeriodSummary.Period)
                        .ToDictionary(y => y.Key, arg2 => arg2.Select(z => z.PeriodSummary).ToList()));

            this.LogManager.LogInformation("{0}. операции лс по дому, периоду", this.sw.Elapsed);

            // берем счет начислений
            foreach (var charAcc in chargeAccounts)
            {
                this.LogManager.LogInformation("{0}. Счет начислений {1}", this.sw.Elapsed, charAcc.RealityObject.Address);
                Dictionary<ChargePeriod, List<PersonalAccountPeriodSummary>> periodSummariesByRo;
                if (!periodSummaries.TryGetValue(charAcc.RealityObject.Id, out periodSummariesByRo))
                {
                    continue;
                }

                // операции лс по этому дому
                // по всем периодам операций лс
                foreach (var chargePeriod in periodSummariesByRo.Keys)
                {
                    RealityObjectChargeAccountOperation chargeAccOper = null;
                    var period = chargePeriod;

                    // ищем операцию за тот же период в счете начислений иначе создаем
                    List<RealityObjectChargeAccountOperation> chargeOperations;
                    if (chargeAccOpers.TryGetValue(charAcc.Id, out chargeOperations))
                    {
                        chargeAccOper = chargeOperations.FirstOrDefault(x => x.Period.Id == period.Id);
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

                    // агрегируем
                    var summariesPeriod = periodSummariesByRo[period];
                    chargeAccOper.ChargedTotal = summariesPeriod.SafeSum(x => x.ChargedByBaseTariff + x.RecalcByBaseTariff + x.Penalty + x.RecalcByPenalty);
                    chargeAccOper.PaidTotal = summariesPeriod.SafeSum(x => x.TariffPayment);
                    chargeAccOper.SaldoIn = summariesPeriod.SafeSum(x => x.SaldoIn);
                    chargeAccOper.SaldoOut = summariesPeriod.SafeSum(x => x.SaldoOut);
                    chargeAccOper.ChargedPenalty = summariesPeriod.SafeSum(x => x.Penalty + x.RecalcByPenalty);
                    chargeAccOper.PaidPenalty = summariesPeriod.SafeSum(x => x.PenaltyPayment);

                    chargeAccOperationsItems.Add(chargeAccOper);
                }

                if (chargeAccOpers.ContainsKey(charAcc.Id))
                {
                    // Итого по счету начислений
                    charAcc.PaidTotal = chargeAccOpers[charAcc.Id].SafeSum(x => x.PaidTotal + x.PaidPenalty);
                }

                chargeAccItems.Add(charAcc);
            }

            // Намеренно закрываю сессию, что orm забыл про измененные записи, полученные объектами
            this.SessionProvider.CloseCurrentSession();

            TransactionHelper.InsertInManyTransactions(this.Container, chargeAccItems, 1000, true, true);
            TransactionHelper.InsertInManyTransactions(this.Container, chargeAccOperationsItems, 1000, true, true);
            this.LogManager.LogInformation("{0}. Сохранение информации информации по счетам начислений заврешено", this.sw.Elapsed);
        }

        private List<EntityLogLight> LogEntity(IEntity entity)
        {
            if (!VersionedEntityHelper.IsUnderVersioning(entity))
            {
                return new List<EntityLogLight>();
            }

            var currentOperator = this.UserManager?.GetActiveOperator();
            var login = currentOperator?.User?.Login ?? "anonymous";

            var parameters =
                VersionedEntityHelper.GetCreator(entity)
                    .CreateParameters()
                    .Where(x => !VersionedEntityHelper.ShouldSkip(entity, x.ParameterName))
                    .ToList();

            var hasDate = entity as IHasDateActualChange;

            var now = DateTime.UtcNow;

            var logs = parameters.Select(
                x => new EntityLogLight
                {
                    EntityId = entity.Id.To<long>(),
                    ClassName = x.ClassName,
                    PropertyName = x.PropertyName,
                    PropertyValue = x.PropertyValue,
                    ParameterName = x.ParameterName,
                    DateApplied = now,
                    DateActualChange = hasDate.Return(
                        y => y.ActualChangeDate,
                        new DateTime(
                            now.Year,
                            now.Month,
                            1,
                            now.Hour,
                            now.Minute,
                            now.Second,
                            now.Millisecond)),
                    User = login,
                    ObjectCreateDate = now,
                    ObjectEditDate = now,
                    Reason = this.FileName,
                    Document = this.File
                })
                .ToList();

            return logs;
        }

        private void UpdateRoomOwnershipType(Gkh.Entities.Room room, RoomOwnershipType type, DateTime factDate)
        {
            this.versions = this.versions ?? new List<BaseParams>();
            var userRepo = this.Container.ResolveDomain<User>();
            var userIdentity = this.Container.Resolve<IUserIdentity>();

            if (room.OwnershipType == type)
            {
                return;
            }

            if (room.Id == 0)
            {
                room.OwnershipType = type;
                return;
            }
            room.OwnershipType = type;

            var login = userRepo.Get(userIdentity.UserId).Return(u => u.Login);

            var entityLogLight = new EntityLogLight()
            {
                ClassName = "Room",
                EntityId = room.Id,
                PropertyName = "OwnershipType",
                PropertyValue = type.GetDisplayName(),
                ParameterName = "Тип собственности помещения",
                DateActualChange = factDate,
                DateApplied = DateTime.UtcNow,
                User = login.IsEmpty() ? "anonymous" : login,
                Reason = "Импорт лицевых счетов"
            };

            this.logLights.Add(entityLogLight);
        }

        /// <summary>
        /// Информация о площади ЛС
        /// </summary>
        public class PersAccAreasInfo
        {
            public long Id { get; set; }

            public decimal Area { get; set; }

            public decimal LivingArea { get; set; }

            public decimal AreaShare { get; set; }

            public State State { get; set; }
        }

        private ITransferParty GetChargeSource(string guid)
        {
            return new ChargeOriginatorWrapper(new FakeTransferParty(guid));
        }
    }
}