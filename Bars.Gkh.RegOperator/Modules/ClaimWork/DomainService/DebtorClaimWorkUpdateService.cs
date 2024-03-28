namespace Bars.Gkh.Modules.ClaimWork.DomainService.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;

    using B4;
    using B4.DataAccess;
    using B4.IoC;
    using B4.Modules.States;
    using B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount;
    using Microsoft.Extensions.Logging;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Modules.ClaimWork.Contracts;
    using Bars.Gkh.Utils;
    using Bars.Gkh.RegOperator.DomainService.PersonalAccount;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Entities.Owner;
    using Bars.Gkh.RegOperator.Entities.ValueObjects;
    using Bars.Gkh.RegOperator.Enums;
    using Bars.Gkh.RegOperator.Modules.ClaimWork.DomainService;
    using Bars.Gkh.RegOperator.Modules.ClaimWork.DomainService.Lawsuit;
    using Bars.Gkh.RegOperator.Modules.ClaimWork.DomainService.States.TransitionRules;

    using Castle.Core;
    using Castle.Windsor;

    using Gkh.ClaimWork.Entities;
    using Config;
    using Domain;
    using Domain.CollectionExtensions;
    using States;
    using Entities;
    using Enums;
    using Repositories;

    using Lawsuit;
    using ConfigSections.RegOperator;

    /// <summary>
    /// Основной сервис, отвечающий за создание и обновление ПИР
    /// </summary>
    public class DebtorClaimWorkUpdateService : IDebtorClaimWorkUpdateService, IInitializable
    {
        private State defaultState;
        private State finalState;
        private State startedEnforcementState;

        private ILawsuitAutoSelector LawsuitAutoSelector { get; set; }

        public IWindsorContainer Container { get; set; }
        public ILogger LogManager { get; set; }

        public IDebtorCalcService DebtorCalcService { get; set; }
        public IRestructDebtScheduleService RestructDebtScheduleService { get; set; }

        private ThreadLocal<Cache> dataCache;

        /// <inheritdoc />
        public IDataResult CreateClaimWorks(IQueryable<BasePersonalAccount> accountQuery)
        {
            var claimWorkAccountDetailDomain = this.Container.ResolveDomain<ClaimWorkAccountDetail>();
            var stateProvider = this.Container.Resolve<IStateProvider>();
            var lawsuitInfoService = this.Container.Resolve<ILawsuitInfoService>();
            var stateDomain = this.Container.ResolveDomain<State>();
            var userManager = this.Container.Resolve<IGkhUserManager>();

            try
            {
                //ПИР с датой рассмотрения в исковом заявлении не обновляются и не создаются заново.
                var claimWorkHasReviewDate = lawsuitInfoService.GetClaimWorkQueryHasReviewDate()
                    .Select(x => ((DebtorClaimWork)x).AccountOwner.Id)
                    .Distinct()
                    .ToHashSet();
                
                var detailsToUpdate = new List<ClaimWorkAccountDetail>();
                var detailsToCreate = new List<ClaimWorkAccountDetail>();
                var claimworksToCreate = new Dictionary<long, DebtorClaimWork>();
                
                var existClaimWorkDetails = claimWorkAccountDetailDomain.GetAll()
                    .Where(x => !x.ClaimWork.State.FinalState && !x.ClaimWork.IsDebtPaid)
                    .GroupBy(x => x.PersonalAccount.Id)
                    .ToDictionary(x => x.Key, x => x.First());

                var debtors = this.DebtorCalcService.GetDebtorsInfo(accountQuery);

                debtors.Values.ForEach(x =>
                {
                    var existDetail = existClaimWorkDetails.Get(x.PersonalAccountId);
                    if (existDetail == null || existDetail.ObjectCreateDate <= DateTime.Now.AddYears(-2))
                    {
                        var claimwork = claimworksToCreate.Get(x.OwnerId);
                        if (claimwork == null)
                        {
                            claimwork = this.CreateDebtorClaimWork(x, userManager);
                            var accDomain = this.Container.ResolveDomain<PersonalAccountOwner>();
                            try
                            {
                                var account = accDomain.GetAll().Where(zx => zx.Id == claimwork.AccountOwner.Id).FirstOrDefault();
                                claimwork.RealityObject = account.Accounts.FirstOrDefault().Room.RealityObject;
                            }
                            catch
                            {
                                claimwork.RealityObject = null;
                            }
                            claimworksToCreate.Add(claimwork.AccountOwner.Id, claimwork);
                        }

                        claimwork.OrigChargeBaseTariffDebt += x.DebtBaseTariffSum;
                        claimwork.OrigChargeDecisionTariffDebt += x.DebtDecisionTariffSum;
                        claimwork.OrigChargeDebt += x.DebtSum;
                        claimwork.OrigPenaltyDebt += x.PenaltyDebt;

                        detailsToCreate.Add(new ClaimWorkAccountDetail
                        {
                            ClaimWork = claimwork,
                            CountDaysDelay = x.ExpiredDaysCount,
                            CountMonthDelay = x.ExpiredMonthCount,
                            CurrChargeBaseTariffDebt = x.DebtBaseTariffSum,
                            CurrChargeDecisionTariffDebt = x.DebtDecisionTariffSum,
                            CurrChargeDebt = x.DebtSum,
                            CurrPenaltyDebt = x.PenaltyDebt,
                            OrigChargeBaseTariffDebt = x.DebtBaseTariffSum,
                            OrigChargeDecisionTariffDebt = x.DebtDecisionTariffSum,
                            OrigChargeDebt = x.DebtSum,
                            OrigPenaltyDebt = x.PenaltyDebt,
                            StartingDate = x.StartDate,
                            PersonalAccount = new BasePersonalAccount
                            {
                                Id = x.PersonalAccountId
                            }
                        });
                    }
                    else
                    {
                        //не обновляем и не создаем ПИР, для которых ИЗ с проставленной датой рассмотрения
                        if (!claimWorkHasReviewDate.Contains(x.OwnerId))
                        {
                            existDetail.CurrChargeBaseTariffDebt = x.DebtBaseTariffSum;
                            existDetail.CurrChargeDecisionTariffDebt = x.DebtDecisionTariffSum;
                            existDetail.CurrChargeDebt = x.DebtSum;
                            existDetail.CurrPenaltyDebt = x.PenaltyDebt;
                            existDetail.StartingDate = x.StartDate;
                            existDetail.CountDaysDelay = x.ExpiredDaysCount;
                            existDetail.CountMonthDelay = x.ExpiredMonthCount;

                            detailsToUpdate.Add(existDetail);
                        }
                    }
                });

                var unProxy = this.Container.Resolve<IUnProxy>();

                claimworksToCreate.Values.ForEach(
                    x =>
                    {
                        x.CurrChargeBaseTariffDebt = x.OrigChargeBaseTariffDebt;
                        x.CurrChargeDecisionTariffDebt = x.OrigChargeDecisionTariffDebt;
                        x.CurrChargeDebt = x.OrigChargeDebt;
                        x.CurrPenaltyDebt = x.OrigPenaltyDebt;
                    });

                var claimworksToUpdate = detailsToUpdate
                    .Select(x => unProxy.GetUnProxyObject(x.ClaimWork) as DebtorClaimWork)
                    .Distinct(x => x.Id)
                    .ToList();
                this.RecalcClaimWorks(claimworksToUpdate);

                TransactionHelper.InsertInManyTransactions(this.Container, claimworksToCreate.Values);
                TransactionHelper.InsertInManyTransactions(this.Container, claimworksToUpdate);
                TransactionHelper.InsertInManyTransactions(this.Container, detailsToCreate);
                TransactionHelper.InsertInManyTransactions(this.Container, detailsToUpdate);

                return new BaseDataResult();
            }
            finally
            {
                this.Container.Release(claimWorkAccountDetailDomain);
                this.Container.Release(lawsuitInfoService);
                this.Container.Release(stateProvider);
                this.Container.Release(stateDomain);
                this.Container.Release(userManager);
            }
        }
        /*
        // Для безумного Воронежа
        /// <summary>
        /// Создание ПИР для долевых собственников
        /// </summary>
        /// <param name="accountQuery"></param>
        /// <returns></returns>
        public IDataResult CreateClaimWorkPartial(long individualOwnerInfoId)
        {
            var claimWorkAccountDetailDomain = this.Container.ResolveDomain<ClaimWorkAccountDetail>();
            var stateProvider = this.Container.Resolve<IStateProvider>();
            var lawsuitInfoService = this.Container.Resolve<ILawsuitInfoService>();
            var stateDomain = this.Container.ResolveDomain<State>();
            var userManager = this.Container.Resolve<IGkhUserManager>();
            var lawsuitIndOwnerDomain = this.Container.ResolveDomain<LawsuitIndividualOwnerInfo>();
            try
            {
                var indOwner = lawsuitIndOwnerDomain.Get(individualOwnerInfoId);

                (indOwner.Lawsuit.ClaimWork as DebtorClaimWork).AccountDetails.GetType();
                var detailsToCreate = new List<ClaimWorkAccountDetail>();
                var claimworksToCreate = new Dictionary<long, DebtorClaimWork>();
                
                debtors.Values.ForEach(x =>
                {
                    var existDetail = existClaimWorkDetails.Get(x.PersonalAccountId);
                    if (existDetail == null)
                    {
                        var claimwork = claimworksToCreate.Get(x.OwnerId);
                        if (claimwork == null)
                        {
                            claimwork = this.CreateDebtorClaimWork(x, userManager);
                            var accDomain = this.Container.ResolveDomain<PersonalAccountOwner>();
                            try
                            {
                                var account = accDomain.GetAll().Where(zx => zx.Id == claimwork.AccountOwner.Id).FirstOrDefault();
                                claimwork.RealityObject = account.Accounts.FirstOrDefault().Room.RealityObject;
                            }
                            catch
                            {
                                claimwork.RealityObject = null;
                            }
                            claimworksToCreate.Add(claimwork.AccountOwner.Id, claimwork);
                        }

                        claimwork.OrigChargeBaseTariffDebt += x.DebtBaseTariffSum;
                        claimwork.OrigChargeDecisionTariffDebt += x.DebtDecisionTariffSum;
                        claimwork.OrigChargeDebt += x.DebtSum;
                        claimwork.OrigPenaltyDebt += x.PenaltyDebt;

                        detailsToCreate.Add(new ClaimWorkAccountDetail
                        {
                            ClaimWork = claimwork,
                            CountDaysDelay = x.ExpiredDaysCount,
                            CountMonthDelay = x.ExpiredMonthCount,
                            CurrChargeBaseTariffDebt = x.DebtBaseTariffSum,
                            CurrChargeDecisionTariffDebt = x.DebtDecisionTariffSum,
                            CurrChargeDebt = x.DebtSum,
                            CurrPenaltyDebt = x.PenaltyDebt,
                            OrigChargeBaseTariffDebt = x.DebtBaseTariffSum,
                            OrigChargeDecisionTariffDebt = x.DebtDecisionTariffSum,
                            OrigChargeDebt = x.DebtSum,
                            OrigPenaltyDebt = x.PenaltyDebt,
                            StartingDate = x.StartDate,
                            PersonalAccount = new BasePersonalAccount
                            {
                                Id = x.PersonalAccountId
                            }
                        });
                    }
                    else
                    {
                        //не обновляем и не создаем ПИР, для которых ИЗ с проставленной датой рассмотрения
                        {
                            existDetail.CurrChargeBaseTariffDebt = x.DebtBaseTariffSum;
                            existDetail.CurrChargeDecisionTariffDebt = x.DebtDecisionTariffSum;
                            existDetail.CurrChargeDebt = x.DebtSum;
                            existDetail.CurrPenaltyDebt = x.PenaltyDebt;
                            existDetail.StartingDate = x.StartDate;
                            existDetail.CountDaysDelay = x.ExpiredDaysCount;
                            existDetail.CountMonthDelay = x.ExpiredMonthCount;

                        }
                    }
                });

                claimworksToCreate.Values.ForEach(
                    x =>
                    {
                        x.CurrChargeBaseTariffDebt = x.OrigChargeBaseTariffDebt;
                        x.CurrChargeDecisionTariffDebt = x.OrigChargeDecisionTariffDebt;
                        x.CurrChargeDebt = x.OrigChargeDebt;
                        x.CurrPenaltyDebt = x.OrigPenaltyDebt;
                    });

                TransactionHelper.InsertInManyTransactions(this.Container, claimworksToCreate.Values);
                TransactionHelper.InsertInManyTransactions(this.Container, detailsToCreate);

                return new BaseDataResult();
            }
            finally
            {
                this.Container.Release(claimWorkAccountDetailDomain);
                this.Container.Release(lawsuitInfoService);
                this.Container.Release(stateProvider);
                this.Container.Release(stateDomain);
                this.Container.Release(userManager);
                this.Container.Release(lawsuitIndOwnerDomain);
            }
        }
*/
        private DebtorClaimWork CreateDebtorClaimWork(DebtorInfo debtorInfo, IGkhUserManager userManager)
        {
            DebtorClaimWork result;
            switch (debtorInfo.OwnerType)
            {
                case PersonalAccountOwnerType.Individual:
                    result = new IndividualClaimWork
                    {
                        AccountOwner = new PersonalAccountOwner { Id = debtorInfo.OwnerId },
                        User = userManager.GetActiveUser(),
                        State = this.defaultState
                    };
                    break;
                case PersonalAccountOwnerType.Legal:
                    result = new LegalClaimWork
                    {
                        AccountOwner = new PersonalAccountOwner { Id = debtorInfo.OwnerId },
                        User = userManager.GetActiveUser(),
                        State = this.defaultState
                    };
                    break;
                default:
                    throw new Exception("Не удалось определить тип абонента");
            }

            var name = debtorInfo.OwnerName;
            if (!string.IsNullOrEmpty(name))
            {
                result.BaseInfo = $"Абонент {name}";
            }

            return result;
        }

        /// <inheritdoc />
        public void RecalcClaimWorks(IEnumerable<DebtorClaimWork> claimworks)
        {
            claimworks.ForEach(
                x =>
                {
                    x.CurrChargeBaseTariffDebt = x.AccountDetails.SafeSum(y => y.CurrChargeBaseTariffDebt);
                    x.CurrChargeDecisionTariffDebt = x.AccountDetails.SafeSum(y => y.CurrChargeDecisionTariffDebt);
                    x.CurrChargeDebt = x.AccountDetails.SafeSum(y => y.CurrChargeDebt);
                    x.CurrPenaltyDebt = x.AccountDetails.SafeSum(y => y.CurrPenaltyDebt);
                    x.OrigChargeBaseTariffDebt = x.AccountDetails.SafeSum(y => y.OrigChargeBaseTariffDebt);
                    x.OrigChargeDecisionTariffDebt = x.AccountDetails.SafeSum(y => y.OrigChargeDecisionTariffDebt);
                    x.OrigChargeDebt = x.AccountDetails.SafeSum(y => y.OrigChargeDebt);
                    x.OrigPenaltyDebt = x.AccountDetails.SafeSum(y => y.OrigPenaltyDebt);

                });
        }

        /// <inheritdoc />
        public IDataResult UpdateStates(BaseParams baseParams)
        {
            var id = baseParams.Params.GetAsId();
            if (id > 0)
            {
                return this.UpdateStates(new[] {id});
            }

            var ids = baseParams.Params.GetAs<string>("ids").ToLongArray();
            return this.UpdateStates(ids);
        }



        /// <inheritdoc />
        public IDataResult UpdateStates(long[] ids = null)
        {
            if (this.finalState.IsNull())
            {
                return BaseDataResult.Error("Отсутствует конечный статус");
            }

            if (this.startedEnforcementState.IsNull())
            {
                return BaseDataResult.Error($"Не найден статус '{ClaimWorkStates.StartedEnforcement}'");
            }

            var debtorClaimWorkDomain = this.Container.ResolveDomain<DebtorClaimWork>();
            var lawsuitDomain = this.Container.ResolveDomain<Lawsuit>();
            var accountDetailDomain = this.Container.ResolveDomain<ClaimWorkAccountDetail>();
            var personalAccountPeriodSummaryDomain = this.Container.ResolveDomain<PersonalAccountPeriodSummary>();
            var periodDomain = this.Container.ResolveDomain<ChargePeriod>();
            var transferDomain = this.Container.ResolveDomain<PersonalAccountChargeTransfer>();
            var configProv = this.Container.Resolve<IGkhConfigProvider>();
            var stateRepo = this.Container.Resolve<IStateRepository>();

            var debtorConfig = configProv.Get<RegOperatorConfig>().DebtorConfig.DebtorRegistryConfig;

            using (this.Container.Using(debtorClaimWorkDomain, accountDetailDomain, lawsuitDomain, configProv, stateRepo))
            {
                var debtorClaimWorkToUpd = new List<DebtorClaimWork>();
                var accountDetailsToUpd = new List<ClaimWorkAccountDetail>();
                var entitiesToSave = new HashSet<BaseEntity>();

                var isMassUpdate = ids?.Length != 1;
                var debtorClaimWorkQuery = debtorClaimWorkDomain.GetAll()
                    .WhereIf(ids != null && ids.Length > 0, x => ids.Contains(x.Id))
                    .WhereIf(isMassUpdate, x => !x.State.FinalState);

                var accountDetailQuery = accountDetailDomain.GetAll()
                    .Where(x => debtorClaimWorkQuery.Any(y => y.Id == x.ClaimWork.Id));

              //  var debtorsInfo = this.DebtorCalcService.GetDebtorsInfo(accountDetailQuery.Select(x => x.PersonalAccount));

                var claimWorks = debtorClaimWorkQuery
                    .ToList();

                var accountDetails = accountDetailQuery
                    .GroupBy(x => x.ClaimWork.Id)
                    .ToDictionary(x => x.Key, x => x.ToList());

                this.dataCache = new ThreadLocal<Cache>(() => new Cache(this.Container, accountDetailQuery, claimWorks));
                foreach (var clw in claimWorks)
                {
                    var needToUpdate = false;
                    var detailsByClw = accountDetails.ContainsKey(clw.Id) ? accountDetails[clw.Id] : new List<ClaimWorkAccountDetail>();
                    var calcDate = clw.PIRCreateDate.HasValue ? clw.PIRCreateDate.Value : DateTime.Now;
                    ChargePeriod summaryPeriod = null;
                    if (calcDate.Day > 25)
                    {
                        summaryPeriod = periodDomain.GetAll()
                        .Where(x => x.EndDate.HasValue)
                        .Where(x => x.StartDate <= calcDate.AddMonths(-1).Date && x.EndDate >= calcDate.AddMonths(-1).Date).FirstOrDefault();
                    }
                    else
                    {
                        summaryPeriod = periodDomain.GetAll()
                       .Where(x => x.EndDate.HasValue)
                       .Where(x => x.StartDate <= calcDate.AddMonths(-2).Date && x.EndDate >= calcDate.AddMonths(-2).Date).FirstOrDefault();
                    }
                    if (summaryPeriod == null)
                    {
                        summaryPeriod = periodDomain.GetAll()
                        .Where(x => x.EndDate.HasValue)
                        .OrderByDescending(x => x.Id)
                        .FirstOrDefault();
                    }
                    decimal currChargeBaseTariffDebt = 0;
                    decimal currChargeDecisionTariffDebt = 0;
                    decimal currChargeDebt = 0;
                    decimal currPenaltyDebt = 0;
                    decimal cancelChargesBt = 0;
                    decimal saldoChangeBt = 0;
                    decimal cancelChargesDt = 0;
                    decimal cancelChargesPenalty = 0;
                    decimal basePayments = 0;
                    decimal penaltyPayments = 0;
                    decimal decisionPayments = 0;

                    foreach (var accountDetail in detailsByClw)
                    {
                        var currentSummary = personalAccountPeriodSummaryDomain.GetAll()
                            .Where(x => x.PersonalAccount == accountDetail.PersonalAccount)
                            .Where(x => x.Period == summaryPeriod).FirstOrDefault();
                        decimal accChargeBaseTariffDebt = currentSummary.BaseTariffDebt + currentSummary.GetBaseTariffDebt();
                        currChargeBaseTariffDebt += accChargeBaseTariffDebt;
                        decimal accChargeDecisionTariffDebt = currentSummary.DecisionTariffDebt + currentSummary.GetDecisionTariffDebt();
                        currChargeDecisionTariffDebt += accChargeDecisionTariffDebt;
                        decimal accPenaltyDebt = currentSummary.PenaltyDebt + currentSummary.GetPenaltyDebt();
                        currPenaltyDebt += currPenaltyDebt;
                        basePayments = personalAccountPeriodSummaryDomain.GetAll()
                              .Where(x => x.PersonalAccount == accountDetail.PersonalAccount)
                            .Where(x => x.Period.StartDate > summaryPeriod.EndDate)
                            .SafeSum(x => x.TariffPayment);
                        decisionPayments = personalAccountPeriodSummaryDomain.GetAll()
                             .Where(x => x.PersonalAccount == accountDetail.PersonalAccount)
                           .Where(x => x.Period.StartDate > summaryPeriod.EndDate)
                           .SafeSum(x => x.TariffDecisionPayment);
                        penaltyPayments = personalAccountPeriodSummaryDomain.GetAll()
                            .Where(x => x.PersonalAccount == accountDetail.PersonalAccount)
                          .Where(x => x.Period.StartDate > summaryPeriod.EndDate)
                          .SafeSum(x => x.PenaltyPayment);
                        //учитываем отмены начислений в открытом периоде
                        cancelChargesBt = transferDomain.GetAll()
                         .Where(x => x.Owner == accountDetail.PersonalAccount)
                         .Where(x => x.ChargePeriod.StartDate > summaryPeriod.EndDate)
                         .Where(x => x.Reason == "Отмена начислений по базовому тарифу")
                         .SafeSum(x => x.Amount);
                        //учитываем установки сальдо в открытом периоде
                        saldoChangeBt = transferDomain.GetAll()
                      .Where(x => x.Owner == accountDetail.PersonalAccount)
                      .Where(x => x.ChargePeriod.StartDate > summaryPeriod.EndDate)
                      .Where(x => x.Reason == "Установка/изменение сальдо по базовому тарифу")
                      .SafeSum(x => x.Amount);
                        currChargeBaseTariffDebt -= basePayments;
                        accChargeBaseTariffDebt -= basePayments;
                        accChargeDecisionTariffDebt -= decisionPayments;
                        currChargeDecisionTariffDebt -= decisionPayments;
                        accPenaltyDebt -= penaltyPayments;
                        currPenaltyDebt -= penaltyPayments;                      
                     
                        accChargeBaseTariffDebt -= cancelChargesBt;
                        currChargeBaseTariffDebt -= cancelChargesBt;
                        accChargeBaseTariffDebt += saldoChangeBt;
                        currChargeBaseTariffDebt += saldoChangeBt;
                        currChargeDebt += currChargeBaseTariffDebt + currChargeDecisionTariffDebt + currPenaltyDebt;

                        try// обновляем детали лс
                        {
                            accountDetail.CurrChargeDebt = (accChargeBaseTariffDebt + accChargeDecisionTariffDebt + accPenaltyDebt)<0?0: (accChargeBaseTariffDebt + accChargeDecisionTariffDebt + accPenaltyDebt);
                            accountDetail.CurrChargeBaseTariffDebt = accChargeBaseTariffDebt<0?0: accChargeBaseTariffDebt;
                            accountDetail.CurrChargeDecisionTariffDebt = accChargeDecisionTariffDebt < 0 ? 0 : accChargeDecisionTariffDebt;
                            accountDetail.CurrPenaltyDebt = accPenaltyDebt < 0 ? 0 : accPenaltyDebt;
                            accountDetailDomain.Update(accountDetail);
                        }
                        catch
                        {

                        }
                    }
                    if (clw.CurrChargeDebt != currChargeDebt)
                    {
                        needToUpdate = true;
                    }
                    clw.CurrChargeBaseTariffDebt = currChargeBaseTariffDebt<0? 0: currChargeBaseTariffDebt;
                    clw.CurrChargeDecisionTariffDebt = currChargeDecisionTariffDebt < 0 ? 0 : currChargeDecisionTariffDebt;
                    clw.CurrChargeDebt = currChargeDebt < 0 ? 0 : currChargeDebt;
                    clw.CurrPenaltyDebt = currPenaltyDebt < 0 ? 0 : currPenaltyDebt;
                    clw.OrigChargeBaseTariffDebt = currChargeBaseTariffDebt < 0 ? 0 : currChargeBaseTariffDebt;
                    clw.OrigChargeDecisionTariffDebt = currChargeDecisionTariffDebt < 0 ? 0 : currChargeDecisionTariffDebt;
                    clw.OrigChargeDebt = currChargeDebt < 0 ? 0 : currChargeDebt;
                    clw.OrigPenaltyDebt = currPenaltyDebt < 0 ? 0 : currPenaltyDebt;

                    if (!clw.IsDebtPaid && (clw.OrigChargeBaseTariffDebt + clw.OrigChargeDecisionTariffDebt + clw.OrigPenaltyDebt) <= 1)
                    {
                        clw.IsDebtPaid = true;
                        clw.DebtorState = DebtorState.PaidDebt;
                    }

                    //foreach (var accountDetail in detailsByClw)
                    //{
                    //    //var debtorInfo = debtorsInfo.Get(accountDetail.PersonalAccount.Id);
                    //    //if (debtorInfo == null)
                    //    //{
                    //    //    continue;
                    //    //}

                    //    if (accountDetail.StartingDate != debtorInfo.StartDate
                    //        || accountDetail.CurrChargeBaseTariffDebt != debtorInfo.DebtBaseTariffSum
                    //        || accountDetail.CurrChargeDecisionTariffDebt != debtorInfo.DebtDecisionTariffSum
                    //        || accountDetail.CurrChargeDebt != debtorInfo.DebtSum
                    //        || accountDetail.CurrPenaltyDebt != debtorInfo.PenaltyDebt)
                    //    {
                    //        needToUpdate = true;
                    //        accountDetail.StartingDate = debtorInfo.StartDate;
                    //        accountDetail.CurrChargeBaseTariffDebt = debtorInfo.DebtBaseTariffSum;
                    //        accountDetail.CurrChargeDecisionTariffDebt = debtorInfo.DebtDecisionTariffSum;
                    //        accountDetail.CurrChargeDebt = debtorInfo.DebtSum;
                    //        accountDetail.CurrPenaltyDebt = debtorInfo.PenaltyDebt;
                    //        accountDetail.CountDaysDelay = debtorInfo.ExpiredDaysCount;
                    //        accountDetail.CountMonthDelay = debtorInfo.ExpiredMonthCount;
                    //        accountDetailsToUpd.Add(accountDetail);
                    //    }
                    //}

                    //var currChargeBaseTariffDebt = detailsByClw.SafeSum(y => y.CurrChargeBaseTariffDebt);
                    //var currChargeDecisionTariffDebt = detailsByClw.SafeSum(y => y.CurrChargeDecisionTariffDebt);
                    //var currChargeDebt = detailsByClw.SafeSum(y => y.CurrChargeDebt);
                    //var currPenaltyDebt = detailsByClw.SafeSum(y => y.CurrPenaltyDebt);

                    //if (clw.CurrChargeBaseTariffDebt != currChargeBaseTariffDebt
                    //    || clw.CurrChargeDecisionTariffDebt != currChargeDecisionTariffDebt
                    //    || clw.CurrChargeDebt != currChargeDebt
                    //    || clw.CurrPenaltyDebt != currPenaltyDebt)
                    //{
                    //    needToUpdate = true;
                    //    clw.CurrChargeBaseTariffDebt = currChargeBaseTariffDebt;
                    //    clw.CurrChargeDecisionTariffDebt = currChargeDecisionTariffDebt;
                    //    clw.CurrChargeDebt = currChargeDebt;
                    //    clw.CurrPenaltyDebt = currPenaltyDebt;
                    //}

                    var isBaseAdnDecisionTariff = debtorConfig.DebtorSumCheckerType == DebtorSumCheckerType.BaseAdnDecisionTariff;
                    var lawSuitState = this.UpdatePretensionsAndLawsuit(clw.Id, entitiesToSave, isBaseAdnDecisionTariff);

                    var lawsuits = lawsuitDomain.GetAll()
                        .Where(x => x.ClaimWork.Id == clw.Id).ToList();
                    var docdatemax = lawsuits.Max(x => x.DocumentDate);
                    if (!docdatemax.HasValue)
                    { docdatemax = clw.PIRCreateDate; }

                    bool courtOrderOnly = false; // признак вынесенного судебного приказа
                    bool courtOrderCancelledless2month = false; // признак отмененного судебного приказа менее 2х месяцев назад
                    bool courtOrderCancelledmore2month = false;// признак отмененного судебного приказа более 2х месяцев назад

                    if (clw.DebtorState != DebtorState.PaidDebt && clw.DebtorState != DebtorState.StartedEnforcement)
                    {
                        decimal persAccAllTransfers = 0;
                        try
                        {
                            persAccAllTransfers = this.Container.Resolve<IDomainService<PersonalAccountPaymentTransfer>>().GetAll()
                                  .Where(x => x.Owner.Id == detailsByClw.FirstOrDefault().PersonalAccount.Id)
                                  .Where(x => x.PaymentDate >= docdatemax)
                                  .Where(x => x.Operation.IsCancelled != true)
                                  .SafeSum(x => x.Amount); //Обратный порядок для итерации
                        }
                        catch (Exception e)
                        {
                        }

                        if (lawsuits.Count() == 1 && lawsuits[0].DocumentType == ClaimWorkDocumentType.CourtOrderClaim)
                        {
                             var lawsw = lawsuits[0];
                            if (lawsw.ResultConsideration == LawsuitResultConsideration.CourtOrderIssued && !lawsw.IsDeterminationCancel)
                            {
                                if (persAccAllTransfers > 0)
                                {
                                    if (persAccAllTransfers >= lawsw.DebtBaseTariffSum)
                                    {
                                        var oldState = clw.DebtorState;
                                        clw.DebtorState = DebtorState.PaidDebt;
                                        clw.DebtorStateHistory = oldState;
                                        courtOrderOnly = true;
                                        needToUpdate = true;
                                    }
                                    else
                                    {

                                        var oldState = clw.DebtorState;
                                        clw.DebtorState = DebtorState.PaymentsIncome;
                                        clw.DebtorStateHistory = oldState;
                                        courtOrderOnly = true;
                                        needToUpdate = true;
                                    }
                                }                             
                                else if (lawsw.ConsiderationDate.HasValue && lawsw.ConsiderationDate.Value.AddMonths(2) <= DateTime.Now)
                                {
                                    var oldState = clw.DebtorState;
                                    clw.DebtorState = DebtorState.ROSPStartRequired;
                                    clw.DebtorStateHistory = oldState;
                                    courtOrderOnly = true;
                                    needToUpdate = true;
                                }
                                else
                                {
                                    var oldState = clw.DebtorState;
                                    clw.DebtorState = DebtorState.CourtOrderApproved;
                                    clw.DebtorStateHistory = oldState;
                                    courtOrderOnly = true;
                                    needToUpdate = true;
                                }
                            }
                            else if (lawsw.ResultConsideration == LawsuitResultConsideration.Denied)
                            {
                                var oldState = clw.DebtorState;
                                clw.DebtorState = DebtorState.LawsuitNeeded;
                                clw.DebtorStateHistory = oldState;
                                courtOrderOnly = true;
                                needToUpdate = true;
                            }
                            else if (lawsw.IsDeterminationCancel && lawsw.DateDeterminationCancel.HasValue)
                            {
                                if (lawsw.DateDeterminationCancel.Value.AddMonths(2) <= DateTime.Now)
                                {
                                    var oldState = clw.DebtorState;
                                    clw.DebtorState = DebtorState.LawsuitNeeded;
                                    clw.DebtorStateHistory = oldState;
                                    courtOrderCancelledmore2month = true;
                                    needToUpdate = true;
                                }
                                else
                                {
                                    var oldState = clw.DebtorState;
                                    clw.DebtorState = DebtorState.CourtOrderCancelled;
                                    clw.DebtorStateHistory = oldState;
                                    courtOrderCancelledless2month = true;
                                    needToUpdate = true;
                                }
                            }
                            else if (lawsw.IsDeterminationCancel && !lawsw.DateDeterminationCancel.HasValue)
                            {
                                var oldState = clw.DebtorState;
                                clw.DebtorState = DebtorState.CourtOrderCancelled;
                                clw.DebtorStateHistory = oldState;
                                courtOrderCancelledless2month = true;
                                needToUpdate = true;
                            }
                        }
                        else if (lawsuits.Count() == 1 && lawsuits[0].DocumentType == ClaimWorkDocumentType.Lawsuit)
                        {
                            var oldState = clw.DebtorState;
                            clw.DebtorState = DebtorState.PetitionFormed;
                            clw.DebtorStateHistory = oldState;
                            needToUpdate = true;
                        }
                        else if (lawsuits.Count() > 1)
                        {
                            var lawsw = lawsuits.Where(x => x.DocumentType == ClaimWorkDocumentType.Lawsuit).FirstOrDefault();
                            if (lawsw != null && lawsw.ResultConsideration == LawsuitResultConsideration.NotSet)
                            {
                                var oldState = clw.DebtorState;
                                clw.DebtorState = DebtorState.PetitionFormed;
                                clw.DebtorStateHistory = oldState;
                                needToUpdate = true;
                            }
                            else if (lawsw != null && lawsw.ResultConsideration == LawsuitResultConsideration.Denied)
                            {
                                var oldState = clw.DebtorState;
                                clw.DebtorState = DebtorState.LawSueenDenied;
                                clw.DebtorStateHistory = oldState;
                                courtOrderOnly = true;
                                needToUpdate = true;
                            }
                            else if ((lawsw != null && lawsw.ResultConsideration == LawsuitResultConsideration.Satisfied && lawsw.ConsiderationDate.HasValue)|| persAccAllTransfers > 0)
                            {
                                if (persAccAllTransfers > 0)
                                {
                                    if (persAccAllTransfers >= lawsw.DebtBaseTariffSum)
                                    {
                                        var oldState = clw.DebtorState;
                                        clw.DebtorState = DebtorState.PaidDebt;
                                        clw.DebtorStateHistory = oldState;
                                        courtOrderOnly = true;
                                        needToUpdate = true;
                                    }
                                    else
                                    {

                                        var oldState = clw.DebtorState;
                                        clw.DebtorState = DebtorState.PaymentsIncome;
                                        clw.DebtorStateHistory = oldState;
                                        courtOrderOnly = true;
                                        needToUpdate = true;
                                    }
                                }
                                else if (lawsw.ConsiderationDate.Value.AddMonths(2) <= DateTime.Now)
                                {
                                    var oldState = clw.DebtorState;
                                    clw.DebtorState = DebtorState.ROSPStartRequired;
                                    clw.DebtorStateHistory = oldState;
                                    needToUpdate = true;
                                }
                            }
                        }
                    }
                    else if (clw.DebtorState == DebtorState.StartedEnforcement)
                    {

                        if (lawsuits.Count() == 1)
                        {
                            var lawsw = lawsuits[0];
                            decimal persAccAllTransfers = 0;
                            try
                            {
                                persAccAllTransfers = this.Container.Resolve<IDomainService<PersonalAccountPaymentTransfer>>().GetAll()
                                      .Where(x => x.Owner.Id == detailsByClw.FirstOrDefault().PersonalAccount.Id)
                                      .Where(x => x.PaymentDate >= docdatemax)
                                      .Where(x => x.Operation.IsCancelled != true)
                                      .SafeSum(x => x.Amount); //Обратный порядок для итерации
                            }
                            catch (Exception e)
                            {
                            }

                            if (persAccAllTransfers > 0)
                            {
                                if (persAccAllTransfers >= lawsw.DebtBaseTariffSum)
                                {
                                    var oldState = clw.DebtorState;
                                    clw.DebtorState = DebtorState.PaidDebt;
                                    clw.DebtorStateHistory = oldState;
                                    courtOrderOnly = true;
                                    needToUpdate = true;
                                }
                                else
                                {

                                    var oldState = clw.DebtorState;
                                    clw.DebtorState = DebtorState.PaymentsIncome;
                                    clw.DebtorStateHistory = oldState;
                                    courtOrderOnly = true;
                                    needToUpdate = true;
                                }
                            }
                            else
                            {
                                if (clw.SubContractDate.HasValue)
                                {
                                    if (clw.SubContractDate.Value.AddYears(1) <= DateTime.Now && clw.SubContractNum != "3")
                                    {
                                        //статус требует исковой работы переаодим только три раза через год по законодательству
                                        if (string.IsNullOrEmpty(clw.SubContractNum))
                                        {
                                            clw.SubContractNum = "1";
                                        }
                                        else if (clw.SubContractNum == "1")
                                        {
                                            clw.SubContractNum = "2";
                                        }
                                        else if (clw.SubContractNum == "2")
                                        {
                                            clw.SubContractNum = "3";
                                        }

                                        var oldState = clw.DebtorState;
                                        clw.DebtorState = DebtorState.ROSPStartRequired;
                                        clw.DebtorStateHistory = oldState;
                                        courtOrderOnly = true;
                                        needToUpdate = true;
                                    }
                                }
                            }
                        }
                        else if(lawsuits.Count() >1)
                        {
                            var lawsw = lawsuits.Where(x => x.DocumentType == ClaimWorkDocumentType.Lawsuit).FirstOrDefault();
                            if (lawsw != null)
                            {
                                decimal persAccAllTransfers = 0;
                                try
                                {
                                    persAccAllTransfers = this.Container.Resolve<IDomainService<PersonalAccountPaymentTransfer>>().GetAll()
                                          .Where(x => x.Owner.Id == detailsByClw.FirstOrDefault().PersonalAccount.Id)
                                          .Where(x => x.PaymentDate >= docdatemax)
                                          .Where(x => x.Operation.IsCancelled != true)
                                          .SafeSum(x => x.Amount); //Обратный порядок для итерации
                                }
                                catch (Exception e)
                                {
                                }

                                if (persAccAllTransfers > 0)
                                {
                                    if (persAccAllTransfers >= lawsw.DebtBaseTariffSum)
                                    {
                                        var oldState = clw.DebtorState;
                                        clw.DebtorState = DebtorState.PaidDebt;
                                        clw.DebtorStateHistory = oldState;
                                        courtOrderOnly = true;
                                        needToUpdate = true;
                                    }
                                    else
                                    {

                                        var oldState = clw.DebtorState;
                                        clw.DebtorState = DebtorState.PaymentsIncome;
                                        clw.DebtorStateHistory = oldState;
                                        courtOrderOnly = true;
                                        needToUpdate = true;
                                    }
                                }
                                else
                                {
                                    if (clw.SubContractDate.HasValue)
                                    {
                                        if (clw.SubContractDate.Value.AddYears(1) <= DateTime.Now && clw.SubContractNum != "3")
                                        {
                                            //статус требует исковой работы переаодим только три раза через год по законодательству
                                            if (string.IsNullOrEmpty(clw.SubContractNum))
                                            {
                                                clw.SubContractNum ="1";
                                            }
                                            else if(clw.SubContractNum == "1")
                                            {
                                                clw.SubContractNum = "2";
                                            }
                                            else if (clw.SubContractNum == "2")
                                            {
                                                clw.SubContractNum = "3";
                                            }

                                            var oldState = clw.DebtorState;
                                            clw.DebtorState = DebtorState.ROSPStartRequired;
                                            clw.DebtorStateHistory = oldState;
                                            courtOrderOnly = true;
                                            needToUpdate = true;                                            
                                        }
                                    }
                                }
                            }
                        }
                      

                    }

                        var nextState = this.dataCache.Value.NextStates[clw.Id];
                    var canUpdateState = false; //  this.TryCloseDebt(clw, nextState > lawSuitState ? nextState : lawSuitState); пока комментим барсовскую обновлялку статуса

                     if (needToUpdate || canUpdateState)
                    {
                        debtorClaimWorkToUpd.Add(clw);
                    }
                }

                this.RestructDebtScheduleService.SumsUpdate(debtorClaimWorkQuery);

                TransactionHelper.InsertInManyTransactions(this.Container, debtorClaimWorkToUpd);
                TransactionHelper.InsertInManyTransactions(this.Container, accountDetailsToUpd);
                TransactionHelper.InsertInManyTransactions(this.Container, entitiesToSave);
            }

            return new BaseDataResult(true, "Статусы успешно обновлены");
        }

        private DebtorState UpdatePretensionsAndLawsuit(long clwId,
            ISet<BaseEntity> toSave,
            bool isBaseAdnDecisionTariff)
        {
            var transfers = this.dataCache.Value.Transfers.Get(clwId);
            var pretension = this.dataCache.Value.Pretensions.Get(clwId);
            var lawsuit = this.Container.Resolve<IDomainService<Lawsuit>>().GetAll()
                .Where(x=> x.ClaimWork.Id == clwId).OrderByDescending(x=> x.Id).FirstOrDefault();

         //   var lawsuit = this.dataCache.Value.Lawsuits.Get(clwId);

            if (pretension != null)
            {
                try
                {
                    var persAccDetails = this.Container.Resolve<IDomainService<ClaimWorkAccountDetail>>().GetAll()
                        .Where(x => x.ClaimWork.Id == pretension.ClaimWork.Id)
                        .Select(x => x.PersonalAccount.Id).Distinct().ToList();

                    var persAccAllTransfers = this.Container.Resolve<IDomainService<PersonalAccountPaymentTransfer>>().GetAll()
               .Where(x => persAccDetails.Contains(x.Owner.Id))
               .Where(x => x.Operation.IsCancelled != true)
               .OrderByDescending(x => x.PaymentDate); //Обратный порядок для итерации


                    var persAccCharges = this.Container.Resolve<IDomainService<PersonalAccountPeriodSummary>>().GetAll()
                         .Where(x => persAccDetails.Contains(x.PersonalAccount.Id))
                         .Where(x=> x.Period.EndDate.HasValue && x.Period.EndDate < (pretension.DocumentDate.HasValue? pretension.DocumentDate.Value.AddMonths(-1): DateTime.Now.AddMonths(-1)))
                         .Sum(x=> x.ChargeTariff + x.RecalcByBaseTariff + x.BaseTariffChange);

                    var persAccAllChargeTransfers = persAccAllTransfers.Where(
                   x => x.Reason == "Оплата по базовому тарифу" ||
                   x.Reason == "Оплата пени" ||
                   x.Reason == "Оплата по тарифу решения").ToList();

                    var persAccAllReturnTransfers = persAccAllTransfers.Where(
                  x => x.Reason == "Отмена оплаты по базовому тарифу" ||
                  x.Reason == "Отмена оплаты по тарифу решения" ||
                  x.Reason == "Отмена оплаты пени" ||
                  x.Reason == "Возврат взносов на КР").ToList();

                    var persAccChargeTransfers = new List<PersonalAccountPaymentTransfer>(persAccAllChargeTransfers);

                    decimal totalPayment = persAccChargeTransfers.Sum(x => x.Amount);
                    decimal totalReturnPayments = persAccAllReturnTransfers.Sum(x => x.Amount);

                    decimal totalPayments = totalPayment - totalReturnPayments;

                    pretension.DebtBaseTariffSum = persAccCharges - totalPayments;
                   



                }
                catch (Exception e)
                {

                }
                var startDate = pretension.DocumentDate;
                var endDate = lawsuit?.DocumentDate ?? DateTime.Today;

                var sum = transfers?
                    .Where(y => y.PaymentDate >= startDate)
                    .Where(y => y.PaymentDate <= endDate)
                    .SafeSum(y => y.Amount) ?? 0;

                var debtSum = isBaseAdnDecisionTariff
                    ? pretension.DebtBaseTariffSum + pretension.DebtDecisionTariffSum
                    : pretension.Sum;

                pretension.RequirementSatisfaction = sum == 0
                    ? RequirementSatisfaction.Not
                    : debtSum + pretension.Penalty > sum
                        ? RequirementSatisfaction.Partial
                        : RequirementSatisfaction.Full;

                toSave.Add(pretension);
            }

            if (lawsuit != null)
            {
                var sum = transfers?
                    .Where(x => x.PaymentDate >= lawsuit.DocumentDate)
                    .Where(x => x.PaymentDate <= (lawsuit.CbDateInitiated ?? lawsuit.DateDirectionForSsp ?? DateTime.Now))
                    .Where(x => !x.IsPenalty)
                    .SafeSum(y => y.Amount) ?? 0;

                var penalty = transfers?
                    .Where(x => x.PaymentDate >= lawsuit.DocumentDate)
                    .Where(x => x.PaymentDate <= (lawsuit.CbDateInitiated ?? lawsuit.DateDirectionForSsp ?? DateTime.Now))
                    .Where(x => x.IsPenalty)
                    .SafeSum(y => y.Amount) ?? 0;

                var debtSum = isBaseAdnDecisionTariff
                    ? lawsuit.DebtBaseTariffSum + lawsuit.DebtDecisionTariffSum
                    : lawsuit.DebtSum;

                lawsuit.CbDebtSum = sum;
                lawsuit.CbPenaltyDebt = penalty;
                lawsuit.CbSize = sum == 0 && penalty == 0
                    ? LawsuitCollectionDebtType.NotRepaiment
                    : debtSum + lawsuit.PenaltyDebt > sum + penalty
                        ? LawsuitCollectionDebtType.PartiallyRepaiment
                        : LawsuitCollectionDebtType.FullRepaid;

                this.LawsuitAutoSelector.TrySetAll(lawsuit);

                toSave.Add(lawsuit);

                if (lawsuit.CbFactInitiated == LawsuitFactInitiationType.Initiated)
                {
                    return DebtorState.StartedEnforcement;
                }
            }

            return DebtorState.StartDebt;
        }

        private bool TryCloseDebt(DebtorClaimWork debtorClaimWork, DebtorState newState)
        {
            if (debtorClaimWork == null || (debtorClaimWork.DebtorState == newState
                && debtorClaimWork.IsDebtPaid && debtorClaimWork.State.Id == this.finalState.Id))
            {
                return false;
            }

            if (debtorClaimWork.CurrChargeDebt <= 0 && debtorClaimWork.CurrPenaltyDebt <= 0)
            {
                debtorClaimWork.SetDebtorState(DebtorState.PaidDebt, this.finalState);
                debtorClaimWork.DebtPaidDate = DateTime.Today;
            }
            else
            {
                debtorClaimWork.SetDebtorState(newState, this.defaultState);
            }

            return true;
        }

        public IDataResult SetDefaultState(IQueryable<DebtorClaimWork> debtorClaimWorkQuery)
        {
            if (this.defaultState == null)
            {
                return BaseDataResult.Error("Не найден начальный статус");
            }

            foreach (var debtorClaimWork in debtorClaimWorkQuery)
            {
                debtorClaimWork.SetDebtorState(DebtorState.StartDebt, this.defaultState);
            }

            TransactionHelper.InsertInManyTransactions(this.Container, debtorClaimWorkQuery, 1000, true, true);

            return new BaseDataResult();
        }

        private class TransferDto
        {
            public long Id { get; set; }

            public bool IsPenalty { get; set; }

            public decimal Amount { get; set; }

            public DateTime OperationDate { get; set; }

            public DateTime PaymentDate { get; set; }

            public long AccountId { get; set; }
        }

        private class Cache
        {
            private readonly IWindsorContainer container;

            public IReadOnlyDictionary<long, PretensionClw> Pretensions { get; }
            public IReadOnlyDictionary<long, Lawsuit> Lawsuits { get; }
            public IReadOnlyDictionary<long, List<TransferDto>> Transfers { get; }
            public IReadOnlyDictionary<long, DebtorState> NextStates { get; }

            public Cache(IWindsorContainer container, IQueryable<ClaimWorkAccountDetail> accountDetailQuery, List<DebtorClaimWork> claimWorks)
            {
                this.container = container;
                var pretensionDomain = container.ResolveDomain<PretensionClw>();
                var lawsuitDomain = container.ResolveDomain<Lawsuit>();
                Dictionary<long, Lawsuit> tempLawsuits = new Dictionary<long, Lawsuit>();
                using (container.Using(pretensionDomain, lawsuitDomain))
                {
                    var clwIds = claimWorks.Select(y => y.Id).ToArray();

                    this.Pretensions = pretensionDomain.GetAll()
                        .WhereContainsBulked(x => x.ClaimWork.Id, clwIds)
                        .Where(x => x.RequirementSatisfaction != RequirementSatisfaction.Refusal)
                        .ToDictionary(x => x.ClaimWork.Id);
                    var lawlist = lawsuitDomain.GetAll()
                        .WhereContainsBulked(x => x.ClaimWork.Id, clwIds)
                        .Where(x => x.CbSize != LawsuitCollectionDebtType.FullRepaid)
                        .Where(x => x.DocumentDate.HasValue);

                    this.Lawsuits = lawsuitDomain.GetAll()
                        .WhereContainsBulked(x => x.ClaimWork.Id, clwIds)
                        .Where(x => x.CbSize != LawsuitCollectionDebtType.FullRepaid)
                        .Where(x => x.DocumentDate.HasValue)
                        .ToDictionary(x => x.Id);
                    
                    // в случае, если по одному ПИР несколько документов (исковое, ЗВСП), берём последний
                    var lawsuitsList = lawsuitDomain.GetAll()
                    .WhereContainsBulked(x => x.ClaimWork.Id, clwIds)
                    .Where(x => x.CbSize != LawsuitCollectionDebtType.FullRepaid)
                    .Where(x => x.DocumentDate.HasValue)
                    .OrderByDescending(x => x.ObjectCreateDate)
                    .ToList();
                    foreach (var lsw in lawsuitsList)
                    {
                        if (!tempLawsuits.ContainsKey(lsw.ClaimWork.Id))
                        {
                            tempLawsuits.Add(lsw.ClaimWork.Id, lsw);
                        }
                    }
                    this.Lawsuits = tempLawsuits;
                    this.Transfers = this.GetTransfers(accountDetailQuery);
                    this.NextStates = this.GetMassNextState(claimWorks, clwIds);
                }
            }

            private Dictionary<long, List<TransferDto>> GetTransfers(IQueryable<ClaimWorkAccountDetail> accountDetails)
            {
                var guids = accountDetails
                    .Select(x => new
                    {
                        BaseTariffWallet = x.PersonalAccount.BaseTariffWallet.WalletGuid,
                        DecisionTariffWallet = x.PersonalAccount.DecisionTariffWallet.WalletGuid,
                        PenaltyWallet = x.PersonalAccount.PenaltyWallet.WalletGuid
                    })
                    .AsEnumerable()
                    .SelectMany(x => new[]
                    {
                    x.BaseTariffWallet,
                    x.DecisionTariffWallet,
                    x.PenaltyWallet
                    })
                    .ToHashSet();

                var penaltyGuids = accountDetails.Select(x => x.PersonalAccount.PenaltyWallet.WalletGuid)
                    .AsEnumerable()
                    .ToHashSet();

                Dictionary<long, long> claimworkAccDict = new Dictionary<long, long>();
                // Если несколько ПИР по одному л/с - берём последний
                var orederedAccountDetails = accountDetails.OrderByDescending(x => x.ClaimWork.ObjectCreateDate);
                foreach (var accDet in orederedAccountDetails)
                {
                    if (!claimworkAccDict.ContainsKey(accDet.PersonalAccount.Id))
                    {
                        claimworkAccDict.Add(accDet.PersonalAccount.Id, accDet.ClaimWork.Id);
                    }
                }

                var accountIds = claimworkAccDict.Keys;

                var transferDomain = this.container.ResolveDomain<PersonalAccountPaymentTransfer>();
                using (this.container.Using(transferDomain))
                {
                    return transferDomain.GetAll()
                        .Where(x => accountIds.Contains(x.Owner.Id))
                        .Where(x => x.IsAffect && !x.Operation.IsCancelled)
                        .Where(x => x.Operation.CanceledOperation == null)
                        .Where(x => guids.Contains(x.TargetGuid) || guids.Contains(x.SourceGuid))
                        .Select(x => new TransferDto
                        {
                            Id = x.Id,
                            OperationDate = x.OperationDate,
                            PaymentDate = x.PaymentDate,
                            Amount = guids.Contains(x.SourceGuid)
                                ? -1 * x.Amount
                                : x.Amount,
                            IsPenalty = penaltyGuids.Contains(x.SourceGuid) || penaltyGuids.Contains(x.TargetGuid),
                            AccountId = x.Owner.Id
                        })
                        .AsEnumerable()
                        .Select(x => new
                        {
                            Transfer = x,
                            ClaimWorkId = claimworkAccDict[x.AccountId]
                        })
                        .GroupBy(x => x.ClaimWorkId)
                        .ToDictionary(x => x.Key, x => x.Select(y => y.Transfer).ToList());
                }
            }

            private Dictionary<long, DebtorState> GetMassNextState(List<DebtorClaimWork> claimWorks, long[] clwIds)
            {
                var result = new Dictionary<long, DebtorState>();
                var rules = new IClwTransitionRule[]
                {
                    new IncludeHiLevelDocRule(this.container),
                    new IncludeLawsuitRule(this.container),
                    new IncludeCourtOrderClaimRule(this.container),
                    new IncludePretensionRule(this.container),
                    new IncludeNotificationRule(this.container)
                };

                var claimWorkStateProvider = this.container.Resolve<IClwStateProvider>();
                var debtorStateProvider = this.container.Resolve<IDebtorStateProvider>();
                try
                {
                    claimWorkStateProvider.InitCache(clwIds);
                    debtorStateProvider.InitCache(clwIds);

                    foreach (var claimwork in claimWorks)
                    {
                        var avaiableDocTypes = claimWorkStateProvider.GetAvailableTransitions(claimwork, rules, true);

                        var state = debtorStateProvider.GetState(claimwork, avaiableDocTypes);

                        result.Add(claimwork.Id, state);
                    }

                    claimWorkStateProvider.Clear();
                }
                finally
                {
                    claimWorkStateProvider?.Clear();

                    this.container.Release(claimWorkStateProvider);
                    this.container.Release(debtorStateProvider);
                }

                return result;
            }
        }

        /// <inheritdoc />
        public void Initialize()
        {
            var stateRepo = this.Container.Resolve<IStateRepository>();
            using (this.Container.Using(stateRepo))
            {
                var defaultStates = stateRepo.GetAllStates<DebtorClaimWork>(x => x.StartState).Take(2).ToList();
                this.defaultState = defaultStates.Count == 1 ? defaultStates[0] : null;

                var finalStates = stateRepo.GetAllStates<DebtorClaimWork>(x => x.FinalState).Take(2).ToList();
                this.finalState = finalStates.Count == 1 ? finalStates[0] : null;

                var startedEnforcementName = DebtorState.StartedEnforcement.GetDisplayName();
                this.startedEnforcementState = stateRepo
                    .GetAllStates<DebtorClaimWork>(x => x.Name == startedEnforcementName)
                    .FirstOrDefault();
            }

            this.LawsuitAutoSelector = this.Container.Resolve<ILawsuitAutoSelector>(DebtorLawsuitAutoSelector.Id);
        }
    }
}