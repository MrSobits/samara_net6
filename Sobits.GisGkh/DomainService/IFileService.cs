using Bars.B4.Modules.FileStorage;
using GisGkhLibrary.Enums;
using Sobits.GisGkh.File;

namespace Sobits.GisGkh.DomainService
{
    /// <summary>
    /// Сервис обмена файлами с ГИС ЖКХ
    /// </summary>
    public interface IFileService
    {
        /// <summary>
        /// Загрузить файл в ГИС ЖКХ
        /// </summary>
        /// <param name="repo">Файловое хранилище ГИС ЖКХ</param>
        /// <param name="File">Файл</param>
        /// <param name="OrgPPAGUID">Идентификатор организации</param>
        /// <returns>Результат загрузки файла</returns>
        FileUploadResult UploadFile(GisFileRepository repo, FileInfo File, string OrgPPAGUID);

        /// <summary>
        /// Скачать файл из ГИС ЖКХ
        /// </summary>
        /// <param name="AttachmentGuid">GUID файла</param>
        /// <param name="OrgPPAGUID">Идентификатор организации</param>
        /// <returns>Результат скачивания фала</returns>
        FileDownloadResult DownloadFile(string AttachmentGuid, string OrgPPAGUID);
    }
}
