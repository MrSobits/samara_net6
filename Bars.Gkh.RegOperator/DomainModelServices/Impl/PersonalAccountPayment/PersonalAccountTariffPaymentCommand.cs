namespace Bars.Gkh.RegOperator.DomainModelServices.Impl.PersonalAccountPayment
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.Utils;
    using Bars.Gkh.DomainEvent.Infrastructure;
    using Bars.Gkh.Entities;
    using Bars.Gkh.RegOperator.Domain;
    using Bars.Gkh.RegOperator.Domain.ValueObjects;
    using Bars.Gkh.RegOperator.DomainEvent;
    using Bars.Gkh.RegOperator.DomainEvent.Events.PersonalAccountPayment.Payment;
    using Bars.Gkh.RegOperator.DomainEvent.Events.PersonalAccountPayment.UndoPayment;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Entities.Refactor;
    using Bars.Gkh.RegOperator.Entities.ValueObjects;
    using Bars.Gkh.RegOperator.Enums;
    using Bars.Gkh.RegOperator.Extenstions;

    /// <summary>
    /// Закидывает деньги на кошельки оплат по базовому, тарифу решения и пени
    /// </summary>
    public class PersonalAccountTariffPaymentCommand : IPersonalAccountPaymentCommand
    {
        public PersonalAccountPaymentResult Execute(BasePersonalAccount account, MoneyStream source, AmountDistributionType type = AmountDistributionType.Tariff, AccountDistributionMoneyReserve reserve = null)
        {
            var result = new PersonalAccountPaymentResult
                {
                    DistributionResult = account.Distribute(new AccountDistributionParams(source.Amount, type, reserve))
                };

            Transfer baseTransfer = null;
            if (result.DistributionResult.ByBaseTariff != 0)
            {
                baseTransfer = account.BaseTariffWallet.StoreMoney(
                    TransferBuilder.Create(
                        account,
                        new MoneyStream(
                            source.SourceOrTargetGuid,
                            source.Operation,
                            source.OperationFactDate,
                            result.DistributionResult.ByBaseTariff)
                        {
                            Description = "Оплата по базовому тарифу",
                            IsAffect = true
                        }));
            }
            result.AddTransfer(baseTransfer);

            Transfer decisionTransfer = null;
            if (result.DistributionResult.ByDecisionTariff != 0)
            {
                decisionTransfer = account.DecisionTariffWallet.StoreMoney(
                    TransferBuilder.Create(
                        account,
                        new MoneyStream(
                            source.SourceOrTargetGuid,
                            source.Operation,
                            source.OperationFactDate,
                            result.DistributionResult.ByDecisionTariff)
                        {
                            Description = "Оплата по тарифу решения",
                            IsAffect = true
                        }));
            }
            result.AddTransfer(decisionTransfer);

            Transfer penaltyTransfer = null;
            if (result.DistributionResult.ByPenalty != 0)
            {
                penaltyTransfer = account.PenaltyWallet.StoreMoney(
                    TransferBuilder.Create(
                        account,
                        new MoneyStream(
                            source.SourceOrTargetGuid,
                            source.Operation,
                            source.OperationFactDate,
                            result.DistributionResult.ByPenalty)
                        {
                            Description = "Оплата пени",
                            IsAffect = true
                        }));
            }
            result.AddTransfer(penaltyTransfer);

            if (baseTransfer.IsNotNull())
            {
                DomainEvents.Raise(
                    new PersonalAccountPaymentByBaseTariffEvent(
                        account,
                        new MoneyStream(
                            account.BaseTariffWallet,
                            source.Operation,
                            source.OperationFactDate,
                            baseTransfer.Amount)
                        {
                            OriginalTransfer = baseTransfer,
                            Description = baseTransfer.Reason,
                            IsAffect = true,
                            OriginatorName = account.PersonalAccountNum
                        }));
            }

            if (decisionTransfer.IsNotNull())
            {
                DomainEvents.Raise(
                    new PersonalAccountPaymentByDecisionEvent(
                        new MoneyStream(
                            account.DecisionTariffWallet,
                            source.Operation,
                            source.OperationFactDate,
                            decisionTransfer.Amount)
                        {
                            OriginalTransfer = decisionTransfer,
                            Description = decisionTransfer.Reason,
                            IsAffect = true,
                            OriginatorName = account.PersonalAccountNum
                        },
                        account));
            }

            if (penaltyTransfer.IsNotNull())
            {
                DomainEvents.Raise(
                    new PersonalAccountPenaltyPaymentEvent(
                        new MoneyStream(
                            account.PenaltyWallet,
                            source.Operation,
                            source.OperationFactDate,
                            penaltyTransfer.Amount)
                        {
                            OriginalTransfer = penaltyTransfer,
                            Description = penaltyTransfer.Reason,
                            IsAffect = true,
                            OriginatorName = account.PersonalAccountNum
                        },
                        account));
            }

            return result;
        }

        public PersonalAccountPaymentUndoResult Undo(
            BasePersonalAccount account,
            MoneyOperation operation,
            ChargePeriod period,
            DateTime? cancelDate)
        {
            var result = new PersonalAccountPaymentUndoResult();

            var bTransfers = account.BaseTariffWallet.GetInTransfers(operation.CanceledOperation.Period)
                .Where(x => x.Operation.Id == operation.CanceledOperation.Id)
                .Where(x => !account.BaseTariffWallet.IsTransferCancelled(x)); // проверка на попытку повторно отменить оплату

            var dTransfers = account.DecisionTariffWallet.GetInTransfers(operation.CanceledOperation.Period)
                .Where(x => x.Operation.Id == operation.CanceledOperation.Id)
                .Where(x => !account.DecisionTariffWallet.IsTransferCancelled(x)); // проверка на попытку повторно отменить оплату

            var pTransfers = account.PenaltyWallet.GetInTransfers(operation.CanceledOperation.Period)
                .Where(x => x.Operation.Id == operation.CanceledOperation.Id)
                .Where(x => !account.PenaltyWallet.IsTransferCancelled(x)); // проверка на попытку повторно отменить оплату

            Transfer undoTransfer;

            foreach (var bTransfer in bTransfers)
            {
                result.UndoByBaseTariff += bTransfer.Amount;

                undoTransfer = account.BaseTariffWallet.TakeMoney(
                    TransferBuilder.Create(
                        account,
                        new MoneyStream(bTransfer.SourceGuid, operation, cancelDate ?? bTransfer.PaymentDate, bTransfer.Amount)
                        {
                            Description = "Отмена оплаты по базовому тарифу",
                            IsAffect = true,
                            OriginalTransfer = bTransfer,
                            OriginatorName = account.PersonalAccountNum
                        }));

                if (undoTransfer != null)
                {
                    result.AddTransfer(undoTransfer);
                    account.BaseTariffWallet.AddOperationTransfer(undoTransfer);

                    DomainEvents.Raise(new PersonalAccountTariffUndoEvent(account, undoTransfer, operation, period, "Отмена оплаты по базовому тарифу"));
                }
            }

            foreach (var dTransfer in dTransfers)
            {
                result.UndoByDecisionTariff += dTransfer.Amount;

                undoTransfer = account.DecisionTariffWallet.TakeMoney(
                    TransferBuilder.Create(
                        account,
                        new MoneyStream(dTransfer.SourceGuid, operation, cancelDate ?? dTransfer.PaymentDate, dTransfer.Amount)
                        {
                            Description = "Отмена оплаты по тарифу решения",
                            IsAffect = true,
                            OriginalTransfer = dTransfer,
                            OriginatorName = account.PersonalAccountNum
                        }));

                result.AddTransfer(undoTransfer);
                account.DecisionTariffWallet.AddOperationTransfer(undoTransfer);

                DomainEvents.Raise(new PersonalAccountDecisionUndoEvent(account, undoTransfer, operation, period, "Отмена оплаты по тарифу решения"));
            }


            foreach (var pTransfer in pTransfers)
            {
                result.UndoPenalty += pTransfer.Amount;

                undoTransfer = account.PenaltyWallet.TakeMoney(
                    TransferBuilder.Create(
                        account,
                        new MoneyStream(pTransfer.SourceGuid, operation, cancelDate ?? pTransfer.PaymentDate, pTransfer.Amount)
                        {
                            Description = "Отмена оплаты пени",
                            IsAffect = true,
                            OriginalTransfer = pTransfer,
                            OriginatorName = account.PersonalAccountNum
                        }));

                result.AddTransfer(undoTransfer);
                account.PenaltyWallet.AddOperationTransfer(undoTransfer);

                DomainEvents.Raise(new PersonalAccountPenaltyUndoEvent(account, undoTransfer, operation, period, "Отмена оплаты пени"));
            }

            return result;
        }
    }
}