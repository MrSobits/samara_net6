namespace Bars.Gkh.Quartz.Scheduler
{
    using System;
    using System.Collections.Generic;

    using Bars.Gkh.Quartz.Scheduler.Log;

    using global::Quartz;

    /// <summary>
    /// Интерфейс выполняемой планировщиком задачи
    /// </summary>
    public interface ITask : IInterruptableJob
    {
        /// <summary>
        /// Процент выполнения работы исполнителя
        /// </summary>
        byte Percent { get; }

        /// <summary>
        /// Запрошено прерывание выполнения задачи
        /// </summary>
        bool InterruptRequested { get; }

        /// <summary>
        /// Исключение, возникшее при выполнении задачи, если оно было
        /// </summary>
        Exception ExecutionException { get; }

        /// <summary>
        /// Возвращает коллекцию записей лога выполнения задачи
        /// </summary>
        /// <returns>Список записей лога</returns>
        List<ILogRecord> GetLog();
    }
}
