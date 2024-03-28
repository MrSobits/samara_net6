namespace Bars.Gkh.RegOperator.DomainModelServices.Impl.Refund
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using B4.Utils;
    using B4.Utils.Annotations;

    using Bars.Gkh.DomainEvent.Infrastructure;
    using Bars.Gkh.Entities;
    using Bars.Gkh.RegOperator.Entities.Refactor;

    using Domain;
    using Domain.ValueObjects;
    using DomainEvent;
    using DomainEvent.Events.PersonalAccountPayment.Payment;
    using DomainEvent.Events.PersonalAccountRefund;
    using Entities;
    using Entities.ValueObjects;
    using Enums;
    using Exceptions;

    public class PersonalAccountRefundCommand : IPersonalAccountRefundCommand
    {
        public PersonalAccountRefundCommand(RefundType refundType)
        {
            RefundType = refundType;
        }

        public RefundType RefundType { get; set; }

        public PersonalAccountRefundResult Execute(BasePersonalAccount account,
            MoneyStream refund,
            ExecutionMode mode = ExecutionMode.Sequential)
        {
            ArgumentChecker.NotNull(account, "account");
            ArgumentChecker.NotNull(refund, "refund");

            if (refund.Amount <= 0)
            {
                throw new RefundException("Сумма возврата должна быть положительной.");
            }

            var accountBalance = GetAccountBalance(account);

            if (accountBalance < refund.Amount)
            {
                throw new RefundException($"На счету номер {account.PersonalAccountNum} недостаточно денег для возврата.");
            }

            refund.Description = RefundType.GetEnumMeta().Display;

            decimal refundFromTariff = 0M, refundFromDecision = 0M, refundFromPenalty = 0M, refundFromSocSupp = 0M;

            switch (RefundType)
            {
                case RefundType.CrPayments:
                    if (refund.Amount <= account.BaseTariffWallet.Balance)
                    {
                        refundFromTariff = refund.Amount;
                    }
                    else
                    {
                        refundFromTariff = account.BaseTariffWallet.Balance;
                        refundFromDecision = refund.Amount - refundFromTariff;
                    }
                    break;

                case RefundType.Msp:
                    refundFromSocSupp = refund.Amount;
                    break;

                case RefundType.Penalty:
                    refundFromPenalty = refund.Amount;
                    break;
            }

            var result = new PersonalAccountRefundResult(refundFromTariff, refundFromDecision, refundFromPenalty, refundFromSocSupp);

            if (refundFromTariff > 0M)
            {
                var baseTransfer = account.BaseTariffWallet.TakeMoney(
                    TransferBuilder.Create(
                        account,
                        new MoneyStream(
                            refund.Operation.OriginatorGuid,
                            refund.Operation,
                            refund.OperationFactDate,
                            refundFromTariff)
                        {
                            Description = refund.Description,
                            IsAffect = true
                        }));
                result.AddTransfer(baseTransfer);

                DomainEvents.Raise(
                    new PersonalAccountTariffRefundEvent(
                        account,
                        new MoneyStream(
                            refund.Operation.OriginatorGuid,
                            refund.Operation,
                            refund.OperationFactDate,
                            refundFromTariff)
                        {
                            Description = refund.Description + " по базовому тарифу",
                            OriginalTransfer = baseTransfer,
                            IsAffect = true,
                            OriginatorName = account.PersonalAccountNum
                        }));
            }

            if (refundFromDecision > 0M)
            {
                var decisionTransfer = account.DecisionTariffWallet.TakeMoney(
                    TransferBuilder.Create(
                        account,
                        new MoneyStream(
                            refund.Operation.OriginatorGuid,
                            refund.Operation,
                            refund.OperationFactDate,
                            refundFromDecision)
                        {
                            Description = refund.Description,
                            IsAffect = true
                        }));
                result.AddTransfer(decisionTransfer);

                DomainEvents.Raise(
                    new PersonalAccountDecisionRefundEvent(
                        account,
                        new MoneyStream(
                            refund.Operation.OriginatorGuid,
                            refund.Operation,
                            refund.OperationFactDate,
                            refundFromDecision)
                        {
                            Description = refund.Description + " по тарифу решения",
                            OriginalTransfer = decisionTransfer,
                            IsAffect = true,
                            OriginatorName = account.PersonalAccountNum
                        }));
            }

            if (refundFromPenalty > 0M)
            {
                var penaltyTransfer = account.PenaltyWallet.TakeMoney(
                    TransferBuilder.Create(
                        account,
                        new MoneyStream(
                            refund.Operation.OriginatorGuid,
                            refund.Operation,
                            refund.OperationFactDate,
                            refundFromPenalty)
                        {
                            Description = refund.Description,
                            IsAffect = true
                        }));
                result.AddTransfer(penaltyTransfer);

                DomainEvents.Raise(
                    new PersonalAccountPenaltyRefundEvent(
                        account,
                        new MoneyStream(
                            refund.Operation.OriginatorGuid,
                            refund.Operation,
                            refund.OperationFactDate,
                            refundFromPenalty)
                        {
                            Description = refund.Description,
                            OriginalTransfer = penaltyTransfer,
                            IsAffect = true,
                            OriginatorName = account.PersonalAccountNum
                        }));
            }

            if (refundFromSocSupp > 0M)
            {
                var socSuppTransfer = account.SocialSupportWallet.TakeMoney(
                    TransferBuilder.Create(
                        account,
                        new MoneyStream(
                            refund.Operation.OriginatorGuid,
                            refund.Operation,
                            refund.OperationFactDate,
                            refundFromSocSupp)
                        {
                            Description = refund.Description,
                            IsAffect = true
                        }));
                result.AddTransfer(socSuppTransfer);

                DomainEvents.Raise(
                    new PersonalAccountPenaltyRefundEvent(
                        account,
                        new MoneyStream(
                            refund.Operation.OriginatorGuid,
                            refund.Operation,
                            refund.OperationFactDate,
                            refundFromSocSupp)
                        {
                            Description = refund.Description,
                            OriginalTransfer = socSuppTransfer,
                            IsAffect = true,
                            OriginatorName = account.PersonalAccountNum
                        }));
            }

            return result;
        }

        public decimal GetAccountBalance(BasePersonalAccount account)
        {
            var accountBalance = 0M;
            switch (RefundType)
            {
                case RefundType.CrPayments:
                    accountBalance = account.BaseTariffWallet.Balance + account.DecisionTariffWallet.Balance;
                    break;

                case RefundType.Msp:
                    accountBalance = account.SocialSupportWallet.Balance;
                    break;

                case RefundType.Penalty:
                    accountBalance = account.PenaltyWallet.Balance;
                    break;
            }
            return accountBalance;
        }

        public PersonalAccountRefundResult Undo(BasePersonalAccount account, MoneyOperation operation, ChargePeriod period, DateTime? cancelDate)
        {
            var baseTariffTransfers = account.BaseTariffWallet.GetOutTransfers(operation.CanceledOperation.Period)
                .Where(x => x.Operation.Id == operation.CanceledOperation.Id);

            var decisionTariffTransfers = account.DecisionTariffWallet.GetOutTransfers(operation.CanceledOperation.Period)
                .Where(x => x.Operation.Id == operation.CanceledOperation.Id);

            var penaltyTransfers = account.PenaltyWallet.GetOutTransfers(operation.CanceledOperation.Period)
                .Where(x => x.Operation.Id == operation.CanceledOperation.Id);

            var socSuppTransfers = account.SocialSupportWallet.GetOutTransfers(operation.CanceledOperation.Period)
                .Where(x => x.Operation.Id == operation.CanceledOperation.Id);

            decimal baseTariffRefund = 0M, decisionTariffRefund = 0M, penaltyRefund = 0M, socSuppRefund = 0M;
            var transfers = new List<Transfer>();

            foreach (var baseTariffTransfer in baseTariffTransfers)
            {
                baseTariffRefund += baseTariffTransfer.Amount;
                var baseMoney = new MoneyStream(
                    baseTariffTransfer.TargetGuid,
                    operation,
                    cancelDate ?? baseTariffTransfer.PaymentDate,
                    baseTariffTransfer.Amount)
                {
                    Description = "Зачисление по базовому тарифу в счет отмены возврата средств",
                    IsAffect = true,
                    OriginalTransfer = baseTariffTransfer
                };
                var baseTransfer = account.BaseTariffWallet.StoreMoney(TransferBuilder.Create(account, baseMoney));
                transfers.Add(baseTransfer);

                DomainEvents.Raise(
                    new PersonalAccountPaymentByBaseTariffEvent(
                        account,
                        new MoneyStream(
                            account.BaseTariffWallet,
                            operation,
                            baseMoney.OperationFactDate,
                            baseTariffTransfer.Amount)
                        {
                            Description = baseMoney.Description,
                            OriginalTransfer = baseTransfer,
                            OriginatorName = account.PersonalAccountNum,
                            IsAffect = true
                        }));
            }

            foreach (var decisionTariffTransfer in decisionTariffTransfers)
            {
                decisionTariffRefund += decisionTariffTransfer.Amount;
                var decisionMoney = new MoneyStream(
                    decisionTariffTransfer.TargetGuid,
                    operation,
                    cancelDate ?? decisionTariffTransfer.PaymentDate,
                    decisionTariffTransfer.Amount)
                {
                    Description = "Зачисление по тарифу решения в счет отмены возврата средств",
                    IsAffect = true,
                    OriginalTransfer = decisionTariffTransfer
                };
                var decisionTransfer = account.DecisionTariffWallet.StoreMoney(TransferBuilder.Create(account, decisionMoney));
                transfers.Add(decisionTransfer);

                DomainEvents.Raise(
                    new PersonalAccountPaymentByDecisionEvent(
                        new MoneyStream(
                            account.DecisionTariffWallet,
                            operation,
                            decisionMoney.OperationFactDate,
                            decisionTariffTransfer.Amount)
                        {
                            Description = decisionMoney.Description,
                            OriginalTransfer = decisionTransfer,
                            OriginatorName = account.PersonalAccountNum,
                            IsAffect = true
                        },
                        account));
            }

            foreach (var penaltyTransfer in penaltyTransfers)
            {
                penaltyRefund += penaltyTransfer.Amount;
                var penaltyMoney = new MoneyStream(
                    penaltyTransfer.TargetGuid,
                    operation,
                    cancelDate ?? penaltyTransfer.PaymentDate,
                    penaltyTransfer.Amount)
                {
                    Description = "Зачисление по пеням в счет отмены возврата",
                    IsAffect = true,
                    OriginalTransfer = penaltyTransfer
                };
                var penaltyTr = account.PenaltyWallet.StoreMoney(TransferBuilder.Create(account, penaltyMoney));
                transfers.Add(penaltyTr);

                DomainEvents.Raise(
                    new PersonalAccountPenaltyPaymentEvent(
                        new MoneyStream(
                            account.PenaltyWallet,
                            operation,
                            penaltyMoney.OperationFactDate,
                            penaltyTransfer.Amount)
                        {
                            Description = penaltyMoney.Description,
                            OriginalTransfer = penaltyTr,
                            OriginatorName = account.PersonalAccountNum,
                            IsAffect = true
                        },
                        account));
            }

            foreach (var socSuppTransfer in socSuppTransfers)
            {
                socSuppRefund += socSuppTransfer.Amount;
                var socSuppMoney = new MoneyStream(
                    socSuppTransfer.TargetGuid,
                    operation,
                    cancelDate ?? socSuppTransfer.PaymentDate,
                    socSuppTransfer.Amount)
                {
                    Description = "Зачисление по МСП в счет отмены возврата",
                    IsAffect = true,
                    OriginalTransfer = socSuppTransfer
                };
                var socSuppTr = account.SocialSupportWallet.StoreMoney(TransferBuilder.Create(account, socSuppMoney));
                transfers.Add(socSuppTr);

                DomainEvents.Raise(
                    new PersonalAccountSocialSupportPaymentEvent(
                        new MoneyStream(
                            account.SocialSupportWallet,
                            operation,
                            socSuppMoney.OperationFactDate,
                            socSuppTransfer.Amount)
                        {
                            Description = socSuppMoney.Description,
                            OriginalTransfer = socSuppTr,
                            OriginatorName = account.PersonalAccountNum,
                            IsAffect = true
                        },
                        account));
            }

            var result = new PersonalAccountRefundResult(
                baseTariffRefund,
                decisionTariffRefund,
                penaltyRefund,
                socSuppRefund,
                transfers);

            return result;
        }
    }
}