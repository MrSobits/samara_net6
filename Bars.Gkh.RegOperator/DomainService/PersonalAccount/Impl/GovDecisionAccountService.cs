namespace Bars.Gkh.RegOperator.DomainService.PersonalAccount.Impl
{
    using System;
    using System.Linq;

    using B4;
    using B4.DataAccess;
    using B4.Utils;

    using Bars.B4.IoC;
    using Microsoft.Extensions.Logging;
    using Bars.B4.Modules.States;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Decisions.Nso.Domain;
    using Bars.Gkh.Decisions.Nso.Entities;
    using Bars.Gkh.Decisions.Nso.Entities.Decisions;
    using Bars.Gkh.Domain;
    using Bars.Gkh.DomainEvent.Infrastructure;
    using Bars.Gkh.Enums;
    using Bars.Gkh.RegOperator.Domain.Repository;
    using Bars.Gkh.RegOperator.DomainModelServices.MassUpdater;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount;
    using Bars.Gkh.RegOperator.Enums;
    using Bars.Gkh.Repositories.ChargePeriod;

    using Castle.Windsor;

    using Entities;
    using Gkh.Utils;
    using DomainEvent.Events.PersonalAccountDto;
    using DomainEvent;

    /// <summary>
    /// Сервис для "Протокол решения органа государственной власти"
    /// </summary>
    public class GovDecisionAccountService: IGovDecisionAccountService
    {
        private readonly IWindsorContainer container;

        /// <summary>
        /// GovDecisionAccountService
        /// </summary>
        /// <param name="container"></param>
        public GovDecisionAccountService(IWindsorContainer container)
        {
            this.container = container;
        }

        /// <summary>
        /// Изменить статус Л/С относящихся к дому протокола на "активный". Если конечно это возможно ;-)
        /// </summary>
        /// <param name="protocol">Протокол решений собственников</param>
        /// <param name="deletingCurrentProtocol">Текущий протокол удаляется</param>
        public void SetPersonalAccountStateIfNeeded(GovDecision protocol, bool deletingCurrentProtocol = false)
        {
            if (this.AnySuitableDecision(protocol, deletingCurrentProtocol))
            {
                return;
            }

            var persAccRepo = this.container.ResolveRepository<BasePersonalAccount>();
            var ownerRepo = this.container.ResolveRepository<PersonalAccountOwner>();
            var stateRepo = this.container.ResolveRepository<State>();
            var stateProvider = this.container.Resolve<IStateProvider>();
            var ownerService = this.container.Resolve<IPersonalAccountOwnerService>();
            var logger = this.container.Resolve<ILogger>();
            var hmaoConfig = this.container.HasComponent<IHouseTypesConfigService>() ? this.container.Resolve<IHouseTypesConfigService>() : null;
            var useBlockedBuilding = hmaoConfig?.GetHouseTypesConfig().UseBlockedBuilding ?? false;

            using (this.container.Using(persAccRepo, stateProvider, stateRepo, logger, hmaoConfig))
            {
                // если в доме состояние дома=Аварийный
                // или в чекбоксе "Дом не участвует в программе КР" стоит галочка
                // или Тип дома = Блокированной застройки и в настройка не стоит галка участвует в программе КР Блокированной застройкой
                // то счета должны оставаться на статусе "Не активен"
                if (protocol.RealityObject.IsNotInvolvedCr
                    || protocol.RealityObject.ConditionHouse == ConditionHouse.Emergency
                    || protocol.RealityObject.ConditionHouse == ConditionHouse.Razed
                    || protocol.RealityObject.TypeHouse == TypeHouse.BlockedBuilding && !useBlockedBuilding)
                {
                    return;
                }

                /*
                 * Если решение о счете регионального оператора)
                 * то ЛС -> статус Открыт, иначе в не активно 
                 */
                bool toActive = protocol.FundFormationByRegop;

                const string typeId = "gkh_regop_personal_account";

                var state = stateRepo.GetAll().FirstOrDefault(x => x.Code == (toActive ? "1" : "4") && x.TypeId == typeId);

                if (state == null)
                {
                    return;
                }
                var accChangeDomain = this.container.ResolveDomain<PersonalAccountChange>();
                var сhargePeriodRepository = this.container.Resolve<IChargePeriodRepository>();
                var period = сhargePeriodRepository.GetCurrentPeriod();
                var userManager = this.container.Resolve<IGkhUserManager>();
                using (var tr = this.container.Resolve<IDataTransaction>())
                {
                    try
                    {
                        var accountsToUpdate = persAccRepo.GetAll()
                            .Where(x => x.Room.RealityObject.Id == protocol.RealityObject.Id)
                            .Where(x => toActive ? x.State.Code == "4" : x.State.Code == "1")
                            .ToList();

                        var context = accountsToUpdate.Count > 200 ? MassUpdateContext.CreateContext(this.container) : null;
                        try
                        {
                            accountsToUpdate.ForEach(x =>
                            {
                                    x.State = state;
                                    persAccRepo.Update(x);

                                    // здесь не сохраняем владельца, т.к. будет отложенное сохранение через MassUpdateContext, если он создался
                                    if (ownerService.OnUpdateOwner(x.AccountOwner))
                                    {
                                        ownerRepo.Update(x.AccountOwner);
                                    }
                                    DomainEvents.Raise(new BasePersonalAccountDtoEvent(x));
                                // добавил запись в историю аккаунта по требованию Смоленска. 
                                accChangeDomain.Save(new PersonalAccountChange
                                {
                                    PersonalAccount = x,
                                    ChangeType = PersonalAccountChangeType.Open,
                                    Date = DateTime.Now,
                                    Description = "Для ЛС установлен статус \"Открыт\"",
                                    ActualFrom = DateTime.Now,
                                    NewValue = "Статус ЛС = Открыт",
                                    ChargePeriod = period,
                                    Operator = userManager.GetActiveUser().Return(y => y.Login),
                                });
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
        }

        /// <summary>
        /// Изменить статус Л/С относящихся к дому протокола на "неактивный". Если конечно это нужно ;-)
        /// </summary>
        /// <param name="protocol">Протокол решений собственников</param>
        /// <param name="deletingCurrentProtocol">Текущий протокол удаляется</param>
        /// <param name="isRealityObjectChange">Не добавляем в excluded protocol при изменении пар-ров дома</param>
        public void SetPersonalAccountStatesNonActiveIfNeeded(
            GovDecision protocol,
            bool deletingCurrentProtocol = false,
            bool isRealityObjectChange = false)
        {
            if (this.AnySuitableDecision(protocol, deletingCurrentProtocol))
            {
                return;
            }

            var decisionService = this.container.Resolve<IRealityObjectDecisionsService>();
            var persAccDomain = this.container.ResolveDomain<BasePersonalAccount>();
            var stateProvider = this.container.Resolve<IStateProvider>();
            var stateRepo = this.container.Resolve<IRepository<State>>();
            var tr = this.container.Resolve<IDataTransaction>();
            var accChangeDomain = this.container.ResolveDomain<PersonalAccountChange>();
            var userManager = this.container.Resolve<IGkhUserManager>();
            var сhargePeriodRepository = this.container.Resolve<IChargePeriodRepository>();
            var ownerService = this.container.Resolve<IPersonalAccountOwnerService>();
            var ownerRepo = this.container.ResolveRepository<PersonalAccountOwner>();

            using (this.container.Using(decisionService, persAccDomain, stateProvider,
                    tr, stateRepo, accChangeDomain, userManager, сhargePeriodRepository))
            {
                using (tr)
                {
                    try
                    {
                        var type = stateProvider.GetStatefulEntityInfo(typeof(BasePersonalAccount));

                        if (type == null)
                        {
                            return;
                        }

                        var state = stateRepo.GetAll().FirstOrDefault(x => x.Code == "4" && x.TypeId == type.TypeId);
                        var period = сhargePeriodRepository.GetCurrentPeriod();

                        var accountsToUpdate =   persAccDomain.GetAll()
                            .Where(x => x.Room.RealityObject.Id == protocol.RealityObject.Id)
                            .Where(x => x.State.Code == "1")
                            .ToList();   
                        
                        var context = accountsToUpdate.Count > 200 ? MassUpdateContext.CreateContext(this.container) : null;
                        try
                        {
                            accountsToUpdate.ForEach(
                           x =>
                           {
                               if (x.State.Code == "1")
                               {
                                   stateProvider.ChangeState(
                                       x.Id,
                                       type.TypeId,
                                       state,
                                       "На основе изменения статуса протокола решения органов гос. власти",
                                       false);
                                   persAccDomain.Update(x);

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
                                           ChargePeriod = period
                                       });

                                   DomainEvents.Raise(new BasePersonalAccountDtoEvent(x));
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
        }

        /// <summary>
        /// Функция проверяет, если кроме текущего проверяемого протокола наиболее "свежий" протокол
        /// </summary>
        /// <param name="protocol">Текущий проверяемый протокол</param>
        /// <param name="deletingCurrentProtocol">Текущий протокол удаляется. В таком случае его дата протокола не учитывается в фильтре</param>
        private bool AnySuitableDecision(GovDecision protocol, bool deletingCurrentProtocol = false)
        {
            var crFundFormDomain = this.container.ResolveDomain<CrFundFormationDecision>();

            /*
             * Если есть более приоритетный протокол решения собственников жилья, т.е.
             * 1) Статус протокола конечный
             * 2) Дата протокола выше, чем дата нового протокола
             * то ничего не делаем
             */
            using (this.container.Using(crFundFormDomain))
            {
                if (crFundFormDomain.GetAll()
                    .Where(x => x.Protocol != null)
                    .WhereIf(!deletingCurrentProtocol, x => x.Protocol.ProtocolDate > protocol.ProtocolDate)
                    .Where(x => x.Protocol.RealityObject.Id == protocol.RealityObject.Id)
                    .Where(x => x.Protocol.State != null).Any(x => x.Protocol.State.FinalState))
                {
                    return true;
                }
            }

            var govDecisionDomain = this.container.ResolveDomain<GovDecision>();

            /*
             * Если есть более приоритетный протокол решения органов гос. власти
             * 1) Статус протокола конечный
             * 2) Дата протокола выше, чем дата нового протокола
             * 3) Принято решение о формировании фонда капитального ремонта на счету регионального оператора
             * то ничего не делаем
             */
            using (this.container.Using(govDecisionDomain))
            {
                if (govDecisionDomain.GetAll()
                    .Where(x => x.Id != protocol.Id)
                    .WhereIf(!deletingCurrentProtocol, x => x.ProtocolDate > protocol.ProtocolDate)
                    .Where(x => x.RealityObject.Id == protocol.RealityObject.Id)
                    .Where(x => x.State != null)
                    .Where(x => x.State.FinalState)
                    .Any(x => x.FundFormationByRegop))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Возвращает действующий протокол с решением о принятии способа формирования фонда
        /// </summary>
        /// <param name="realityObjectId">Идентификатор дома</param>
        /// <param name="excludingRoDecisionProtocolId">Идентификатор протокола</param>
        /// /// <param name="excludingGovDecisionProtocolId">Идентификатор протокола</param>
        /// <returns>Протокол типа RealityObjectDecisionProtocol или GovDecision, или null, если протокол не найден</returns>
        private object GetActiveProtocol(long realityObjectId, long excludingRoDecisionProtocolId,
            long excludingGovDecisionProtocolId)
        {
            var realityObjectDecisionProtocol = this.container.ResolveDomain<RealityObjectDecisionProtocol>().GetAll()
                .Where(x => x.RealityObject.Id == realityObjectId)
                .Where(x => x.Id != excludingRoDecisionProtocolId)
                .OrderByDescending(x => x.ProtocolDate)
                .FirstOrDefault(x => x.State.FinalState);

            // var accountOwnerDecision = _container.ResolveDomain<AccountOwnerDecision>().GetAll()
            // .Where(x => x.Protocol.RealityObject.Id == realityObjectId)
            // .Where(x => x.Protocol.Id != excludingRoDecisionProtocolId)
            // .OrderByDescending(x => x.Protocol.ProtocolDate)
            // .FirstOrDefault(x => x.Protocol.State.FinalState);

            // var crFundFormationDecision = _container.ResolveDomain<CrFundFormationDecision>().GetAll()
            // .Where(x => x.Protocol.RealityObject.Id == realityObjectId)
            // .Where(x => x.Protocol.Id != excludingRoDecisionProtocolId)
            // .OrderByDescending(x => x.Protocol.ProtocolDate)
            // .FirstOrDefault(x => x.Protocol.State.FinalState);
            var oneMoreProtocol = this.container.ResolveDomain<GovDecision>().GetAll()
                .Where(x => x.RealityObject.Id == realityObjectId)
                .Where(x => x.FundFormationByRegop)
                .Where(x => x.Id != excludingGovDecisionProtocolId)
                .OrderByDescending(x => x.ProtocolDate)
                .DefaultIfEmpty(null)
                .First();

            if (realityObjectDecisionProtocol != null && oneMoreProtocol == null) return realityObjectDecisionProtocol;
            if (realityObjectDecisionProtocol == null && oneMoreProtocol != null) return oneMoreProtocol;
            if (realityObjectDecisionProtocol == null && oneMoreProtocol == null) return null;
            if (realityObjectDecisionProtocol.ProtocolDate > oneMoreProtocol.ProtocolDate) return realityObjectDecisionProtocol;
            return oneMoreProtocol;
        }
    }
}
