namespace Bars.Gkh.Utils.PerformanceLogging
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Интерфейс для сохранения логов производительности
    /// </summary>
    public interface IPerformanceLogsCollector
    {
        /// <summary>
        /// Собрать данные логгирования
        /// </summary>
        /// <param name="logs"></param>
        /// <param name="aggregator">Функция агрегации, с помощью неё можно получить результирующий лог элемент при группировке по ключу</param>
        /// <param name="sorter">Функция сортировки результирующей коллекции</param>
        void CollectLogData(
            IEnumerable<PerformanceLog> logs, 
            Func<IEnumerable<PerformanceLog>, PerformanceLog> aggregator = null,
            Func<PerformanceLog, object> sorter = null);
    }
}