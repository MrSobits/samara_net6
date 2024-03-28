namespace Bars.Gkh.RegOperator.DomainModelServices.Impl.PersonalAccountPayment
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using B4.Utils;

    using Bars.Gkh.Entities;
    using Bars.Gkh.RegOperator.Entities.Refactor;

    using Domain.ValueObjects;
    using Entities;
    using Entities.ValueObjects;
    using Entities.Wallet;
    using Enums;

    /// <summary>
    /// Обработчик для неосновной оплаты по ЛС. Раскидывает оплату по базовому тарифу, тарифу решения и пени
    /// </summary>
    public abstract class AbstractPersonalAccountTariffPaymentCommand : IPersonalAccountPaymentCommand
    {
        /// <summary>
        /// Выполнение команды
        /// </summary>
        /// <param name="account">Лицевой счёт</param>
        /// <param name="source">Источник</param>
        /// <param name="type">Тип</param>
        /// <param name="reserve">Готовые суммы для распределения</param>
        /// <returns>результат оплаты</returns>
        public virtual PersonalAccountPaymentResult Execute(
            BasePersonalAccount account,
            MoneyStream source,
            AmountDistributionType type = AmountDistributionType.Tariff,
            AccountDistributionMoneyReserve reserve = null)
        {
            var result = new PersonalAccountPaymentResult
            {
                DistributionResult = account.Distribute(new AccountDistributionParams(source.Amount, type, reserve))
            };

            var wallet = this.GetWallet(account);

            source.IsAffect = true;
            var mainTransfer = wallet.StoreMoney(TransferBuilder.Create(account, source));

            result.AddTransfer(mainTransfer);

            result.AddTransfer(
                wallet.MoveToAnotherWallet(
                    TransferBuilder.Create(
                        account,
                        new MoneyStream(
                            account.BaseTariffWallet,
                            source.Operation,
                            source.OperationFactDate,
                            result.DistributionResult.ByBaseTariff)
                        {
                            Description = source.Description + " в счёт оплаты по минимальному тарифу",
                            OriginatorName = account.PersonalAccountNum,
                            OriginalTransfer = mainTransfer
                        })));

            result.AddTransfer(
                wallet.MoveToAnotherWallet(
                    TransferBuilder.Create(
                        account,
                        new MoneyStream(
                            account.DecisionTariffWallet,
                            source.Operation,
                            source.OperationFactDate,
                            result.DistributionResult.ByDecisionTariff)
                        {
                            Description = source.Description + " в счёт оплаты по тарифу решения",
                            OriginatorName = account.PersonalAccountNum,
                            OriginalTransfer = mainTransfer
                        })));

            result.AddTransfer(
                wallet.MoveToAnotherWallet(
                    TransferBuilder.Create(
                        account,
                        new MoneyStream(
                            account.PenaltyWallet,
                            source.Operation,
                            source.OperationFactDate,
                            result.DistributionResult.ByPenalty)
                        {
                            Description = source.Description + " в счёт оплаты пени",
                            OriginatorName = account.PersonalAccountNum,
                            OriginalTransfer = mainTransfer
                        })));

            if (mainTransfer.IsNotNull())
            {
                source.OriginalTransfer = mainTransfer;
                this.FireExecuteAction(account, source);
            }

            return result;
        }

        /// <inheritdoc />
        public virtual PersonalAccountPaymentUndoResult Undo( 
            BasePersonalAccount account,
            MoneyOperation operation,
            ChargePeriod period,
            DateTime? cancelDate)
        {
            var wallet = this.GetWallet(account);
            var transfer = this.FindTransferByOperation(wallet.InTransfers, operation.CanceledOperation);
            if (transfer == null)
            {
                return new PersonalAccountPaymentUndoResult();
            }

            var takenForBase =
                account.BaseTariffWallet.InTransfers.SingleOrDefault(x => x.Operation.Id == operation.CanceledOperation.Id);
            var takenForDecision =
                account.DecisionTariffWallet.InTransfers.SingleOrDefault(x => x.Operation.Id == operation.CanceledOperation.Id);
            var takenForPenalty =
                account.PenaltyWallet.InTransfers.SingleOrDefault(x => x.Operation.Id == operation.CanceledOperation.Id);

            var result = new PersonalAccountPaymentUndoResult
            {
                UndoByDecisionTariff = takenForDecision.Return(x => x.Amount),
                UndoByBaseTariff = takenForBase.Return(x => x.Amount),
                UndoPenalty = takenForPenalty.Return(x => x.Amount)
            };

            /* Делаем откат */
            var undoTransfer = wallet.TakeMoney(
                TransferBuilder.Create(
                    account,
                    new MoneyStream(transfer.SourceGuid, operation, cancelDate ?? transfer.PaymentDate, transfer.Amount)
                    {
                        Description = this.UndoReason,
                        OriginatorName = account.PersonalAccountNum,
                        IsAffect = true,
                        OriginalTransfer = transfer
                    }));

            if (takenForBase != null)
            {
                result.AddTransfer(
                    account.BaseTariffWallet.MoveToAnotherWallet(
                        TransferBuilder.Create(
                            account,
                            new MoneyStream(
                                wallet, 
                                operation, 
                                cancelDate ?? takenForBase.PaymentDate, 
                                takenForBase.Amount)
                            {
                                Description = this.UndoReason + " в счёт оплаты по минимальному тарифу",
                                OriginatorName = account.PersonalAccountNum,
                                IsAffect = false,
                                OriginalTransfer = undoTransfer
                            })));
            }

            if (takenForDecision != null)
            {
                result.AddTransfer(
                    account.DecisionTariffWallet.MoveToAnotherWallet(
                    TransferBuilder.Create(
                        account,
                        new MoneyStream(
                            wallet,
                            operation,
                            cancelDate ?? takenForDecision.PaymentDate,
                            takenForDecision.Amount)
                        {
                            Description = this.UndoReason + " в счёт оплаты по тарифу решения",
                            OriginatorName = account.PersonalAccountNum,
                            IsAffect = false,
                            OriginalTransfer = undoTransfer
                        })));
            }

            if (takenForPenalty != null)
            {
                result.AddTransfer(
                    account.PenaltyWallet.MoveToAnotherWallet(
                        TransferBuilder.Create(
                            account,
                            new MoneyStream(
                                wallet,
                                operation, 
                                cancelDate ?? 
                                takenForPenalty.PaymentDate, 
                                takenForPenalty.Amount)
                            {
                                Description = this.UndoReason + " в счёт оплаты пени",
                                OriginatorName = account.PersonalAccountNum,
                                IsAffect = false,
                                OriginalTransfer = undoTransfer
                            })));
            }

            result.AddTransfer(undoTransfer);

            this.FireUndoAction(account, undoTransfer, operation, period);

            return result;
        }

        /// <summary>
        /// Причина отмены
        /// </summary>
        public abstract string UndoReason { get; }

        /// <summary>
        /// Селектор кошелька
        /// </summary>
        public abstract Wallet GetWallet(BasePersonalAccount basePersonalAccount);

        /// <summary>
        /// Сообщить об оплате
        /// </summary>
        public abstract Action<BasePersonalAccount, MoneyStream> FireExecuteAction { get; }

        /// <summary>
        /// Сообщить об отмене оплаты
        /// </summary>
        public abstract Action<BasePersonalAccount, Transfer, MoneyOperation, ChargePeriod> FireUndoAction { get; } 

        /// <summary>
        /// Найти трансфер по операции
        /// </summary>
        /// <param name="transfers"></param>
        /// <param name="operation"></param>
        /// <returns></returns>
        protected Transfer FindTransferByOperation(IEnumerable<Transfer> transfers, MoneyOperation operation)
        {
            return transfers.SingleOrDefault(x => x.Operation.Id == operation.Id);
        }
    }
}