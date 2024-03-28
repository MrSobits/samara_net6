namespace Bars.GkhGji.Regions.Tatarstan.Entities.Protocol
{
    using System;

    using Bars.B4.Modules.FIAS;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Entities.Dict;
    using Bars.GkhGji.Enums;

    /// <summary>
    /// Протокол для Татарстана (расширяется дополнительными полями)
    /// </summary>
    public class TatProtocol : Protocol
    {
        /// <summary>
        /// Место составления протокола
        /// </summary>
        public virtual FiasAddress DocumentPlace { get; set; }

        /// <summary>
        /// Дата выписки из ЕГРЮЛ
        /// </summary>
        public virtual DateTime? DateWriteOut { get; set; }

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
        public virtual DateTime? BirthDate { get; set; }

        /// <summary>
        /// Место рождения
        /// </summary>
        public virtual string BirthPlace { get; set; }

        /// <summary>
        /// Фактический адрес проживания
        /// </summary>
        public virtual string FactAddress { get; set; }

        /// <summary>
        /// Гражданство
        /// </summary>
        public virtual Citizenship Citizenship { get; set; }

        /// <summary>
        /// Тип гражданства
        /// </summary>
        public virtual CitizenshipType? CitizenshipType { get; set; }

        /// <summary>
        /// Серия и номер паспорта
        /// </summary>
        public virtual string SerialAndNumber { get; set; }

        /// <summary>
        /// Дата выдачи
        /// </summary>
        public virtual DateTime? IssueDate { get; set; }

        /// <summary>
        /// Кем выдан
        /// </summary>
        public virtual string IssuingAuthority { get; set; }

        /// <summary>
        /// Место работы, должность
        /// </summary>
        public virtual string Company { get; set; }

        /// <summary>
        /// СНИЛС
        /// </summary>
        public virtual string Snils { get; set; }
    }
}