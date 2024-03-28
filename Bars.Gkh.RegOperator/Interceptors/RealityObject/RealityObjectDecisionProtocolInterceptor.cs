namespace Bars.Gkh.RegOperator.Interceptors
{
    using System.Linq;
    using B4;
    using B4.Modules.States;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.Gkh.Decisions.Nso.Domain;
    using Bars.Gkh.Decisions.Nso.Entities.Decisions;
    using Bars.Gkh.DomainService;
    using Decisions.Nso.Entities;
    using DomainService;
    using Entities;
    using System;

    using Bars.B4.Utils;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount;
    using Bars.Gkh.RegOperator.Enums;
    using Bars.Gkh.Repositories.ChargePeriod;

    using Gkh.Enums.Decisions;
    /// <summary>
    /// Интерцептор для <see cref="RealityObjectDecisionProtocol"/>
    /// </summary>
    public class RealityObjectDecisionProtocolInterceptor : EmptyDomainInterceptor<RealityObjectDecisionProtocol>
    {
        private readonly ISpecialCalcAccountService accountService;
        private readonly IRealityObjectDecisionProtocolService protocolService;
        private readonly IRealtyObjectAccountFormationService realtyObjectAccountFormationService;
        private readonly IDomainService<BasePersonalAccount> personalAccountDomainService;
        private readonly IDomainService<GovDecision> govDecision;
        private readonly IRealityObjectDecisionsService realityObjectDecisionsService;
        private readonly IDomainService<PersonalAccountChange> accChangeDomain;
        private readonly IGkhUserManager userManager;
        private readonly IStateProvider stateProvider;
        private readonly IDomainService<State> stateDomainService;
        private readonly IChargePeriodRepository periodRepository;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="accountService">Сервис для работы со спец. счётом</param>
        /// <param name="protocolService">Сервис для получения/установки активного протокола</param>
        /// <param name="realtyObjectAccountFormationService">Сервиса для актуализации способа формирования фонда дома.</param>
        public RealityObjectDecisionProtocolInterceptor(
            ISpecialCalcAccountService accountService,
            IRealityObjectDecisionProtocolService protocolService,
            IRealtyObjectAccountFormationService realtyObjectAccountFormationService,
            IDomainService<BasePersonalAccount> personalAccountDomainService,
            IDomainService<GovDecision> govDecision,
            IRealityObjectDecisionsService realityObjectDecisionsService,
            IDomainService<PersonalAccountChange> accChangeDomain,
            IGkhUserManager userManager,
            IStateProvider stateProvider,
            IDomainService<State> stateDomainService,
            IChargePeriodRepository periodRepository)
        {
            this.accountService = accountService;
            this.protocolService = protocolService;
            this.realtyObjectAccountFormationService = realtyObjectAccountFormationService;
            this.personalAccountDomainService = personalAccountDomainService;
            this.govDecision = govDecision;
            this.realityObjectDecisionsService = realityObjectDecisionsService;
            this.accChangeDomain = accChangeDomain;
            this.userManager = userManager;
            this.stateProvider = stateProvider;
            this.stateDomainService = stateDomainService;
            this.periodRepository = periodRepository;
        }

        /// <summary>
        /// Действие, выполняемое до удаления сущности
        /// </summary>
        /// <param name="service">Домен-сервис <see cref="RealityObjectDecisionProtocol"/></param>
        /// <param name="entity">Сущность</param>
        /// <returns>Результат операции</returns>
        public override IDataResult BeforeDeleteAction(IDomainService<RealityObjectDecisionProtocol> service, RealityObjectDecisionProtocol entity)
        {
            this.accountService.SetPersonalAccountStatesNonActiveIfNeeded(entity, true);
            var entityInfo = this.stateProvider.GetStatefulEntityInfo(typeof(BasePersonalAccount));

            var accounts = this.personalAccountDomainService.GetAll().Where(x => x.Room.RealityObject.Id == entity.RealityObject.Id);

            var protocols = service.GetAll().Where(x => x.RealityObject.Id == entity.RealityObject.Id).ToList();
            var govProtocols = this.govDecision.GetAll().Count(x => x.RealityObject.Id == entity.RealityObject.Id);
            if ((protocols.Count + govProtocols) == 1)
            {
                var state = this.stateDomainService.GetAll().FirstOrDefault(x => x.Code == "4" && x.TypeId == entityInfo.TypeId);

                foreach (var account in accounts)
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
                            Reason = "Удаление Протокола решения",
                            ChargePeriod = this.periodRepository.GetCurrentPeriod()
                        });
                }
            }

            if (protocols.Count == 2 && govProtocols == 0)
            {
                var crFundDec = this.realityObjectDecisionsService.GetActualDecision<CrFundFormationDecision>(entity.RealityObject);

                if (crFundDec.Decision == CrFundFormationDecisionType.SpecialAccount && entity.State.FinalState)
                {
                    var checkSecondAccOwnerDec = this.realityObjectDecisionsService.GetActualDecisions<AccountOwnerDecision>(
                        entity.RealityObject, new[] { entity })
                        .Any(x => x.DecisionType == AccountOwnerDecisionType.Custom);
                    var checkSecondCrFundDec = this.realityObjectDecisionsService.GetActualDecisions<CrFundFormationDecision>(
                        entity.RealityObject, new[] { entity })
                        .Any(x => x.Decision == CrFundFormationDecisionType.SpecialAccount);

                    if (checkSecondAccOwnerDec && checkSecondCrFundDec)
                    {
                        foreach (var account in accounts)
                        {
                            var state = this.stateDomainService.GetAll().FirstOrDefault(x => x.Code == "4" && x.TypeId == entityInfo.TypeId);

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
                                    Reason = "Удаление Протокола решения",
                                    ChargePeriod = this.periodRepository.GetCurrentPeriod()
                                });
                        }
                    }
                }
            }

            return base.BeforeDeleteAction(service, entity);
        }

        /// <summary>
        /// Действие, выполняемое до создания сущности
        /// </summary>
        /// <param name="service">Домен-сервис <see cref="RealityObjectDecisionProtocol"/></param>
        /// <param name="entity">Сущность</param>
        /// <returns>Результат операции</returns>
        public override IDataResult BeforeCreateAction(IDomainService<RealityObjectDecisionProtocol> service, RealityObjectDecisionProtocol entity)
        {
            var stateProvider = this.Container.Resolve<IStateProvider>();
            stateProvider.SetDefaultState(entity);

            var validationResult = this.CheckExistanceOfDecisionWithSameStartDate(service, entity);

            return validationResult.Success ? base.BeforeCreateAction(service, entity) : validationResult;
        }

        /// <summary>
        /// Действие, выполняемое до обновления сущности
        /// </summary>
        /// <param name="service">Домен-сервис <see cref="RealityObjectDecisionProtocol"/></param>
        /// <param name="entity">Сущность</param>
        /// <returns>Результат операции</returns>
        public override IDataResult BeforeUpdateAction(IDomainService<RealityObjectDecisionProtocol> service, RealityObjectDecisionProtocol entity)
        {
            var validationResult = this.CheckExistanceOfDecisionWithSameStartDate(service, entity);

            return validationResult.Success ? base.BeforeUpdateAction(service, entity) : validationResult;
        }

        /// <summary>
        /// Действие, выполняемое после удаления сущности
        /// </summary>
        /// <param name="service">Домен-сервис <see cref="RealityObjectDecisionProtocol"/></param>
        /// <param name="entity">Сущность</param>
        /// <returns>Результат операции</returns>
        public override IDataResult AfterDeleteAction(IDomainService<RealityObjectDecisionProtocol> service, RealityObjectDecisionProtocol entity)
        {
            this.protocolService.SetNextActualProtocol(entity);
            this.realtyObjectAccountFormationService.ActualizeAccountFormationType(entity.RealityObject.Id);

            return base.AfterDeleteAction(service, entity);
        }

        /// <summary>
        /// Действие, выполняемое после создания сущности
        /// </summary>
        /// <param name="service">Домен-сервис <see cref="RealityObjectDecisionProtocol"/></param>
        /// <param name="entity">Сущность</param>
        /// <returns>Результат операции</returns>
        public override IDataResult AfterCreateAction(IDomainService<RealityObjectDecisionProtocol> service, RealityObjectDecisionProtocol entity)
        {
            this.realtyObjectAccountFormationService.ActualizeAccountFormationType(entity.RealityObject.Id);
            return base.AfterCreateAction(service, entity);
        }

        /// <summary>
        /// Действие, выполняемое после обновления сущности
        /// </summary>
        /// <param name="service">Домен-сервис <see cref="RealityObjectDecisionProtocol"/></param>
        /// <param name="entity">Сущность</param>
        /// <returns>Результат операции</returns>
        public override IDataResult AfterUpdateAction(IDomainService<RealityObjectDecisionProtocol> service, RealityObjectDecisionProtocol entity)
        {
            this.realtyObjectAccountFormationService.ActualizeAccountFormationType(entity.RealityObject.Id);
            return base.AfterUpdateAction(service, entity);
        }

        private IDataResult CheckExistanceOfDecisionWithSameStartDate(IDomainService<RealityObjectDecisionProtocol> service, RealityObjectDecisionProtocol entity)
        {
            var roDecisions =
                service.GetAll().Where(x => x.RealityObject.Id == entity.RealityObject.Id && x.State.FinalState).Where(x => x.Id != entity.Id).ToList();

            if (roDecisions.Any(x => x.DateStart.Date == entity.DateStart.Date))
            {
                return new BaseDataResult(false, "На введенную дату вступления в силу документа уже заведен протокол");
            }

            var govDecisionDomain = this.Container.ResolveDomain<GovDecision>();
            using (this.Container.Using(govDecisionDomain))
            {
                var govDecisions = govDecisionDomain.GetAll().Where(x => x.RealityObject.Id == entity.RealityObject.Id && x.State.FinalState).ToList();

                if (govDecisions.Any(x => x.DateStart.Date == DateTime.MinValue))
                {
                    if (govDecisions.Any(x => x.ProtocolDate == entity.DateStart.Date))
                    {
                        return new BaseDataResult(false, "На введенную дату вступления в силу документа уже заведен протокол");
                    }
                }
                else if (govDecisions.Any(x => x.DateStart.Date == entity.DateStart.Date))
                {
                    return new BaseDataResult(false, "На введенную дату вступления в силу документа уже заведен протокол");
                }
            }
            return new BaseDataResult();
        }
    }
}