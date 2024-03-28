namespace Bars.GisIntegration.Smev.DomainService
{
    using Bars.B4.Modules.FileStorage;
    using Bars.GisIntegration.Smev.Entity;

    /// <summary>
    /// Сервис для работы с вложениями СМЭВ
    /// </summary>
    public interface IAttachmentManager
    {
        /// <summary>
        /// Запаковать файлы в вложение
        /// </summary>
        /// <param name="file">Файл</param>
        /// <param name="id">Идентифиактор для наименования архива</param>
        /// <returns>Вложение</returns>
        FileMetadata PackFiles(FileInfo file, string id);
    }
}