namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;

    public class InspectionGjiDocumentGjiMap : BaseEntityMap<InspectionGjiDocumentGji>
    {
        
        public InspectionGjiDocumentGjiMap() : 
                base("Проверка ГЖИ - Документы ГЖИ основания", "GJI_BASESTAT_DOCUMENT")
        {
        }

        /// <inheritdoc />
        protected override void Map()
        {
            this.Reference(x => x.Inspection, "Основание обращение граждан ГЖИ").Column("INSPECTION_ID").NotNull();
            this.Reference(x => x.Document, "Документ ГЖИ").Column("DOCUMENT_ID").NotNull();
        }
    }
}
