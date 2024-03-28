namespace Bars.GisIntegration.Base.GisServiceProvider
{
    using System.ServiceModel;
    using System.ServiceModel.Channels;
    using System.ServiceModel.Description;
    using System.ServiceModel.Dispatcher;
    using System.Xml;

    using Bars.B4.Utils;
    using Bars.GisIntegration.Base.Enums;
    using Bars.GisIntegration.Base.Service;
    using Bars.GisIntegration.Base.Utils;

    using Castle.Core.Internal;
    using Castle.Windsor;

    /// <summary>
    /// Базовый класс поставщик объектов для работы с сервисом ГИС с учетом хранимых настроек
    /// </summary>
    /// <typeparam name="T">Тип soap-клиента</typeparam>
    /// <typeparam name="K">Тип канала</typeparam>
    public abstract class BaseGisServiceProvider<T, K> : IGisServiceProvider<T>
        where T : ClientBase<K>
        where K : class
    {
        private string serviceAddress;

        /// <summary>
        /// Url по-умолчанию
        /// </summary>
        protected abstract string DefaultUrl { get; }

        /// <summary>
        /// Использует асинхронный сервис
        /// </summary>
        protected virtual bool IsAsync => true;

        /// <summary>
        /// Сервис для работы с настройками интеграции ГИС
        /// </summary>
        public IGisSettingsService GisSettingsService { get; set; }

        /// <summary>
        /// Значение перечисления, соответствующее сервису
        /// </summary>
        public abstract IntegrationService IntegrationService { get; }

        /// <summary>
        /// Адрес сервиса
        /// </summary>
        public virtual string ServiceAddress
        {
            get
            {
                if (this.serviceAddress.IsNullOrEmpty())
                {
                    this.serviceAddress = this.GisSettingsService.GetServiceAddress(this.IntegrationService, this.IsAsync, this.DefaultUrl);
                }

                return this.serviceAddress;
            }
        }

        /// <summary>
        /// Ioc контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Получить soap клиент для работы с сервисом
        /// </summary>
        public T GetSoapClient()
        {
            var gisIntegrationConfig = this.Container.Resolve<GisIntegrationConfig>();
            var isHttps = this.ServiceAddress.Split(":")[0] == "https";

            var binding = new BasicHttpBinding
            {
                Security =
                {
                    Mode = isHttps
                        ? BasicHttpSecurityMode.Transport
                        : BasicHttpSecurityMode.TransportCredentialOnly,
                    Transport = new HttpTransportSecurity
                    {
                        ClientCredentialType = isHttps
                            ? HttpClientCredentialType.Certificate
                            : HttpClientCredentialType.Basic
                    }
                },
                MaxReceivedMessageSize = 2147483647
            };

            var remoteAddress = new EndpointAddress(this.ServiceAddress);
            var client = this.GetClient(binding, remoteAddress);

            var defaultCredentials = client.Endpoint.Behaviors.Find<ClientCredentials>();
            client.Endpoint.Behaviors.Remove(defaultCredentials);

            // test
            // SDldfls4lz5@!82d
            if (gisIntegrationConfig.UseLoginCredentials)
            {
                var loginCredentials = new ClientCredentials();
                loginCredentials.UserName.UserName = gisIntegrationConfig.Login;
                loginCredentials.UserName.Password = gisIntegrationConfig.Password;
                client.Endpoint.Behaviors.Add(loginCredentials);
            }

            client.Endpoint.Behaviors.Add(new NsPrefixerBehavior());

            return client;
        }

        /// <summary>
        /// Получить клиент для работы с сервисом
        /// </summary>
        protected abstract T GetClient(BasicHttpBinding binding, EndpointAddress remoteAddress);

        private class NsPrefixerBehavior : IEndpointBehavior, IClientMessageInspector
        {
            void IEndpointBehavior.Validate(ServiceEndpoint endpoint)
            {
            }

            void IEndpointBehavior.AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
            {
            }

            void IEndpointBehavior.ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
            {
            }

            void IEndpointBehavior.ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
            {
                clientRuntime.MessageInspectors.Add(this);
            }

            object IClientMessageInspector.BeforeSendRequest(ref Message request, IClientChannel channel)
            {
                var message = WcfUtils.MessageToString(ref request);

                var document = new XmlDocument();
                document.LoadXml(message);

                var prefixer = new XmlNsPrefixer();
                prefixer.Process(document);

                request = WcfUtils.CreateMessageFromString(document.OuterXml, request.Version);

                return null;
            }

            void IClientMessageInspector.AfterReceiveReply(ref Message reply, object correlationState)
            {
            }
        }
    }
}