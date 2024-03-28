namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;

    using Bars.GkhGji.Entities.Dict;

    public class InspectionBasisMap : BaseEntityMap<InspectionBasis>
    {
        
        public InspectionBasisMap()
            : base("Основание создания проверки", "GJI_INSPECTION_BASIS")
        {
        }

        /// <inheritdoc />
        protected override void Map()
        {
            this.Property(x => x.Code, "Код").Column("CODE").NotNull();
            this.Property(x => x.Name, "Наименование").Column("NAME").NotNull();
        }
    }
}
