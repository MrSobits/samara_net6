namespace Bars.Gkh.RegOperator.Wcf.Contracts.PersonalAccount
{
    using System;
    using System.Runtime.Serialization;
    using B4.Utils;

    /// <summary>
    /// Строка импорта платёжки
    /// </summary>
    [DataContract]
    public class PersonalAccountPaymentInfoIn
    {
        /// <summary>
        /// Код платежного агента
        /// </summary>
        [DataMember(Name = "AgentId")]
        [Display("Код платежного агента")]
        public virtual string AgentId { get; set; }

        /// <summary>
        /// Тип абонента
        /// </summary>
        [DataMember(Name = "OwnerType")]
        [Display("Тип абонента")]
        public virtual AccountType OwnerType { get; set; }

        /// <summary>
        /// Номер счета
        /// </summary>
        [DataMember(Name = "AccountNumber")]
        [Display("Номер ЛС")]
        public virtual string AccountNumber { get; set; }

		/// <summary>
		/// Р/С получателя (файл)
		/// </summary>
		[DataMember]
        [Display("Р/С получателя (файл)")]
        public virtual string ReceiverNumber { get; set; }

        /// <summary>
        /// Номер счета
        /// </summary>
        [DataMember(Name = "ExternalAccountNumber")]
        [Display("Номер лс из сторонней системы")]
        public virtual string ExternalAccountNumber { get; set; }

        /// <summary>
        /// Дата оплаты
        /// </summary>
        [DataMember(Name = "PaymentDate")]
        [Display("Дата оплаты")]
        public virtual DateTime PaymentDate { get; set; }

        /// <summary>
        /// Статус счета (false-открыт, true-закрыт)
        /// </summary>
        [DataMember(Name = "AccountIsClosed")]
        public virtual bool AccountIsClosed { get; set; }

        /// <summary>
        /// Оплачено пени
        /// </summary>
        [DataMember(Name = "PenaltyPaid")]
        [Display("Сумма пени")]
        public virtual decimal PenaltyPaid { get; set; }

        /// <summary>
        /// Оплачено за капремонт
        /// </summary>
        [DataMember(Name = "TargetPaid")]
        [Display("Сумма начисления")]
        public virtual decimal TargetPaid { get; set; }

        /// <summary>
        /// Соц. поддержка
        /// </summary>
        [DataMember(Name = "SocialSupport")]
        [Display("Соц. поддержка")]
        public virtual decimal SocialSupport { get; set; }

        /// <summary>
        /// Сумма возврата взносов
        /// </summary>
        [DataMember(Name = "Refund")]
        [Display("Сумма возврата взносов")]
        public virtual decimal Refund { get; set; }

        /// <summary>
        /// Сумма возврата пени
        /// </summary>
        [DataMember(Name = "PenaltyRefund")]
        [Display("Сумма возврата пени")]
        public virtual decimal PenaltyRefund { get; set; }

        /// <summary>
        /// ФИО Абонента
        /// </summary>
        [DataMember(Name = "Fio")]
        [Display("ФИО Абонента")]
        public virtual string Fio { get; set; }

        /// <summary>
        /// Имя
        /// </summary>
        [DataMember(Name = "Name")]
        [Display("Имя")]
        public virtual string Name { get; set; }

        /// <summary>
        /// Фамилия
        /// </summary>
        [DataMember(Name = "Surname")]
        [Display("Фамилия")]
        public virtual string Surname { get; set; }

        /// <summary>
        /// Отчество
        /// </summary>
        [DataMember(Name = "Patronymic")]
        [Display("Отчество")]
        public virtual string Patronymic { get; set; }

        /// <summary>
        /// ИНН контрагента
        /// </summary>
        [DataMember(Name = "Inn")]
        [Display("ИНН контрагента")]
        public virtual string Inn { get; set; }

        /// <summary>
        /// КПП контрагента
        /// </summary>
        [DataMember(Name = "Kpp")]
        [Display("КПП контрагента")]
        public virtual string Kpp { get; set; }

        /// <summary>
        /// Дата документа
        /// </summary>
        [DataMember(Name = "DocumentDate")]
        [Display("Дата приема платежа")]
        public virtual DateTime DocumentDate { get; set; }

        /// <summary>
        /// Номер документа
        /// </summary>
        [DataMember(Name = "DocumentNumber")]
        [Display("Номер документа")]
        public virtual string DocumentNumber { get; set; }

        /// <summary>
        /// Назначение платежа
        /// </summary>
        [DataMember(Name = "Details")]
        [Display("Назначение платежа")]
        public virtual string Details { get; set; }

        /// <summary>
        /// Счет плательщика
        /// </summary>
        [DataMember(Name = "PayerAccount")]
        [Display("Счет плательщика")]
        public virtual string PayerAccount { get; set; }

        /// <summary>
        /// Адрес плательщика
        /// </summary>
        [DataMember(Name = "PayerAddress")]
        [Display("Адрес плательщика")]
        public virtual string PayerAddress { get; set; }

        /// <summary>
        /// Дата, по которой определяется период
        /// </summary>
        [DataMember(Name = "DatePeriod")]
        [Display("Дата, по которой определяется период")]
        public virtual DateTime DatePeriod { get; set; }

        /// <summary>
        /// Период в формате ММГГ
        /// </summary>
        [DataMember(Name = "Period")]
        [Display("Период в формате ММГГ")]
        public virtual string Period { get; set; }

        /// <summary>
        /// Причина
        /// </summary>
        [DataMember(Name = "Reason")]
        [Display("Причина")]
        public virtual string Reason { get; set; }

        /// <summary>
        /// Код расчетного центра
        /// </summary>
        [DataMember(Name = "PaymentCenterCode")]
        [Display("Код расчетного центра")]
        public virtual string PaymentCenterCode { get; set; }

        /// <summary>
        /// Оплаченная сумма (костыль для 43217)
        /// </summary>
        [Display("Оплаченная сумма")]
        public virtual decimal SumPaid { get; set; }

		/// <summary>
		/// Оплаченная пеня
		/// </summary>
		[Display("Оплаченная пеня")]
		public virtual decimal SumPenalty { get; set; }

		/// <summary>
		/// Идентификатор оплаченной квитанции
		/// </summary>
		[Display("Идентификатор оплаченной квитанции")]
        public virtual string ReceiptId { get; set; }

        /// <summary>
        /// Внешний идентификатор транзакции в платежной системе или банке
        /// </summary>
        public virtual string ExternalSystemTransactionId { get; set; }

        public enum AccountType
        {
            Personal = 1,
            Legal = 2,
            Suspense = 3,
            Undefined = 4
        }
    }
}