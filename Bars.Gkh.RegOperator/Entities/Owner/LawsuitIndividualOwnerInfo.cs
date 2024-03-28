namespace Bars.Gkh.RegOperator.Entities.Owner
{
    using System;

    /// <summary>
    /// Собственник физ. лицо в исковом заявлении
    /// </summary>
    public class LawsuitIndividualOwnerInfo : LawsuitOwnerInfo
    {
        /// <summary>
        /// Фамилия
        /// </summary>
        public virtual string Surname { get; set; }

        /// <summary>
        /// Имя
        /// </summary>
        public virtual string FirstName { get; set; }

        /// <summary>
        /// Отчество
        /// </summary>
        public virtual string SecondName { get; set; }

        /// <summary>
        /// Дата рождения
        /// </summary>
        public virtual string BirthDate { get; set; }
        
        /// <summary>
        /// Место рождения
        /// </summary>
        public virtual string BirthPlace { get; set; }

        /// <summary>
        /// Место жительства
        /// </summary>
        public virtual string LivePlace { get; set; }
        
        /// <summary>
        /// код документа
        /// </summary>
        public virtual string DocIndCode { get; set; }

        /// <summary>
        /// Название документа
        /// </summary>
        public virtual string DocIndName { get; set; }

        /// <summary>
        /// серия документа
        /// </summary>
        public virtual string DocIndSerial { get; set; }

        /// <summary>
        /// Номер документа
        /// </summary>
        public virtual string DocIndNumber { get; set; }

        /// <summary>
        /// Дата документа
        /// </summary>
        public virtual DateTime? DocIndDate { get; set; }

        /// <summary>
        /// Организация, выдавшая документ
        /// </summary>
        public virtual string DocIndIssue { get; set; }
    }
}