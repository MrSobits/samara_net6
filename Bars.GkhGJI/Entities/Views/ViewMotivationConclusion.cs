namespace Bars.GkhGji.Entities
{
    using System;

    using Bars.B4.DataAccess;
    using Bars.B4.Modules.States;

    /// <summary>
    /// Мотивировочное заключение
    /// </summary>
    public class ViewMotivationConclusion : PersistentObject
    {
        /// <summary>
        /// Статус
        /// </summary>
        public virtual State State { get; set; }

        /// <summary>
        /// Id основания
        /// </summary>
        public virtual long? InspectionId { get; set; }

        /// <summary>
        /// Муниципальный район
        /// </summary>
        public virtual long? MunicipalityId { get; set; }

        /// <summary>
        /// Тип основания
        /// </summary>
        public virtual int? TypeBase { get; set; }

        /// <summary>
        /// Тип документа
        /// </summary>
        public virtual int TypeDocumentGji { get; set; }

        /// <summary>
        /// Муниципальный район
        /// </summary>
        public virtual string MunicipalityName { get; set; }

        /// <summary>
        /// Основание
        /// </summary>
        public virtual string InspectionBasis { get; set; }

        /// <summary>
        /// Юридическое лицо
        /// </summary>
        public virtual string ContragentName { get; set; }

        /// <summary>
        /// ФИО
        /// </summary>
        public virtual string PhysicalPerson { get; set; }

        /// <summary>
        /// Объект проверки
        /// </summary>
        public virtual string PersonInspection { get; set; }

        /// <summary>
        /// Id домов
        /// </summary>
        public virtual string RealityObjectIds{ get; set; }

        /// <summary>
        /// Количество домов
        /// </summary>
        public virtual int? RealityObjectCount { get; set; }

        /// <summary>
        /// Инспекторы
        /// </summary>
        public virtual string Inspectors { get; set; }

        /// <summary>
        /// номер документа
        /// </summary>
        public virtual string DocumentNumber { get; set; }

        /// <summary>
        /// Целая часть номера документа
        /// </summary>
        public virtual int? DocumentNum { get; set; }

        /// <summary>
        /// Дата документа
        /// </summary>
        public virtual DateTime? DocumentDate { get; set; }
    }
}