namespace Bars.GkhGji.Regions.Tatarstan.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Tatarstan.Entities;

    public class EffectivenessAndPerformanceIndexValueMap : BaseEntityMap<EffectivenessAndPerformanceIndexValue>
    {
        /// <inheritdoc />
        public EffectivenessAndPerformanceIndexValueMap()
            : base("Bars.GkhGji.Map.EffectivenessAndPerformanceIndexValue", "GJI_EFFEC_PERF_INDEX_VALUE")
        {
        }

        /// <inheritdoc />
        protected override void Map()
        {
            this.Reference(x => x.EffectivenessAndPerformanceIndex, "EffectivenessAndPerformanceIndex").Column("effec_perf_index_id");
            this.Property(x => x.CalculationStartDate, "CalculationStartDate").Column("calc_start_date").NotNull();
            this.Property(x => x.CalculationEndDate, "CalculationEndDate").Column("calc_end_date").NotNull();
            this.Property(x => x.Value, "Value").Column("value");
            this.Property(x => x.TorId, "TorId").Column("tor_id");
        }
    }
}
