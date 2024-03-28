namespace Bars.Gkh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities;

    /// <summary>
    /// Маппинг <see cref="QualificationDocument"/>
    /// </summary>
    public class QualificationDocumentMap : BaseImportableEntityMap<QualificationDocument>
    {
        /// <inheritdoc />
        public QualificationDocumentMap()
            : base("Bars.Gkh.Entities.QualificationDocument", "GKH_CERTIFICATE_DOCUMENT")
        {
        }

        /// <inheritdoc />
        protected override void Map()
        {
            this.Property(x => x.DocumentType, "Тип документа").Column("DOC_TYPE");
            this.Property(x => x.Number, "Номер документа").Column("DOC_NUMBER");
            this.Property(x => x.StatementNumber, "Номер заявления").Column("STMNT_NUMBER");
            this.Property(x => x.IssuedDate, "Дата выдачи").Column("ISSUE_DATE");
            this.Property(x => x.Note, "Комментарий").Column("NOTE").Length(500);

            this.Reference(x => x.Document, "Файл заявления").Column("FILE_ID");
            this.Reference(x => x.QualificationCertificate, "Квалификационный аттестат").Column("CERTIFICATE_ID").NotNull();
        }
    }
}