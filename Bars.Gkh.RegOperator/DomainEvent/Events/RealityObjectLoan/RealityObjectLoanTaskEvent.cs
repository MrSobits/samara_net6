namespace Bars.Gkh.RegOperator.DomainEvent.Events.RealityObjectLoan
{
    using Bars.B4.Modules.Tasks.Common.Entities;
    using Bars.Gkh.DomainEvent.Infrastructure;
    using Bars.Gkh.Entities;

    /// <summary>
    /// Событие задачи взятия займа с домом
    /// </summary>
    public abstract class RealityObjectLoanTaskEvent : IDomainEvent
    {
        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="realityObject">Дом</param>
        /// <param name="task">Задача</param>
        public RealityObjectLoanTaskEvent(RealityObject realityObject, TaskEntry task)
        {
            this.Task = task;
            this.RealityObject = realityObject;
        }

        /// <summary>
        /// Жилой дом
        /// </summary>
        public RealityObject RealityObject { get; }

        /// <summary>
        /// Задача взятия займа
        /// </summary>
        public TaskEntry Task { get; }
    }
}