namespace Bars.GkhGji.Map.FuelInfo
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;

    /// <summary>
    /// Маппинг для Задолженность за ранее потребленные топливно-энергетические ресурсы (ТЭР) по состоянию на конец отчетного периода
    /// </summary>
    public class FuelEnergyDebtInfoMap : JoinedSubClassMap<FuelEnergyDebtInfo>
    {
        /// <summary>
        /// .ctor
        /// </summary>
        public FuelEnergyDebtInfoMap()
            : base("Bars.GkhGji.Entities.FuelInfo.FuelEnergyDebtInfo", "GJI_FUEL_ENERGY_DEBT_INFO")
        {
        }

        /// <inheritdoc/>
        protected override void Map()
        {
            this.Property(x => x.Total, "Всего").Column("TOTAL");
        }
    }
}