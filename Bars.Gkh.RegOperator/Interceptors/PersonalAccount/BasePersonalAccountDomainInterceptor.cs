namespace Bars.Gkh.RegOperator.Interceptors
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.States;
    using Bars.B4.Utils;
    using Bars.Gkh.Decisions.Nso.Domain;
    using Bars.Gkh.Decisions.Nso.Entities;
    using Bars.Gkh.DomainEvent.Infrastructure;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Enums.Decisions;
    using Bars.Gkh.RegOperator.Domain.Repository;
    using Bars.Gkh.RegOperator.DomainEvent;
    using Bars.Gkh.RegOperator.DomainService.PersonalAccount;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount;
    using Bars.Gkh.RegOperator.Entities.Wallet;
    using Bars.Gkh.Repositories.ChargePeriod;
    using Bars.Gkh.Utils;
    using Bars.Gkh.Utils.Caching;
    using DomainEvent.Events.PersonalAccountDto;

    using FastMember;
    using NHibernate;


    /// <summary>
    /// Интерцептор для <see cref="BasePersonalAccount"/>
    /// </summary>
    public class BasePersonalAccountDomainInterceptor : EmptyDomainInterceptor<BasePersonalAccount>
    {
        private readonly TypeAccessor accessor;
        private FlushMode oldFlush;

        /// <summary>
        /// .ctor
        /// </summary>
        public BasePersonalAccountDomainInterceptor()
        {
            this.accessor = TypeAccessor.Create(typeof(BasePersonalAccount));
        }

        /// <summary>
        /// Репозиторий периода начислений
        /// </summary>
        public IChargePeriodRepository ChargePeriodRepo { get; set; }

        /// <summary>
        /// Домен-сервис <see cref="PersonalAccountPeriodSummary"/>
        /// </summary>
        public IDomainService<PersonalAccountPeriodSummary> SummaryDomain { get; set; }

        /// <summary>
        /// Домен-сервис <see cref="PersonalAccountOwner"/>
        /// </summary>
        public IRepository<PersonalAccountOwner> PersonalAccountOwnerRepository { get; set; }

        /// <summary>
        /// Интерфейс сервиса абонентов
        /// </summary>
        public IPersonalAccountOwnerService PersonalAccountOwnerService { get; set; }

        /// <summary>
        /// Домен-сервис <see cref="State"/>
        /// </summary>
        public IDomainService<State> StateDomainService { get; set; }

        /// <summary>
        /// Домен-сервис <see cref="Wallet"/>
        /// </summary>
        public IDomainService<Wallet> WalletDomain { get; set; }

        public IRealityObjectDecisionsService RealityObjectDecisionsService { get; set; }

        /// <summary>
        /// Сервис Протокол решений собственников
        /// </summary>
        public ISessionProvider Sessions { get; set; }

        /// <inheritdoc />
        public override IDataResult BeforeCreateAction(IDomainService<BasePersonalAccount> service, BasePersonalAccount entity)
        {
            if (entity.OpenDate == DateTime.MinValue)
            {
                entity.OpenDate = DateTime.Today;
            }

            this.oldFlush = this.Sessions.GetCurrentSession().FlushMode;
            this.Sessions.GetCurrentSession().FlushMode = FlushMode.Never;

            this.CreateWalletsIfNeeded(entity);

            entity.State = this.GetPersonalAccountState(entity.Room.RealityObject);

            return base.BeforeCreateAction(service, entity);
        }

        /// <inheritdoc />
        public override IDataResult BeforeUpdateAction(IDomainService<BasePersonalAccount> service, BasePersonalAccount entity)
        {
            if (entity.State.Code != "4")
            {
                var validation = this.ValidateProperties(service, entity);

                if (!validation.Success)
                {
                    return validation;
                }
            }

            return base.BeforeUpdateAction(service, entity);
        }

        /// <inheritdoc />
        public override IDataResult BeforeDeleteAction(IDomainService<BasePersonalAccount> service, BasePersonalAccount entity)
        {
            var anyPayment = this.Container.Resolve<IDomainService<PersonalAccountPayment>>()
                .GetAll()
                .Count(x => x.BasePersonalAccount.Id == entity.Id) > 0;

            if (anyPayment)
            {
                return this.Failure("По лицевому счету есть оплаты. Удаление отменено.");
            }

            var anyCharge = this.Container.Resolve<IDomainService<PersonalAccountCharge>>()
                .GetAll()
                .Count(x => x.BasePersonalAccount.Id == entity.Id) > 0
                || this.Container.Resolve<IDomainService<UnacceptedCharge>>()
                    .GetAll()
                    .Count(x => x.PersonalAccount.Id == entity.Id) > 0;

            if (anyCharge)
            {
                return this.Failure("По лицевому счету есть начисления. Удаление отменено.");
            }

            var personalAccountPeriodSummaryDomain = this.Container.Resolve<IDomainService<PersonalAccountPeriodSummary>>();
            var accountOwnershipHistoryDomain = this.Container.ResolveDomain<AccountOwnershipHistory>();

            try
            {
                personalAccountPeriodSummaryDomain.GetAll()
                    .Where(x => x.PersonalAccount.Id == entity.Id)
                    .Select(x => x.Id)
                    .ForEach(x => personalAccountPeriodSummaryDomain.Delete(x));

                accountOwnershipHistoryDomain.GetAll()
                    .Where(x => x.PersonalAccount.Id == entity.Id)
                    .Select(x => x.Id)
                    .ForEach(x => accountOwnershipHistoryDomain.Delete(x));
            }
            finally
            {
                this.Container.Release(personalAccountPeriodSummaryDomain);
                this.Container.Release(accountOwnershipHistoryDomain);
            }

            return this.Success();
        }

        /// <inheritdoc />
        public override IDataResult AfterCreateAction(IDomainService<BasePersonalAccount> service, BasePersonalAccount entity)
        {
            var period = this.ChargePeriodRepo.GetCurrentPeriod();

            if (period == null)
            {
                return this.Failure("Отстутствует текущий открытый период");
            }

            var summary = new PersonalAccountPeriodSummary
            {
                Period = period,
                PersonalAccount = entity
            };

            this.SummaryDomain.Save(summary);
            entity.SetOpenedPeriodSummary(summary);

            this.Sessions.GetCurrentSession().FlushMode = this.oldFlush;

            this.RecalcOwnerAccountsCount(entity);

            CountCacheHelper.InvalidateCache<BasePersonalAccount>(this.Container);
            DomainEvents.Raise(new BasePersonalAccountDtoEvent(entity));
            return base.AfterCreateAction(service, entity);
        }

        /// <inheritdoc />
        public override IDataResult AfterUpdateAction(IDomainService<BasePersonalAccount> service, BasePersonalAccount entity)
        {
            this.RecalcOwnerAccountsCount(entity);
            CountCacheHelper.InvalidateCache<BasePersonalAccount>(this.Container);
            DomainEvents.Raise(new BasePersonalAccountDtoEvent(entity));
            return base.AfterUpdateAction(service, entity);
        }

        /// <inheritdoc />
        public override IDataResult AfterDeleteAction(IDomainService<BasePersonalAccount> service, BasePersonalAccount entity)
        {
            this.RecalcOwnerAccountsCount(entity);
            CountCacheHelper.InvalidateCache<BasePersonalAccount>(this.Container);

            return base.AfterDeleteAction(service, entity);
        }

        private void RecalcOwnerAccountsCount(BasePersonalAccount entity)
        {
            if (this.PersonalAccountOwnerService.OnUpdateOwner(entity.AccountOwner))
            {
                this.PersonalAccountOwnerRepository.Update(entity.AccountOwner);
            }
        }

        private IDataResult ValidateProperties(IDomainService<BasePersonalAccount> service, BasePersonalAccount entity)
        {
            var existingPortions = service.GetAll()
                .Where(x => x.Room.Id == entity.Room.Id)
                .Where(x => x.Id != entity.Id)
                .Sum(x => (decimal?) x.AreaShare) ?? 0;

            if ((existingPortions + entity.AreaShare).RegopRoundDecimal(2) > 1)
            {
                return this.Failure("Внимание!! Сумма долей собственности по текущему помещению превышает допустимых значений!");
            }

            return this.Success();
        }

        private State GetPersonalAccountState(RealityObject realityObject)
        {
            if (realityObject.IsNotInvolvedCr
                || realityObject.ConditionHouse == ConditionHouse.Emergency
                || realityObject.TypeHouse == TypeHouse.BlockedBuilding)
            {
                return this.GetNonActivePersonalAccountState();
            }

            var crFundDecision = this.RealityObjectDecisionsService.GetActualDecision<CrFundFormationDecision>(realityObject, true);

            var crTypeDecision = crFundDecision.Return(x => x.Decision, CrFundFormationDecisionType.Unknown);

            if (crFundDecision == null)
            {
                var govDecision = this.RealityObjectDecisionsService.GetCurrentGovDecision(realityObject);

                if (govDecision != null)
                {
                    crTypeDecision = govDecision.FundFormationByRegop
                        ? CrFundFormationDecisionType.RegOpAccount
                        : CrFundFormationDecisionType.Unknown;
                }
            }

            switch (crTypeDecision)
            {
                case CrFundFormationDecisionType.Unknown:
                    return this.GetNonActivePersonalAccountState();
                case CrFundFormationDecisionType.RegOpAccount:
                    return this.GetOpenPersonalAccountState();
                case CrFundFormationDecisionType.SpecialAccount:
                    var accOwnerDecision = this.RealityObjectDecisionsService.GetActualDecision<AccountOwnerDecision>(realityObject, true);
                    if (accOwnerDecision == null)
                    {
                        return this.GetNonActivePersonalAccountState();
                    }

                    switch (accOwnerDecision.DecisionType)
                    {
                        case AccountOwnerDecisionType.Custom:
                            return this.GetNonActivePersonalAccountState();
                        case AccountOwnerDecisionType.RegOp:
                            return this.GetOpenPersonalAccountState();
                    }

                    break;
            }

            return null;
        }

        private State GetOpenPersonalAccountState()
        {
            return this.StateDomainService.FirstOrDefault(x => x.TypeId == "gkh_regop_personal_account" && x.Code == "1");
        }

        private State GetNonActivePersonalAccountState()
        {
            return this.StateDomainService.FirstOrDefault(x => x.TypeId == "gkh_regop_personal_account" && x.Code == "4");
        }

        private void CreateWalletsIfNeeded(BasePersonalAccount entity)
        {
            using (var tr = this.Container.Resolve<IDataTransaction>())
            {
                foreach (var walletProp in this.accessor.GetMembers().Where(x => x.Type == typeof(Wallet)))
                {
                    var wallet = this.accessor[entity, walletProp.Name] as Wallet;
                    if (wallet != null && wallet.Id == 0)
                    {
                        this.WalletDomain.Save(wallet);
                    }
                }

                try
                {
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
}