namespace Bars.Gkh.Utils.PerformanceLogging
{
    /// <summary>
    /// Интерфейс фабрики логгера производительности
    /// </summary>
    public interface IPerformanceLoggerFactory
    {
        /// <summary>
        /// Получить сконфигурированный логгер производительности
        /// </summary>
        /// <param name="sessionId">Идентификатор сессии, для агрегирования результатов</param>
        /// <returns>Интерфейс логгирования скорости выполнения</returns>
        IPerformanceLogger GetLogger(string sessionId = null);

        /// <summary>
        /// Получить сконфигурированный коллектор для логов производительности
        /// </summary>
        /// <param name="storageType">Целевое хранилище логов</param>
        /// <returns>Интерфейс логгирования скорости выполнения</returns>
        IPerformanceLogsCollector GetCollector(LoggerStorageType storageType = LoggerStorageType.SystemLog);

    }

    /// <summary>
    /// Тип хранилища результатов
    /// </summary>
    public enum LoggerStorageType
    {
        /// <summary>
        /// Лог файл
        /// </summary>
        SystemLog
    }
}