namespace Bars.Gkh.Entities
{
    using System;
    using Enums;
    
    using B4.Modules.States;

    using Bars.Gkh.Entities;

    /// <summary>
    /// Физ лицо
    /// </summary>
    public class Person : BaseImportableEntity, IStatefulEntity
    {
        /// <summary>
        /// Фамилия
        /// </summary>
        public virtual string Surname { get; set; }

        /// <summary>
        /// Имя
        /// </summary>
        public virtual string Name { get; set; }
        
        /// <summary>
        /// Отчество
        /// </summary>
        public virtual string Patronymic { get; set; }

        /// <summary>
        /// ФИО - автогенерируемое поле после сохранения
        /// </summary>
        public virtual string FullName { get; set; }

        /// <summary>
        /// ИНН
        /// </summary>
        public virtual string Inn { get; set; }

        /// <summary>
        /// Email
        /// </summary>
        public virtual string Email { get; set; }

        /// <summary>
        /// Телефон
        /// </summary>
        public virtual string Phone { get; set; }

        /// <summary>
        /// Адрес регистрации
        /// </summary>
        public virtual string AddressReg { get; set; }

        /// <summary>
        /// Адрес места жительства
        /// </summary>
        public virtual string AddressLive { get; set; }

		/// <summary>
		/// Адрес места рождения
		/// </summary>
		public virtual string AddressBirth { get; set; }

		/// <summary>
		/// Дата рождения
		/// </summary>
		public virtual DateTime? Birthdate { get; set; }

        /// <summary>
        /// Документ удостоверяющий личность 
        /// </summary>
        public virtual TypeIdentityDocument TypeIdentityDocument { get; set; }

        /// <summary>
        /// Серия документа удостоверяющего личность
        /// </summary>
        public virtual string IdSerial  { get; set; }

        /// <summary>
        /// Номер документа удостоверяющего личность
        /// </summary>
        public virtual string IdNumber { get; set; }

        /// <summary>
        /// Кем выдан документ удостоверяющий личность 
        /// </summary>
        public virtual string IdIssuedBy { get; set; }

        /// <summary>
        /// Дата выдачи документа удостоверяющег оличность
        /// </summary>
        public virtual DateTime? IdIssuedDate { get; set; }

        public virtual State State { get; set; }
    }
}
