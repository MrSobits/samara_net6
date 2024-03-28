namespace Bars.Gkh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities;

    /// <summary>
    /// Маппинг для Период Мониторинга жилищного фонда
    /// </summary>
    public class HousingFundMonitoringPeriodMap : BaseImportableEntityMap<HousingFundMonitoringPeriod>
    {
        /// <summary>
        /// .ctor
        /// </summary>
        public HousingFundMonitoringPeriodMap()
            : base("Bars.Gkh.Map.HousingFundMonitoring.HousingFundMonitoringPeriod", "GKH_HOUSING_FUND_MONITOR_PERIOD")
        {
        }

        /// <inheritdoc />
        protected override void Map()
        {
            this.Reference(x => x.Municipality, "Муниципальное образование").Column("MU_ID").NotNull();

            this.Property(x => x.Year, "Год").Column("YEAR");
        }
    }
}