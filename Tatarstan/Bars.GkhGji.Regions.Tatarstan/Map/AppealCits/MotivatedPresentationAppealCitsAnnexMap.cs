namespace Bars.GkhGji.Regions.Tatarstan.Map.ActionIsolated
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Tatarstan.Entities.ActionIsolated;

    /// <summary>
    /// Маппинг для <see cref="MotivatedPresentationAppealCitsAnnex"/>
    /// </summary>
    public class MotivatedPresentationAppealCitsAnnexMap
        : BaseEntityMap<MotivatedPresentationAppealCitsAnnex>
    {
        /// <inheritdoc />
        public MotivatedPresentationAppealCitsAnnexMap() : 
            base("Приложение мотивированного представления по обращению гражданина",
                "GJI_MOTIVATED_PRESENTATION_APPEALCITS_ANNEX")
        {
        }

        /// <inheritdoc />
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
