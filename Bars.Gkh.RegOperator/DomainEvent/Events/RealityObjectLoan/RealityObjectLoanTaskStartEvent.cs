namespace Bars.Gkh.RegOperator.DomainEvent.Events.RealityObjectLoan
{
    using Bars.B4.Modules.Tasks.Common.Entities;
    using Bars.Gkh.Entities;

    /// <summary>
    /// События постановки задачи
    /// </summary>
    public class RealityObjectLoanTaskStartEvent : RealityObjectLoanTaskEvent
    {
        /// <inheritdoc />
        public RealityObjectLoanTaskStartEvent(RealityObject realityObject, TaskEntry task)
            : base(realityObject, task)
        {
        }
    }
}