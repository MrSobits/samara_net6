namespace Sobits.GisGkh.File
{
    /// <summary>
    /// Интерфейс для скачивания файлов
    /// </summary>
    public interface IFileDownloader
    {
        /// <summary>
        /// Получить файл с рест-сервиса
        /// </summary>
        /// <param name="fileGuid">Гуид файла</param>
        /// <param name="orgPpaGuid">Идентификатор зарегистрированной организации</param>
        /// <returns>Файл</returns>
        FileDownloadResult DownloadFile(string fileGuid, string orgPpaGuid);
    }
}