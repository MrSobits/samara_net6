namespace Bars.Gkh.Overhaul.Tat.Entities
{
    using System;

    using Bars.B4.DataAccess;

    /// <summary>
    /// Операция по счету
    /// </summary>
    public class SpecialAccountOperation : BaseEntity
    {
        /// <summary>
        /// Специальный счет
        /// </summary>
        public virtual SpecialAccount Account { get; set; }

        /// <summary>
        /// Наименование операции
        /// </summary>
        public virtual AccountOperation Name { get; set; }

        /// <summary>
        /// Дата операции
        /// </summary>
        public virtual DateTime OperationDate { get; set; }

        /// <summary>
        /// Сумма
        /// </summary>
        public virtual Double Sum { get; set; }

        /// <summary>
        /// Получатель
        /// </summary>
        public virtual String Receiver { get; set; }

        /// <summary>
        /// Плательщик
        /// </summary>
        public virtual String Payer { get; set; }
 
        /// <summary>
        /// Назначение
        /// </summary>
        public virtual String Purpose { get; set; }
    }
}
