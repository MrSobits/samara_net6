namespace Bars.Gkh.RegOperator.Domain
{
    using Bars.Gkh.DomainEvent.Infrastructure;

    using DomainEvent;
    using Gkh.Entities;

    /// <summary>
    /// Сессия обновления счетов дома. Аггрегирует оплаты по разным ЛС
    /// </summary>
    public interface IRealtyObjectPaymentSession
    {
        /// <summary>
        /// Послать событие обновления дома
        /// </summary>
        /// <param name="realityObject">Дом</param>
        /// <param name="event">Событие с аргументами</param>
        void FireEvent(RealityObject realityObject, IDomainEvent @event);

        /// <summary>
        /// Завершить сессию и обновить дома
        /// </summary>
        void Complete();

        /// <summary>
        /// Откатить сессию
        /// </summary>
        void Rollback();
    }
}