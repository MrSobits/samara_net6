namespace Bars.Gkh.RegOperator.Map.Import
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.RegOperator.Entities.Import;

    /// <summary>
    /// Маппинг для сущности "Шапка импорта начислений в закрытый период"
    /// </summary>
    public class HeaderOfChargesToClosedPeriodsImportMap : JoinedSubClassMap<HeaderOfChargesToClosedPeriodsImport>
    {
        /// <summary>
        /// .ctor
        /// </summary>
        public HeaderOfChargesToClosedPeriodsImportMap() :
            base("Шапка импорта начислений в закрытый период", "REGOP_HEADER_OF_CHARGES_TO_CLOSED_PERIODS_IMPORT")
        {
        }

        /// <summary>
        /// Задать маппинг
        /// </summary>
        protected override void Map()
        {            
            this.Property(x => x.WithoutSaldo, "Не обновлять сальдо").Column("IS_WITHOUT_SALDO_IN");            
        }
    }
}