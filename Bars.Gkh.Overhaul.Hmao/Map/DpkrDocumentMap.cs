namespace Bars.Gkh.Overhaul.Hmao.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Overhaul.Hmao.Entities;

    public class DpkrDocumentMap : BaseEntityMap<DpkrDocument>
    {
        /// <inheritdoc />
        public DpkrDocumentMap()
            : base(typeof(DpkrDocument).FullName, "OVRHL_DPKR_DOCUMENTS")
        {
        }

        /// <inheritdoc />
        protected override void Map()
        {
            this.Reference(x => x.DocumentKind, nameof(DpkrDocument.DocumentKind)).Column("DOC_KIND_ID").Fetch().NotNull();
            this.Property(x => x.DocumentName, nameof(DpkrDocument.DocumentName)).Column("DOC_NAME").NotNull();
            this.Reference(x => x.File, nameof(DpkrDocument.File)).Column("FILE_ID").Fetch().NotNull();
            this.Property(x => x.DocumentNumber, nameof(DpkrDocument.DocumentNumber)).Column("DOC_NUMBER");
            this.Property(x => x.DocumentDate, nameof(DpkrDocument.DocumentDate)).Column("DOC_DATE");
            this.Property(x => x.DocumentDepartment, nameof(DpkrDocument.DocumentDepartment)).Column("DOC_DEPARTMENT");
            this.Reference(x => x.State, nameof(DpkrDocument.State)).Column("STATE_ID").NotNull();
        }
    }
}
