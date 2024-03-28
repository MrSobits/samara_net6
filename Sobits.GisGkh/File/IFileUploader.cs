using System;
using Bars.B4.Modules.FileStorage;
using GisGkhLibrary.Enums;
using GisGkhLibrary.Utils;

namespace Sobits.GisGkh.File
{
    /// <summary>
    /// Интерфейс загрузчика файлов
    /// </summary>
    public interface IFileUploader
    {
        /// <summary>
        /// Отправить файл на рест-сервис
        /// </summary>
        /// <param name="fileInfo">Файл</param>
        /// <param name="orgPpaGuid">Идентификатор зарегистрированной организации</param>
        /// <returns>Результат загрузки файла</returns>
        FileUploadResult UploadFile(GisFileRepository repository, FileInfo fileInfo, string orgPpaGuid);
    }
}