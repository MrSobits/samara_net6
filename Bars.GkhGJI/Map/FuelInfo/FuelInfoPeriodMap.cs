namespace Bars.GkhGji.Map.FuelInfo
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;

    /// <summary>
    /// Маппинг для Период сведений о наличии и расходе топлива
    /// </summary>
    public class FuelInfoPeriodMap : BaseEntityMap<FuelInfoPeriod>
    {
        /// <summary>
        /// .ctor
        /// </summary>
        public FuelInfoPeriodMap()
            : base("Bars.GkhGji.Entities.FuelInfo.FuelInfoPeriod", "GJI_FUEL_INFO_PERIOD")
        {
        }

        /// <inheritdoc/>
        protected override void Map()
        {
            this.Reference(x => x.Municipality, "Муниципальное образование").Column("MU_ID");

            this.Property(x => x.Year, "Год").Column("YEAR");
            this.Property(x => x.Month, "Месяц").Column("MONTH");
        }
    }
}