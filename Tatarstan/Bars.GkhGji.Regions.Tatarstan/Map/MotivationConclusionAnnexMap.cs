namespace Bars.GkhGji.Regions.Tatarstan.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;

    /// <summary>
    /// Маппинг полей сущности <see cref="MotivationConclusionAnnex"/>
    /// </summary>
    public class MotivationConclusionAnnexMap : BaseEntityMap<MotivationConclusionAnnex>
    {
        /// <inheritdoc />
        public MotivationConclusionAnnexMap()
            : base("Документы. Мотивировочное заключение", "GJI_MOTIVATION_CONCLUSION_ANNEX")
        {
        }

        /// <inheritdoc />
        protected override void Map()
        {
            this.Property(x => x.Name, "Наименование").Column("NAME");
            this.Property(x => x.DocumentDate, "Дата документа").Column("DOCUMENT_DATE");
            this.Property(x => x.Description, "Описание").Column("DESCRIPTION");
            this.Reference(x => x.MotivationConclusion, "Предостережение ГЖИ").Column("DOC_ID");
            this.Reference(x => x.File, "Файл").Column("FILE_ID");
        }
    }
}