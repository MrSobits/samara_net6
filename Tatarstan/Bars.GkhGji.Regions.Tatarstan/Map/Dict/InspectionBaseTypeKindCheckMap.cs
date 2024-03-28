namespace Bars.GkhGji.Regions.Tatarstan.Map.Dict
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Tatarstan.Entities.Dict;

    public class InspectionBaseTypeKindCheckMap : PersistentObjectMap<InspectionBaseTypeKindCheck>
    {
        /// <inheritdoc />
        public InspectionBaseTypeKindCheckMap()
            : base(nameof(InspectionBaseTypeKindCheck), "GJI_DICT_INSPECTION_BASE_TYPE_KIND_CHECK")
        {
        }

        /// <inheritdoc />
        protected override void Map()
        {
            this.Reference(x => x.InspectionBaseType, "Справочник. Основание проверки").Column("INSPECTION_BASE_TYPE_ID").NotNull();
            this.Reference(x => x.KindCheck, "Вид проверки").Column("KIND_CHECK_ID").NotNull();
        }
    }
}