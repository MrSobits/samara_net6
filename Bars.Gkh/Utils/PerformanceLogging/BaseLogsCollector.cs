namespace Bars.Gkh.Utils.PerformanceLogging
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    using Bars.B4.Utils;

    /// <summary>
    /// Сервис для работы с логами производительности, сохраняемым в лог файл
    /// </summary>
    public abstract class BaseLogsCollector : IPerformanceLogsCollector
    {
        /// <summary>
        /// Логгированные данные
        /// </summary>
        public IReadOnlyList<PerformanceLog> PerforanceLogItems { get; private set; }

        /// <inheritdoc />
        public void CollectLogData(
            IEnumerable<PerformanceLog> logs, 
            Func<IEnumerable<PerformanceLog>, PerformanceLog> aggregator = null,
            Func<PerformanceLog, object> sorter = null)
        {
            if (this.PerforanceLogItems.IsNotEmpty())
            {
                return;
            }

            var logsEnumerable = logs;

            if (aggregator.IsNotNull())
            {
                logsEnumerable = logs.GroupBy(x => x.Key)
                    .ToDictionary(x => x.Key, x => aggregator(x))
                    .Select(x => x.Value);
            }

            if (sorter.IsNotNull())
            {
                logsEnumerable = logsEnumerable.OrderBy(sorter);
            }

            this.PerforanceLogItems = new ReadOnlyCollection<PerformanceLog>(logsEnumerable.ToList());

            this.SaveLogs();
        }

        /// <summary>
        /// Сохранение данных логов
        /// </summary>
        protected abstract void SaveLogs();
    }
}