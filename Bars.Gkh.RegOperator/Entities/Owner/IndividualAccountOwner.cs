namespace Bars.Gkh.RegOperator.Entities
{
    using System;
    using Enums;
    using B4.Modules.FIAS;

    using Bars.Gkh.Entities;

    using Gkh.Enums;

    /// <summary>
    /// Абонент - физ.лицо
    /// </summary>
    public class IndividualAccountOwner : PersonalAccountOwner
    {
        /// <summary>
        /// Имя
        /// </summary>
        public virtual string FirstName { get; set; }

        /// <summary>
        /// Фамилия
        /// </summary>
        public virtual string Surname { get; set; }

        /// <summary>
        /// Отчество
        /// </summary>
        public virtual string SecondName { get; set; }

        /// <inheritdoc />
        public override string Name => $"{this.Surname} {this.FirstName} {this.SecondName}";

        /// <summary>
        /// Дата рождения
        /// </summary>
        public virtual DateTime? BirthDate { get; set; }

        /// <summary>
        /// Место рождения
        /// </summary>
        public virtual string BirthPlace { get; set; }

        /// <summary>
        /// Тип документа
        /// </summary>
        public virtual IdentityType IdentityType { get; set; }

        /// <summary>
        /// Серия документа
        /// </summary>
        public virtual string IdentitySerial { get; set; }

        /// <summary>
        /// Номер документа
        /// </summary>
        public virtual string IdentityNumber { get; set; }

        /// <summary>
        /// Адрес за пределами субъекта
        /// </summary>
        public virtual string AddressOutsideSubject { get; set; }

        /// <summary>
        /// Фактический адрес
        /// </summary>
        public virtual FiasAddress FiasFactAddress { get; set; }

        /// <summary>
        /// Жилой дом
        /// </summary>
        public virtual RealityObject RealityObject { get; set; }

        /// <summary>
        /// Электронный адрес
        /// </summary>
        public virtual string Email { get; set; }

        /// <summary>
        /// Пол
        /// </summary>
        public virtual Gender? Gender { get; set; }

        /// <summary>
        /// Дата выдачи документа
        /// </summary>
        public virtual DateTime? DateDocumentIssuance { get; set; }
        /// <summary>
        /// Кем выдан документ
        /// </summary>
        public virtual string DocumentIssuededOrg { get; set; }

        /// <summary>
        /// Адрес прописки
        /// </summary>
        public virtual RealityObject RegistrationAddress { get; set; }

        /// <summary>
        /// Адрес прописки + помещение
        /// </summary>
        public virtual Room RegistrationRoom { get; set; }



    }
}