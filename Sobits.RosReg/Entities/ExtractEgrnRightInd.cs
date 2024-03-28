namespace Sobits.RosReg.Entities
{
    using System;

    using Bars.B4.DataAccess;

    public class ExtractEgrnRightInd : PersistentObject
    {
        /// <inheritdoc />
        public override long Id { get; set; }

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
        public virtual string Patronymic { get; set; }

        /// <summary>
        /// Дата рождения
        /// </summary>
        public virtual DateTime BirthDate { get; set; }

        /// <summary>
        /// Место рождения
        /// </summary>
        public virtual string BirthPlace { get; set; }

        /// <summary>
        /// СНИЛС
        /// </summary>
        public virtual string Snils { get; set; }

        /// <summary>
        /// Право собственности
        /// </summary>
        public virtual ExtractEgrnRight RightId { get; set; }

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