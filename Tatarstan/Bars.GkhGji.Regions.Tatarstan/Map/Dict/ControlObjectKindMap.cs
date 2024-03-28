namespace Bars.GkhGji.Regions.Tatarstan.Map.Dict
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Tatarstan.Entities.Dict;

    public class ControlObjectKindMap: BaseEntityMap<ControlObjectKind>
    {
        /// <inheritdoc />
        public ControlObjectKindMap()
            : base(nameof(ControlObjectKind), "GJI_DICT_CONTROL_OBJECT_KIND")
        {
        }

        /// <inheritdoc />
        protected override void Map()
        {
            Property(x => x.Name, "Наименование").Column("NAME").Length(1000).NotNull();
            Property(x => x.ErvkId, "Идентификатор в ЕРВК").Column("ERVK_ID").Length(36);

            Reference(x => x.ControlType, "Вид контроля").Column("CONTROL_TYPE_ID").NotNull();
            Reference(x => x.ControlObjectType, "Тип объекта контроля").Column("CONTROL_OBJECT_TYPE_ID").NotNull();
        }
    }
}