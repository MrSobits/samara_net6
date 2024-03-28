namespace Bars.Gkh.ImportExport
{
    /// <summary>
    /// Логгер импорта/экспорта данных
    /// </summary>
    public interface IImportExportLogger
    {
        /// <summary>
        /// Предупреждение
        /// </summary>
        /// <param name="message">Сообщение</param>
        void Warn(string message);

        /// <summary>
        /// Сообщение
        /// </summary>
        /// <param name="message">Сообщение</param>
        void Info(string message);

        /// <summary>
        /// Ошибка
        /// </summary>
        /// <param name="message">Сообщение</param>
        void Error(string message);

        /// <summary>
        /// Получение ошибок
        /// </summary>
        /// <returns>Список ошибок</returns>
        string GetErrors();

        /// <summary>
        /// Получение сообщений
        /// </summary>
        /// <returns>Сообщение</returns>
        string GetMessages();

        /// <summary>
        /// Сохранить лог. Очищает внутреннее состояние
        /// </summary>
        void Save();

        /// <summary>
        /// Начинаем операцию
        /// </summary>
        /// <param name="type">Тип операции</param>
        void Begin(ImportExportType type);
    }
}