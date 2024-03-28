namespace Bars.GkhGji.Regions.Tatarstan.Map.ActionIsolated
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Tatarstan.Entities.ActionIsolated;

    public class MotivatedPresentationAnnexMap
        : BaseEntityMap<MotivatedPresentationAnnex>
    {
        public MotivatedPresentationAnnexMap() : 
            base("Приложение мотивированного представления", "GJI_MOTIVATED_PRESENTATION_ANNEX")
        {
        }

        protected override void Map()
        {
            this.Reference(x => x.MotivatedPresentation, "Мотивированное представление").Column("MOTIVATED_PRESENTATION_ID");
            this.Property(x => x.DocumentDate, "Дата документа").Column("DOCUMENT_DATE");
            this.Property(x => x.Name, "Наименование").Column("NAME");
            this.Property(x => x.Description, "Описание").Column("DESCRIPTION");
            this.Reference(x => x.File, "Файл").Column("FILE_ID");
        }
    }
}
