namespace Bars.Gkh.Utils.PerformanceLogging
{
    using System;
    using System.Collections.Generic;

    using Castle.Windsor;

    /// <summary>
    /// Фабрика для <see cref="IPerformanceLogger"/>
    /// </summary>
    public class PerformanceLoggerFactory : IPerformanceLoggerFactory
    {
        /// <summary>
        /// Контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Получить сконфигурированный логгер производительности
        /// </summary>
        /// <param name="sessionId">Идентификатор сессии, для агрегирования результатов</param>
        /// <returns>Интерфейс логгирования скорости выполнения</returns>
        public IPerformanceLogger GetLogger(string sessionId)
        {
            return this.Container.Resolve<IPerformanceLogger>(new[]
            {
                new KeyValuePair<string, object>("sessionId", sessionId ?? Guid.NewGuid().ToString()),
            });
        }

        /// <summary>
        /// Получить сконфигурированный коллектор для логов производительности
        /// </summary>
        /// <param name="storageType">Целевое хранилище логов</param>
        /// <returns>Интерфейс логгирования скорости выполнения</returns>
        public IPerformanceLogsCollector GetCollector(LoggerStorageType storageType = LoggerStorageType.SystemLog)
        {
            switch (storageType)
            {
                case LoggerStorageType.SystemLog:
                    return this.Container.Resolve<IPerformanceLogsCollector>("SystemLogsCollector");
                default:
                    throw new ArgumentOutOfRangeException(nameof(storageType), storageType, null);
            }
        }
    }
}