namespace Bars.Gkh.InspectorMobile.Api.Version1.Models.ActCheck
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Bars.Gkh.BaseApiIntegration.Attributes;
    using Bars.Gkh.Entities;
    using Bars.Gkh.InspectorMobile.Api.Version1.Models.Common;
    using Bars.Gkh.InspectorMobile.Api.Version1.Models.Enums;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Regions.Tatarstan.Entities.Decision;

    /// <summary>
    /// Модель получения документа "Акт проверки"
    /// </summary>
    public class DocumentActCheckGet : BaseDocumentActCheck<ActCheckWitnessGet, ActCheckEventResultGet,
        ActCheckProvidedDocumentGet, ActCheckInspectedPartGet, FileInfoGet>
    {
        /// <summary>
        /// Уникальный идентификатор документа
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Уникальный идентификатор проверки, к которой относится документ
        /// </summary>
        public long InspectionId { get; set; }

        /// <summary>
        /// Уникальный идентификатор родительского документа
        /// </summary>
        public long ParentDocumentId { get; set; }

        /// <summary>
        /// Площадь
        /// </summary>
        public decimal? Square { get; set; }

        /// <summary>
        /// Признак того, что у документа "Акт проверки" есть дочерние документы в НЕ конечном статусе
        /// </summary>
        public virtual bool RelatedDocuments { get; set; }

        /// <summary>
        /// Тип дочернего документа "Акт проверки". (Предписание или Протокол)
        /// </summary>
        public virtual ActCheckChildrenDocumentType[] ChildrenDocumentTypes { get; set; }
        
        /// <summary>
        /// Номер документа
        /// </summary>
        public string DocumentNumber { get; set; }

        /// <summary>
        /// Подписанные файлы документа
        /// </summary>
        public IEnumerable<SignedFileInfo> SignedFiles { get; set; }
    }

    /// <summary>
    /// Модель создания документа "Акт проверки"
    /// </summary>
    public class DocumentActCheckCreate : BaseDocumentActCheck<ActCheckWitnessCreate, ActCheckEventResultCreate,
        ActCheckProvidedDocumentCreate, ActCheckInspectedPartCreate, FileInfoCreate>
    {
        /// <summary>
        /// Уникальный идентификатор родительского документа
        /// </summary>
        [RequiredExistsEntity(typeof(Decision))]
        public virtual long? ParentDocumentId { get; set; }
    }

    /// <summary>
    /// Модель обновления документа "Акт проверки"
    /// </summary>
    public class DocumentActCheckUpdate : BaseDocumentActCheck<ActCheckWitnessUpdate, ActCheckEventResultUpdate,
        ActCheckProvidedDocumentUpdate, ActCheckInspectedPartUpdate, FileInfoUpdate>
    {
    }

    /// <summary>
    /// Базовая модель документа "Акт проверки"
    /// </summary>
    public class BaseDocumentActCheck<TWitness, TEventResult,
        TProvidedDocument, TInspectedPart, TFileInfo>
    {
        /// <summary>
        /// Дата документа
        /// </summary>
        [Required]
        public DateTime? DocumentDate { get; set; }

        /// <summary>
        /// Время составления документа
        /// </summary>
        [Required]
        public TimeSpan? Time { get; set; }

        /// <summary>
        /// Инспекторы
        /// </summary>
        [RequiredExistsEntity(typeof(Inspector))]
        public IEnumerable<long> InspectorIds { get; set; }

        /// <summary>
        /// Место составления
        /// </summary>
        [Required]
        public FiasAddress DocumentPlace { get; set; }

        /// <summary>
        /// Квартира
        /// </summary>
        public string Flat { get; set; }

        /// <summary>
        /// ДЛ, подписавшее акт проверки
        /// </summary>
        [OnlyExistsEntity(typeof(Inspector))]
        public virtual long? ExecutiveId { get; set; }

        /// <summary>
        /// Лица, присутствующие при проверке (или свидетели)
        /// </summary>
        public virtual IEnumerable<TWitness> Witnesses { get; set; }

        /// <summary>
        /// Статус ознакомления с результатами проверки
        /// </summary>
        public virtual AcquaintState? AcquaintedStatus { get; set; }

        /// <summary>
        /// Дата ознакомления
        /// </summary>
        public virtual DateTime? AcquaintedDate { set; get; }

        /// <summary>
        /// ФИО должностного лица
        /// </summary>
        public virtual string OfficialFullName { get; set; }

        /// <summary>
        /// Должность
        /// </summary>
        public virtual string OfficialPosition { get; set; }

        /// <summary>
        /// Результаты проведения проверки
        /// </summary>
        [Required]
        public virtual IEnumerable<TEventResult> EventResults { get; set; }

        /// <summary>
        /// Предоставленные документы
        /// </summary>
        public virtual IEnumerable<TProvidedDocument> ProvidedDocuments { get; set; }

        /// <summary>
        /// Инспектируемые части
        /// </summary>
        public virtual IEnumerable<TInspectedPart> InspectedParts { get; set; }

        /// <summary>
        /// Файлы - вложения документа
        /// </summary>
        public IEnumerable<TFileInfo> Files { get; set; }
    }
}