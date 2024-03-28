namespace Bars.Gkh.RegOperator.DomainModelServices.Impl.PersonalAccountPayment
{
    using System;
    using System.Collections.Generic;
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
    public class PersonalAccountSocialSupportPaymentCommand : IPersonalAccountPaymentCommand
    {
        /// <inheritdoc />
        public PersonalAccountPaymentResult Execute(BasePersonalAccount account, MoneyStream source, AmountDistributionType type = AmountDistributionType.Tariff, AccountDistributionMoneyReserve reserve = null)
        {
            var result = new PersonalAccountPaymentResult
            {
                DistributionResult = new AccountDistributionResult { BySocSupport = source.Amount }
            };

            source.Description = "Поступление денег соц. поддержки";

            var mainTransfer = account.SocialSupportWallet.StoreMoney(TransferBuilder.Create(account, source));
            result.AddTransfer(mainTransfer);

            if (mainTransfer.IsNotNull())
            {
                DomainEvents.Raise(
                    new PersonalAccountSocialSupportPaymentEvent(
                        new MoneyStream(
                            account.SocialSupportWallet,
                            source.Operation,
                            source.OperationFactDate,
                            source.Amount)
                        {
                            OriginalTransfer = mainTransfer,
                            Description = mainTransfer.Reason,
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
            var transfers = account.SocialSupportWallet.GetInTransfers(operation.CanceledOperation.Period)
                .Where(x => x.Operation.Id == operation.CanceledOperation.Id)
                .Where(x => !account.SocialSupportWallet.IsTransferCancelled(x))
                .ToList();

            var result = new PersonalAccountPaymentUndoResult();

            foreach (var transfer in transfers)
            {
                var undoTransfer = account.SocialSupportWallet.TakeMoney(
                    TransferBuilder.Create(
                        account,
                        new MoneyStream(
                            transfer.SourceGuid,
                            operation,
                            cancelDate ?? transfer.PaymentDate,
                            transfer.Amount)
                        {
                            Description = "Отмена поступления по соц. поддержке",
                            IsAffect = true,
                            OriginalTransfer = transfer,
                            OriginatorName = account.PersonalAccountNum
                        }));

                result.AddTransfer(undoTransfer);
                result.UndoBySocSupport += transfer.Amount;

                DomainEvents.Raise(
                    new PersonalAccountSocialSupportUndoEvent(
                        account,
                        undoTransfer,
                        operation,
                        period,
                        "Отмена поступления по соц. поддержке"));
            }

            return result;
        }
    }
}