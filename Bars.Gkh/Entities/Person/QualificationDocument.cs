namespace Bars.Gkh.Entities
{
    using System;

    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.Enums;

    /// <summary>
    /// Документ квалификационного аттестата
    /// </summary>
    public class QualificationDocument : BaseGkhEntity
    {
        /// <summary>
        /// Квалификационный аттестат
        /// </summary>
        public virtual PersonQualificationCertificate QualificationCertificate { get; set; }

        /// <summary>
        /// Тип документа
        /// </summary>
        public virtual QualificationDocumentType DocumentType { get; set; }

        /// <summary>
        /// Номер документа
        /// </summary>
        public virtual string Number { get; set; }

        /// <summary>
        /// Номер заявления
        /// </summary>
        public virtual string StatementNumber { get; set; }

        /// <summary>
        /// Дата выдачи
        /// </summary>
        public virtual DateTime? IssuedDate { get; set; }

        /// <summary>
        /// Файл заявления
        /// </summary>
        public virtual FileInfo Document { get;set; }

        /// <summary>
        /// Комментарий
        /// </summary>
        public virtual string Note { get;set; }
    }
}