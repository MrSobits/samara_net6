namespace Bars.Gkh.RegOperator.DomainService.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using B4;
    using B4.DataAccess;
    using B4.Modules.States;
    using B4.Utils;
    using Bars.B4.IoC;
    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Decisions.Nso.Entities.Decisions;
    using Bars.Gkh.DomainEvent.Infrastructure;
    using Bars.Gkh.Modules.RegOperator.DomainService;
    using Bars.Gkh.RegOperator.Domain.Repository;
    using Bars.Gkh.RegOperator.DomainModelServices.MassUpdater;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount;
    using Bars.Gkh.Repositories.ChargePeriod;
    using Bars.Gkh.Utils;

    using Castle.Windsor;
    using DataResult;
    using Decisions.Nso.Domain;
    using Decisions.Nso.Entities;
    using Entities;
    using Enums;
    using Gkh.Domain;
    using Gkh.Domain.CollectionExtensions;
    using Gkh.DomainService;
    using Gkh.Entities;
    using Gkh.Enums;
    using Gkh.Enums.Decisions;
    using PersonalAccount;
    using DomainEvent;
    using DomainEvent.Events.PersonalAccountDto;

    using NHibernate.Linq;

    /// <summary>
    /// Сервис для Специальных расчетныйx счетов
    /// </summary>
    public class SpecialCalcAccountService : ISpecialCalcAccountService
    {
        private readonly IWindsorContainer container;
        private readonly IDomainService<SpecialCalcAccount> specaccDomain;
        private readonly IDomainService<CalcAccountRealityObject> calcroDomain;
        private readonly IRegopService regopService;
        private readonly IFileManager fileManager;
        private readonly IDomainService<RealityObjectDecisionProtocol> roDecProtocolService;
        private readonly IDomainService<AccountOwnerDecision> accOwnDecService;
        private readonly IDomainService<CrFundFormationDecision> crFundFormDecService;
        private readonly IDomainService<ChargePeriod> chargePeriodService;
        private readonly IDomainService<PaymentCrSpecAccNotRegop> paymCrSpecAccNotRegopService;
        private readonly IDomainService<RealityObject> realityObjectService;
        private readonly IDomainService<FileInfo> fileInfoService;
        private readonly IDomainService<CrFundFormationDecision> crfundDecisionDomain;
        private readonly IDomainService<AccountOwnerDecision> accountOwnerDecisionDomain;
        private readonly IDomainService<GovDecision> govDecisionDomain;
        private readonly IGovDecisionAccountService govDecService;
        private readonly ISessionProvider sessionProvider;
        private readonly IChargePeriodRepository periodRepository;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="container"></param>
        /// <param name="specaccDomain">Специальный расчетный счет</param>
        /// <param name="calcroDomain">Жилой дом расчетного счета</param>
        /// <param name="regopService"></param>
        /// <param name="roDecProtocolService">Протокол решений собственников</param>
        /// <param name="accOwnDecService">Решение о владельце счета</param>
        /// <param name="crFundFormDecService">Решение о формировании фонда КР</param>
        /// <param name="chargePeriodService">Период начислений</param>
        /// <param name="paymCrSpecAccNotRegopService"> Взнос на капремонт дома, у которого в протоколе решений</param>
        /// <param name="realityObjectService">Жилой дом</param>
        /// <param name="fileManager"></param>
        /// <param name="fileInfoService">Описание файла</param>
        /// <param name="crfundDecisionDomain">Решение о формировании фонда КР</param>
        /// <param name="accountOwnerDecisionDomain">Решение о владельце счета</param>
        /// <param name="govDecisionDomain">Протокол решения органа государственной власти</param>
        /// <param name="govDecService">Сервис "Протокол решения органа государственной власти"</param>
        /// <param name="sessionProvider"></param>
        /// <param name="periodRepository"></param>
        public SpecialCalcAccountService(IWindsorContainer container,
            IDomainService<SpecialCalcAccount> specaccDomain,
            IDomainService<CalcAccountRealityObject> calcroDomain,
            IRegopService regopService,
            IDomainService<RealityObjectDecisionProtocol> roDecProtocolService,
            IDomainService<AccountOwnerDecision> accOwnDecService,
            IDomainService<CrFundFormationDecision> crFundFormDecService,
            IDomainService<ChargePeriod> chargePeriodService,
            IDomainService<PaymentCrSpecAccNotRegop> paymCrSpecAccNotRegopService,
            IDomainService<RealityObject> realityObjectService,
            IFileManager fileManager,
            IDomainService<FileInfo> fileInfoService,
            IDomainService<CrFundFormationDecision> crfundDecisionDomain,
            IDomainService<AccountOwnerDecision> accountOwnerDecisionDomain,
            IDomainService<GovDecision> govDecisionDomain,
            IGovDecisionAccountService govDecService,
            ISessionProvider sessionProvider,
            IChargePeriodRepository periodRepository)
        {
            this.container = container;
            this.specaccDomain = specaccDomain;
            this.calcroDomain = calcroDomain;
            this.regopService = regopService;
            this.roDecProtocolService = roDecProtocolService;
            this.accOwnDecService = accOwnDecService;
            this.crFundFormDecService = crFundFormDecService;
            this.chargePeriodService = chargePeriodService;
            this.paymCrSpecAccNotRegopService = paymCrSpecAccNotRegopService;
            this.realityObjectService = realityObjectService;
            this.fileManager = fileManager;
            this.fileInfoService = fileInfoService;
            this.crfundDecisionDomain = crfundDecisionDomain;
            this.accountOwnerDecisionDomain = accountOwnerDecisionDomain;
            this.govDecisionDomain = govDecisionDomain;
            this.govDecService = govDecService;
            this.sessionProvider = sessionProvider;
            this.periodRepository = periodRepository;
        }

        /// <summary>
        /// EditPaymentCrSpecAccNotRegop
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        public IDataResult EditPaymentCrSpecAccNotRegop(BaseParams baseParams)
        {
            var record = baseParams.Params.GetAs<List<ProxyPaymentCrSpecAccNotRegop>>("records").First();

            var newPaymentCrSpecAccNotRegop = new PaymentCrSpecAccNotRegop
            {
                RealityObject = this.realityObjectService.Load(record.Id),
                Period = this.chargePeriodService.Load(record.Period),
                InputDate = DateTime.Now,
                AmountIncome = record.AmountIncome,
                EndYearBalance = record.EndYearBalance
            };
            var file = baseParams.Files.ContainsKey("File") ? baseParams.Files["File"] : null;
            if (file != null)
            {
                var fileInfo = this.fileManager.SaveFile(file);
                this.fileInfoService.Save(fileInfo);
                newPaymentCrSpecAccNotRegop.File = fileInfo;
            }
            var paymentCrSpecAccNotRegop = this.paymCrSpecAccNotRegopService.GetAll()
                    .Where(x => x.RealityObject.Id == newPaymentCrSpecAccNotRegop.RealityObject.Id)
                    .FirstOrDefault(x => x.Period.Id == newPaymentCrSpecAccNotRegop.Period.Id);

            if (paymentCrSpecAccNotRegop != null)
            {
                paymentCrSpecAccNotRegop.RealityObject = newPaymentCrSpecAccNotRegop.RealityObject;
                paymentCrSpecAccNotRegop.Period = newPaymentCrSpecAccNotRegop.Period;
                paymentCrSpecAccNotRegop.InputDate = newPaymentCrSpecAccNotRegop.InputDate;
                paymentCrSpecAccNotRegop.AmountIncome = newPaymentCrSpecAccNotRegop.AmountIncome;
                paymentCrSpecAccNotRegop.EndYearBalance = newPaymentCrSpecAccNotRegop.EndYearBalance;
                paymentCrSpecAccNotRegop.File = newPaymentCrSpecAccNotRegop.File;
                this.paymCrSpecAccNotRegopService.Update(paymentCrSpecAccNotRegop);
            }
            else
            {
                this.paymCrSpecAccNotRegopService.Save(newPaymentCrSpecAccNotRegop);
            }

            return new BaseDataResult(newPaymentCrSpecAccNotRegop);
        }

        /// <summary>
        /// GetPaymentCrSpecAccNotRegop
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns>IDataResult</returns>
        public IDataResult GetPaymentCrSpecAccNotRegop(BaseParams baseParams)
        {
            var roId = baseParams.Params.GetAs<long>("id");
            var periodId = baseParams.Params.GetAs<long>("periodId");
            var paymCrSpecAccNotRegop = this.paymCrSpecAccNotRegopService.GetAll()
                    .FirstOrDefault(x => x.RealityObject.Id == roId && x.Period.Id == periodId);

            var list = new List<ProxyPaymentCrSpecAccNotRegop>();
            var proxy = new ProxyPaymentCrSpecAccNotRegop
            {
                Id = roId,
                Period = periodId,
                Address = this.realityObjectService.Get(roId).Return(x => x.Address),
                InputDate = paymCrSpecAccNotRegop != null
                    ? paymCrSpecAccNotRegop.InputDate
                    : null,
                AmountIncome = paymCrSpecAccNotRegop != null
                    ? paymCrSpecAccNotRegop.AmountIncome
                    : null,
                EndYearBalance = paymCrSpecAccNotRegop != null
                    ? paymCrSpecAccNotRegop.EndYearBalance
                    : null,
                File = paymCrSpecAccNotRegop != null
                    ? paymCrSpecAccNotRegop.File
                    : null
            };
            list.Add(proxy);

            return new BaseDataResult(list);
        }

        /// <summary>
        /// ListPaymentCrSpecAccNotRegop
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns>IDataResult</returns>
        public IDataResult ListPaymentCrSpecAccNotRegop(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();

            var periodId = loadParams.Filter.GetAsId("periodId");
            var endPeriodDate = DateTime.MaxValue;

            var period = this.chargePeriodService.Get(periodId);
            if (period != null)
            {
                endPeriodDate = period.EndDate.HasValue
                    ? period.EndDate.Value
                    : period.StartDate.AddMonths(1).AddDays(-1);
            }

            var crFundFormDecQuery = this.crFundFormDecService.GetAll()
                    .Where(x => x.Protocol.State.FinalState)
                    .Where(x => x.Decision == CrFundFormationDecisionType.SpecialAccount);

            var accOwnDecQuery = this.accOwnDecService.GetAll()
                    .Where(x => x.Protocol.State.FinalState)
                    .Where(x => x.DecisionType != AccountOwnerDecisionType.RegOp);

            var protocolQuery = this.roDecProtocolService.GetAll()
                    .Where(x => crFundFormDecQuery.Any(y => y.Protocol.Id == x.Id))
                    .Where(x => accOwnDecQuery.Any(y => y.Protocol.Id == x.Id))
                    .Where(x => x.ProtocolDate <= endPeriodDate)
                    .OrderByDescending(x => x.ProtocolDate);

            var paymCrSpecAccNotRegopByRoIdDict = this.paymCrSpecAccNotRegopService.GetAll()
                    .Where(x => x.Period.Id == periodId)
                    .Where(x => protocolQuery.Any(y => y.RealityObject.Id == x.RealityObject.Id))
                    .ToDictionary(x => x.RealityObject.Id);

            var data =
                protocolQuery
                    .Select(x => new
                    {
                        RoId = x.RealityObject.Id,
                        Municipality = x.RealityObject.Municipality.Name,
                        Settlement = x.RealityObject.MoSettlement.Name,
                        x.RealityObject.Address,
                        x.ProtocolDate
                    })
                    .AsEnumerable()
                    .Select(x => new
                    {
                        Id = x.RoId,
                        Period = periodId,
                        x.Municipality,
                        x.Settlement,
                        x.Address,
                        InputDate = paymCrSpecAccNotRegopByRoIdDict.Get(x.RoId).Return(y => y.InputDate),
                        AmountIncome = paymCrSpecAccNotRegopByRoIdDict.Get(x.RoId).Return(y => y.AmountIncome),
                        EndYearBalance = paymCrSpecAccNotRegopByRoIdDict.Get(x.RoId).Return(y => y.EndYearBalance),
                        File = paymCrSpecAccNotRegopByRoIdDict.Get(x.RoId).Return(y => y.File),
                        x.ProtocolDate
                    })
                    .DistinctBy(x => x.Id)
                    .AsQueryable()
                    .Filter(loadParams, this.container);

            var totalCount = data.Count();

            data = data
                .OrderIf(loadParams.Order.Length == 0, true, x => x.Municipality)
                .OrderThenIf(loadParams.Order.Length == 0, true, x => x.Settlement)
                .OrderThenIf(loadParams.Order.Length == 0, true, x => x.Address);

            var result = loadParams.Order.Length == 0 ? data.Paging(loadParams) : data.Order(loadParams).Paging(loadParams);

            return new ListDataResult(result.ToList(), totalCount);
        }

        /// <summary>
        /// ListRegister
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns>IDataResult</returns>
        public IDataResult ListRegister(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();

            var paymentAccDomain = this.container.ResolveDomain<RealityObjectPaymentAccount>();
            var chaaccroDomain = this.container.ResolveDomain<RealityObjectChargeAccount>();
            var chaccoperDomain = this.container.ResolveDomain<RealityObjectChargeAccountOperation>();

            var showAll = baseParams.Params.GetAs<bool>("showall");

            var accroQuery = this.calcroDomain.GetAll()
                .Where(x => x.Account.TypeAccount == TypeCalcAccount.Special)
                .WhereIf(!showAll, x => x.Account.TypeOwner != TypeOwnerCalcAccount.Manorg)
                .Where(x => x.DateStart <= DateTime.Today && (!x.DateEnd.HasValue || x.DateEnd >= DateTime.Today));

            var dictOwners = accroQuery
                .Select(x => new
                {
                    x.Account.Id,
                    x.RealityObject.Address,
                    Municipality = x.RealityObject.Municipality.Name,
                    RealityObjectId = x.RealityObject.Id
                })
                .AsEnumerable()
                .GroupBy(x => x.Id)
                .ToDictionary(x => x.Key, y => y.Select(x => new { x.Address, x.Municipality, x.RealityObjectId }).First());

            var dictChargeMoney = chaaccroDomain.GetAll()
                .Join(this.calcroDomain.GetAll(),
                    x => x.RealityObject.Id,
                    x => x.RealityObject.Id,
                    (characc, accro) => new
                    {
                        AccountId = accro.Account.Id,
                        characc.PaidTotal,
                        ChargeTotal = characc.Operations.Any() ? characc.Operations.Sum(y => y.ChargedTotal) : 0m,
                        Debt = (characc.Operations.Any() ? characc.Operations.Sum(y => y.ChargedTotal) : 0m) - characc.PaidTotal
                    })
                .AsEnumerable()
                .GroupBy(x => x.AccountId)
                .ToDictionary(x => x.Key, y => new
                {
                    PaidTotal = y.Sum(x => x.PaidTotal),
                    ChargeTotal = y.Sum(x => x.ChargeTotal),
                    Debt = y.Sum(x => x.Debt)
                });

            var paidTotal =
                chaccoperDomain.GetAll()
                    .Select(x => new { x.PaidTotal, x.PaidPenalty, x.Account.RealityObject.Id })
                    .ToList()
                    .GroupBy(x => x.Id)
                    .ToDictionary(x => x.Key, y => y.Sum(x => (decimal?)(x.PaidPenalty + x.PaidTotal)) ?? 0m);

            var paymentAccounts =
                paymentAccDomain.GetAll()
                    .Select(x => new
                    {
                        x.RealityObject.Id,
                        x.DebtTotal,
                        x.CreditTotal
                    })
                    .ToList()
                    .GroupBy(x => x.Id)
                    .ToDictionary(x => x.Key, y => y.Sum(x => (decimal?)(x.DebtTotal - x.CreditTotal)) ?? 0m);

            var data = this.specaccDomain.GetAll()
                .Where(x => x.IsActive)
                // если не стоит галочка показывать все, показываем только те, у которых владелец регоп
                .WhereIf(!showAll, x => x.TypeOwner != TypeOwnerCalcAccount.Manorg)
                .Select(x => new
                {
                    x.Id,
                    x.AccountNumber,
                    x.TotalIn,
                    x.TotalOut,
                    ContragentCreditOrg = x.CreditOrg.Name,
                    x.DateOpen,
                    x.DateClose,
                    AccountOwner = x.AccountOwner.Name
                })
                .ToList()
                .Select(x => new
                {
                    x.Id,
                    AccountNumber = x.AccountNumber ?? string.Empty,
                    x.ContragentCreditOrg,
                    x.DateOpen,
                    x.DateClose,
                    AccountOwner = x.AccountOwner ?? string.Empty,
                    ChargeTotal = dictChargeMoney.Get(x.Id).Return(y => y.ChargeTotal),
                    Debt = dictChargeMoney.Get(x.Id).Return(y => y.Debt),
                    Address = dictOwners.Get(x.Id).Return(y => y.Address) ?? string.Empty,
                    RealityObjectId = dictOwners.Get(x.Id).Return(y => y.RealityObjectId),
                    Municipality = dictOwners.Get(x.Id).Return(y => y.Municipality) ?? string.Empty
                })
                .Select(x => new
                {
                    x.Id,
                    x.AccountNumber,
                    x.ContragentCreditOrg,
                    x.DateOpen,
                    x.DateClose,
                    x.AccountOwner,
                    Saldo = paymentAccounts.Get(x.RealityObjectId),
                    x.ChargeTotal,
                    PaidTotal = paidTotal.Get(x.RealityObjectId),
                    Debt = x.ChargeTotal - paidTotal.Get(x.RealityObjectId),
                    x.Address,
                    x.RealityObjectId,
                    x.Municipality
                })
                .AsQueryable()
                .Filter(loadParams, this.container);

            return new ListSummaryResult(
                data.Order(loadParams).Paging(loadParams).ToList(),
                data.Count(),
                new
                {
                    ChargeTotal = data.SafeSum(x => x.ChargeTotal),
                    PaidTotal = data.SafeSum(x => x.PaidTotal),
                    Debt = data.SafeSum(x => x.Debt),
                    Saldo = data.SafeSum(x => x.Saldo)
                });
        }

        /// <summary>
        /// GetRobjectActiveSpecialAccount
        /// </summary>
        /// <param name="ro"></param>
        /// <returns>SpecialCalcAccount</returns>
        public SpecialCalcAccount GetRobjectActiveSpecialAccount(RealityObject ro)
        {
            var calcaccService = this.container.Resolve<ICalcAccountService>();
            using (this.container.Using(calcaccService))
            {
                return calcaccService.GetRobjectAccount<SpecialCalcAccount>(ro);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ownerDecision"></param>
        /// <param name="crFundFormDecision"></param>
        /// <param name="govDecision"></param>
        /// <param name="ro"></param>
        public void HandleSpecialAccountByProtocolChange(AccountOwnerDecision ownerDecision, CrFundFormationDecision crFundFormDecision,
            GovDecision govDecision, RealityObject ro)
        {
            var protocolDomain = this.container.ResolveDomain<RealityObjectDecisionProtocol>();

            using (this.container.Using(protocolDomain, this.govDecisionDomain))
            {
                var isExistNewProtocol = false;
                var dateStart = crFundFormDecision != null
                    ? crFundFormDecision.Protocol.DateStart
                    : govDecision.DateStart;

                if (ownerDecision != null
                    && ownerDecision.Protocol != null
                    && protocolDomain.GetAll()
                        .Where(x => x.RealityObject == ro)
                        .Where(x => x.DateStart > dateStart && x.DateStart <= DateTime.Today)
                        .Any(x => x.State.FinalState))
                {
                    isExistNewProtocol = true;
                }

                if (crFundFormDecision != null
                    && crFundFormDecision.Protocol != null
                    && crFundFormDecision.Decision == CrFundFormationDecisionType.RegOpAccount)
                {
                    if (protocolDomain.GetAll()
                        .Where(x => x.RealityObject == ro)
                        .Where(x => x.DateStart > dateStart && x.DateStart <= DateTime.Today)
                        .Any(x => x.State.FinalState))
                    {
                        isExistNewProtocol = true;
                    }
                }

                if (govDecision != null && this.govDecisionDomain.GetAll().Where(x => x.RealityObject == ro && x.FundFormationByRegop)
                    .Where(x => x.DateStart > dateStart && x.DateStart <= DateTime.Today)
                    .Any(x => x.State.FinalState))
                {
                    isExistNewProtocol = true;
                }

                if (isExistNewProtocol)
                {
                    return;
                }
            }

            this.container.InTransaction(() =>
            {
                if (crFundFormDecision != null)
                {
                    var specacc = this.GetSpecialAccount(ro) ?? this.CreateNewSpecialAccount(crFundFormDecision.Protocol);

                    if (crFundFormDecision.Decision == CrFundFormationDecisionType.SpecialAccount)
                    {
                        if (ownerDecision.Return(x => x.DecisionType) == AccountOwnerDecisionType.RegOp)
                        {
                            this.HandleRegOperatorDecision(specacc);
                        }
                        else
                        {
                            this.HandleSpecialAccountDecision(ro, ownerDecision, specacc);
                        }

                        if (specacc != null)
                        {
                            this.CreateRelationIfNeed(ro, specacc);
                        }

                        this.DeleteCalcAccountRealityObject(ro, crFundFormDecision.Protocol.DateStart.AddDays(-1));
                    }
                    else
                    {
                        this.SetNonActive(specacc);
                    }
                }

                if (govDecision != null && govDecision.FundFormationByRegop)
                {
                    var specacc = this.GetSpecialAccount(ro) ?? new SpecialCalcAccount
                    {
                        AccountNumber = string.Empty,
                        DateOpen = govDecision.DateStart,
                        DateClose = null,
                        CreditOrg = null,
                        TypeAccount = TypeCalcAccount.Special
                    };

                    this.SetNonActive(specacc);
                }
            });
        }

        public void SetNonActiveByRealityObject(RealityObject ro)
        {
            var account = this.GetSpecialAccount(ro);
            this.SetNonActive(account);
        }

        public void SetPersonalAccountStatesActiveIfAble(RealityObjectDecisionProtocol protocol, bool deletingCurrentProtocol = false)
        {
            var session = this.sessionProvider.GetCurrentSession();
            if (this.AnySuitableCrFundDecision(protocol, deletingCurrentProtocol))
            {
                return;
            }

            var persAccRepo = this.container.ResolveRepository<BasePersonalAccount>();
            var stateRepo = this.container.ResolveRepository<State>();
            var stateProvider = this.container.Resolve<IStateProvider>();
            var accChangeDomain = this.container.ResolveDomain<PersonalAccountChange>();
            var userManager = this.container.Resolve<IGkhUserManager>();
            var hmaoConfig = this.container.HasComponent<IHouseTypesConfigService>() ? this.container.Resolve<IHouseTypesConfigService>() : null;
            var useBlockedBuilding = hmaoConfig?.GetHouseTypesConfig().UseBlockedBuilding ?? false;

            using (this.container.Using(persAccRepo, stateProvider, stateRepo, hmaoConfig))
            {
                var accOwnerDec = this.GetDecisionByProtocol<AccountOwnerDecision>(protocol);
                var crFundDec = this.GetDecisionByProtocol<CrFundFormationDecision>(protocol);

                if (crFundDec == null)
                {
                    return;
                }

                if (crFundDec.Decision == CrFundFormationDecisionType.SpecialAccount && accOwnerDec == null)
                {
                    return;
                }

                /*
                 * Если (решение  о спец счете И владаелец спец счета  регоп)
                 * ИЛИ (если решение о счете регионального оператора)
                 * то ЛС -> статус Открыт, иначе в не активно 
                 */

                bool toActive =
                    (crFundDec.Decision == CrFundFormationDecisionType.SpecialAccount
                        && accOwnerDec.DecisionType == AccountOwnerDecisionType.RegOp)
                    || crFundDec.Decision == CrFundFormationDecisionType.RegOpAccount;

                if (toActive)
                {
                    // если в доме состояние дома=Аварийный
                    // или в чекбоксе "Дом не участвует в программе КР" стоит галочка
                    // или Тип дома = Блокированной застройки
                    // то счета должны оставаться на статусе "Не активен"
                    if (protocol.RealityObject.IsNotInvolvedCr
                        || protocol.RealityObject.ConditionHouse == ConditionHouse.Emergency
                        || protocol.RealityObject.ConditionHouse == ConditionHouse.Razed
                        || protocol.RealityObject.TypeHouse == TypeHouse.BlockedBuilding && !useBlockedBuilding)
                    {
                        return;
                    }
                }

                const string typeId = "gkh_regop_personal_account";

                var state = stateRepo.GetAll().FirstOrDefault(x => x.Code == (toActive ? "1" : "4") && x.TypeId == typeId);

                if (state == null)
                {
                    return;
                }

                using (var tr = this.container.Resolve<IDataTransaction>())
                {
                    try
                    {
                        persAccRepo.GetAll()
                            .Where(x => x.Room.RealityObject == protocol.RealityObject)
                            .Where(x => toActive ? x.State.Code == "4" : x.State.Code == "1")
                            .ToList()
                            .ForEach(
                                x =>
                                {
                                    if (x.State.Code == (toActive ? "4" : "1"))
                                    {
                                        var useStateProvider = true;

                                        if (state.Code != "4")
                                        {
                                            var existingPortions = persAccRepo.GetAll()
                                                .Where(y => y.Room.Id == x.Room.Id)
                                                .Where(y => y.Id != x.Id)
                                                .Sum(y => (decimal?)y.AreaShare) ?? 0;

                                            if ((existingPortions + x.AreaShare).RegopRoundDecimal(2) > 1)
                                            {
                                                //"Внимание!! Сумма долей собственности по текущему помещению превышает допустимых значений!"

                                                //По идее здесь используется Repository, но внутри StateProvider'a используется DomainService,
                                                //который внутри BasePersonalAccountDomainInterceptor запускает проверку на долю собственности
                                                //Так как здесь использовали Repository, значит видимо подразумевалось, что эта проверка работать не будет
                                                //Чтобы не переопределять StateProvider на использование Repository делаем проверку заранее, чтобы статус протокола переводился нормально,
                                                //без ошибок

                                                useStateProvider = false;

                                                x.State = state;
                                                persAccRepo.Update(x);
                                                DomainEvents.Raise(new BasePersonalAccountDtoEvent(x));
                                            }
                                        }
                                        else
                                        {
                                            accChangeDomain.Save(
                                                new PersonalAccountChange
                                                {
                                                    PersonalAccount = x,
                                                    ChangeType = PersonalAccountChangeType.NonActive,
                                                    Date = DateTime.UtcNow,
                                                    Description = "Для ЛС установлен статус \"Не активен\"",
                                                    Operator = userManager.GetActiveUser().Return(y => y.Login),
                                                    ActualFrom = DateTime.UtcNow,
                                                    NewValue = "Статус ЛС = Не активен",
                                                    Reason = "Смена статуса Протокола решения на \"Черновик\"",
                                                    ChargePeriod = this.periodRepository.GetCurrentPeriod()
                                                });
                                        }

                                        if (useStateProvider)
                                        {
                                            stateProvider.ChangeState(x.Id, typeId, state, "На основе изменения статуса протокола решения", false);
                                        }
                                    }
                                });

                        tr.Commit();
                    }
                    catch
                    {
                        tr.Rollback();
                        throw;
                    }
                    finally
                    {
                        this.container.Release(accChangeDomain);
                        this.container.Release(userManager);
                    }
                }
                session.Evict(accOwnerDec);
                session.Evict(crFundDec);
            }
        }

        public void SetPersonalAccountStatesNonActiveIfNeeded(
            RealityObjectDecisionProtocol protocol,
            bool deletingCurrentProtocol = false,
            bool isRealityObjectChange = false)
        {
            if (this.AnySuitableCrFundDecision(protocol, deletingCurrentProtocol))
            {
                return;
            }

            var persAccRepo = this.container.ResolveRepository<BasePersonalAccount>();
            var decisionService = this.container.Resolve<IRealityObjectDecisionsService>();
            var persAccDomain = this.container.ResolveDomain<BasePersonalAccount>();
            var stateProvider = this.container.Resolve<IStateProvider>();
            var stateRepo = this.container.Resolve<IRepository<State>>();
            var session = this.sessionProvider.GetCurrentSession();

            using (this.container.Using(decisionService, persAccDomain, stateProvider, stateRepo, persAccRepo))
            {
                AccountOwnerDecision accOwnerDec;
                CrFundFormationDecision crFundDec;
                if (isRealityObjectChange)
                {
                    accOwnerDec = decisionService.GetActualDecision<AccountOwnerDecision>(protocol.RealityObject, true);
                    crFundDec = decisionService.GetActualDecision<CrFundFormationDecision>(protocol.RealityObject, true);
                }
                else
                {
                    accOwnerDec = decisionService.GetActualDecision<AccountOwnerDecision>(protocol.RealityObject, true, new[] { protocol });
                    crFundDec = decisionService.GetActualDecision<CrFundFormationDecision>(protocol.RealityObject, true, new[] { protocol });
                }

                var needToChangeStates = crFundDec == null;

                /*
                 * Если решение не о спец счете
                 * или владаелец спец счета не регоп - установим конечный статус
                 */
                if (needToChangeStates
                    || (crFundDec.Decision == CrFundFormationDecisionType.SpecialAccount
                        && accOwnerDec.DecisionType != AccountOwnerDecisionType.RegOp))
                {
                    using (var tr = this.container.Resolve<IDataTransaction>())
                    {
                        try
                        {

                            var type = stateProvider.GetStatefulEntityInfo(typeof(BasePersonalAccount));

                            if (type == null)
                            {
                                return;
                            }

                            var state =
                                stateRepo.GetAll().FirstOrDefault(x => x.Code == "4" && x.TypeId == type.TypeId);

                           var accountsToUpdate = persAccDomain.GetAll()
                                .Fetch(x => x.AccountOwner)
                                .Fetch(x => x.State)
                                .Fetch(x => x.Room)
                                .Where(x => x.Room.RealityObject == protocol.RealityObject)
                                .Where(x => x.State.Id != state.Id)
                                .ToList();

                            var context = accountsToUpdate.Count > 200 ? MassUpdateContext.CreateContext(this.container) : null;
                            try
                            {
                                accountsToUpdate.ForEach(
                                   x =>
                                   {
                                       if (x.State.Code == "1")
                                       {
                                           stateProvider.ChangeState(x.Id, type.TypeId, state, "На основе изменения статуса протокола решения", false);
                                           persAccDomain.Update(x);
                                       }
                                   });
                            }
                            finally
                            {
                                context?.Dispose();
                            }

                            tr.Commit();
                        }
                        catch
                        {
                            tr.Rollback();
                            throw;
                        }
                    }
                }
                session.Evict(accOwnerDec);
                session.Evict(crFundDec);
            }
        }

        /// <summary>
        /// AddPaymentForOpeningAccount
        /// </summary>
        /// <param name="protocol"></param>
        public void AddPaymentForOpeningAccount(RealityObjectDecisionProtocol protocol)
        {
            var accOwnerDec = this.GetDecisionByProtocol<AccountOwnerDecision>(protocol);
            var crFundDec = this.GetDecisionByProtocol<CrFundFormationDecision>(protocol);

            if (crFundDec == null || accOwnerDec == null || crFundDec.Decision != CrFundFormationDecisionType.SpecialAccount || accOwnerDec.DecisionType != AccountOwnerDecisionType.RegOp)
            {
                return;
            }

            if (this.AnySuitableCrFundDecision(protocol))
            {
                return;
            }

            var roAccDomain = this.container.ResolveDomain<RealityObjectPaymentAccount>();
            var creditOrgDecisionDomain = this.container.ResolveDomain<CreditOrgDecision>();
            var creditOrgServCondDomain = this.container.ResolveDomain<CreditOrgServiceCondition>();
            var roAccOperationDomain = this.container.ResolveDomain<RealityObjectPaymentAccountOperation>();
            var regopParamDomain = this.container.ResolveDomain<RegoperatorParam>();

            try
            {
                var isPayForOpenAcc = regopParamDomain.GetAll().FirstOrDefault(x => x.Key == "OpenAccCredit").Return(x => x.Value).ToBool();

                if (isPayForOpenAcc)
                {
                    var realObjPaymAcc =
                        roAccDomain.GetAll().FirstOrDefault(x => x.RealityObject.Id == protocol.RealityObject.Id);
                    var creditOrgId =
                        creditOrgDecisionDomain.GetAll()
                            .FirstOrDefault(x => x.Protocol.Id == protocol.Id)
                            .Return(x => x.Decision.Id);
                    var openingAccPay = creditOrgServCondDomain.GetAll()
                        .Where(x => x.OpeningAccDateFrom <= protocol.ProtocolDate
                                    && (!x.OpeningAccDateTo.HasValue || x.OpeningAccDateTo >= protocol.ProtocolDate))
                        .FirstOrDefault(x => x.CreditOrg.Id == creditOrgId)
                        .Return(x => x.OpeningAccPay);

                    var accOpenAccOperation = new RealityObjectPaymentAccountOperation
                    {
                        Date = DateTime.Now,
                        Account = realObjPaymAcc,
                        OperationSum = openingAccPay,
                        OperationType = PaymentOperationType.OpeningAcc,
                        OperationStatus = OperationStatus.Default
                    };

                    roAccOperationDomain.Save(accOpenAccOperation);
                }
            }
            finally
            {
                this.container.Release(roAccDomain);
                this.container.Release(creditOrgDecisionDomain);
                this.container.Release(creditOrgServCondDomain);
                this.container.Release(roAccOperationDomain);
            }
        }

        /// <summary>
        /// Получить специальный расчетный счет по дому.
        /// </summary>
        /// <param name="robject">
        /// Жилой дом.
        /// </param>
        /// <returns>
        /// Специальный расчетный счет <see cref="SpecialCalcAccount"/>.
        /// </returns>
        public SpecialCalcAccount GetSpecialAccount(RealityObject robject)
        {
            var domain = this.container.ResolveDomain<CalcAccountRealityObject>();

            using (this.container.Using(domain))
            {
                return
                    domain.GetAll()
                        .Where(x => x.Account.TypeAccount == TypeCalcAccount.Special)
                        .Where(x => x.RealityObject.Id == robject.Id)
                        .Select(x => x.Account)
                        .Where(x => x.DateOpen <= DateTime.Today)
                        .Where(x => !x.DateClose.HasValue || (x.DateClose.Value != DateTime.MinValue && x.DateClose.Value >= DateTime.Today))
                        .OrderByDescending(x => x.DateOpen)
                        .FirstOrDefault() as SpecialCalcAccount;
            }
        }

        /// <summary>
        /// ProxyPaymentCrSpecAccNotRegop
        /// </summary>
        public class ProxyPaymentCrSpecAccNotRegop
        {
            /// <summary>
            /// ID
            /// </summary>
            public long Id { get; set; }

            /// <summary>
            /// Период
            /// </summary>
            public long Period { get; set; }

            /// <summary>
            /// Адрес
            /// </summary>
            public string Address { get; set; }

            /// <summary>
            /// Входная Дата
            /// </summary>
            public DateTime? InputDate { get; set; }

            /// <summary>
            /// Сумма дохода
            /// </summary>
            public decimal? AmountIncome { get; set; }

            /// <summary>
            /// Баланс на конец года 
            /// </summary>
            public decimal? EndYearBalance { get; set; }

            /// <summary>
            /// Описание файла
            /// </summary>
            public FileInfo File { get; set; }
        }

        /// <summary>
        /// Производит актуализацию спецсчетов. 
        /// Вызывается при изменении "Дата протокола" или "Дата вступления в силу", 
        ///     т.к в этом случае спецсчета дома на котором висит протокол нуждаются в проверке областей действия
        /// </summary>
        /// <param name="realityObjectId">Идентифкатор дома</param>
        public void ValidateActualSpecAccount(long realityObjectId)
        {
            var crFundDecisionCache = this.crfundDecisionDomain.GetAll()
                .Where(x => x.Protocol.State.FinalState)
                .Where(x => x.Protocol.DateStart <= DateTime.Today)
                .Select(x => new
                {
                    x.Protocol.RealityObject.Id,
                    x.Protocol.DateStart,
                    Decision = x
                })
                .AsEnumerable()
                .GroupBy(x => x.Id)
                .ToDictionary(x => x.Key, z => z.OrderByDescending(x => x.DateStart).Select(x => x.Decision).First());

            var accountOwnerDecisionCache = this.accountOwnerDecisionDomain.GetAll()
                .Where(x => x.Protocol.State.FinalState)
                .Where(x => x.Protocol.DateStart <= DateTime.Today)
                .Select(x => new
                {
                    x.Protocol.RealityObject.Id,
                    x.Protocol.DateStart,
                    Decision = x
                })
                .AsEnumerable()
                .GroupBy(x => x.Id)
                .ToDictionary(x => x.Key, z => z.OrderByDescending(x => x.DateStart).Select(x => x.Decision).First());

            var govDecisionCache = this.govDecisionDomain.GetAll()
                .Where(x => x.State.FinalState)
                .Where(x => x.DateStart <= DateTime.Today)
                .Where(x => x.FundFormationByRegop)
                .Select(x => new
                {
                    x.RealityObject.Id,
                    x.DateStart,
                    Decision = x
                })
                .AsEnumerable()
                .GroupBy(x => x.Id)
                .ToDictionary(x => x.Key, z => z.OrderByDescending(x => x.DateStart).Select(x => x.Decision).First());

            var isRefresh = crFundDecisionCache
                .Select(x => x.Key)
                .Union(govDecisionCache.Select(x => x.Key))
                .Any(x => x == realityObjectId);

            if (!isRefresh)
            {
                return;
            }

            var activeProtocol = this.GetActiveProtocol(realityObjectId, crFundDecisionCache, govDecisionCache);

            var protocol = activeProtocol as GovDecision;
            if (protocol != null)
            {
                this.govDecService.SetPersonalAccountStateIfNeeded(protocol);

                this.HandleSpecialAccountByProtocolChange(
                    null,
                    null,
                    protocol,
                    new RealityObject { Id = realityObjectId });
            }
            else
            {
                var decision = activeProtocol as CrFundFormationDecision;
                if (decision != null)
                {
                    this.HandleSpecialAccountByProtocolChange(
                        accountOwnerDecisionCache.Get(realityObjectId),
                        decision,
                        null,
                        new RealityObject { Id = realityObjectId });
                }
            }
        }

        #region private methods

        private T GetDecisionByProtocol<T>(RealityObjectDecisionProtocol protocol)
            where T : UltimateDecision
        {
            var domain = this.container.ResolveDomain<T>();
            using (this.container.Using(domain))
            {
                return domain.GetAll().FirstOrDefault(x => x.Protocol == protocol);
            }
        }

        /// <summary>
        /// Функция проверяет, если кроме текщуего проверяемого протокола наиболее "свежий" протокол на спец счете
        /// </summary>
        /// <param name="protocol">Текущий проверяемый протокол</param>
        /// <param name="deletingCurrentProtocol">Текущий протокол удаляется. В таком случае его дата протокола не учитывается в фильтре</param>
        private bool AnySuitableCrFundDecision(RealityObjectDecisionProtocol protocol, bool deletingCurrentProtocol = false)
        {
            var crFundFormDomain = this.container.ResolveDomain<CrFundFormationDecision>();

            /*
             * Если есть решение о выборе спец счета такое, что
             * 1) Статус протокола конечный
             * 2) Дата протокола выше, чем дата нового протокола
             * то ничего не делаем
             */
            using (this.container.Using(crFundFormDomain))
            {
                if (crFundFormDomain.GetAll()
                    .Where(x => x.Protocol != null)
                    .Where(x => x.Protocol.Id != protocol.Id)
                    .WhereIf(!deletingCurrentProtocol, x => x.Protocol.DateStart > protocol.DateStart)
                    .Where(x => x.Protocol.RealityObject.Id == protocol.RealityObject.Id)
                    .Where(x => x.Protocol.State != null)
                    .Where(x => x.Protocol.State.FinalState)
                    .Any(x => x.Decision == CrFundFormationDecisionType.SpecialAccount))
                {
                    return true;
                }
            }
            return false;
        }

        private void HandleRegOperatorDecision(SpecialCalcAccount specialAccount)
        {
            this.ChooseRegOp(specialAccount);
        }

        private void DeleteCalcAccountRealityObject(RealityObject ro, DateTime date)
        {
            var calcAccRoDomain = this.container.ResolveDomain<CalcAccountRealityObject>();
            using (this.container.Using(calcAccRoDomain))
            {
                calcAccRoDomain.GetAll()
                    .Where(x => x.RealityObject.Id == ro.Id)
                    .Where(x => x.Account.TypeAccount == TypeCalcAccount.Regoperator)
                    .ToList()
                    .ForEach(x =>
                    {
                        x.DateEnd = date;
                        calcAccRoDomain.Update(x);
                    });
            }
        }


        private void HandleSpecialAccountDecision(RealityObject ro, AccountOwnerDecision ownerDecision, SpecialCalcAccount specialAccount)
        {
            if (ownerDecision == null)
            {
                this.SetNonActive(specialAccount);
            }
            else
            {
                var chooseRegop = ownerDecision.DecisionType == AccountOwnerDecisionType.RegOp;
                this.ChooseContragent(ro, specialAccount, chooseRegop);
            }
        }

        private void ChooseRegOp(SpecialCalcAccount account)
        {
            account.IsActive = true;

            var regop = this.regopService.GetCurrentRegOperator().Return(x => x.Contragent);

            if (regop == null)
            {
                throw new Exception("Нет действующего регоператора");
            }

            account.AccountOwner = regop;
            account.TypeOwner = TypeOwnerCalcAccount.Regoperator;

            this.SaveOrUpdate(account);

        }

        private void ChooseContragent(RealityObject ro, SpecialCalcAccount account, bool chooseRegop)
        {
            account.IsActive = true;
            account.TypeAccount = TypeCalcAccount.Special;

            if (chooseRegop)
            {
                account.TypeOwner = TypeOwnerCalcAccount.Regoperator;

                var regop = this.regopService.GetCurrentRegOperator().Return(x => x.Contragent);

                if (regop == null)
                {
                    throw new ValidationException("Нет действующего регоператора");
                }

                account.AccountOwner = regop;
            }
            else
            {
                account.TypeOwner = TypeOwnerCalcAccount.Manorg;

                var manorg = this.GetCurrentManOrgContract(ro)
                    .Return(x => x.ManagingOrganization)
                    .Return(x => x.Contragent);

                if (manorg == null)
                {
                    throw new ValidationException("Отсутствует активный договор управления");
                }

                account.AccountOwner = manorg;
            }

            this.SaveOrUpdate(account);
        }

        private void SaveOrUpdate<T>(T entity)
            where T : PersistentObject
        {
            var domain = this.container.ResolveDomain<T>();

            using (this.container.Using(domain))
            {
                if (entity.Id > 0)
                {
                    domain.Update(entity);
                }
                else
                {
                    domain.Save(entity);
                }
            }
        }

        private void SetNonActive(SpecialCalcAccount account)
        {
            if (account != null && account.IsActive)
            {
                account.IsActive = false;
                this.specaccDomain.Update(account);
            }
        }

        private ManOrgBaseContract GetCurrentManOrgContract(RealityObject ro)
        {
            var domain = this.container.Resolve<IManagingOrgRealityObjectService>();

            using (this.container.Using(domain))
            {
                return domain.GetCurrentManOrg(ro);
            }
        }

        private SpecialCalcAccount CreateNewSpecialAccount(RealityObjectDecisionProtocol decisionProtocol)
        {
            // заглушка, т.к. для RegOpAccount нет возможности сформировать протокол и заполнить информацию о кредитной организации,
            // а данные для special calc берутся именнно оттуда, возможно, что в этот метод мы в принципе не должны попасть,
            // т.к. далее по стеку идет проставление IsActive = false для данного аккаунта,
            // пока ответа на этот вопрос не нашел, чтобы ничего не поломать, заглушка пока здесь
            var crFundFormationDecision = this.GetDecisionByProtocol<CrFundFormationDecision>(decisionProtocol);
            if (crFundFormationDecision == null ||
                crFundFormationDecision.Decision == CrFundFormationDecisionType.RegOpAccount)
            {
                return null;
            }

            var notif = this.GetNotification(decisionProtocol);
            var creditOrgDecision = this.GetDecisionByProtocol<CreditOrgDecision>(decisionProtocol);

            if (notif == null)
            {
                throw new ValidationException("Не сформировано уведомление в протоколе решений");
            }

            if (creditOrgDecision == null)
            {
                throw new ValidationException("Не заполнена информация о кредитной организации в протоколе решений");
            }

            return new SpecialCalcAccount
            {
                AccountNumber = notif.Return(x => x.AccountNum),
                DateOpen = notif.Return(x => x.OpenDate),
                DateClose =
                    notif.Return(x => x.CloseDate) != DateTime.MinValue
                        ? (DateTime?)notif.Return(x => x.CloseDate)
                        : null,
                CreditOrg = creditOrgDecision.Return(x => x.Decision),
                TypeAccount = TypeCalcAccount.Special
            };
        }

        private DecisionNotification GetNotification(RealityObjectDecisionProtocol decisionProtocol)
        {
            var notifDomain = this.container.ResolveDomain<DecisionNotification>();

            using (this.container.Using(notifDomain))
            {
                return notifDomain.GetAll().FirstOrDefault(x => x.Protocol == decisionProtocol);
            }
        }

        private void CreateRelationIfNeed(RealityObject ro, SpecialCalcAccount account)
        {
            if (!this.calcroDomain.GetAll()
                .Where(x => x.RealityObject == ro)
                .Any(x => x.Account.Id == account.Id))
            {
                var relation = new CalcAccountRealityObject
                {
                    Account = account,
                    RealityObject = ro,
                    DateStart = account.DateOpen
                };
                this.calcroDomain.Save(relation);
            }
        }

        private object GetActiveProtocol(long roId, Dictionary<long, CrFundFormationDecision> crFundCache, Dictionary<long, GovDecision> govdecCache)
        {
            var anotherProtocol = crFundCache.Get(roId);

            var oneMoreProtocol = govdecCache.Get(roId);

            if (anotherProtocol != null && oneMoreProtocol == null)
            {
                return anotherProtocol;
            }

            if (anotherProtocol == null && oneMoreProtocol != null)
            {
                return oneMoreProtocol;
            }

            if (anotherProtocol == null) return null;

            if (anotherProtocol.Protocol.DateStart > oneMoreProtocol.ProtocolDate)
            {
                return anotherProtocol;
            }

            return oneMoreProtocol;
        }

        #endregion
    }
}