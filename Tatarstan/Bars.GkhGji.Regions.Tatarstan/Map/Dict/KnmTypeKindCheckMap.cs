namespace Bars.GkhGji.Regions.Tatarstan.Map.Dict
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Tatarstan.Entities.Dict;

    public class KnmTypeKindCheckMap : BaseEntityMap<KnmTypeKindCheck>
    {
        /// <inheritdoc />
        public KnmTypeKindCheckMap()
            : base("Bars.GkhGji.Map.Dict.KNMTypeKindCheck", "GJI_DICT_KNM_TYPE_KIND_CHECK")
        {
        }

        /// <inheritdoc />
        protected override void Map()
        {
            Reference(x => x.KindCheckGji, "KindCheckGji").Column("KIND_CHECK_ID").Fetch();
            Reference(x => x.KnmTypes, "KnmTypes").Column("KNM_TYPE_ID").Fetch();
        }
    }
}