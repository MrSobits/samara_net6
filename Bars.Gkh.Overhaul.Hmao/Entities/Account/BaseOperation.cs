namespace Bars.Gkh.Overhaul.Hmao.Entities
{
    using System;
    
    using Bars.Gkh.Entities;

    public class BaseOperation : BaseImportableEntity
    {
        /// <summary>
        /// Специальный счет
        /// </summary>
        public virtual AccBankStatement BankStatement { get; set; }

        /// <summary>
        /// Наименование операции
        /// </summary>
        public virtual AccountOperation Operation { get; set; }

        /// <summary>
        /// Дата операции
        /// </summary>
        public virtual DateTime OperationDate { get; set; }

        /// <summary>
        /// Сумма
        /// </summary>
        public virtual decimal Sum { get; set; }

        /// <summary>
        /// Получатель
        /// </summary>
        public virtual string Receiver { get; set; }

        /// <summary>
        /// Плательщик
        /// </summary>
        public virtual string Payer { get; set; }

        /// <summary>
        /// Назначение
        /// </summary>
        public virtual string Purpose { get; set; }


        /// <summary>
        /// Номер П/П
        /// </summary>
        public virtual string Number { get; set; }
    }
}
