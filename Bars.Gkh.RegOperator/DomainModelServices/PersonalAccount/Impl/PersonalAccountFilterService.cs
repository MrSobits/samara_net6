namespace Bars.Gkh.RegOperator.DomainModelServices.Impl
{
    using Authentification;

    using B4;
    using B4.DataAccess;
    using B4.Utils;
    using B4.Utils.Annotations;
    using Bars.B4.IoC;

    using Bars.Gkh.Extensions.Expressions;
    using Bars.Gkh.RegOperator.Domain.Extensions;
    using Bars.Gkh.RegOperator.DomainService.PersonalAccount;
    using Bars.Gkh.RegOperator.Dto;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount;
    using Bars.Gkh.Repositories.ChargePeriod;

    using Castle.Core.Internal;
    using Castle.Windsor;
    using ConfigSections.RegOperator;
    using Decisions.Nso.Domain;
    using Decisions.Nso.Entities;
    using Decisions.Nso.Entities.Decisions;
    using DomainService.RealityObjectAccount;
    using Entities;
    using Enums;

    using Gkh.Domain;
    using Gkh.Entities;
    using Gkh.Enums;
    using Gkh.Enums.Decisions;
    using Gkh.Utils;

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    /// <summary>
    /// Сервис фильтров лицевых счетов
    /// </summary>
    public class PersonalAccountFilterService : IPersonalAccountFilterService
    {
        public IDomainService<AccountManagementDecision> AccountManagementDecisionDomain { get; set; }
        public IDomainService<PersonalAccountPeriodSummary> PaPeriodSummaryDomainService { get; set; }
        public IDomainService<PersonalAccountCharge> PaChargeDomainService { get; set; }
        public IDomainService<BasePersonalAccount> PersAccDomainService { get; set; }
        public IDomainService<PersonalAccountRecalcEvent> PersonalAccountRecalcEventService { get; set; }
        public IDomainService<PersonalAccountPrivilegedCategory> PersonalAccountPrivilegedCategories { get; set; }
        public IDomainService<PersAccGroup> PaGroupService { get; set; }
        public IDomainService<PersAccGroupRelation> PersAccGroupRelationDomain { get; set; }
        public IDomainService<CalcAccountRealityObject> CalcAccRoDomain { get; set; }
        public IDomainService<CashPaymentCenterPersAcc> CashCenterPersAccDomain { get; set; }
        public IDomainService<CashPaymentCenterRealObj> CashCenterRealObjDomain { get; set; }
        public IDomainService<ChargePeriod> ChargePeriod { get; set; }
        public IDomainService<RealityObject> RealityObjDomainService { get; set; }
        public IDomainService<DeliveryAgentRealObj> DeliveryAgentRealityObjectDomainService { get; set; }
        public IDomainService<GovDecision> GovDecisionDomain { get; set; }

        public IPersonalAccountService AccountService { get; set; }
        public IBankAccountDataProvider BankProvider { get; set; }
        public IWindsorContainer Container { get; set; }
        public IRealityObjectDecisionsService RobjectDecisionService { get; set; }
        public IGkhUserManager UserManager { get; set; }
       
        private BaseParamsAccountFilter PreviousFilter;

        /// <summary>
        /// Отфильтровать запрос по параметрам запроса
        /// </summary>
        /// <param name="query">Запрос</param>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>Отфильтрованный запрос</returns>
        public IQueryable<TModel> FilterByBaseParams<TModel>(IQueryable<TModel> query, BaseParams baseParams) where TModel : PersonalAccountDto
        {
            if (baseParams != null)
            {
                this.PreviousFilter = new BaseParamsAccountFilter(baseParams);
            }

            return this.FilterByBaseParams(query, this.PreviousFilter);
        }

        /// <summary>
        /// Отфильтровать запрос для расчетов
        /// </summary>
        /// <param name="query">Запрос</param>
        /// <param name="period">Период</param>
        /// <returns>Отфильтрованный запрос</returns>
        public IQueryable<TModel> FilterCalculable<TModel>(IQueryable<TModel> query, IPeriod period) where TModel : PersonalAccountDto
        {
            ArgumentChecker.NotNull(period, nameof(period));
            var connectionTypeByAccount = this.Container.GetGkhConfig<RegOperatorConfig>().GeneralConfig.CachPaymentCenterConnectionType
                                          == CachPaymentCenterConnectionType.ByAccount;

            // var actualDecisionForCollection = _robjectDecisionService.GetActualDecisionForCollection<AccountManagementDecision>(null, true);

            // var roIdsQuery = actualDecisionForCollection
            // .Where(x => x.Value.Decision == AccountManagementType.ByOwners)
            // .Select(x => x.Key.Id);
            var realityObjectQuery =
                this.AccountManagementDecisionDomain.GetAll()
                    .Where(x => x.Protocol.State.FinalState)
                    .Select(x => x.Protocol.RealityObject.Id)
                    .Select(
                        x =>
                            new
                            {
                                RoId = x,
                                Decision =
                                this.AccountManagementDecisionDomain.GetAll()
                                    .Where(y => y.Protocol.RealityObject.Id == x)
                                    .OrderByDescending(y => y.Protocol.ProtocolDate)
                                    .Select(y => y.Decision)
                                    .FirstOrDefault()
                            })
                    .Where(x => x.Decision == AccountManagementType.ByOwners);

            var date = period.Return(x => x.StartDate);

            if (connectionTypeByAccount)
            {
                var cashPayQuery =
                    this.CashCenterPersAccDomain.GetAll()
                        .Where(x => x.CashPaymentCenter.ConductsAccrual)
                        .Where(x => x.DateStart <= date)
                        .Where(x => !x.DateEnd.HasValue || x.DateEnd.Value >= date)
                        .Select(x => x.PersonalAccount.Id);

                query = query.Where(x => !cashPayQuery.Any(y => y == x.Id));
            }
            else
            {
                var roIdsQuery =
                    this.CashCenterRealObjDomain.GetAll()
                        .Where(x => x.CashPaymentCenter.ConductsAccrual)
                        .Where(x => x.DateStart <= date)
                        .Where(x => !x.DateEnd.HasValue || x.DateEnd.Value >= date)
                        .Select(x => x.RealityObject.Id);

                query = query.Where(x => !roIdsQuery.Any(y => y == x.RoId));
            }

            var roIdsToExclude = this.GetActualRoIdsByConditionHouse();
            var paIdsInSystemGroup = this.GetPersonalAccountsIdInSystemGroups();

            // у ЛС был закрытие задней датой
            var recalcQuery = this.PersonalAccountRecalcEventService.GetAll()
                .Where(x => x.Period.Id == period.Id)
                .Where(x => x.RecalcEventType == RecalcEventType.ChangeCloseDate);

            return
                query.Where(x => !realityObjectQuery.Any(y => y.RoId == x.RoId))

                    // если дом неаварийный или по нему есть закрытие задним числом
                    .Where(x => !roIdsToExclude.Contains(x.RoId) || recalcQuery.Any(y => y.PersonalAccount.Id == x.Id))

                    // неактивные никогда не расчитываем
                    .Where(x => x.State.Code != BasePersonalAccount.StateNonActiveCode)

                    //выбираем ЛС которые не содержаться в системной группу "Сформирован в открытом периоде"
                    .WhereIf(paIdsInSystemGroup != null, x => !paIdsInSystemGroup.Any(y => y == x.Id))

                    // фильтр по статусу: открытые, закрытые в текущем периоде, перерасчет задним числом
                    .Where(
                        x =>
                            x.State.Code == BasePersonalAccount.StateOpenedCode
                                || ((x.CloseDate <= DateTime.MinValue || period.StartDate < x.CloseDate)
                                    && x.State.Code == BasePersonalAccount.StateCloseDebtCode)
                                || (x.State.Code == BasePersonalAccount.StateCloseCode && period.StartDate < x.CloseDate)
                                || recalcQuery.Any(y => y.PersonalAccount.Id == x.Id));
        }

        /// <summary>
        /// Фильтрация по единым настройкам приложения: Расчет вести для домов, у которых способ формирования фонда
        /// </summary>
        /// <typeparam name="TModel">
        /// Тип данных
        /// </typeparam>
        /// <param name="data">
        /// Запрос для фильтрации
        /// </param>
        /// <returns>
        /// Запрос с фильтрацией
        /// </returns>
        public IQueryable<TModel> FilterByRegFondSetting<TModel>(IQueryable<TModel> data) where TModel : PersonalAccountDto
        {
            var config = this.Container.GetGkhConfig<RegOperatorConfig>().GeneralConfig;
            var includeSpecialCalcAccount = config.HouseCalculationConfig.SpecialCalcAccount;
            var includeRegopCalcAccount = config.HouseCalculationConfig.RegopCalcAccount;
            var includeRegopSpecialCalcAccount = config.HouseCalculationConfig.RegopSpecialCalcAccount;
            var includeUnknown = config.HouseCalculationConfig.Unknown;
            if (includeSpecialCalcAccount && includeRegopCalcAccount && includeRegopSpecialCalcAccount && includeUnknown)
            {
                return data;
            }

            var crFundFormationTypes = new List<int>();

            if (includeSpecialCalcAccount)
            {
                crFundFormationTypes.Add((int) CrFundFormationType.SpecialAccount);
            }

            if (includeRegopCalcAccount)
            {
                crFundFormationTypes.Add((int) CrFundFormationType.RegOpAccount);
            }

            if (includeRegopSpecialCalcAccount)
            {
                crFundFormationTypes.Add((int) CrFundFormationType.SpecialRegOpAccount);
            }

            if (includeUnknown)
            {
                crFundFormationTypes.Add((int) CrFundFormationType.Unknown);
                crFundFormationTypes.Add((int) CrFundFormationType.NotSelected);
            }

            return data.Where(x => crFundFormationTypes.Contains((int) x.AccountFormationVariant));
        }

        private IQueryable<TModel> FilterByBaseParams<TModel>(IQueryable<TModel> query, BaseParamsAccountFilter filter) where TModel : PersonalAccountDto
        {
            ArgumentChecker.NotNull(query, nameof(query));

            var connectionTypeByAccount = this.Container.GetGkhConfig<RegOperatorConfig>().GeneralConfig
                .CachPaymentCenterConnectionType == CachPaymentCenterConnectionType.ByAccount;

            if (filter == null)
            {
                return query;
            }

            // если пришли из реестра ЛС, то отсюда получим дату сегодняшнего дня
            // если из платежных документов, то дату начала периода
            var periodDate = this.GetPeriodStartDate(filter.PeriodId, filter.FilterByPeriod);

            // если запрос из реестра абонентов, то никакие другие фильтры не нужны
            if (filter.FromOwner)
            {
                return query.Where(x => x.OwnerId == filter.OwnerId);
            }

            var userMuIds = this.GetUserMuIds();

            List<long> listRoIdsByBankAccNum = null;
            if (filter.BankAccNumSpecified)
            {
                var calcAccId = this.BankProvider.GetBankAccountInfo(filter.RoId).Return(x => x.CalcAccId);

                listRoIdsByBankAccNum =
                    this.CalcAccRoDomain.GetAll()
                        .Where(x => x.Account.Id == calcAccId)
                        .Where(x => x.DateStart <= periodDate)
                        .Where(x => !x.DateEnd.HasValue || x.DateEnd >= periodDate)
                        .Select(x => x.RealityObject.Id)
                        .ToList();
            }

            // фильтр по группа ЛС
            IQueryable<long> personalAccountGroupQuery = null;
            if (filter.GroupIds.IsNotEmpty() || filter.ShowInAllGroups)
            {
                personalAccountGroupQuery = this.PersAccGroupRelationDomain.GetAll()
                        .WhereIf(!filter.ShowInAllGroups, x => filter.GroupIds.Contains(x.Group.Id))
                        .Select(x => x.PersonalAccount.Id);
            }

            var period = this.ChargePeriod.Get(filter.PeriodId);
            var accountPrivilegedCategories = this.PersonalAccountPrivilegedCategories.GetAll()

                // фильтруем категории только по тем периодам которые пришли явно
                .WhereIf(
                    !filter.PrivilegedCategory.IsPrivilegedCategoryAll && filter.PrivilegedCategory.PrivilegedCategoryIds.IsNotEmpty(),
                    x => filter.PrivilegedCategory.PrivilegedCategoryIds.Contains(x.PrivilegedCategory.Id));

            IQueryable<long> roIdsHaveDeliveryAgents = null;
            IQueryable<long> allRoIdsHaveDeliveryAgents = null;
            IQueryable<long> roIdsFilteredByDeliveryAgents = null;
            if (filter.DeliveryAgentIds.IsNotEmpty() || filter.DeliveryAgentShowAll)
            {
                roIdsHaveDeliveryAgents =
                    this.DeliveryAgentRealityObjectDomainService.GetAll()
                        .WhereIf(!filter.DeliveryAgentShowAll, x => filter.DeliveryAgentIds.Contains(x.DeliveryAgent.Id))
                        .Where(x => x.DateStart < periodDate && (x.DateEnd > periodDate || x.DateEnd == null))
                        .Select(x => x.RealityObject.Id);
            }

            if (filter.HasNotDeliveryAgent)
            {
                allRoIdsHaveDeliveryAgents = this.DeliveryAgentRealityObjectDomainService.GetAll().Select(x => x.RealityObject.Id);
            }

            if (roIdsHaveDeliveryAgents != null || allRoIdsHaveDeliveryAgents != null)
            {
                Expression<Func<RealityObject, bool>> filterExpression = null;
                if (roIdsHaveDeliveryAgents != null)
                {
                    filterExpression = x => roIdsHaveDeliveryAgents.Any(y => y == x.Id);
                }

                if (allRoIdsHaveDeliveryAgents != null)
                {
                    Expression<Func<RealityObject, bool>> tempFilterExpression = x => !allRoIdsHaveDeliveryAgents.Any(y => y == x.Id);

                    filterExpression = filterExpression.IsNull() ? tempFilterExpression : filterExpression.Or(tempFilterExpression);
                }

                roIdsFilteredByDeliveryAgents = this.RealityObjDomainService.GetAll()
                    .Where(filterExpression)
                    .Select(x => x.Id);
            }

            IQueryable<long> paInCharges = null;
            IQueryable<long> paInSummaries = null;
            IQueryable<long> persAccOwType = null;
            if (filter.HasChargesValues.IsNotEmpty())
            {
                if (filter.HasBaseTariffCharge ||
                        filter.HasBaseTariffZeroCharge ||
                        filter.HasDecisionTariffCharge ||
                        filter.HasPenaltyCharge ||
                        filter.HasPenaltyZeroCharge)
                {
                    paInCharges =
                        this.PaChargeDomainService.GetAll()
                            .Where(x => x.ChargePeriod.Id == period.Id && x.IsActive)
                            .Where(
                                x =>
                                    (filter.HasBaseTariffCharge && x.ChargeTariff - x.OverPlus > 0)
                                    || (filter.HasBaseTariffZeroCharge && x.ChargeTariff - x.OverPlus == 0)
                                    || (filter.HasDecisionTariffCharge && x.OverPlus > 0)
                                    || (filter.HasPenaltyCharge && x.Penalty > 0)
                                    || (filter.HasPenaltyZeroCharge && x.Penalty == 0))
                            .Select(x => x.BasePersonalAccount.Id);
                }

                if (filter.HasOverpayment)
                {
                    paInSummaries = this.PaPeriodSummaryDomainService.GetAll()
                            .Where(x => x.Period.Id == period.Id)
                            .Where(x => x.SaldoOut < 0)
                            .Select(x => x.PersonalAccount.Id);
                }

                if (filter.HasBaseTariffOverpayment)
                {
                    paInSummaries = this.PaPeriodSummaryDomainService.GetAll()
                            .Where(x => x.Period.Id == period.Id)
                            .Where(x => (x.BaseTariffDebt + x.ChargedByBaseTariff + x.RecalcByBaseTariff + x.BaseTariffChange - x.TariffPayment) < 0)
                            .Select(x => x.PersonalAccount.Id);
                }

                if (filter.HasDecisionTariffOverpayment)
                {
                    paInSummaries = this.PaPeriodSummaryDomainService.GetAll()
                            .Where(x => x.Period.Id == period.Id)
                            .Where(x => (x.DecisionTariffDebt + (x.ChargeTariff - x.ChargedByBaseTariff) + x.RecalcByDecisionTariff
                                          + x.DecisionTariffChange - x.TariffDecisionPayment) < 0)
                            .Select(x => x.PersonalAccount.Id);
                }

                if (filter.HasPenaltyOverpayment)
                {
                    paInSummaries = this.PaPeriodSummaryDomainService.GetAll()
                            .Where(x => x.Period.Id == period.Id)
                            .Where(x => (x.PenaltyDebt + x.Penalty + x.RecalcByPenalty + x.PenaltyChange - x.PenaltyPayment) < 0)
                            .Select(x => x.PersonalAccount.Id);
                }

                if (filter.HasBaseTariffDebt)
                {
                    paInSummaries = this.PaPeriodSummaryDomainService.GetAll()
                            .Where(x => x.Period.Id == period.Id)
                            .Where(x => (x.BaseTariffDebt + x.ChargedByBaseTariff + x.RecalcByBaseTariff + x.BaseTariffChange - x.TariffPayment) > 0)
                            .Select(x => x.PersonalAccount.Id);
                }

                if (filter.HasDecisionDebt)
                {
                    paInSummaries = this.PaPeriodSummaryDomainService.GetAll()
                            .Where(x => x.Period.Id == period.Id)
                            .Where(x => (x.DecisionTariffDebt + (x.ChargeTariff - x.ChargedByBaseTariff) + x.RecalcByDecisionTariff
                                          + x.DecisionTariffChange - x.TariffDecisionPayment) > 0)
                            .Select(x => x.PersonalAccount.Id);
                }

                if (filter.HasPenaltyDebt)
                {
                    paInSummaries = this.PaPeriodSummaryDomainService.GetAll()
                            .Where(x => x.Period.Id == period.Id)
                            .Where(x => (x.PenaltyDebt + x.Penalty + x.RecalcByPenalty + x.PenaltyChange - x.PenaltyPayment) > 0)
                            .Select(x => x.PersonalAccount.Id);
                }
            }

            if (filter.OwnershipTypeValues.IsNotEmpty())
            {
                persAccOwType = this.PersAccDomainService.GetAll()
                    .Where(x => filter.OwnershipTypeValues.Contains(x.Room.OwnershipType))
                    .Select(x => x.Id);
            }

            var filteredQuery = query

                // фильтр по владельцу
                .WhereIf(filter.OwnerIdSpecified, x => x.OwnerId == filter.OwnerId)

                // фильтр по дому
                .WhereIf(filter.RoIdSpecified && !filter.BankAccNumSpecified, x => x.RoId == filter.RoId)

                // фильтр по р/с
                .WhereIf(listRoIdsByBankAccNum != null, x => listRoIdsByBankAccNum.Contains(x.RoId))

                // фильтр по муниципальнцым образованиям пользователя
                .WhereIf(userMuIds.Count > 0,
                    x => userMuIds.Contains(x.MuId)
                        || (x.SettleId.HasValue && userMuIds.Contains(x.SettleId.Value)))

                // фильтр по типу решения
                .WhereIf(filter.CrFundTypeList.IsNotEmpty(), x => filter.CrFundTypeList.Contains(x.AccountFormationVariant))

                // фильтр по льготным категориям
                .WhereIf(
                    filter.PrivilegedCategory.IsPrivilegedCategoryAll || filter.PrivilegedCategory.PrivilegedCategoryIds.IsNotEmpty(),
                    x => accountPrivilegedCategories.Any(y => y.PersonalAccount.Id == x.Id))

                // фильтр по расчетно-кассовым центрам (если стоит в единых настройках привязка РКЦ - ЛС)
                .WhereIf(connectionTypeByAccount && filter.CashPaymentCenterId > 0,
                    x =>
                        this.CashCenterPersAccDomain.GetAll()
                            .Any(
                                y =>
                                    y.CashPaymentCenter.Id == filter.CashPaymentCenterId && y.PersonalAccount.Id == x.Id
                                    && (!y.DateEnd.HasValue || y.DateEnd > periodDate) && y.DateStart <= periodDate))

                // фильтр по расчетно-кассовым центрам (если стоит в единых настройках привязка РКЦ - Дом)
                .WhereIf(!connectionTypeByAccount && filter.CashPaymentCenterId > 0,
                    x =>
                        this.CashCenterRealObjDomain.GetAll()
                            .Any(y =>
                                    y.CashPaymentCenter.Id == filter.CashPaymentCenterId && y.RealityObject.Id == x.RoId
                                    && (!y.DateEnd.HasValue || y.DateEnd > periodDate) && y.DateStart <= periodDate))

                // фильтр по статусу
                .WhereIf(!filter.ShowAll, x => x.State.Code != BasePersonalAccount.StateNonActiveCode)

                // если печатаем документы, то нужно также скрыть закрытые ЛС по дате
                .WhereIf(
                    !filter.ShowAll && filter.FilterByPeriod,
                    x => x.CloseDate <= DateTime.MinValue || !x.CloseDate.HasValue || x.CloseDate.Value.Year >= 3000
                        || (BasePersonalAccount.StateCloseDebtCode.Equals(x.State.Code) && periodDate <= x.CloseDate))

                // фильтр по Id квартиры
                .WhereIf(filter.RoomId > 0, x => x.RoomId == filter.RoomId)

                // фильтр по типу абонента
                .WhereIf(filter.CrOwnerTypeList.IsNotEmpty(), this.CreateExpressionForFilterValue<TModel>(filter.CrOwnerTypeList))

                // фильтр по типу собственности
                .WhereIf(persAccOwType != null, x => persAccOwType.Any(y => y == x.Id))

                // фильтр по группам ЛС
                .WhereIf(personalAccountGroupQuery != null, x => personalAccountGroupQuery.Any(y => y == x.Id))

                // фильтр по агентам доставки
                .WhereIf(roIdsFilteredByDeliveryAgents != null, x => roIdsFilteredByDeliveryAgents.Any(y => y == x.RoId))

                // фильтр по начислениям закрытого периода
                .WhereIf(paInSummaries != null, x => paInSummaries.Any(y => y == x.Id))

                // фильтр по начислениям текущего периода
                .WhereIf(paInCharges != null, x => paInCharges.Any(y => y == x.Id));
            
            if (filter.AccountRegistryMode == AccountRegistryMode.Calc)
            {
                var periodRepo = this.Container.Resolve<IChargePeriodRepository>();

                try
                {
                    var currentPeriod = periodRepo.GetCurrentPeriod();
                    return this.FilterCalculable(filteredQuery, currentPeriod);
                }
                finally
                {
                    this.Container.Release(periodRepo);
                }
            }

            return filteredQuery;
        }

        private Expression<Func<TModel, bool>> CreateExpressionForFilterValue<TModel>(List<CrOwnerFilterType> crOwnerFilterTypes) where TModel : PersonalAccountDto
        {
            if (crOwnerFilterTypes == null)
            {
                return dto => true;
            }

            var parameterDto = Expression.Parameter(typeof(PersonalAccountDto), "dto");
            Expression propertyOwnerType = Expression.Property(parameterDto, "OwnerType");
            Expression constantLegal = Expression.Constant(PersonalAccountOwnerType.Legal);

            Expression totalExpression = null;
            var first = true;

            foreach (var currentOwnerType in crOwnerFilterTypes)
            {
                switch (currentOwnerType)
                {
                    case CrOwnerFilterType.LegalPerson:
                        {
                            Expression expression = Expression.Equal(propertyOwnerType, constantLegal);
                            if (first)
                            {
                                totalExpression = expression;
                                first = false;
                            }
                            else
                            {
                                totalExpression = Expression.OrElse(totalExpression, expression);
                            }
                        }

                        break;
                    case CrOwnerFilterType.PrysicalPerson:
                        {
                            Expression constantIndividual = Expression.Constant(PersonalAccountOwnerType.Individual);
                            Expression expression = Expression.Equal(propertyOwnerType, constantIndividual);
                            if (first)
                            {
                                totalExpression = expression;
                                first = false;
                            }
                            else
                            {
                                totalExpression = Expression.OrElse(totalExpression, expression);
                            }
                        }

                        break;
                    case CrOwnerFilterType.LegalPersonWithOneRoom:
                        {
                            Expression ownerEqualConstant = Expression.Equal(propertyOwnerType, constantLegal);

                            Expression propertyHasOnlyOneRoom = Expression.Property(parameterDto, "HasOnlyOneRoomWithOpenState");

                            Expression constantTrue = Expression.Constant(true);

                            Expression ownerHasOnlyRoom = Expression.Equal(propertyHasOnlyOneRoom, constantTrue);

                            Expression ownerEqualConstantAndownerHasOnlyRoom = Expression.AndAlso(ownerEqualConstant, ownerHasOnlyRoom);
                            if (first)
                            {
                                totalExpression = ownerEqualConstantAndownerHasOnlyRoom;
                                first = false;
                            }
                            else
                            {
                                totalExpression = Expression.OrElse(totalExpression, ownerEqualConstantAndownerHasOnlyRoom);
                            }
                        }

                        break;
                }
            }

            if (totalExpression == null)
            {
                return dto => true;
            }

            var result = Expression.Lambda<Func<TModel, bool>>(totalExpression, parameterDto);
            return result;
        }

        private DateTime GetPeriodStartDate(long periodId, bool usePeriodId)
        {
            var periodStartDate = DateTime.Today;
            var periodDomain = this.Container.Resolve<IChargePeriodRepository>();

            using (this.Container.Using(periodDomain))
            {
                var period = periodDomain.Get(periodId);

                if (period != null && usePeriodId)
                {
                    periodStartDate = period.StartDate;
                }
                return periodStartDate;
            }
        }

        /// <summary>
        /// Класс, который содержит в себе вытащенные из параметра запроса
        /// </summary>
        private class BaseParamsAccountFilter
        {
            /// <summary>
            /// Конструктор
            /// </summary>
            /// <param name="baseParams">Параметры запроса</param>
            public BaseParamsAccountFilter(BaseParams baseParams)
            {
                this.LoadParam = baseParams.GetLoadParam();

                this.OwnerIdSpecified = baseParams.Params.ContainsKey("ownerId");
                this.OwnerId = baseParams.Params.GetAsId("ownerId");
                this.PeriodId = baseParams.Params.GetAsId("periodId");
                this.FilterByPeriod = baseParams.Params.GetAs<bool>("filterByPeriod");
                this.FromOwner = baseParams.Params.GetAs<bool>("fromOwner", ignoreCase: true);

                if (baseParams.Params.ContainsKey("crFoundType") &&
                    baseParams.Params.GetAs<string>("crFoundType").IsNotEmpty())
                {
                    this.CrFundTypeList =
                        baseParams.Params.GetAs<object>("crFoundType")
                            .CastAs<List<object>>()
                            .Cast<DynamicDictionary>()
                            .Select(
                                decType =>
                                    (CrFundFormationType)
                                    Enum.Parse(typeof(CrFundFormationType), decType.GetAs<string>("Id")))
                            .ToList();
                }

                this.ShowAll = this.LoadParam.Filter.GetAs<bool>("showAll", ignoreCase: true) || baseParams.Params.GetAs<bool>("showAll", ignoreCase: true);

                this.CashPaymentCenterId = baseParams.Params.GetAsId("cashPaymentCenterId");

                this.PrivilegedCategory = new PrivilegedCategoryFilter(baseParams);

                this.RoIdSpecified = baseParams.Params.ContainsKey("roId");
                this.RoId = baseParams.Params.GetAsId("roId");

                this.BankAccNumSpecified = baseParams.Params.GetAs<bool>("bankAccNum");
                this.RoomId = baseParams.Params.GetAsId("RoomId");

                if (baseParams.Params.ContainsKey("crOwnerTypeValues") && baseParams.Params.GetAs<string>("crOwnerTypeValues").IsNotEmpty())
                {
                    this.CrOwnerTypeList =
                        baseParams.Params.GetAs<object>("crOwnerTypeValues")
                            .CastAs<List<object>>()
                            .Cast<DynamicDictionary>()
                            .Select(decType => (CrOwnerFilterType)Enum.Parse(typeof(CrOwnerFilterType), decType.GetAs<string>("Id")))
                            .ToList();
                }

                if (baseParams.Params.ContainsKey("showAllGroups"))
                {
                    this.ShowInAllGroups = baseParams.Params.GetAs<bool>("showAllGroups");
                }

                if (baseParams.Params.ContainsKey("groupIds") && baseParams.Params.GetAs<string>("groupIds").IsNotEmpty())
                {
                    this.GroupIds = baseParams.Params.GetAs<List<long>>("groupIds");
                }

                this.DeliveryAgentIds = baseParams.Params.GetAs<long[]>("deliveryAgentIds");
                this.DeliveryAgentShowAll = baseParams.Params.GetAs<bool>("deliveryAgentShowAll");
                this.HasNotDeliveryAgent = this.DeliveryAgentIds.IsNotEmpty() && this.DeliveryAgentIds.Contains(-1);
                if (this.HasNotDeliveryAgent)
                {
                    this.DeliveryAgentIds = this.DeliveryAgentIds.Where(x => x != -1).ToArray();
                }


                this.OwnershipTypeValues = baseParams.Params.GetAs<RoomOwnershipType[]>("ownershipTypeValues");

                this.HasChargesValues = baseParams.Params.GetAs<AccountFilterHasCharges[]>("hasChargesValues");
                if (this.HasChargesValues.IsNotEmpty())
                {
                    this.HasBaseTariffCharge = this.HasChargesValues.Contains(AccountFilterHasCharges.BaseTariffCharge);
                    this.HasDecisionTariffCharge = this.HasChargesValues.Contains(AccountFilterHasCharges.DecisionTariffCharge);
                    this.HasPenaltyCharge = this.HasChargesValues.Contains(AccountFilterHasCharges.PenaltyCharge);
                    this.HasPenaltyZeroCharge = this.HasChargesValues.Contains(AccountFilterHasCharges.PenaltyZeroCharge);
                    this.HasOverpayment = this.HasChargesValues.Contains(AccountFilterHasCharges.Overpayment);
                    this.HasBaseTariffZeroCharge = this.HasChargesValues.Contains(AccountFilterHasCharges.BaseTariffZeroCharge);
                    this.HasBaseTariffOverpayment = this.HasChargesValues.Contains(AccountFilterHasCharges.BaseTariffOverpayment);
                    this.HasDecisionTariffOverpayment = this.HasChargesValues.Contains(AccountFilterHasCharges.DecisionTariffOverpayment);
                    this.HasPenaltyOverpayment = this.HasChargesValues.Contains(AccountFilterHasCharges.PenaltyOverpayment);
                    this.HasBaseTariffDebt = this.HasChargesValues.Contains(AccountFilterHasCharges.BaseTariffDebt);
                    this.HasDecisionDebt = this.HasChargesValues.Contains(AccountFilterHasCharges.DecisionDebt);
                    this.HasPenaltyDebt = this.HasChargesValues.Contains(AccountFilterHasCharges.PenaltyDebt);
                }

                this.AccountRegistryMode = baseParams.Params.GetAs<AccountRegistryMode>("mode");
            }

            public bool FilterByPeriod { get; private set; }

            public bool OwnerIdSpecified { get; private set; }

            public long RoId { get; private set; }

            public long PeriodId { get; private set; }

            public bool RoIdSpecified { get; private set; }

            public bool BankAccNumSpecified { get; private set; }

            public LoadParam LoadParam { get; set; }

            public long OwnerId { get; private set; }

            public bool FromOwner { get; private set; }

            public long CashPaymentCenterId { get; private set; }

            public bool ShowAll { get; private set; }

            public PrivilegedCategoryFilter PrivilegedCategory { get; private set; }

            public List<CrFundFormationType> CrFundTypeList { get; private set; }

            public long RoomId { get; private set; }

            public List<CrOwnerFilterType> CrOwnerTypeList { get; private set; }

            public List<long> GroupIds { get; private set; }

            public bool ShowInAllGroups { get; private set; }

            public long[] DeliveryAgentIds { get; private set; }

            public bool DeliveryAgentShowAll { get; private set; }

            public bool HasNotDeliveryAgent { get; private set; }

            public RoomOwnershipType[] OwnershipTypeValues { get; private set; }

            public AccountFilterHasCharges[] HasChargesValues { get; private set; }

            public bool HasBaseTariffCharge { get; private set; }

            public bool HasDecisionTariffCharge { get; private set; }

            public bool HasPenaltyCharge { get; private set; }

            public bool HasPenaltyZeroCharge { get; private set; }

            public bool HasOverpayment { get; private set; }

            public bool HasBaseTariffZeroCharge { get; private set; }

            public bool HasBaseTariffOverpayment { get; private set; }

            public bool HasDecisionTariffOverpayment { get; private set; }

            public bool HasPenaltyOverpayment { get; private set; }

            public bool HasBaseTariffDebt { get; private set; }

            public bool HasDecisionDebt { get; private set; }

            public bool HasPenaltyDebt { get; private set; }

            public AccountRegistryMode AccountRegistryMode { get; private set; }
        }

        #region Cache
        private List<long> GetUserMuIds()
        {
            return this._userMuIds ?? (this._userMuIds = this.UserManager.GetMunicipalityIds());
        }

        /// <summary>
        /// Данный метод забирает всю фильтрацию которая делается в реестре Фильтры в компонентах,
        /// с учетом настроек адреса для получения платежек и получает IQueryable
        /// </summary>
        /// <param name="baseParams">Входные параметры</param>
        /// <param name="queryable">Запрос</param>
        /// <returns>Отфильтрованный запрос</returns>
        public IQueryable<BillingPersonalAccountDto> GetQueryableByFiltersAndBillingAddress(BaseParams baseParams, IQueryable<BasePersonalAccount> queryable)
        {
            var loadParams = baseParams.GetLoadParam();
            var addressFilteredIds = AccountService.GetAccountIdsByAddress(loadParams);

            return queryable
                .WhereIfContains(addressFilteredIds != null, x => x.Id, addressFilteredIds) //фильтр по адресу
                .ToBillingDto(baseParams) //уплющивание
                .FilterByBaseParams(baseParams, Container)
                .Filter(loadParams, Container);
        }

        /// <summary>
        /// Данный метод забирает всю фильтрацию которая делается в реестре Фильтры в компонентах, и получает IQueryable
        /// </summary>
        public IQueryable<BasePersonalAccountDto> GetQueryableByFilters(BaseParams baseParams, IQueryable<BasePersonalAccount> queryable)
        {
            var loadParams = baseParams.GetLoadParam();

            return queryable
                .ToBasePersonalAccountDto(baseParams)
                .FilterByBaseParams(baseParams, this.Container)
                .Filter(loadParams, this.Container);
        }

        /// <summary>
        /// Возвращает лицевые счета которые состоят в системных группах "Сформирован документ в открытом периоде"
        /// </summary>
        /// <returns>Массив идентификаторов</returns>
        private IQueryable<long> GetPersonalAccountsIdInSystemGroups()
        {
            var systemGroupsId = this.PaGroupService
                   .GetAll()
                   .Where(x => x.Name.StartsWith("Сформирован документ в открытом периоде"))
                   .Select(x => x.Id);

            return systemGroupsId.IsNullOrEmpty() ?
                null :
                this.PersAccGroupRelationDomain.GetAll().WhereContains(x => x.Group.Id, systemGroupsId).Select(x => x.PersonalAccount.Id);
        }

        // муниципальные образования текущего юзера
        private List<long> _userMuIds;

        /// <summary>
        /// Вернуть дома с решениями отфильтрованными по способам формирования фонда КР
        /// </summary>
        /// <param name="crFundTypeList">Список способов формирования фонда КР</param>
        /// <returns>Идентификаторы домов</returns>
        public List<long> GetDecisionFilteredRoCrFundTypeList(List<CrFundFormationType> crFundTypeList)
        {
            var result = new List<long>();
            foreach (var crFundType in crFundTypeList)
            {
                result.AddRange(this.GetDecisionFilteredRoCrFundType(crFundType));
            }

            return result.Distinct().ToList();
        }

        /// <summary>
        /// Фильтрация решения о формировании фонда КР
        /// </summary>
        /// <param name="crFundType">Решение о формировании фонда КР</param>
        /// <returns>Список результата</returns>
        public List<long> GetDecisionFilteredRoCrFundType(CrFundFormationType crFundType)
        {
            switch (crFundType)
            {
                // Не выбран - выводить те ЛС, к которых не выбран способ формирования фонда на данный момент
                case CrFundFormationType.NotSelected:
                    return this.GetRoWithoutProtocols();

                // Специальный счет - вторая часть фильтра Специальный счет, по фильтру выводить только те ЛС,
                // у которых действующий способ формирования фонда на специальном счете и
                // владелец НЕ региональный оператор
                case CrFundFormationType.SpecialAccount:
                    return this.GetRoAccountOwnerType(AccountOwnerDecisionType.Custom).ToList();
                default:
                    var decType = (CrFundFormationDecisionType)Enum.Parse(typeof(CrFundFormationDecisionType), ((int)crFundType).ToString());
                    var regopSpecAccRo = this.GetRoAccountOwnerType(AccountOwnerDecisionType.Custom);
                    return this.GetRoCrFundType(decType).Where(x => !regopSpecAccRo.Contains(x)).ToList(); // убираем дома со спец счетом дома
            }
        }

        private List<long> GetRoWithoutProtocols()
        {
            var roRepo = this.Container.ResolveRepository<RealityObject>();
            var roDecisionsService = this.Container.Resolve<IRealityObjectDecisionsService>();
            var roList = roRepo.GetAll().ToList();
            var crRoIds = roDecisionsService.GetActualDecisionForCollection<CrFundFormationDecision>(roList, true).Select(kv => kv.Key.Id);
            var accOwnerIds = roDecisionsService.GetActualDecisionForCollection<AccountOwnerDecision>(roList, true).Select(kv => kv.Key.Id);

            var period = this.Container.Resolve<IChargePeriodRepository>().GetCurrentPeriod();
            var govRoIds = this.GovDecisionDomain.GetAll()
                .Where(x => x.State.FinalState && x.DateStart <= period.StartDate && x.FundFormationByRegop)
                .Select(x => x.RealityObject.Id)
                .ToList();

            return roList.Select(ro => ro.Id).Except(crRoIds).Except(accOwnerIds).Except(govRoIds).Distinct().ToList();
        }

        private Dictionary<RealityObject, CrFundFormationDecision> GetGovRoIds()
        {
            var roRepo = this.Container.ResolveRepository<RealityObject>();
            var period = this.Container.Resolve<IChargePeriodRepository>().GetCurrentPeriod();

            // получаем решения для домов
            var dictRo = this.RobjectDecisionService.GetActualDecisionForCollection<CrFundFormationDecision>(roRepo.GetAll(), true);

            // получаем дома с решениями о переходе на спецсчёт, где владелец спецсчёта дом
            var roIdsWhereRoOwner =
                this.RobjectDecisionService.GetActualDecisionForCollection<AccountOwnerDecision>(dictRo.Keys, true)
                    .Where(x => x.Value != null && x.Value.DecisionType == AccountOwnerDecisionType.Custom)
                    .Select(x => x.Key)
                    .ToArray();

            // убираем дома, где владелец спецсчёта - дом
            foreach (var realityObject in roIdsWhereRoOwner)
            {
                dictRo.Remove(realityObject);
            }

            var govProtocols =
                this.GovDecisionDomain.GetAll()
                    .Where(x => x.State.FinalState && x.DateStart <= period.StartDate && x.FundFormationByRegop)
                    .Select(
                        x =>
                            new
                            {
                                Ro = x.RealityObject,
                                CrFundFormationDecisionType = CrFundFormationDecisionType.RegOpAccount,
                                x.ProtocolDate
                            })
                    .ToList();

            foreach (var govProtocol in govProtocols)
            {
                if (dictRo.ContainsKey(govProtocol.Ro))
                {
                    var dec = dictRo[govProtocol.Ro];

                    if (dec.Protocol != null && dec.Protocol.ProtocolDate < govProtocol.ProtocolDate)
                    {
                        dec.Protocol.ProtocolDate = govProtocol.ProtocolDate;
                        dec.Decision = govProtocol.CrFundFormationDecisionType;
                    }
                }
                else
                {
                    var protocol = new RealityObjectDecisionProtocol { ProtocolDate = govProtocol.ProtocolDate };
                    var decision = new CrFundFormationDecision { Decision = govProtocol.CrFundFormationDecisionType, Protocol = protocol };
                    dictRo.Add(govProtocol.Ro, decision);
                }
            }

            return dictRo;
        }

        private List<long> GetRoCrFundType(CrFundFormationDecisionType crFundType)
        {
            var roRepo = this.Container.ResolveRepository<RealityObject>();
            var dictRo = this.GetGovRoIds();

            var listRoIds = dictRo.Where(kvp => kvp.Value != null && kvp.Value.Decision == crFundType).Select(kvp => kvp.Key.Id).ToList();

            if (crFundType == CrFundFormationDecisionType.Unknown)
            {
                var roWithoutDecision = this.GetRoWithoutProtocols();
                listRoIds.AddRange(roRepo.GetAll().Where(x => roWithoutDecision.Contains(x.Id)).Select(x => x.Id));
            }

            return listRoIds;
        }

        private long[] GetRoAccountOwnerType(AccountOwnerDecisionType ownerDecision)
        {
            var roDecisionsService = this.Container.Resolve<IRealityObjectDecisionsService>();
            var roDomain = this.Container.ResolveDomain<RealityObject>();

            // получаем решения для домов
            var dictRo = roDecisionsService.GetActualDecisionForCollection<AccountOwnerDecision>(roDomain.GetAll(), true);

            return dictRo.Where(x => x.Value != null && x.Value.DecisionType == ownerDecision).Select(x => x.Key.Id).ToArray();
        }

       
        /// <summary>
        /// Возвращает дома, с состоянием "Аварийный" или "Разрушенный"
        /// </summary>
        /// <returns>Массив идентификаторов</returns>
        private long[] GetActualRoIdsByConditionHouse()
        {
            return
                this.RealityObjDomainService.GetAll()
                    .Where(x => x.ConditionHouse == ConditionHouse.Emergency || x.ConditionHouse == ConditionHouse.Razed)
                    .Select(x => x.Id)
                    .ToArray();
        }

        #endregion Cache
    }

    /// <summary>
    /// Фильтр по льготной категории
    /// </summary>
    internal class PrivilegedCategoryFilter
    {
        /// <summary>
        /// .ctor
        /// </summary>
        public PrivilegedCategoryFilter(BaseParams baseParams)
        {
            this.IsPrivilegedCategoryAll = baseParams.Params.GetAs<bool>("privilegedCategory.showAll");
            this.PrivilegedCategoryIds = baseParams.Params.GetAs<IList<long>>("privilegedCategory.ids");
        }

        /// <summary>
        /// Выбраны все категории
        /// </summary>
        public bool IsPrivilegedCategoryAll { get; private set; }

        /// <summary>
        /// Списко ID категорий
        /// </summary>
        public IList<long> PrivilegedCategoryIds { get; private set; }
    }
}