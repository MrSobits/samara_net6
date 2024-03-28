namespace Bars.Gkh.RegOperator.DomainService.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using B4;
    using B4.DataAccess;
    using B4.Utils;

    using Bars.Gkh.Repositories.ChargePeriod;

    using Castle.Windsor;
    using Distribution;
    using Distribution.Args;
    using Domain;
    using Domain.Repository;
    using Domain.Repository.Transfers;
    using DomainModelServices.PersonalAccount;
    using Entities;
    using Entities.ValueObjects;
    using Enums;
    using Gkh.Domain.CollectionExtensions;
    using Gkh.Utils;
    using NHibernate;

    public class SubsidyIncomeService : ISubsidyIncomeService
    {
        public IWindsorContainer Container { get; set; }

        public IDataResult Apply(BaseParams baseParams)
        {
            var subsidyIncomeDomain = Container.ResolveDomain<SubsidyIncome>();
            var subsidyIncomeDetailDomain = Container.ResolveDomain<SubsidyIncomeDetail>();
            var realObjPaymentAccountDomain = Container.ResolveDomain<RealityObjectPaymentAccount>();
            var sessionProvider = Container.Resolve<ISessionProvider>();
            var paymentSession = Container.Resolve<IRealtyObjectPaymentSession>();
            var recalcManager = Container.Resolve<IPersonalAccountRecalcEventManager>();

            try
            {
                var recIds = baseParams.Params.GetAs("recIds", new long[0]);
                var detailIds = baseParams.Params.GetAs("detailIds", new long[0]) ?? new long[0];

                var subsidyIncomeQuery = subsidyIncomeDetailDomain.GetAll()
                    .WhereIf(detailIds.Length == 0, x => recIds.Contains(x.SubsidyIncome.Id))
                    .WhereIf(detailIds.Length > 0, x => detailIds.Contains(x.Id))
                    .Where(x => x.RealityObject != null)
                    .Where(x => !x.IsConfirmed); 

                var distribs = Container.ResolveAll<IDistribution>()
                        .Where(x => x.Code.Contains("Subsidy"))
                        .GroupBy(x => x.Code)
                        .ToDictionary(x => x.Key, y => y.First());

                var payAccByRoId = realObjPaymentAccountDomain.GetAll()
                    .Where(x => subsidyIncomeQuery.Any(y => y.RealityObject.Id == x.RealityObject.Id))
                    .Select(x => new
                    {
                        x.RealityObject.Id,
                        Account = x
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.Id)
                    .ToDictionary(x => x.Key, y => y.Select(x => x.Account).First());

                var subIncomeIds = subsidyIncomeQuery.Select(x => x.SubsidyIncome.Id).Distinct().ToArray();

                var incomesWithNoConfirmDetails = subsidyIncomeDetailDomain.GetAll()
                    .Where(x => subIncomeIds.Contains(x.SubsidyIncome.Id))
                    .Where(x => !subsidyIncomeQuery.Any(y => y.Id == x.Id))
                    .Where(x => !x.IsConfirmed)
                    .Select(x => x.SubsidyIncome.Id)
                    .ToHashSet();

                var subIncomeDetForUpd = new List<long>();

                var groupingDetails = subsidyIncomeQuery
                    .Select(x => new
                    {
                        x.Id,
                        x.SubsidyIncome,
                        RealObjId = x.RealityObject.Id,
                        x.Sum,
                        x.DateReceipt,
                        x.TypeSubsidyDistr
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.SubsidyIncome)
                    .ToDictionary(x => x.Key, y => y
                        .GroupBy(x => x.TypeSubsidyDistr)
                        .ToDictionary(x => x.Key, z => z.Select(x =>
                        {
                            var payAccount = payAccByRoId.Get(x.RealObjId);

                            subIncomeDetForUpd.Add(x.Id);

                            return new DistributionByRealtyAccountArgs.ByRealtyRecord(payAccount, x.Sum, x.DateReceipt);
                        }).ToList()));

                using (var tr = Container.Resolve<IDataTransaction>())
                {
                    var nhSession = sessionProvider.GetCurrentSession();
                    var oldFlush = nhSession.FlushMode;
                    nhSession.FlushMode = FlushMode.Never;

                    try
                    {                
                        foreach (var grouping in groupingDetails)
                        {
                            var subsidyIncome = grouping.Key;

                            foreach (var details in grouping.Value)
                            {
                                var distrib = distribs.Get(details.Key);

                                distrib.Apply(new DistributionByRealtyAccountArgs(subsidyIncome, details.Value));

                                subsidyIncome.DistributionCode = subsidyIncome.DistributionCode.IsEmpty() ? distrib.Code : "{0},{1}".FormatUsing(subsidyIncome.DistributionCode, distrib.Code);
                            }

                            subsidyIncome.DistributeState = incomesWithNoConfirmDetails.Contains(subsidyIncome.Id) ? DistributionState.PartiallyDistributed : DistributionState.Distributed;
                            subsidyIncome.DistributionDate = DateTime.UtcNow;
                            subsidyIncomeDomain.Update(subsidyIncome);
                        }

                        paymentSession.Complete();
                        recalcManager.SaveEvents();

                        subIncomeDetForUpd.ForEach(x =>
                        {
                            var det = subsidyIncomeDetailDomain.Load(x);
                            det.IsConfirmed = true;
                            subsidyIncomeDetailDomain.Update(det);
                        });

                        nhSession.FlushMode = oldFlush;

                        tr.Commit();
                    }
                    catch (ValidationException e)
                    {
                        paymentSession.Rollback();
                        tr.Rollback();
                        return new BaseDataResult(false, e.Message);
                    }
                    catch (Exception)
                    {
                        paymentSession.Rollback();
                        tr.Rollback();
                        throw;
                    }
                }

                return new BaseDataResult();
            }
            finally
            {
                Container.Release(subsidyIncomeDomain);
                Container.Release(subsidyIncomeDetailDomain);
                Container.Release(realObjPaymentAccountDomain);
                Container.Release(sessionProvider);
                Container.Release(paymentSession);
                Container.Release(recalcManager);
            }
        }

        public IDataResult Undo(BaseParams baseParams)
        {
            var subsidyIncomeDomain = Container.ResolveDomain<SubsidyIncome>();
            var subsidyIncomeDetailDomain = Container.ResolveDomain<SubsidyIncomeDetail>();
            var moneyOperationDomain = Container.ResolveDomain<MoneyOperation>();
            var distributionDetailDomain = Container.ResolveDomain<DistributionDetail>();
            var sessionProvider = Container.Resolve<ISessionProvider>();
            var transferRepo = Container.Resolve<ITransferRepository<PersonalAccountPaymentTransfer>>();
            var paymentSession = Container.Resolve<IRealtyObjectPaymentSession>();
            var recalcManager = Container.Resolve<IPersonalAccountRecalcEventManager>();

            try
            {
                var recIds = baseParams.Params.GetAs("recIds", new long[0]);

                var subsidyIncomeList = subsidyIncomeDomain.GetAll()
                    .Where(x => recIds.Contains(x.Id))
                    .Where(x => x.DistributeState == DistributionState.Distributed 
                        || x.DistributeState == DistributionState.PartiallyDistributed)
                    .ToList();

                var distribs = Container.ResolveAll<IDistribution>()
                        .Where(x => x.Code.Contains("Subsidy"))
                        .GroupBy(x => x.Code)
                        .ToDictionary(x => x.Key, y => y.First());

                using (var tr = Container.Resolve<IDataTransaction>())
                {
                    var nhSession = sessionProvider.GetCurrentSession();
                    var oldFlush = nhSession.FlushMode;
                    nhSession.FlushMode = FlushMode.Never;

                    try
                    {
                        foreach (var subsidyIncome in subsidyIncomeList)
                        {
                            var codes = subsidyIncome.DistributionCode.Split(',');

                            if (subsidyIncome.DistributionCode.IsEmpty())
                            {
                                continue;
                            }

                            var transferQuery = transferRepo.GetByOriginatorGuid(subsidyIncome.TransferGuid);
                            var operations = transferQuery
                                .Where(
                                    x =>
                                        !moneyOperationDomain.GetAll()
                                            .Any(y => y.CanceledOperation.Id == x.Operation.Id))
                                .Where(x => x.Operation.CanceledOperation == null)
                                .Select(x => x.Operation)
                                .AsEnumerable()
                                .Distinct()
                                .OrderBy(x => x.OperationDate)
                                .ToList();

                            foreach (var code in codes)
                            {
                                var canceledOperation = operations.FirstOrDefault();

                                if (canceledOperation == null)
                                {
                                    return new BaseDataResult(false, "Не удалось найти отменяемую операцию");
                                }

                                distribs.Get(code).Undo(subsidyIncome, canceledOperation);

                                operations.Remove(canceledOperation);
                            }

                            subsidyIncome.DistributeState = DistributionState.NotDistributed;
                            subsidyIncome.DistributionCode = string.Empty;
                            subsidyIncomeDomain.Update(subsidyIncome);
                        }

                        ClearDetails(subsidyIncomeList, distributionDetailDomain, subsidyIncomeDetailDomain);

                        paymentSession.Complete();
                        recalcManager.SaveEvents();

                        nhSession.FlushMode = oldFlush;

                        tr.Commit();
                    }
                    catch (ValidationException e)
                    {
                        paymentSession.Rollback();
                        tr.Rollback();
                        return new BaseDataResult(false, e.Message);
                    }
                    catch (Exception)
                    {
                        paymentSession.Rollback();
                        tr.Rollback();
                        throw;
                    }
                }

                return new BaseDataResult();
            }
            finally
            {
                Container.Release(subsidyIncomeDomain);
                Container.Release(subsidyIncomeDetailDomain);
                Container.Release(moneyOperationDomain);
                Container.Release(distributionDetailDomain);
                Container.Release(sessionProvider);
                Container.Release(transferRepo);
                Container.Release(paymentSession);
                Container.Release(recalcManager);
            }
        }

        public IDataResult CheckDate(BaseParams baseParams)
        {
            var recIds = baseParams.Params.GetAs("recIds", new long[0]);
            var detailIds = baseParams.Params.GetAs("detailIds", new long[0]) ?? new long[0];

            var chargeService = Container.Resolve<IChargePeriodRepository>();
            var detailsDomain = Container.ResolveDomain<SubsidyIncomeDetail>();

            try
            {
                var maxDate = detailsDomain.GetAll()
                    .WhereIf(detailIds.Length == 0, x => recIds.Contains(x.SubsidyIncome.Id))
                    .WhereIf(detailIds.Length > 0, x => detailIds.Contains(x.Id))
                    .Select(x => x.DateReceipt)
                    .SafeMax(x => x);

                var currentPeriod = chargeService.GetCurrentPeriod();

                if (currentPeriod.GetEndDate().Date < maxDate)
                {
                    return new BaseDataResult(false,
                        "Дата поступления больше даты окончания текущего периода. Распределение невозможно.");
                }
            }
            finally
            {
                Container.Release(chargeService);
            }

            return new BaseDataResult();
        }

        protected void ClearDetails(List<SubsidyIncome> subsidyIncomes, IDomainService<DistributionDetail> detailDomain, IDomainService<SubsidyIncomeDetail> subIncomeDetailDomain)
        {
            var ids = subsidyIncomes.Select(x => x.Id).ToList();

            detailDomain.GetAll()
                .Where(x => ids.Contains(x.EntityId))
                .Where(x => x.Source == DistributionSource.SubsidyIncome)
                .ForEach(x => detailDomain.Delete(x.Id));

            subIncomeDetailDomain.GetAll()
                .Where(x => ids.Contains(x.SubsidyIncome.Id))
                .Where(x => x.IsConfirmed)
                .ForEach(x =>
                {
                    x.IsConfirmed = false;
                    subIncomeDetailDomain.Update(x);
                });
        }
    }
}