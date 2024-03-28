namespace Bars.Gkh.Utils.PerformanceLogging
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Threading;

    using Bars.B4.Config;
    using Bars.B4.Utils;

    /// <summary>
    /// Класс логгирования скорости выполнения
    /// (многопоточный, т.е. может получить несколько логгеров с одним sessionId и в итоге все данные будут собраны со всех потоков).
    /// <remarks>Для того, чтобы логгер записал результаты, необходимо в b4.config/b4.user.config добавить 
    /// в блок appSetting ключ PerformanceLogger.Enabled со значением true. 
    /// Расширяемый логгер, результаты логгирования можно сохранить в бд, если реализовать 
    /// собственный <see cref="IPerformanceLogsCollector"/>, и уже при получении из фабрики логгера, указать, 
    /// куда будут сохранятся данные.</remarks>
    /// </summary>
    public class PerformanceLogger : IPerformanceLogger
    {
        private readonly ConcurrentDictionary<string, Tuple<PerformanceLog, Stopwatch>> performanceLogs;

        private readonly string sessionId;

        private readonly bool enabled;
        private bool closed;

        private static Dictionary<string, IList<PerformanceLog>> logs;
        private static Dictionary<string, HashSet<int>> workThreadIds;
        private static ConcurrentDictionary<string, object> syncRoots;

        /// <summary>
        /// Статический конструктор
        /// </summary>
        static PerformanceLogger()
        {
            PerformanceLogger.workThreadIds = new Dictionary<string, HashSet<int>>();
            PerformanceLogger.syncRoots = new ConcurrentDictionary<string, object>();
            PerformanceLogger.logs = new Dictionary<string, IList<PerformanceLog>>();
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        public PerformanceLogger(IConfigProvider configProvider, string sessionId)
        { 
            this.sessionId = sessionId;

            PerformanceLogger.syncRoots.TryAdd(sessionId, new object());
            this.enabled = configProvider.GetConfig().AppSettings.GetAs("PerformanceLogger.Enabled", false);

            this.performanceLogs = new ConcurrentDictionary<string, Tuple<PerformanceLog, Stopwatch>>();
        }

        /// <summary>
        /// Начать отсчёт
        /// </summary>
        /// <param name="key">Ключ</param>
        /// <param name="description">Описание выполняемого действия</param>
        public void StartTimer(string key, string description = null)
        {
            this.ThrowIfClosed();

            if (!this.enabled)
            {
                return;
            }

            this.SaveThreadId();
            var sessionKey = this.GetSessionKey(key);

            if (this.performanceLogs.ContainsKey(sessionKey))
            {
                throw new InvalidOperationException("Запрещено запускать таймер по идентичному ключу");
            }
            
            Debug.WriteLine($"{key} started");
            var value = new Tuple<PerformanceLog, Stopwatch>(new PerformanceLog(key, description), Stopwatch.StartNew());
            this.performanceLogs.TryAdd(sessionKey, value);
        }

        /// <summary>
        /// Остановить отсчёт по указанному ключу
        /// </summary>
        /// <param name="key">Ключ</param>
        /// <returns>Время выполнения</returns>
        public TimeSpan StopTimer(string key)
        {
            this.ThrowIfClosed();

            if (!this.enabled)
            {
                return TimeSpan.MinValue;
            }

            Tuple<PerformanceLog, Stopwatch> log;

            var sessionKey = this.GetSessionKey(key);

            if (this.performanceLogs.TryRemove(sessionKey, out log))
            {
                log.Item2.Stop();
                log.Item1.TimeSpan = log.Item2.Elapsed;
                Debug.WriteLine($"{key}: {log.Item1.TimeSpan}");

                lock (PerformanceLogger.syncRoots[this.sessionId])
                {
                    var listData = PerformanceLogger.logs.Get(this.sessionId);
                    if (listData.IsNull())
                    {
                        PerformanceLogger.logs[this.sessionId] = listData = new List<PerformanceLog>();
                    }

                    listData.Add(log.Item1);
                }

                return log.Item1.TimeSpan;
            }

            return TimeSpan.MinValue;
        }

        /// <summary>
        /// Закончить текущую сессию логгирования
        /// </summary>
        public void ClearSession()
        {
            if (!this.enabled)
            {
                return;
            }

            lock (PerformanceLogger.syncRoots[this.sessionId])
            {
                this.ThrowIfClosed();
                this.closed = true;

                PerformanceLogger.workThreadIds[this.sessionId].Clear();
                PerformanceLogger.workThreadIds.Remove(this.sessionId);

                PerformanceLogger.logs[this.sessionId].Clear();
                PerformanceLogger.logs.Remove(this.sessionId);

                object obj;
                PerformanceLogger.syncRoots.TryRemove(this.sessionId, out obj);
            }
        }


        /// <inheritdoc/>
        public void SaveLogs(
            IPerformanceLogsCollector logsCollector, 
            Func<IEnumerable<PerformanceLog>, PerformanceLog> aggregator = null, 
            Func<PerformanceLog, object> sorter = null)
        {
            this.ThrowIfClosed();

            if (!this.enabled)
            {
                return;
            }

            lock (PerformanceLogger.syncRoots[this.sessionId])
            {
                logsCollector.CollectLogData(PerformanceLogger.logs[this.sessionId], aggregator, sorter);
            }
        }

        /// <summary>
        /// Сохраним текущий идентификатор потока, для понимания, сколько потоков сейчас работает с логгером
        /// </summary>
        private void SaveThreadId()
        {
            var threadId = Thread.CurrentThread.ManagedThreadId;

            lock (PerformanceLogger.syncRoots[this.sessionId])
            {
                var hashSet = PerformanceLogger.workThreadIds.Get(this.sessionId);
                if (hashSet.IsNull())
                {
                    PerformanceLogger.workThreadIds[this.sessionId] = hashSet = new HashSet<int>();
                }

                hashSet.Add(threadId);
            }
        }

        private string GetSessionKey(string key)
        {
            return $"{this.sessionId}#{Thread.CurrentThread.ManagedThreadId.ToString()}#{key}";
        }

        private void ThrowIfClosed()
        {
            if (this.closed)
            {
                throw new InvalidOperationException("Log session closed");
            }
        }
    }
}