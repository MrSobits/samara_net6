namespace Bars.Gkh.RegOperator.Entities
{
    using System;

    using Bars.Gkh.Entities;

    /// <summary>
    /// Дом обслуживаемой УК расчетно-кассового центра
    /// </summary>
    public class CashPaymentCenterManOrgRo : BaseGkhEntity
    {
        /// <summary>
        /// Обслуживаемая УК расчетно-кассового центра
        /// </summary>
        public virtual CashPaymentCenterManOrg CashPaymentCenterManOrg { get; set; }

        /// <summary>
        /// Дом
        /// </summary>
        public virtual RealityObject RealityObject { get; set; }

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