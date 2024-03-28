namespace Bars.Gkh.RegOperator.DomainEvent.Events.PersonalAccountDto
{
    using Bars.Gkh.DomainEvent.Infrastructure;
    using Bars.Gkh.Entities;
    using Bars.Gkh.RegOperator.Entities.Dto;

    /// <summary>
    /// Событие изменение данных в помещении, необходим для актуализации <see cref="BasePersonalAccountDto"/>
    /// </summary>
    public class RoomChangeEvent : IDomainEvent
    {
        /// <summary>
        /// Помещение
        /// </summary>
        public Room Room { get; }

        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="room">Измененное помещение</param>
        public RoomChangeEvent(Room room)
        {
            this.Room = room;
        }
    }
}