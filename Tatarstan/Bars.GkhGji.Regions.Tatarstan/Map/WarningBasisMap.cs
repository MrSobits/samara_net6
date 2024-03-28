namespace Bars.GkhGji.Regions.Tatarstan.Map
{
    using Bars.B4.Modules.Mapping.Mappers;

    using Bars.GkhGji.Entities.Dict;

    public class WarningBasisMap : BaseEntityMap<WarningBasis>
    {
        
        public WarningBasisMap() : base("Основание предостережений", "GJI_WARNING_BASIS")
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
