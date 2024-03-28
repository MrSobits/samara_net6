namespace Bars.Gkh.RegOperator.Entities
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using B4.Utils.Annotations;

    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.Entities;

    using Dict;

    using DomainModelServices;
    using Enums;

    using Newtonsoft.Json;

    /// <summary>
    /// Абонент
    /// </summary>
    public class PersonalAccountOwner : BaseImportableEntity
    {
        public PersonalAccountOwner()
        {
            this.accounts = new List<BasePersonalAccount>();
        }

        public PersonalAccountOwner(string name) : this()
        {
            this.Name = name;
        }

        #region Persist entries
        /// <summary>
        /// Наименование
        /// </summary>
        public virtual string Name { get; protected set; }

        /// <summary>
        /// Тип абонента
        /// </summary>
        public virtual PersonalAccountOwnerType OwnerType { get; set; }

        /// <summary>
        /// Льготная категория
        /// </summary>
        public virtual PrivilegedCategory PrivilegedCategory { get; set; }

        /// <summary>
        /// ИНН (не хранимое)
        /// </summary>
        public virtual string Inn { get; set; }

        /// <summary>
        /// КПП (не хранимое)
        /// </summary>
        public virtual string Kpp { get; set; }

        /// <summary>
        /// Количество ЛС (всего)
        /// </summary>
        public virtual int TotalAccountsCount { get; set; }
        
        /// <summary>
        /// Документ - основание установки фактического адреса
        /// </summary>
        public virtual FileInfo FactAddrDoc { get; set; }

        /// <summary>
        /// Количество ЛС (открытые)
        /// </summary>
        public virtual int ActiveAccountsCount { get; set; }

        public override string ToString()
        {
            return this.Name;
        }

        /// <summary>
        /// Какой адрес использовать для отправки корреспонденции
        /// </summary>
        public virtual AddressType BillingAddressType { get; set; }

        [JsonIgnore]
        [IgnoreDataMember]
        public virtual IEnumerable<BasePersonalAccount> Accounts => this.accounts;

        private IList<BasePersonalAccount> accounts;
        #endregion Persist entries

        #region Methods

        /// <summary>
        /// Создание лицевого счета с текущим собственником
        /// </summary>
        /// <param name="factory">Фабрика лицевых счетов</param>
        /// <param name="room">Помещение к которому привязан счет</param>
        /// <param name="dateOpen">Дата открытия счета</param>
        /// <param name="areaShare">Доля собственности в помещении</param>
        /// <returns>Лицевой счет</returns>
        public virtual BasePersonalAccount CreateAccount(IPersonalAccountFactory factory, Room room, DateTime dateOpen, decimal areaShare)
        {
            ArgumentChecker.NotNull(factory, "factory");

            var account = factory.CreateNewAccount(this, room, dateOpen, areaShare);

            this.accounts.Add(account);

            return account;
        }

        /// <summary>
        /// Обновить имя владельца
        /// </summary>
        /// <param name="name">Новое имя</param>
        public virtual bool UpdateOwnerName(string name)
        {
            if (this.Name != name)
            {
                this.Name = name;
                return true;
            }

            return false;
        }

        #endregion Methods
    }
}