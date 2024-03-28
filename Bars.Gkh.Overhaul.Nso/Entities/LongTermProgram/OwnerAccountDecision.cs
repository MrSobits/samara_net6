namespace Bars.Gkh.Overhaul.Nso.Entities
{
    using System;

    using Bars.Gkh.Entities;
    using Bars.Gkh.Overhaul.Nso.Enum;

    /// <summary>
    /// Решение собственников помещений МКД (Владельца специального счета)
    /// </summary>
    public class OwnerAccountDecision : BasePropertyOwnerDecision
    {
        /// <summary>
        /// Тип владельца специального счета
        /// </summary>
        public virtual OwnerAccountDecisionType OwnerAccountType { get; set; }
        
        /// <summary>
        /// Владелец спец счета
        /// </summary>
        public virtual Contragent Contragent { get; set; }

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