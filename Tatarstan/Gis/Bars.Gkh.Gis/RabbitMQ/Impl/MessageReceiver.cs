namespace Bars.Gkh.Gis.RabbitMQ.Impl
{
    using System;

    using Bars.B4.Application;
    using Bars.B4.IoC;
    using Bars.Gkh.Utils.Json;

    using Castle.Windsor;

    using global::RabbitMQ.Client;

    /// <summary>
    /// Приемник сообщений из очереди Rabbit
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MessageReceiver<T> : DefaultBasicConsumer where T : class
    {
        private readonly IModel _channel;

        private readonly IWindsorContainer container = ApplicationContext.Current.Container;

        public MessageReceiver(IModel channel)
        {
            _channel = channel;
        }
        
        /// <summary>
        /// Обработка доставки сообщения
        /// </summary>
        public override void HandleBasicDeliver(string consumerTag, ulong deliveryTag, bool redelivered, string exchange, string routingKey, IBasicProperties properties, ReadOnlyMemory<byte> body)
        {
            var serializer = new CustomJsonMessageSerializer();
            var message = serializer.Deserialize<T>(body.ToArray());
            var messageHandler = this.container.Resolve<IMessageHandler<T>>();

            using (this.container.Using(messageHandler))
            {
                messageHandler.HandleMessage(message);
            }

            _channel.BasicAck(deliveryTag, false);
        }
    }
}