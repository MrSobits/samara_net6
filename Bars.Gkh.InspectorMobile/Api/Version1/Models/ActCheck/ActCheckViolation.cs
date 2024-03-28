namespace Bars.Gkh.InspectorMobile.Api.Version1.Models.ActCheck
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using Bars.Gkh.BaseApiIntegration.Attributes;
    using Bars.Gkh.InspectorMobile.Api.Version1.Models.Interfaces;
    using Bars.GkhGji.Entities;

    /// <summary>
    /// Модель получения "Информация о выявленных нарушениях"
    /// </summary>
    public class ActCheckViolationGet : BaseActCheckViolation
    {
        /// <summary>
        /// Уникальный идентификатор записи
        /// </summary>
        public long Id { get; set; }
    }

    /// <summary>
    /// Модель создания "Информация о выявленных нарушениях"
    /// </summary>
    public class ActCheckViolationCreate : BaseActCheckViolation
    {
    }

    /// <summary>
    /// Модель обновления "Информация о выявленных нарушениях"
    /// </summary>
    public class ActCheckViolationUpdate : BaseActCheckViolation, INestedEntityId
    {
        /// <inheritdoc />
        [OnlyExistsEntity(typeof(ActCheckViolation))]
        public long? Id { get; set; }
    }

    /// <summary>
    /// Базовая модель "Информация о выявленных нарушениях"
    /// </summary>
    public class BaseActCheckViolation
    {
        /// <summary>
        /// Уникальный идентификатор нарушения
        /// </summary>
        [RequiredExistsEntity(typeof(ViolationGji))]
        public long? ViolationId { get; set; }

        /// <summary>
        /// Срок устранения выявленного нарушения
        /// </summary>
        [Required]
        public DateTime? TermElimination { get; set; }
    }
}