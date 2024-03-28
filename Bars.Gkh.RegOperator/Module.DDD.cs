namespace Bars.Gkh.RegOperator
{
    using System;
    using System.Linq;

    using B4.Application;
    using B4.Config;
    using B4.DataAccess;
    using B4.Events;
    using B4.IoC;
    using B4.Modules.Quartz;
    using B4.Utils;

    using Bars.Gkh.ConfigSections.RegOperator;
    using Bars.Gkh.DomainEvent.Infrastructure;
    using Bars.Gkh.RegOperator.Domain.Repository.ChargePeriod;
    using Bars.Gkh.RegOperator.DomainEvent.Events.PersonalAccountDto;
    using Bars.Gkh.RegOperator.DomainEvent.Events.RealityObjectLoan;
    using Bars.Gkh.RegOperator.DomainService.BankDocumentImport;
    using Bars.Gkh.RegOperator.DomainService.BankDocumentImport.Impl;
    using Bars.Gkh.RegOperator.DomainService.PartialOperationCancellation;
    using Bars.Gkh.RegOperator.DomainService.PersonalAccount;
    using Bars.Gkh.RegOperator.Entities.ValueObjects;
    using Bars.Gkh.RegOperator.Extenstions;
    using Bars.Gkh.Repositories.ChargePeriod;

    using Castle.MicroKernel.Lifestyle;
    using Castle.MicroKernel.Registration;
    using Castle.Windsor;

    using Distribution;
    using Distribution.Impl;
    using Distribution.Validators;

    using Domain;
    using Domain.RealtyObjectPayment;
    using Domain.Repository;
    using Domain.Repository.MoneyOperations;
    using Domain.Repository.PersonalAccounts;
    using Domain.Repository.RealityObjectAccount;
    using Domain.Repository.Transfers;
    using Domain.Repository.Wallets;

    using DomainEvent.Events.PersonalAccount;
    using DomainEvent.Events.PersonalAccountPayment;
    using DomainEvent.Events.PersonalAccountPayment.Payment;
    using DomainEvent.Events.PersonalAccountPayment.UndoPayment;
    using DomainEvent.Events.PersonalAccountRefund;
    using DomainEvent.Handlers;
    using DomainEvent.Handlers.PersonalAccountPayment;
    using DomainEvent.Infrastructure;

    using DomainModelServices;
    using DomainModelServices.Impl;
    using DomainModelServices.Impl.Loan;
    using DomainModelServices.PersonalAccount;
    using DomainModelServices.PersonalAccount.Impl;

    using Entities;

    using Export;
    using Export.Impl;

    using Gkh.Domain;
    using Gkh.Utils;

    using global::Quartz;

    using Repositories;

    using Repository;
    using Repository.Impl;

    public partial class Module
    {
        private void RegisterDDD()
        {
            this.RegisterDomainEventHandlers();

            this.RegisterModelFactories();

            this.RegisterRepositories();

            this.RegisterDomainModelServices();

            this.RegisterDistribution();

            this.Container.RegisterSessionScoped<IRealityObjectLoanRepayment, RealityObjectLoanRepayment>();
            this.Container.RegisterSessionScoped<IRealtyObjectPaymentSession, RealtyObjectPaymentSession>();

            this.Container.RegisterSessionScoped<IBankDocumentImportCheckerTask, BankDocumentImportCheckerTask>();

            Component.For<RealtyObjectEventContainer>().Instance(new RealtyObjectEventContainer()).LifestyleSingleton().RegisterIn(this.Container);

            this.RegisterQuartzTasks();

            this.Container.RegisterTransient<IChargeExportService, ChargeExportService>();
        }

        private void RegisterQuartzTasks()
        {
            ApplicationContext.Current.Events
                .GetEvent<AppStartEvent>()
                .Subscribe(e =>
                {
                    using (this.Container.BeginScope())
                    {
                        try
                        {
                            var config = this.Container.Resolve<IConfigProvider>().GetConfig();

                            if (!config.AppSettings.GetAs<bool>("Debug"))
                            {
                                var regopConfig = this.Container.GetGkhConfig<RegOperatorConfig>();
                                var bankDocCheckTime = regopConfig.GeneralConfig.BankDocumentImportCheckTime;
                                if (bankDocCheckTime.HasValue)
                                {
                                    TriggerBuilder.Create()
                                        .WithDailyTimeIntervalSchedule(
                                            x =>
                                                x.StartingDailyAt(
                                                        TimeOfDay.HourAndMinuteOfDay(bankDocCheckTime.Value.Hours, bankDocCheckTime.Value.Minutes))
                                                    .WithIntervalInHours(24))
                                        .StartNow()
                                        .ScheduleTask<QuartzTaskWrapper<IBankDocumentImportCheckerTask>>();
                                }
                            }
                        }
                        catch
                        {
                            // ignored
                        }
                    }

                });


            if (!this.Container.Resolve<IConfigProvider>().GetConfig().AppSettings.Get("RegoperatorAutoLoan").ToBool())
            {
                return;
            }

            ApplicationContext.Current.Events
                .GetEvent<AppStartEvent>()
                .Subscribe(e =>
                {
                    using (this.Container.BeginScope())
                    {
                        DateTime timeOfDay;

                        try
                        {
                            timeOfDay = this.Container.ResolveRepository<RegoperatorParam>().GetAll()
                                .FirstOrDefault(x => x.Key == "RepaymentTime")
                                .Return(x => x.Value)
                                .ToDateTime();
                        }
                        catch
                        {
                            timeOfDay = DateTime.MinValue;
                        }

                        TriggerBuilder.Create()
                            .WithDailyTimeIntervalSchedule(
                                x => x.StartingDailyAt(TimeOfDay.HourAndMinuteOfDay(timeOfDay.Hour, timeOfDay.Minute)))
                            .StartNow()
                            .ScheduleTask<QuartzTaskWrapper<IRealityObjectLoanRepayment>>();
                    }
                });
        }

        private void RegisterDistribution()
        {
            this.Container.RegisterSessionScoped<ICancellableSourceProvider, IDistributionProvider, DistributionProviderImpl>();
            this.Container.RegisterSessionScoped<IDistributionService, DistributionService>();

            this.Container.RegisterDistribution<FundSubsidyDistribution>();
            this.Container.RegisterDistribution<RefundFundSubsidyDistribution>();
            this.Container.RegisterDistribution<RegionalSubsidyDistribution>();
            this.Container.RegisterDistribution<RefundRegionalSubsidyDistribution>();
            this.Container.RegisterDistribution<StimulateSubsidyDistribution>();
            this.Container.RegisterDistribution<RefundStimulateSubsidyDistribution>();
            this.Container.RegisterDistribution<RefundTargetSubsidyDistribution>();
            this.Container.RegisterDistribution<TargetSubsidyDistribution>();
            this.Container.RegisterDistribution<OtherSourcesDistribution>();
            this.Container.RegisterDistribution<BankPercentDistribution>();

            this.Container.RegisterDistribution<TransferCrDistribution>();
            this.Container.RegisterDistribution<TransferCrROSPDistribution>();
            this.Container.RegisterDistribution<AccumulatedFundsDistribution>();
            this.Container.RegisterDistribution<PreviousWorkPaymentDistribution>();
            this.Container.RegisterDistribution<RentPaymentDistribution>();
            this.Container.RegisterDistribution<PerformedWorkActsDistribution>();
            this.Container.RegisterDistribution<RefundDistribution>();
            this.Container.RegisterDistribution<RefundMspDistribution>();
            this.Container.RegisterDistribution<RefundPenaltyDistribution>();
            this.Container.RegisterDistribution<ComissionForAccountServiceDistribution>();
            this.Container.RegisterDistribution<TransferContractorDistribution>();
            this.Container.RegisterDistribution<BuildControlPaymentDistribution>();
            this.Container.RegisterDistribution<DEDPaymentDistribution>();
            this.Container.RegisterDistribution<RestructAmicableAgreementDistribution>();
            this.Container.RegisterDistribution<RefundTransferFundsDistribution>();
            this.Container.RegisterDistribution<RefundBuilderDistribution>();
            this.Container.RegisterDistribution<SpecialAccountDistribution>();

            this.Container.RegisterTransient<IDistributionValidator, PerformedWorkActsPaymentDistributionValidator>();
            this.Container.RegisterTransient<IDistributionValidator, SpecialAccountDistributionValidator>();
            this.Container.RegisterTransient<IDistributionValidator, TransferContractorDistributionValidator>();
            this.Container.RegisterTransient<IDistributionValidator, BuildControlPaymentDistributionValidator>();
            this.Container.RegisterTransient<IDistributionValidator, DEDPaymentDistributionValidator>();
            this.Container.RegisterTransient<IDistributionValidator, PersonalAccountDistributionPaymentDocumentValidator>();
        }

        private void RegisterRepositories()
        {
            this.Container.RegisterTransient<IRealityObjectPaymentAccountRepository, RealityObjectPaymentAccountRepository>();
            this.Container.RegisterTransient<IRealityObjectChargeAccountRepository, RealityObjectChargeAccountRepository>();
            this.Container.RegisterTransient<ITransferRepository, ITransferRepository<RealityObjectTransfer>, TransferRepository<RealityObjectTransfer>>("RealityObjectTransferRepo");
            this.Container.RegisterTransient<ITransferRepository, ITransferRepository<PersonalAccountPaymentTransfer>, TransferRepository<PersonalAccountPaymentTransfer>>("PersonalAccountPaymentTransferRepo");
            this.Container.RegisterTransient<ITransferRepository, ITransferRepository<PersonalAccountChargeTransfer>, TransferRepository<PersonalAccountChargeTransfer>>("PersonalAccountChargeTransferRepo");
            this.Container.RegisterTransient<IMoneyOperationRepository, MoneyOperationRepository>();
            this.Container.RegisterTransient<IPersonalAccountRepository, PersonalAccountRepository>();
            this.Container.RegisterTransient<IWalletRepository, WalletRepository>();
            this.Container.RegisterSessionScoped<IChargePeriodRepository, ChargePeriodRepository>();
            this.Container.RegisterSessionScoped<IRealtyObjectPaymentRootRepository, RealtyObjectPaymentRootRepository>();
            this.Container.RegisterSessionScoped<IRealtyObjectMoneyRepository, RealtyObjectMoneyRepository>();
            this.Container.RegisterSessionScoped<IPersonalAccountOwnerRepository, PersonalAccountOwnerRepository>();
            this.Container.RegisterSessionScoped<IPersonalAccountChargeRepository, PersonalAccountChargeRepository>();
            this.Container.RegisterSessionScoped<IWalletOperationHistoryRepository, WalletOperationHistoryRepository>();

            this.Container.RegisterTransient<ILoanSourceRepository, LoanSourceRepository>();
            this.Container.RegisterTransient<IEntranceTariffRepository, EntranceTariffRepository>();
        }

        private void RegisterDomainEventHandlers()
        {
            this.Container.RegisterTransient<IDomainEventHandler<PersonalAccountDebtIsZeroEvent>, PersonalAccountDebtIsZeroAfterPaymentEventHandler>();

            Component
                .For<IDomainEventHandler<PersonalAccountChangeOwnerEvent>>()
                .Forward<IDomainEventHandler<PersonalAccountChangeAreaShareEvent>>()
                .Forward<IDomainEventHandler<PersonalAccountChangeDateOpenEvent>>()
                .Forward<IDomainEventHandler<PersonalAccountBalanceChangeEvent>>()
                .ImplementedBy<PersonalAccountChangeHandler>()
                .LifeStyle.Scoped()
                .RegisterIn(this.Container);


            Component
                .For<IDomainEventHandler<PersonalAccountPaymentByBaseTariffEvent>>()
                .Forward<IDomainEventHandler<PersonalAccountPaymentByDecisionEvent>>()
                .Forward<IDomainEventHandler<PersonalAccountPenaltyPaymentEvent>>()
                .Forward<IDomainEventHandler<PersonalAccountPreviousWorkPaymentEvent>>()
                .Forward<IDomainEventHandler<PersonalAccountRentPaymentEvent>>()
                .Forward<IDomainEventHandler<PersonalAccountSocialSupportPaymentEvent>>()
                .Forward<IDomainEventHandler<PersonalAccountAccumulatedFundPaymentEvent>>()
                .Forward<IDomainEventHandler<PersAccRestructAmicableAgreementPaymentEvent>>()
                .Forward<IRealtyObjectPaymentEventHandler<PersonalAccountPaymentByBaseTariffEvent>>()
                .Forward<IRealtyObjectPaymentEventHandler<PersonalAccountPaymentByDecisionEvent>>()
                .Forward<IRealtyObjectPaymentEventHandler<PersonalAccountPenaltyPaymentEvent>>()
                .Forward<IRealtyObjectPaymentEventHandler<PersonalAccountPreviousWorkPaymentEvent>>()
                .Forward<IRealtyObjectPaymentEventHandler<PersonalAccountRentPaymentEvent>>()
                .Forward<IRealtyObjectPaymentEventHandler<PersonalAccountSocialSupportPaymentEvent>>()
                .Forward<IRealtyObjectPaymentEventHandler<PersonalAccountAccumulatedFundPaymentEvent>>()
                .Forward<IRealtyObjectPaymentEventHandler<PersAccRestructAmicableAgreementPaymentEvent>>()
                .ImplementedBy<RealtyObjectPaymentHandler>()
                .LifeStyle.Scoped()
                .RegisterIn(this.Container);

            Component.For<IDomainEventHandler<PersonalAccountTariffUndoEvent>>()
                .Forward<IDomainEventHandler<PersonalAccountDecisionUndoEvent>>()
                .Forward<IDomainEventHandler<PersonalAccountPenaltyUndoEvent>>()
                .Forward<IDomainEventHandler<PersonalAccountPreviousWorkUndoEvent>>()
                .Forward<IDomainEventHandler<PersonalAccountRentUndoEvent>>()
                .Forward<IDomainEventHandler<PersonalAccountSocialSupportUndoEvent>>()
                .Forward<IDomainEventHandler<PersonalAccountAccumulatedFundUndoEvent>>()
                .Forward<IDomainEventHandler<PersAccRestructAmicableAgreementUndoEvent>>()
                .Forward<IRealtyObjectPaymentEventHandler<PersonalAccountTariffUndoEvent>>()
                .Forward<IRealtyObjectPaymentEventHandler<PersonalAccountDecisionUndoEvent>>()
                .Forward<IRealtyObjectPaymentEventHandler<PersonalAccountPenaltyUndoEvent>>()
                .Forward<IRealtyObjectPaymentEventHandler<PersonalAccountPreviousWorkUndoEvent>>()
                .Forward<IRealtyObjectPaymentEventHandler<PersonalAccountRentUndoEvent>>()
                .Forward<IRealtyObjectPaymentEventHandler<PersonalAccountSocialSupportUndoEvent>>()
                .Forward<IRealtyObjectPaymentEventHandler<PersonalAccountAccumulatedFundUndoEvent>>()
                .Forward<IRealtyObjectPaymentEventHandler<PersAccRestructAmicableAgreementUndoEvent>>()
                .ImplementedBy<RealtyObjectUndoPaymentHandler>()
                .LifeStyle.Scoped()
                .RegisterIn(this.Container);

            Component.For<IDomainEventHandler<PersonalAccountTariffRefundEvent>>()
                .Forward<IDomainEventHandler<PersonalAccountDecisionRefundEvent>>()
                .Forward<IDomainEventHandler<PersonalAccountPenaltyRefundEvent>>()
                .Forward<IRealtyObjectPaymentEventHandler<PersonalAccountTariffRefundEvent>>()
                .Forward<IRealtyObjectPaymentEventHandler<PersonalAccountDecisionRefundEvent>>()
                .Forward<IRealtyObjectPaymentEventHandler<PersonalAccountPenaltyRefundEvent>>()
                .ImplementedBy<RealtyObjectPaRefundEventHandler>()
                .LifeStyle.Scoped()
                .RegisterIn(this.Container);

            Component.For<IDomainEventHandler<PersonalAccountPenaltyChargeUndoEvent>>()
                .Forward<IRealtyObjectPaymentEventHandler<PersonalAccountPenaltyChargeUndoEvent>>()
                .ImplementedBy<RealtyObjectPersonalAccountPenaltyUndoHandler>()
                .LifeStyle.Scoped()
                .RegisterIn(this.Container);

            Component.For<IDomainEventHandler<PersonalAccountSaldoChangeMassEvent>>()
                .Forward<IRealtyObjectPaymentEventHandler<PersonalAccountSaldoChangeMassEvent>>()
                .ImplementedBy<RealityObjectPersonalAccountSaldoChangeHandler>()
                .LifeStyle.Scoped()
                .RegisterIn(this.Container);

            Component.For<IDomainEventHandler<PersonalAccountPenaltyChargeUndoEvent>>()
                .ImplementedBy<PersonalAccountMoneyChangeHandler>()
                .LifeStyle.Scoped()
                .RegisterIn(this.Container);

            Component.For<IDomainEventHandler<PersonalAccountChargeUndoEvent>>()
                .Forward<IRealtyObjectPaymentEventHandler<PersonalAccountChargeUndoEvent>>()
                .ImplementedBy<RealtyObjectPersonalAccountChargeUndoHandler>()
                .LifeStyle.Scoped()
                .RegisterIn(this.Container);

            Component
                .For<IDomainEventHandler<PersonalAccountCloseEvent>>()
                .ImplementedBy<PersonalAccountRecalcHandler>()
                .LifeStyle.Scoped()
                .RegisterIn(this.Container);

            Component
                .For<IDomainEventHandler<RealityObjectLoanTaskEndEvent>>()
                .Forward<IDomainEventHandler<RealityObjectLoanTaskStartEvent>>()
                .ImplementedBy<RealityObjectLoanTaskHandler>()
                .LifeStyle.Scoped()
                .RegisterIn(this.Container);

            this.Container.RegisterSessionScoped<IPersonalAccountRecalcEventManager, PersonalAccountRecalcEventManager>();
            this.Container.RegisterSessionScoped<IPersonalAccountBanRecalcManager, PersonalAccountBanRecalcManager>();

            Component
                .For<IDomainEventHandler<BasePersonalAccountDtoEvent>>()
                .Forward<IDomainEventHandler<PersonalAccountChangeOwnerDtoEvent>>()
                .Forward<IDomainEventHandler<PersonalAccountOwnerUpdateEvent>>()
                .Forward<IDomainEventHandler<RoomChangeEvent>>()
                .Forward<IDomainEventHandler<RealityObjectForDtoChangeEvent>>()
                .Forward<IDomainEventHandler<MunicipalityChangeEvent>>()
                .ImplementedBy<PersonalAccountDtoChangeHandler>()
                .LifeStyle.Scoped()
                .RegisterIn(this.Container);

            // создаём DTO при старте приложения
            ApplicationContext.Current.Events
                .GetEvent<AppStartEvent>()
                .Subscribe(e =>
                {
                    using (this.Container.BeginScope())
                    {
                        if (ApplicationContext.Current.IsDebug || ApplicationContext.Current.GetContextType() != ApplicationContextType.WebApplication)
                        {
                            return;
                        }

                        // когда самый первый раз стартуем, создаём DTO, не перезаписывая
                        var massPersonalAccountDtoService = this.Container.Resolve<IMassPersonalAccountDtoService>();
                        using (this.Container.Using(massPersonalAccountDtoService))
                        {
                            massPersonalAccountDtoService.MassCreatePersonalAccountDto(false);
                        }
                    }
                });
        }

        private void RegisterModelFactories()
        {
            this.Container.RegisterTransient<IPersonalAccountFactory, PersonalAccountFactory>();
            this.Container.RegisterSingleton<IPersonalAccountPaymentCommandFactory, PersonalAccountPaymentCommandFactory>();
            this.Container.RegisterSingleton<IPersonalAccountRefundCommandFactory, PersonalAccountRefundCommandFactory>();
        }

        private void RegisterDomainModelServices()
        {
            #region PersonalAccount
            this.Container.RegisterTransient<IPersonalAccountCreateService, PersonalAccountCreateService>();
            this.Container.RegisterTransient<IPersonalAccountChangeService, PersonalAccountChangeService>();
            this.Container.RegisterTransient<IPersonalAccountBalanceUpdater, PersonalAccountBalanceUpdater>();
            this.Container.RegisterTransient<IRealityObjectAccountUpdater, RealityObjectAccountUpdater>();
            this.Container.RegisterTransient<IRealityObjectLoanReserver, RealityObjectLoanReserver>();

            this.Container.RegisterTransient<IPersonalAccountDetailService, PersonalAccountDetailService>();

            this.Container.RegisterTransient<IPersonalAccountFilterService, PersonalAccountFilterService>();

            #endregion PersonalAccount

            #region Room
            this.Container.RegisterTransient<IRoomAreaShareSpecification, RoomAreaShareSpecification>();

            #endregion

            this.Container.RegisterTransient<ICalcAccountMoneyService, CalcAccountMoneyService>();

            this.Container.RegisterTransient<IPersonalAccountHistoryCreator, PersonalAccountHistoryCreator>();
            
            this.Container.RegisterTransient<IWalletBalanceService, WalletBalanceService>();

            this.Container.RegisterTransient<ITakeLoanService, TakeLoanService>();
            this.Container.RegisterTransient<ILoanReserver, LoanReserver>();

            this.Container.RegisterService<ICancellableProviderFactory, CancellableProviderFactory>();
            this.Container.RegisterService<IPartialOperationCancellationService, PartialOperationCancellationService>();
        }
    }

    public static class ContainerExtensions
    {
        public static void RegisterSessionScoped<TInterface, TImpl>(this IWindsorContainer container) where TImpl : TInterface where TInterface : class
        {
            Component.For<TInterface>()
                .ImplementedBy<TImpl>()
                .LifeStyle.Scoped()
                .RegisterIn(container);
        }
    }
}