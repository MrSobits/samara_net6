namespace Bars.Gkh.RegOperator.DomainEvent.Events.PersonalAccountDto
{
    using Bars.Gkh.DomainEvent.Infrastructure;
    using Bars.Gkh.RegOperator.Entities;

    /// <summary>
    /// Событие изменения владельца ЛС
    /// </summary>
    public class PersonalAccountOwnerUpdateEvent : IDomainEvent
    {
        /// <summary>
        /// Абонент
        /// </summary>
        public PersonalAccountOwner Owner { get;  }

        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="owner">Абонент</param>
        public PersonalAccountOwnerUpdateEvent(PersonalAccountOwner owner)
        {
            this.Owner = owner;
        }
    }
}