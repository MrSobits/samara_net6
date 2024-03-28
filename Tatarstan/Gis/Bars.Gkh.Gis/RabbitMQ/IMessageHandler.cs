namespace Bars.Gkh.Gis.RabbitMQ
{
    /// <summary>
    /// Интерфейс обработчика сообщений из очередей Rabbit
    /// </summary>
    /// <typeparam name="T">Тип сообщения</typeparam>
    public interface IMessageHandler<T> where T: class
    {
        /// <summary>
        /// Обработать сообщение
        /// </summary>
        /// <param name="message">Сообщение</param>
        void HandleMessage(T message);
    }
}