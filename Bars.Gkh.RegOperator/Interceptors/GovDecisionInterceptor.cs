namespace Bars.Gkh.RegOperator.Interceptors
{
    using System.Linq;
    using B4.Utils;
    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Modules.States;
    using Bars.Gkh.Decisions.Nso.Domain;
    using Bars.Gkh.Decisions.Nso.Entities;
    using Bars.Gkh.Decisions.Nso.Entities.Decisions;
    using Bars.Gkh.DomainService;
    using Bars.Gkh.RegOperator.Entities.Decisions;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount;
    using Bars.Gkh.RegOperator.Enums;
    using System;

    using Bars.Gkh.Authentification;
    using Bars.Gkh.RegOperator.Domain.Repository;
    using Bars.Gkh.Repositories.ChargePeriod;

    using Gkh.Enums.Decisions;
    using Entities;
    using DomainService.PersonalAccount;

    /// <summary>
    /// Интерцептор для <see cref="GovDecision"/>
    /// </summary>
    public class GovDecisionInterceptor : EmptyDomainInterceptor<GovDecision>
    {
        private readonly IRealityObjectDecisionProtocolService _protocolService;
        private readonly IRealtyObjectAccountFormationService realtyObjectAccountFormationService;
        private readonly IDomainService<RealityObjectDecisionProtocol> realityObjectDecisionProtocolDomainService;
        private readonly IDomainService<BasePersonalAccount> personalAccountDomainService;
        private readonly IPersonalAccountService personalAccountService;
        private readonly IRealityObjectDecisionsService realityObjectDecisionsService;
        private readonly IDomainService<PersonalAccountChange> accChangeDomain;
        private readonly IGkhUserManager userManager;
        private readonly IRepository<State> stateRepo;
        private readonly IStateProvider stateProvider;
        private readonly IChargePeriodRepository periodRepository;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="protocolService">Сервис для получения/установки активного протокола</param>
        /// <param name="realtyObjectAccountFormationService">Сервис для актуализации способа формирования фонда дома.</param>
        public GovDecisionInterceptor(
            IRealityObjectDecisionProtocolService protocolService,
            IRealtyObjectAccountFormationService realtyObjectAccountFormationService,
            IDomainService<RealityObjectDecisionProtocol> realityObjectDecisionProtocolDomainService,
            IDomainService<BasePersonalAccount> personalAccountDomainService,
            IPersonalAccountService personalAccountService,
            IRealityObjectDecisionsService realityObjectDecisionsService,
            IDomainService<PersonalAccountChange> accChangeDomain,
            IGkhUserManager userManager,
            IRepository<State> stateRepo,
            IStateProvider stateProvider,
            IChargePeriodRepository periodRepository
            )
        {
            _protocolService = protocolService;
            this.realtyObjectAccountFormationService = realtyObjectAccountFormationService;
            this.realityObjectDecisionProtocolDomainService = realityObjectDecisionProtocolDomainService;
            this.personalAccountDomainService = personalAccountDomainService;
            this.personalAccountService = personalAccountService;
            this.realityObjectDecisionsService = realityObjectDecisionsService;
            this.accChangeDomain = accChangeDomain;
            this.userManager = userManager;
            this.stateRepo = stateRepo;
            this.stateProvider = stateProvider;
            this.periodRepository = periodRepository;
        }

        /// <summary>
        /// Действие, выполняемое до создания сущности
        /// </summary>
        /// <param name="service">Домен-сервис <see cref="GovDecision"/></param>
        /// <param name="entity">Сущность</param>
        /// <returns>Результат операции</returns>
        public override IDataResult BeforeCreateAction(IDomainService<GovDecision> service, GovDecision entity)
        {
            var servStateProvider = Container.Resolve<IStateProvider>();
            using (Container.Using(servStateProvider))
            {
                servStateProvider.SetDefaultState(entity);
            }

            if (entity.State == null)
            {
                return Failure("Не удалось получить начальный статус для сущности");
            }

            var validationResult = CheckExistanceOfDecisionWithSameStartDate(service, entity);

            return validationResult.Success ? base.BeforeCreateAction(service, entity) : validationResult;
        }


        /// <summary>
        /// Действие, выполняемое до удаления сущности
        /// </summary>
        /// <param name="service">Домен-сервис <see cref="GovDecision"/></param>
        /// <param name="entity">Сущность</param>
        /// <returns>Результат операции</returns>
        public override IDataResult BeforeDeleteAction(IDomainService<GovDecision> service, GovDecision entity)
        {
            var domainService = Container.Resolve<IDomainService<DefferedUnactivation>>();

            domainService.GetAll()
                    .Where(x => x.GovDecision == entity)
                    .Select(x => x.Id)
                    .ForEach(x => domainService.Delete(x));

            var accounts = this.personalAccountDomainService.GetAll().Where(x => x.Room.RealityObject.Id == entity.RealityObject.Id);

            var protocols = this.realityObjectDecisionProtocolDomainService.GetAll().Where(x => x.RealityObject.Id == entity.RealityObject.Id).ToList();
            var govProtocols = service.GetAll().Count(x => x.RealityObject.Id == entity.RealityObject.Id);

            const string typeId = "gkh_regop_personal_account";
            var nonActiveState = this.stateRepo.GetAll().FirstOrDefault(x => x.Code == "4" && x.TypeId == typeId);

            if ((protocols.Count + govProtocols) == 1)
            {
                foreach (var account in accounts)
                {
                    stateProvider.ChangeState(account.Id, typeId, nonActiveState, "На основе удаления протокола решения", false);
                    personalAccountDomainService.Update(account);

                    accChangeDomain.Save(
                        new PersonalAccountChange
                        {
                            PersonalAccount = account,
                            ChangeType = PersonalAccountChangeType.NonActive,
                            Date = DateTime.UtcNow,
                            Description = "Для ЛС установлен статус \"Не активен\"",
                            Operator = userManager.GetActiveUser().Return(y => y.Login),
                            ActualFrom = DateTime.UtcNow,
                            NewValue = "Статус ЛС = Не активен",
                            Reason = "Удаление Протокола решения",
                            ChargePeriod = this.periodRepository.GetCurrentPeriod()
                        });
                }
            }

            if ((protocols.Count + govProtocols) == 2)
            {
                if (entity.FundFormationByRegop && protocols.Count != 0 && protocols.First().State.FinalState)
                {
                    var chackSecondaccOwnerDec = this.realityObjectDecisionsService.GetActualDecisions<AccountOwnerDecision>(
                        protocols.First().RealityObject)
                        .Any(x => x.DecisionType == AccountOwnerDecisionType.Custom);
                    var chackSecondCrFundDec = this.realityObjectDecisionsService.GetActualDecisions<CrFundFormationDecision>(
                        protocols.First().RealityObject)
                        .Any(x => CrFundFormationDecisionType.SpecialAccount == x.Decision);

                    

                    if (chackSecondaccOwnerDec && chackSecondCrFundDec)
                    {
                        foreach (var account in accounts)
                        {
                            stateProvider.ChangeState(account.Id, typeId, nonActiveState, "На основе удаления протокола решения", false);
                            personalAccountDomainService.Update(account);

                            accChangeDomain.Save(
                                new PersonalAccountChange
                                {
                                    PersonalAccount = account,
                                    ChangeType = PersonalAccountChangeType.NonActive,
                                    Date = DateTime.UtcNow,
                                    Description = "Для ЛС установлен статус \"Не активен\"",
                                    Operator = userManager.GetActiveUser().Return(y => y.Login),
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
        /// Действие, выполняемое до обновления сущности
        /// </summary>
        /// <param name="service">Домен-сервис <see cref="GovDecision"/></param>
        /// <param name="entity">Сущность</param>
        /// <returns>Результат операции</returns>
        public override IDataResult BeforeUpdateAction(IDomainService<GovDecision> service, GovDecision entity)
        {
            var validationResult = CheckExistanceOfDecisionWithSameStartDate(service, entity);
            return validationResult.Success ? base.BeforeUpdateAction(service, entity) : validationResult;
        }

        private IDataResult CheckExistanceOfDecisionWithSameStartDate(IDomainService<GovDecision> service, GovDecision entity)
        {
            var govDecisions =
                service.GetAll().Where(x => x.RealityObject.Id == entity.RealityObject.Id).Where(x => x.Id != entity.Id).ToList();

            if (govDecisions.Any(x => x.DateStart.Date == entity.DateStart.Date))
            {
                return new BaseDataResult(false, "На введенную дату вступления в силу документа уже заведен протокол");
            }

            var roDecisionDomain = Container.ResolveDomain<RealityObjectDecisionProtocol>();
            using (Container.Using(roDecisionDomain))
            {
                var roDecisions = roDecisionDomain.GetAll().Where(x => x.RealityObject.Id == entity.RealityObject.Id).ToList();

                if (roDecisions.Any(x => x.DateStart.Date == entity.DateStart.Date))
                {
                    return new BaseDataResult(false, "На введенную дату вступления в силу документа уже заведен протокол");
                }
            }

            return new BaseDataResult();
        }

        /// <summary>
        /// Метод вызывается после создания объекта
        /// </summary>
        /// <param name="service">Домен</param><param name="entity">Объект</param>
        /// <returns>
        /// Результат выполнения
        /// </returns>
        public override IDataResult AfterCreateAction(IDomainService<GovDecision> service, GovDecision entity)
        {
            var domainService = Container.ResolveDomain<CoreDecision>();
            domainService.Save(new CoreDecision
            {
                DecisionType = CoreDecisionType.Government,
                GovDecision = entity
            });

            realtyObjectAccountFormationService.ActualizeAccountFormationType(entity.RealityObject.Id);

            return base.AfterCreateAction(service, entity);
        }

        /// <summary>
        /// Действие, выполняемое после обновления сущности
        /// </summary>
        /// <param name="service">Домен-сервис <see cref="GovDecision"/></param>
        /// <param name="entity">Сущность</param>
        /// <returns>Результат операции</returns>
        public override IDataResult AfterUpdateAction(IDomainService<GovDecision> service, GovDecision entity)
        {
            realtyObjectAccountFormationService.ActualizeAccountFormationType(entity.RealityObject.Id);
            return Success();
        }

        /// <summary>
        /// Действие, выполняемое после создания сущности
        /// </summary>
        /// <param name="service">Домен-сервис <see cref="GovDecision"/></param>
        /// <param name="entity">Сущность</param>
        /// <returns>Результат операции</returns>
        public override IDataResult AfterDeleteAction(IDomainService<GovDecision> service, GovDecision entity)
        {
            _protocolService.SetNextActualProtocol(entity);
            realtyObjectAccountFormationService.ActualizeAccountFormationType(entity.RealityObject.Id);
            return base.AfterDeleteAction(service, entity);
        }
    }
}