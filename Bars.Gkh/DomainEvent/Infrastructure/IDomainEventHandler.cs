namespace Bars.Gkh.DomainEvent.Infrastructure
{
    /// <summary>
    /// Обработчик события домена
    /// </summary>
    /// <typeparam name="T">Тип события</typeparam>
    public interface IDomainEventHandler<in T> where T : IDomainEvent
    {
        /// <summary>
        /// Обработать событие
        /// </summary>
        /// <param name="args">Аргумент-событие</param>
        void Handle(T args);
    }
}