namespace Bars.Gkh.InspectorMobile.Api.Version1.Models.Protocol
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Bars.Gkh.BaseApiIntegration.Attributes;
    using Bars.Gkh.Entities;
    using Bars.Gkh.InspectorMobile.Api.Version1.Models.Common;
    using Bars.GkhGji.Entities;

    /// <summary>
    /// Модель получения документа "Протокол"
    /// </summary>
    public class DocumentProtocolGet : BaseDocumentProtocol<ProtocolViolationGet, FileInfoGet>
    {
        /// <summary>
        /// Уникальный идентификатор документа
        /// </summary>
        public virtual long Id { get; set; }

        /// <summary>
        /// Номер документа
        /// </summary>
        public string DocumentNumber { get; set; }

        /// <summary>
        /// Уникальный идентификатор проверки, к которой относится документ
        /// </summary>
        public virtual long InspectionId { get; set; }

        /// <summary>
        /// Уникальный идентификатор родительского документа
        /// </summary>
        public long ParentDocumentId { get; set; }

        /// <summary>
        /// Документ-основание
        /// </summary>
        public string DocumentBase { get; set; }

        /// <summary>
        /// Уникальный идентификатор дома, который проверялся в рамках проверки
        /// </summary>
        public long? AddressId { get; set; }

        /// <summary>
        /// Подписанные файлы документа
        /// </summary>
        public IEnumerable<SignedFileInfo> SignedFiles { get; set; }
    }

    /// <summary>
    /// Модель создания документа "Протокол"
    /// </summary>
    public class DocumentProtocolCreate : BaseDocumentProtocol<ProtocolViolationCreate, FileInfoCreate>
    {
        /// <summary>
        /// Уникальный идентификатор родительского документа
        /// </summary>
        [Required]
        public long? ParentDocumentId { get; set; }
    }

    /// <summary>
    /// Модель обновления документа "Протокол"
    /// </summary>
    public class DocumentProtocolUpdate : BaseDocumentProtocol<ProtocolViolationUpdate, FileInfoUpdate>
    {
    }

    /// <summary>
    /// Базовая модель документа "Протокол"
    /// </summary>
    public class BaseDocumentProtocol<TViolation, TFileInfo>
    {
        /// <summary>
        /// Дата документа
        /// </summary>
        [Required]
        public DateTime? DocumentDate { get; set; }

        /// <summary>
        /// Время составления протокола
        /// </summary>
        [Required]
        public string TimeCompilation { get; set; }

        /// <summary>
        /// Место составления протокола
        /// </summary>
        [Required]
        public FiasAddress DocumentPlace { get; set; }

        /// <summary>
        /// Список инспекторов
        /// </summary>
        [RequiredExistsEntity(typeof(Inspector))]
        public IEnumerable<long> InspectorIds { get; set; }

        /// <summary>
        /// Признак "Вручено через канцелярию"
        /// </summary>
        [Required]
        public bool? SignOffice { get; set; }

        /// <summary>
        /// Дата вручения уведомления
        /// </summary>
        public DateTime? DateNotification { get; set; }

        /// <summary>
        /// Номер регистрации
        /// </summary>
        public string NumberRegistration { get; set; }

        /// <summary>
        /// Тип исполнителя (Идентификтаор справочника "Типы исполнителей")
        /// </summary>
        [OnlyExistsEntity(typeof(ExecutantDocGji))]
        public long? ExecutantId { get; set; }

        /// <summary>
        /// Уникальный идентификатор контрагента
        /// </summary>
        [OnlyExistsEntity(typeof(Contragent))]
        public long? OrganizationId { get; set; }

        /// <summary>
        /// Физическое лицо
        /// </summary>
        public string Individual { get; set; }

        /// <summary>
        /// Реквизиты
        /// </summary>
        public string Requisites { get; set; }

        /// <summary>
        /// Место рассмотрения дела
        /// </summary>
        public string PlaceReview { get; set; }

        /// <summary>
        /// Информация о выявленных нарушениях
        /// </summary>
        public IEnumerable<TViolation> Violations { get; set; }

        /// <summary>
        /// Файлы - вложения документа
        /// </summary>
        public IEnumerable<TFileInfo> Files { get; set; }
    }
}