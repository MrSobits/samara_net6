namespace Bars.GkhGji.Regions.Tatarstan.Map.TatarstanProtocolGji
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Tatarstan.Entities.TatarstanProtocolGji;

    public class TatarstanProtocolGjiViolationMap : BaseEntityMap<TatarstanProtocolGjiViolation>
    {
        /// <inheritdoc />
        public TatarstanProtocolGjiViolationMap()
            : base(typeof(TatarstanProtocolGjiViolation).FullName, "GJI_TATARSTAN_PROTOCOL_GJI_VIOLATION")
        {
        }

        /// <inheritdoc />
        protected override void Map()
        {
            this.Reference(x => x.TatarstanProtocolGji, "TatarstanProtocolGji")
                .Column("TATARSTAN_PROTOCOL_GJI_ID").Fetch();
            this.Reference(x => x.ViolationGji, "ViolationGji")
                .Column("VIOLATION_GJI_ID").Fetch();
        }
    }
}
