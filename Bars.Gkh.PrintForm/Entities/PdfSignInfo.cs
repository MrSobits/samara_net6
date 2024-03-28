namespace Bars.Gkh.PrintForm.Entities
{
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.FileStorage;

    /// <summary>
    /// Информация о подписании файла Pdf
    /// </summary>
    public class PdfSignInfo : BaseEntity
    {
        /// <summary>
        /// Оригинальный подписываемый файл
        /// <remarks>Допускаются форматы Pdf, Doc, Docx, Xls, Xlsx</remarks>
        /// </summary>
        public virtual FileInfo OriginalFile { get; set; }

        /// <summary>
        /// Pdf файл с штампом
        /// </summary>
        public virtual FileInfo StampedPdf { get; set; }
        
        /// <summary>
        /// Файл для хэширования
        /// </summary>
        public virtual FileInfo ForHasFile { get; set; }

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