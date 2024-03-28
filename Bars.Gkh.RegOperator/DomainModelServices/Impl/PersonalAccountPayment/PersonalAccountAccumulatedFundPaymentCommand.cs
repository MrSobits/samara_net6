namespace Bars.Gkh.RegOperator.DomainModelServices.Impl.PersonalAccountPayment
{
    using System;
    using B4.Utils;

    using Bars.Gkh.DomainEvent.Infrastructure;
    using Bars.Gkh.Entities;

    using Domain;
    using Domain.ValueObjects;
    using DomainEvent;
    using DomainEvent.Events.PersonalAccountPayment;
    using DomainEvent.Events.PersonalAccountPayment.UndoPayment;
    using Entities;
    using Entities.ValueObjects;
    using Entities.Wallet;
    using Enums;

    /// <summary>
    /// Команда оплаты с типом "Поступление ранее накопленных средств"
    /// </summary>
    public class PersonalAccountAccumulatedFundPaymentCommand : AbstractPersonalAccountTariffPaymentCommand
    {
        public override PersonalAccountPaymentResult Execute(BasePersonalAccount account, MoneyStream source, AmountDistributionType type = AmountDistributionType.Tariff, AccountDistributionMoneyReserve reserve = null)
        {
            source.Description = "Поступление ранее накопленных средств";

            return base.Execute(account, source, type, reserve);
        }

        public override string UndoReason
        {
            get { return "Отмены поступления ранее накопленных средств"; }
        }

        public override Wallet GetWallet(BasePersonalAccount basePersonalAccount)
        {
            return basePersonalAccount.AccumulatedFundWallet; 
        }

        public override Action<BasePersonalAccount, MoneyStream> FireExecuteAction
        {
            get
            {
                return (a, s) =>
                {
                    DomainEvents.Raise(
                           new PersonalAccountAccumulatedFundPaymentEvent(
                               new MoneyStream(a.AccumulatedFundWallet,
                                   s.Operation,
                                   s.OperationFactDate,
                                   s.Amount)
                               {
                                   OriginalTransfer = s.OriginalTransfer,
                                   Description = s.OriginalTransfer.Return(x => x.Reason),
                                   OriginatorName = s.OriginalTransfer.Return(x => x.OriginatorName),
                                   IsAffect = true
                               }, a));
                };
            }
        }

        public override Action<BasePersonalAccount, Transfer, MoneyOperation, ChargePeriod> FireUndoAction
        {
            get { return (a, t, o, p) => DomainEvents.Raise(new PersonalAccountAccumulatedFundUndoEvent(a, t, o, p, UndoReason)); }
        }
    }
}