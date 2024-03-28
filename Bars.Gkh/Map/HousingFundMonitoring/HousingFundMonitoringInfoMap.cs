namespace Bars.Gkh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities;

    /// <summary>
    /// Маппинг для Запись Мониторинга жилищного фонда
    /// </summary>
    public class HousingFundMonitoringInfoMap : BaseImportableEntityMap<HousingFundMonitoringInfo>
    {
        /// <summary>
        /// .ctor
        /// </summary>
        public HousingFundMonitoringInfoMap()
            : base("Bars.Gkh.Map.HousingFundMonitoring.HousingFundMonitoringInfo", "GKH_HOUSING_FUND_MONITOR_INFO")
        {
        }

        /// <inheritdoc />
        protected override void Map()
        {
            this.Reference(x => x.Period, "Период Мониторинга жилищного фонда").Column("PERIOD_ID").NotNull();

            this.Property(x => x.RowNumber, "Номер по порядку").Column("ROW_NUMBER");
            this.Property(x => x.Mark, "Наименование показателя").Column("MARK");
            this.Property(x => x.UnitMeasure, "Единица измерения").Column("UNIT_MEASURE");
            this.Property(x => x.Value, "Значение").Column("VALUE");
            this.Property(x => x.DataProvider, "Поставщик информации").Column("DATA_PROVIDER");
        }
    }
}