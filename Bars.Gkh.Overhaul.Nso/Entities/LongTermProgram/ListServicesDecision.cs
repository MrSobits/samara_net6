namespace Bars.Gkh.Overhaul.Nso.Entities
{
    using System;

    /// <summary>
    /// Решение собственников помещений МКД (Перечень услуг и(или) работ по Капитальному ремонту)
    /// </summary>
    public class ListServicesDecision : BasePropertyOwnerDecision
    {
        /// <summary>
        /// Дата начала действия
        /// </summary>
        public virtual DateTime? DateStart { get; set; }

        /// <summary>
        /// Дата окончания действия
        /// </summary>
        public virtual DateTime? DateEnd { get; set; }
    }
}