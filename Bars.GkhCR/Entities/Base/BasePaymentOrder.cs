namespace Bars.GkhCr.Entities
{
    using System;

    using Bars.Gkh.Entities;
    using Bars.GkhCr.Enums;

    /// <summary>
    /// Базовая сущность платежного поручения
    /// </summary>
    public class BasePaymentOrder : BaseGkhEntity
    {
        /// <summary>
        /// Банковская выписка
        /// </summary>
        public virtual BankStatement BankStatement { get; set; }

        /// <summary>
        /// Тип платежного поручения
        /// </summary>
        public virtual TypePaymentOrder TypePaymentOrder { get; set; }

        /// <summary>
        /// Тип источника финансирования
        /// </summary>
        public virtual TypeFinanceSource TypeFinanceSource { get; set; }

        /// <summary>
        /// Плательщик
        /// </summary>
        public virtual Contragent PayerContragent { get; set; }

        /// <summary>
        /// Получатель
        /// </summary>
        public virtual Contragent ReceiverContragent { get; set; }

        /// <summary>
        /// Назначение платежа
        /// </summary>
        public virtual string PayPurpose { get; set; }

        /// <summary>
        /// Номер заявки 
        /// </summary>
        public virtual string BidNum { get; set; }

        /// <summary>
        /// Номер 
        /// </summary>
        public virtual string DocumentNum { get; set; }

        /// <summary>
        /// Дата заявки 
        /// </summary>
        public virtual DateTime? BidDate { get; set; }

        /// <summary>
        /// Дата 
        /// </summary>
        public virtual DateTime? DocumentDate { get; set; }

        /// <summary>
        /// Сумма по документу 
        /// </summary>
        public virtual decimal? Sum { get; set; }

        /// <summary>
        /// сумма повторных направленных средств
        /// </summary>
        public virtual decimal? RedirectFunds { get; set; }

        /// <summary>
        /// Повторно направленные средства 
        /// </summary>
        public virtual bool RepeatSend { get; set; }

        /// <summary>
        /// Id документа 
        /// </summary>
        public virtual string DocId { get; set; }
    }
}
