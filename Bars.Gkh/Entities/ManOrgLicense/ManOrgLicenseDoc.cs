namespace Bars.Gkh.Entities
{
    using System;
    using B4.Modules.FileStorage;

    using Enums;

    /// <summary>
    /// Документ Лицензии управляющей организации
    /// </summary>
    public class ManOrgLicenseDoc : BaseImportableEntity
    {
        /// <summary>
        /// Лицензия
        /// </summary>
        public virtual ManOrgLicense ManOrgLicense { get; set; }

        /// <summary>
        /// Основание прекращения деятельности
        ///  </summary>
        public virtual TypeManOrgTypeDocLicense DocType { get; set; }

        /// <summary>
        /// Номер
        /// </summary>
        public virtual string DocNumber { get; set; }

        /// <summary>
        /// Дата 
        /// </summary>
        public virtual DateTime? DocDate { get; set; }

        /// <summary>
        /// Файл 
        /// </summary>
        public virtual FileInfo File { get; set; }
    }
}