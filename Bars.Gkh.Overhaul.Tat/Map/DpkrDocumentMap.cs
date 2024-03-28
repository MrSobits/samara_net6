namespace Bars.Gkh.Overhaul.Tat.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Overhaul.Tat.Entities;

    public class DpkrDocumentMap: BaseEntityMap<DpkrDocument>
    {
        /// <inheritdoc />
        public DpkrDocumentMap()
            : base(typeof(DpkrDocument).FullName, "OVRHL_TAT_DPKR_DOCUMENTS")
        {
        }

        /// <inheritdoc />
        protected override void Map()
        {
            this.Reference(x => x.DocumentKind, nameof(DpkrDocument.DocumentKind)).Column("DOC_KIND_ID").Fetch().NotNull();
            this.Property(x => x.DocumentName, nameof(DpkrDocument.DocumentName)).Column("DOC_NAME").NotNull();
            this.Reference(x => x.File, nameof(DpkrDocument.File)).Column("FILE_ID").Fetch();
            this.Property(x => x.DocumentNumber, nameof(DpkrDocument.DocumentNumber)).Column("DOC_NUMBER");
            this.Property(x => x.DocumentDate, nameof(DpkrDocument.DocumentDate)).Column("DOC_DATE");
            this.Property(x => x.DocumentDepartment, nameof(DpkrDocument.DocumentDepartment)).Column("DOC_DEPARTMENT");
            this.Property(x => x.PublicationDate, nameof(DpkrDocument.PublicationDate)).Column("PUBLICATION_DATE");
            this.Property(x => x.ObligationBefore2014, nameof(DpkrDocument.ObligationBefore2014)).Column("OBLIGATION_BEFORE_2014");
            this.Property(x => x.ObligationAfter2014, nameof(DpkrDocument.ObligationAfter2014)).Column("OBLIGATION_AFTER_2014");
        }
    }
}