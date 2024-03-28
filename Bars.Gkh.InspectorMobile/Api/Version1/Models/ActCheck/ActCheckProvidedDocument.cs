namespace Bars.Gkh.InspectorMobile.Api.Version1.Models.ActCheck
{
    using System;

    using Bars.Gkh.BaseApiIntegration.Attributes;
    using Bars.Gkh.InspectorMobile.Api.Version1.Models.Interfaces;
    using Bars.GkhGji.Entities;

    /// <summary>
    /// Модель получения "Предоставленный документ"
    /// </summary>
    public class ActCheckProvidedDocumentGet : BaseActCheckProvidedDocument
    {
        /// <summary>
        /// Уникальный идентификатор записи
        /// </summary>
        public long Id { get; set; }
    }

    /// <summary>
    /// Модель создания "Предоставленный документ"
    /// </summary>
    public class ActCheckProvidedDocumentCreate : BaseActCheckProvidedDocument
    {
    }

    /// <summary>
    /// Модель обновления "Предоставленный документ"
    /// </summary>
    public class ActCheckProvidedDocumentUpdate : BaseActCheckProvidedDocument, INestedEntityId
    {
        /// <inheritdoc />
        [OnlyExistsEntity(typeof(ActCheckProvidedDoc))]
        public long? Id { get; set; }
    }

    /// <summary>
    /// Базовая модель "Предоставленный документ"
    /// </summary>
    public class BaseActCheckProvidedDocument
    {
        /// <summary>
        /// Уникальный идентификатор предоставленного документа
        /// </summary>
        [RequiredExistsEntity(typeof(ProvidedDocGji))]
        public long? ProvidedDocumentId { get; set; }

        /// <summary>
        /// Дата предоставления
        /// </summary>
        public DateTime? ProvidedDocumentDate { get; set; }
    }
}