namespace Bars.Gkh.RegOperator.Entities
{
    using System;
    
    using Bars.Gkh.Entities;
    using Bars.Gkh.Overhaul.Entities;

    /// <summary>
    /// Условия обслуживания кредитными организациями
    /// </summary>
    public class CreditOrgServiceCondition : BaseImportableEntity
    {
        /// <summary>
        /// Кредитная организация
        /// </summary>
        public virtual CreditOrg CreditOrg { get; set; }

        /// <summary>
        /// Размер расчётно-кассового обслуживания
        /// </summary>
        public virtual decimal CashServiceSize { get; set; }

        /// <summary>
        /// Размер дата "с"
        /// </summary>
        public virtual DateTime CashServiceDateFrom { get; set; }

        /// <summary>
        /// Размер дата "по"
        /// </summary>
        public virtual DateTime? CashServiceDateTo { get; set; }

        /// <summary>
        /// Плата за открытие счета
        /// </summary>
        public virtual decimal OpeningAccPay { get; set; }

        /// <summary>
        /// Плата дата "с"
        /// </summary>
        public virtual DateTime OpeningAccDateFrom { get; set; }

        /// <summary>
        /// Плата дата "по"
        /// </summary>
        public virtual DateTime? OpeningAccDateTo { get; set; }
    }
}