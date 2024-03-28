namespace Bars.Gkh.RegOperator.DomainService.RealityObjectDecisionProtocolService.Impl
{
    using System;
    using System.Linq;
    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.Gkh.Decisions.Nso.Domain;
    using Bars.Gkh.Decisions.Nso.Entities;
    using Bars.Gkh.Decisions.Nso.Entities.Decisions;
    using Bars.Gkh.DomainEvent.Infrastructure;
    using Bars.Gkh.DomainService;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Enums.Decisions;
    using Bars.Gkh.RegOperator.Domain;
    using Bars.Gkh.RegOperator.DomainEvent.Events.PersonalAccountDto;
    using Bars.Gkh.Repositories.ChargePeriod;

    /// <summary>
    /// Сервис для актуализации способа формирования фонда дома.
    /// <remarks>Устанавливает значение свойства AccountFormationVariant</remarks>
    /// </summary>
    public class RealtyObjectAccountFormationService : IRealtyObjectAccountFormationService
    {
        public IChargePeriodRepository ChargePeriodRepository { get; set; }

        private IRepository<RealityObject> realityObjectRepository;

        private IDomainService<GovDecision> govDecisionDomain;
        private ISessionProvider sessionProvider;
        private IRealityObjectDecisionsService realityObjectDecisionsService;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="realityObjectDomain">Домен-сервис <see cref="RealityObject"/></param>
        /// <param name="realityObjectDecisionsService">Сервис для работы с решениями на доме</param>
        /// <param name="govDecisionDomain">Домен-сервис решений органов гос. власти</param>
        /// <param name="sessionProvider">Сессия</param>
        public RealtyObjectAccountFormationService(
            IRepository<RealityObject> realityObjectDomain,
            IRealityObjectDecisionsService realityObjectDecisionsService,
            IDomainService<GovDecision> govDecisionDomain,
            ISessionProvider sessionProvider)
        {
            this.realityObjectRepository = realityObjectDomain;
            this.realityObjectDecisionsService = realityObjectDecisionsService;
            this.govDecisionDomain = govDecisionDomain;
            this.sessionProvider = sessionProvider;
        }

        /// <summary>
        /// Актуализировать способ формирования фонда в сущности дома
        /// </summary>
        /// <param name="realityObjectId">Идентификатор дома</param>
        public void ActualizeAccountFormationType(long realityObjectId)
        {
            var realityObject = this.realityObjectRepository.Get(realityObjectId);
            var session = this.sessionProvider.GetCurrentSession();
            var currentPeriod = this.ChargePeriodRepository.GetCurrentPeriod();
            if (realityObject == null)
            {
                return;
            }

            var ownerProtocol = this.realityObjectDecisionsService.GetActualDecision<AccountOwnerDecision>(realityObject, true, date: currentPeriod.GetEndDate());
            var protocol = this.realityObjectDecisionsService.GetActualDecision<CrFundFormationDecision>(realityObject, true, date: currentPeriod.GetEndDate());
            CrFundFormationType? type = null;

            if (protocol == null)
            {
                var govDecision = this.govDecisionDomain.GetAll()
                    .Where(x => x.State.FinalState && x.RealityObject.Id == realityObjectId)
                    .Where(x => x.DateStart <= currentPeriod.GetEndDate())
                    .OrderByDescending(x => x.ProtocolDate)
                    .FirstOrDefault();

                if (govDecision != null)
                {
                    type = govDecision.FundFormationByRegop ? CrFundFormationType.RegOpAccount : CrFundFormationType.NotSelected;
                }
                else
                {
                    type = CrFundFormationType.NotSelected;
                }
            }
            else
            {
                if (protocol.Decision == CrFundFormationDecisionType.RegOpAccount)
                {
                    type = CrFundFormationType.RegOpAccount;
                }
                else if (protocol.Decision == CrFundFormationDecisionType.SpecialAccount && ownerProtocol.IsNotNull())
                {
                    type = ownerProtocol.DecisionType == AccountOwnerDecisionType.RegOp
                        ? CrFundFormationType.SpecialRegOpAccount
                        : CrFundFormationType.SpecialAccount;
                }
                else
                {
                    type = CrFundFormationType.NotSelected;
                }
            }

            realityObject.AccountFormationVariant = type;
            this.realityObjectRepository.Update(realityObject);
            DomainEvents.Raise(new RealityObjectForDtoChangeEvent(realityObject));

            session.Evict(ownerProtocol);
            session.Evict(protocol);
        }
    }
}