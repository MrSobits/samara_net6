namespace Bars.Gkh.RegOperator.DomainModelServices.Impl.PersonalAccountPayment
{
    using System;
    using System.Linq;
    using B4.Utils;

    using Bars.Gkh.DomainEvent.Infrastructure;
    using Bars.Gkh.Entities;
    using Bars.Gkh.RegOperator.Entities.Refactor;
    using Bars.Gkh.RegOperator.Extenstions;

    using Domain;
    using Domain.ValueObjects;
    using DomainEvent;
    using DomainEvent.Events.PersonalAccountPayment.Payment;
    using DomainEvent.Events.PersonalAccountPayment.UndoPayment;
    using Entities;
    using Entities.ValueObjects;
    using Enums;

    /// <summary>
    /// Закидывает деньги на кошелек пеней ЛС
    /// </summary>
    public class PersonalAccountPenaltyPaymentCommand : IPersonalAccountPaymentCommand
    {
        /// <inheritdoc />
        public PersonalAccountPaymentResult Execute(BasePersonalAccount account, MoneyStream source, AmountDistributionType type = AmountDistributionType.Tariff, AccountDistributionMoneyReserve reserve = null)
        {
            var result = new PersonalAccountPaymentResult
            {
                DistributionResult = new AccountDistributionResult { ByPenalty = source.Amount }
            };

            source.Description = "Оплата пени";
            source.IsAffect = true;

            var transfer = account.PenaltyWallet.StoreMoney(TransferBuilder.Create(account, source));

            result.AddTransfer(transfer);

            if (transfer.IsNotNull())
            {
                DomainEvents.Raise(
                    new PersonalAccountPenaltyPaymentEvent(
                        new MoneyStream(
                            account.PenaltyWallet,
                            source.Operation,
                            source.OperationFactDate,
                            source.Amount) 
                            {
                                OriginalTransfer = transfer,
                                Description = transfer.Reason,
                                OriginatorName = account.PersonalAccountNum,
                                IsAffect = true
                            },
                        account));
            }

            return result;
        }

        /// <inheritdoc />
        public PersonalAccountPaymentUndoResult Undo(BasePersonalAccount account, MoneyOperation operation, ChargePeriod period, DateTime? cancelDate)
        {
            var result = new PersonalAccountPaymentUndoResult();

            var transfers = account.PenaltyWallet.InTransfers
                .Where(x => x.Operation.Id == operation.CanceledOperation.Id)
                .Where(x => !account.PenaltyWallet.IsTransferCancelled(x)); // проверка на попытку повторно отменить оплату по пени

            foreach (var transfer in transfers)
            {
                var undoTransfer = account.PenaltyWallet.TakeMoney(
                    TransferBuilder.Create(
                        account,
                        new MoneyStream(transfer.SourceGuid, operation, cancelDate ?? transfer.PaymentDate, transfer.Amount)
                        {
                            IsAffect = true,
                            Description = "Отмена оплаты пени",
                            OriginalTransfer = transfer,
                            OriginatorName = account.PersonalAccountNum
                        }));

                result.AddTransfer(undoTransfer);
                account.PenaltyWallet.AddOperationTransfer(undoTransfer);

                result.UndoPenalty += transfer.Amount;

                DomainEvents.Raise(new PersonalAccountPenaltyUndoEvent(account, undoTransfer, operation, period, "Отмена оплаты пени"));
            }

            return result;
        }
    }
}