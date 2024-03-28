namespace Bars.Gkh.RegOperator.Domain.RealtyObjectPayment
{
    using System;
    using System.Collections.Generic;

    using Bars.Gkh.DomainEvent.Infrastructure;

    using DomainEvent;
    using Gkh.Entities;

    /// <summary>
    /// Контейнер с событиями обновления балансов домов
    /// </summary>
    public class RealtyObjectEventContainer : IDisposable
    {
        private static readonly object SyncRoot = new object();
        private Dictionary<IRealtyObjectPaymentSession, Dictionary<RealityObject, List<IDomainEvent>>> container;
        
        /// <summary>
        /// .ctor
        /// </summary>
        public RealtyObjectEventContainer()
        {
            this.container = new Dictionary<IRealtyObjectPaymentSession, Dictionary<RealityObject, List<IDomainEvent>>>();
        }

        /// <summary>
        /// Добавить событие обновления дома
        /// </summary>
        /// <param name="realityObject">Дом</param>
        /// <param name="session">Сессия обновления</param>
        /// <param name="event">Событие обновления</param>
        public void AddEvent(
            RealityObject realityObject,
            IRealtyObjectPaymentSession session,
            IDomainEvent @event)
        {
            lock (RealtyObjectEventContainer.SyncRoot)
            {
                Dictionary<RealityObject, List<IDomainEvent>> sessionItems;

                if (!this.container.TryGetValue(session, out sessionItems))
                {
                    sessionItems = new Dictionary<RealityObject, List<IDomainEvent>>
                    {
                        [realityObject] = new List<IDomainEvent>()
                    };

                    this.container[session] = sessionItems;
                }

                if (!sessionItems.ContainsKey(realityObject))
                {
                    sessionItems[realityObject] = new List<IDomainEvent>();
                }

                sessionItems[realityObject].Add(@event);
            }
        }

        /// <summary>
        /// Очистить контейнер от событий сессии
        /// </summary>
        /// <param name="session">Сессия обновления</param>
        public void ClearSessionItems(IRealtyObjectPaymentSession session)
        {
            lock (RealtyObjectEventContainer.SyncRoot)
            {
                this.container.Remove(session);
            }
        }

        /// <summary>
        /// Получить список событий обновления с разбивкой по домам
        /// </summary>
        /// <param name="session">Сессия обновления</param>
        /// <returns></returns>
        public Dictionary<RealityObject, List<IDomainEvent>> GetSnapshot(IRealtyObjectPaymentSession session)
        {
            var result = new Dictionary<RealityObject, List<IDomainEvent>>();

            lock (RealtyObjectEventContainer.SyncRoot)
            {
                Dictionary<RealityObject, List<IDomainEvent>> item;

                if (this.container.TryGetValue(session, out item))
                {
                    result = item;
                }
            }

            this.ClearSessionItems(session);

            return result;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.container = null;
        }
    }
}