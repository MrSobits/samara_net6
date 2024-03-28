namespace Bars.Gkh.RegOperator.Entities
{
    using System;
    using B4.DataAccess;

    using Bars.Gkh.RegOperator.Entities.PersonalAccount.Operations;

    using Enums;

    /// <summary>
    /// Запись об импорте оплаты в закрытый период
    /// </summary>
    public class RecordPaymentsToClosedPeriodsImport : BaseEntity
    {
        /// <summary>
        /// Базовая сущность операции оплат
        /// </summary>
        public virtual PaymentOperationBase PaymentOperation { get; set; } 

        /// <summary>
        /// Период
        /// </summary>
        public virtual string Period { get; set; }

        /// <summary>
        /// Источник поступления
        /// </summary>
        public virtual TypeTransferSource Source { get; set; }

        /// <summary>
        /// Номер документа/реестра 
        /// </summary>
        public virtual string DocumentNum { get; set; }

        /// <summary>
        /// Дата документа/реестра
        /// </summary>
        public virtual DateTime? DocumentDate { get; set; }

        /// <summary>
        /// Код платежного агента
        /// </summary>
        public virtual string PaymentAgentCode { get; set; }

        /// <summary>
        /// Наименование платежного агента
        /// </summary>
        public virtual string PaymentAgentName { get; set; }

        /// <summary>
        /// Номер платежа в Системе ПА
        /// </summary>
        public virtual string PaymentNumberUs { get; set; }

        /// <summary>
        /// Дата операции
        /// </summary>
        public virtual DateTime OperationDate { get; set; }

        /// <summary>
        /// Гуид
        /// </summary>
        public virtual string TransferGuid { get; set; }
    }
}