namespace Bars.Gkh.Gis.RabbitMQ
{

    /// <summary>
    /// Прослушиватель очереди
    /// </summary>
    public interface IConsumerService
    {
        /// <summary>
        /// Начать прослушивание
        /// </summary>
        /// <param name="queueName">Имя очереди, в которую будет публиковаться сообщения</param>
        void StartConsuming(string queueName);

        /// <summary>
        /// Начать прослушивание с обработчиком <see cref="IMessageHandler{T}"/>
        /// </summary>
        /// <param name="queueName">Наименование очереди</param>
        /// <typeparam name="T">Тип сообщения</typeparam>
        void StartConsuming<T>(string queueName)
            where T : class;
    }
}
