namespace Bars.Gkh.Gis.RabbitMQ
{
    /// <summary>Публикатор сообщений в очередь</summary>
    public interface IProducerService
    {
        /// <summary>Метод публикации сообщения в очередь.</summary>
        /// <typeparam name="T">Тип сообщения</typeparam>
        /// <param name="queueName">Имя очереди, в которую будет публиковаться сообщения</param>
        /// <param name="message">Сообщение</param>
        void SendMessage<T>(string queueName, T message);

        /// <summary>Метод публикации сообщения вида json в очередь.</summary>
        /// <typeparam name="T">Тип сообщения</typeparam>
        /// <param name="routingKey">Имя очереди, в которую будет публиковаться сообщения</param>
        /// <param name="message">Сообщение</param>
        /// <param name="exchange">exchange</param>
        void SendCustomMessage<T>(string exchange, string routingKey, T message);
    }
}
