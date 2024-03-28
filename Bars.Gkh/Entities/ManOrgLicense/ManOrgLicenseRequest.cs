namespace Bars.Gkh.Entities
{
    using System;

    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Modules.States;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Enums.Licensing;

    /// <summary>
    /// Заявка на Лицензию управляющей организации
    /// </summary>
    public class ManOrgLicenseRequest : BaseImportableEntity, IStatefulEntity
    {
        /// <summary>
        /// Контрагент - Выбирается из УО но сохраняется Контрагент
        /// </summary>
        public virtual Contragent Contragent { get; set; }

        /// <summary>
        /// Дата обращения
        /// </summary>
        public virtual DateTime? DateRequest { get; set; }

        /// <summary>
        /// как обычно для всех сущностей с нумерацией делаю и стркоовый номер и тектовый наслучай если заходят изенить номер на маску
        /// </summary>
        public virtual string RegisterNumber { get; set; }

        /// <summary>
        /// как обычно для всех сущностей с нумерацией делаю и стркоовый номер и тектовый наслучай если заходят изенить номер на маску
        /// </summary>
        public virtual int? RegisterNum { get; set; }

        /// <summary>
        /// подтверждение гос пошлины
        /// </summary>
        public virtual string ConfirmationOfDuty { get; set; }

        /// <summary>
        /// Основание предложения
        /// </summary>
        public virtual string ReasonOffers { get; set; }

        /// <summary>
        /// Файл
        /// </summary>
        public virtual FileInfo File { get; set; }

        /// <summary>
        /// Причина отказа
        /// </summary>
        public virtual string ReasonRefusal { get; set; }

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
		/// Причина отклонения
		/// </summary>
		public virtual string DeclineReason { get; set; }

        /// <summary>
        /// Статус
        /// </summary>
        public virtual State State { get; set; }

        #region для Перми
        /// <summary>
        /// Тип обращения
        /// </summary>
        public virtual LicenseRequestType? Type { get; set; }

        /// <summary>
        /// Примечание
        /// </summary>
        public virtual string Note { get; set; }

        /// <summary>
        /// Лицензия
        /// </summary>
        public virtual ManOrgLicense ManOrgLicense { get; set; }

        /// <summary>
        /// Заявитель
        /// </summary>
        public virtual string Applicant { get; set; }

        /// <summary>
        /// Сумма пошлины
        /// </summary>
        public virtual decimal TaxSum { get; set; }

        /// <summary>
        /// Причина переоформления лицензии
        /// </summary>
        public virtual LicenseRegistrationReason LicenseRegistrationReason { get; set; }

        /// <summary>
        /// Причина отказа
        /// </summary>
        public virtual LicenseRejectReason LicenseRejectReason { get; set; }

        /// <summary>
        /// Дата уведомления о принятии документов к рассмотрению
        /// </summary>
        public virtual DateTime? NoticeAcceptanceDate { get; set; }

        /// <summary>
        /// Дата уведомления об устранении нарушений
        /// </summary>
        public virtual DateTime? NoticeViolationDate { get; set; }

        /// <summary>
        /// Дата рассмотрения документов
        /// </summary>
        public virtual DateTime? ReviewDate { get; set; }

        /// <summary>
        /// Дата уведомления о возврате документов
        /// </summary>
        public virtual DateTime? NoticeReturnDate { get; set; }

        /// <summary>
        /// Дата рассмотрения документов ЛК
        /// </summary>
        public virtual DateTime? ReviewDateLk { get; set; }

        /// <summary>
        /// Дата подготовки мотивированного предложения
        /// </summary>
        public virtual DateTime? PreparationOfferDate { get; set; }

        /// <summary>
        /// Дата отправки результата
        /// </summary>
        public virtual DateTime? SendResultDate { get; set; }

        /// <summary>
        /// Способ отправки
        /// </summary>
        public virtual SendMethod? SendMethod { get; set; }

        /// <summary>
        /// Дата приказа/отказа о выдаче лицензии
        /// </summary>
        public virtual DateTime? OrderDate { get; set; }

        /// <summary>
        /// Номер приказа/отказа о выдаче лицензии
        /// </summary>
        public virtual string OrderNumber { get; set; }

        /// <summary>
        /// Файл приказа/отказа о выдаче лицензии
        /// </summary>
        public virtual FileInfo OrderFile { get; set; }

        /// <summary>
        /// Документ удостоверяющий личность 
        /// </summary>
        public virtual TypeIdentityDocument? TypeIdentityDocument { get; set; }

        /// <summary>
        /// Серия документа удостоверяющего личность
        /// </summary>
        public virtual string IdSerial { get; set; }

        /// <summary>
        /// Номер документа удостоверяющего личность
        /// </summary>
        public virtual string IdNumber { get; set; }

        /// <summary>
        /// Кем выдан документ удостоверяющий личность 
        /// </summary>
        public virtual string IdIssuedBy { get; set; }

        /// <summary>
        /// Дата выдачи документа удостоверяющег оличность
        /// </summary>
        public virtual DateTime? IdIssuedDate { get; set; }
        #endregion
    }
}