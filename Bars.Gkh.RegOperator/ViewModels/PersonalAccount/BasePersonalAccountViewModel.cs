namespace Bars.Gkh.RegOperator.ViewModels.PersonalAccount
{
    using System;
    using System.Linq;

    using B4;
    using B4.Utils;
    
    using ConfigSections.RegOperator;
    using DataResult;

    using Domain.Extensions;
    using DomainModelServices;
    using DomainService.PersonalAccount;

    using Entities;
    using Enums;

    using Gkh.Decisions.Nso.Domain;
    using Gkh.Decisions.Nso.Entities.Decisions;
    using Gkh.Domain;
    using Gkh.Domain.CollectionExtensions;
    using Gkh.Enums;
    using Gkh.Enums.Decisions;
    using Gkh.Utils;

    using Bars.Gkh.RegOperator.Distribution;
    using Bars.Gkh.RegOperator.DomainService.RealityObjectAccount;
    using Bars.Gkh.Repositories.ChargePeriod;
    using Bars.Gkh.Utils.Caching;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Extensions;

    using NHibernate.Linq;

    /// <summary>
    /// ViewModel счетов
    /// </summary>
    public class BasePersonalAccountViewModel : BaseViewModel<BasePersonalAccount>, ICacheableViewModel<BasePersonalAccount>
    {
        public IChargePeriodRepository ChargePeriodRepository { get; set; }
        public IRealityObjectDecisionsService UltimateDecisionService { get; set; }
        public IPersonalAccountFilterService AccountFilterService { get; set; }
        public IDomainService<CashPaymentCenterPersAcc> CashPayCenterPersAccDomain { get; set; }
        public IDomainService<CashPaymentCenterRealObj> CashPayCenterRealObjDomain { get; set; }
        public IPersonalAccountService PersonalAccountService { get; set; }
        public IDomainService<PersonalAccountPeriodSummary> PersonalAccountPeriodSummaryDomainService { get; set; }
        public IPerformedWorkDistribution PerfWorkDistribService { get; set; }
        public IBankAccountDataProvider BankAccountDataProvider { get; set; }

        /// <inheritdoc />
        void ICacheableViewModel<BasePersonalAccount>.InvalidateCache()
        {
            CountCacheHelper.InvalidateCache(this.Container, nameof(BasePersonalAccountViewModel) + "List");
        }

        /// <summary>
        /// Получить счёт
        /// </summary>
        /// <param name="domainService">Домен-сервис</param>
        /// <param name="baseParams">Входные параметры</param>
        /// <returns>Счёт</returns>
        public override IDataResult Get(IDomainService<BasePersonalAccount> domainService, BaseParams baseParams)
        {
            var id = baseParams.Params.GetAsId();

            // операция Get возращается по какой-то причине null, поэтому сделал вот так
            var account = domainService.GetAll()
                .Fetch(x => x.AccountOwner)
                .Fetch(x => x.Room)
                .ThenFetch(x => x.RealityObject)
                .ThenFetch(x => x.Municipality)
                .Fetch(x => x.Room)
                .ThenFetch(x => x.RealityObject)
                .ThenFetch(x => x.MoSettlement)
                .FirstOrDefault(x => x.Id == id);

            if (account == null)
            {
                return new BaseDataResult(false, "Не удалось получить лицевой счет");
            }

            var room = account.Room;

            var summaries = account.Summaries.ToList();

            var perfWorkCreditedBalance = summaries.SafeSum(x => x.PerformedWorkChargedBase);

            var ro = room.Return(x => x.RealityObject);
            var muId = ro.Return(x => x.Municipality).Return(x => x.Id);
            var settlId = ro.Return(x => x.MoSettlement).Return(x => (long?) x.Id);

            var tariff = this.PersonalAccountService.GetTariff(ro.Return(x => x.Id), muId, settlId, DateTime.Today, room.Id);

            var cachPaymentCenterConnectionType = this.Container.GetGkhConfig<RegOperatorConfig>().GeneralConfig.CachPaymentCenterConnectionType;

            string casPayCenter;

            // в зависимости от настроек тянем РКЦ через связь ЛС-РКЦ или Дом-РКЦ
            if (cachPaymentCenterConnectionType == CachPaymentCenterConnectionType.ByAccount)
            {
                casPayCenter = this.CashPayCenterPersAccDomain.GetAll()
                    .Where(x => x.DateStart <= DateTime.Now.Date && (!x.DateEnd.HasValue || DateTime.Now.Date <= x.DateEnd))
                    .Where(x => x.PersonalAccount.Id == account.Id)
                    .Select(x => x.CashPaymentCenter.Contragent.Name)
                    .FirstOrDefault();
            }
            else
            {
                casPayCenter = this.CashPayCenterRealObjDomain.GetAll()
                    .Where(x => x.DateStart <= DateTime.Now.Date && (!x.DateEnd.HasValue || DateTime.Now.Date <= x.DateEnd))
                    .Where(x => x.RealityObject.Id == account.Room.RealityObject.Id)
                    .Select(x => x.CashPaymentCenter.Contragent.Name)
                    .FirstOrDefault();
            }

            var restructSum = 0M;

            var perfWorkBalance = this.PerfWorkDistribService.GetPerformedWorkChargeBalance(account.Id);

            var obj = new
            {
                account.Id,
                account.OpenDate,
                account.CloseDate,
                account.PersonalAccountNum,
                account.State,
                FiasAddress = account.Room.RealityObject.FiasAddress.AddressName,
                RoomAddress =
                room.ChamberNum.IsNotEmpty()
                    ? $"{ro.Address}, кв. {room.RoomNum}, ком. {room.ChamberNum}"
                    : $"{ro.Address}, кв. {room.RoomNum}",
                RoomArea = room.Area,
                RealityObject = new {room.RealityObject.Id, room.RealityObject.Address},
                account.ContractNumber,
                account.ContractDate,
                account.ContractSendDate,
                account.ContractDocument,
                account.OwnershipDocumentType,
                account.DocumentNumber,
                account.DocumentRegistrationDate,
                AccountOwner = account.AccountOwner.Id,
                account.AccountOwner.OwnerType,
                OwnerName = account.AccountOwner.Name,
                account.ServiceType,
                account.AreaShare,
                account.PersAccNumExternalSystems,
                Room = room,
                room.ChamberNum,
                OwnershipType = room?.OwnershipType ?? RoomOwnershipType.NotSet,
                account.OwnershipTypeNewLs,
                Tariff = tariff,
                ChargeBalance = (account.TariffChargeBalance + account.DecisionChargeBalance).RegopRoundDecimal(2),
                ChargedBaseTariff =
                summaries.SafeSum(x => x.GetChargedByBaseTariff() + x.BaseTariffChange).RegopRoundDecimal(2),
              //  summaries.SafeSum(x => x.GetChargedByBaseTariff() + x.BaseTariffChange - x.PerformedWorkChargedBase).RegopRoundDecimal(2), по просьбе самохвала не учитываем займы
                ChargedDecisionTariff =
                summaries.SafeSum(x => x.GetChargedByDecisionTariff() + x.DecisionTariffChange - x.PerformedWorkChargedDecision).RegopRoundDecimal(2),
                ChargedPenalty = summaries.SafeSum(x => x.Penalty + x.RecalcByPenalty + x.PenaltyChange).RegopRoundDecimal(2),
                TotalCharge = summaries.SafeSum(x => x.GetTotalCharge() + x.GetTotalChange() - x.GetTotalPerformedWorkCharge()).RegopRoundDecimal(2),

                PaymentBaseTariff = summaries.SafeSum(x => x.TariffPayment).RegopRoundDecimal(2),
                PaymentDecisionTariff = summaries.SafeSum(x => x.TariffDecisionPayment).RegopRoundDecimal(2),
                PaymentPenalty = summaries.SafeSum(x => x.PenaltyPayment).RegopRoundDecimal(2),
                TotalPayment = summaries.SafeSum(x => x.GetTotalPayment()).RegopRoundDecimal(2),

                DebtBaseTariff = summaries.SafeSum(x => x.GetBaseTariffDebt() - x.PerformedWorkChargedBase).RegopRoundDecimal(2),
                DebtDecisionTariff = summaries.SafeSum(x => x.GetDecisionTariffDebt() - x.PerformedWorkChargedDecision).RegopRoundDecimal(2),
                DebtPenalty = summaries.SafeSum(x => x.GetPenaltyDebt()).RegopRoundDecimal(2),
                TotalDebt =
                    (summaries.SafeSum(x => x.GetTotalCharge() + x.GetTotalChange() - x.GetTotalPayment() - x.GetTotalPerformedWorkCharge()) - restructSum)
                        .RegopRoundDecimal(2),

                CashPayCenter = casPayCenter,
                PrivilegedCategoryPercent = account.AccountOwner.PrivilegedCategory != null ? account.AccountOwner.PrivilegedCategory.Percent : 0,
                AccountFormVariant = this.GetAccountFormationVariant(ro).GetDisplayName(),

                PerfWorkChargeBalance = perfWorkBalance,
                PerfWorkCreditedBalance = perfWorkCreditedBalance,
                RoPayAccountNum = this.BankAccountDataProvider.GetBankAccountNumber(ro),
                account.IsNotDebtor,
                account.InstallmentPlan
            };

            return new BaseDataResult(obj);
        }

        /// <summary>
        /// Получить список счетов
        /// </summary>
        /// <param name="domainService">Домен-сервис</param>
        /// <param name="baseParams">Входные параметры</param>
        /// <returns>Список счетов</returns>
        public override IDataResult List(IDomainService<BasePersonalAccount> domainService, BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();
            var curPeriod = this.ChargePeriodRepository.GetCurrentPeriod();
            var fromOwner = baseParams.Params.GetAs<bool>("fromOwner", ignoreCase: true);
            var addSummary = baseParams.Params.GetAs<bool>("summary", ignoreCase: true);
            var periodId = baseParams.Params.GetAs<long>("periodId", ignoreCase: true);
            var returnOwnerTypeAsString = baseParams.Params.GetAs<bool>("returnOwnerTypeAsString", ignoreCase: true);
            var addAnApostrophe = baseParams.Params.GetAs<bool>("addAnApostrophe", ignoreCase: true);

            periodId = periodId == 0 ? curPeriod.Id : periodId;

            var roIds = baseParams.Params.GetAs("roIds", string.Empty).ToLongArray();
            var mode = baseParams.Params.GetAs("mode", AccountRegistryMode.Common);
            var useCurrentPeriod = mode != AccountRegistryMode.PayDoc;

            // шаманим фильтрацию лицевых счетов по номеру
            var accNumFilter = loadParams.FindInComplexFilter("PersonalAccountNum");

            if (accNumFilter != null)
            {
                var value = accNumFilter.Value.ToStr();
                var isStartsWith = value.StartsWith("%");
                var isEndsWith = value.EndsWith("%");

                if (isStartsWith && isEndsWith)
                {
                    accNumFilter.Operator = ComplexFilterOperator.icontains;
                    value = value.Trim('%');
                }
                else if (isStartsWith)
                {
                    accNumFilter.Operator = ComplexFilterOperator.startswith;
                    value = value.Trim('%');
                }
                else if (isEndsWith)
                {
                    accNumFilter.Operator = ComplexFilterOperator.endswith;
                    value = value.Trim('%');
                }

                accNumFilter.Value = value;
            }

            var addressFilteredIds = this.PersonalAccountService.GetAccountIdsByAddress(loadParams);

            // Фильтры в реестре вынесены в отдельный метод поскольку Выгрузки, и все действия делаются по отфильтрованному реестру через этот метод
            decimal? tariffValue = null;
            if (this.GetTariffFilter(loadParams.ComplexFilter, ref tariffValue))
            {
                loadParams.ComplexFilter = null;
            }

            // Фильтры в реестре вынесены в отдельный метод поскольку Выгрузки, и все действия делаются по отфильтрованному реестру через этот метод
            // если есть фильтр по тарифу - то используем менее эффективный алгоритм подгрузки счетов из-за того, что необходима выборка тарифа по счету

            var query = domainService.GetAll()
                .ToDto(true, periodId: periodId, isCurrentPeriod: useCurrentPeriod, joinPeriodSummary: !tariffValue.HasValue)
                .FilterByBaseParams(baseParams, this.AccountFilterService)
                .WhereIf(roIds.Any(), x => roIds.Contains(x.RoId))
                .WhereIfContains(addressFilteredIds != null, x => x.Id, addressFilteredIds)
                .Filter(loadParams, this.Container);

            if (tariffValue.HasValue)
            {
                var outputData = query.Select(x => new
                    {
                        x.Id,
                        x.AccountOwner,
                        x.Settlement,
                        x.Municipality,
                        x.MuId,
                        x.SettleId,
                        x.RoomAddress,
                        x.RoomNum,
                        x.State,
                        x.PersonalAccountNum,
                        x.UnifiedAccountNumber,
                        x.AreaShare,
                        x.OwnerType,
                        x.RoomId,
                        x.OpenDate,
                        x.CloseDate,
                        x.PersAccNumExternalSystems,
                        x.HasCharges,
                        x.RoId,
                        RoomArea = x.Area,
                        RealArea = x.Area * x.AreaShare,
                        x.PrivilegedCategory,
                        x.DigitalReceipt,
                        x.HaveEmail,
                        x.IsNotDebtor
                })
                    .ToList();

                var accIds = outputData.Select(x => x.Id).ToList();
                var pagedAccs = domainService.GetAll().Where(x => accIds.Contains(x.Id));

                // Получение информации о решениях по домам
                var ros = pagedAccs.Select(x => x.Room.RealityObject);
                var decisions =
                    this.UltimateDecisionService.GetActualDecisionForCollection<AccountManagementDecision>(ros, true)
                        .ToDictionary(x => x.Key.Id, y => y.Value);

                var result = outputData
                    .ToList()
                    .Select(x => new
                    {
                        x.Id,
                        x.AccountOwner,
                        x.Settlement,
                        x.Municipality,
                        x.RoomAddress,
                        x.RoomNum,
                        x.State,
                        PersonalAccountNum = addAnApostrophe ? "'" + x.PersonalAccountNum : x.PersonalAccountNum,
                        x.UnifiedAccountNumber,
                        x.AreaShare,
                        x.OwnerType,
                        x.RoomId,
                        x.OpenDate,
                        x.CloseDate,
                        PersAccNumExternalSystems = addAnApostrophe ? "'" + x.PersAccNumExternalSystems : x.PersAccNumExternalSystems,
                        x.HasCharges,
                        AccuralByOwnersDecision = !decisions.Get(x.RoId)
                            .Return(d => d.Decision == AccountManagementType.ByOwners),
                        Tariff = fromOwner && curPeriod != null
                            ? this.PersonalAccountService.GetTariff(x.RoId, x.MuId, x.SettleId, DateTime.Today, x.RoomId)
                            : 0,
                        x.RoomArea,
                        x.RealArea,
                        x.PrivilegedCategory,
                        x.DigitalReceipt,
                        x.HaveEmail,
                        x.IsNotDebtor
                    })
                    .Where(x => x.Tariff == tariffValue.Value)
                    .AsQueryable();

                var totalCount = outputData.Count;

                var pagedData = result.Order(loadParams).Paging(loadParams).ToList();

                return addSummary
                    ? new ListSummaryResult(pagedData, totalCount, new { RealArea = result.Sum(x => x.RealArea) })
                    : new ListDataResult(pagedData, totalCount);
            }
            else
            {
                var outputData = query;
                var totalCount = outputData.GetCountCached(nameof(BasePersonalAccountViewModel) + "List");

                decimal? summary = null;
                if (addSummary)
                {
                    summary = outputData.SafeSum(v => v.Area * v.AreaShare);
                }

                // Поле лицевой счет уникальное, поэтому нам достаточно сортировки только по этому полю
                // А в методе Order из B4 ещё по умолчанию в конце добавляется сортировка по Id, т.к. сортировка
                // по неуникальному полю каждый раз может выдавать разные результаты
                var hasPersAccOrder = loadParams.Order.IsNotEmpty() && loadParams.Order.Any(x => x.Name == "PersonalAccountNum");
                var pagedData = outputData.GkhOrder(loadParams, !hasPersAccOrder).Paging(loadParams).ToList();

                var accIds = pagedData.Select(a => a.Id).ToArray();
                var pagedAccs = domainService.GetAll().Where(x => accIds.Contains(x.Id));

                // Получение информации о решениях по домам
                var ros = pagedAccs.Select(x => x.Room.RealityObject);
                var decisions = this.UltimateDecisionService.GetActualDecisionForCollection<AccountManagementDecision>(ros, true)
                        .ToDictionary(x => x.Key.Id, y => y.Value);

                var result = pagedData
                    .Select(x => new
                    {
                        x.Id,
                        AccountOwner = addAnApostrophe ? "'" + x.AccountOwner : x.AccountOwner,
                        x.Settlement,
                        x.Municipality,
                        x.MuId,
                        x.SettleId,
                        x.RoomAddress,
                        x.RoomNum,
                        x.State,
                        PersonalAccountNum = addAnApostrophe ?  "'" + x.PersonalAccountNum : x.PersonalAccountNum,
                        x.UnifiedAccountNumber,
                        x.AreaShare,
                        OwnerType = returnOwnerTypeAsString ? (dynamic)x.OwnerType.GetDisplayName() : x.OwnerType,
                        x.RoomId,
                        x.OpenDate,
                        x.CloseDate,
                        PersAccNumExternalSystems = addAnApostrophe ? "'" + x.PersAccNumExternalSystems : x.PersAccNumExternalSystems,
                        x.HasCharges,
                        AccuralByOwnersDecision = !decisions.Get(x.RoId)
                            .Return(d => d.Decision == AccountManagementType.ByOwners),
                        Tariff = fromOwner && curPeriod != null
                            ? this.PersonalAccountService.GetTariff(x.RoId, x.MuId, x.SettleId, DateTime.Today, x.RoomId)
                            : 0,
                        RoomArea = x.Area,
                        RealArea = x.Area * x.AreaShare,
                        x.PrivilegedCategory,
                        x.SaldoIn,
                        x.SaldoOut,
                        x.CreditedWithPenalty,
                        x.PaidWithPenalty,
                        x.RecalculationWithPenalty,
                        x.DigitalReceipt,
                        x.HaveEmail,
                        x.IsNotDebtor
                    })
                    .ToList();

                return addSummary
                    ? new ListSummaryResult(result, totalCount, new { summary })
                    : new ListDataResult(result, totalCount);
            }
        }

        /// <summary>
        /// Получение тарифный фильтров
        /// </summary>
        /// <param name="complexFilter">Класс для хранения правил фильтрации</param>
        /// <param name="value">Значение</param>
        /// <returns></returns>
        private bool GetTariffFilter(ComplexFilter complexFilter, ref decimal? value)
        {
            if (value.HasValue)
            {
                return false;
            }

            if (complexFilter == null)
            {
                value = null;
                return false;
            }

            if (complexFilter.Field != null)
            {
                if (complexFilter.Field == "Tariff")
                {
                    value = complexFilter.Value.ToDecimal();
                    return true;
                }

                value = null;
                return false;
            }

            if (this.GetTariffFilter(complexFilter.Left, ref value))
            {
                var right = complexFilter.Right;
                complexFilter.Operator = right.Operator;
                complexFilter.Field = right.Field;
                complexFilter.Value = right.Value;
                complexFilter.Left = right.Left;
                complexFilter.Right = right.Right;
                return false;
            }

            if (this.GetTariffFilter(complexFilter.Right, ref value))
            {
                var left = complexFilter.Left;
                complexFilter.Operator = left.Operator;
                complexFilter.Field = left.Field;
                complexFilter.Value = left.Value;
                complexFilter.Left = left.Left;
                complexFilter.Right = left.Right;
                return false;
            }

            return false;
        }

        private CrFundFormationType GetAccountFormationVariant(RealityObject ro)
        {
            return ro.AccountFormationVariant ?? CrFundFormationType.NotSelected;
        }
        
        /// <summary>
        /// Временное класс
        /// </summary>
        public class ChargeProxy
        {
            /// <summary>
            /// Штраф
            /// </summary>
            public decimal Penalty { get; set; }

            /// <summary>
            /// Пересчёт
            /// </summary>
            public decimal Recalc { get; set; }

            /// <summary>
            /// Тариф
            /// </summary>
            public decimal ChargeTariff { get; set; }
        }
    }
}