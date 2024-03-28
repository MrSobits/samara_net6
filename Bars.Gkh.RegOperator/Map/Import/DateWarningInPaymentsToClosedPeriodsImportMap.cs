namespace Bars.Gkh.RegOperator.Map.Import
{    
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.RegOperator.Entities.Import;

    /// <summary>
    /// Маппинг для сущности "Предупреждение про даты при импорте оплаты в закрытый период"
    /// </summary>
    public class DateWarningInPaymentsToClosedPeriodsImportMap : JoinedSubClassMap<DateWarningInPaymentsToClosedPeriodsImport>
    {
        /// <summary>
        /// .ctor
        /// </summary>
        public DateWarningInPaymentsToClosedPeriodsImportMap() :
            base("Предупреждение про даты при импорте оплаты в закрытый период", "REGOP_DATE_WARNING_IN_PAYMENTS_TO_CLOSED_PERIODS_IMPORT")
        {
        }

        /// <summary>
        /// Задать маппинг
        /// </summary>
        protected override void Map()
        {
            this.Property(x => x.PaymentDate, "Дата платежа").Column("PAYMENT_DATE");
        }
    }
}
