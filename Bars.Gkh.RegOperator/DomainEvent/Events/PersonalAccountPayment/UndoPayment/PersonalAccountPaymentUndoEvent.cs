namespace Bars.Gkh.RegOperator.DomainEvent.Events.PersonalAccountPayment.UndoPayment
{
    using B4.Utils.Annotations;

    using Bars.Gkh.DomainEvent.Infrastructure;
    using Bars.Gkh.Entities;

    using Entities;
    using Entities.ValueObjects;

    /// <summary>
    /// Базовый класс для отмен оплат
    /// </summary>
    public abstract class PersonalAccountPaymentUndoEvent : IDomainEvent
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="account"></param>
        /// <param name="transfer"></param>
        /// <param name="operation"></param>
        /// <param name="period"></param>
        /// <param name="reason"></param>
        public PersonalAccountPaymentUndoEvent(BasePersonalAccount account, Transfer transfer, MoneyOperation operation, ChargePeriod period, string reason)
        {
            ArgumentChecker.NotNull(account, "account");
            ArgumentChecker.NotNull(transfer, "transfer");
            ArgumentChecker.NotNull(operation, "operation");
            ArgumentChecker.NotNull(period, "period");

            this.Transfer = transfer;
            this.Operation = operation;
            this.Period = period;
            this.Reason = reason;
            this.Account = account;
        }

        /// <summary>
        /// Лс
        /// </summary>
        public BasePersonalAccount Account { get; private set; }

        /// <summary>
        /// Трансфер отмены, который мы копируем
        /// </summary>
        public Transfer Transfer { get; private set; }

        /// <summary>
        /// Операция, в рамках которой происходит отмена
        /// </summary>
        public MoneyOperation Operation { get; private set; }

        /// <summary>
        /// Период
        /// </summary>
        public ChargePeriod Period { get; private set; }

        /// <summary>
        /// Причина
        /// </summary>
        public string Reason { get; set; }
    }
}