namespace Bars.Gkh.Overhaul.Nso.Entities
{
    using System;

    using Bars.Gkh.Overhaul.Entities;

    /// <summary>
    /// Решение собственников помещений МКД (Кредитная организация)
    /// </summary>
    public class CreditOrganizationDecision : BasePropertyOwnerDecision
    {
        /// <summary>
        /// Кредитная организация
        /// </summary>
        public virtual CreditOrg CreditOrganization { get; set; }
        
        /// <summary>
        /// Расчетный счет
        /// </summary>
        public virtual string SettlementAccount { get; set; }

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