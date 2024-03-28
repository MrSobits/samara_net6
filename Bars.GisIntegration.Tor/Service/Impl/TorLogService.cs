namespace Bars.GisIntegration.Tor.Service.Impl
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Text;

    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.B4.Modules.FileStorage;
    using Bars.GisIntegration.Tor.Service.LogService;

    using Castle.Windsor;

    public class TorLogService : ITorLogService
    {
        /// <summary>
        /// Словарь логов.
        /// </summary>
        private readonly ConcurrentDictionary<long, List<string>> logs = new ConcurrentDictionary<long, List<string>>();

        /// <summary>
        /// Добавляет запись в лог
        /// </summary>
        /// <param name="record">Запись</param>
        /// <param name="torTaskId">Уникальный идентификатор задачи</param>
        public void AddLogRecord(string record, long torTaskId)
        {
            if (!this.logs.ContainsKey(torTaskId))
            {
                this.logs[torTaskId] = new List<string>();
            }
            var rec = $"{DateTime.Now: dd.MM.yyyy HH:mm:ss}: {record}";
            this.logs[torTaskId].Add(rec);
        }

        /// <summary>
        /// Сохраняет лог в файл
        /// </summary>
        /// <param name="torTaskId">Уникальный идентификатор задачи</param>
        /// <param name="container">IoC контейнер</param>
        /// <returns>Идентификатор файла лога</returns>
        public FileInfo SaveLogFile(long torTaskId, IWindsorContainer container)
        {
            if (!this.logs.TryGetValue(torTaskId, out var logList))
            {
                return null;
            }
            
            var fileManager = container.Resolve<IFileManager>();

            using (container.Using(fileManager))
            {
                var fileData = new FileData($"Log_{torTaskId}", "txt",
                    Encoding.UTF8.GetBytes(string.Join("\r\n", logList)));
                var file = fileManager.SaveFile(fileData);
                this.logs.TryRemove(torTaskId, out _);
                return file;
            }
        }
    }
}
