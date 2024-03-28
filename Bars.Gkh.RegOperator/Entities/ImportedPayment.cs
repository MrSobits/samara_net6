namespace Bars.Gkh.RegOperator.Entities
{
    using System;

    using Bars.Gkh.Entities;
    using Bars.Gkh.RegOperator.DomainService.PartialOperationCancellation;

    using Enums;

    /// <summary>
    /// Импортируемая оплата
    /// </summary>
    public class ImportedPayment : BaseImportableEntity, ICancelablePayment
    {
        /// <summary>
        /// Документ, загруженный из банка
        /// </summary>
        public virtual BankDocumentImport BankDocumentImport { get; set; }

        /// <summary>
        /// Лицевой Счет (файл)
        /// </summary>
        public virtual string Account { get; set; }

        /// <summary>
        /// Лицевой Счет (во внешней системе) (файл)
        /// </summary>
        public virtual string ExternalAccountNumber { get; set; }

        /// <summary>
        /// Лицевой Счет (в системе)
        /// </summary>
        public virtual BasePersonalAccount PersonalAccount { get; set; }

        /// <summary>
        /// Р/С получателя (файл)
        /// </summary>
        public virtual string ReceiverNumber { get; set; }

        /// <summary>
        /// Адрес (файл)
        /// </summary>
        public virtual string AddressByImport { get; set; }

        /// <summary>
        /// Абонент (файл)
        /// </summary>
        public virtual string OwnerByImport { get; set; }

        /// <summary>
        /// Р/С получателя (в системе)
        /// </summary>
        public virtual string FactReceiverNumber { get; set; }

        /// <summary>
        /// Тип оплаты
        /// </summary>
        public virtual ImportedPaymentType PaymentType { get; set; }

        /// <summary>
        /// Идентификатор созданной сущности
        /// </summary>
        public virtual long? PaymentId { get; set; }

        /// <summary>
        /// Сумма
        /// </summary>
        public virtual decimal Sum { get; set; }

        /// <summary>
        /// Дата оплаты
        /// </summary>
        public virtual DateTime PaymentDate { get; set; }

        /// <summary>
        /// Дата подтверждения
        /// </summary>
        public virtual DateTime? AcceptDate { get; set; }

        /// <summary>
        /// Статус оплаты
        /// </summary>
        public virtual ImportedPaymentState PaymentState { get; set; }

        /// <summary>
        /// Номер платежа в УС агента
        /// </summary>
        public virtual string PaymentNumberUs { get; set; }

        /// <summary>
        /// Идентификатор внешней транзакции если есть
        /// </summary>
        public virtual string ExternalTransaction { get; set; }

        /// <summary>
        /// Подтверждено
        /// </summary>
        public virtual bool Accepted { get; set; }

        /// <summary>
        /// Статус определения ЛС
        /// </summary>
        public virtual ImportedPaymentPersAccDeterminateState PersonalAccountDeterminationState { get; set; }

        /// <summary>
        /// Статус подтверждения оплат
        /// </summary>
        public virtual ImportedPaymentPaymentConfirmState PaymentConfirmationState { get; set; }

        /// <summary>
        /// ЛС сопоставлен вручную
        /// </summary>
        public virtual bool IsDeterminateManually { get; set; }

        /// <summary>
        /// Причина несоответствия ЛС
        /// </summary>
        public virtual PersonalAccountNotDeterminationStateReason? PersonalAccountNotDeterminationStateReason { get; set; }
    }
}