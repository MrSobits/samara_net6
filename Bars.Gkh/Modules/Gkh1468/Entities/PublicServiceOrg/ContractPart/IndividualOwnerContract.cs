namespace Bars.Gkh.Modules.Gkh1468.Entities.ContractPart
{
    using System;

    using Bars.Gkh.Modules.Gkh1468.Enums;

    /// <summary>
    /// Сторона договора "Физическое лицо"
    /// </summary>
    public class IndividualOwnerContract : BaseContractPart
    {
        /// <summary>
        /// Лицо, являющееся стороной договора
        /// </summary>
        public virtual TypeContactPerson TypeContactPerson { get; set; }

        /// <summary>
        /// Фамилия
        /// </summary>
        public virtual string LastName { get; set; }

        /// <summary>
        /// Имя
        /// </summary>
        public virtual string FirstName { get; set; }

        /// <summary>
        /// Отчество
        /// </summary>
        public virtual string MiddleName { get; set; }

        /// <summary>
        /// Пол
        /// </summary>
        public virtual GenderR Gender { get; set; }

        /// <summary>
        /// Тип документа
        /// </summary>
        public virtual OwnerDocumentType OwnerDocumentType { get; set; }

        /// <summary>
        /// Дата выдачи
        /// </summary>
        public virtual DateTime? IssueDate { get; set; }

        /// <summary>
        /// Серия документа
        /// </summary>
        public virtual string DocumentSeries { get; set; }

        /// <summary>
        /// Номер документа
        /// </summary>
        public virtual string DocumentNumber { get; set; }

        /// <summary>
        /// Место рождения
        /// </summary>
        public virtual string BirthPlace { get; set; }

        /// <summary>
        /// Дата рождения
        /// </summary>
        public virtual DateTime BirthDate { get; set; }
    }
}
