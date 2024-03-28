namespace Bars.GkhGji.Regions.BaseChelyabinsk.Entities
{
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.States;
	using Bars.B4.Modules.FileStorage;
	using Bars.Gkh.Entities;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Enums;
    using System;

    /// <summary>
    /// Уведомление о смене способа управления МКД
    /// </summary>
    public class LicenseAction : BaseEntity, IStatefulEntity
    {
		/// <summary>
		/// Тип запроса
		/// </summary>
		public virtual LicenseActionType LicenseActionType { get; set; }

		/// <summary>
		/// Тип Пользователя (ЮЛ, ФЛ, ИП)
		/// </summary>
		public virtual string ApplicantType { get; set; }

		/// <summary>
		/// Идентификатор Заявителя в ЕСИА
		/// </summary>
		public virtual string ApplicantEsiaId { get; set; }

		/// <summary>
		/// Отметка о пользовательском соглашении для передачи сведений
		/// </summary>
		public virtual string ApplicantAgreement { get; set; }

		/// <summary>
		/// СНИЛС Пользователя
		/// </summary>
		public virtual string ApplicantSnils { get; set; }

		/// <summary>
		/// Email Пользователя для отправки уведомлений
		/// </summary>
		public virtual string ApplicantEmail { get; set; }

		/// <summary>
		/// Номер моб телефона Пользователя для отправки уведомлений
		/// </summary>
		public virtual string ApplicantPhone { get; set; }

		/// <summary>
		/// ИНН Пользователя
		/// </summary>
		public virtual string ApplicantInn { get; set; }

		/// <summary>
		/// Код ОКВЭД
		/// </summary>
		public virtual string ApplicantOkved { get; set; }

		/// <summary>
		/// Фамилия
		/// </summary>
		public virtual string ApplicantLastName { get; set; }

		/// <summary>
		/// Имя
		/// </summary>
		public virtual string ApplicantFirstName { get; set; }

		/// <summary>
		/// Отчество
		/// </summary>
		public virtual string ApplicantMiddleName { get; set; }

		/// <summary>
		/// Контрагент
		/// </summary>
		public virtual Contragent Contragent { get; set; }

		/// <summary>
		/// Фамилия
		/// </summary>
		public virtual string SurnameFl { get; set; }

		/// <summary>
		/// Имя
		/// </summary>
		public virtual string NameFl { get; set; }

		/// <summary>
		/// Отчество
		/// </summary>
		public virtual string MiddleNameFl { get; set; }

		/// <summary>
		/// Документ (тип)
		/// </summary>
		public virtual string DocumentType { get; set; }

		/// <summary>
		/// Наименование документа
		/// </summary>
		public virtual string DocumentName { get; set; }

		/// <summary>
		/// Серия
		/// </summary>
		public virtual string DocumentSeries { get; set; }

		/// <summary>
		/// Номер документа
		/// </summary>
		public virtual string DocumentNumber { get; set; }

		/// <summary>
		/// Дата выдачи документа
		/// </summary>
		public virtual DateTime DocumentDate { get; set; }

		/// <summary>
		/// Орган выдачи документа
		/// </summary>
		public virtual string DocumentIssuer { get; set; }

		/// <summary>
		/// Должность
		/// </summary>
		public virtual string Position { get; set; }

		/// <summary>
		/// Дата лицензии
		/// </summary>
		public virtual DateTime LicenseDate { get; set; }

		/// <summary>
		/// Номер лицензии
		/// </summary>
		public virtual string LicenseNumber { get; set; }

		/// <summary>
		/// Файл запроса
		/// </summary>
		public virtual FileInfo FileInfo { get; set; }

		/// <summary>
		/// Статус
		/// </summary>
		public virtual State State { get; set; }

		/// <summary>
		/// MessageId
		/// </summary>
		public virtual string MessageId { get; set; }

		/// <summary>
		/// Номер заявки в рпгу
		/// </summary>
		public virtual string RPGUNumber { get; set; }

		/// <summary>
		/// Получатель ответа
		/// </summary>
		public virtual string ReplyTo { get; set; }

		/// <summary>
		/// Адрес заявителя (почтовый)
		/// </summary>
		public virtual string Address { get; set; }

		/// <summary>
		/// Куда отправить ответ
		/// </summary>
		public virtual string TypeAnswer { get; set; }

		/// <summary>
		/// Причина отклонения
		/// </summary>
		public virtual string DeclineReason { get; set; }

		/// <summary>
		/// Файл
		/// </summary>
		public virtual FileInfo File { get; set; }
	}
}