namespace Bars.Gkh.RegOperator.StateChange
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.States;
    using Bars.B4.Utils;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Decisions.Nso.Domain;
    using Bars.Gkh.Decisions.Nso.Entities;
    using Bars.Gkh.Decisions.Nso.Entities.Decisions;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums.Decisions;
    using Bars.Gkh.RegOperator.DomainModelServices.MassUpdater;
    using Bars.Gkh.RegOperator.DomainService;
    using Bars.Gkh.RegOperator.DomainService.PersonalAccount;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount;
    using Bars.Gkh.RegOperator.Enums;
    using Bars.Gkh.Repositories;
    using Bars.Gkh.Repositories.ChargePeriod;

    using Castle.Windsor;

    /// <summary>
    /// Обработчик изменения статуса протокола решения/>
    /// </summary>
    public class DecisionProtocolStateChangeHandler : IStateChangeHandler
    {
        /// <summary>
        /// Черновик
        /// </summary>
        public const string Draft = "1";

        /// <summary>
        /// Утверждено
        /// </summary>
        public const string Approved = "2";

        /// <summary>
        /// Репозиторий для получения различных данных по статусам
        /// </summary>
        public IStateRepository StateRepository { get; set; }

        private readonly ISpecialCalcAccountService service;
        private readonly IGovDecisionAccountService govDecisionAccountService;
        private readonly IDomainService<GovDecision> govDecisionService;
        private readonly IDomainService<AccountOwnerDecision> accOwnerDecisionDomain;
        private readonly IDomainService<CrFundFormationDecision> crFundDecDomain;
        private readonly IDomainService<RealityObjectDecisionProtocol> protocolDomain;
        private readonly IDomainService<RealityObjectLoan> roLoanDomain;
        private readonly IRealityObjectDecisionsService realityObjectDecisionsService;
        private readonly IDomainService<BasePersonalAccount> personalAccountDomainService;
        private readonly IDomainService<PersonalAccountChange> accChangeDomain;
        private readonly IGkhUserManager userManager;
        private readonly IStateProvider stateProvider;
        private readonly IDomainService<State> stateDomainService;
        private readonly IWindsorContainer container;
        private readonly IChargePeriodRepository periodRepository;

        /// <summary>
        /// .ctor
        /// </summary>
        public DecisionProtocolStateChangeHandler(
            ISpecialCalcAccountService service,
            IGovDecisionAccountService govDecisionAccountService,
            IDomainService<GovDecision> govDecisionService,
            IDomainService<AccountOwnerDecision> accOwnerDecisionDomain,
            IDomainService<CrFundFormationDecision> crFundDecDomain,
            IDomainService<RealityObjectDecisionProtocol> protocolDomain,
            IDomainService<RealityObjectLoan> roLoanDomain,
            IRealityObjectDecisionsService realityObjectDecisionsService,
            IDomainService<BasePersonalAccount> personalAccountDomainService,
            IDomainService<PersonalAccountChange> accChangeDomain,
            IGkhUserManager userManager,
            IStateProvider stateProvider,
            IDomainService<State> stateDomainService,
            IWindsorContainer container,
            IChargePeriodRepository periodRepository)
        {
            this.service = service;
            this.govDecisionAccountService = govDecisionAccountService;
            this.govDecisionService = govDecisionService;
            this.accOwnerDecisionDomain = accOwnerDecisionDomain;
            this.crFundDecDomain = crFundDecDomain;
            this.protocolDomain = protocolDomain;
            this.roLoanDomain = roLoanDomain;
            this.realityObjectDecisionsService = realityObjectDecisionsService;
            this.personalAccountDomainService = personalAccountDomainService;
            this.accChangeDomain = accChangeDomain;
            this.userManager = userManager;
            this.stateProvider = stateProvider;
            this.stateDomainService = stateDomainService;
            this.container = container;
            this.periodRepository = periodRepository;
        }

        /// <summary>
        /// Обработка изменения статуса
        /// </summary>
        /// <param name="entity">Сущность</param>
        /// <param name="oldState">Старый статус</param>
        /// <param name="newState">Новый статус</param>
        public void OnStateChange(IStatefulEntity entity, State oldState, State newState)
        {
            using (var transaction = this.container.Resolve<IDataTransaction>())
            {
                try
                {
                    this.OnStateChangeInternal(entity, newState);

                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();

                    throw;
                }
            }
        }

        private void OnStateChangeInternal(IStatefulEntity entity, State newState)
        {
            RealityObject realityObject;

            BaseEntity activeProtocol;

            if (entity.State.TypeId.ToLowerInvariant() == "gkh_real_obj_gov_dec")
            {
                var govDecision = entity as GovDecision;

                if (govDecision == null)
                {
                    return;
                }

                //Если дата начала действия меньше текущей даты, то ничего не делаем
                if (govDecision.DateStart > DateTime.Today)
                {
                    return;
                }

                this.AcceptNewGovDecision(govDecision);

                //Определяем последний действующий протокол с решением о способе формирования фонда КР
                activeProtocol = this.GetActiveProtocol(govDecision.RealityObject.Id, 0, govDecision.Id);

                //Определяем как он соотносится с текущим
                if (activeProtocol is GovDecision)
                {
                    if ((activeProtocol as GovDecision).ProtocolDate > govDecision.ProtocolDate)
                    {
                        return;
                    }
                }
                else if (activeProtocol is RealityObjectDecisionProtocol)
                {
                    if ((activeProtocol as RealityObjectDecisionProtocol).ProtocolDate > govDecision.ProtocolDate)
                    {
                        return;
                    }
                }

                // Если поменяли на конечный, нужно отразить в реестрах счетов регоператора
                // Но это не сделано
                if (newState.FinalState && govDecision.FundFormationByRegop)
                {
                    //Текущий протокол и есть самый последний. Меняем статус лицевых счетов при необходимости
                    this.govDecisionAccountService.SetPersonalAccountStateIfNeeded(govDecision);
                    return;
                }

                // либо переводим на черновик, либо решения на протоколе нет
                if (newState.StartState || !govDecision.FundFormationByRegop)
                {
                    //При отсутствии действующего протокола меняем статус лицевых счетов на неактивный
                    if (activeProtocol == null)
                    {
                        this.govDecisionAccountService.SetPersonalAccountStatesNonActiveIfNeeded(govDecision);
                    }
                }

                realityObject = govDecision.RealityObject;
            }

            else if (entity.State.TypeId.ToLowerInvariant() == "gkh_real_obj_dec_protocol")
            {
                var protocol = entity as RealityObjectDecisionProtocol;

                if (protocol == null)
                {
                    return;
                }

                this.CheckStatus(protocol, newState);

                //если дата вступления в силу протокола меньше текущей даты, то ничего не делаем
                if (protocol.DateStart > DateTime.Today)
                {
                    return;
                }

                //Проверяем, принято ли в данном протоколе решение о способе формирования фонда КР
                //Если нет, то данный протокол никоим образом не влияет на статусы лицевых счетов

                //Определяем последний действующий протокол с решением о способе формирования фонда КР
                activeProtocol = this.GetActiveProtocol(protocol.RealityObject.Id, protocol.Id, 0);

                var hasNotPaidLoans = this.roLoanDomain.GetAll()
                    .Where(x => x.LoanTaker.RealityObject.Id == protocol.RealityObject.Id)
                    .Any(x => x.LoanSum != x.LoanReturnedSum);

                if (hasNotPaidLoans && newState.FinalState && activeProtocol is RealityObjectDecisionProtocol)
                {
                    var crFundDec = this.crFundDecDomain.GetAll().FirstOrDefault(x => x.Protocol == protocol);

                    var activeCrFund = this.crFundDecDomain.GetAll()
                        .FirstOrDefault(x => x.Protocol.Id == activeProtocol.Id);

                    if (crFundDec.Return(x => x.Decision) != activeCrFund.Return(x => x.Decision))
                    {
                        throw new ValidationException("Для смены способа формирования КР необходимо вернуть займ через реестр займов");
                    }
                }

                // Если поменяли на конечный, нужно отразить в реестрах счетов регоператора
                if (newState.FinalState)
                {
                    this.AcceptNewProtocol(protocol);
                    this.service.SetPersonalAccountStatesActiveIfAble(protocol);
                    this.service.AddPaymentForOpeningAccount(protocol);
                    return;
                }

                if (newState.StartState)
                {
                    this.service.SetPersonalAccountStatesNonActiveIfNeeded(protocol);
                }

                realityObject = protocol.RealityObject;
            }
            else
            {
                return;
            }

            this.container.Resolve<ISessionProvider>().GetCurrentSession().Flush();

            // значит надо получить последний протокол в состоянии утверждено, в котором есть решения
            //activeProtocol = GetActiveProtocol(realityObject.Id, realityObjectDecisionProtocolId, govDecisionProtocolId);

            if (activeProtocol is RealityObjectDecisionProtocol)
            {
                this.AcceptNewProtocol(activeProtocol as RealityObjectDecisionProtocol);
                return;
            }

            if (activeProtocol is GovDecision)
            {
                this.govDecisionAccountService.SetPersonalAccountStateIfNeeded(activeProtocol as GovDecision);
                return;
            }

            // ну все. Теперь делаем счет неактивным
            this.service.SetNonActiveByRealityObject(realityObject);
        }

        /// <summary>
        /// Возвращает действующий протокол с решением о принятии способа формирования фонда
        /// И меняет статус на "черновик" у активного протокола
        /// </summary>
        /// <param name="realityObjectId">Идентификатор дома</param>
        /// <param name="excludeRoDecProtocolId">Код протокола собственников, который нужно исключить из выборки</param>
        /// <param name="excludeGovDecisionId">Код протокола органов гос. власти, который нужно исключить из выборки</param>
        /// <returns>Протокол типа RealityObjectDecisionProtocol или GovDecision, или null, если протокол не найден</returns>
        private BaseEntity GetActiveProtocol(long realityObjectId, long excludeRoDecProtocolId, long excludeGovDecisionId)
        {
            var anotherProtocol = this.crFundDecDomain.GetAll()
                .Where(x => x.Protocol.RealityObject.Id == realityObjectId)
                .Where(x => x.Protocol.Id != excludeRoDecProtocolId)
                .Where(x => x.Protocol.DateStart < DateTime.Today)
                .OrderByDescending(x => x.Protocol.ProtocolDate)
                .FirstOrDefault(x => x.Protocol.State.FinalState)
                .Return(x => x.Protocol);

            var oneMoreProtocol = this.govDecisionService.GetAll()
                .Where(x => x.RealityObject.Id == realityObjectId)
                .Where(x => x.FundFormationByRegop)
                .Where(x => x.Id != excludeGovDecisionId)
                .Where(x => x.DateStart < DateTime.Today)
                .OrderByDescending(x => x.ProtocolDate)
                .FirstOrDefault(x => x.State.FinalState);

            if (anotherProtocol != null && oneMoreProtocol == null)
            {
                var approvedState = this.StateRepository.GetAllStates<RealityObjectDecisionProtocol>().FirstOrDefault(x => x.FinalState);
                var draftState = this.StateRepository.GetAllStates<RealityObjectDecisionProtocol>().FirstOrDefault(x => x.StartState);

                if (anotherProtocol.State == approvedState)
                {
                    anotherProtocol.State = draftState;
                }

                return anotherProtocol;
            }

            if (anotherProtocol == null && oneMoreProtocol != null)
            {
                var approvedState = this.StateRepository.GetAllStates<GovDecision>().FirstOrDefault(x => x.FinalState);
                var draftState = this.StateRepository.GetAllStates<GovDecision>().FirstOrDefault(x => x.StartState);

                if (oneMoreProtocol.State == approvedState)
                {
                    oneMoreProtocol.State = draftState;
                }

                return oneMoreProtocol;
            }

            if (anotherProtocol == null)
            {
                return null;
            }

            if (anotherProtocol.ProtocolDate > oneMoreProtocol.ProtocolDate)
            {
                return anotherProtocol;
            }

            return oneMoreProtocol;
        }

        private void AcceptNewProtocol(RealityObjectDecisionProtocol protocol)
        {
            if (protocol == null)
            {
                return;
            }

            var protocolAny = this.protocolDomain.GetAll()
                .Where(x => x.DateStart < DateTime.Today)
                .Where(x => x.State.FinalState)
                .Where(x => x.DateStart > protocol.DateStart)
                .Where(x => x.RealityObject == protocol.RealityObject)
                .Any(x => x != protocol);

            var govDecisionAny = this.govDecisionService.GetAll()
                .Where(x => x.FundFormationByRegop)
                .Where(x => x.DateStart < DateTime.Today)
                .Where(x => x.State.FinalState)
                .Where(x => x.DateStart > protocol.DateStart)
                .Any(x => x.RealityObject == protocol.RealityObject);

            if (protocolAny || govDecisionAny)
            {
                return;
            }

            var crFundDec = this.crFundDecDomain.GetAll().FirstOrDefault(x => x.Protocol.Id == protocol.Id);
            var accDec = this.accOwnerDecisionDomain.GetAll().FirstOrDefault(x => x.Protocol.Id == protocol.Id);

            //значит у протокола нет решения о способе формирования счета
            //и состояние счета не должно измениться
            if (crFundDec == null)
            {
                return;
            }

            this.service.HandleSpecialAccountByProtocolChange(accDec, crFundDec, null, protocol.RealityObject);
        }

        private void AcceptNewGovDecision(GovDecision govDecision)
        {
            if (govDecision == null)
            {
                return;
            }

            var protocolAny = this.protocolDomain.GetAll()
                .Where(x => x.DateStart < DateTime.Today)
                .Where(x => x.State.FinalState)
                .Where(x => x.DateStart > govDecision.DateStart)
                .Any(x => x.RealityObject == govDecision.RealityObject);

            var govDecisionAny = this.govDecisionService.GetAll()
                .Where(x => x.FundFormationByRegop)
                .Where(x => x.DateStart < DateTime.Today)
                .Where(x => x.State.FinalState)
                .Where(x => x.DateStart > govDecision.DateStart)
                .Where(x => x.RealityObject == govDecision.RealityObject)
                .Any(x => x != govDecision);

            if (protocolAny || govDecisionAny)
            {
                return;
            }

            this.service.HandleSpecialAccountByProtocolChange(null, null, govDecision, govDecision.RealityObject);
        }

        private void CheckStatus(RealityObjectDecisionProtocol protocol, State newState)
        {
            var accOwnerDec = this.realityObjectDecisionsService.GetActualDecision<AccountOwnerDecision>(protocol.RealityObject);
            var crFundDec = this.realityObjectDecisionsService.GetActualDecision<CrFundFormationDecision>(protocol.RealityObject);
            var entityInfo = this.stateProvider.GetStatefulEntityInfo(typeof(BasePersonalAccount));

            if (newState.Code == DecisionProtocolStateChangeHandler.Approved && crFundDec.Decision == CrFundFormationDecisionType.SpecialAccount
                && accOwnerDec.DecisionType == AccountOwnerDecisionType.Custom)
            {
                var accountsToUpdate = this.personalAccountDomainService.GetAll().Where(x => x.Room.RealityObject.Id == protocol.RealityObject.Id).ToList();
                var state = this.stateDomainService.GetAll().FirstOrDefault(x => x.Code == "4" && x.TypeId == entityInfo.TypeId);

                var context = accountsToUpdate.Count > 200 ? MassUpdateContext.CreateContext(this.container) : null;
                {
                    try
                    {
                        foreach (var account in accountsToUpdate)
                        {
                            account.State = state;
                            this.personalAccountDomainService.Update(account);

                            this.accChangeDomain.Save(
                                new PersonalAccountChange
                                {
                                    PersonalAccount = account,
                                    ChangeType = PersonalAccountChangeType.NonActive,
                                    Date = DateTime.UtcNow,
                                    Description = "Для ЛС установлен статус \"Не активен\"",
                                    Operator = this.userManager.GetActiveUser().Return(y => y.Login),
                                    ActualFrom = DateTime.UtcNow,
                                    NewValue = "Статус ЛС = Не активен",
                                    Reason = "Добавление Протокола решения (Способ формирования фонда - Специальный счет, Владелец счета - Дом)",
                                    ChargePeriod = this.periodRepository.GetCurrentPeriod()
                                });
                        }
                    }
                    finally
                    {
                        context?.Dispose();
                    }

                }

            }
            if (newState.Code == DecisionProtocolStateChangeHandler.Draft)
            {
                var checkCrFundDecs = this.realityObjectDecisionsService.GetActualDecisions<CrFundFormationDecision>(
                    protocol.RealityObject,
                    new[] { protocol })
                    .Any(x => x.Decision == CrFundFormationDecisionType.SpecialAccount);

                if (!checkCrFundDecs)
                {
                    var accountsToUpdate = this.personalAccountDomainService.GetAll().Where(x => x.Room.RealityObject.Id == protocol.RealityObject.Id).ToList();
                    var state = this.stateDomainService.GetAll().FirstOrDefault(x => x.Code == "4" && x.TypeId == entityInfo.TypeId);


                    var context = accountsToUpdate.Count > 200 ? MassUpdateContext.CreateContext(this.container) : null;
                    {
                        try
                        {
                            foreach (var account in accountsToUpdate)
                            {
                                account.State = state;

                                this.personalAccountDomainService.Update(account);

                                this.accChangeDomain.Save(
                                    new PersonalAccountChange
                                    {
                                        PersonalAccount = account,
                                        ChangeType = PersonalAccountChangeType.NonActive,
                                        Date = DateTime.UtcNow,
                                        Description = "Для ЛС установлен статус \"Не активен\"",
                                        Operator = this.userManager.GetActiveUser().Return(y => y.Login),
                                        ActualFrom = DateTime.UtcNow,
                                        NewValue = "Статус ЛС = Не активен",
                                        Reason = "Смена статуса Протокола решения на \"Черновик\"",
                                        ChargePeriod = this.periodRepository.GetCurrentPeriod()
                                    });
                            }
                        }
                        finally
                        {
                            context?.Dispose();
                        }
                    }
                }
            }
        }
    }
}