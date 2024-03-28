namespace Bars.Gkh.Entities
{
    using System;
    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.Entities;

    using Dicts;

	/// <summary>
    /// Приложение заявки на лицензию
    /// </summary>
    public class ManOrgRequestAnnex : BaseImportableEntity
    {
        /// <summary>
        /// Заявка на лицензию
        /// </summary>
        public virtual ManOrgLicenseRequest LicRequest { get; set; }

        /// <summary>
        /// Наименование
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Номер документа
        /// </summary>
        public virtual string DocumentNumber { get; set; }

        /// <summary>
        /// Дата документа
        /// </summary>
        public virtual DateTime? DocumentDate { get; set; }

		/// <summary>
		/// Тип документа
		/// </summary>
		public virtual AnnexToAppealForLicenseIssuance DocumentType { get; set; }

		/// <summary>
		/// Описание
		/// </summary>
		public virtual string Description { get; set; }

        /// <summary>
        /// Файл
        /// </summary>
        public virtual FileInfo File { get; set; }
    }
}