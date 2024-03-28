namespace Bars.Gkh.RegOperator.DomainEvent.Events.PersonalAccountDto
{
    using Bars.Gkh.DomainEvent.Infrastructure;
    using Bars.Gkh.Entities;

    /// <summary>
    /// Событие изменения МО или МР
    /// </summary>
    public class MunicipalityChangeEvent : IDomainEvent
    {
        /// <summary>
        /// Муниципальное образование
        /// </summary>
        public Municipality Municipality { get; }

        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="municipality">Муниципальное образование</param>
        public MunicipalityChangeEvent(Municipality municipality)
        {
            this.Municipality = municipality;
        }
    }
}