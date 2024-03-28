namespace Bars.Gkh.Regions.Tatarstan.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Regions.Tatarstan.Entities.Dicts;

    public class ElementOutdoorMap : BaseEntityMap<ElementOutdoor>
    {
        /// <inheritdoc />
        public ElementOutdoorMap()
            : base(typeof(ElementOutdoor).FullName, "GKH_DICT_ELEMENT_OUTDOOR")
        {
        }

        /// <inheritdoc />
        protected override void Map()
        {
            this.Property(x => x.Code, nameof(ElementOutdoor.Code)).Column("CODE").Length(255);
            this.Property(x => x.Name, nameof(ElementOutdoor.Name)).Column("NAME").Length(255).NotNull();
            this.Property(x => x.ElementGroup, nameof(ElementOutdoor.ElementGroup)).Column("ELEMENT_GROUP");
            this.Reference(x => x.UnitMeasure, nameof(ElementOutdoor.UnitMeasure)).Column("UNIT_MEASURE_ID").Fetch();
        }
    }
}