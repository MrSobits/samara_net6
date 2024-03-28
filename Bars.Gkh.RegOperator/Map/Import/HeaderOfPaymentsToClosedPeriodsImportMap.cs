namespace Bars.Gkh.RegOperator.Map.Import
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.RegOperator.Entities.Import;

    /// <summary>
    /// Маппинг для сущности "Шапка импорта оплаты в закрытый период"
    /// </summary>
    public class HeaderOfPaymentsToClosedPeriodsImportMap : JoinedSubClassMap<HeaderOfPaymentsToClosedPeriodsImport>
    {
        /// <summary>
        /// .ctor
        /// </summary>
        public HeaderOfPaymentsToClosedPeriodsImportMap() :
            base("Шапка импорта оплаты в закрытый период", "REGOP_HEADER_OF_PAYMENTS_TO_CLOSED_PERIODS_IMPORT")
        {
        }

        /// <summary>
        /// Задать маппинг
        /// </summary>
        protected override void Map()
        {            
            this.Property(x => x.IsUpdateSaldoIn, "Обновлять сальдо").Column("IS_UPDATE_SALDO_IN");            
            this.Property(x => x.ExternalRkcId, "Внешний номер РКЦ").Column("EXTERNAL_RKC_ID");
        }
    }
}
