namespace Bars.Gkh.InspectorMobile.Api.Version1.Models.Prescription
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.BaseApiIntegration.Attributes;
    using Bars.Gkh.InspectorMobile.Api.Version1.Models.Interfaces;
    using Bars.GkhGji.Enums;

    /// <summary>
    /// Документ по закрытию предписания. Модель выборки 
    /// </summary>
    public class PrescriptionClosingDocumentGet : BasePrescriptionClosinDocument
    {
        /// <summary>
        /// Уникальный идентификатор записи документа по закрытию предписания
        /// </summary>
        public long Id { get; set; }
    }

    /// <summary>
    /// Документ по закрытию предписания. Модель создания
    /// </summary>
    public class PrescriptionClosingDocumentCreate : BasePrescriptionClosinDocument
    {
        /// <inheritdoc />
        [RequiredExistsEntity(typeof(FileInfo))]
        public override long FileId { get; set; }

        /// <inheritdoc />
        [Required]
        public override DateTime? FileDate { get; set; }

        /// <inheritdoc />
        [Required]
        public override PrescriptionDocType? FileType { get; set; }
    }

    /// <summary>
    /// Документ по закрытию предписания. Модель обновления
    /// </summary>
    public class PrescriptionClosingDocumentUpdate : PrescriptionClosingDocumentCreate, INestedEntityId
    {
        /// <summary>
        /// Уникальный идентификатор записи документа по закрытию предписания
        /// </summary>
        public long? Id { get; set; }
    }

    /// <summary>
    /// Документ по закрытию предписания. Базовая модель
    /// </summary>
    public class BasePrescriptionClosinDocument
    {
        /// <summary>
        /// Уникальный идентификатор файла
        /// </summary>
        public virtual long FileId { get; set; }

        /// <summary>
        /// Наименование файла
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Дата файла
        /// </summary>
        public virtual DateTime? FileDate { get; set; }

        /// <summary>
        /// Тип документа
        /// </summary>
        public virtual PrescriptionDocType? FileType { get; set; }
    }
}