namespace Bars.Gkh.RegOperator.Domain.Extensions
{
    using System;
    using System.Linq;

    using B4;
    using B4.Application;
    using B4.DataAccess;
    using B4.IoC;
    using B4.Utils.Annotations;

    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.RegOperator.DomainModelServices;
    using Bars.Gkh.RegOperator.Dto;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount;
    using Bars.Gkh.RegOperator.Enums;
    using Bars.Gkh.Repositories.ChargePeriod;

    using Castle.Windsor;

    /// <summary>
    /// Расширения для фильтра лицевого счета
    /// </summary>
    public static class PersonalAccountFilterExtensions
    {
        /// <summary> Отфильтровать по базовым параметрам </summary>
        /// <typeparam name="TModel"> Тип модели </typeparam>
        /// <param name="query"> Исходный запрос </param>
        /// <param name="baseParams"> Базовые параметры </param>
        /// <param name="container"> Контейнер </param>
        /// <returns> Модифицированный запрос </returns>
        public static IQueryable<TModel> FilterByBaseParams<TModel>(
            this IQueryable<TModel> query,
            BaseParams baseParams,
            IWindsorContainer container) where TModel : PersonalAccountDto
        {
            ArgumentChecker.NotNull(container, nameof(container));

            var filterService = container.Resolve<IPersonalAccountFilterService>();
            using (container.Using(filterService))
            {
                return filterService.FilterByBaseParams(query, baseParams);
            }
        }

        /// <summary>
		/// Отфильтровать по базовым параметрам
		/// </summary>
		/// <param name="query">Исходный запрос</param>
		/// <param name="baseParams">Базовые параметры</param>
		/// <param name="container">Контейнер</param>
		/// <returns>Модифицированный запрос</returns>
        public static IQueryable<PersonalAccountDto> FilterByBaseParams(
            this IQueryable<PersonalAccountDto> query,
            BaseParams baseParams,
            IWindsorContainer container)
        {
            ArgumentChecker.NotNull(container, "container");

            var filterService = container.Resolve<IPersonalAccountFilterService>();
            using (container.Using(filterService))
            {
                return filterService.FilterByBaseParams(query, baseParams);
            }
        }

        /// <summary>
        /// Отфильтровать по базовым параметрам
        /// </summary>
        /// <typeparam name="TModel"> Тип модели </typeparam>
        /// <param name="query">Исходный запрос</param>
        /// <param name="baseParams">Базовые параметры</param>
        /// <param name="filterService">Сервис фильтрации</param>
        /// <returns>Модифицированный запрос</returns>
        public static IQueryable<TModel> FilterByBaseParams<TModel>(
            this IQueryable<TModel> query,
            BaseParams baseParams,
            IPersonalAccountFilterService filterService) where TModel : PersonalAccountDto
        {
            ArgumentChecker.NotNull(filterService, nameof(filterService));

            return filterService.FilterByBaseParams(query, baseParams);
        }

        /// <summary>
        /// Отфильтровать по базовым параметрам c условием
        /// </summary>
        /// <typeparam name="TModel"> Тип модели </typeparam>
        /// <param name="query">Исходный запрос</param>
        /// <param name="condition">Условие для выполнения</param>
        /// <param name="baseParams">Базовые параметры</param>
        /// <param name="filterService">Сервис фильтрации</param>
        /// <returns>Модифицированный запрос</returns>
        public static IQueryable<TModel> FilterByBaseParamsIf<TModel>(
            this IQueryable<TModel> query,
            bool condition,
            BaseParams baseParams,
            IPersonalAccountFilterService filterService) where TModel : PersonalAccountDto
        {
            if (!condition)
            {
                return query;
            }

            ArgumentChecker.NotNull(filterService, nameof(filterService));

            return filterService.FilterByBaseParams(query, baseParams);
        }

        /// <summary>
        /// Отфильтровать по настройкам фонда регионального оператора
        /// </summary>
        /// <typeparam name="TModel"> Тип модели </typeparam>
        /// <param name="query">Исходный запрос</param>
        /// <param name="filterService">Сервис фильтрации</param>
        /// <returns>Модифицированный запрос</returns>
        public static IQueryable<TModel> FilterByRegFondSetting<TModel>(
            this IQueryable<TModel> query,
            IPersonalAccountFilterService filterService) where TModel : PersonalAccountDto
        {
            ArgumentChecker.NotNull(filterService, nameof(filterService));

            PersonalAccountFilterExtensions.ClearSession();

            return filterService.FilterByRegFondSetting(query);
        }

        /// <summary>
        /// Отфильтровать запрос для расчетов
        /// </summary>
        /// <typeparam name="TModel"> Тип модели </typeparam>
        /// <param name="query">Запрос</param>
        /// <param name="chargePeriod">Период</param>
        /// <param name="filterService">Сервис фильтрации</param>
        /// <returns>Отфильтрованный запрос</returns>
        public static IQueryable<TModel> FilterCalculable<TModel>(
            this IQueryable<TModel> query,
            IPeriod chargePeriod,
            IPersonalAccountFilterService filterService) where TModel : PersonalAccountDto
        {
            ArgumentChecker.NotNull(filterService, nameof(filterService));

            PersonalAccountFilterExtensions.ClearSession();

            return filterService.FilterCalculable(query, chargePeriod);
        }

        /// <summary>
        /// Преобразовать в <see cref="PersonalAccountDto"/>
        /// <para></para>
        /// </summary>
        /// <param name="query">Запрос</param>
        /// <param name="useCache">Использовать кэш</param>
        /// <param name="joinInputQuery">Отфильтровать по входящему <see cref="IQueryable{BasePersonalAccount}"/></param>
        /// <param name="joinPeriodSummary">Включить информацию о суммах за период <see cref="PersonalAccountPeriodSummary"/></param>
        /// <param name="isCurrentPeriod">Данные за текущий открытый период</param>
        /// <param name="periodId">Id периода, на который нужны данные</param>
        /// <returns>Модифицированный запрос</returns>
        public static IQueryable<PersonalAccountDto> ToDto(
            this IQueryable<BasePersonalAccount> query,
            bool useCache = false,
            bool joinInputQuery = false,
            bool joinPeriodSummary = false,
            bool isCurrentPeriod = true,
            long periodId = 0)
        {
            var container = ApplicationContext.Current.Container;
            var accountDomain = container.ResolveDomain<BasePersonalAccount>();
            var chargeDomain = container.ResolveDomain<PersonalAccountCharge>();
            var chargePeriodRepository = container.Resolve<IChargePeriodRepository>();
            var personalAccountPrivilegedCategies = container.ResolveDomain<PersonalAccountPrivilegedCategory>();

            try
            {
                var curPeriod = chargePeriodRepository.GetCurrentPeriod();
                periodId = isCurrentPeriod && periodId == 0 ? curPeriod.Id : periodId;

                return isCurrentPeriod
                    ? useCache
                        ? query.GetDtoQuery(periodId, joinInputQuery, joinPeriodSummary)
                        : query.ToDtoInCurrentPeriod(joinPeriodSummary)
                    : query.ToActualDtoForPeriod(periodId, joinPeriodSummary);
            }
            finally
            {
                container.Release(accountDomain);
                container.Release(personalAccountPrivilegedCategies);
                container.Release(chargeDomain);
                container.Release(chargePeriodRepository);
            }
        }

        public static IQueryable<PersonalAccountDto> ToDtoInCurrentPeriod(this IQueryable<BasePersonalAccount> query, bool joinPeriodSummary)
        {
            var container = ApplicationContext.Current.Container;
            var periodSummaryDomain = container.ResolveDomain<PersonalAccountPeriodSummary>();
            var chargeDomain = container.ResolveDomain<PersonalAccountCharge>();
            var personalAccountPrivilegedCategies = container.ResolveDomain<PersonalAccountPrivilegedCategory>();
            var chargePeriodRepository = container.Resolve<IChargePeriodRepository>();

            using (container.Using(periodSummaryDomain, chargeDomain, personalAccountPrivilegedCategies, chargePeriodRepository))
            {
                var periodId = chargePeriodRepository.GetCurrentPeriod().Id;

                return joinPeriodSummary
                    ? periodSummaryDomain.GetAll()
                        .Where(sum => sum.Period.Id == periodId)
                        .Join(query,
                            sum => sum.PersonalAccount.Id,
                            x => x.Id,
                            (sum, x) =>
                                new PersonalAccountDto
                                {
                                    Id = x.Id,
                                    OwnerId = x.AccountOwner.Id,
                                    OwnerType = x.AccountOwner.OwnerType,
                                    AccountOwner = (x.AccountOwner as IndividualAccountOwner).Name
                                        ?? (x.AccountOwner as LegalAccountOwner).Contragent.Name,
                                    RoomAddress =
                                        x.Room.RealityObject.Address + ", кв. " + x.Room.RoomNum
                                        + (x.Room.ChamberNum != string.Empty && x.Room.ChamberNum != null
                                            ? ", ком. " + x.Room.ChamberNum
                                            : string.Empty),
                                    Address = x.Room.RealityObject.Address,
                                    RoomNum = x.Room.RoomNum,
                                    Area = x.Room.Area,
                                    AreaShare = x.AreaShare,
                                    CloseDate = x.CloseDate != DateTime.MinValue ? (DateTime?)x.CloseDate : null,
                                    OpenDate = x.OpenDate,
                                    Municipality = x.Room.RealityObject.Municipality.Name,
                                    Settlement = x.Room.RealityObject.MoSettlement.Name,
                                    MuId = x.Room.RealityObject.Municipality.Id,
                                    SettleId = x.Room.RealityObject.MoSettlement.Id,
                                    PersAccNumExternalSystems = x.PersAccNumExternalSystems,
                                    PersonalAccountNum = x.PersonalAccountNum,
                                    RoId = x.Room.RealityObject.Id,
                                    RoomId = x.Room.Id,
                                    AreaMkd = x.Room.RealityObject.AreaMkd,
                                    State = x.State,
                                    PrivilegedCategoryId = x.AccountOwner.PrivilegedCategory.Id,
                                    HasOnlyOneRoomWithOpenState = x.State.StartState && x.AccountOwner.ActiveAccountsCount == 1,
                                    RealArea = x.AreaShare * x.Room.Area,
                                    HasCharges =
                                        chargeDomain.GetAll().Where(z => z.BasePersonalAccount.Id == x.Id).Any(z => z.ChargePeriod.Id == periodId),
                                    AccountFormationVariant =
                                        x.Room.RealityObject.AccountFormationVariant.HasValue
                                            ? x.Room.RealityObject.AccountFormationVariant.Value
                                            : CrFundFormationType.Unknown,
                                    PrivilegedCategory = personalAccountPrivilegedCategies.GetAll().Any(z => z.PersonalAccount.Id == x.Id),
                                    SaldoIn = sum.SaldoIn,
                                    SaldoOut = sum.SaldoOut,
                                    CreditedWithPenalty = sum.ChargeTariff + sum.Penalty + sum.RecalcByBaseTariff + sum.RecalcByDecisionTariff
                                        + sum.RecalcByPenalty + sum.BaseTariffChange + sum.DecisionTariffChange + sum.PenaltyChange,
                                    PaidWithPenalty = sum.TariffPayment + sum.PenaltyPayment,
                                    RecalculationWithPenalty = sum.RecalcByBaseTariff + sum.RecalcByDecisionTariff + sum.RecalcByPenalty,
                                    PeriodId = periodId,
                                    DigitalReceipt = x.DigitalReceipt,
                                    HaveEmail = GetEmailMark(x),
                                    IsNotDebtor = x.IsNotDebtor,
                                    UnifiedAccountNumber = x.UnifiedAccountNumber,
                                })
                    : query.Select(x =>
                        new PersonalAccountDto
                        {
                            Id = x.Id,
                            OwnerId = x.AccountOwner.Id,
                            OwnerType = x.AccountOwner.OwnerType,
                            AccountOwner = (x.AccountOwner as IndividualAccountOwner).Name
                                ?? (x.AccountOwner as LegalAccountOwner).Contragent.Name,
                            RoomAddress =
                                x.Room.RealityObject.Address + ", кв. " + x.Room.RoomNum
                                + (x.Room.ChamberNum != string.Empty && x.Room.ChamberNum != null
                                    ? ", ком. " + x.Room.ChamberNum
                                    : string.Empty),
                            Address = x.Room.RealityObject.Address,
                            RoomNum = x.Room.RoomNum,
                            Area = x.Room.Area,
                            AreaShare = x.AreaShare,
                            CloseDate = x.CloseDate != DateTime.MinValue ? (DateTime?)x.CloseDate : null,
                            OpenDate = x.OpenDate,
                            Municipality = x.Room.RealityObject.Municipality.Name,
                            Settlement = x.Room.RealityObject.MoSettlement.Name,
                            MuId = x.Room.RealityObject.Municipality.Id,
                            SettleId = x.Room.RealityObject.MoSettlement.Id,
                            PersAccNumExternalSystems = x.PersAccNumExternalSystems,
                            PersonalAccountNum = x.PersonalAccountNum,
                            RoId = x.Room.RealityObject.Id,
                            RoomId = x.Room.Id,
                            AreaMkd = x.Room.RealityObject.AreaMkd,
                            State = x.State,
                            PrivilegedCategoryId = x.AccountOwner.PrivilegedCategory.Id,
                            HasOnlyOneRoomWithOpenState = x.State.StartState && x.AccountOwner.ActiveAccountsCount == 1,
                            RealArea = x.AreaShare * x.Room.Area,
                            HasCharges = chargeDomain.GetAll()
                                .Where(z => z.BasePersonalAccount.Id == x.Id)
                                .Any(z => z.ChargePeriod.Id == periodId),
                            AccountFormationVariant =
                                x.Room.RealityObject.AccountFormationVariant.HasValue
                                    ? x.Room.RealityObject.AccountFormationVariant.Value
                                    : CrFundFormationType.Unknown,
                            PrivilegedCategory = personalAccountPrivilegedCategies.GetAll().Any(z => z.PersonalAccount.Id == x.Id),
                            DigitalReceipt = x.DigitalReceipt,
                            HaveEmail = GetEmailMark(x),
                            IsNotDebtor = x.IsNotDebtor,
                            UnifiedAccountNumber = x.UnifiedAccountNumber
                        });
            }
        }

        /// <summary>
		/// Преобразовать в Dto
		/// </summary>
		/// <param name="query">Запрос</param>
		/// <returns>Модифицированный запрос</returns>
        public static IQueryable<PersonalAccountWithPaymentAccountDto> ToDtoWithPaymentAccount(this IQueryable<BasePersonalAccount> query)
        {
            var container = ApplicationContext.Current.Container;
            var accountDomain = container.ResolveDomain<BasePersonalAccount>();
            var chargeDomain = container.ResolveDomain<PersonalAccountCharge>();
            var chargePeriodRepository = container.Resolve<IChargePeriodRepository>();
            var personalAccountPrivilegedCategies = container.ResolveDomain<PersonalAccountPrivilegedCategory>();
            var calcAccountRoDomain = container.ResolveDomain<CalcAccountRealityObject>();
            try
            {
                var curPeriod = chargePeriodRepository.GetCurrentPeriod();
                return query
                    .Select(
                        x => new PersonalAccountWithPaymentAccountDto
                        {
                            Id = x.Id,
                            OwnerId = x.AccountOwner.Id,
                            OwnerType = x.AccountOwner.OwnerType,
                            AccountOwner =
                                (x.AccountOwner as IndividualAccountOwner).Name ??
                                    (x.AccountOwner as LegalAccountOwner).Contragent.Name,
                            RoomAddress =
                                x.Room.RealityObject.Address + ", кв. " + x.Room.RoomNum
                                    + (x.Room.ChamberNum != "" && x.Room.ChamberNum != null ? ", ком. " + x.Room.ChamberNum : string.Empty),
                            Address = x.Room.RealityObject.Address,
                            RoomNum = x.Room.RoomNum,
                            Area = x.Room.Area,
                            AreaShare = x.AreaShare,
                            CloseDate = x.CloseDate != DateTime.MinValue ? (DateTime?)x.CloseDate : null,
                            OpenDate = x.OpenDate,
                            MuId = x.Room.RealityObject.Municipality.Id,
                            SettleId = x.Room.RealityObject.MoSettlement.Id,
                            Municipality = x.Room.RealityObject.Municipality.Name,
                            Settlement = x.Room.RealityObject.MoSettlement.Name,
                            PersAccNumExternalSystems = x.PersAccNumExternalSystems,
                            PersonalAccountNum = x.PersonalAccountNum,
                            RoId = x.Room.RealityObject.Id,
                            RoomId = x.Room.Id,
                            AreaMkd = x.Room.RealityObject.AreaMkd,
                            State = x.State,
                            PrivilegedCategoryId = x.AccountOwner.PrivilegedCategory.Id,
                            RealArea = x.AreaShare * x.Room.Area,
                            HasCharges = chargeDomain.GetAll()
                                .Where(z => z.BasePersonalAccount.Id == x.Id)
                                .Where(z => z.ChargeDate >= curPeriod.StartDate)
                                .Any(z => !curPeriod.EndDate.HasValue || z.ChargeDate <= curPeriod.EndDate),
                            HasOnlyOneRoomWithOpenState = x.State.StartState && x.AccountOwner.ActiveAccountsCount == 1,
                            AccountFormationVariant =
                                x.Room.RealityObject.AccountFormationVariant.HasValue
                                    ? x.Room.RealityObject.AccountFormationVariant.Value
                                    : CrFundFormationType.Unknown,
                            PrivilegedCategory = personalAccountPrivilegedCategies.GetAll()
                                .Where(z => z.DateTo >= DateTime.Now.Date || z.DateTo == null)
                                .Where(z => z.DateFrom <= DateTime.Now.Date)
                                .Any(z => z.PersonalAccount.Id == x.Id),
                            RoPayAccountNum = calcAccountRoDomain.GetAll()
                                .Where(y => (y.Account.TypeAccount == TypeCalcAccount.Special && ((SpecialCalcAccount)y.Account).IsActive)
                                       || y.Account.TypeAccount == TypeCalcAccount.Regoperator)
                                .Where(y => y.RealityObject.Id == x.Room.RealityObject.Id)
                                .Where(y => y.Account.DateOpen <= DateTime.Today)
                                .Where(y => !y.Account.DateClose.HasValue || y.Account.DateClose.Value >= DateTime.Today)
                                .Where(
                                    y => (y.Account.TypeAccount == TypeCalcAccount.Special)
                                        || (y.Account.TypeAccount == TypeCalcAccount.Regoperator && y.DateStart <= DateTime.Today
                                            && (!y.DateEnd.HasValue || y.DateEnd >= DateTime.Today)))
                                .OrderByDescending(y => y.Account.DateOpen)
                                .Select(y => y.Account.AccountNumber ?? ((RegopCalcAccount)y.Account).ContragentCreditOrg.SettlementAccount)
                                .FirstOrDefault()
                        });
            }
            finally
            {
                container.Release(accountDomain);
                container.Release(personalAccountPrivilegedCategies);
                container.Release(chargeDomain);
                container.Release(chargePeriodRepository);
                container.Release(calcAccountRoDomain);
            }
        }

        /// <summary>
        /// Преобразовать в Dto, заполнив кошельки
        /// </summary>
        /// <param name="query">Запрос</param>
        /// <returns>Модифицированный запрос</returns>
        public static IQueryable<PersonalAccountWalletDto> ToDtoWithWallets(this IQueryable<BasePersonalAccount> query)
        {
            var container = ApplicationContext.Current.Container;
            var accountDomain = container.ResolveDomain<BasePersonalAccount>();
            var chargeDomain = container.ResolveDomain<PersonalAccountCharge>();
            var chargePeriodRepository = container.Resolve<IChargePeriodRepository>();
            var personalAccountPrivilegedCategies = container.ResolveDomain<PersonalAccountPrivilegedCategory>();

            try
            {
                var curPeriod = chargePeriodRepository.GetCurrentPeriod();

                return
                    query.Select(x =>
                        new PersonalAccountWalletDto
                        {
                            Id = x.Id,
                            OwnerId = x.AccountOwner.Id,
                            OwnerType = x.AccountOwner.OwnerType,
                            AccountOwner = (x.AccountOwner as IndividualAccountOwner).Name ?? (x.AccountOwner as LegalAccountOwner).Contragent.Name,
                            RoomAddress =
                                x.Room.RealityObject.Address + ", кв. " + x.Room.RoomNum
                                + (x.Room.ChamberNum != string.Empty && x.Room.ChamberNum != null ? ", ком. " + x.Room.ChamberNum : string.Empty),
                            Address = x.Room.RealityObject.Address,
                            RoomNum = x.Room.RoomNum,
                            Area = x.Room.Area,
                            AreaShare = x.AreaShare,
                            CloseDate = x.CloseDate != DateTime.MinValue ? (DateTime?)x.CloseDate : null,
                            OpenDate = x.OpenDate,
                            Municipality = x.Room.RealityObject.Municipality.Name,
                            Settlement = x.Room.RealityObject.MoSettlement.Name,
                            MuId = x.Room.RealityObject.Municipality.Id,
                            SettleId = x.Room.RealityObject.MoSettlement.Id,
                            PersAccNumExternalSystems = x.PersAccNumExternalSystems,
                            PersonalAccountNum = x.PersonalAccountNum,
                            RoId = x.Room.RealityObject.Id,
                            RoomId = x.Room.Id,
                            AreaMkd = x.Room.RealityObject.AreaMkd,
                            State = x.State,
                            PrivilegedCategoryId = (long?)x.AccountOwner.PrivilegedCategory.Id,
                            RealArea = x.AreaShare * x.Room.Area,
                            HasOnlyOneRoomWithOpenState = x.State.StartState && x.AccountOwner.ActiveAccountsCount == 1,
                            AccountFormationVariant =
                                x.Room.RealityObject.AccountFormationVariant.HasValue
                                    ? x.Room.RealityObject.AccountFormationVariant.Value
                                    : CrFundFormationType.Unknown,
                            PrivilegedCategory =
                                personalAccountPrivilegedCategies.GetAll()
                                    .Where(z => z.DateTo >= DateTime.Now.Date || z.DateTo == null)
                                    .Where(z => z.DateFrom <= DateTime.Now.Date)
                                    .Any(z => z.PersonalAccount.Id == x.Id),
                            HasCharges = chargeDomain.GetAll().Where(z => z.BasePersonalAccount.Id == x.Id).Any(z => z.ChargePeriod.Id == curPeriod.Id),
                            BaseTariffWalletGuid = x.BaseTariffWallet.WalletGuid,
                            DecisionTariffWalletGuid = x.DecisionTariffWallet.WalletGuid,
                            PenaltyWalletGuid = x.PenaltyWallet.WalletGuid,
                            SocialSupportWalletGuid = x.SocialSupportWallet.WalletGuid,
                            AccumulatedFundWalletGuid = x.AccumulatedFundWallet.WalletGuid,
                            PreviosWorkPaymentWalletGuid = x.PreviosWorkPaymentWallet.WalletGuid,
                            RestructAmicableAgreementWalletGuid = x.RestructAmicableAgreementWallet.WalletGuid,
                            RentWallet = x.RentWallet.WalletGuid
                        });
            }
            finally
            {
                container.Release(accountDomain);
                container.Release(personalAccountPrivilegedCategies);
                container.Release(chargeDomain);
                container.Release(chargePeriodRepository);
            }
        }

        /// <summary>
        /// Преобразовать в <see cref="BasePersonalAccountDto"/>, заполнив кошельки
        /// </summary>
        /// <param name="query">Запрос</param>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>Модифицированный запрос</returns>
        public static IQueryable<BasePersonalAccountDto> ToBasePersonalAccountDto(this IQueryable<BasePersonalAccount> query, BaseParams baseParams)
        {
            var container = ApplicationContext.Current.Container;

            var periodId = baseParams.Params.GetAs<long>("periodId");
            baseParams.Params.SetValue("filterByPeriod", true);

            var accountDomain = container.ResolveDomain<BasePersonalAccount>();
            var chargeDomain = container.ResolveDomain<PersonalAccountCharge>();
            var chargePeriodRepository = container.Resolve<IChargePeriodRepository>();
            var personalAccountPrivilegedCategies = container.ResolveDomain<PersonalAccountPrivilegedCategory>();

            try
            {
                return query.Select(x => new BasePersonalAccountDto
                {
                    Id = x.Id,
                    OwnerId = x.AccountOwner.Id,
                    OwnerType = x.AccountOwner.OwnerType,
                    AccountOwner = (x.AccountOwner as IndividualAccountOwner).Name ?? (x.AccountOwner as LegalAccountOwner).Contragent.Name,
                    RoomAddress =
                        x.Room.RealityObject.Address + ", кв. " + x.Room.RoomNum
                        + (x.Room.ChamberNum != string.Empty && x.Room.ChamberNum != null ? ", ком. " + x.Room.ChamberNum : string.Empty),
                    Address = x.Room.RealityObject.Address,
                    RoomNum = x.Room.RoomNum,
                    Area = x.Room.Area,
                    AreaShare = x.AreaShare,
                    CloseDate = x.CloseDate != DateTime.MinValue ? (DateTime?)x.CloseDate : null,
                    OpenDate = x.OpenDate,
                    Municipality = x.Room.RealityObject.Municipality.Name,
                    Settlement = x.Room.RealityObject.MoSettlement.Name,
                    MuId = x.Room.RealityObject.Municipality.Id,
                    SettleId = x.Room.RealityObject.MoSettlement.Id,
                    PersAccNumExternalSystems = x.PersAccNumExternalSystems,
                    PersonalAccountNum = x.PersonalAccountNum,
                    RoId = x.Room.RealityObject.Id,
                    RoomId = x.Room.Id,
                    AreaMkd = x.Room.RealityObject.AreaMkd,
                    State = x.State,
                    PrivilegedCategoryId = (long?)x.AccountOwner.PrivilegedCategory.Id,
                    RealArea = x.AreaShare * x.Room.Area,
                    HasCharges = chargeDomain.GetAll().Where(z => z.BasePersonalAccount.Id == x.Id).Any(z => z.ChargePeriod.Id == periodId),
                    HasOnlyOneRoomWithOpenState = x.State.StartState && x.AccountOwner.ActiveAccountsCount == 1,
                    AccountFormationVariant =
                        x.Room.RealityObject.AccountFormationVariant.HasValue
                            ? x.Room.RealityObject.AccountFormationVariant.Value
                            : CrFundFormationType.Unknown,
                    PrivilegedCategory = personalAccountPrivilegedCategies.GetAll().Any(z => z.PersonalAccount.Id == x.Id),

                    Letter = x.Room.RealityObject.FiasAddress.Letter,
                    Housing = x.Room.RealityObject.FiasAddress.Housing,
                    Building = x.Room.RealityObject.FiasAddress.Building,
                    PostalCode = x.Room.RealityObject.FiasAddress.PostCode,
                    RoomArea = x.Room.Area,
                    Tariff = x.Tariff,
                    IntNumber = x.IntNumber.ToString(),
                    AccuralByOwnersDecision = false,
                    DigitalReceipt = x.DigitalReceipt,
                    HaveEmail = GetEmailMark(x),
                    IsNotDebtor = x.IsNotDebtor,
                });
            }
            finally
            {
                container.Release(accountDomain);
                container.Release(personalAccountPrivilegedCategies);
                container.Release(chargeDomain);
                container.Release(chargePeriodRepository);
            }
        }

        /// <summary>
        /// Преобразовать в <see cref="BillingPersonalAccountDto"/>
        /// </summary>
        /// <param name="query">Запрос</param>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>Модифицированный запрос</returns>
        public static IQueryable<BillingPersonalAccountDto> ToBillingDto(this IQueryable<BasePersonalAccount> query, BaseParams baseParams)
        {
            var container = ApplicationContext.Current.Container;
            var accountDomain = container.ResolveDomain<BasePersonalAccount>();
            var chargeDomain = container.ResolveDomain<PersonalAccountCharge>();
            var ownershipHistoryDomain = container.ResolveDomain<AccountOwnershipHistory>();
            var chargePeriodRepository = container.Resolve<IChargePeriodRepository>();
            var personalAccountPrivilegedCategies = container.ResolveDomain<PersonalAccountPrivilegedCategory>();

            var periodId = baseParams.Params.GetAs<long>("periodId");
            baseParams.Params.SetValue("filterByPeriod", true);

            try
            {
                var curPeriod = chargePeriodRepository.GetCurrentPeriod();
                var period = chargePeriodRepository.Get(periodId);

                var persAccOwnerQuery = ownershipHistoryDomain.GetAll()
                    .Where(x => x.PersonalAccount != null && query.Any(y => y.Id == x.PersonalAccount.Id))
                    .Where(x => x.Date <= (period.EndDate ?? DateTime.Today))
                    .OrderByDescending(y => y.Date)
                    .Select(
                        x => new
                        {
                            x.PersonalAccount.Id,
                            x.AccountOwner
                        });

                var query2 = query
                    .Where(x => persAccOwnerQuery.Any(y => y.Id == x.Id))
                    .Select(
                        x =>
                            new BillingPersonalAccountDto
                            {
                                Id = x.Id,
                                OwnerId = persAccOwnerQuery.Where(y => y.Id == x.Id).Select(y => y.AccountOwner.Id).FirstOrDefault(),
                                OwnerType = persAccOwnerQuery.Where(y => y.Id == x.Id).Select(y => y.AccountOwner.OwnerType).FirstOrDefault(),
                                AccountOwner = persAccOwnerQuery.Where(y => y.Id == x.Id)
                                    .Select(y => y.AccountOwner is IndividualAccountOwner ? y.AccountOwner.Name : (y.AccountOwner as LegalAccountOwner).Contragent.Name)
                                    .FirstOrDefault(),
                                RoomAddress =
                                    x.Room.RealityObject.Address + ", кв. " + x.Room.RoomNum
                                    + (x.Room.ChamberNum != string.Empty && x.Room.ChamberNum != null ? ", ком. " + x.Room.ChamberNum : string.Empty),
                                Address = x.Room.RealityObject.Address,
                                RoomNum = x.Room.RoomNum,
                                Area = x.Room.Area,
                                AreaShare = x.AreaShare,
                                CloseDate = x.CloseDate != DateTime.MinValue ? (DateTime?)x.CloseDate : null,
                                OpenDate = x.OpenDate,
                                Municipality = x.Room.RealityObject.Municipality.Name,
                                Settlement = x.Room.RealityObject.MoSettlement.Name,
                                MuId = x.Room.RealityObject.Municipality.Id,
                                SettleId = x.Room.RealityObject.MoSettlement.Id,
                                PersAccNumExternalSystems = x.PersAccNumExternalSystems,
                                PersonalAccountNum = x.PersonalAccountNum,
                                RoId = x.Room.RealityObject.Id,
                                RoomId = x.Room.Id,
                                AreaMkd = x.Room.RealityObject.AreaMkd,
                                State = x.State,
                                RealArea = x.AreaShare * x.Room.Area,
                                HasCharges = chargeDomain.GetAll().Where(z => z.BasePersonalAccount.Id == x.Id).Any(z => z.ChargePeriod.Id == curPeriod.Id),

                                AccountFormationVariant =
                                    x.Room.RealityObject.AccountFormationVariant.HasValue
                                        ? x.Room.RealityObject.AccountFormationVariant.Value
                                        : CrFundFormationType.Unknown,
                                PrivilegedCategory = personalAccountPrivilegedCategies.GetAll().Any(z => z.PersonalAccount.Id == x.Id),
                                AccuralByOwnersDecision = false,
                                BillingAddressType = x.AccountOwner.BillingAddressType,

                                PrivilegedCategoryId = persAccOwnerQuery.Where(y => y.Id == x.Id).Select(y => y.AccountOwner.PrivilegedCategory.Id).FirstOrDefault(),
                                HasOnlyOneRoomWithOpenState = x.State.StartState
                                    && persAccOwnerQuery.Where(y => y.Id == x.Id).Select(y => y.AccountOwner.ActiveAccountsCount).FirstOrDefault() == 1,

                                Email = persAccOwnerQuery.Where(y => y.Id == x.Id)
                                    .Select(y => y.AccountOwner is IndividualAccountOwner
                                        ? (y.AccountOwner as IndividualAccountOwner).Email
                                        : (y.AccountOwner as LegalAccountOwner).Contragent.Email)
                                    .FirstOrDefault(),

                                AddressOutsideSubject = persAccOwnerQuery.Where(y => y.Id == x.Id)
                                    .Select(y => y.AccountOwner is IndividualAccountOwner
                                        ? (y.AccountOwner as IndividualAccountOwner).AddressOutsideSubject
                                        : (y.AccountOwner as LegalAccountOwner).Contragent.AddressOutsideSubject)
                                    .FirstOrDefault(),

                                LegalFactAddress = persAccOwnerQuery.Where(y => y.Id == x.Id)
                                    .Select(y => (y.AccountOwner as LegalAccountOwner).Contragent.FiasFactAddress)
                                    .FirstOrDefault(),


                                RobjectAddress = x.Room.RealityObject.FiasAddress,
                                HasNonZeroCharges = x.Charges
                                    .Where(y => y.ChargePeriod.Id == periodId)
                                    .Any(y => y.ChargeTariff != 0 || y.Penalty != 0 || y.RecalcByBaseTariff + y.RecalcByDecisionTariff + y.RecalcPenalty != 0),
                                SaldoOut =
                                (decimal?)x.Summaries
                                    .Where(y => y.Period.Id == periodId)
                                    .Select(y => y.SaldoOut).FirstOrDefault()
                                ?? 0
                            });

                return query2;
            }
            finally
            {
                container.Release(accountDomain);
                container.Release(personalAccountPrivilegedCategies);
                container.Release(chargeDomain);
                container.Release(ownershipHistoryDomain);
                container.Release(chargePeriodRepository);
            }
        }

        /// <summary>
        /// Метод возвращает Dto из хранимой сущности <see cref="Entities.Dto.BasePersonalAccountDto"/>
        /// </summary>
        /// <param name="query">Входящий запрос</param>
        /// <param name="periodId">Идентификатор периода</param>
        /// <param name="joinQuery">Фильтровать по входящему экземпляру <see cref="IQueryable{BasePersonalAccount}"/></param>
        /// <param name="joinPeriodSummary">Включить информацию о суммах за период <see cref="PersonalAccountPeriodSummary"/></param>
        /// <returns>Результирующий запрос</returns>
        public static IQueryable<PersonalAccountDto> GetDtoQuery(this IQueryable<BasePersonalAccount> query, long periodId, bool joinQuery = false, bool joinPeriodSummary = false)
        {
            var container = ApplicationContext.Current.Container;
            var dtoDomain = container.ResolveDomain<Entities.Dto.BasePersonalAccountDto>();
            var chargeDomain = container.ResolveDomain<PersonalAccountCharge>();
            var periodSummaryDomain = container.ResolveDomain<PersonalAccountPeriodSummary>();
            var privilegedCategiesDomain = container.ResolveDomain<PersonalAccountPrivilegedCategory>();

            using (container.Using(dtoDomain, chargeDomain, privilegedCategiesDomain, periodSummaryDomain))
            {
                if (joinPeriodSummary)
                {
                    return periodSummaryDomain.GetAll()
                        .Where(sum => sum.Period.Id == periodId)
                        .Where(sum => query.Any(y => y.Id == sum.PersonalAccount.Id))
                        .Join(dtoDomain.GetAll(),
                            x => x.PersonalAccount.Id,
                            x => x.Id,
                            (sum, x) => new PersonalAccountDto
                            {
                                // хранимые
                                Id = x.Id,
                                OwnerId = x.OwnerId,
                                OwnerType = x.OwnerType,
                                AccountOwner = x.AccountOwner,
                                RoomAddress = x.RoomAddress,
                                Address = x.Address,
                                RoomNum = x.RoomNum,
                                Area = x.Area,
                                AreaShare = x.AreaShare,
                                CloseDate = x.CloseDate != DateTime.MinValue ? x.CloseDate : null,
                                OpenDate = x.OpenDate,
                                Municipality = x.Municipality,
                                Settlement = x.Settlement,
                                MuId = x.MuId,
                                SettleId = x.SettleId,
                                PersAccNumExternalSystems = x.PersAccNumExternalSystems,
                                PersonalAccountNum = x.PersonalAccountNum,
                                RoId = x.RoId,
                                RoomId = x.RoomId,
                                AreaMkd = x.AreaMkd,
                                State = x.State,
                                PrivilegedCategoryId = x.PrivilegedCategoryId,
                                HasOnlyOneRoomWithOpenState = x.HasOnlyOneRoomWithOpenState,
                                AccountFormationVariant = x.AccountFormationVariant,
                                DigitalReceipt = x.DigitalReceipt,
                                HaveEmail = x.HaveEmail,
                                IsNotDebtor = x.IsNotDebtor,
                                UnifiedAccountNumber = x.UnifiedAccountNumber,

                                // рассчитываемые
                                RealArea = x.Area * x.AreaShare,
                                HasCharges = chargeDomain.GetAll().Where(z => z.BasePersonalAccount.Id == x.Id).Any(z => z.ChargePeriod.Id == periodId),
                                PrivilegedCategory = privilegedCategiesDomain.GetAll().Any(z => z.PersonalAccount.Id == x.Id),

                                SaldoIn = sum.SaldoIn,
                                SaldoOut = sum.SaldoOut,
                                CreditedWithPenalty = sum.ChargeTariff + sum.Penalty + sum.RecalcByBaseTariff + sum.RecalcByDecisionTariff
                                    + sum.RecalcByPenalty + sum.BaseTariffChange + sum.DecisionTariffChange + sum.PenaltyChange,
                                PaidWithPenalty = sum.TariffPayment + sum.PenaltyPayment,
                                RecalculationWithPenalty = sum.RecalcByBaseTariff + sum.RecalcByDecisionTariff + sum.RecalcByPenalty,
                                PeriodId = periodId
                            });
                }
                else
                {
                    return dtoDomain.GetAll()
                        .Where(x => query.Any(y => y.Id == x.Id))
                        .Select(x => new PersonalAccountDto
                        {
                            // хранимые
                            Id = x.Id,
                            OwnerId = x.OwnerId,
                            OwnerType = x.OwnerType,
                            AccountOwner = x.AccountOwner,
                            RoomAddress = x.RoomAddress,
                            Address = x.Address,
                            RoomNum = x.RoomNum,
                            Area = x.Area,
                            AreaShare = x.AreaShare,
                            CloseDate = x.CloseDate != DateTime.MinValue ? x.CloseDate : null,
                            OpenDate = x.OpenDate,
                            Municipality = x.Municipality,
                            Settlement = x.Settlement,
                            MuId = x.MuId,
                            SettleId = x.SettleId,
                            PersAccNumExternalSystems = x.PersAccNumExternalSystems,
                            PersonalAccountNum = x.PersonalAccountNum,
                            RoId = x.RoId,
                            RoomId = x.RoomId,
                            AreaMkd = x.AreaMkd,
                            State = x.State,
                            PrivilegedCategoryId = x.PrivilegedCategoryId,
                            HasOnlyOneRoomWithOpenState = x.HasOnlyOneRoomWithOpenState,
                            AccountFormationVariant = x.AccountFormationVariant,
                            DigitalReceipt = x.DigitalReceipt,
                            HaveEmail = x.HaveEmail,
                            IsNotDebtor = x.IsNotDebtor,
                            UnifiedAccountNumber = x.UnifiedAccountNumber,

                            // рассчитываемые
                            RealArea = x.Area * x.AreaShare,
                            HasCharges = chargeDomain.GetAll().Where(z => z.BasePersonalAccount.Id == x.Id).Any(z => z.ChargePeriod.Id == periodId),
                            PrivilegedCategory = privilegedCategiesDomain.GetAll().Any(z => z.PersonalAccount.Id == x.Id),

                            PeriodId = periodId
                        });
                }
            }
        }

        private static IQueryable<PersonalAccountDto> ToActualDtoForPeriod(this IQueryable<BasePersonalAccount> query, long periodId, bool joinPeriodSummary = false)
        {
            var container = ApplicationContext.Current.Container;
            var chargePeriodRepository = container.Resolve<IChargePeriodRepository>();
            var personalAccountPrivilegedCategies = container.ResolveDomain<PersonalAccountPrivilegedCategory>();
            var chargeDomain = container.ResolveDomain<PersonalAccountCharge>();
            var periodSummaryDomain = container.ResolveDomain<PersonalAccountPeriodSummary>();
            var ownershipHistoryDomain = container.ResolveDomain<AccountOwnershipHistory>();

            try
            {
                var period = chargePeriodRepository.Get(periodId);
                var curPeriod = chargePeriodRepository.GetCurrentPeriod();

                var persAccOwnerQuery = ownershipHistoryDomain.GetAll()
                    .Where(x => x.Date <= (period.EndDate ?? DateTime.Today))
                    .OrderByDescending(y => y.Date)
                    .ThenByDescending(y => y.Id)
                    .Select(
                        x => new
                        {
                            x.PersonalAccount.Id,
                            x.AccountOwner
                        });

                if (joinPeriodSummary)
                {
                    return periodSummaryDomain.GetAll()
                        .Where(sum => sum.Period.Id == periodId)
                        .Where(sum => persAccOwnerQuery.Any(y => y.Id == sum.PersonalAccount.Id))
                        .Join(query,
                            x => x.PersonalAccount.Id,
                            x => x.Id,
                            (sum, x) =>
                                new PersonalAccountDto
                                {
                                    Id = x.Id,
                                    OwnerId = persAccOwnerQuery.Where(y => y.Id == x.Id).Select(y => y.AccountOwner.Id).FirstOrDefault(),
                                    OwnerType = persAccOwnerQuery.Where(y => y.Id == x.Id)
                                        .Select(y => (PersonalAccountOwnerType?)y.AccountOwner.OwnerType).FirstOrDefault() ?? 0,
                                    AccountOwner = persAccOwnerQuery.Where(y => y.Id == x.Id)
                                        .Select(y => y.AccountOwner is IndividualAccountOwner
                                            ? y.AccountOwner.Name
                                            : (y.AccountOwner as LegalAccountOwner).Contragent.Name)
                                        .FirstOrDefault() ?? string.Empty,
                                    RoomAddress =
                                        x.Room.RealityObject.Address + ", кв. " + x.Room.RoomNum
                                        + (x.Room.ChamberNum != string.Empty && x.Room.ChamberNum != null ? ", ком. " + x.Room.ChamberNum : string.Empty),
                                    Address = x.Room.RealityObject.Address,
                                    RoomNum = x.Room.RoomNum,
                                    Area = x.Room.Area,
                                    AreaShare = x.AreaShare,
                                    CloseDate = x.CloseDate != DateTime.MinValue ? (DateTime?)x.CloseDate : null,
                                    OpenDate = x.OpenDate,
                                    Municipality = x.Room.RealityObject.Municipality.Name,
                                    Settlement = x.Room.RealityObject.MoSettlement.Name,
                                    MuId = x.Room.RealityObject.Municipality.Id,
                                    SettleId = x.Room.RealityObject.MoSettlement.Id,
                                    PersAccNumExternalSystems = x.PersAccNumExternalSystems,
                                    PersonalAccountNum = x.PersonalAccountNum,
                                    RoId = x.Room.RealityObject.Id,
                                    RoomId = x.Room.Id,
                                    AreaMkd = x.Room.RealityObject.AreaMkd,
                                    State = x.State,
                                    PrivilegedCategoryId = persAccOwnerQuery.Where(y => y.Id == x.Id).Select(y => y.AccountOwner.PrivilegedCategory.Id)
                                        .FirstOrDefault(),
                                    HasOnlyOneRoomWithOpenState = x.State.StartState
                                        && persAccOwnerQuery.Where(y => y.Id == x.Id).Select(y => y.AccountOwner.ActiveAccountsCount).FirstOrDefault()
                                        == 1,
                                    RealArea = x.AreaShare * x.Room.Area,
                                    HasCharges = chargeDomain.GetAll().Where(z => z.BasePersonalAccount.Id == x.Id)
                                        .Any(z => z.ChargePeriod.Id == curPeriod.Id),
                                    AccountFormationVariant =
                                        x.Room.RealityObject.AccountFormationVariant.HasValue
                                            ? x.Room.RealityObject.AccountFormationVariant.Value
                                            : CrFundFormationType.Unknown,
                                    PrivilegedCategory = personalAccountPrivilegedCategies.GetAll().Any(z => z.PersonalAccount.Id == x.Id),

                                    SaldoIn = sum.SaldoIn,
                                    SaldoOut = sum.SaldoOut,
                                    CreditedWithPenalty = sum.ChargeTariff + sum.Penalty + sum.RecalcByBaseTariff + sum.RecalcByDecisionTariff
                                        + sum.RecalcByPenalty + sum.BaseTariffChange + sum.DecisionTariffChange + sum.PenaltyChange,
                                    PaidWithPenalty = sum.TariffPayment + sum.PenaltyPayment,
                                    RecalculationWithPenalty = sum.RecalcByBaseTariff + sum.RecalcByDecisionTariff + sum.RecalcByPenalty,
                                    PeriodId = periodId,
                                    DigitalReceipt = x.DigitalReceipt,
                                    HaveEmail = GetEmailMark(x),
                                    IsNotDebtor = x.IsNotDebtor,
                                    UnifiedAccountNumber = x.UnifiedAccountNumber
                                });
                }
                else
                {
                    return query.Where(x => persAccOwnerQuery.Any(y => y.Id == x.Id))
                        .Select(x => new PersonalAccountDto
                        {
                            Id = x.Id,
                            OwnerId = persAccOwnerQuery.Where(y => y.Id == x.Id).Select(y => y.AccountOwner.Id).FirstOrDefault(),
                            OwnerType = persAccOwnerQuery.Where(y => y.Id == x.Id)
                                .Select(y => (PersonalAccountOwnerType?)y.AccountOwner.OwnerType).FirstOrDefault() ?? 0,
                            AccountOwner = persAccOwnerQuery.Where(y => y.Id == x.Id)
                                .Select(y => y.AccountOwner is IndividualAccountOwner
                                    ? y.AccountOwner.Name
                                    : (y.AccountOwner as LegalAccountOwner).Contragent.Name)
                                .FirstOrDefault() ?? string.Empty,
                            RoomAddress =
                                x.Room.RealityObject.Address + ", кв. " + x.Room.RoomNum
                                + (x.Room.ChamberNum != string.Empty && x.Room.ChamberNum != null ? ", ком. " + x.Room.ChamberNum : string.Empty),
                            Address = x.Room.RealityObject.Address,
                            RoomNum = x.Room.RoomNum,
                            Area = x.Room.Area,
                            AreaShare = x.AreaShare,
                            CloseDate = x.CloseDate != DateTime.MinValue ? (DateTime?)x.CloseDate : null,
                            OpenDate = x.OpenDate,
                            Municipality = x.Room.RealityObject.Municipality.Name,
                            Settlement = x.Room.RealityObject.MoSettlement.Name,
                            MuId = x.Room.RealityObject.Municipality.Id,
                            SettleId = x.Room.RealityObject.MoSettlement.Id,
                            PersAccNumExternalSystems = x.PersAccNumExternalSystems,
                            PersonalAccountNum = x.PersonalAccountNum,
                            RoId = x.Room.RealityObject.Id,
                            RoomId = x.Room.Id,
                            AreaMkd = x.Room.RealityObject.AreaMkd,
                            State = x.State,
                            PrivilegedCategoryId = persAccOwnerQuery.Where(y => y.Id == x.Id).Select(y => y.AccountOwner.PrivilegedCategory.Id)
                                .FirstOrDefault(),
                            HasOnlyOneRoomWithOpenState = x.State.StartState
                                && persAccOwnerQuery.Where(y => y.Id == x.Id).Select(y => y.AccountOwner.ActiveAccountsCount).FirstOrDefault()
                                == 1,
                            RealArea = x.AreaShare * x.Room.Area,
                            HasCharges = chargeDomain.GetAll().Where(z => z.BasePersonalAccount.Id == x.Id)
                                .Any(z => z.ChargePeriod.Id == curPeriod.Id),
                            AccountFormationVariant =
                                x.Room.RealityObject.AccountFormationVariant.HasValue
                                    ? x.Room.RealityObject.AccountFormationVariant.Value
                                    : CrFundFormationType.Unknown,
                            PrivilegedCategory = personalAccountPrivilegedCategies.GetAll().Any(z => z.PersonalAccount.Id == x.Id),
                            PeriodId = periodId,
                            DigitalReceipt = x.DigitalReceipt,
                            HaveEmail = GetEmailMark(x),
                            IsNotDebtor = x.IsNotDebtor,
                            UnifiedAccountNumber = x.UnifiedAccountNumber
                        });
                }
            }
            finally
            {
                container.Release(personalAccountPrivilegedCategies);
                container.Release(chargeDomain);
                container.Release(chargePeriodRepository);
                container.Release(ownershipHistoryDomain);
            }
        }

        private static void ClearSession()
        {
            var container = ApplicationContext.Current.Container;

            var sessions = container.Resolve<ISessionProvider>();

            using (container.Using(sessions))
            {
                sessions.GetCurrentSession().Clear();
            }
        }

        private static bool GetEmailMark(BasePersonalAccount acc)
        {
            string email;

            if (acc.AccountOwner.OwnerType == PersonalAccountOwnerType.Individual)
            {
                email = (acc.AccountOwner as IndividualAccountOwner)?.Email;
            }
            else
            {
                email = (acc.AccountOwner as LegalAccountOwner)?.Contragent?.Email;
            }

            if (!string.IsNullOrWhiteSpace(email))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}