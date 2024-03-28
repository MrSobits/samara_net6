namespace Bars.GkhGji.Regions.Tatarstan.Map.Dict
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Tatarstan.Entities.Dict;

    public class KnmTypesMap: BaseEntityMap<KnmTypes>
    {
        /// <inheritdoc />
        public KnmTypesMap()
            : base("Bars.GkhGji.Map.Dict.KnmTypes", "GJI_DICT_KNM_TYPES")
        {
        }

        /// <inheritdoc />
        protected override void Map()
        {
            this.Property(x => x.ErvkId, "ErvkId").Length(36).Column("ERVK_ID");
        }
    }
}