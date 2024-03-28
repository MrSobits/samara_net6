namespace Bars.Gkh.Modules.ClaimWork.Entities
{
    using System;
    using B4.DataAccess;
    using B4.Modules.States;
    using Enums;

    /// <summary>
    /// Базовый документ ПиР
    /// </summary>
    public class DocumentClw : BaseEntity, IStatefulEntity
    {
        /// <summary>
        /// Основание ПИР
        /// </summary>
        public virtual BaseClaimWork ClaimWork { get; set; }
        
        /// <summary>
        /// Тип документа ПиР
        /// </summary>
        public virtual ClaimWorkDocumentType DocumentType { get; set; }

        /// <summary>
        /// Дата документа
        /// </summary>
        public virtual DateTime? DocumentDate { get; set; }

        /// <summary>
        /// Номер документа
        /// </summary>
        public virtual string DocumentNumber { get; set; }

        /// <summary>
        /// Номер документа (Целая часть)
        /// </summary>
        public virtual int? DocumentNum { get; set; }

        /// <summary>
        /// Статус
        /// </summary>
        public virtual State State { get; set; }
    }
}