namespace Bars.GisIntegration.Base.Entities.Payment
{
    using System;
    using Bars.GisIntegration.Base.Entities;
    using Bars.GisIntegration.Base.Entities.Bills;

    /// <summary>
    /// Уведомление о выполнении распоряжения
    /// </summary>
    public class NotificationOfOrderExecution : BaseRisEntity
    {
        /// <summary>
        /// Уникальный идентификатор плательщика
        /// </summary>
        public virtual string SupplierId { get; set; }

        /// <summary>
        /// Наименование плательщика
        /// </summary>
        public virtual string SupplierName { get; set; }

        /// <summary>
        /// ИНН получателя платежа
        /// </summary>
        public virtual string RecipientInn { get; set; }

        /// <summary>
        /// КПП получателя платежа
        /// </summary>
        public virtual string RecipientKpp { get; set; }

        /// <summary>
        /// Сведения об исполнителе - ИНН
        /// </summary>
        public virtual string Inn { get; set; }

        /// <summary>
        /// Исполнитель-ИП - Фамилия
        /// </summary>
        public virtual string RecipientEntprSurname { get; set; }

        /// <summary>
        /// Исполнитель-ИП - Имя
        /// </summary>
        public virtual string RecipientEntprFirstName { get; set; }

        /// <summary>
        /// Исполнитель-ИП - Отчество
        /// </summary>
        public virtual string RecipientEntprPatronymic { get; set; }

        /// <summary>
        /// Исполнитель-ЮЛ - КПП
        /// </summary>
        public virtual string RecipientLegalKpp { get; set; }

        /// <summary>
        /// Исполнитель-ЮЛ - Наименование
        /// </summary>
        public virtual string RecipientLegalName { get; set; }

        /// <summary>
        /// Исполнитель-ИП - (ФИО одной строкой)
        /// </summary>
        public virtual string RecipientEntprFio { get; set; }

        /// <summary>
        /// Наименование банка получателя платежа
        /// </summary>
        public virtual string BankName { get; set; }

        /// <summary>
        /// БИК банка получателя платежа
        /// </summary>
        public virtual string RecipientBik { get; set; }

        /// <summary>
        /// Корр. счет банка получателя
        /// </summary>
        public virtual string CorrespondentBankAccount { get; set; }

        /// <summary>
        /// Счет получателя
        /// </summary>
        public virtual string RecipientAccount { get; set; }

        /// <summary>
        /// Наименование получателя платежа
        /// </summary>
        public virtual string RecipientName { get; set; }

        /// <summary>
        /// Уникальный идентификатор распоряжения
        /// </summary>
        public virtual string OrderId { get; set; }

        /// <summary>
        /// Платежный документ
        /// </summary>
        public virtual RisPaymentDocument RisPaymentDocument { get; set; }

        /// <summary>
        /// Номер лицевого счета/Иной идентификтатор плательщика
        /// </summary>
        public virtual string AccountNumber { get; set; }

        /// <summary>
        /// Номер распоряжения
        /// </summary>
        public virtual string OrderNum { get; set; }

        /// <summary>
        /// Дата распоряжения
        /// </summary>
        public virtual DateTime? OrderDate { get; set; }

        /// <summary>
        /// Сумма
        /// </summary>
        public virtual decimal? Amount { get; set; }

        /// <summary>
        /// Назначение платежа
        /// </summary>
        public virtual string PaymentPurpose { get; set; }

        /// <summary>
        /// Произвольный комментарий
        /// </summary>
        public virtual string Comment { get; set; }

        /// <summary>
        /// Идентификатор платежного документа
        /// </summary>
        public virtual string PaymentDocumentID { get; set; }

        /// <summary>
        /// Номер платежного документа
        /// </summary>
        public virtual string PaymentDocumentNumber { get; set; }

        /// <summary>
        /// Год
        /// </summary>
        public virtual short? Year { get; set; }

        /// <summary>
        /// Месяц
        /// </summary>
        public virtual int? Month { get; set; }

        /// <summary>
        /// Единый лицевой счет
        /// </summary>
        public virtual string UnifiedAccountNumber { get; set; }

        /// <summary>
        /// Глобальный уникальный идентификатор дома по ФИАС
        /// </summary>
        public virtual string FiasHouseGuid { get; set; }

        /// <summary>
        /// Номер жилого помещения
        /// </summary>
        public virtual string Apartment { get; set; }

        /// <summary>
        /// Номер комнаты жилого помещения
        /// </summary>
        public virtual string Placement { get; set; }

        /// <summary>
        /// Номер нежилого помещения
        /// </summary>
        public virtual string NonLivingApartment { get; set; }

        /// <summary>
        /// Реквизиты потребителя - Фамилия
        /// </summary>
        public virtual string ConsumerSurname { get; set; }

        /// <summary>
        /// Реквизиты потребителя - Имя
        /// </summary>
        public virtual string ConsumerFirstName { get; set; }

        /// <summary>
        /// Реквизиты потребителя - Отчество
        /// </summary>
        public virtual string ConsumerPatronymic { get; set; }

        /// <summary>
        /// Реквизиты потребителя - ИНН
        /// </summary>
        public virtual string ConsumerInn { get; set; }

        /// <summary>
        /// Идентификатор жилищно-коммунальной услуги
        /// </summary>
        public virtual string ServiceID { get; set; }
    }
}