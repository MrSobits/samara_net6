namespace Bars.GkhGji.Map.FuelInfo
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;

    /// <summary>
    /// Маппинг для Базовый класс сведений наличии и расходе топлива
    /// </summary>
    public class BaseFuelInfoMap : BaseEntityMap<BaseFuelInfo>
    {
        /// <summary>
        /// .ctor
        /// </summary>
        public BaseFuelInfoMap()
            : base("Bars.GkhGji.Entities.FuelInfo.BaseFuelInfo", "GJI_BASE_FUEL_INFO")
        {
        }

        /// <inheritdoc/>
        protected override void Map()
        {
            this.Reference(x => x.FuelInfoPeriod, "Период сведенияй о наличии и расходе топлива").Column("PERIOD_ID").NotNull();

            this.Property(x => x.Mark, "Показатель").Column("MARK");
            this.Property(x => x.RowNumber, "Номер строки").Column("ROW_NUMBER");
        }
    }
}