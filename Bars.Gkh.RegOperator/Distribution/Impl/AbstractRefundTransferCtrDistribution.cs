namespace Bars.Gkh.RegOperator.Distribution.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.RegOperator.Distribution.Args;
    using Bars.Gkh.RegOperator.Domain.Extensions;
    using Bars.Gkh.RegOperator.Domain.Repository.RealityObjectAccount;
    using Bars.Gkh.RegOperator.Domain.Repository.Transfers;
    using Bars.Gkh.RegOperator.Domain.ValueObjects;
    using Bars.Gkh.RegOperator.DomainModelServices;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Entities.ValueObjects;
    using Bars.Gkh.RegOperator.Entities.Wallet;
    using Bars.Gkh.RegOperator.Enums;
    using Bars.Gkh.Repositories.ChargePeriod;

    /// <summary>
    /// Абстрактное распределение по возврату заявке на перечисление средств подрядчикам
    /// </summary>
    public abstract class AbstractRefundTransferCtrDistribution : BaseDistribution
    {
        private readonly IDomainService<TransferCtr> transferCtrDomain;
        private readonly IDomainService<Wallet> walletDomain;
        private readonly IDomainService<MoneyOperation> moneyOperationDomain;
        private readonly IDomainService<Transfer> transferDomain;
        private readonly IChargePeriodRepository periodRepo;
        private readonly IRealityObjectPaymentAccountRepository accountRepo;
        private readonly ITransferRepository<RealityObjectTransfer> transferRepo;
        private readonly IDomainService<DistributionDetail> detailDomain;
        private readonly IDomainService<TransferCtrPaymentDetail> transferCtrDetailDomain;

        /// <summary>
        /// .ctor
        /// </summary>
        protected AbstractRefundTransferCtrDistribution(
            IDomainService<TransferCtr> transferCtrDomain,
            IRealityObjectPaymentAccountRepository accountRepo,
            IChargePeriodRepository periodRepo,
            IDomainService<Wallet> walletDomain,
            IDomainService<MoneyOperation> moneyOperationDomain,
            IDomainService<Transfer> transferDomain,
            ITransferRepository<RealityObjectTransfer> transferRepo,
            IDomainService<DistributionDetail> detailDomain,
            IDomainService<TransferCtrPaymentDetail> transferCtrDetailDomain)
        {
            this.transferCtrDomain = transferCtrDomain;
            this.periodRepo = periodRepo;
            this.accountRepo = accountRepo;
            this.walletDomain = walletDomain;
            this.moneyOperationDomain = moneyOperationDomain;
            this.transferDomain = transferDomain;
            this.transferRepo = transferRepo;
            this.detailDomain = detailDomain;
            this.transferCtrDetailDomain = transferCtrDetailDomain;
        }

        /// <summary>
        /// Роут клиентского контроллера
        /// </summary>
        public override string Route => "refundtransferctrdistribution";

        /// <summary>
        /// MoneyOperation Reason
        /// </summary>
        public abstract string MoneyOperationReason { get; }

        /// <summary>
        /// Получение Transfer's ApplyReason
        /// </summary>
        public abstract string GetApplyTransferReason(TransferCtrPaymentDetail transferCtrDetail, RealityObjectPaymentAccount account);

        /// <summary>
        /// Получение Transfer's UndoReason
        /// </summary>
        public abstract string GetUndoTransferReason(TransferCtrPaymentDetail transferCtrDetail, RealityObjectPaymentAccount account);

        /// <summary>
        /// Проверка применимости распределения к счету НВС
        /// </summary>
        /// <param name="distributable"></param>
        /// <returns></returns>
        public override bool CanApply(IDistributable distributable)
        {
            return distributable.MoneyDirection == MoneyDirection.Income;
        }

        /// <summary>
        /// Отменить распределение
        /// </summary>
        /// <param name="distributable">Счет НВС</param>
        /// <param name="currentOperation">Отменяемая операция</param>
        public override void Undo(IDistributable distributable, MoneyOperation currentOperation)
        {
            var transferQuery = this.transferRepo.GetByMoneyOperation(currentOperation);

            var account = this.accountRepo.GetTargetAccountsForTransfers(transferQuery).First();
            var transferCtr = this.transferCtrDomain.GetAll().Single(x => transferQuery.Any(y => y.SourceGuid == x.TransferGuid));
            var transferCtrDetails = this.transferCtrDetailDomain
                .GetAll()
                .Where(x => x.TransferCtr.Id == transferCtr.Id)
                .ToDictionary(x => x.Wallet.WalletGuid);

            var transfers = transferQuery
                .Where(x => !x.Operation.IsCancelled)
                .ToList();

            var moneyOperation = distributable.CancelOperation(currentOperation, this.ChargePeriodRepository.GetCurrentPeriod());

            foreach (var transfer in transfers)
            {
                var detail = transferCtrDetails.Get(transfer.TargetGuid);

                var wallet = detail.Wallet;

                var undoTransfer = account.UndoPayment(
                    wallet,
                    new MoneyStream(distributable, moneyOperation, distributable.DateReceipt, transfer.Amount)
                    {
                        IsAffect = true,
                        OriginatorName = transferCtr.GetMyInfo(),
                        Description = this.GetUndoTransferReason(detail, account)
                    });

                detail.RefundSum -= undoTransfer.Amount;
                transferCtr.PaidSum -= undoTransfer.Amount;

                this.moneyOperationDomain.SaveOrUpdate(moneyOperation);
                this.walletDomain.SaveOrUpdate(wallet);
                this.transferCtrDetailDomain.Update(detail);
                this.transferDomain.Save(undoTransfer);
            }

            // обнуляем даты заявки пп
            transferCtr.DateFromPp = null;

            this.transferCtrDomain.SaveOrUpdate(transferCtr);
            this.accountRepo.SaveOrUpdate(account);
        }

        /// <summary>
        /// Применить распределение
        /// </summary>
        /// <param name="args"></param>
        public override IDataResult Apply(IDistributionArgs args)
        {
            var distrArgs = args as DistributionByRefundTransferCtrArgs;
            if (distrArgs == null)
            {
                throw new ArgumentException("args should be convertible to DistributionByRefundTransferCtrArgs");
            }

            this.Apply(distrArgs);

            return new BaseDataResult();
        }

        /// <summary>
        /// Применить распределение
        /// </summary>
        /// <param name="args"></param>
        protected void Apply(DistributionByRefundTransferCtrArgs args)
        {
            var transferCtr = args.DistributionRecords.Select(x => x.TransferCtrDetail.TransferCtr).First();
            var ropayacc = this.accountRepo.GetByRealtyObject(transferCtr.ObjectCr.RealityObject);

            var moneyOperation = this.CreateMoneyOperation(args.Distributable);
            moneyOperation.Reason = this.MoneyOperationReason;

            var details = new List<DistributionDetail>();

            foreach (var distrRecord in args.DistributionRecords)
            {
                var tr = ropayacc.StoreMoney(
                    distrRecord.TransferCtrDetail.Wallet,
                    new MoneyStream(
                        transferCtr,
                        moneyOperation,
                        DateTime.Now,
                        args.SumDistribution != default ? args.SumDistribution / args.DistributionRecords.Count() : distrRecord.Sum)
                    {
                        IsAffect = true,
                        OriginatorName = transferCtr.GetMyInfo(),
                        Description = this.GetApplyTransferReason(distrRecord.TransferCtrDetail, ropayacc)
                    });

                details.Add(new DistributionDetail(args.Distributable.Id, distrRecord.TransferCtrDetail.TransferCtr.ObjectCr.RealityObject.Address, tr.Amount)
                {
                    Source = args.Distributable.Source,
                    Destination = "Распределение оплаты"
                });

                distrRecord.TransferCtrDetail.RefundSum += tr.Amount;
                transferCtr.PaidSum -= tr.Amount;

                this.moneyOperationDomain.SaveOrUpdate(moneyOperation);
                this.walletDomain.SaveOrUpdate(distrRecord.TransferCtrDetail.Wallet);
                this.transferDomain.SaveOrUpdate(tr);
                this.transferCtrDetailDomain.Update(distrRecord.TransferCtrDetail);
            }

            // проставление даты заявки пп
            transferCtr.DateFromPp = args.Distributable.DateReceipt;

            details.ForEach(this.detailDomain.Save);
            this.transferCtrDomain.SaveOrUpdate(transferCtr);
            this.accountRepo.SaveOrUpdate(ropayacc);
        }

        /// <summary>
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        public override IDistributionArgs ExtractArgsFrom(BaseParams baseParams)
        {
            return DistributionByRefundTransferCtrArgs.FromParams(baseParams);
        }

        /// <summary>
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        public override IDistributionArgs ExtractArgsFromMany(BaseParams baseParams, int counter, decimal thisOneDistribSum)
        {
            return DistributionByRefundTransferCtrArgs.FromManyParams(baseParams, counter, thisOneDistribSum);
        }

        /// <summary>
        /// Получить объекты распределения
        /// </summary>
        /// <param name="baseParams">параметры клиента</param>
        /// <returns>результат запроса</returns>
        public override IDataResult ListDistributionObjs(BaseParams baseParams)
        {
            var ids = baseParams.Params.GetAs<string>("ids").ToLongArray();

            if (ids == null || ids.Length == 0)
            {
                return new BaseDataResult(false, "Не выбраны записи для распределения");
            }

            var distributionSum = baseParams.Params.GetAs<decimal?>("distribSum");
            var distributable = DistributionHelper.ExtractDistributable(baseParams, 0);

            if (distributionSum == null)
            {
                distributionSum = distributable.RemainSum == 0 ? distributable.Sum : distributable.RemainSum;
            }

            var distribSum = distributionSum.ToDecimal();

            var transferCtrs = this.transferCtrDetailDomain.GetAll().Where(x => ids.Contains(x.TransferCtr.Id)).Select(x => x.TransferCtr).Distinct()
                .ToList();

            if (transferCtrs.Count > 1)
            {
                return new BaseDataResult(false, "{0} возможно выполнить только для одной заявки".FormatUsing(this.MoneyOperationReason));
            }

            var transferCtr = transferCtrs.First();

            var ropayacc = this.accountRepo.GetByRealtyObject(transferCtr.ObjectCr.RealityObject);

            var ctrTransfers = this.transferCtrDetailDomain.GetAll()
                .Where(x => ids.Contains(x.TransferCtr.Id))
                .Select(x => new
                {
                    x.Id,
                    x.Wallet,
                    x.Amount,
                })
                .ToArray()
                .Select(x => new
                {
                    x.Id,
                    WalletName = x.Wallet.GetWalletName(this.Container, ropayacc),
                    x.Amount,
                    Sum = x.Amount
                })
                .ToList();

            return new ListDataResult(ctrTransfers, ctrTransfers.Count);
        }
    }
}