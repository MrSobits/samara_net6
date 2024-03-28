namespace Bars.Gkh.RegOperator.DomainEvent.Events.PersonalAccountDto
{
    using Bars.Gkh.DomainEvent.Infrastructure;
    using Bars.Gkh.RegOperator.Entities;

    /// <summary>
    /// Событие изменения ЛС
    /// </summary>
    public class BasePersonalAccountDtoEvent : IDomainEvent
    {
        /// <summary>
        /// Измененный лицевой счет
        /// </summary>
        public BasePersonalAccount Account { get; }

        /// <summary>Initializes a new instance of the <see cref="T:System.Object" /> class.</summary>
        public BasePersonalAccountDtoEvent(BasePersonalAccount account)
        {
            this.Account = account;
        }
    }
}