using Bars.B4.Modules.Mapping.Mappers;
using Bars.GkhGji.Entities;

namespace Bars.GkhGji.Map
{
    public class PdfSignInfoMap : BaseEntityMap<PdfSignInfo>
    {
        /// <inheritdoc />
        public PdfSignInfoMap()
            : base(nameof(PdfSignInfo), "PDF_SIGN_INFO")
        {
        }

        /// <inheritdoc />
        protected override void Map()
        {
            this.Reference(x => x.OriginalPdf, "Оригинальный Pdf файл").Column("ORIGINAL_PDF_ID");
            this.Reference(x => x.StampedPdf, "Pdf файл с штампом").Column("STAMPED_PDF_ID");
            this.Reference(x => x.ForHashFile, "Файл для хэширования").Column("FOR_HASH_FILE_ID");
            this.Reference(x => x.SignedPdf, "Подписанный Pdf файл").Column("SIGNED_PDF_ID");
            this.Property(x => x.ContextGuid, "Контекст подписания (Наименования контейнера подписи)").Column("CONTEXT_GUID");
        }
    }
}
