namespace Bars.GkhGji.Regions.Tatarstan.Map.TatarstanProtocolGji
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Tatarstan.Entities.TatarstanProtocolGji;

    public class TatarstanProtocolGjiAnnexMap : BaseEntityMap<TatarstanProtocolGjiAnnex>
    {
        /// <inheritdoc />
        public TatarstanProtocolGjiAnnexMap()
            : base(typeof(TatarstanProtocolGjiAnnex).FullName, "GJI_TATARSTAN_PROTOCOL_GJI_ANNEX")
        {
        }

        /// <inheritdoc />
        protected override void Map()
        {
            this.Property(x => x.DocumentDate, "DocumentDate").Column("DOCUMENT_DATE");
            this.Property(x => x.Name, "Name").Column("NAME");
            this.Property(x => x.Description, "Description").Column("DESCRIPTION");
            this.Reference(x => x.DocumentGji, "DocumentGji").Column("DOCUMENT_GJI_ID").Fetch();
            this.Reference(x => x.File, "File").Column("FILE_ID").Fetch();
        }
    }
}
