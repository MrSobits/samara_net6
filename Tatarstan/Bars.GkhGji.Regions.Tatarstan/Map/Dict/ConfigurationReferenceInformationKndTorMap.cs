namespace Bars.GkhGji.Regions.Tatarstan.Map.Dict
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Tatarstan.Entities.Dict;

    public class ConfigurationReferenceInformationKndTorMap : BaseEntityMap<ConfigurationReferenceInformationKndTor>
    {
        /// <inheritdoc />
        public ConfigurationReferenceInformationKndTorMap()
            : base("Bars.GkhGji.Map.Dict.ConfigurationReferenceInformationKndTor", "GJI_DICT_CONFIG_REF_INFORMATION_KND_TOR")
        {
        }

        /// <inheritdoc />
        protected override void Map()
        {
            this.Property(x => x.TorId, "TorId").Column("TOR_ID");
            this.Property(x => x.Value, "Value").Column("VALUE").NotNull();
            this.Property(x => x.Type, "Type").Column("TYPE").NotNull();
        }
    }
}
