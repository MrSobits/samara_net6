namespace Bars.Gkh.InspectorMobile.Api.Version1.Models.File
{
    /// <summary>
    /// Информация о подписываемом документе
    /// </summary>
    public class SigningFileInfo
    {
        /// <summary>
        /// Уникальный идентификтаор pdf файла в ГИС МЖФ РТ, в который необходимо внедрить подпись
        /// </summary>
        public long FileId { get; set; }
        
        /// <summary>
        /// Уникальный идентификатор документа ГЖИ, по которому подписывается печатная форма
        /// </summary>
        public long DocumentId { get; set; }
        
        /// <summary>
        /// Информация о сертификате
        /// </summary>
        public CertificateInfo CertificateInfo { get; set; }
    }
}