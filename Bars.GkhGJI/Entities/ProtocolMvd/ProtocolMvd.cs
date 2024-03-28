namespace Bars.GkhGji.Entities
{
    using System;

    using Bars.GkhGji.Enums;

    using Gkh.Entities;

    /// <summary>
    /// Протокол МВД
    /// </summary>
    public class ProtocolMvd : DocumentGji
    {
        /// <summary>
        /// Муниципальное образование 
        /// </summary>
        public virtual Municipality Municipality { get; set; }

        /// <summary>
        /// Дата поступления в ГЖИ
        /// </summary>
        public virtual DateTime? DateSupply { get; set; }

        /// <summary>
        /// Тип исполнителя документа
        /// </summary>
        public virtual TypeExecutantProtocolMvd TypeExecutant { get; set; }

        /// <summary>
        /// Физическое лицо
        /// </summary>
        public virtual string PhysicalPerson { get; set; }

        /// <summary>
        /// Реквизиты физ. лица
        /// </summary>
        public virtual string PhysicalPersonInfo { get; set; }

        /// <summary>
        /// Не хранимое поле InspectionId потомучто поле Inspection JSONIgnore ичтобы работат ьна клиенте нужен id инспекции
        /// </summary>
        public virtual long InspectionId { get; set; }

        /// <summary>
        /// Орган МВД, оформивший протокол
        /// </summary>
        public virtual OrganMvd OrganMvd { get; set; }

        /// <summary>
        /// Дата парвонарушения
        /// </summary>
        public virtual DateTime? DateOffense { get; set; }

        /// <summary>
        /// Время правонарушения
        /// </summary>
        public virtual string TimeOffense { get; set; }

        /// <summary>
        /// Серия и номер паспорта
        /// </summary>
        public virtual string SerialAndNumber { get; set; }

        /// <summary>
        /// Дата рождения
        /// </summary>
        public virtual DateTime? BirthDate { get; set; }

        /// <summary>
        /// Дата выдачи
        /// </summary>
        public virtual DateTime? IssueDate { get; set; }

        /// <summary>
        /// Место рождения
        /// </summary>
        public virtual string BirthPlace { get; set; }

        /// <summary>
        /// Кем выдан
        /// </summary>
        public virtual string IssuingAuthority { get; set; }

        /// <summary>
        /// Место работы, должность
        /// </summary>
        public virtual string Company { get; set; }
    }
}