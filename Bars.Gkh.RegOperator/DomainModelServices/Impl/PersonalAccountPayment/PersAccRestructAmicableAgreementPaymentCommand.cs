namespace Bars.Gkh.RegOperator.DomainModelServices.Impl.PersonalAccountPayment
{
    using System;
    using B4.Utils;

    using Bars.Gkh.DomainEvent.Infrastructure;
    using Bars.Gkh.Entities;

    using Domain.ValueObjects;
    using DomainEvent.Events.PersonalAccountPayment;
    using DomainEvent.Events.PersonalAccountPayment.UndoPayment;
    using Entities;
    using Entities.ValueObjects;
    using Entities.Wallet;
    using Enums;

    public class PersAccRestructAmicableAgreementPaymentCommand : AbstractPersonalAccountTariffPaymentCommand
    {
        public override PersonalAccountPaymentResult Execute(BasePersonalAccount account, MoneyStream source, AmountDistributionType type = AmountDistributionType.Tariff, AccountDistributionMoneyReserve reserve = null)
        {
            source.Description = "Оплата по мировому соглашению";

            return base.Execute(account, source, type, reserve);
        }

        public override string UndoReason
        {
            get { return "Отмены оплаты по мировому соглашению"; }
        }

        public override Wallet GetWallet(BasePersonalAccount basePersonalAccount)
        {
            return basePersonalAccount.RestructAmicableAgreementWallet;
        }

        public override Action<BasePersonalAccount, MoneyStream> FireExecuteAction
        {
            get
            {
                return (a, s) =>
                {
                    DomainEvents.Raise(
                            new PersAccRestructAmicableAgreementPaymentEvent(
                                new MoneyStream(
                                    a.RestructAmicableAgreementWallet,
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
            get { return (a, t, o, p) => DomainEvents.Raise(new PersAccRestructAmicableAgreementUndoEvent(a, t, o, p, this.UndoReason)); }
        }
    }
}