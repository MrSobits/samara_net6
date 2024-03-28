namespace Bars.Gkh.RegOperator.Distribution.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Domain.CollectionExtensions;
    using Bars.Gkh.RegOperator.Distribution.Args;
    using Bars.Gkh.RegOperator.Domain.Repository.RealityObjectAccount;
    using Bars.Gkh.RegOperator.Domain.Repository.Transfers;
    using Bars.Gkh.RegOperator.Domain.ValueObjects;
    using Bars.Gkh.RegOperator.DomainModelServices;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Entities.Refactor;
    using Bars.Gkh.RegOperator.Entities.ValueObjects;
    using Bars.Gkh.RegOperator.Entities.Wallet;
    using Bars.Gkh.RegOperator.Enums;
    using Bars.Gkh.RegOperator.Extenstions;
    using Bars.Gkh.Repositories.ChargePeriod;

    /// <summary>
    /// Абстрактное распределение по заявке на перечисление средств подрядчикам
    /// </summary>
    public abstract class AbstractTransferCtrDistribution : BaseDistribution
    {
        private readonly IDomainService<TransferCtr> transferCtrDomain;
        private readonly IDomainService<MoneyLock> moneyLockDomain;
        private readonly IDomainService<Wallet> walletDomain;
        private readonly IDomainService<MoneyOperation> moneyOperationDomain;
        private readonly IChargePeriodRepository periodRepo;
        private readonly IRealityObjectPaymentAccountRepository accountRepo;
        private readonly ITransferRepository<RealityObjectTransfer> transferRepo;
        private readonly IDomainService<DistributionDetail> detailDomain;

        /// <summary>
        /// .ctor
        /// </summary>
        protected AbstractTransferCtrDistribution(
            IDomainService<TransferCtr> transferCtrDomain,
            IDomainService<MoneyLock> moneyLockDomain,
            IRealityObjectPaymentAccountRepository accountRepo,
            IChargePeriodRepository periodRepo,
            IDomainService<Wallet> walletDomain,
            IDomainService<MoneyOperation> moneyOperationDomain,
            ITransferRepository<RealityObjectTransfer> transferRepo,
            IDomainService<DistributionDetail> detailDomain)
        {
            this.transferCtrDomain = transferCtrDomain;
            this.moneyLockDomain = moneyLockDomain;
            this.periodRepo = periodRepo;
            this.accountRepo = accountRepo;
            this.walletDomain = walletDomain;
            this.moneyOperationDomain = moneyOperationDomain;
            this.transferRepo = transferRepo;
            this.detailDomain = detailDomain;
        }

        /// <summary>
        /// Роут клиентского контроллера
        /// </summary>
        public override string Route => "transferctrdistribution";

        /// <summary>
        /// MoneyOperation Reason
        /// </summary>
        public abstract string MoneyOperationReason { get; }

        /// <summary>
        /// Получение Transfer's ApplyReason
        /// </summary>
        public abstract string GetApplyTransferReason(TransferCtr transferCtr, MoneyLock mLock);

        /// <summary>
        /// Получение Transfer's UndoReason
        /// </summary>
        public abstract string GetUndoTransferReason(TransferCtr transferCtr, MoneyLock mLock);

        /// <summary>
        /// Проверка применимости распределения к счету НВС
        /// </summary>
        /// <param name="suspenseAccount"></param>
        /// <returns></returns>
        public override bool CanApply(IDistributable suspenseAccount)
        {
            return suspenseAccount.MoneyDirection == MoneyDirection.Outcome;
        }

        /// <summary>
        /// Отменить распределение
        /// </summary>
        /// <param name="distributable">Счет НВС</param>
        /// <param name="currentOperation">Отменяемая операция</param>
        public override void Undo(IDistributable distributable, MoneyOperation currentOperation)
        {
            var transfers = this.transferRepo.GetByMoneyOperation(currentOperation).ToList();

            var moneyLocks = this.moneyLockDomain.GetAll()
                .Where(x => x.CancelOperation.OperationGuid == currentOperation.OperationGuid)
                .ToList();

            var guids = moneyLocks.Select(x => x.TargetGuid).ToList();

            var transfersCtr = this.transferCtrDomain.GetAll().Where(x => guids.Contains(x.TransferGuid));

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
                            TransferCtr = transfersCtr.Single(x => x.TransferGuid == lck.TargetGuid)
                        })
                .ToList();

            var accounts = this.accountRepo.GetSourceAccountsForTransfers(transfers).ToList();

            var undoOperation = distributable.CancelOperation(currentOperation, this.ChargePeriodRepository.GetCurrentPeriod());

            this.moneyOperationDomain.Update(currentOperation);
            this.moneyOperationDomain.Save(undoOperation);

            foreach (var account in accounts)
            {
                foreach (var action in actions)
                {
                    if (account.IsOwner(action.Wallet))
                    {
                        // Отменяем трансферы
                        var tr = account.StoreMoney(
                            action.Wallet,
                            new MoneyStream(
                                action.Transfer.TargetGuid,
                                undoOperation,
                                distributable.DateReceipt,
                                action.Transfer.Amount)
                            {
                                OriginatorName = action.TransferCtr.GetMyInfo(),
                                Description = this.GetUndoTransferReason(action.TransferCtr, action.MoneyLock),
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

                        action.TransferCtr.UndoPayment(action.MoneyLock.Amount);

                        // обнуление даты заявки пп
                        action.TransferCtr.DateFromPp = null;

                        this.moneyLockDomain.Save(lck);
                        this.transferRepo.SaveOrUpdate(tr);
                        this.walletDomain.SaveOrUpdate(lck.Wallet);
                        this.transferCtrDomain.SaveOrUpdate(action.TransferCtr);
                    }
                }

                this.accountRepo.SaveOrUpdate(account);
            }
        }

        /// <summary>
        /// Применить распределение
        /// </summary>
        /// <param name="args">аргументы распределения</param>
        public override IDataResult Apply(IDistributionArgs args)
        {
            var distrArgs = args as DistributionByTransferCtrArgs;
            if (distrArgs == null)
            {
                throw new ArgumentException("args should be convertible to DistributionByTransferCtrArgs");
            }

            this.Apply(distrArgs);

            return new BaseDataResult();
        }

        /// <summary>
        /// Применить распределение
        /// </summary>
        /// <param name="args">аргументы распределения</param>
        protected void Apply(DistributionByTransferCtrArgs args)
        {
            var paymentOrderGuids = args.DistributionRecords.Select(x => x.TransferCtr.TransferGuid).ToList();

            var moneyLocksByOriginator = this.moneyLockDomain.GetAll()
                .Where(x => x.IsActive)
                .Where(x => paymentOrderGuids.Contains(x.TargetGuid))
                .ToList()
                .GroupBy(x => x.TargetGuid)
                .ToDictionary(x => x.Key);

            var moneyOperation = this.CreateMoneyOperation(args.Distributable);
            moneyOperation.Reason = this.MoneyOperationReason;

            var details = new List<DistributionDetail>();

            foreach (var distrRecord in args.DistributionRecords)
            {
                var ropayacc = this.accountRepo
                    .GetByRealtyObject(distrRecord.TransferCtr.ObjectCr.RealityObject);

                distrRecord.TransferCtr.ApplyPayment(args.SumDistribution != default ? args.SumDistribution / args.DistributionRecords.Count() : distrRecord.Sum, args.Distributable.DateReceipt);

                // проставление даты заявки пп
                distrRecord.TransferCtr.DateFromPp = args.Distributable.DateReceipt;

                IGrouping<string, MoneyLock> mLocks;
                moneyLocksByOriginator.TryGetValue(distrRecord.TransferCtr.TransferGuid, out mLocks);

                foreach (var mLock in mLocks ?? Enumerable.Empty<MoneyLock>())
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
                            OriginatorName = distrRecord.TransferCtr.GetMyInfo(),
                            Description = this.GetApplyTransferReason(distrRecord.TransferCtr, mLock)
                        });

                    details.Add(new DistributionDetail(args.Distributable.Id, distrRecord.TransferCtr.ObjectCr.RealityObject.Address, tr.Amount)
                    {
                        Source = args.Distributable.Source,
                        Destination = "Распределение оплаты"
                    });

                    this.moneyOperationDomain.SaveOrUpdate(moneyOperation);
                    this.walletDomain.SaveOrUpdate(mLock.Wallet);
                    this.transferRepo.SaveOrUpdate(tr);
                }

                this.accountRepo.SaveOrUpdate(ropayacc);
                this.transferCtrDomain.SaveOrUpdate(distrRecord.TransferCtr);
            }

            details.ForEach(this.detailDomain.Save);

            args.Distributable.DistributeState = DistributionState.Distributed;
        }

        /// <summary>
        /// </summary>
        /// <param name="baseParams">параметры клиента</param>
        /// <returns>результат запроса</returns>
        public override IDistributionArgs ExtractArgsFrom(BaseParams baseParams)
        {
            return DistributionByTransferCtrArgs.FromParams(baseParams);
        }

        /// <summary>
        /// </summary>
        /// <param name="baseParams">параметры клиента</param>
        /// <returns>результат запроса</returns>
        public override IDistributionArgs ExtractArgsFromMany(BaseParams baseParams, int counter, decimal thisOneDistribSum)
        {
            return DistributionByTransferCtrArgs.FromManyParams(baseParams, counter, thisOneDistribSum);
        }

        /// <summary>
        /// Получить объекты распределения
        /// </summary>
        /// <param name="baseParams">параметры клиента</param>
        /// <returns>результат запроса</returns>
        public override IDataResult ListDistributionObjs(BaseParams baseParams)
        {
            var ids = baseParams.Params.GetAs<string>("ids").ToLongArray();
            var distribSum = baseParams.Params.GetAs<decimal>("distribSum");

            if (ids == null || ids.Length == 0)
            {
                return new BaseDataResult(false, "Не выбраны записи для распределения");
            }

            var ctrTransfers = this.transferCtrDomain.GetAll()
                .Where(x => ids.Contains(x.Id))
                .Select(x => new
                {
                    x.Id,
                    x.DocumentNum,
                    x.DateFrom,
                    ObjectCr = x.ObjectCr.RealityObject.Address,
                    TypeWorkCr = x.TypeWorkCr.Work.Name,
                    x.Sum,
                    x.PaidSum
                })
                .ToList();

            if (distribSum < ctrTransfers.SafeSum(x => x.Sum))
            {
                return new BaseDataResult(false,
                    "Сумма к распределению не может быть меньше общей суммы к оплате по всем заявкам");
            }

            return new ListDataResult(ctrTransfers, ctrTransfers.Count);
        }

        /// <summary>
        /// Очищение таблицы детализации по заявке
        /// </summary>
        /// <param name="distributable">распределяемый объект</param>
        protected void ClearDetails(IDistributable distributable)
        {
            this.detailDomain.GetAll()
                .Where(x => x.EntityId == distributable.Id)
                .Where(x => x.Source == distributable.Source)
                .ForEach(x => this.detailDomain.Delete(x.Id));
        }
    }
}