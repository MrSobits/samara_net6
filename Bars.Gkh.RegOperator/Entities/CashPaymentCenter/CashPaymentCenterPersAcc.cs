namespace Bars.Gkh.RegOperator.Entities
{
    using System;

    using Bars.Gkh.Entities;

    /// <summary>
    /// Лицевой счет расчетно-кассового центра
    /// </summary>
    public class CashPaymentCenterPersAcc : BaseGkhEntity
    {
        /// <summary>
        /// Лицевой счет
        /// </summary>
        public virtual BasePersonalAccount PersonalAccount { get; set; }

        /// <summary>
        /// Агент доставки
        /// </summary>
        public virtual CashPaymentCenter CashPaymentCenter { get; set; }

        /// <summary>
        /// Дата начала действия договора
        /// </summary>
        public virtual DateTime DateStart { get; set; }

        /// <summary>
        /// Дата окончания действия договора
        /// </summary>
        public virtual DateTime? DateEnd { get; set; }
    }
}