namespace Bars.Gkh.Gis.Entities.PersonalAccount
{
    using System;
    using B4.DataAccess;

    /// <summary>
    /// Собственник ЛС
    /// </summary>
    public class PersonalAccountOwner : PersistentObject
    {
        /// <summary>
        /// Внутренний идентификатор квартиры (nzp_kvar)
        /// </summary>
        public virtual long ApartmentId { get; set; }

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
        /// Дата рождения
        /// </summary>
        public virtual DateTime? BithDate { get; set; }

        /// <summary>
        /// Идентификатор наименования документа, удостоверяющий личность
        /// </summary>
        public virtual long IdentityDocumentId { get; set; }

        /// <summary>
        /// Наименование документа, удостоверяющий личность
        /// </summary>
        public virtual string IdentityDocument { get; set; }

        /// <summary>
        ///  Серия 
        /// </summary>
        public virtual string IdentityDocumentSeries { get; set; }

        /// <summary>
        ///  Номер 
        /// </summary>
        public virtual string IdentityDocumentNumber { get; set; }

        /// <summary>
        ///  Дата выдачи 
        /// </summary>
        public virtual DateTime? IdentityDocumentIssuedDate { get; set; }

        /// <summary>
        ///  СНИЛС 
        /// </summary>
        public virtual string Snils { get; set; }

        /// <summary>
        ///  Место рождения  
        /// </summary>
        public virtual string BirthPlace { get; set; }
    }
}
