namespace Bars.Gkh.Entities
{
    using System;
    using Bars.B4.Modules.FileStorage;

    /// <summary>
    /// Оплата договора
    /// </summary>
    public class BelayPolicyPayment : BaseGkhEntity
    {
        /// <summary>
        /// Страховой полис
        /// </summary>
        public virtual BelayPolicy BelayPolicy { get; set; }

        /// <summary>
        /// Дата оплаты
        /// </summary>
        public virtual DateTime? PaymentDate { get; set; }

        /// <summary>
        /// Наименование
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Номер документа
        /// </summary>
        public virtual string DocumentNumber { get; set; }

        /// <summary>
        /// Сумма, руб.
        /// </summary>
        public virtual decimal? Sum { get; set; }

        /// <summary>
        /// Файл
        /// </summary>
        public virtual FileInfo FileInfo { get; set; }
    }
}