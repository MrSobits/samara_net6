namespace Bars.Gkh.RegOperator.DomainModelServices.Impl.ChargeCalculators
{
    using System;

    using Bars.Gkh.RegOperator.Enums;

    /// <summary>
    /// Причина перерасчета
    /// </summary>
    public class RecalcReasonProxy
    {
        /// <summary>
        /// Причина перерасчета
        /// </summary>
        public RecalcReason Reason { get; set; }

        /// <summary>
        /// Дата перерасчета
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Значение
        /// </summary>
        public string Value { get; set; }
    }
}