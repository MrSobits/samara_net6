namespace Bars.Gkh.Quartz.Scheduler
{
    /// <summary>
    /// Интерфейс инициатора выполнения задачи
    /// </summary>
    public interface IExecutionOwner
    {
        /// <summary>
        /// Тип инициатора выполнения задачи
        /// </summary>
        ExecutionOwnerType Type { get; }

        /// <summary>
        /// Наименование
        /// </summary>
        string Name { get; }
    }
}
