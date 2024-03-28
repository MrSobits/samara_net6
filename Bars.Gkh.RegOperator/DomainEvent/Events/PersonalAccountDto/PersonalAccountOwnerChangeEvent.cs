namespace Bars.Gkh.RegOperator.DomainEvent.Events.PersonalAccountDto
{
    using Bars.Gkh.RegOperator.Entities;

    /// <summary>
    /// Событие изменения абонента ЛС
    /// </summary>
    public class PersonalAccountChangeOwnerDtoEvent : BasePersonalAccountDtoEvent
    {
        /// <summary>
        /// Старый абонент
        /// </summary>
        public PersonalAccountOwner OldOwner { get; }

        /// <summary>
        /// Новый абонент
        /// </summary>
        public PersonalAccountOwner NewOwner { get; }

        /// <summary>
        /// .ctor
        /// </summary>
        public PersonalAccountChangeOwnerDtoEvent(BasePersonalAccount account, PersonalAccountOwner newOwner, PersonalAccountOwner oldOwner) : base(account)
        {
            this.OldOwner = oldOwner;
            this.NewOwner = newOwner;
        }
    }
}