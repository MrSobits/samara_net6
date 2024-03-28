namespace Bars.GkhGji.Map.FuelInfo
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;

    /// <summary>
    /// Маппинг для Расстояние от места добычи топлива до потребителя
    /// </summary>
    public class FuelExtractionDistanceInfoMap : JoinedSubClassMap<FuelExtractionDistanceInfo>
    {
        /// <summary>
        /// .ctor
        /// </summary>
        public FuelExtractionDistanceInfoMap()
            : base("Bars.GkhGji.Entities.FuelInfo.FuelExtractionDistanceInfo", "GJI_FUEL_EXTRACT_DIST_INFO")
        {
        }

        ///<inheritdoc/>
        protected override void Map()
        {
            this.Property(x => x.Distance, "Расстояние").Column("DISTANCE");
        }
    }
}