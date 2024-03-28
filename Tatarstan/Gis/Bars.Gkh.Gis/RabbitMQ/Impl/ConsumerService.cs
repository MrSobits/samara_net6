namespace Bars.Gkh.Gis.RabbitMQ.Impl
{
    using System;
    using System.Configuration;
    using System.IO;
    using System.Runtime.Serialization.Formatters.Binary;

    using Bars.B4.Config;
    using Bars.B4.Utils;

    using Castle.Windsor;
    using global::RabbitMQ.Client;
    using global::RabbitMQ.Client.Events;
    using global::RabbitMQ.Client.Exceptions;

    /// <summary>Подписчик</summary>
    public class ConsumerService : IConsumerService, IDisposable
    {
        private IWindsorContainer container;

        ///// <summary>used to pass messages back to for processing</summary>
        ///// <param name="message">Сообщение</param>
        ///// <param name="queue">Очередь</param>
        //public delegate void OnReceiveMessage(byte[] message, string queue);

        /// <summary>internal delegate to run the consuming queue on a seperate thread</summary>
        private delegate void ConsumeDelegate();

        /// <summary>Событие получения сообщения</summary>
        //public event OnReceiveMessage OnMessageReceived;

        private IModel _model;

        private IConnection _connection;

        private string _queueName;

        protected bool _isConsuming;
        
        public ConsumerService(IWindsorContainer container)
        {
            this.container = container;
            
            var appSettings = this.container.Resolve<IConfigProvider>().GetConfig().GetModuleConfig("Bars.Gkh.Gis");

            if (appSettings == null || !appSettings[SettingsKeyStore.Enable].To<bool>())
            {
                throw new ConfigurationException("Модуль Rabbit не активирован либо его настройки отсутствуют в файле конфигурации");
            }

            this._connection = new ConnectionFactory
            {
                HostName = appSettings[SettingsKeyStore.Ip].To<string>(),
                Port = appSettings[SettingsKeyStore.Port].To<int>(),
                UserName = appSettings[SettingsKeyStore.Login].To<string>(),
                Password = appSettings[SettingsKeyStore.Password].To<string>(),
                VirtualHost = appSettings[SettingsKeyStore.VirtualHost].To<string>()
            }.CreateConnection();
            this._model = this._connection.CreateModel();
            this._model.BasicQos(0, 1, false);
        }

        /// <summary>
        /// Запуск примеки сообщений
        /// </summary>
        public void StartConsuming(string queueName)
        {
            this._queueName = queueName;
            this._model.QueueDeclare(this._queueName, true, false, false, null);

            this._isConsuming = true;
            var consumeDelegate = new ConsumeDelegate(Consume);
            consumeDelegate.BeginInvoke(null, null);
        }
        
        public void StartConsuming<T>(string queueName) where T: class
        {
            this._queueName = queueName;
            this._model.QueueDeclare(this._queueName, true, false, false, null);

            var messageReceiver = typeof(MessageReceiver<>)
                .MakeGenericType(typeof(T))
                .GetConstructor(new[] { typeof(IModel)})
                .Invoke(new object[] { this._model }) as IBasicConsumer;
            
            this._model.BasicConsume(this._queueName, false, messageReceiver);
        }

        /// <summary>Метод извлечения сообщений из очереди</summary>
        private void Consume()
        {
            // TODO: Заменить консьюмера
            var consumer = new EventingBasicConsumer(this._model);
            var consumerTag = this._model.BasicConsume(this._queueName, false, consumer);
           
            consumer.Received += (model, e) =>
            {
                try
                {
                    var body = e.Body.ToArray();
                    OnMessageReceived(body);
                    this._model.BasicAck(e.DeliveryTag, false);
                }
                catch (OperationInterruptedException exc)
                {
                    this._model.BasicCancel(consumerTag);
                    Dispose();
                }
                catch (EndOfStreamException)
                {
                    Dispose();
                }
                catch (Exception ex)
                {
                    this._model.BasicCancel(consumerTag);
                    Dispose();
                }
            };
        }

        private void OnMessageReceived(byte[] body)
        {
            BaseTask task;

            using (var memoryStream = new MemoryStream(body))
            {
                var deserializer = new BinaryFormatter();
                task = (BaseTask)deserializer.Deserialize(memoryStream);
            }

            var genericType = typeof(ITaskHandler<>);
            var taskHandlerType = genericType.MakeGenericType(new[] { task.Type });
            var taskHandler = this.container.Resolve(taskHandlerType);
            var method = taskHandler.GetType().GetMethod("Run");
            method.Invoke(taskHandler, new object[] { task });
        }

        public void Dispose()
        {
            try
            {
                this._isConsuming = false;
                this._connection?.Close();
                this._model?.Abort();
            }
            catch
            {
                // ignored
            }
        }
    }
}