namespace Bars.Gkh.RegOperator.Entities
{
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.B4.Modules.FileStorage;
    using System;

    /// <summary>
    /// Неподтвержденные оплаты
    /// </summary>
    public class UnconfirmedPayments : BaseImportableEntity
    {
        /// <summary>
        /// Л/С
        /// </summary>
        public virtual BasePersonalAccount PersonalAccount { get; set; }

        /// <summary>
        /// Сумма оплаты
        /// </summary>
        public virtual decimal Sum { get; set; }

        /// <summary>
        /// Guid оплаты
        /// </summary>
        public virtual string Guid { get; set; }

        /// <summary>
        /// Дата оплаты
        /// </summary>
        public virtual DateTime? PaymentDate { get; set; }

        /// <summary>
        /// Описание пакета
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// БИК Банка
        /// </summary>
        public virtual string BankBik { get; set; }

        /// <summary>
        /// Наименование банка
        /// </summary>
        public virtual string BankName { get; set; }

        /// <summary>
        /// Оплата подтверждена
        /// </summary>
        public virtual YesNo IsConfirmed { get; set; }

        /// <summary>
        /// Файл
        /// </summary>
        public virtual FileInfo File { get; set; }
    }
}