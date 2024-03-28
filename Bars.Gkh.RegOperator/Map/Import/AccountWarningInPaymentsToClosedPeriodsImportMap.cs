namespace Bars.Gkh.RegOperator.Map.Import
{    
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.RegOperator.Entities.Import;    
    
    /// <summary>
    /// Маппинг для сущности "Предупреждение про ЛС при импорте оплаты в закрытый период"
    /// </summary>
    public class AccountWarningInPaymentsToClosedPeriodsImportMap : JoinedSubClassMap<AccountWarningInPaymentsToClosedPeriodsImport>
    {
        /// <summary>
        /// .ctor
        /// </summary>
        public AccountWarningInPaymentsToClosedPeriodsImportMap(): 
            base("Предупреждение про ЛС при импорте оплаты в закрытый период", "REGOP_ACCOUNT_WARNING_IN_PAYMENTS_TO_CLOSED_PERIODS_IMPORT")
        {
        }

        /// <summary>
        /// Задать маппинг
        /// </summary>
        protected override void Map()
        {
            this.Property(x => x.InnerNumber, "Номер ЛС").Column("INNER_NUMBER");            
            this.Property(x => x.InnerRkcId, "Идентификатор РКЦ").Column("INNER_RKC_ID");            
        }
    }
}
