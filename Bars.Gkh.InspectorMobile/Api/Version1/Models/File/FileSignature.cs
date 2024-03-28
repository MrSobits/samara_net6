namespace Bars.Gkh.InspectorMobile.Api.Version1.Models.File
{
    /// <summary>
    /// Связь файла и подписи
    /// </summary>
    public class FileSignature
    {
        /// <summary>
        /// Уникальный идентификтаор pdf файла в ГИС МЖФ РТ, в который необходимо внедрить подпись
        /// </summary>
        public long FileId { get; set; }

        /// <summary>
        /// Подпись (формат base64)
        /// </summary>
        public string Signature { get; set; }
    }
}