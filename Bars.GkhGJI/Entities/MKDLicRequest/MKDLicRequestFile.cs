
namespace Bars.GkhGji.Entities
{
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.FileStorage;
    using Enums;
    using System;

    /// <summary>
    /// Прилагаемые документы
    /// </summary>
    public class MKDLicRequestFile : BaseEntity
    {
        /// <summary>
        /// MKDLicRequest
        /// </summary>
        public virtual MKDLicRequest MKDLicRequest { get; set; }

        /// <summary>
        ///Файл
        /// </summary>
        public virtual  FileInfo FileInfo { get; set; }

        /// <summary>
        /// Тип документа
        /// </summary>
        public virtual LicStatementDocType LicStatementDocType { get; set; }

        /// <summary>
        /// Наименование документа
        /// </summary>
        public virtual string DocumentName { get; set; }

        /// <summary>
        /// Комментарий
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// Дата документа
        /// </summary>
        public virtual DateTime? DocDate { get; set; }

        /// <summary>
        /// Подписанный файл
        /// </summary>
        public virtual FileInfo SignedFile { get; set; }

        /// <summary>
        /// Подпись
        /// </summary>
        public virtual FileInfo Signature { get; set; }

        /// <summary>
        /// Сертификат
        /// </summary>
        public virtual FileInfo Certificate { get; set; }
    }
}
