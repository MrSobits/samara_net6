namespace Bars.Gkh.RegOperator.DomainEvent.Events.PersonalAccountDto
{
    using Bars.Gkh.DomainEvent.Infrastructure;
    using Bars.Gkh.Entities;

    /// <summary>
    /// Событие изменения дома
    /// </summary>
    public class RealityObjectForDtoChangeEvent : IDomainEvent
    {
        /// <summary>
        /// Жилой дом
        /// </summary>
        public RealityObject RealityObject { get; }

        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="realityObject">Жилой дом</param>
        public RealityObjectForDtoChangeEvent(RealityObject realityObject)
        {
            this.RealityObject = realityObject;
        }
    }
}