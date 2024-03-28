namespace Bars.GisIntegration.Base.File
{
    using Bars.B4.Modules.FileStorage;

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
        FileUploadResult UploadFile(FileInfo fileInfo, string orgPpaGuid);
    }
}
