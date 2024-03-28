namespace Bars.Gkh.FormatDataExport.ExportableEntities
{
    using System.Collections.Generic;
    using Bars.Gkh.FormatDataExport.ExportableEntities.ExportableFile;

    /// <summary>
    /// Коллекция экспортируемых файлов
    /// </summary>
    public interface IFileEntityCollection
    {
        /// <summary>
        /// Добавить файл в коллекцию
        /// </summary>
        bool AddFile(ExportableFileInfo fileInfo);

        /// <summary>
        /// Количество файлов в коллекции
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Добавить коллекцию файлов
        /// </summary>
        /// <returns>Коллекция добавленных файлов</returns>
        IEnumerable<ExportableFileInfo> AddFileRange(IEnumerable<ExportableFileInfo> fileRange);

        /// <summary>
        /// Получить список экспортируемых файлов
        /// </summary>
        IEnumerable<ExportableFileInfo> GetFiles();

        /// <summary>
        /// Список допустимых расширений
        /// </summary>
        IList<string> AllowExtensionList { get; }

        /// <summary>
        /// Получить файловые потоки экспортируемых файлов
        /// </summary>
        ICollection<ExportFileStream> GetFileStreams();
    }
}