namespace Bars.GkhGji.Regions.Tatarstan.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Tatarstan.Entities;

    public class DocumentGjiPdfSignInfoMap : BaseEntityMap<DocumentGjiPdfSignInfo>
    {
        /// <inheritdoc />
        public DocumentGjiPdfSignInfoMap()
            : base(nameof(DocumentGjiPdfSignInfo), "GJI_DOCUMENT_PDF_SIGN_INFO")
        {
        }

        /// <inheritdoc />
        protected override void Map()
        {
            this.Reference(x => x.DocumentGji, "Документ ГЖИ").Column("DOCUMENT_ID").NotNull();
            // TODO : Расскоментировать после реализации GisIntegration
            //this.Reference(x => x.PdfSignInfo, "Информация о подписании Pdf файла").Column("PDF_SIGN_INFO_ID").NotNull();
        }
    }
}