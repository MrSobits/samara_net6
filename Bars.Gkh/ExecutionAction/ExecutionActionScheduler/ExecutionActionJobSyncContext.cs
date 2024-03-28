namespace Bars.Gkh.ExecutionAction.ExecutionActionScheduler
{
    using System.Threading;

    /// <summary>
    /// Контекст синхронизации окончания выполнения действия и старта следующего
    /// </summary>
    public static class ExecutionActionJobSyncContext
    {
        private static bool jobIsRunning;
        private static readonly object SyncRoot = new object();

        /// <summary>
        /// Дождаться завершения работающей задачи, если такая есть, затем выполнение метода закончится, если имеется возможность запуска действия
        /// </summary>
        public static void WaitOrStartJob()
        {
            lock (ExecutionActionJobSyncContext.SyncRoot)
            {
                while (ExecutionActionJobSyncContext.jobIsRunning)
                {
                    Monitor.Wait(ExecutionActionJobSyncContext.SyncRoot);
                }

                ExecutionActionJobSyncContext.jobIsRunning = true;
            }
        }

        /// <summary>
        /// Сообщить другим участникам, что задача завершена
        /// </summary>
        public static void EndJob()
        {
            lock (ExecutionActionJobSyncContext.SyncRoot)
            {
                ExecutionActionJobSyncContext.jobIsRunning = false;

                // сообщаем первому потоку в очереди, что мы закончили
                Monitor.Pulse(ExecutionActionJobSyncContext.SyncRoot);
            }
        }
    }
}