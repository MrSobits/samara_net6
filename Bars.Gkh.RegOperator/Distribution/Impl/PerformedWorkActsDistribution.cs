namespace Bars.Gkh.RegOperator.Distribution.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.RegOperator.Distribution.Args;
    using Bars.Gkh.RegOperator.Domain.Repository.RealityObjectAccount;
    using Bars.Gkh.RegOperator.Domain.Repository.Transfers;
    using Bars.Gkh.RegOperator.Domain.Repository.Wallets;
    using Bars.Gkh.RegOperator.Domain.ValueObjects;
    using Bars.Gkh.RegOperator.DomainModelServices;
    using Bars.Gkh.RegOperator.Entities.Refactor;
    using Bars.Gkh.RegOperator.Entities.ValueObjects;
    using Bars.Gkh.RegOperator.Enums;
    using Bars.Gkh.RegOperator.Extenstions;
    using Bars.Gkh.Repositories.ChargePeriod;
    using Bars.GkhCr.Entities;
    using Bars.GkhCr.Repositories.PerformedWorkActPayments;

    public class PerformedWorkActsDistribution : BaseDistribution
    {
        private readonly ITransferRepository<RealityObjectTransfer> transferRepo;
        private readonly IDomainService<PerformedWorkActPayment> actPaymentDomain;
        private readonly IPerformedWorkActPaymentRepository performedWorkRepo;
        private readonly IDomainService<MoneyLock> moneyLockDomain;
        private readonly IDomainService<MoneyOperation> operationDomain;
        private readonly IChargePeriodRepository periodRepo;
        private readonly IWalletRepository walletRepo;
        private readonly IRealityObjectPaymentAccountRepository accountRepository;
        private readonly IDomainService<DistributionDetail> detailDomain;

        public PerformedWorkActsDistribution(
            ITransferRepository<RealityObjectTransfer> transferRepo,
            IDomainService<PerformedWorkActPayment> actPaymentDomain,
            IWalletRepository walletRepo,
            IDomainService<MoneyLock> moneyLockDomain,
            IPerformedWorkActPaymentRepository performedWorkRepo,
            IRealityObjectPaymentAccountRepository accountRepository,
            IDomainService<MoneyOperation> operationDomain,
            IChargePeriodRepository periodRepo,
            IDomainService<DistributionDetail> detailDomain)
        {
            this.transferRepo = transferRepo;
            this.actPaymentDomain = actPaymentDomain;
            this.walletRepo = walletRepo;
            this.moneyLockDomain = moneyLockDomain;
            this.performedWorkRepo = performedWorkRepo;
            this.accountRepository = accountRepository;
            this.operationDomain = operationDomain;
            this.periodRepo = periodRepo;
            this.detailDomain = detailDomain;
        }

        public override string Route => "work_act_distribution";

        /// <summary>
        /// Код распределения
        /// </summary>
        public override DistributionCode DistributionCode => DistributionCode.PerformedWorkActsDistribution;

        /// <summary>
        /// Идентификатор права доступа
        /// </summary>
        public override string PermissionId => "GkhRegOp.Accounts.SuspenseAccount.EnrollmentTypes.PerformedWorkAct";

        /// <summary>
        /// Проверка применимости распределения к счету НВС
        /// </summary>
        /// <param name="suspenseAccount"></param>
        /// <returns></returns>
        public override bool CanApply(IDistributable suspenseAccount)
        {
            return suspenseAccount.MoneyDirection == MoneyDirection.Outcome;
        }

        public override void Undo(IDistributable distributable, MoneyOperation currentOperation)
        {
            var transfers = this.transferRepo.GetByMoneyOperation(currentOperation).ToList();

            var moneyLocks =
                this.moneyLockDomain.GetAll()
                    .Where(x => x.CancelOperation.OperationGuid == currentOperation.OperationGuid)
                    .ToList();

            var actPaymentGuids = moneyLocks.Select(x => x.TargetGuid).ToList();

            var actPayments = this.actPaymentDomain.GetAll().Where(x => actPaymentGuids.Contains(x.TransferGuid));

            var actions = transfers.Join(moneyLocks,
                    transfer => transfer.TargetGuid + "_" + transfer.SourceGuid,
                    mlock => mlock.TargetGuid + "_" + mlock.Wallet.TransferGuid,
                    (tr, lck) =>
                        new
                        {
                            MoneyLock = lck,
                            Transfer = tr,
                            WalletGuid = tr.SourceGuid,
                            lck.Wallet,
                            ActPayment = actPayments.Single(x => x.TransferGuid == lck.TargetGuid)
                        })
                .ToList();

            var accounts = this.accountRepository.GetSourceAccountsForTransfers(transfers).ToList();

            var undoOperation = distributable.CancelOperation(currentOperation, this.ChargePeriodRepository.GetCurrentPeriod());

            this.operationDomain.Update(currentOperation);
            this.operationDomain.Save(undoOperation);

            var period = this.periodRepo.GetCurrentPeriod();

            foreach (var account in accounts)
            {
                foreach (var action in actions)
                {
                    var description = action.ActPayment.GetActInfo();

                    if (account.IsOwner(action.Wallet))
                    {
                        // Отменяем трансферы
                        var tr = account.StoreMoney(
                            action.Wallet,
                            new MoneyStream(action.Transfer.TargetGuid, undoOperation, distributable.DateReceipt, action.Transfer.Amount)
                            {
                                Description = "Отмена оплаты акта выполненных работ({0})".FormatUsing(action.MoneyLock.SourceName),
                                OriginatorName = description,
                                IsAffect = true
                            },
                            false);

                        // Создаем новые блокировки
                        var lck = account.LockMoney(
                            action.Wallet,
                            undoOperation,
                            action.MoneyLock.Amount,
                            action.MoneyLock.TargetGuid);

                        lck.SourceName = action.MoneyLock.SourceName;

                        action.ActPayment.UndoPayment(action.MoneyLock.Amount);

                        this.moneyLockDomain.Save(lck);
                        this.transferRepo.SaveOrUpdate(tr);
                        this.walletRepo.SaveOrUpdate(lck.Wallet);
                        this.actPaymentDomain.Update(action.ActPayment);
                    }
                }
                this.accountRepository.SaveOrUpdate(account);
            }
        }

        public void Apply(DistributionByPerformedWorkActsArgs args)
        {
            var paymentOrderGuids = args.DistributionRecords.Select(x => x.PerformedWorkActPayment.TransferGuid).ToList();

            var moneyLocksByOriginator =
                this.moneyLockDomain.GetAll()
                    .Where(x => paymentOrderGuids.Contains(x.TargetGuid) && x.IsActive)
                    .ToList()
                    .GroupBy(x => x.TargetGuid)
                    .ToDictionary(x => x.Key);

            var moneyOperation = this.CreateMoneyOperation(args.Distributable);
            moneyOperation.Reason = "Оплата акта выполненных работ";

            var currentPeriod = this.periodRepo.GetCurrentPeriod();

            var details = new List<DistributionDetail>();

            foreach (var distrRecord in args.DistributionRecords)
            {
                var ropayacc =
                    this.accountRepository
                        .GetByRealtyObject(distrRecord.PerformedWorkActPayment.PerformedWorkAct.Realty);

                var description = distrRecord.PerformedWorkActPayment.GetActInfo();

                distrRecord.PerformedWorkActPayment.ApplyPayment(args.SumDistribution != default ? args.SumDistribution / args.DistributionRecords.Count() : distrRecord.Sum, args.Distributable.DateReceipt);
                foreach (var mLock in moneyLocksByOriginator[distrRecord.PerformedWorkActPayment.TransferGuid])
                {
                    ropayacc.UnlockMoney(mLock.Wallet, moneyOperation, mLock);

                    var tr = ropayacc.TakeMoney(
                        mLock.Wallet,
                        new MoneyStream(
                            mLock.TargetGuid,
                            moneyOperation,
                            args.Distributable.DateReceipt,
                            mLock.Amount)
                        {
                            IsAffect = true,
                            Description = "Оплата акта выполненных работ({0})".FormatUsing(mLock.SourceName),
                            OriginatorName = description
                        });

                    details.Add(new DistributionDetail
                    {
                        Sum = tr.Amount,
                        Object = ropayacc.RealityObject.Address
                    });

                    this.operationDomain.Save(moneyOperation);
                    this.walletRepo.SaveOrUpdate(mLock.Wallet);
                    this.transferRepo.SaveOrUpdate(tr);
                }

                this.performedWorkRepo.SaveOrUpdate(distrRecord.PerformedWorkActPayment);
            }

            details.GroupBy(x => x.Object)
                .Select(x => new DistributionDetail(args.Distributable.Id, x.Key, x.Sum(z => z.Sum))
                {
                    Destination = "Распределение оплаты",
                    Source = args.Distributable.Source
                })
                .ForEach(this.detailDomain.Save);

            args.Distributable.DistributeState = DistributionState.Distributed;
        }

        public override IDataResult Apply(IDistributionArgs args)
        {
            var distrArgs = args as DistributionByPerformedWorkActsArgs;
            if (distrArgs == null)
            {
                throw new ArgumentException("args should be convertible to DistributionByPerformedWorkActsArgs");
            }

            this.Apply(distrArgs);

            return new BaseDataResult();
        }

        public override IDistributionArgs ExtractArgsFrom(BaseParams baseParams)
        {
            return DistributionByPerformedWorkActsArgs.FromParams(baseParams);
        }

        public override IDistributionArgs ExtractArgsFromMany(BaseParams baseParams, int counter, decimal thisOneDistribSum)
        {
            return DistributionByPerformedWorkActsArgs.FromManyParams(baseParams, counter, thisOneDistribSum);
        }

        public override IDataResult ListDistributionObjs(BaseParams baseParams)
        {
            var ids = baseParams.Params.GetAs<string>("ids").ToLongArray();
            var distributable = DistributionHelper.ExtractDistributable(baseParams, 0);

            if (distributable == null)
            {
                return new BaseDataResult(false, "Распределяемый объект не существует");
            }

            var acts = this.actPaymentDomain.GetAll()
                .Where(x => ids.Contains(x.PerformedWorkAct.Id))
                .Where(x => x.Paid == 0)
                .Where(x => x.Sum != 0)
                .Select(x => new
                {
                    State = x.PerformedWorkAct.State.Name,
                    x.PerformedWorkAct.ObjectCr.RealityObject.Address,
                    TypeWorkCr = x.PerformedWorkAct.TypeWorkCr.Work.Name,
                    DatePayment = distributable.DateReceipt,
                    PaymentType = x.TypeActPayment,
                    x.Sum,
                    x.Id
                })
                .ToList();

            return new ListDataResult(acts, acts.Count);
        }
    }
}