namespace Bars.Gkh.Gis.RabbitMQ.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.IO;
    using System.Reflection;
    using System.Runtime.Serialization.Formatters.Binary;

    using Bars.B4.Config;
    using Bars.B4.Utils;
    using Bars.Gkh.Gis.MessageContext;
    using Bars.Gkh.Utils.Json;

    using Castle.Windsor;

    using global::RabbitMQ.Client;

    using Newtonsoft.Json;

    /// <summary>Реализация публикатора сообщений в очередь</summary>
    public class ProducerService : IDisposable, IProducerService
    {
        private IWindsorContainer container;
        private IModel _model;
        private IConnection _connection;

        public ProducerService(IWindsorContainer container)
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
        }

        /// <inheritdoc />
        public void SendMessage<T>(string queueName, T message)
        {
            var basicProperties = this._model.CreateBasicProperties();
            var bf = new BinaryFormatter();
            
            basicProperties.Persistent = true;
            this._model.QueueDeclare(queueName, true, false, false, null);
            
            using (var ms = new MemoryStream())
            {
                bf.Serialize(ms, message);
                this._model.BasicPublish(string.Empty, queueName, basicProperties, ms.ToArray());
            }
        }

        /// <inheritdoc />
        public void SendCustomMessage<T>(string exchange, string routingKey, T message)
        {
            var basicProperties = this._model.CreateBasicProperties();
            var customSerializer = new CustomJsonMessageSerializer();
            var serializedMessage = customSerializer.Serialize(message);
            
            basicProperties.Persistent = true;
            this.SetAdditionalProps<T>(basicProperties);
            
            this._model.QueueDeclare(routingKey, true, false, false, null);
            this._model.BasicPublish(exchange, routingKey, basicProperties, serializedMessage);
        }

        private void SetAdditionalProps<T>(IBasicProperties basicProperties)
        {
            var jsonSerializer = new JsonSerializer();
            var contextProvider = new DefaultMessageContextProvider<MessageContext>(jsonSerializer);
            Guid guid = Guid.Empty;
            basicProperties.MessageId = Guid.NewGuid().ToString();
            basicProperties.Headers = new Dictionary<string, object>();

            basicProperties.Headers.Add("sent", DateTime.UtcNow.ToString("u"));
            basicProperties.Headers.Add("message_type", this.GetTypeName(typeof(T)));
            basicProperties.Headers.Add("message_context", contextProvider.GetMessageContext(ref guid));
        }

        private string GetTypeName(Type type)
        {
            var name = $"{type.Namespace}.{type.Name}";
            if (type.GenericTypeArguments.Length > 0)
            {
                var shouldInsertComma = false;
                name += '[';
                foreach (var genericType in type.GenericTypeArguments)
                {
                    if (shouldInsertComma)
                        name += ",";
                    name += $"[{this.GetTypeName(genericType)}]";
                    shouldInsertComma = true;
                }
                name += ']';
            }
            name += $", {type.GetTypeInfo().Assembly.GetName().Name}";
            return name;
        }
        
        public void Dispose()
        {
            if (this._connection != null)
            {
                this._connection.Close();
            }

            if (this._model != null)
            {
                this._model.Abort();
            }
        }
    }
}