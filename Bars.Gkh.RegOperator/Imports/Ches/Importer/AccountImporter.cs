namespace Bars.Gkh.RegOperator.Imports.Ches
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Modules.Caching.LinqExtensions;
    using Bars.B4.Modules.States;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.ExecutionAction;
    using Bars.Gkh.RegOperator.Domain.ParametersVersioning;
    using Bars.Gkh.RegOperator.DomainService.PersonalAccount;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Entities.Import.Ches;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount;
    using Bars.Gkh.RegOperator.Entities.Wallet;
    using Bars.Gkh.RegOperator.Enums;
    using Bars.Gkh.RegOperator.ExecutionAction;
    using Bars.Gkh.RegOperator.Extenstions;
    using Bars.Gkh.RegOperator.Imports.Ches.PreImport;
    using Bars.Gkh.Repositories;
    using Bars.Gkh.Repositories.ChargePeriod;
    using Bars.Gkh.Utils;

    using Castle.Core.Internal;

    using NHibernate.Linq;

    using EnumerableExtensions = Bars.B4.Modules.Analytics.Reports.Extensions.EnumerableExtensions;

    /// <summary>
    /// Импорт лицевых счетов
    /// </summary>
    public class AccountImporter : BaseChesImporter<AccountFileInfo>
    {
        /// <summary>
        /// Идентификатор импорта
        /// </summary>
        public static string Id = MethodBase.GetCurrentMethod().DeclaringType.FullName;

        /// <summary>
        /// Название импорта
        /// </summary>
        public override string ImportName => "Импорт лицевых счетов";

        /// <summary>
        /// Репозиторий для <see cref="BasePersonalAccount"/>
        /// </summary>
        public IRepository<BasePersonalAccount> PersonalAccountRepo { get; set; }

        /// <summary>
        /// Репозиторий для <see cref="PersonalAccountOwner"/>
        /// </summary>
        public IRepository<PersonalAccountOwner> PersonalAccountOwnerRepo { get; set; }

        /// <summary>
        /// Репозиторий для <see cref="LegalAccountOwner"/>
        /// </summary>
        public IRepository<LegalAccountOwner> LegalAccountOwnerRepo { get; set; }

        /// <summary>
        /// Репозиторий для <see cref="IndividualAccountOwner"/>
        /// </summary>
        public IRepository<IndividualAccountOwner> IndividualAccountOwnerRepo { get; set; }

        /// <summary>
        /// Репозиторий для <see cref="Contragent"/>
        /// </summary>
        public IRepository<Contragent> ContragentRepo { get; set; }

        /// <summary>
        /// Репозиторий для <see cref="Room"/>
        /// </summary>
        public IRepository<Room> RoomRepo { get; set; }

        /// <summary>
        /// Репозиторий для <see cref="AccountOwnershipHistory"/>
        /// </summary>
        public IRepository<AccountOwnershipHistory> AccountOwnershipHistoryRepo { get; set; }

        /// <summary>
        /// Репозиторий для <see cref="RealityObject"/>
        /// </summary>
        public IRepository<RealityObject> RealityObjectRepo { get; set; }

        public IRepository<CalcDebtDetail> CalcDebtDetailRepo { get; set; }

        public IRepository<AddressMatch> AddressMatchRepo { get; set; }
        public IRepository<ChesMatchLegalAccountOwner> ChesMatchLegalAccountOwnerRepo { get; set; }

        public IChargePeriodRepository ChargePeriodRepository { get; set; }

        public IStateRepository StateRepository { get; set; }

        public ISessionProvider SessionProvider { get; set; }

        private Dictionary<string, long> accountsByNumDict;
        private Dictionary<string, Tuple<DateTime?, long>[]> individualAccountOwners;
        private Dictionary<long, LegalAccountOwner[]> legalAccountOwnersByContragent;
        private Dictionary<long, LegalAccountOwner> legalAccountOwnersById;
        private Dictionary<string, Contragent[]> contragentsDict;
        private Dictionary<string, long> rooms;
        private Dictionary<string, OwnershipHistoryItem[]> ownershipHistory;

        private IDictionary<string, Room> portionRoomDict;
        private IDictionary<long, BasePersonalAccount> portionAccountDict;

        private HashSet<long> realityObjects;
        private HashSet<LegalOwnerProxy> legalOwnerMatchSet;
        private Dictionary<string, Dictionary<string, decimal>> roomAreaSharesDict;
        private Dictionary<long, RealityObjectTypeCRCondition> typeCRConditionDict;
        private State accountState;
        private State closeState;
        private State notActiveState;

        private Dictionary<string, Room> createdRooms;

        private Dictionary<string, long> roIdByHouseGuid;
        private Dictionary<string, long> roIdByHouseAddress;

        private readonly List<BasePersonalAccount> accountsForSave = new List<BasePersonalAccount>();
        private readonly List<Tuple<DateTime, long>> ownerToUpdateBirthDate = new List<Tuple<DateTime, long>>();
        private readonly List<IndividualAccountOwner> individualOwnersForSave = new List<IndividualAccountOwner>();
        private readonly List<LegalAccountOwner> legalOwnersForSave = new List<LegalAccountOwner>();
        private readonly List<BasePersonalAccount> accountsForUpdate = new List<BasePersonalAccount>();
        private readonly List<PersonalAccountPeriodSummary> summaryForSave = new List<PersonalAccountPeriodSummary>();
        private readonly List<Func<EntityLogLight>> entityLogsForSave = new List<Func<EntityLogLight>>();
        private readonly List<Func<PersonalAccountChange>> accountChangesForSave = new List<Func<PersonalAccountChange>>();
        private readonly List<Func<AccountOwnershipHistory>> ownershipHistoryForSave = new List<Func<AccountOwnershipHistory>>();
        private readonly List<Room> roomsForSave = new List<Room>();
        private readonly List<Room> roomsForUpdate = new List<Room>();
        private readonly List<Wallet> walletsForSave = new List<Wallet>();
        private Dictionary<long, string> ownershipHistoryDict;

        /// <summary>
        /// Проинициализировать справочники
        /// </summary>
        /// <param name="accountFileInfo">Файл импорта</param>
        public override void InitDicts(AccountFileInfo accountFileInfo)
        {
            this.Indicate?.Invoke(3, $"Подготовка кэша ({accountFileInfo.Rows.Count} строк)");

            var filterByRows = accountFileInfo.Rows.Count < 50000;

            using (var statelessSession = this.SessionProvider.OpenStatelessSession())
            {
                this.contragentsDict = statelessSession.Query<Contragent>()
                    .Where(x => x.ContragentState == ContragentState.Active)
                    .AsEnumerable()
                    .GroupBy(x => $"{x.Inn?.Trim()}#{x.Kpp?.Trim()}")
                    .ToDictionary(x => x.Key, x => x.ToArray());

                this.legalAccountOwnersById = statelessSession.Query<LegalAccountOwner>()
                    .Fetch(x => x.Contragent)
                    .Where(x => x.Contragent.ContragentState == ContragentState.Active)
                    .AsEnumerable()
                    .ToDictionary(x => x.Id);

                this.legalAccountOwnersByContragent = this.legalAccountOwnersById.Values
                    .GroupBy(x => x.Contragent.Id)
                    .ToDictionary(x => x.Key, x => x.ToArray());
            }

            var accounts = this.PersonalAccountRepo.GetAll()
                 .Select(x => new
                 {
                     x.Id,
                     x.PersonalAccountNum,
                     x.PersAccNumExternalSystems
                 })
                 .ToArray();

            this.accountsByNumDict = accounts.ToDictionary(x => x.PersonalAccountNum, x => x.Id);

            var surnames = accountFileInfo.Rows.Select(y => y.Surname).ToArray();
            var firstNames = accountFileInfo.Rows.Select(y => y.Name).ToArray();
            var lastNames = accountFileInfo.Rows.Select(y => y.Lastname).ToArray();
            var birstDates = accountFileInfo.Rows.Where(x => x.BirthDate.HasValue).Select(y => y.BirthDate.Value).ToArray();

            this.individualAccountOwners = this.IndividualAccountOwnerRepo.GetAll()
                .Where(x => x.BirthDate.HasValue)
                .Select(x => new
                {
                    x.Id,
                    x.Surname,
                    x.FirstName,
                    x.SecondName,
                    x.BirthDate
                })
                .WhereIf(filterByRows, x => surnames.Contains(x.Surname))
                .WhereIf(filterByRows, x => firstNames.Contains(x.FirstName))
                .WhereIf(filterByRows, x => lastNames.Contains(x.SecondName))
                .WhereIf(filterByRows, x => birstDates.Contains(x.BirthDate.Value))
                .Select(x => new
                {
                    Key = $"{x.Surname}#{x.FirstName}#{x.SecondName}",
                    x.Id,
                    x.BirthDate
                })
                .AsEnumerable()
                .GroupBy(x => x.Key, x => Tuple.Create(x.BirthDate, x.Id))
                .ToDictionary(x => x.Key, x => x.ToArray());

            this.rooms = this.RoomRepo.GetAll()
                .Select(x => new
                {
                    RoId = x.RealityObject.Id,
                    x.RoomNum,
                    x.ChamberNum,
                    x.Id
                })
                .AsEnumerable()
                .GroupBy(x => $"{x.RoId}#{x.RoomNum}#{x.ChamberNum}", x => x.Id)
                .ToDictionary(x => x.Key, x => x.First());

            this.roomAreaSharesDict = this.PersonalAccountRepo.GetAll()
                .Select(x => new
                {
                    x.PersonalAccountNum,
                    x.Room.Id,
                    x.AreaShare,
                    RoId = x.Room.RealityObject.Id,
                    x.Room.RoomNum,
                    x.Room.ChamberNum
                })
                .AsEnumerable()
                .GroupBy(x => $"{x.RoId}#{x.RoomNum}#{x.ChamberNum}")
                .ToDictionary(x => x.Key, y => y.ToDictionary(x => x.PersonalAccountNum, x => x.AreaShare));
            
            this.realityObjects = this.RealityObjectRepo.GetAll().Select(x => x.Id).ToHashSet();
            this.createdRooms = new Dictionary<string, Room>();

            var listAddresses = this.AddressMatchRepo.GetAll()
                .Select(x => new
                {
                    ExternalAddress = x.ExternalAddress.ToLower(),
                    RoId = x.RealityObject.Id,
                    x.HouseGuid
                })
                .ToList();

            this.roIdByHouseAddress = listAddresses
                .DistinctBy(x => x.ExternalAddress)
                .ToDictionary(x => x.ExternalAddress, x => x.RoId);

            this.roIdByHouseGuid = listAddresses.Where(x => x.HouseGuid.IsNotEmpty())
                .GroupBy(x => x.HouseGuid, x => x.RoId)
                .Where(x => x.Count() == 1)
                .ToDictionary(x => x.Key, x => x.First());

            var ownerMatchList = this.ChesMatchLegalAccountOwnerRepo.GetAll()
                .Select(x => new LegalOwnerProxy
                {
                    Inn = x.Inn,
                    Kpp = x.Kpp,
                    Name = x.Name,
                    OwnerId = x.AccountOwner.Id
                })
                .AsEnumerable();
            this.legalOwnerMatchSet = ownerMatchList.ToHashSet();

            this.ownershipHistoryDict = this.AccountOwnershipHistoryRepo.GetAll()
                .Where(x => x.Date <= accountFileInfo.Period.EndDate)
                .Select(x => new
                {
                    x.Date,
                    x.PersonalAccount.Id,
                    x.AccountOwner.Name
                })
                .AsEnumerable()
                .GroupBy(x => x.Id)
                .ToDictionary(x => x.Key, x => x.OrderByDescending(y => y.Date).Select(y => y.Name).FirstOrDefault());
            try
            {
                typeCRConditionDict = RealityObjectRepo.GetAll()
                   .Select(x => new
                   {
                       x.Id,
                       x.ConditionHouse,
                       x.AccountFormationVariant
                   })
                   .ToDictionary(x => x.Id, x => new RealityObjectTypeCRCondition()
                   {
                       ConditionHouse = x.ConditionHouse,
                       CrFundFormationType = x.AccountFormationVariant.HasValue? x.AccountFormationVariant.Value : CrFundFormationType.NotSelected
                   });
            }
            catch
            {
                // на всякий случай, чтобы не повредить импортер
            }



            this.accountState = this.StateRepository.GetAllStates<BasePersonalAccount>().FirstOrDefault(x => x.StartState && x.Code == "1");
            this.closeState = this.StateRepository.GetAllStates<BasePersonalAccount>().FirstOrDefault(x => x.Code == "2");
            this.notActiveState = this.StateRepository.GetAllStates<BasePersonalAccount>().FirstOrDefault(x => x.Code == "4");

            this.ChargePeriodRepository.InitCache();
        }

        private void InitPortionDict(AccountFileInfo.Row[] portion)
        {
            var roIds = portion.Select(
                    x =>
                    {
                        long roId;
                        this.TryGetRoId(x, out roId);

                        return roId;
                    })
                .Distinct()
                .ToArray();

            var accountNumbers = portion.Select(x => x.LsNum).ToArray();

            using (var statelessSession = this.SessionProvider.OpenStatelessSession())
            {
                this.portionRoomDict = statelessSession.Query<Room>()
                    .WhereContains(x => x.RealityObject.Id, roIds)
                    .AsEnumerable()
                    .GroupBy(x => $"{x.RealityObject.Id}#{x.RoomNum}#{x.ChamberNum}")
                    .ToDictionary(x => x.Key, x => x.First());

                var accountIds = accountNumbers
                    .Select(x => this.accountsByNumDict.Get(x))
                    .Distinct()
                    .ToArray();

                var accounts = statelessSession.Query<BasePersonalAccount>()
                    .Fetch(x => x.AccountOwner)
                    .Fetch(x => x.Room)
                    .Fetch(x => x.State)
                    .GetPortioned(x => x.Id, accountIds, 2000);

                this.portionAccountDict = accounts.ToDictionary(x => x.Id);

                this.ownershipHistory = statelessSession.Query<AccountOwnershipHistory>()
                    .Select(x => new
                    {
                        AccountId = x.PersonalAccount.Id,
                        PersAccNum = x.PersonalAccount.PersonalAccountNum,
                        OwnerId = x.AccountOwner.Id,
                        ActualFrom = x.ActualFrom
                    })
                    .GetPortioned(x => x.AccountId, accountIds, 2000)
                    .GroupBy(
                        x => x.PersAccNum,
                        x => new OwnershipHistoryItem
                        {
                            AccountId = x.AccountId,
                            OwnerId = x.OwnerId,
                            ActualFrom = x.ActualFrom
                        })
                    .ToDictionary(x => x.Key, y => y.OrderByDescending(x => x.ActualFrom).ToArray());
            }
        }

        /// <summary>
        /// Обработать данные импорта
        /// </summary>
        /// <param name="accountFileInfo">Файл импорта</param>
        public override void ProcessData(AccountFileInfo accountFileInfo)
        {
            var percent = 0;

            //сортируем лс по:
            // 1. Адрес дома
            // 2. Помещение
            // 3. Закрытые сначала (у них доля собственности 0)
            // 4. По долям собственности
            var accounts = accountFileInfo.Rows
                .OrderBy(x => x.AddHouse)
                .ThenBy(x => x.RoomAddress)
                .ThenBy(x => x.Share)
                .Split(100000);

            foreach (var rows in accounts)
            {
                var rowArray = rows.ToArray();
                var percentValue = ((double)percent / accountFileInfo.Rows.Count) * 85;
                this.Indicate?.Invoke(Math.Min((int)percentValue, 100), $"Подготовка кэша ({percent}/{accountFileInfo.Rows.Count})");

                this.InitPortionDict(rowArray);

                percentValue = ((double)percent / accountFileInfo.Rows.Count) * 90;
                this.Indicate?.Invoke(Math.Min((int)percentValue, 100), $"Импорт ({percent}/{accountFileInfo.Rows.Count})");

                foreach (var row in rowArray)
                {
                    var account = this.GetAccount(row);
                    if (account == null)
                    {
                        long roId;
                        if (!this.TryGetRoId(row, out roId))
                        {
                            accountFileInfo.AddErrorRow(row, $"Жилой дом \"{row.AddHouse}\" не найден. Строка {row.RowNumber}", row.LsNum, row.ExtLsNum);
                            continue;
                        }

                        account = this.CreateAccount(row);
                    }

                    if (!this.ProcessRoom(account, row, accountFileInfo))
                    {
                        continue;
                    }

                    if (!this.ProcessAccountChange(account, row, accountFileInfo))
                    {
                        continue;
                    }

                    if (row.BillType == PersonalAccountOwnerType.Individual)
                    {
                        if (!this.ProcessIndividualOwner(row, account, accountFileInfo))
                        {
                            continue;
                        }
                    }
                    else if (row.BillType == PersonalAccountOwnerType.Legal)
                    {
                        if (!this.ProcessLegalOwner(row, account, accountFileInfo))
                        {
                            this.Container.UsingForResolved<DomainService.Import.Ches.IChesComparingService>((cnt, srv) => srv.ProcessAccountImported(this.GetTemporaryImporter(accountFileInfo)));
                           
                            continue;
                        }
                    }

                    if (account.Id == 0)
                    {
                        var wallets = account.GetWallets();
                        wallets.AddTo(this.walletsForSave);
                        this.accountsForSave.Add(account);

                        var periods = this.ChargePeriodRepository.GetAll()
                            .Where(x => x.StartDate >= accountFileInfo.Period.StartDate)
                            .ToList();

                        foreach (var period in periods)
                        {
                            this.summaryForSave.Add(new PersonalAccountPeriodSummary(account, period, 0));
                        }

                        this.LogImport.CountAddedRows++;
                        this.LogImport.Info(this.ImportName,
                            $"Строка {row.RowNumber}: создан лицевой счет", row.LsNum, row.ExtLsNum);
                    }

                    if (row.HasChangeOwner)
                    {
                        this.LogImport.Info(this.ImportName,
                            $"Строка {row.RowNumber}: абонент обновлен", row.LsNum, row.ExtLsNum);
                    }

                    if (row.HasChangeRoom)
                    {
                        this.roomsForUpdate.Add(account.Room);
                        this.LogImport.Info(this.ImportName,
                            $"Строка {row.RowNumber}: помещение обновлено", row.LsNum, row.ExtLsNum);
                    }

                    if (row.HasChangeAccount && account.Id > 0)
                    {
                        this.accountsForUpdate.Add(account);
                        this.LogImport.Info(this.ImportName,
                            $"Строка {row.RowNumber}: лицевой счет обновлен", row.LsNum, row.ExtLsNum);
                    }

                    var incerementChangedRow = row.HasChangeOwner || row.HasChangeAccount && account.Id > 0 || row.HasChangeRoom;
                    if (incerementChangedRow)
                    {
                        this.LogImport.CountChangedRows++;
                    }
                }

                var ownerIds = this.ownerToUpdateBirthDate.Select(x => x.Item2).Distinct().ToArray();

                var ownersList = new List<IndividualAccountOwner>();
                foreach (var ownerSection in ownerIds.Split(2000))
                {
                    this.IndividualAccountOwnerRepo.GetAll()
                        .Where(x => ownerSection.Contains(x.Id))
                        .AddTo(ownersList);
                }

                var owners = ownersList.ToDictionary(x => x.Id);

                this.ownerToUpdateBirthDate.ForEach(x =>
                {
                    var owner = owners.Get(x.Item2);
                    if (owner.IsNotNull())
                    {
                        owner.BirthDate = x.Item1;
                    }
                });

                percent = Math.Min(percent + 100000, accountFileInfo.Rows.Count);
                percentValue = ((double)percent / accountFileInfo.Rows.Count) * 85;
                this.Indicate?.Invoke(Math.Min((int)percentValue, 100), $"Сохранение ({percent}/{accountFileInfo.Rows.Count} строк)");

                TransactionHelper.InsertInManyTransactions(this.Container, this.roomsForSave, 10000, false, true);
                TransactionHelper.InsertInManyTransactions(this.Container, this.roomsForUpdate, 10000, true, true);
                TransactionHelper.InsertInManyTransactions(this.Container, this.individualOwnersForSave, 10000, false, true);
                TransactionHelper.InsertInManyTransactions(this.Container, owners.Values, 10000, true, true);
                TransactionHelper.InsertInManyTransactions(this.Container, this.legalOwnersForSave, 10000, false, true);
                TransactionHelper.InsertInManyTransactions(this.Container, this.walletsForSave, 10000, true, true);
                TransactionHelper.InsertInManyTransactions(this.Container, this.accountsForSave, 10000, false, true);
                TransactionHelper.InsertInManyTransactions(this.Container, this.accountsForUpdate, 10000, true, true);
                TransactionHelper.InsertInManyTransactions(this.Container, this.summaryForSave, 10000, true, true);

                this.roomsForSave.ForEach(x => this.rooms[$"{x.RealityObject.Id}#{x.RoomNum}#{x.ChamberNum}"] = x.Id);
                this.roomsForSave.Clear();

                this.individualOwnersForSave.ForEach(x => this.individualAccountOwners[$"{x.Surname}#{x.FirstName}#{x.SecondName}"] = new[] { Tuple.Create(x.BirthDate, x.Id) });
                this.individualOwnersForSave.Clear();

                this.legalOwnersForSave.ForEach(x => this.legalAccountOwnersByContragent[x.Contragent.Id] = new[] { x });
                this.legalOwnersForSave.ForEach(x => this.legalAccountOwnersById[x.Id] = x);
                this.legalOwnersForSave.Clear();

                this.accountsForSave.ForEach(x => this.accountsByNumDict[x.PersonalAccountNum] = x.Id);
                this.accountsForSave.Clear();

                this.roomsForUpdate.Clear();
                this.walletsForSave.Clear();
                this.accountsForUpdate.Clear();
                this.ownerToUpdateBirthDate.Clear();
                this.summaryForSave.Clear();

                var changes = new List<PersonalAccountChange>();
                foreach (var changeFunc in this.accountChangesForSave)
                {
                    var change = changeFunc();
                    if (change.PersonalAccount.Id == 0)
                    {
                        continue;
                    }
                    change.NewValue = change.PersonalAccount.AccountOwner.Id.ToString();
                    changes.Add(change);
                }
                TransactionHelper.InsertInManyTransactions(this.Container, changes, 10000, true, true);
                TransactionHelper.InsertInManyTransactions(
                    this.Container,
                    this.entityLogsForSave.Select(x => x()).Where(x => x.EntityId > 0).ToArray(),
                    10000,
                    true,
                    true);

                TransactionHelper.InsertInManyTransactions(this.Container, this.ownershipHistoryForSave.Select(x => x()).ToArray(), 10000, true, true);

                this.accountChangesForSave.Clear();
                this.entityLogsForSave.Clear();
                this.ownershipHistoryForSave.Clear();

                this.SessionProvider.GetCurrentSession().Clear();
            }

            this.Indicate?.Invoke(96, "Обновление данных по абонентам");
            this.Container.UsingForResolved<IExecutionAction>(nameof(RecalcOwnerAccountsCountAction), (cnt, action) => action.Action());

            this.Indicate?.Invoke(98, "Обновление реестра лицевых счетов");
            this.Container.UsingForResolved<IMassPersonalAccountDtoService>((cnt, service) => service.MassCreatePersonalAccountDto(true));
        }

        private bool ProcessIndividualOwner(AccountFileInfo.Row row, BasePersonalAccount account, AccountFileInfo accountFileInfo)
        {
            // Если Тип абонента или Тип собственности не изменился, то не меняем абонента
            var shouldChangeOwner = row.BillType != account.AccountOwner?.OwnerType || row.PropertyType != account.Room?.OwnershipType;

            if (account.AccountOwner.IsNull() || shouldChangeOwner)
            {
            IndividualAccountOwner owner;

            var existOwners = this.individualAccountOwners.Get($"{row.Surname}#{row.Name}#{row.Lastname}");
            if (existOwners.IsNullOrEmpty())
            {
                owner = this.individualOwnersForSave
                    .Where(x => x.Surname == row.Surname)
                    .Where(x => x.FirstName == row.Name)
                    .FirstOrDefault(x => x.SecondName == row.Lastname);

                if (owner == null)
                {
                    owner = this.CreateIndividualOwner(row);
                }
            }
            else if (existOwners.Count(x => x.Item1 == row.BirthDate) > 1)
            {
                    accountFileInfo.AddErrorRow(row,
                        $"В системе найдено несколько абонентов {row.Surname} {row.Name} {row.Lastname}, {row.BirthDate}. Строка {row.RowNumber}", row.LsNum, row.ExtLsNum);
                return false;
            }
            else
            {
                var ownerTuple = existOwners.FirstOrDefault(x => x.Item1 == row.BirthDate) ?? existOwners.First();
                owner = new IndividualAccountOwner
                {
                    Id = ownerTuple.Item2
                };

                if (row.BirthDate.HasValue && ownerTuple.Item1 != row.BirthDate)
                {
                    row.HasChangeOwner = true;
                    this.ownerToUpdateBirthDate.Add(Tuple.Create(row.BirthDate.Value, ownerTuple.Item2));
                }
            }

            if (account.AccountOwner == null)
            {
                account.AccountOwner = owner;
                owner.TotalAccountsCount++;

                return true;
            }

            if (account.AccountOwner.Id != owner.Id)
            {
                this.ChangeOwner(account, owner, row, accountFileInfo.Period);
            }
            }

            return true;
        }

        private bool ProcessLegalOwner(AccountFileInfo.Row row, BasePersonalAccount account, AccountFileInfo accountFileInfo)
        {
            // Если Тип абонента или Тип собственности не изменился, то не меняем абонента
            var shouldChangeOwner = row.BillType != account.AccountOwner?.OwnerType || row.PropertyType != account.Room?.OwnershipType;

            if (account.AccountOwner.IsNull() || shouldChangeOwner)
            {
                LegalAccountOwner owner;

                if (this.TryGetLegalAccountOwner(accountFileInfo, row, out owner))
                {
                    if (account.AccountOwner == null)
                    {
                        account.AccountOwner = owner;
                        return true;
                    }

                    if (account.AccountOwner.Id != owner.Id)
                    {
                        this.ChangeOwner(row, account, owner, accountFileInfo.Period);
                    }
                }
                else
                {
                    return false;
                }
            }

            return true;
        }

        private bool ProcessAccountChange(BasePersonalAccount account, AccountFileInfo.Row row, AccountFileInfo accountFileInfo)
        {
            if (account.Id == 0 && (!row.Share.HasValue || !row.ShareDate.HasValue))
            {
                accountFileInfo.AddErrorRow(row, $"Не заполнена доля собственности или дата начала ее действия. Строка {row.RowNumber}", row.LsNum, row.ExtLsNum);
                return false;
            }

            if (account.PersAccNumExternalSystems != row.ExtLsNum)
            {
                row.HasChangeAccount = true;
                account.PersAccNumExternalSystems = row.ExtLsNum;

                this.AddExternalNumLog(row, account);
            }

            if (row.Share.HasValue && row.ShareDate.HasValue && account.AreaShare != row.Share && this.CheckAreaShare(account, row, accountFileInfo))
            {
                row.HasChangeAccount = true;
                account.AreaShare = row.Share.Value;

                var roomKey = $"{account.Room.RealityObject.Id}#{account.Room.RoomNum}#{account.Room.ChamberNum}";
                this.roomAreaSharesDict[roomKey][account.PersonalAccountNum] = account.AreaShare;

                this.AddAccountAreaShareLog(row, account);
            }

            if (account.Id == 0 && row.LsOpenDate == null)
            {
                accountFileInfo.AddErrorRow(row, $"Не заполнена дата открытия ЛС. Строка {row.RowNumber}", row.LsNum, row.ExtLsNum);
            }
            else if (account.OpenDate != row.LsOpenDate && row.LsOpenDate.HasValue && row.LsOpenStart.HasValue)
            {
                row.HasChangeAccount = true;
                account.OpenDate = row.LsOpenDate.Value;

                this.AddAccountOpenDateLog(account, row);
            }

            if (account.CloseDate != (row.LsCloseDate ?? DateTime.MinValue))
            {
                row.HasChangeAccount = true;
                account.SetCloseDate(row.LsCloseDate ?? DateTime.MinValue, false);
                account.State = row.LsCloseDate.HasValue ? this.closeState : this.accountState;

                if (row.LsCloseDate.HasValue)
                {
                    this.AddACloseChange(account, row, accountFileInfo.Period);
                }
            }

            return true;
        }

        private bool ProcessRoom(BasePersonalAccount account, AccountFileInfo.Row row, AccountFileInfo accountFileInfo)
        {
            if (account.Room == null)
            {
                long roId;

                if (this.TryGetRoId(row, out roId))
                {
                    if (row.RoomNum.IsEmpty())
                    {
                        accountFileInfo.AddErrorRow(row, $"Не заполнен номер помещения. Строка {row.RowNumber}", row.LsNum, row.ExtLsNum);
                        return false;
                    }

                    var room = this.GetOrCreateRoom(row, roId);
                    account.Room = room;

                    this.LogImport.Info(this.ImportName, $"Строка {row.RowNumber}: к ЛС привязано помещение {account.Room.RoomNum}");
                }
                else
                {
                    accountFileInfo.AddErrorRow(row, $"Жилой дом \"{row.AddHouse}\" не найден. Строка {row.RowNumber}", row.LsNum, row.ExtLsNum);
                    return false;
                }
            }

            if (row.TotalArea > 0 && account.Room.Area != row.TotalArea)
            {
                row.HasChangeRoom = true;
                account.Room.Area = row.TotalArea;

                this.AddRoomAreaLog(row, account.Room);
            }

            if (row.PropertyType > 0 && account.Room.OwnershipType != row.PropertyType)
            {
                row.HasChangeRoom = true;
                account.Room.OwnershipType = row.PropertyType;

                this.AddRoomOwnershipTypeLog(row, account.Room);
            }

            if (row.FlatPlaceType > 0 && account.Room.Type != row.FlatPlaceType)
            {
                row.HasChangeRoom = true;
                account.Room.Type = row.FlatPlaceType;
            }

            return true;
        }

        private bool TryGetRoId(AccountFileInfo.Row row, out long roId)
        {
            roId = 0;

            if (this.realityObjects.Contains(row.HouseId.ToLong()))
            {
                roId = row.HouseId.ToLong();
                return true;
            }

            if (row.FiasHouseId.IsNotEmpty() && this.roIdByHouseGuid.TryGetValue(row.FiasHouseId, out roId))
            {
                return true;
            }

            if (row.AddHouse.IsNotEmpty() && this.roIdByHouseAddress.TryGetValue(row.AddHouse.ToLower(), out roId))
            {
                return true;
            }

            return false;
        }

        private BasePersonalAccount GetAccount(AccountFileInfo.Row row)
        {
            var accountId = this.accountsByNumDict.Get(row.LsNum);
            return accountId > 0 ? this.portionAccountDict.Get(accountId) : null;
        }

        private BasePersonalAccount CreateAccount(AccountFileInfo.Row row)
        {

            long ro_id = !string.IsNullOrEmpty(row.HouseId) ? Convert.ToInt64(row.HouseId) : 0;
             
            var account = new BasePersonalAccount
            {
                PersonalAccountNum = row.LsNum,
                PersAccNumExternalSystems = row.ExtLsNum,
                State = this.accountState
            };

            if (ro_id > 0 && this.typeCRConditionDict.ContainsKey(ro_id))
            {
                if (typeCRConditionDict[ro_id].ConditionHouse == ConditionHouse.Emergency || typeCRConditionDict[ro_id].ConditionHouse == ConditionHouse.Razed)
                {
                    account.State = this.closeState;
                }
                else if (typeCRConditionDict[ro_id].CrFundFormationType != CrFundFormationType.RegOpAccount)
                {
                    account.State = this.notActiveState;
                }
               
            }

            return account;
        }

        private bool TryGetLegalAccountOwner(AccountFileInfo accountFileInfo, AccountFileInfo.Row row, out LegalAccountOwner owner)
        {
            owner = null;
            var contragentKey = $"{row.Inn}#{row.Kpp}";

            var matchOwner = this.legalOwnerMatchSet.FirstOrDefault(x => x.Inn == row.Inn && x.Kpp == row.Kpp && x.Name == row.RenterName);
            if (matchOwner.IsNotNull())
            {
                owner = this.legalAccountOwnersById[matchOwner.OwnerId];
            }
            else if (this.contragentsDict.ContainsKey(contragentKey))
            {
                var contragents = this.contragentsDict[contragentKey];
                if (contragents.Length > 1)
                {
                    accountFileInfo.AddErrorRow(
                        row,
                        $"Найдено несколько контрагентов с подходящим ИНН({row.Inn}) и КПП({row.Kpp}). Строка {row.RowNumber}",
                        row.LsNum,
                        row.ExtLsNum);
                }
                var contragent = contragents.First();
                if (this.legalAccountOwnersByContragent.ContainsKey(contragent.Id) && this.legalAccountOwnersByContragent[contragent.Id].Length == 1)
                {
                    owner = this.legalAccountOwnersByContragent[contragent.Id].First();
                }
                else
                {
                    owner = this.CreateLegalOwner(row, contragent);
                }
            }

            if (owner.IsNull())
            {
                accountFileInfo.AddErrorRow(row, $"Контрагент с подходящим ИНН({row.Inn}) и КПП({row.Kpp}) не найден. Строка {row.RowNumber}", row.LsNum, row.ExtLsNum);
            }

            return owner.IsNotNull();
        }

        private IndividualAccountOwner CreateIndividualOwner(AccountFileInfo.Row row)
        {
            var owner = new IndividualAccountOwner
            {
                Surname = row.Surname,
                FirstName = row.Name,
                SecondName = row.Lastname,
                BirthDate = row.BirthDate.GetValueOrDefault()
            };
            this.individualOwnersForSave.Add(owner);
            this.LogImport.CountAddedRows++;
            this.LogImport.Info(this.ImportName,
                $"Строка {row.RowNumber}: создан абонент с типом физическое лицо;{row.LsNum};{row.ExtLsNum}");

            return owner;
        }

        private LegalAccountOwner CreateLegalOwner(AccountFileInfo.Row row, Contragent contragent)
        {
            var owner = new LegalAccountOwner
            {
                OwnerType = PersonalAccountOwnerType.Legal,
                Contragent = contragent
            };
            owner.UpdateOwnerName(contragent.Name);

            this.legalOwnersForSave.Add(owner);
            this.LogImport.CountAddedRows++;
            this.LogImport.Info(this.ImportName,
                $"Строка {row.RowNumber}: создан абонент с типом юридическое лицо;{row.LsNum};{row.ExtLsNum}");

            return owner;
        }

        private Room GetOrCreateRoom(AccountFileInfo.Row row, long roId)
        {
            var roomKey = $"{roId}#{row.RoomNum}#{row.ChamberNum}";

            if (!this.roomAreaSharesDict.ContainsKey(roomKey))
            {
                this.roomAreaSharesDict[roomKey] = new Dictionary<string, decimal>();
            }

            if (this.rooms.ContainsKey(roomKey))
            {
                var importedRoom = this.portionRoomDict.Get(roomKey);
                return importedRoom;
            }
            if (this.createdRooms.ContainsKey(roomKey))
            {
                return this.createdRooms[roomKey];
            }

            return this.CreateRoom(row, roId);
        }

        private Room CreateRoom(AccountFileInfo.Row row, long roId)
        {
            var newRoom = new Room
            {
                RoomNum = row.RoomNum,
                ChamberNum = row.ChamberNum,
                Area = row.TotalArea,
                Type = row.FlatPlaceType,
                OwnershipType = row.PropertyType,
                RealityObject = new RealityObject { Id = roId }
            };

            this.roomsForSave.Add(newRoom);
            this.LogImport.CountAddedRows++;
            this.LogImport.Info(this.ImportName,
                $"Строка {row.RowNumber}: создано помещение;{row.LsNum};{row.ExtLsNum}");

            this.AddRoomNumLog(row, newRoom);
            this.AddChamberNumLog(row, newRoom);
            this.AddRoomAreaLog(row, newRoom);
            this.AddRoomOwnershipTypeLog(row, newRoom);

            this.createdRooms.Add($"{roId}#{row.RoomNum}#{row.ChamberNum}", newRoom);
            return newRoom;
        }

        private void ChangeOwner(BasePersonalAccount account, IndividualAccountOwner newOwner, AccountFileInfo.Row row, ChargePeriod period)
        {
            var ownerHistory = this.ownershipHistory.Get(row.LsNum) ?? new OwnershipHistoryItem[0];
            var lastOwnerDate = ownerHistory.LastOrDefault()?.ActualFrom;
            var oldOwnerByDate = ownerHistory.All(x => x.OwnerId != newOwner.Id);

            var oldOwnerName = ownershipHistoryDict.Get(account.Id);

            var shouldChangeOnwer = !lastOwnerDate.HasValue || row.BillTypeDate > lastOwnerDate || oldOwnerByDate;
            if (shouldChangeOnwer)
            {
                this.AddAccountChangeOwnerLog(
                    row,
                    account,
                    newOwner,
                    oldOwnerName,
                    $"{row.Surname} {row.Name} {row.Lastname}",
                    period);
            }

            if (shouldChangeOnwer)
            {
                newOwner.TotalAccountsCount++;
                account.AccountOwner = newOwner;
                row.HasChangeAccount = true;
            }

        }

        private void ChangeOwner(AccountFileInfo.Row row, BasePersonalAccount account, LegalAccountOwner newOwner, ChargePeriod period)
        {
            var ownerHistory = this.ownershipHistory.Get(row.LsNum) ?? new OwnershipHistoryItem[0];
            var lastOwnerDate = ownerHistory.LastOrDefault()?.ActualFrom;
            var oldOwnerByDate = ownerHistory.All(x => x.OwnerId != newOwner.Id);

            var shouldChangeOnwer = !lastOwnerDate.HasValue || row.BillTypeDate > lastOwnerDate || oldOwnerByDate;
            if (shouldChangeOnwer)
            {
                this.AddAccountChangeOwnerLog(row, account, newOwner, oldOwnerByDate ? null : account.AccountOwner.Name, newOwner.Name, period);
            }

            if (shouldChangeOnwer)
            {
                newOwner.TotalAccountsCount++;
                account.AccountOwner = newOwner;
                row.HasChangeAccount = true;
            }
        }

        private bool CheckAreaShare(BasePersonalAccount account, AccountFileInfo.Row row, AccountFileInfo accountFileInfo)
        {
            var areaShareDelta = row.Share - account.AreaShare;
            if (!areaShareDelta.HasValue)
            {
                return true;
            }

            var roomKey = $"{account.Room.RealityObject.Id}#{account.Room.RoomNum}#{account.Room.ChamberNum}";
            if (this.roomAreaSharesDict[roomKey].Values.Sum() + areaShareDelta > 1)
            {
                accountFileInfo.AddErrorRow(row, $"Невозможно поменять Долю собственности: Доля собственности в сумме превышает 1. Строка {row.RowNumber}", row.LsNum, row.ExtLsNum);
                return false;
            }

            return true;
        }

        private void AddAccountOpenDateLog(BasePersonalAccount account, AccountFileInfo.Row row)
        {
            this.entityLogsForSave.Add(() =>
                new EntityLogLight
                {
                    ClassName = "BasePersonalAccount",
                    EntityId = account.Id,
                    PropertyName = "OpenDate",
                    PropertyValue = row.LsOpenDate.ToString(),
                    DateActualChange = row.LsOpenStart.Value,
                    DateApplied = DateTime.Now,
                    ParameterName = "account_open_date",
                    User = this.Login,
                    Reason = this.ParentImportName
                });
        }

        private void AddAccountAreaShareLog(AccountFileInfo.Row row, BasePersonalAccount account)
        {
            this.entityLogsForSave.Add(() =>
                new EntityLogLight
                {
                    ClassName = "BasePersonalAccount",
                    EntityId = account.Id,
                    PropertyName = "AreaShare",
                    PropertyValue = row.Share.ToString(),
                    DateActualChange = row.ShareDate.Value,
                    DateApplied = DateTime.Now,
                    ParameterName = "area_share",
                    User = this.Login,
                    Reason = this.ParentImportName
                });
        }

        private void AddRoomOwnershipTypeLog(AccountFileInfo.Row row, Room room)
        {
            this.entityLogsForSave.Add(() =>
                new EntityLogLight
                {
                    ClassName = "Room",
                    EntityId = room.Id,
                    PropertyName = "OwnershipType",
                    PropertyValue = row.PropertyType.ToString(),
                    DateActualChange = row.PropertyTypeDate,
                    DateApplied = DateTime.Now,
                    ParameterName = "room_ownership_type",
                    User = this.Login,
                    Reason = this.ParentImportName
                });
        }

        private void AddRoomAreaLog(AccountFileInfo.Row row, Room room)
        {
            this.entityLogsForSave.Add(() =>
                new EntityLogLight
                {
                    ClassName = "Room",
                    EntityId = room.Id,
                    PropertyName = "Area",
                    PropertyValue = row.TotalArea.ToString(),
                    DateActualChange = row.TotalAreaDate,
                    DateApplied = DateTime.Now,
                    ParameterName = "room_area",
                    User = this.Login,
                    Reason = this.ParentImportName
                });
        }

        private void AddRoomNumLog(AccountFileInfo.Row row, Room room)
        {
            this.entityLogsForSave.Add(() =>
                new EntityLogLight
                {
                    ClassName = "Room",
                    EntityId = room.Id,
                    PropertyName = "RoomNum",
                    PropertyValue = row.RoomNum,
                    DateActualChange = row.RoomNumDate,
                    DateApplied = DateTime.Now,
                    ParameterName = "room_num",
                    User = this.Login,
                    Reason = this.ParentImportName
                });
        }

        private void AddChamberNumLog(AccountFileInfo.Row row, Room room)
        {
            this.entityLogsForSave.Add(() =>
                new EntityLogLight
                {
                    ClassName = "Room",
                    EntityId = room.Id,
                    PropertyName = "ChamberNum",
                    PropertyValue = row.ChamberNum,
                    DateActualChange = row.RoomNumDate,
                    DateApplied = DateTime.Now,
                    ParameterName = "room_chamber_num",
                    User = this.Login,
                    Reason = this.ParentImportName
                });
        }

        private void AddExternalNumLog(AccountFileInfo.Row row, BasePersonalAccount account)
        {
            this.entityLogsForSave.Add(() =>
                new EntityLogLight
                {
                    ClassName = "BasePersonalAccount",
                    EntityId = account.Id,
                    PropertyName = "PersAccNumExternalSystems",
                    PropertyValue = row.ExtLsNum,
                    DateActualChange = DateTime.Now,
                    DateApplied = DateTime.Now,
                    ParameterName = VersionedParameters.PersonalAccountExternalNum,
                    User = this.Login,
                    Reason = this.ParentImportName
                });
        }

        private void AddAccountChangeOwnerLog(AccountFileInfo.Row row, BasePersonalAccount account, PersonalAccountOwner owner, string oldOwnerName, string newOwnerName, ChargePeriod period)
        {
            var actualFrom = row.BillTypeDate ?? DateTime.Now;
            this.accountChangesForSave.Add(() =>
                new PersonalAccountChange(
                    account,
                    $"Смена абонента ЛС с \"{oldOwnerName}\" на \"{newOwnerName}\"",
                    PersonalAccountChangeType.OwnerChange,
                    DateTime.UtcNow,
                    actualFrom,
                    this.Login,
                    null,
                    account.AccountOwner.Id.ToString(),
                    period)
                {
                    Reason = this.ParentImportName
                });

            this.ownershipHistoryForSave.Add(() => new AccountOwnershipHistory(account, owner, actualFrom));
        }

        private void AddACloseChange(BasePersonalAccount account, AccountFileInfo.Row row, ChargePeriod period)
        {
            this.accountChangesForSave.Add(() =>
                new PersonalAccountChange(
                account,
                null,
                PersonalAccountChangeType.Close,
                DateTime.Now,
                row.LsCloseStart,
                this.Login,
                null,
                account.AccountOwner?.Id.ToStr(),
                    period)
                {
                    Reason = this.ParentImportName
                });
        }

        private class LegalOwnerProxy : IEquatable<LegalOwnerProxy>
        {
            public string Name { get; set; }
            public string Inn { get; set; }
            public string Kpp { get; set; }
            public long OwnerId { get; set; }

            /// <inheritdoc />
            public bool Equals(LegalOwnerProxy other)
            {
                if (object.ReferenceEquals(null, other))
                {
                    return false;
                }
                if (object.ReferenceEquals(this, other))
                {
                    return true;
                }

                return string.Equals(this.Name, other.Name) && string.Equals(this.Inn, other.Inn) && string.Equals(this.Kpp, other.Kpp);
            }

            /// <inheritdoc />
            public override bool Equals(object obj)
            {
                if (object.ReferenceEquals(null, obj))
                {
                    return false;
                }

                if (object.ReferenceEquals(this, obj))
                {
                    return true;
                }

                if (obj.GetType() != this.GetType())
                {
                    return false;
                }

                return this.Equals((LegalOwnerProxy)obj);
            }

            /// <inheritdoc />
            public override int GetHashCode()
            {
                unchecked
                {
                    var hashCode = (this.Name != null ? this.Name.GetHashCode() : 0);
                    hashCode = (hashCode * 397) ^ (this.Inn != null ? this.Inn.GetHashCode() : 0);
                    hashCode = (hashCode * 397) ^ (this.Kpp != null ? this.Kpp.GetHashCode() : 0);
                    return hashCode;
                }
            }
        }

        private class OwnershipHistoryItem
        {
            public long AccountId { get; set; }
            public long OwnerId { get; set; }
            public DateTime? ActualFrom { get; set; }
        }

        private class RealityObjectTypeCRCondition
        {
            public ConditionHouse ConditionHouse { get; set; }
            public CrFundFormationType CrFundFormationType { get; set; }
        }
    }
}