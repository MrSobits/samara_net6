namespace Bars.Gkh.RegOperator.Entities.PersonalAccount
{
    using B4.DataAccess;
    using System;

    using Bars.Gkh.Entities;

    /// <summary>
    /// История сформированных документов на оплату (Количество лицевых счетов)
    /// </summary>
    public class PaymentDocumentLog : BaseEntity
    {
        /// <summary>
        /// Родительская запись
        /// </summary>
        public virtual PaymentDocumentLog Parent { get; set; }

        /// <summary>
        /// Период, за который печатаются квитанции
        /// </summary>
        public virtual ChargePeriod Period { get; set; }

        /// <summary>
        /// Уникальный идентификатор группы документов на оплату
        /// </summary>
        public virtual string Uid { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// Время начала
        /// </summary>
        public virtual DateTime StartTime { get; set; }

        /// <summary>
        /// Количество обработанных лицевых счетов
        /// </summary>
        public virtual int Count { get; set; }

        /// <summary>
        /// Общее количество лицевых счетов
        /// </summary>
        public virtual int AllCount { get; set; }
    }
}
