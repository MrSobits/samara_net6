namespace Bars.Gkh.RegOperator.Imports
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.Security;
    using Bars.B4.Modules.Tasks.Common.Entities;
    using Bars.B4.Utils;
    using Bars.Gkh.ConfigSections.RegOperator;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Import;
    using Bars.Gkh.Import.Impl;
    using Bars.Gkh.RegOperator.Domain.ImportExport;
    using Bars.Gkh.RegOperator.Domain.ImportExport.ImportMaps;
    using Bars.Gkh.RegOperator.Domain.ImportExport.Mapping;
    using Bars.Gkh.RegOperator.Domain.ProxyEntity;
    using Bars.Gkh.RegOperator.DomainService.PersonalAccount;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Entities.Import;
    using Bars.Gkh.RegOperator.Entities.ValueObjects;
    using Bars.Gkh.RegOperator.Entities.ValueObjects;
    using Bars.Gkh.RegOperator.Enums;
    using Bars.Gkh.RegOperator.Utils;
    using Bars.Gkh.Utils;

    using NHibernate.Linq;
    using NHibernate.Util;

    /// <summary>
    /// Импорт начислений в закрытый период
    /// </summary>
    public class ChargesToClosedPeriodsImport : GkhImportBase
    {
        private ChargePeriod period;

        private Dictionary<long, PersonalAccountPeriodSummary> summariesByAccount;
        private Dictionary<long, PersonalAccountCharge> chargiesByAccount;
        private Dictionary<long, RealityObjectChargeAccountOperation> summariesByRo;
        private Dictionary<long, RealityObjectChargeAccount> roChargeAccounts;
        private Dictionary<long, Contragent> contragents;

        private Dictionary<string, long> accountsByInnerNumber;
        private Dictionary<string, long[]> accountsByExternalNumber;

        private Dictionary<string, HashSet<long>> accountsByCashCenterNumber;
        private Dictionary<long, BasePersonalAccount> accountsById;

        private long[] roIds;
        private long[] accountIds;

        private IEnumerable<IChargeDescriptor> chargeDescriptors;

        // Реквизиты лицевых счетов - ключ: Адрес#ФИО, значение: реквизиты. 
        // Используется для поиска ЛС, которые не были найдены по внешнему номеру и коду РЦ.        
        private Dictionary<string, accountRequisitesProxy> accountRequisites;

        private readonly List<WarningInClosedPeriodsImport> warnings = new List<WarningInClosedPeriodsImport>();

        private readonly List<EntityLogLight> entityLogLightForSave = new List<EntityLogLight>();

        /// <summary>
        /// Предупреждение про ЛС при импорте в закрытый период
        /// </summary>
        public IDomainService<AccountWarningInClosedPeriodsImport> AccountWarningInClosedPeriodsImportDomain { get; set; }

        /// <summary>
        /// Лицевой счёт
        /// </summary>
        public IDomainService<BasePersonalAccount> BasePersonalAccountDomain { get; set; }

        /// <summary>
        /// Шапка импорта начислений в закрытый период
        /// </summary>
        public IDomainService<HeaderOfChargesToClosedPeriodsImport> HeaderOfChargesToClosedPeriodsImportDomain { get; set; }

        /// <summary>
        /// Предупреждение про существование начислений при импорте начислений в закрытый период
        /// </summary>
        public IDomainService<ExistWarningInChargesToClosedPeriodsImport> ExistWarningInChargesToClosedPeriodsImportDomain { get; set; }

        /// <summary>
        /// Изменение сущностей
        /// </summary>
        public IDomainService<EntityLogLight> EntityLogLightDomain { get; set; }

        /// <summary>
        /// Сервис абонентов
        /// </summary>
        public IPersonalAccountOwnerService PersonalAccountOwnerService { get; set; }

        /// <summary>
        /// Ключ импорта
        /// </summary>
        public override string Key
        {
            get { return this.GetType().FullName; }
        }

        /// <summary>
        /// Код группировки импорта (например группировка в меню)
        /// </summary>
        public override string CodeImport
        {
            get { return "ChargesToClosedPeriodImport"; }
        }

        /// <summary>
        /// Наименование импорта
        /// </summary>
        public override string Name
        {
            get { return "Импорт начислений (в закрытые периоды)"; }
        }

        /// <summary>
        /// Разрешенные расширения файлов
        /// </summary>
        public override string PossibleFileExtensions
        {
            get { return "dbf"; }
        }

        /// <summary>
        /// Права
        /// </summary>
        public override string PermissionName
        {
            get { return "Import.ChargesToClosedPeriods.View"; }
        }

        /// <summary>
        /// Зависимости от других импортов
        /// </summary>
        public override string[] Dependencies => new[] {"Bars.Gkh.RegOperator.Imports.PaymentsToClosedPeriodsImport"};

        private IEnumerable<IChargeDescriptor> ChargeDescriptors
        {
            get
            {
                if (this.chargeDescriptors == null)
                {
                    this.chargeDescriptors = new List<IChargeDescriptor>()
                    {
                        new ChargeByTariffDescriptor(),
                        new RecalcChargeDescriptor(),
                        new PenaltyChargeDescriptor()
                    };
                }

                return this.chargeDescriptors;
            }
        }

        /// <summary>
        /// Метод импорта
        /// </summary>
        protected override ImportResult ImportUsingGkhApi(BaseParams @params)
        {
            var ownerForSave = new List<PersonalAccountOwner>();
            var accountForSave = new List<BasePersonalAccount>();
            var contragentForSave = new List<Contragent>();

            var provider = this.Container.Resolve<ImportExportDataProvider>();

            var map = new PersonalAccountSberProxyMap();

            var periodId = @params.Params.GetAsId("periodId");
            var withoutSaldo = @params.Params.ContainsKey("withoutSaldo") && @params.Params["withoutSaldo"].ToBool();

            this.period = this.Container.ResolveDomain<ChargePeriod>().Get(periodId);

            if (this.period == null)
            {
                throw new ArgumentException("Не удалось получить период");
            }

            this.Indicate(1, "Чтение данных");

            var data = provider.Deserialize<PersonalAccountInfoProxy>(
                @params.Files.First().Value,
                DynamicDictionary.Create(),
                map.GetKey());

            var rows = data.Rows
                .Select(x => x.Value)
                .ToArray();

            //данный костыль требуется потому что в файле тип колонки numeric
            //и в результате считывания получается 123,00
            foreach (var row in rows)
            {
                if (!row.RkcId.IsEmpty())
                {
                    if (row.RkcId.Contains(','))
                    {
                        row.RkcId = row.RkcId.Substring(0, row.RkcId.IndexOf(','));
                    }

                    if (row.RkcId.Contains('.'))
                    {
                        row.RkcId = row.RkcId.Substring(0, row.RkcId.IndexOf('.'));
                    }
                }
            }

            this.Indicate(5, "Инициализация кеша");

            this.InitCache(rows);

            this.Indicate(10, "Обработка данных");

            var userRepo = this.Container.ResolveDomain<User>();
            var userIdentity = this.Container.Resolve<IUserIdentity>();
            var basePersonalAccountDomain = this.Container.ResolveDomain<BasePersonalAccount>();
            var indivAccountOwnerService = this.Container.ResolveDomain<IndividualAccountOwner>();
            var legalAccountOwnerService = this.Container.ResolveDomain<LegalAccountOwner>();
            var personalAccountOwnerService = this.Container.ResolveDomain<PersonalAccountOwner>();

            try
            {
                var login = userRepo.Get(userIdentity.UserId).Return(u => u.Login);
                if (login.IsEmpty())
                {
                    login = "admin";
                }
                var dictRoomOwnershipType = new Dictionary<string, RoomOwnershipType>()
                {
                    {"1", RoomOwnershipType.Private},
                    {"2", RoomOwnershipType.Municipal},
                    {"3", RoomOwnershipType.Goverenment},
                    {"4", RoomOwnershipType.Commerse}
                };
                var dictRoomType = new Dictionary<string, RoomType>()
                {
                    {"1", RoomType.Living},
                    {"0", RoomType.NonLiving}
                };

                var defaultDateTime = default(DateTime);
                var defaultDecimal = default(decimal);

                var entitiesForSave = new List<BaseEntity>();

                foreach (var row in rows)
                {
                    if (row.ChargeTotal == 0 || row.SettlementPeriodRecalc == 0 || row.Penalty == 0)
                    {
                        this.LogExistWarning(row, "Сумма не корректна, так как равна нулю");
                    }
                    var account = this.GetAccount(row);
                    var needUpdateAccount = false;

                    if (account == null)
                    {
                        continue;
                    }

                    if (row.OpenDate != defaultDateTime && row.OpenDate != account.OpenDate)
                    {
                        account.OpenDate = row.OpenDate;
                        needUpdateAccount = true;
                        this.AddLog(
                            "BasePersonalAccount",
                            account.Id,
                            "OpenDate",
                            account.OpenDate.ToString(),
                            DateTime.UtcNow,
                            "account_open_date",
                            "Изменение даты открытия л.с.",
                            login);
                    }
                    if (row.AreaShare != defaultDecimal && row.AreaShare != account.AreaShare && row.AreaShareChangedDate != defaultDateTime)
                    {
                        account.AreaShare = row.AreaShare;
                        needUpdateAccount = true;
                        this.AddLog(
                            "BasePersonalAccount",
                            account.Id,
                            "AreaShare",
                            account.AreaShare.ToString(),
                            row.AreaShareChangedDate,
                            "area_share",
                            "Изменение доли собственности",
                            login);
                    }
                    if (row.RoomType != null)
                    {
                        RoomType roomType;
                        if (dictRoomType.TryGetValue(row.RoomType, out roomType) && roomType != account.Room.Type)
                        {
                            account.Room.Type = roomType;
                            needUpdateAccount = true;
                        }
                    }
                    if (row.TotalArea != defaultDecimal && row.TotalArea != account.Room.Area && row.TotalAreaChangedDate != defaultDateTime)
                    {
                        account.Room.Area = row.TotalArea;
                        needUpdateAccount = true;
                        this.AddLog(
                            "Room",
                            account.Room.Id,
                            "Area",
                            account.Room.Area.ToString(),
                            row.TotalAreaChangedDate,
                            "room_area",
                            "Изменение общей площади",
                            login);
                    }
                    if (row.LivingArea != defaultDecimal && row.LivingArea != account.Room.LivingArea && row.LivingAreaChangedDate != defaultDateTime)
                    {
                        account.Room.LivingArea = row.LivingArea;
                        needUpdateAccount = true;
                    }
                    if (row.OwnershipType != null)
                    {
                        RoomOwnershipType ownershipType;
                        if (dictRoomOwnershipType.TryGetValue(row.OwnershipType, out ownershipType) && ownershipType != account.Room.OwnershipType)
                        {
                            account.Room.OwnershipType = ownershipType;
                            needUpdateAccount = true;
                            this.AddLog(
                                "Room",
                                account.Room.Id,
                                "OwnershipType",
                                account.Room.OwnershipType.ToString(),
                                DateTime.UtcNow,
                                "room_ownership_type",
                                "Изменение типа собственности",
                                login);
                        }
                    }

                    PersonalAccountOwner accountOwner = null;

                    if (row.OwnerType == "1")
                    {
                        var indivAccount = account.AccountOwner as IndividualAccountOwner;

                        if (indivAccount.IsNull())
                        {
                            var logText = new StringBuilder();
                            if (row.Surname.IsEmpty())
                            {
                                logText.Append("фамилия собственника, ");
                            }
                            if (row.Name.IsEmpty())
                            {
                                logText.Append("имя собственника, ");
                            }
                            if (row.SecondName.IsEmpty())
                            {
                                logText.Append("отчество собственника ");
                            }

                            if (logText.Length != 0)
                            {
                                this.LogExistWarning(row, $"Не заполнено поле {logText.ToString()}");
                                continue;
                            }

                            indivAccount = new IndividualAccountOwner
                            {
                                OwnerType = PersonalAccountOwnerType.Individual,
                                TotalAccountsCount = account.AccountOwner.TotalAccountsCount,
                                ActiveAccountsCount = account.AccountOwner.ActiveAccountsCount,
                                BillingAddressType = account.AccountOwner.BillingAddressType,
                                PrivilegedCategory = account.AccountOwner.PrivilegedCategory,
                                Surname = row.Surname,
                                FirstName = row.Name,
                                SecondName = row.SecondName,
                            };
                        }
                        else
                        {
                            if (row.Surname.IsNotEmpty() && row.Surname != indivAccount.Surname)
                            {
                                indivAccount.Surname = row.Surname;
                                this.AddLog(
                                    "IndividualAccountOwner",
                                    indivAccount.Id,
                                    "Surname",
                                    indivAccount.Surname,
                                    DateTime.UtcNow,
                                    "surname",
                                    "Изменение фамилии собственника",
                                    login);
                            }
                            if (row.Name.IsNotEmpty() && row.Name != indivAccount.FirstName)
                            {
                                indivAccount.FirstName = row.Name;
                                this.AddLog(
                                    "IndividualAccountOwner",
                                    indivAccount.Id,
                                    "FirstName",
                                    indivAccount.FirstName,
                                    DateTime.UtcNow,
                                    "first_name",
                                    "Изменение имени собственника",
                                    login);
                            }
                            if (row.SecondName.IsNotEmpty() && row.SecondName != indivAccount.SecondName)
                            {
                                indivAccount.SecondName = row.SecondName;
                                this.AddLog(
                                    "IndividualAccountOwner",
                                    indivAccount.Id,
                                    "SecondName",
                                    indivAccount.SecondName,
                                    DateTime.UtcNow,
                                    "second_name",
                                    "Изменение отчества собственника",
                                    login);
                            }
                        }

                        accountOwner = indivAccount;
                        needUpdateAccount = true;
                    }
                    else if (row.OwnerType == "2")
                    {
                        var legalAccount = account.AccountOwner as LegalAccountOwner;

                        if (legalAccount.IsNull())
                        {
                            legalAccount = new LegalAccountOwner
                            {
                                OwnerType = PersonalAccountOwnerType.Legal,
                                TotalAccountsCount = account.AccountOwner.TotalAccountsCount,
                                ActiveAccountsCount = account.AccountOwner.ActiveAccountsCount,
                                BillingAddressType = account.AccountOwner.BillingAddressType,
                                PrivilegedCategory = account.AccountOwner.PrivilegedCategory
                            };

                            var contragent = this.contragents.Values.FirstOrDefault(x => x.Inn == row.InnLegal && x.Kpp == row.KppLegal);

                            if (contragent == null)
                            {
                                contragent = new Contragent {Name = row.NameLegal, Inn = row.InnLegal, Kpp = row.KppLegal};
                                contragentForSave.Add(contragent);
                            }
                            else if (row.NameLegal.IsNotEmpty() && row.NameLegal != contragent.Name)
                            {
                                contragent.Name = row.NameLegal;
                                this.AddLog(
                                    "Contragent",
                                    contragent.Id,
                                    "Name",
                                    contragent.Name,
                                    DateTime.UtcNow,
                                    "name",
                                    "Изменение наименования юр. лица",
                                    login);
                            }
                            else
                            {
                                this.LogInfo("", "Контрагент уже существует");
                            }

                            legalAccount.Contragent = contragent;
                        }
                        else
                        {
                            var contragent = legalAccount.Contragent;

                            if (row.InnLegal.IsNotEmpty() && row.InnLegal != contragent.Inn)
                            {
                                contragent.Inn = row.InnLegal;
                                this.AddLog(
                                    "Contragent",
                                    contragent.Id,
                                    "Inn",
                                    contragent.Inn,
                                    DateTime.UtcNow,
                                    "inn",
                                    "Изменение ИНН юр. лица",
                                    login);
                            }
                            if (row.KppLegal.IsNotEmpty() && row.KppLegal != contragent.Kpp)
                            {
                                contragent.Kpp = row.KppLegal;
                                this.AddLog(
                                    "Contragent",
                                    contragent.Id,
                                    "Kpp",
                                    contragent.Kpp,
                                    DateTime.UtcNow,
                                    "kpp",
                                    "Изменение КПП юр. лица",
                                    login);
                            }
                            if (row.NameLegal.IsNotEmpty() && row.NameLegal != contragent.Name)
                            {
                                contragent.Name = row.NameLegal;
                                this.AddLog(
                                    "Contragent",
                                    contragent.Id,
                                    "Name",
                                    contragent.Name,
                                    DateTime.UtcNow,
                                    "name",
                                    "Изменение наименования юр. лица",
                                    login);
                            }
                        }

                        accountOwner = legalAccount;
                        needUpdateAccount = true;
                    }

                    if (needUpdateAccount)
                    {
                        accountForSave.Add(account);
                    }

                    if (accountOwner != null)
                    {
                        if (this.PersonalAccountOwnerService.OnUpdateOwner(accountOwner))
                        {
                            ownerForSave.Add(accountOwner);
                        }
                    }

                    this.ProcessCharges(account, row, entitiesForSave, !withoutSaldo);

                    this.LogImport.CountAddedRows++;
                }
                if (!this.IsCancelled())
                {
                    this.Indicate(70, "Сохранение данных");
                }

                TransactionHelper.InsertInManyTransactions(this.Container, contragentForSave, 1000, false, true);
                TransactionHelper.InsertInManyTransactions(this.Container, ownerForSave, 1000, false, true);
                TransactionHelper.InsertInManyTransactions(this.Container, accountForSave, 1000);
                TransactionHelper.InsertInManyTransactions(this.Container, entitiesForSave, 10000, false, true);
                TransactionHelper.InsertInManyTransactions(this.Container, this.entityLogLightForSave, 1000, false, true);

                if (this.IsCancelled())
                {
                    this.LogError("Отмена", "Импорт отменен");
                }

                if (!this.IsCancelled())
                {
                    this.Indicate(90, "Обновление данных");

                    this.UpdateRobjectSummaries();
                }

                // Записать параметры импорта в "шапку". Для возможности повторного запуска.
                var header = new HeaderOfChargesToClosedPeriodsImport
                {
                    Task = this.TaskEntryDomain.Load(this.TaskId),
                    WithoutSaldo = withoutSaldo,
                    Period = this.period
                };
                this.HeaderOfChargesToClosedPeriodsImportDomain.Save(header);

                // Записать предупреждения
                TransactionHelper.InsertInManyTransactions(this.Container, this.warnings, this.warnings.Count, false, true);

                this.Container.Resolve<ISessionProvider>().GetCurrentSession().Clear();

                return new ImportResult();
            }
            finally
            {
                this.Container.Release(userRepo);
                this.Container.Release(userIdentity);
                this.Container.Release(basePersonalAccountDomain);
                this.Container.Release(indivAccountOwnerService);
                this.Container.Release(legalAccountOwnerService);
                this.Container.Release(personalAccountOwnerService);
            }
        }

        private void InitCache(PersonalAccountInfoProxy[] rows)
        {
            var accountNumbers = rows
                .Select(x => x.AccountNumber)
                .Where(x => !x.IsEmpty())
                .Distinct()
                .ToArray();

            var accountNumberExternal = rows
                .Select(x => x.PersAccNumExternalSystems)
                .Where(x => !x.IsEmpty())
                .Distinct()
                .ToArray();

            var accounts = new List<BasePersonalAccount>();
            var tmpRoIds = new List<long>();

            this.summariesByAccount = new Dictionary<long, PersonalAccountPeriodSummary>();
            this.chargiesByAccount = new Dictionary<long, PersonalAccountCharge>();
            this.summariesByRo = new Dictionary<long, RealityObjectChargeAccountOperation>();
            this.roChargeAccounts = new Dictionary<long, RealityObjectChargeAccount>();
            this.contragents = new Dictionary<long, Contragent>();

            var length = Math.Max(accountNumbers.Length, accountNumberExternal.Length);

            var accountDomain = this.Container.ResolveDomain<BasePersonalAccount>();
            var summaryDomain = this.Container.ResolveDomain<PersonalAccountPeriodSummary>();
            var accountChargeDomain = this.Container.ResolveDomain<PersonalAccountCharge>();
            var roAccountOperationDomain = this.Container.ResolveDomain<RealityObjectChargeAccountOperation>();
            var roChargeAccountDomain = this.Container.ResolveDomain<RealityObjectChargeAccount>();
            var contragentDomain = this.Container.ResolveDomain<Contragent>();

            for (int i = 0; i <= length; i += 1000)
            {
                var numbers = accountNumbers
                    .Skip(i)
                    .Take(1000)
                    .ToArray();

                var extNumbers = accountNumberExternal
                    .Skip(i)
                    .Take(1000)
                    .ToArray();

                var query = accountDomain.GetAll()
                    .Where(x => numbers.Contains(x.PersonalAccountNum) || extNumbers.Contains(x.PersAccNumExternalSystems));

                var accs = query
                    .Fetch(x => x.BaseTariffWallet)
                    .Fetch(x => x.PenaltyWallet)
                    .ToArray();

                accounts.AddRange(accs);
                tmpRoIds.AddRange(query.Select(x => x.Room.RealityObject.Id));

                var accIds = accs.Select(x => x.Id).ToArray();

                var summaries = summaryDomain.GetAll()
                    .Where(x => x.Period.Id == this.period.Id)
                    .Where(x => accIds.Contains(x.PersonalAccount.Id))
                    .ToArray();

                foreach (var summary in summaries)
                {
                    this.summariesByAccount[summary.PersonalAccount.Id] = summary;
                }

                this.chargiesByAccount.AddOrOverride(
                    accountChargeDomain.GetAll()
                        .Where(x => accIds.Contains(x.BasePersonalAccount.Id))
                        .Where(x => x.ChargePeriod.Id == this.period.Id && x.IsActive)

                        .OrderBy(x => x.ChargeDate)
                        .GroupBy(x => x.BasePersonalAccount.Id)
                        .ToDictionary(x => x.Key, x => x.OrderBy(y => y.ChargeDate).FirstOrDefault()));
            }

            var distAccounts = accounts.Distinct().ToList();
            this.accountsById = distAccounts.ToDictionary(x => x.Id);

            this.accountIds = this.accountsById.Keys.ToArray();

            this.roIds = tmpRoIds.Distinct().ToArray();

            this.accountsByInnerNumber = distAccounts
                .Select(
                    x => new
                    {
                        x.Id,
                        x.PersonalAccountNum
                    })
                .ToArray()
                .ToDictionary(x => x.PersonalAccountNum, x => x.Id);

            this.accountsByExternalNumber = distAccounts
                .Select(
                    x => new
                    {
                        x.Id,
                        x.PersAccNumExternalSystems
                    })
                .ToArray()
                .Where(x => !x.PersAccNumExternalSystems.IsEmpty())
                .GroupBy(x => x.PersAccNumExternalSystems)
                .ToDictionary(x => x.Key, x => x.Select(z => z.Id).ToArray());

            var cashCenterNumbers = rows
                .Select(x => x.RkcId)
                .Where(x => !x.IsEmpty())
                .Distinct()
                .ToArray();

            var cachPaymentCenterConnectionType = this.Container.GetGkhConfig<RegOperatorConfig>().GeneralConfig.CachPaymentCenterConnectionType;
            CashPaymentCenterPersAccProxy[] cashCentersAndAccounts;

            var periodEndDate = this.period.EndDate ?? this.period.StartDate.AddMonths(1).AddDays(-1);

                // Выбирать на дату конца (периода). Если вдруг импорт делается по текущему периоду, то конец это последний день месяца.
            if (cachPaymentCenterConnectionType == CachPaymentCenterConnectionType.ByAccount)
            {
                cashCentersAndAccounts = this.Container.ResolveDomain<CashPaymentCenterPersAcc>().GetAll()
                    .Where(x => cashCenterNumbers.Contains(x.CashPaymentCenter.Identifier))
                    .Where(x => this.accountIds.Contains(x.PersonalAccount.Id))
                    .Where(x => x.DateStart <= periodEndDate)
                    .Where(x => !x.DateEnd.HasValue || x.DateEnd.Value >= periodEndDate)
                    .Select(
                        x => new CashPaymentCenterPersAccProxy
                        {
                            Id = x.CashPaymentCenter.Id,
                            Identifier = x.CashPaymentCenter.Identifier,
                            PersonalAccountId = x.PersonalAccount.Id
                        })
                    .ToArray();
            }
            else
            {
                var accountRoIds = distAccounts.ToDictionary(x => x.Id, y => y.Room.RealityObject.Id);

                var cashCentersByRo = this.Container.ResolveDomain<CashPaymentCenterRealObj>().GetAll()
                    .Where(x => cashCenterNumbers.Contains(x.CashPaymentCenter.Identifier))
                    .Where(x => accountRoIds.Values.Distinct().Contains(x.RealityObject.Id))
                    .Where(x => x.DateStart <= periodEndDate)
                    .Where(x => !x.DateEnd.HasValue || x.DateEnd.Value >= periodEndDate)
                    .ToDictionary(x => x.RealityObject.Id);

                cashCentersAndAccounts = accountRoIds
                    .Select(
                        x => new CashPaymentCenterPersAccProxy
                        {
                            Id = cashCentersByRo.Get(x.Value).ReturnSafe(y => y.CashPaymentCenter.Id),
                            Identifier = cashCentersByRo.Get(x.Value).ReturnSafe(y => y.CashPaymentCenter.Identifier),
                            PersonalAccountId = x.Key
                        })
                    .ToArray();
            }

            var rowContragents = rows.Select(x => x.InnLegal).Distinct();

            this.contragents = contragentDomain.GetAll()
                .Where(x => rowContragents.Contains(x.Inn))
                .Select(
                    x => new
                    {
                        x.Id,
                        Contragent = x
                    })
                .ToArray()
                .ToDictionary(x => x.Id, x => x.Contragent);

            this.accountsByCashCenterNumber = cashCentersAndAccounts
                .Where(x => !x.Identifier.IsEmpty())
                .GroupBy(x => x.Identifier)
                .ToDictionary(x => x.Key, z => z.Select(x => x.PersonalAccountId).ToHashSet());

            this.roChargeAccounts = roChargeAccountDomain.GetAll()
                .Where(x => this.roIds.Contains(x.RealityObject.Id))
                .Select(
                    x => new
                    {
                        RoId = x.RealityObject.Id,
                        Account = x
                    })
                .ToArray()
                .ToDictionary(x => x.RoId, x => x.Account);

            this.summariesByRo = roAccountOperationDomain.GetAll()
                .Where(x => this.roIds.Contains(x.Account.RealityObject.Id))
                .Where(x => x.Period.Id == this.period.Id)
                .Select(
                    x => new
                    {
                        RoId = x.Account.RealityObject.Id,
                        Summaries = x
                    })
                .ToArray()
                .ToDictionary(x => x.RoId, x => x.Summaries);

            // Для поиска по ФИО не подходит типовая конструкция "(x.AccountOwner as IndividualAccountOwner).Name ?? (x.AccountOwner as LegalAccountOwner).Contragent.Name".
            // Т.к. она даёт AccountOwner.Name. А там вместо ФИО лежит "XXXXX" в случае, если ещё не открывали карточку лицевого счёта.
            // По этому ФИО физ.лица собирается из IndividualAccountOwner.
            this.accountRequisites = this.Container.ResolveDomain<BasePersonalAccount>().GetAll()
                .Select(
                    x => new
                    {
                        x.Id,
                        x.PersonalAccountNum,
                        Address = x.Room.RealityObject.Address + ", кв. " +
                        x.Room.RoomNum +
                        (x.Room.ChamberNum != "" && x.Room.ChamberNum != null ? ", ком. " + x.Room.ChamberNum : string.Empty),
                        Name = (x.AccountOwner as LegalAccountOwner).Contragent.Name ??
                        (x.AccountOwner as IndividualAccountOwner).Surname + " " +
                        (x.AccountOwner as IndividualAccountOwner).FirstName + " " +
                        (x.AccountOwner as IndividualAccountOwner).SecondName
                    })
                .AsEnumerable()
                .Select(
                    x => new
                    {
                        SearchKey = $"{x.Address}#{x.Name}".ToUpper(),
                        x.Id,
                        x.PersonalAccountNum,
                        x.Name,
                        x.Address
                    })
                .GroupBy(x => x.SearchKey)
                .Where(x => x.Count() == 1)
                .ToDictionary(
                    x => x.Key,
                    y =>
                        y.Select(
                                v => new accountRequisitesProxy {Id = v.Id, PersonalAccountNum = v.PersonalAccountNum, Name = v.Name, Address = v.Address})
                            .FirstOrDefault());
        }

        private void AddLog(
            string className,
            long id,
            string propName,
            string propVal,
            DateTime dateApplied,
            string paramName,
            string description,
            string login)
        {
            this.entityLogLightForSave.Add(
                new EntityLogLight
                {
                    ClassName = className,
                    EntityId = id,
                    PropertyName = propName,
                    PropertyValue = propVal,
                    DateActualChange = DateTime.UtcNow,
                    DateApplied = dateApplied,
                    ParameterName = paramName,
                    PropertyDescription = description + " - импорт начислений (в закрытые периоды)",
                    User = login
                });
        }

        private PersonalAccountPeriodSummary GetSummary(BasePersonalAccount account, PersonalAccountInfoProxy proxy)
        {
            var summary = this.summariesByAccount.Get(account.Id);

            if (summary == null)
            {
                this.LogExistWarning(proxy, "Отсутствует информация за период. Добавлена новая запись");
                summary = new PersonalAccountPeriodSummary(account, this.period, 0);
                this.summariesByAccount.Add(account.Id, summary);
            }

            return summary;
        }

        private RealityObjectChargeAccountOperation GetRoSummary(BasePersonalAccount acc)
        {
            var roSummary = this.summariesByRo.Get(acc.Room.RealityObject.Id);

            if (roSummary == null)
            {
                roSummary = new RealityObjectChargeAccountOperation(this.roChargeAccounts.Get(acc.Room.RealityObject.Id), this.period);
                this.summariesByRo.Add(acc.Room.RealityObject.Id, roSummary);
            }

            return roSummary;
        }

        private PersonalAccountCharge GetAccountCharge(BasePersonalAccount account)
        {
            var accountCharge = this.chargiesByAccount.Get(account.Id);

            if (accountCharge == null)
            {
                accountCharge = new PersonalAccountCharge
                {
                    Guid = Guid.NewGuid().ToString(),
                    BasePersonalAccount = account,
                    ChargeDate = this.period.StartDate.AddDays(1),
                    IsFixed = true,
                    ChargePeriod = this.period
                };
            }

            return accountCharge;
        }

        private BasePersonalAccount GetAccount(PersonalAccountInfoProxy proxy)
        {
            if (proxy.AccountNumber.IsNotEmpty())
            {
                var id = this.accountsByInnerNumber.Get(proxy.AccountNumber);

                if (id > 0)
                {
                    return this.accountsById.Get(id);
                }
            }

            if (proxy.PersAccNumExternalSystems.IsNotEmpty())
            {
                var possibleAccountIds = this.accountsByExternalNumber.Get(proxy.PersAccNumExternalSystems);

                if (possibleAccountIds.IsNotEmpty())
                {
                    if (proxy.RkcId.IsNotEmpty())
                    {
                        //проверка по идентификатору РКЦ
                        if (this.accountsByCashCenterNumber.ContainsKey(proxy.RkcId))
                        {
                            var accIds = this.accountsByCashCenterNumber.Get(proxy.RkcId);

                            if (accIds.IsNotEmpty())
                            {
                                var tmpAccountIds = possibleAccountIds
                                    .Where(accIds.Contains)
                                    .ToArray();

                                if (tmpAccountIds.Length > 1)
                                {
                                    this.LogAccountWarning(proxy, "В указанном ркц найдено более одного лицевого счета с таким номером");

                                    return null;
                                }

                                if (tmpAccountIds.Length == 0)
                                {
                                    this.LogAccountWarning(proxy, "Не удалось сопоставить лицевой счет в указанном ркц");

                                    return null;
                                }

                                if (tmpAccountIds.Length == 1)
                                {
                                    return this.accountsById.Get(tmpAccountIds[0]);
                                }
                            }
                        }
                    }

                    if (possibleAccountIds.Length == 1)
                    {
                        return this.accountsById.Get(possibleAccountIds[0]);
                    }
                }
            }

            this.LogAccountWarning(proxy, "Не удалось определить лицевой счет");
            return null;
        }

        private void UpdateRobjectSummaries()
        {
            try
            {
                var periods = this.Container.ResolveDomain<ChargePeriod>().GetAll()
                    .Where(x => x.StartDate >= this.period.StartDate) //обновляем только периоды после заданного
                    .ToArray();

                using (var ss = this.Container.Resolve<ISessionProvider>().OpenStatelessSession())
                {
                    var action = new UpdateSaldoSqlAction(ss);

                    const int take = 10000;
                    var amount = (int) Math.Ceiling((double) this.accountIds.Length / take);

                    for (int i = 0; i < amount; i++)
                    {
                        var accountIterIds = this.accountIds
                            .Skip(i * take)
                            .Take(take)
                            .ToArray();
                        action.UpdatePaSaldos(periods, accountIterIds);

                        var percent = ((decimal) i / amount) * 10;
                        this.Indicate((int) percent + 90, "Обновление данных");
                    }
                    action.UpdateRoSaldos(periods, this.roIds);
                }
            }
            catch (Exception)
            {
                this.LogError("Обновление данных", "Во время обновления данных произошла ошибка. Рекомендуется выполнить обновление вручную");
            }
        }

        private void ProcessCharges(
            BasePersonalAccount account,
            PersonalAccountInfoProxy proxy,
            List<BaseEntity> entitiesForSave,
            bool updateSummary)
        {
            var summary = this.GetSummary(account, proxy);
            var roSummary = this.GetRoSummary(account);

            var actualCharges = new List<IChargeDescriptor>();

            foreach (var chargeDesc in this.ChargeDescriptors)
            {
                var result = chargeDesc.GetSummaryAmount(summary);

                if (result > 0.0m || result < 0.0m)
                {
                    this.LogExistWarning(
                        proxy,
                        string.Format("{0} за период '{1}' уже существует", chargeDesc.Name, this.period.Name),
                        chargeDesc.Name);
                }
                else
                {
                    actualCharges.Add(chargeDesc);
                }
            }

            if (actualCharges.Count == 0)
            {
                return;
            }

            var accountCharge = this.GetAccountCharge(account);

            var moneyOperation = accountCharge.CreateOperation();

            var transfers = new List<Transfer>();

            foreach (var chargeDesc in actualCharges)
            {
                var amount = chargeDesc.GetProxyAmount(proxy).RegopRoundDecimal(2);

                chargeDesc.UpdateAccountCharge(accountCharge, amount);

                accountCharge.Charge += amount;
                moneyOperation.Amount += amount;

                var transfer = new PersonalAccountChargeTransfer(
                    account,
                    chargeDesc.GetSourceWalletGuid(account),
                    accountCharge.Guid,
                    amount,
                    moneyOperation)
                {
                    Reason = chargeDesc.Name,
                    PaymentDate = accountCharge.ChargeDate,
                    OperationDate = accountCharge.ChargeDate
                };

                transfers.Add(transfer);

                this.LogInfo(
                    this.FormatTitle(proxy),
                    string.Format("Добавлено: {0} на сумму {1}", chargeDesc.Name, amount));
            }

            entitiesForSave.Add(moneyOperation);
            entitiesForSave.AddRange(transfers);
            entitiesForSave.Add(accountCharge);

            if (updateSummary)
            {
                EnumerableExtension.ForEach(
                    this.ChargeDescriptors.Where(x => !actualCharges.Contains(x)),
                    x => x.UpdateSummaryAmount(summary, 0.0m));

                summary.ApplyCharge(accountCharge);
            }

            entitiesForSave.Add(summary);

            roSummary.ApplyCharge(accountCharge, updateSummary);

            entitiesForSave.Add(roSummary);
        }

        private string FormatTitle(PersonalAccountInfoProxy proxy)
        {
            return string.Format(
                "Лицевой счет \"{0}\" (внешний номер \"{1}\") ркц \"{2}\"",
                proxy.AccountNumber,
                proxy.PersAccNumExternalSystems,
                proxy.RkcId);
        }


        private void LogInfo(string title, string message)
        {
            this.LogImport.Info(title, message);
        }

        private void LogError(string title, string message)
        {
            this.LogImport.Error(title, message);
        }

        /// <summary>
        /// Записать предупреждение о ЛС
        /// </summary>
        /// <param name="proxy">Сведения о начислениях ЛС</param>
        /// <param name="message">Сообщение</param>
        private void LogAccountWarning(PersonalAccountInfoProxy proxy, string message)
        {
            var title = this.FormatTitle(proxy);

            long? autoComparingAccountId = null;
            string autoComparingInfo = null;
            YesNo isCanAutoCompared = YesNo.No;

            // Если из файла пришли адрес и ФИО - попробовать найти сопоствление. По адресу и ФИО.
            // Для юрлиц не будет имени и отчества. Т.е. наименование будет лежать в фамилии.
            // Соответственно, для соспоставления необходимо, чтобы из фала пришёл адрес и фамилия, 
            // а имя и отчество могут быть пустыми, если это юрлицо.
            if (!string.IsNullOrEmpty(proxy.Address) && !string.IsNullOrEmpty(proxy.Surname))
            {
                var name = $"{proxy.Surname} {proxy.Name} {proxy.SecondName}".TrimEnd(); // Собрать ФИО. Для юрлица будет только фамилия. 
                var cmpr = this.accountRequisites.Get($"{proxy.Address}#{name}".ToUpper());
                if (cmpr != null)
                {
                    autoComparingAccountId = cmpr.Id;
                    autoComparingInfo = $"({cmpr.PersonalAccountNum}) {cmpr.Address} - {cmpr.Name}"; // номер ЛС - адрес - ФИО
                    isCanAutoCompared = YesNo.Yes;
                }
            }

            this.LogImport.Warn(title, message);
            this.warnings.Add(
                new AccountWarningInClosedPeriodsImport
            {
                Title = title,
                    Message = message,
                    ExternalNumber = proxy.PersAccNumExternalSystems,
                ExternalRkcId = proxy.RkcId,
                Name = $"{proxy.Surname} {proxy.Name} {proxy.SecondName}".TrimEnd(),
                Address = proxy.Address,
                    Task = new TaskEntry
                    {
                        Id = this.TaskId
                    },
                IsProcessed = YesNo.No,
                IsCanAutoCompared = isCanAutoCompared,
                ComparingAccountId = autoComparingAccountId,
                ComparingInfo = autoComparingInfo
            });
        }

        /// <summary>
        /// Записать предупреждение про существование начислений при импорте начислений в закрытый период
        /// </summary>
        /// <param name="proxy">Сведения о начислениях ЛС</param>
        /// <param name="message">Сообщение</param>
        /// <param name="chargeDescriptorName">Описатель</param>
        private void LogExistWarning(PersonalAccountInfoProxy proxy, string message, string chargeDescriptorName = null)
        {
            var title = this.FormatTitle(proxy);
            this.LogImport.Warn(title, message);
            this.warnings.Add(
                new ExistWarningInChargesToClosedPeriodsImport
                {
                    Title = title,
                    Message = message,
                    ChargeDescriptorName = chargeDescriptorName,
                    Task = new TaskEntry
                    {
                        Id = this.TaskId
                    }
                });
        }

        private class CashPaymentCenterPersAccProxy
        {
            /// <summary>
            /// Идентификатор РКЦ в БД
            /// </summary>
            public long Id { get; set; }

            /// <summary>
            /// Идентификатор РКЦ
            /// </summary>
            public string Identifier { get; set; }

            /// <summary>
            /// Идентификатор ЛС
            /// </summary>
            public long PersonalAccountId { get; set; }
        }

        private interface IChargeDescriptor
        {
            string Name { get; }

            decimal GetProxyAmount(PersonalAccountInfoProxy accountProxyInfo);

            decimal GetSummaryAmount(PersonalAccountPeriodSummary personalAccountSummary);

            string GetSourceWalletGuid(BasePersonalAccount account);

            void UpdateAccountCharge(PersonalAccountCharge accountCharge, decimal amount);

            void UpdateSummaryAmount(PersonalAccountPeriodSummary personalAccountSummary, decimal amount);
        }

        private class ChargeByTariffDescriptor : IChargeDescriptor
        {
            public string Name
            {
                get { return "Начисление по базовому тарифу"; }
            }

            public decimal GetProxyAmount(PersonalAccountInfoProxy accountProxyInfo)
            {
                return accountProxyInfo.ChargeTotal;
            }

            public decimal GetSummaryAmount(PersonalAccountPeriodSummary personalAccountSummary)
            {
                return personalAccountSummary.ChargeTariff;
            }

            public string GetSourceWalletGuid(BasePersonalAccount account)
            {
                return account.BaseTariffWallet.WalletGuid;
            }

            public void UpdateAccountCharge(PersonalAccountCharge accountCharge, decimal amount)
            {
                accountCharge.ChargeTariff = amount;
            }

            public void UpdateSummaryAmount(PersonalAccountPeriodSummary personalAccountSummary, decimal amount)
            {
                personalAccountSummary.ChargedByBaseTariff = amount;
                personalAccountSummary.ChargeTariff = amount;
            }
        }

        private class RecalcChargeDescriptor : IChargeDescriptor
        {
            public string Name
            {
                get { return "Перерасчет"; }
            }

            public decimal GetProxyAmount(PersonalAccountInfoProxy accountProxyInfo)
            {
                return accountProxyInfo.SettlementPeriodRecalc;
            }

            public decimal GetSummaryAmount(PersonalAccountPeriodSummary personalAccountSummary)
            {
                return personalAccountSummary.RecalcByBaseTariff;
            }

            public string GetSourceWalletGuid(BasePersonalAccount account)
            {
                return account.BaseTariffWallet.WalletGuid;
            }

            public void UpdateAccountCharge(PersonalAccountCharge accountCharge, decimal amount)
            {
                accountCharge.RecalcByBaseTariff = amount;
            }

            public void UpdateSummaryAmount(PersonalAccountPeriodSummary personalAccountSummary, decimal amount)
            {
                personalAccountSummary.RecalcByBaseTariff = amount;
            }
        }

        private class PenaltyChargeDescriptor : IChargeDescriptor
        {
            public string Name
            {
                get { return "Начисление пени"; }
            }

            public decimal GetProxyAmount(PersonalAccountInfoProxy accountProxyInfo)
            {
                return accountProxyInfo.Penalty;
            }

            public decimal GetSummaryAmount(PersonalAccountPeriodSummary personalAccountSummary)
            {
                return personalAccountSummary.Penalty;
            }

            public string GetSourceWalletGuid(BasePersonalAccount account)
            {
                return account.PenaltyWallet.WalletGuid;
            }

            public void UpdateAccountCharge(PersonalAccountCharge accountCharge, decimal amount)
            {
                accountCharge.Penalty = amount;
            }

            public void UpdateSummaryAmount(PersonalAccountPeriodSummary personalAccountSummary, decimal amount)
            {
                personalAccountSummary.Penalty = amount;
            }
        }

        private class accountRequisitesProxy
        {
            /// <summary>
            /// Идентификатор ЛС
            /// </summary>
            public long Id { get; set; }

            /// <summary>
            /// Номер ЛС
            /// </summary>
            public string PersonalAccountNum { get; set; }

            /// <summary>
            /// ФИО
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// Адрес
            /// </summary>
            public string Address { get; set; }
        }
    }
}