namespace Bars.GkhGji.Regions.Tatarstan.Map
{
    using Bars.B4.Modules.Mapping.Mappers;

    using Bars.GkhGji.Entities;

    public class WarningDocBasisMap : BaseEntityMap<WarningDocBasis>
    {
        
        public WarningDocBasisMap()
            : base("Предостережение ГЖИ - основание для предостережения", "GJI_WARNING_DOC_BASIS")
        {
        }

        /// <inheritdoc />
        protected override void Map()
        {
            this.Reference(x => x.WarningDoc, "Предостережение ГЖИ").Column("DOC_ID");
            this.Reference(x => x.WarningBasis, "Основание для предостережения").Column("BASIS_ID");
        }
    }
}
