using System;
using System.Collections.Generic;

namespace Bars.Gkh.Utils.PerformanceLogging
{
    /// <summary>
    /// Интерфейс логгирования скорости выполнения
    /// </summary>
    public interface IPerformanceLogger
    {
        /// <summary>
        /// Сохранить результаты замера производительности
        /// </summary>
        /// <param name="logsCollector">Интерфейс для сохранения логов производительности</param>
        /// <param name="aggregator">Функция агрегации для результатов логов, если не указать, будут записаны все результаты</param>
        /// <param name="sorter">Функция сортировки результатов логов</param>
        void SaveLogs(
            IPerformanceLogsCollector logsCollector, 
            Func<IEnumerable<PerformanceLog>, PerformanceLog> aggregator = null,
            Func<PerformanceLog, object> sorter = null);

        /// <summary>
        /// Начать отсчёт
        /// </summary>
        /// <param name="key">Ключ</param>
        /// <param name="description">Описание выполняемого действия</param>
        void StartTimer(string key, string description = null);

        /// <summary>
        /// Остановить отсчёт по указанному ключу
        /// </summary>
        /// <param name="key">Ключ</param>
        /// <returns>Время выполнения</returns>
        TimeSpan StopTimer(string key);

        /// <summary>
        /// Закончить текущую сессию логгирования
        /// </summary>
        void ClearSession();
    }
}