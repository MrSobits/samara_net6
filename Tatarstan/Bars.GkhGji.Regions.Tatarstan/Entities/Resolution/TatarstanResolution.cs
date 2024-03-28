namespace Bars.GkhGji.Regions.Tatarstan.Entities.Resolution
{
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Entities.Dict;
    using Bars.GkhGji.Enums;
    using System;

    public class TatarstanResolution : Resolution
    {
        /// <summary>
        /// Cправочник шаблонов
        /// </summary>
        public virtual GisGmpPatternDict PatternDict { get; set; }

        /// <summary>
        /// Фамилия
        /// </summary>
        public virtual string SurName { get; set; }

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
        public virtual string Address { get; set; }

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
        /// Причина изменения
        /// </summary>
        public virtual string ChangeReason { get; set; }

        /// <summary>
        /// СНИЛС
        /// </summary>
        public virtual string Snils { get; set; }

        /// <summary>
        /// Срок накладываемых санкций
        /// </summary>
        public virtual string SanctionsDuration { get; set; }
    }
}