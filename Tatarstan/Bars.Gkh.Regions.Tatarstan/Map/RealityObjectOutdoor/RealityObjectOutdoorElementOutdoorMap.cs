namespace Bars.Gkh.Regions.Tatarstan.Map.RealityObjectOutdoor
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Regions.Tatarstan.Entities.RealityObjectOutdoor;

    public class RealityObjectOutdoorElementOutdoorMap : BaseEntityMap<RealityObjectOutdoorElementOutdoor>
    {
        /// <inheritdoc />
        public RealityObjectOutdoorElementOutdoorMap()
            : base(typeof(RealityObjectOutdoorElementOutdoor).FullName, "GKH_RO_OUTDOOR_ELEMENT")
        {
        }

        /// <inheritdoc />
        protected override void Map()
        {
            this.Reference(x => x.Outdoor, "Двор").Column("OUTDOOR_ID").Fetch();
            this.Reference(x => x.Element, "Элемент").Column("ELEMENT_ID").Fetch();
            this.Property(x => x.Condition, "Состояние").Column("CONDITION_ELEMENT").NotNull().DefaultValue(10);
            this.Property(x => x.Volume, "Объем").Column("VOLUME").NotNull();
        }
    }
}

