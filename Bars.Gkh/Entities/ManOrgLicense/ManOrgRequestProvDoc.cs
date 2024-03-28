namespace Bars.Gkh.Entities
{
    using System;
    
    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.Entities;

    /// <summary>
    /// Предоставляемый документ заявки на лицензию
    /// </summary>
    public class ManOrgRequestProvDoc : BaseImportableEntity
    {
        /// <summary>
        /// Заявка на лицензию
        /// </summary>
        public virtual ManOrgLicenseRequest LicRequest { get; set; }

        /// <summary>
        /// Предосталвяемы документ заявки на лицензию
        /// </summary>
        public virtual LicenseProvidedDoc LicProvidedDoc { get; set; }

        /// <summary>
        /// Номер предоставляемого документа
        /// </summary>
        public virtual string Number { get; set; }

        /// <summary>
        /// Дата предоставляемого документа
        /// </summary>
        public virtual DateTime? Date { get; set; }

        /// <summary>
        /// Файл предоставляемого документа
        /// </summary>
        public virtual FileInfo File { get; set; }

        /// <summary>
        ///Файл
        /// </summary>
        public virtual string SignedInfo { get; set; }
    }
}