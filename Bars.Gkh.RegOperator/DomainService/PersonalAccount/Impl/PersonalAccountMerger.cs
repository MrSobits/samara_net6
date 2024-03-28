namespace Bars.Gkh.RegOperator.DomainService.PersonalAccount.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using B4;
    using B4.Application;
    using B4.DataAccess;
    using B4.IoC;
    using B4.Modules.Security;
    using B4.Modules.States;
    using B4.Utils;

    using Bars.Gkh.Domain.DatabaseMutex;
    using Bars.Gkh.RegOperator.DomainModelServices.PersonalAccount;
    using Bars.Gkh.Repositories.ChargePeriod;

    using Castle.Windsor;

    using Domain.Repository.Wallets;
    using Domain.ValueObjects;

    using DomainModelServices;
    using DomainModelServices.Impl;

    using Entities;
    using Entities.PersonalAccount;
    using Entities.ValueObjects;
    using Entities.Wallet;

    using Enums;

    using Exceptions;

    using Gkh.Domain;
    using Gkh.Domain.CollectionExtensions;
    using Gkh.Entities;

    /// <summary>
    /// Сервис слияния ЛС
    /// </summary>
    public class PersonalAccountMerger : IPersonalAccountMerger, IChargeOriginator // TODO: Create charge source!
    {
        /// <summary>
        /// Идентификатор операций
        /// </summary>
        public static string TransferGuid = "0CFE40F2-184E-4F4B-A721-AED27C51C13C";

        /// <inheritdoc />
        string ITransferParty.TransferGuid => PersonalAccountMerger.TransferGuid;

        /// <inheritdoc />
        public MoneyOperation CreateOperation(ChargePeriod period)
        {
            return new MoneyOperation(this.OriginatorGuid, period) { Reason = "Перенос долга при слиянии" };
        }

        /// <inheritdoc />
        public TypeChargeSource ChargeSource => TypeChargeSource.MergeCharge;

        /// <inheritdoc />
        public string OriginatorGuid => PersonalAccountMerger.TransferGuid;

        private IWindsorContainer Container => ApplicationContext.Current.Container;

        public IDataResult MergeAccounts(BaseParams baseParams)
        {
            var personalAccountMerger = this.Container.Resolve<IPersonalAccountMerger>();

            try
            {
                personalAccountMerger.Merge(PersonalAccountMergeArgs.FromParams(baseParams));

                return new BaseDataResult(true, "Слияние счетов прошло успешно!");
            }
            catch (PersonalAccountOperationException ex)
            {
                return BaseDataResult.Error(ex.Message);
            }
            catch (DatabaseMutexException)
            {
                return BaseDataResult.Error("Слияние лицевых счетов уже запущен");
            }
            finally
            {
                this.Container.Release(personalAccountMerger);
            }

        }

        /// <summary>
        /// Выполнить слияние
        /// </summary>
        /// <param name="args">Аргументы-параметры слияния аккаунтов</param>
        public void Merge(PersonalAccountMergeArgs args)
        {
            this.Container.InTransaction(() =>
                {
                    using (new DatabaseMutexContext("Merge_Account", "Слияние лицевых счетов"))
                    {
                        this.MergeInternal(args);
                    }
                });
        }

        private void MergeInternal(PersonalAccountMergeArgs args)
        {
            var transferRepo = this.Container.ResolveDomain<Transfer>();
            var operationDomain = this.Container.Resolve<IDomainService<MoneyOperation>>();
            var walletRepo = this.Container.Resolve<IWalletRepository>();
            var accountDomain = this.Container.Resolve<IDomainService<BasePersonalAccount>>();
            var period = this.Container.Resolve<IChargePeriodRepository>().GetCurrentPeriod();
            var stateService = this.Container.Resolve<IDomainService<State>>();
            var periodSummaryService = this.Container.Resolve<IDomainService<PersonalAccountPeriodSummary>>();
            var historyService = this.Container.Resolve<IPersonalAccountHistoryCreator>();
            var changeDomain = this.Container.ResolveDomain<PersonalAccountChange>();
            var userRepo = this.Container.ResolveDomain<User>();
            var userIdentity = this.Container.Resolve<IUserIdentity>();
            var logDomain = this.Container.ResolveDomain<EntityLogLight>();

            using (this.Container.Using(
                transferRepo, 
                operationDomain, 
                walletRepo, 
                accountDomain, 
                stateService,
                periodSummaryService, 
                historyService, 
                changeDomain, 
                userRepo, 
                userIdentity, 
                logDomain))
            {
                var closeDate = period.StartDate;
                var closeState = stateService.GetAll().FirstOrDefault(x => x.FinalState && x.TypeId == "gkh_regop_personal_account");
                var transfers = new List<Transfer>();
                var wallets = new List<Wallet>();
                var updateSummaries = new List<PersonalAccountPeriodSummary>();
                var operation = this.CreateOperation(period);
                operationDomain.Save(operation);
                var oldAreaShare = 0M;
                var newAreaShare = 0M;
                var histories = new List<PersonalAccountChange>();

                foreach (var closingItem in args.ClosingItems)
                {
                    oldAreaShare += closingItem.PersonalAccount.AreaShare;
                    newAreaShare += closingItem.NewShare;
                }

                foreach (var recipientItem in args.RecipientItems)
                {
                    oldAreaShare += recipientItem.PersonalAccount.AreaShare;
                    newAreaShare += recipientItem.NewShare;
                }

                if (Math.Abs(oldAreaShare - newAreaShare) > 0.001M)
                {
                    throw new PersonalAccountOperationException("Сумма новых долей собственности должны быть равна сумме старых долей собственности");
                }

                foreach (var closingItem in args.ClosingItems)
                {
                    var ciPeriodSummary = closingItem.PersonalAccount.GetOpenedPeriodSummary();

                    var summaries = closingItem.PersonalAccount.Summaries.ToList();
                    var debtBaseTariff = summaries.SafeSum(x => x.GetBaseTariffDebt());
                    var debtDecisionTariff = summaries.SafeSum(x => x.GetDecisionTariffDebt());
                    var debtPenalty = summaries.SafeSum(x => x.GetPenaltyDebt());

                    var closingItemSaldoBefore = ciPeriodSummary.SaldoOut;
                    foreach (var recipientItem in args.RecipientItems)
                    {
                        var riPeriodSummary = recipientItem.PersonalAccount.GetOpenedPeriodSummary();

                        var mergeCoef = args.GetMergeKoeff(recipientItem);
                        var recipientSaldoBefore = riPeriodSummary.SaldoOut;

                        // base tariff
                        var amount = mergeCoef * debtBaseTariff;
                        var source = amount < 0 ? (ITransferParty)new PaymentOriginatorWrapper(this, operation) : this;

                        transfers.AddRange(closingItem.PersonalAccount.MoveMoney(
                            closingItem.PersonalAccount.BaseTariffWallet,
                            recipientItem.PersonalAccount,
                            recipientItem.PersonalAccount.BaseTariffWallet,
                            operation,
                            source,
                            amount, 
                            period.StartDate));

                        wallets.Add(closingItem.PersonalAccount.BaseTariffWallet);
                        wallets.Add(recipientItem.PersonalAccount.BaseTariffWallet);

                        closingItem.PersonalAccount
                            .MoveBaseTariffCharge(
                                recipientItem.PersonalAccount,
                                debtBaseTariff * mergeCoef);

                        // decision tariff
                        amount = mergeCoef * debtDecisionTariff;
                        source = amount < 0 ? (ITransferParty)new PaymentOriginatorWrapper(this, operation) : this;
                        transfers.AddRange(closingItem.PersonalAccount.MoveMoney(
                            closingItem.PersonalAccount.DecisionTariffWallet,
                            recipientItem.PersonalAccount,
                            recipientItem.PersonalAccount.DecisionTariffWallet,
                            operation,
                            source,
                            amount, 
                            period.StartDate));

                        wallets.Add(closingItem.PersonalAccount.DecisionTariffWallet);
                        wallets.Add(recipientItem.PersonalAccount.DecisionTariffWallet);

                        closingItem.PersonalAccount
                            .MoveDecisionTariffCharge(
                                recipientItem.PersonalAccount,
                                debtDecisionTariff * mergeCoef);

                        // penalty
                        amount = mergeCoef * debtPenalty;
                        source = amount < 0 ? (ITransferParty)new PaymentOriginatorWrapper(this, operation) : this;
                        transfers.AddRange(closingItem.PersonalAccount.MoveMoney(
                            closingItem.PersonalAccount.PenaltyWallet,
                            recipientItem.PersonalAccount,
                            recipientItem.PersonalAccount.PenaltyWallet,
                            operation,
                            source,
                            amount, 
                            period.StartDate));

                        wallets.Add(closingItem.PersonalAccount.PenaltyWallet);
                        wallets.Add(recipientItem.PersonalAccount.PenaltyWallet);

                        closingItem.PersonalAccount
                            .MovePenaltyCharge(
                                recipientItem.PersonalAccount,
                                debtPenalty * mergeCoef);

                        histories.Add(
                            historyService.CreateChange(
                                recipientItem.PersonalAccount,
                                PersonalAccountChangeType.MergeAndSaldoChange,
                                "Изменение сальдо в связи со слиянием ЛС",
                                riPeriodSummary.SaldoOut.ToString(),
                                recipientSaldoBefore.ToString(),
                                period.StartDate.Date,
                                args.ChangeInfo.Document,
                                args.ChangeInfo.Reason));

                        updateSummaries.Add(riPeriodSummary);
                    }

                    histories.Add(
                        historyService.CreateChange(
                            closingItem.PersonalAccount,
                            PersonalAccountChangeType.MergeAndClose,
                            "Изменение сальдо в связи со слиянием ЛС",
                            ciPeriodSummary.SaldoOut.ToString(),
                            closingItemSaldoBefore.ToString(),
                            period.StartDate.Date,
                            args.ChangeInfo.Document,
                            args.ChangeInfo.Reason));

                    updateSummaries.Add(ciPeriodSummary);
                }

                histories.Where(x => x != null).ForEach(changeDomain.Save);
                transfers.Where(x => x != null).ForEach(x =>
                {
                    x.TargetCoef = -1;
                    x.IsAffect = x.Amount < 0;
                    transferRepo.SaveOrUpdate(x);
                });

                wallets.Where(x => x != null).ForEach(walletRepo.SaveOrUpdate);
                updateSummaries.Where(x => x != null).ForEach(periodSummaryService.Update);

                foreach (var closingItem in args.ClosingItems)
                {
                    var specification = new RoomAreaShareSpecification();
                    if (!specification.ValidateAreaShare(closingItem.PersonalAccount, closingItem.PersonalAccount.Room, closingItem.NewShare, period.StartDate.Date))
                    {
                        throw new ArgumentException("Общая доля собственности в помещении превышает максимальное значение");
                    }

                    var login = userRepo.Get(userIdentity.UserId).Return(u => u.Login);

                    logDomain.Save(new EntityLogLight
                    {
                        ClassName = "BasePersonalAccount",
                        EntityId = closingItem.PersonalAccount.Id,
                        PropertyDescription = "Изменение доли собственности в связи со слиянием ЛС",
                        PropertyName = "AreaShare",
                        PropertyValue = closingItem.NewShare.ToStr(),
                        DateActualChange = period.StartDate.Date,
                        DateApplied = DateTime.UtcNow,
                        Document = args.ChangeInfo.Document,
                        Reason = args.ChangeInfo.Reason,
                        ParameterName = "area_share",
                        User = login.IsEmpty() ? "anonymous" : login
                    });

                    closingItem.PersonalAccount.AreaShare = closingItem.NewShare;
                    closingItem.PersonalAccount.SetCloseDate(closeDate, false);
                    closingItem.PersonalAccount.State = closeState;
                    
                    accountDomain.Update(closingItem.PersonalAccount);
                }

                foreach (var recipientItem in args.RecipientItems)
                {
                    var specification = new RoomAreaShareSpecification();
                    if (!specification.ValidateAreaShare(recipientItem.PersonalAccount, recipientItem.PersonalAccount.Room, recipientItem.NewShare, period.StartDate.Date))
                    {
                        throw new ArgumentException("Общая доля собственности в помещении превышает максимальное значение");
                    }

                    var login = userRepo.Get(userIdentity.UserId).Return(u => u.Login);

                    logDomain.Save(new EntityLogLight
                    {
                        ClassName = "BasePersonalAccount",
                        EntityId = recipientItem.PersonalAccount.Id,
                        PropertyDescription = "Изменение доли собственности в связи со слиянием ЛС",
                        PropertyName = "AreaShare",
                        PropertyValue = recipientItem.NewShare.ToStr(),
                        DateActualChange = period.StartDate.Date,
                        DateApplied = DateTime.UtcNow,
                        Document = args.ChangeInfo.Document,
                        Reason = args.ChangeInfo.Reason,
                        ParameterName = "area_share",
                        User = login.IsEmpty() ? "anonymous" : login
                    });

                    recipientItem.PersonalAccount.AreaShare = recipientItem.NewShare;
                    accountDomain.Update(recipientItem.PersonalAccount);
                }
            }
        }
    }
}