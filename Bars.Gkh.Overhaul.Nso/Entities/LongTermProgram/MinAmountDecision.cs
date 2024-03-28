namespace Bars.Gkh.Overhaul.Nso.Entities
{
    using System;

    /// <summary>
    /// Решение собственников помещений МКД (Установление минимального размера фонда кап.ремонта)
    /// </summary>
    public class MinAmountDecision : BasePropertyOwnerDecision
    {
        /// <summary>
        /// Размер вноса, установленный собственниками (руб.)
        /// </summary>
        public virtual decimal SizeOfPaymentOwners { get; set; }

        /// <summary>
        /// Дата начала действия взноса
        /// </summary>
        public virtual DateTime PaymentDateStart { get; set; }

        /// <summary>
        /// Дата окончания действия взноса
        /// </summary>
        public virtual DateTime? PaymentDateEnd { get; set; }

    }
}