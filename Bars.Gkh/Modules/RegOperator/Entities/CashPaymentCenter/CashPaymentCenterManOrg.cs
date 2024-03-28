namespace Bars.Gkh.RegOperator.Entities
{
    using System;

    using Bars.Gkh.Entities;

    /// <summary>
    /// Обслуживаемая УК расчетно-кассового центра
    /// </summary>
    public class CashPaymentCenterManOrg : BaseGkhEntity
    {
        /// <summary>
        /// РКЦ
        /// </summary>
        public virtual CashPaymentCenter CashPaymentCenter { get; set; }

        /// <summary>
        /// УК
        /// </summary>
        public virtual ManagingOrganization ManOrg { get; set; }

        /// <summary>
        /// Номер договора
        /// </summary>
        public virtual string NumberContract { get; set; }

        /// <summary>
        /// Дата договора
        /// </summary>
        public virtual DateTime? DateContract { get; set; }

        /// <summary>
        /// Дата начала действия договора
        /// </summary>
        public virtual DateTime DateStart { get; set; }

        /// <summary>
        /// Дата начала действия договора
        /// </summary>
        public virtual DateTime? DateEnd { get; set; }
    }
}