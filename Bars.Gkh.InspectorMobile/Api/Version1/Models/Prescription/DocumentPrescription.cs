namespace Bars.Gkh.InspectorMobile.Api.Version1.Models.Prescription
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Bars.Gkh.BaseApiIntegration.Attributes;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.InspectorMobile.Api.Version1.Models.Common;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;

    /// <summary>
    /// Документ "Предписание". Модель выборки
    /// </summary>
    public class DocumentPrescriptionGet : BaseDocumentPrescription<PrescriptionClosingDocumentGet, PrescriptionViolationGet, PrescriptionAnnexGet>
    {
        /// <summary>
        /// Уникальный идентификатор родительского документа предписания - Акт проверки 
        /// </summary>
        public long? ParentDocumentId { get; set; }

        /// <summary>
        /// Номер документа "Предписание"
        /// </summary>
        public string DocumentNumber { get; set; }
        
        /// <summary>
        /// Идентификатор дома
        /// </summary>
        public long? AddressId { get; set; }
        
        /// <summary>
        /// Уникальный идентификатор документа "Предписание" в ГИС МЖФ РТ
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Уникальный идентификатор проверки, к которой относится документ
        /// </summary>
        public long InspectionId { get; set; }

        /// <summary>
        /// Место составления
        /// </summary>
        public FiasAddress DocumentPlace { get; set; }

        /// <summary>
        /// Документ основания
        /// </summary>
        public string DocumentBase { get; set; }

        /// <summary>
        /// У предписания присутствуют дочерние документ "Протокол" не в конечном статусе
        /// </summary>
        public bool RelatedDocuments { get; set; }
        
        /// <summary>
        /// Подписанные файлы документа
        /// </summary>
        public IEnumerable<SignedFileInfo> SignedFiles { get; set; }
    }
    
    /// <summary>
    /// Документ "Предписание". Модель создания
    /// </summary>
    public class DocumentPrescriptionCreate: BaseDocumentPrescription<PrescriptionClosingDocumentCreate, PrescriptionViolationCreate, PrescriptionAnnexCreate>
    {
        /// <summary>
        /// Уникальный идентификатор родительского документа предписания - Акт проверки 
        /// </summary>
        [RequiredExistsEntity(typeof(ActCheck))]
        public long? ParentDocumentId { get; set; }

        /// <summary>
        /// Идентификатор дома
        /// </summary>
        [RequiredExistsEntity(typeof(RealityObject))]
        public long? AddressId { get; set; }
    }

    /// <summary>
    /// Документ "Предписание". Модель обновления
    /// </summary>
    public class DocumentPrescriptionUpdate : BaseDocumentPrescription<PrescriptionClosingDocumentUpdate, PrescriptionViolationUpdate, FileInfoUpdate>
    {
    }

    /// <summary>
    /// Документ "Предписание". Базовая модель
    /// </summary>
    public class BaseDocumentPrescription<TClosingDoc, TViolation, TFileInfo>
    {
        /// <summary>
        /// Инспектор
        /// </summary>
        [RequiredExistsEntity(typeof(Inspector))]
        public long? InspectorId { get; set; }

        /// <summary>
        /// Организация
        /// </summary>
        public long? OrganizationId { get; set; }
        
        /// <summary>
        /// Тип исполнителя
        /// </summary>
        [RequiredExistsEntity(typeof(ExecutantDocGji))]
        public long? ExecutantId { get; set; }
        
        /// <summary>
        /// Дата документа "Акт проверки"
        /// </summary>
        [Required]
        public DateTime? DocumentDate { get; set; }

        /// <summary>
        /// Примечание к документу "Предписание"
        /// </summary>
        public string Note { get; set; }
        
        /// <summary>
        /// ФИО физического лица
        /// </summary>
        public string Individual { get; set; }
        
        /// <summary>
        /// Реквизиты физического лица
        /// </summary>
        public string Requisites { get; set; }
        
        /// <summary>
        /// Признак закрытия предписания
        /// </summary>
        [Required]
        public YesNoNotSet? PrescriptionClosed { get; set; }
        
        /// <summary>
        /// Причина закрытия предписания
        /// </summary>
        public PrescriptionCloseReason? Cause { get; set; }
        
        /// <summary>
        /// Примечание по закрытию предписания
        /// </summary>
        public string ClosingNote { get; set; }

        /// <summary>
        /// Информация о выявленных нарушениях
        /// </summary>
        [Required]
        public IEnumerable<TViolation> Violations { get; set; }

        /// <summary>
        /// Документы по закрытию предписания
        /// </summary>
        public IEnumerable<TClosingDoc> PrescriptionClosingDocuments { get; set; }

        /// <summary>
        /// Вложения документа
        /// </summary>
        public IEnumerable<TFileInfo> Files { get; set; }
    }
}