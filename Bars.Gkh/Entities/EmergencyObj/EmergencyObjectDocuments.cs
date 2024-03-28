namespace Bars.Gkh.Entities
{
	using System;
	using B4.Modules.FileStorage;

	/// <summary>
    /// Документы аварийного дома
    /// </summary>
    public class EmergencyObjectDocuments : BaseGkhEntity
    {
        /// <summary>
        /// Аварийность жилого дома
        /// </summary>
        public virtual EmergencyObject EmergencyObject { get; set; }

		/// <summary>
		/// Дата издания требования
		/// </summary>
		public virtual DateTime RequirementPublicationDate { get; set; }

		/// <summary>
		/// Номер документа
		/// </summary>
		public virtual string RequirementDocumentNumber { get; set; }

		/// <summary>
		/// Файл требования
		/// </summary>
		public virtual FileInfo RequirementFile { get; set; }

		/// <summary>
		/// Дата издания постановления
		/// </summary>
		public virtual DateTime DecreePublicationDate { get; set; }

		/// <summary>
		/// Реквизиты МПА
		/// </summary>
		public virtual string DecreeRequisitesMpa { get; set; }

		/// <summary>
		/// Дата опубликования МПА
		/// </summary>
		public virtual DateTime? DecreeMpaPublicationDate { get; set; }

		/// <summary>
		/// Дата регистрации МПА, изданного в Управлении Росреестра по РТ
		/// </summary>
		public virtual DateTime? DecreeMpaRegistrationDate { get; set; }

		/// <summary>
		/// Дата уведомления Управления Росреестра по РТ об изданном МПА
		/// </summary>
		public virtual DateTime? DecreeMpaNotificationDate { get; set; }

		/// <summary>
		/// Файл постановления
		/// </summary>
		public virtual FileInfo DecreeFile { get; set; }

		/// <summary>
		/// Дата издания соглашения
		/// </summary>
		public virtual DateTime AgreementPublicationDate { get; set; }

		/// <summary>
		/// Файл соглашения
		/// </summary>
		public virtual FileInfo AgreementFile { get; set; }
	}
}
