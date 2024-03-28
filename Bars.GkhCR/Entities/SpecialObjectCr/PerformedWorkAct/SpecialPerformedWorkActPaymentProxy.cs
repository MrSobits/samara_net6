namespace Bars.GkhCr.Entities
{
    using System;
    using Enums;

    /// <summary>
    /// 
    /// </summary>
    public class SpecialPerformedWorkActPaymentProxy
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Дата распоряжения
        /// </summary>
        public DateTime DateDisposal { get; set; }

        /// <summary>
        /// Дата оплаты
        /// </summary>
        public DateTime? DatePayment { get; set; }

        /// <summary>
        /// Сумма оплаты
        /// </summary>
        public decimal Paid { get; set; }

        /// <summary>
        /// Вид оплаты
        /// </summary>
        public ActPaymentType TypeActPayment { get; set; }

        /// <summary>
        /// Id PerformedWorkAct
        /// </summary>
        public long PerformedWorkAct { get; set; }
    }
}