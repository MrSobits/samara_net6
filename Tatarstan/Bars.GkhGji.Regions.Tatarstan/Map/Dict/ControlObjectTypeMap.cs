namespace Bars.GkhGji.Regions.Tatarstan.Map.Dict
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Tatarstan.Entities.Dict;

    public class ControlObjectTypeMap : BaseEntityMap<ControlObjectType>
    {
        /// <inheritdoc />
        public ControlObjectTypeMap()
            : base(nameof(ControlObjectType), "GJI_DICT_CONTROL_OBJECT_TYPE")
        {
        }
        
        /// <inheritdoc />
        protected override void Map()
        {
            Property(x => x.Name, "Наименование").Column("NAME").NotNull();
            Property(x => x.ErvkId, "Идентификатор в ЕРВК").Column("ERVK_ID").NotNull();
        }
    }
}