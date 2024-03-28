namespace Bars.Gkh.DomainEvent.Events
{
    using Bars.B4.DataModels;

    using Infrastructure;

    /// <summary>
    /// Событие смены обощенного статуса
    /// </summary>
    public class GeneralStateChangeEvent : IDomainEvent
    {
        /// <summary>
        /// Сущность
        /// </summary>
        public IHaveId Entity { get; private set; }

        /// <summary>
        /// Старое значение
        /// </summary>
        public object OldValue { get; private set; }

        /// <summary>
        /// Новое значение
        /// </summary>
        public object NewValue { get; private set; }

        /// <summary>
        /// Имя свойства, которое нужно журналировать
        /// </summary>
        public string PropertyName { get; private set; }

        public GeneralStateChangeEvent(
            IHaveId entity,
            object oldValue,
            object newValue,
            string propertyName = null)
        {
            this.Entity = entity;
            this.OldValue = oldValue;
            this.NewValue = newValue;
            this.PropertyName = propertyName;
        }
    }
}