namespace Bars.Gkh.RegOperator.DomainEvent.Events.RealityObjectLoan
{
    using Bars.Gkh.Entities;

    /// <summary>
    /// События окончания задачи
    /// </summary>
    public class RealityObjectLoanTaskEndEvent : RealityObjectLoanTaskEvent
    {
        /// <inheritdoc />
        public RealityObjectLoanTaskEndEvent(RealityObject realityObject)
            : base(realityObject, null)
        {
        }
    }
}