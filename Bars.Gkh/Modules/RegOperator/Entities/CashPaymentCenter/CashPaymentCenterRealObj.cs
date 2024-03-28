namespace Bars.Gkh.RegOperator.Entities
{
    using System;

    using Bars.Gkh.Entities;

    /// <summary>
    /// Жилой дом расчетно-кассового центра
    /// </summary>
    public class CashPaymentCenterRealObj : BaseGkhEntity
    {
        /// <summary>
        /// Жилой дом
        /// </summary>
        public virtual RealityObject RealityObject { get; set; }

        /// <summary>
        /// Агент доставки
        /// </summary>
        public virtual CashPaymentCenter CashPaymentCenter { get; set; }

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