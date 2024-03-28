namespace Bars.Gkh.RegOperator.Entities.PersonalAccount
{
    using System;
    using Bars.Gkh.Entities;

    /// <summary>
    /// История принадлежности лс абоненту
    /// </summary>
    public class AccountOwnershipHistory : BaseImportableEntity
    {
        /// <summary>
        /// Лс
        /// </summary>
        public virtual BasePersonalAccount PersonalAccount { get; set; }

        /// <summary>
        /// Абонент
        /// </summary>
        public virtual PersonalAccountOwner AccountOwner { get; set; }

        /// <summary>
        /// Дата установки
        /// </summary>
        public virtual DateTime Date { get; set; }

        /// <summary>
        /// Дата, с которой лс принадлежал абоненту
        /// </summary>
        public virtual DateTime? ActualFrom { get; set; }

        public AccountOwnershipHistory()
        {
            this.Date = DateTime.Today;
        }

        /// <summary>
        /// Конструктор сущности <see cref="AccountOwnershipHistory"/>>
        /// </summary>
        /// <param name="account">Лс</param>
        /// <param name="owner">Владелец</param>
        /// <param name="actualFrom">Дата, с которой лс принадлежал абоненту</param>
        public AccountOwnershipHistory(BasePersonalAccount account, PersonalAccountOwner owner, DateTime actualFrom) : this()
        {
            this.PersonalAccount = account;
            this.AccountOwner = owner;
            this.ActualFrom = actualFrom;
        }
    }
}