namespace Bars.Gkh.InspectorMobile.Api.Version1.Models.Prescription
{
    using System;

    using Bars.Gkh.BaseApiIntegration.Attributes;
    using Bars.Gkh.InspectorMobile.Api.Version1.Models.Interfaces;
    using Bars.GkhGji.Entities;

    /// <summary>
    /// Нарушение предписания. Модель выборки
    /// </summary>
    public class PrescriptionViolationGet : BasePrescriptionViolation
    {
        /// <summary>
        /// Уникальный идентификатор записи
        /// </summary>
        public long Id { get; set; }
        
        /// <summary>
        /// Срок устранения выявленного нарушения
        /// </summary>
        public virtual DateTime? TermElimination { get; set; }
    }
    
    /// <summary>
    /// Нарушение предписания. Модель создания
    /// </summary>
    public class PrescriptionViolationCreate : BasePrescriptionViolation
    {
        /// <inheritdoc />
        [RequiredExistsEntity(typeof(ViolationGji))]
        public override long? ViolationId { get; set; }
    }

    /// <summary>
    /// Нарушение предписания. Модель обновления
    /// </summary>
    public class PrescriptionViolationUpdate : PrescriptionViolationCreate, INestedEntityId
    {
        /// <summary>
        /// Уникальный идентификатор записи
        /// </summary>
        public long? Id { get; set; }
    }

    /// <summary>
    /// Нарушение предписания. Базовая модель
    /// </summary>
    public class BasePrescriptionViolation
    {
        /// <summary>
        /// Уникальный идентификатор нарушения
        /// </summary>
        public virtual long? ViolationId { get; set; }
        
        /// <summary>
        /// Мероприятие
        /// </summary>
        public string Event { get; set; }
        
        /// <summary>
        /// Дата факт. исполнения
        /// </summary>
        public DateTime? DateFactRemoval { get; set; }
        
        /// <summary>
        /// Сумма работ
        /// </summary>
        public decimal? Amount { get; set; }
    }
}