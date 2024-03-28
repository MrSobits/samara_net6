namespace Bars.GkhGji.Regions.Tatarstan.Map.Dict
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities.Dict;

    public class ControlTypeMap : BaseEntityMap<ControlType>
    {
        /// <inheritdoc />
        public ControlTypeMap()
            : base("Bars.GkhGji.Map.Dict.ControlType", "GJI_DICT_CONTROL_TYPES")
        {
        }

        /// <inheritdoc />
        protected override void Map()
        {
            this.Property(x => x.TorId, "ExternalId").Column("TOR_ID");
            this.Property(x => x.Name, "Name").Length(500).Column("NAME").NotNull();
            this.Property(x => x.Level, "Level").Column("LEVEL");
            this.Property(x => x.ErvkId, "ErvkId").Column("ERVK_ID");
        }
    }
}