namespace Bars.Gkh.RegOperator.Dto
{
    using System;

    /// <summary>
    /// Операция, в рамках которой могут происходить различные движения денег
    /// </summary>
    public class MoneyOperationDto
    {
        /// <summary>
        /// Операция, которая была отменена данной операцией
        /// </summary>
        public virtual long? CanceledOperationId { get; protected set; }
        
        /// <summary>
        /// Гуид операции
        /// </summary>
        public virtual string OperationGuid { get; protected set; }

        /// <summary>
        /// Гуид инициатора
        /// </summary>
        public virtual string OriginatorGuid { get; protected set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual bool IsCancelled { get; protected set; }

        /// <summary>
        /// Сумма перевода
        /// </summary>
        public virtual decimal Amount { get; set; }

        /// <summary>
        /// Причина перевода
        /// </summary>
        public virtual string Reason { get; set; }

        /// <summary>
        /// Документ операции
        /// </summary>
        public virtual long DocumentId { get; set; }


        /// <summary>
        /// Дата операции
        /// </summary>
        public virtual DateTime OperationDate { get; set; }
    }
}