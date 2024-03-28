namespace Bars.Gkh.Utils.PerformanceLogging
{
    using Bars.B4.Utils;

    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Коллектор сбора логов в файл .logs/info.log
    /// </summary>
    public class SystemLogsCollector : BaseLogsCollector
    {
        /// <summary>
        /// Менеджер логов
        /// </summary>
        public ILogger LogManager { get; set; }

        /// <summary>
        /// Сохранение данных логов
        /// </summary>
        protected override void SaveLogs()
        {
            if (this.PerforanceLogItems.IsNotEmpty())
            {
                this.PerforanceLogItems.ForEach(x => this.LogManager.LogInformation(
                    $"Key:{x.Key}|Time: {x.TimeSpan}{(x.Description != null ? "|Description: " + x.Description : string.Empty)}"));
            }
        }
    }
}