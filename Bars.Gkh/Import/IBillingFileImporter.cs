namespace Bars.Gkh.Import
{
    using System.IO;
    using B4.Modules.Tasks.Common.Service;

    /// <summary>
    /// Базовый импортер из биллинга
    /// </summary>
    public interface IBillingFileImporter
    {
        /// <summary>
        /// Имя используемого файла (без расширения)
        /// </summary>
        string FileName { get; }

        /// <summary>
        /// Очередность, в которой будет производится импорт
        /// </summary>
        int Order { get; }

        /// <summary>
        /// Импорт
        /// </summary>
        /// <param name="fileStream">Поток файла</param>
        /// <param name="archiveName">Название архива</param>
        /// <param name="logger">Логгер</param>
        /// <param name="indicator">Индикатор</param>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <param name="param">Опциональный параметр</param>
        void Import(Stream fileStream, string archiveName, ILogImport logger = null, IProgressIndicator indicator = null, long userId = 0, object param = null);
    }
}
