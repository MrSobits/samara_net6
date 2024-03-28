namespace Bars.GkhGji.Regions.Tatarstan.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;

    /// <summary>
    /// Маппинг полей сущности <see cref="WarningDocAnnex"/>
    /// </summary>
    public class WarningDocAnnexMap : BaseEntityMap<WarningDocAnnex>
    {
        /// <inheritdoc />
        public WarningDocAnnexMap()
            : base("Документы. Предостережение ГЖИ", "GJI_WARNING_DOC_ANNEX")
        {
        }

        /// <inheritdoc />
        protected override void Map()
        {
            this.Property(x => x.Name, "Наименование").Column("NAME");
            this.Property(x => x.DocumentDate, "Дата документа").Column("DOCUMENT_DATE");
            this.Property(x => x.Description, "Описание").Column("DESCRIPTION");
            this.Reference(x => x.WarningDoc, "Предостережение ГЖИ").Column("WARNING_DOC_ID");
            this.Reference(x => x.File, "Файл").Column("FILE_ID");
        }
    }
}