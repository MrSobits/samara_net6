namespace Bars.GkhGji.Regions.Tatarstan.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;

    /// <summary>
    /// Маппинг полей сущности <see cref="WarningDocRealObj"/>
    /// </summary>
    public class WarningDocRealObjMap : BaseEntityMap<WarningDocRealObj>
    {
        /// <inheritdoc />
        public WarningDocRealObjMap()
            : base("Дом предостережения ГЖИ", "GJI_WARNING_DOC_ROBJECT")
        {
        }

        /// <inheritdoc />
        protected override void Map()
        {
            this.Reference(x => x.WarningDoc, "Предостережение").Column("WARNING_DOC_ID").NotNull();
            this.Reference(x => x.RealityObject, "Жилой дом").Column("REALITY_OBJECT_ID").NotNull();
        }
    }
}
