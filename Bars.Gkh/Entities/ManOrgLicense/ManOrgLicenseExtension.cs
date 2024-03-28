namespace Bars.Gkh.Entities
{
    using System;
    using B4.Modules.FileStorage;
    using Bars.B4.DataAccess;
    using Enums;

    /// <summary>
    /// Документ о продлении Лицензии управляющей организации
    /// </summary>
    public class ManOrgLicenseExtension : BaseEntity
    {
        /// <summary>
        /// Лицензия
        /// </summary>
        public virtual ManOrgLicense ManOrgLicense { get; set; }

        /// <summary>
        /// Основание продления лицензии
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