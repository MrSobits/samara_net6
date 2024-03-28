namespace Bars.Gkh.RegOperator.DomainModelServices.Impl.PersonalAccountPayment
{
    using System;
    using B4.Utils;

    using Bars.Gkh.DomainEvent.Infrastructure;
    using Bars.Gkh.Entities;

    using Domain;
    using Domain.ValueObjects;
    using DomainEvent;
    using DomainEvent.Events.PersonalAccountPayment.Payment;
    using DomainEvent.Events.PersonalAccountPayment.UndoPayment;
    using Entities;
    using Entities.ValueObjects;
    using Entities.Wallet;
    using Enums;

    /// <summary>
    /// Закидывает деньги на кошелек предыдущих работ
    /// </summary>
    public class PersonalAccountPreviosWorkPaymentCommand : AbstractPersonalAccountTariffPaymentCommand
    {
        public override PersonalAccountPaymentResult Execute(BasePersonalAccount account, MoneyStream source, AmountDistributionType type = AmountDistributionType.Tariff, AccountDistributionMoneyReserve reserve = null)
        {
            source.Description = "Поступление за проделанные работы";

            return base.Execute(account, source, type, reserve);
        }

        public override string UndoReason
        {
            get { return "Отмена поступления средств за ранее выполненные работы"; }
        }

        public override Wallet GetWallet(BasePersonalAccount basePersonalAccount)
        {
            return basePersonalAccount.PreviosWorkPaymentWallet;
        }

        public override Action<BasePersonalAccount, MoneyStream> FireExecuteAction
        {
            get
            {
                return (a, s) =>
                {
                    DomainEvents.Raise(
                            new PersonalAccountPreviousWorkPaymentEvent(
                                new MoneyStream(
                                    a.PreviosWorkPaymentWallet,
                                    s.Operation,
                                    s.OperationFactDate,
                                    s.Amount)
                                {
                                    OriginalTransfer = s.OriginalTransfer,
                                    Description = s.OriginalTransfer.Return(x => x.Reason),
                                    OriginatorName = a.PersonalAccountNum,
                                    IsAffect = true
                                },
                                a));
                };
            }
        }

        public override Action<BasePersonalAccount, Transfer, MoneyOperation, ChargePeriod> FireUndoAction
        {
            get { return (a, t, o, p) => DomainEvents.Raise(new PersonalAccountPreviousWorkUndoEvent(a, t, o, p, this.UndoReason)); }
        }
    }
}