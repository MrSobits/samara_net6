namespace Bars.GkhGji.Entities
{
    using System;

    using B4.DataAccess;
    using B4.Modules.States;

    using Gkh.Enums;
    using Enums;

    /// <summary>
    /// Реестр предостережений
    /// </summary>
    public class ViewWarningInspection : PersistentObject
    {
        /// <summary>
        /// Статус
        /// </summary>
        public virtual State State { get; set; }

        /// <summary>
        /// Муниципальный район
        /// </summary>
        public virtual string Municipality { get; set; }

        /// <summary>
        /// Юридическое лицо
        /// </summary>
        public virtual string ContragentName { get; set; }

        /// <summary>
        /// Объект проверки
        /// </summary>
        public virtual PersonInspection? PersonInspection { get; set; }

        /// <summary>
        /// Тип контрагента
        /// </summary>
        public virtual TypeJurPerson? TypeJurPerson { get; set; }

        /// <summary>
        /// Дата документа
        /// </summary>
        public virtual DateTime? InspectionDate { get; set; }

        /// <summary>
        /// Дата проверки
        /// </summary>
        public virtual DateTime? CheckDate { get; set; }

        /// <summary>
        /// Колчиство домов
        /// </summary>
        public virtual int RealityObjectCount { get; set; }

        /// <summary>
        /// Номер
        /// </summary>
        public virtual string DocumentNumber { get; set; }

        /// <summary>
        /// Номер проверки
        /// </summary>
        public virtual string InspectionNumber { get; set; }

        /// <summary>
        /// Учетный номер проверки в едином реестре
        /// </summary>
        public virtual string RegistrationNumber { get; set; }

        /// <summary>
        /// Инспекторы
        /// </summary>
        public virtual string Inspectors { get; set; }

        /// <summary>
        /// Номер и дата обращения гражданина
        /// </summary>
        public virtual string AppealCitsNumberDate { get; set; }
    }
}