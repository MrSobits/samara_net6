namespace Bars.Gkh.RegOperator.DomainEvent.Infrastructure
{
    using Bars.Gkh.DomainEvent.Infrastructure;

    using Domain.AggregationRoots;

    /// <summary>
    /// Интерфейс обработчиков событий, которые будут выполнены
    /// при вызове метода Complete() реализации интерфейса IRealtyObjectPaymentSession
    /// </summary>
    /// <typeparam name="TEvent">Тип события</typeparam>
    public interface IRealtyObjectPaymentEventHandler<in TEvent>
        where TEvent : IDomainEvent
    {
        void Execute(RealtyObjectPaymentRoot root, TEvent args);
    }
}