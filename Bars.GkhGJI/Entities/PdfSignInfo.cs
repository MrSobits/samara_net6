using Bars.B4.DataAccess;
using Bars.B4.Modules.FileStorage;

namespace Bars.GkhGji.Entities
{
    /// <summary>
    /// Информация о подписании файла Pdf
    /// </summary>
    public class PdfSignInfo : BaseEntity
    {
        /// <summary>
        /// Оригинальный подписываемый Pdf файл
        /// </summary>
        public virtual FileInfo OriginalPdf { get; set; }

        /// <summary>
        /// Pdf файл с штампом
        /// </summary>
        public virtual FileInfo StampedPdf { get; set; }

        /// <summary>
        /// Файл для хэширования
        /// </summary>
        public virtual FileInfo ForHashFile { get; set; }

        /// <summary>
        /// Подписанный Pdf файл
        /// </summary>
        public virtual FileInfo SignedPdf { get; set; }

        /// <summary>
        /// Контекст подписания (Наименования контейнера подписи)
        /// </summary>
        public virtual string ContextGuid { get; set; }
    }
}
