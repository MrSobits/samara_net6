namespace Bars.GkhGji.Regions.Tatarstan.Map.TatarstanProtocolGji
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Tatarstan.Entities.TatarstanProtocolGji;

    public class TatarstanProtocolGjiRealityObjectMap : BaseEntityMap<TatarstanProtocolGjiRealityObject>
    {
        /// <inheritdoc />
        public TatarstanProtocolGjiRealityObjectMap()
            : base(typeof(TatarstanProtocolGjiRealityObject).FullName, "GJI_TATARSTAN_PROTOCOL_GJI_REALITY_OBJECT")
        {
        }

        /// <inheritdoc />
        protected override void Map()
        {
            this.Reference(x => x.TatarstanProtocolGji, "TatarstanProtocolGji")
                .Column("TATARSTAN_PROTOCOL_GJI_ID").Fetch();
            this.Reference(x => x.RealityObject, "RealityObject")
                .Column("REALITY_OBJECT_ID").Fetch();
        }
    }
}
