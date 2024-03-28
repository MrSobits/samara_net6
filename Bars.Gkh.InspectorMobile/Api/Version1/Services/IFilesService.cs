namespace Bars.Gkh.InspectorMobile.Api.Version1.Services
{
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web;

    using Bars.Gkh.InspectorMobile.Api.Version1.Models.Enums;
    using Bars.Gkh.InspectorMobile.Api.Version1.Models.File;

    /// <summary>
    /// Интерфейс сервиса для работы с файлами
    /// </summary>
    public interface IFilesService
    {
        /// <summary>
        /// Загрузить файл на сервер
        /// </summary>
        /// <param name="files">Загружаемые файлы</param>
        /// <returns>Идентификатор загруженного файла</returns>
        long Upload(HttpFileCollection files);

        /// <summary>
        /// Скачать файл
        /// </summary>
        /// <param name="fileId">Идентификатор файла</param>
        Task<HttpResponseMessage> GetAsync(long fileId);

        /// <summary>
        /// Сформировать и получить печатную форму документа
        /// </summary>
        /// <param name="documentId">Идентификатор документа</param>
        /// <param name="documentType">Тип документа</param>
        /// <returns>Сформированная печатная форма документа</returns>
        Task<HttpResponseMessage> GetPrintedFormAsync(long documentId, PrintedFormDocumentType documentType);

        /// <summary>
        /// Добавить штамп в pdf файл
        /// </summary>
        Task<HttpResponseMessage> AddSignatureStamp(SigningFileInfo signatureInfo);

        /// <summary>
        /// Внедрить подпись в файл
        /// </summary>
        Task<HttpResponseMessage> EmbedSignature(FileSignature signatureInfo);
    }
}
