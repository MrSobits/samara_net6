namespace Bars.Gkh.RegOperator.Map.Import
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.RegOperator.Entities.Import;

    /// <summary>
    /// Маппинг для сущности "Предупреждение про существование начислений при импорте начислений в закрытый период"
    /// </summary>
    public class ExistWarningInChargesToClosedPeriodsImportMap : JoinedSubClassMap<ExistWarningInChargesToClosedPeriodsImport>
    {
        /// <summary>
        /// .ctor
        /// </summary>
        public ExistWarningInChargesToClosedPeriodsImportMap() :
            base("Предупреждение про существование начислений при импорте начислений в закрытый период", "REGOP_EXIST_WARNING_IN_CHARGES_TO_CLOSED_PERIODS_IMPORT")
        {
        }

        /// <summary>
        /// Задать маппинг
        /// </summary>
        protected override void Map()
        {
            this.Property(x => x.ChargeDescriptorName, "Описатель").Column("CHARGE_DESCRIPTOR_NAME");
        }
    }
}