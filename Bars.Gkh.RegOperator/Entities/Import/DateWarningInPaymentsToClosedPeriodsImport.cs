namespace Bars.Gkh.RegOperator.Entities.Import
{
    using System;

    /// <summary>
    /// Предупреждение про даты при импорте оплаты в закрытый период
    /// </summary>
    public class DateWarningInPaymentsToClosedPeriodsImport : WarningInClosedPeriodsImport
    {
        /// <summary>
        /// Дата платежа
        /// </summary>
        public virtual DateTime? PaymentDate { get; set; }
    }
}
