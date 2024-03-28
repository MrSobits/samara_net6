namespace Bars.GkhGji.Entities
{
    using System;

    using Bars.B4.Modules.FileStorage;
    using Bars.GkhGji.Enums;

    /// <summary>
    /// Основание инспекционная проверка ГЖИ
    /// </summary>
    public class BaseInsCheck : InspectionGji
    {
        /// <summary>
        /// План инспекционных проверок
        /// </summary>
        public virtual PlanInsCheckGji Plan { get; set; }

        /// <summary>
        /// Дата 
        /// </summary>
        public virtual DateTime? InsCheckDate { get; set; }

        /// <summary>
        /// Дата начала
        /// </summary>
        public virtual DateTime? DateStart { get; set; }

        /// <summary>
        /// Площадь МКД (кв. м)
        /// </summary>
        public virtual Decimal? Area { get; set; }

        /// <summary>
        /// Срок проверки (Количество дней)
        /// </summary>
        public virtual int? CountDays { get; set; }

        /// <summary>
        /// Причина
        /// </summary>
        public virtual string Reason { get; set; }

        /// <summary>
        /// Факт проверки
        /// </summary>
        public virtual TypeFactInspection TypeFact { get; set; }

        /// <summary>
        /// Тип документа
        /// </summary>
        public virtual TypeDocumentInsCheck TypeDocument { get; set; }

        /// <summary>
        /// Дата документа
        /// </summary>
        public virtual DateTime? DocumentDate { get; set; }

        /// <summary>
        /// Файл
        /// </summary>
        public virtual FileInfo DocFile { get; set; }

        /// <summary>
        /// Номер документа
        /// </summary>
        public virtual string DocumentNumber { get; set; }

        /// <summary>
        /// Не хранимое поле Идентификатор Дома.
        /// </summary>
        public virtual long? RealityObjectId { get; set; }

        /// <summary>
        /// Не хранимое поле Идентификатор Инспектора.
        /// </summary>
        public virtual long? InspectorId { get; set; }
    }
}