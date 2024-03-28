namespace Bars.GkhGji.Regions.Tatarstan.Entities
{
    using Bars.B4.DataAccess;
    // TODO : Расскоментировать после реализации PrintForm
    //using Bars.Gkh.PrintForm.Entities;
    using Bars.GkhGji.Entities;

    /// <summary>
    /// Связь документа с информацией о подписании Pdf файла
    /// </summary>
    public class DocumentGjiPdfSignInfo : BaseEntity
    {
        /// <summary>
        /// Документ ГЖИ
        /// </summary>
        public virtual DocumentGji DocumentGji { get; set; }

        /// <summary>
        /// Информация о подписании Pdf файла
        /// </summary>
        //public virtual PdfSignInfo PdfSignInfo { get; set; }
    }
}