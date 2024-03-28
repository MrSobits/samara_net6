namespace Bars.GkhGji.Regions.Voronezh.Entities
{
    using System;
    using Gkh.Entities;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.FileStorage;

    /// <summary>
    /// Предоставляемый документ заявки на лицензию
    /// </summary>
    public class LicenseReissuanceProvDoc : BaseEntity
    {
        /// <summary>
        /// Заявка на лицензию
        /// </summary>
        public virtual LicenseReissuance LicenseReissuance { get; set; }

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
        ///Подпись
        /// </summary>
        public virtual string SignedInfo { get; set; }
    }
}