namespace Bars.Gkh.Reforma.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.ServiceModel;

    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.Reforma.Domain;
    using Bars.Gkh.Reforma.Enums;
    using Bars.Gkh.Reforma.Impl.Logger;
    using Bars.Gkh.Reforma.Impl.Performer;
    using Bars.Gkh.Reforma.Impl.Session;
    using Bars.Gkh.Reforma.Interface;
    using Bars.Gkh.Reforma.Interface.Logger;
    using Bars.Gkh.Reforma.Interface.Performer;
    using Bars.Gkh.Reforma.Interface.Session;
    using Bars.Gkh.Reforma.ReformaService;

    using Castle.Windsor;

    /// <summary>
    ///     Провайдер синхронизации с Реформаой ЖКХ
    /// </summary>
    public class SyncProvider : ISyncProvider
    {
        #region Properties

        /// <summary>
        ///     IoC контейнер
        /// </summary>
        private IWindsorContainer Container { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Закрыть сессию интеграции с реформой
        /// </summary>
        public void Close()
        {
            //TODO: необходимо реализовать освобождение очереди, т.к. сама интеграция продолжает работу
            // будет исправлено в рамках разработки по отмене интеграции с реформой
            if (!this.closed)
            {
                lock (this.lockObject)
                {
                    if (!this.closed)
                    {
                        if (this.Session != null)
                        {
                            this.Session.Close();
                            this.Container.Release(this.Session);
                        }

                        if (this.Logger != null)
                        {
                            this.Container.Release(this.Logger);
                        }

                        if (this.performer != null)
                        {
                            this.Container.Release(this.performer);
                        }

                        this.closed = true;
                    }
                }
            }
        }

        #endregion

        #region Fields

        private readonly object lockObject = new object();

        private ApiSoapPortClient client;

        private bool closed;

        private ISyncActionPerformer performer;

        private string remoteAddress;

        private string login;

        private string password;

        #endregion

        #region Constructors and Destructors
        /// <summary>
        ///     Конструктор
        /// </summary>
        /// <param name="container">
        ///     IoC контейнер
        /// </param>
        /// <param name="remoteAddress">
        ///     Адрес сервиса
        /// </param>
        /// <param name="login">
        ///     Имя пользователя
        /// </param>
        /// <param name="password">
        ///     Пароль
        /// </param>
        /// <param name="sessionId">
        ///     Идентификатор переоткрываемой сессии
        /// </param>
        /// <param name="silentMode">Отключить заведение сессии и логирование</param>
        /// <param name="typeIntegration">
        ///     Тип запускаемой интеграции
        /// </param>
        public SyncProvider(IWindsorContainer container, string remoteAddress, string login, string password, Guid? sessionId = null, bool silentMode = false, TypeIntegration typeIntegration = TypeIntegration.Automatic)
        {
            this.Container = container;
            this.Init(remoteAddress, login, password, sessionId, silentMode, typeIntegration);
        }

        /// <summary>
        ///     Конструктор
        /// </summary>
        /// <param name="container">IoC контейнер</param>
        /// <param name="sessionId">Идентификатор переоткрываемой сессии</param>
        /// <param name="silentMode">Отключить заведение сессии и логирование</param>
        /// <param name="typeIntegration">Тип запускаемой интеграции</param>
        public SyncProvider(IWindsorContainer container, Guid? sessionId = null, bool silentMode = false, TypeIntegration typeIntegration = TypeIntegration.Automatic)
        {
            this.Container = container;
            this.Container.UsingForResolved<ISyncService>(
                (windsorContainer, service) =>
                    {
                        var parameters = service.GetParams();
                        var data = (Dictionary<string, object>)parameters.Data;
                        this.Init((string)data.Get("RemoteAddress"), (string)data.Get("User"), (string)data.Get("Password"), sessionId, silentMode, typeIntegration);
                    });
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Клиент сервиса Реформа ЖКХ
        /// </summary>
        public ApiSoapPortClient Client
        {
            get
            {
                this.UpdateClient();
                return this.client;
            }
        }

        /// <summary>
        ///     Логгер
        /// </summary>
        public ISyncLogger Logger { get; private set; }

        /// <summary>
        ///     Планировщик действий
        /// </summary>
        public ISyncActionPerformer Performer
        {
            get
            {
                this.UpdatePerformer();
                return this.performer;
            }
        }

        /// <summary>
        ///     Сессия синхронизации
        /// </summary>
        public ISyncSession Session { get; private set; }

        #endregion

        #region Methods

        private void UpdateClient()
        {
            this.ThrowIfClosed();

            if (this.client == null || this.client.State == CommunicationState.Faulted)
            {
                lock (lockObject)
                {
                    if (this.client == null || this.client.State == CommunicationState.Faulted)
                    {
                        // TODO Разобраться с HttpBinding, WCF
                        // new BasicHttpBinding("ApiSoapBinding")
                        var binding = new BasicHttpBinding();
                        binding.Name = "";
                        this.client = !string.IsNullOrEmpty(this.remoteAddress)
                                          ? new ApiSoapPortClient(binding, new EndpointAddress(this.remoteAddress))
                                          : new ApiSoapPortClient();

                        if (this.remoteAddress.StartsWith("https"))
                        {
                            ((BasicHttpBinding)client.Endpoint.Binding).Security = new BasicHttpSecurity
                                                                                       {
                                                                                           Mode = BasicHttpSecurityMode.Transport,
                                                                                           Transport =
                                                                                               new HttpTransportSecurity
                                                                                                   {
                                                                                                       ClientCredentialType = HttpClientCredentialType.None,
                                                                                                       ProxyCredentialType = HttpProxyCredentialType.None
                                                                                                   }
                                                                                       };
                        }

                        this.client.Endpoint.EndpointBehaviors.Add(new AuthHeaderBehavior(this.login, this.password));
                        this.client.Endpoint.EndpointBehaviors.Add(new NamespaceFixingBehavior(this.remoteAddress));
                        if (this.Logger != null)
                        {
                            this.client.Endpoint.EndpointBehaviors.Add(new LoggerBehavior(this.Logger));
                        }

                        var vs = this.client.Endpoint.EndpointBehaviors.FirstOrDefault(endpointBehavior => endpointBehavior.GetType().Namespace == "Microsoft.VisualStudio.Diagnostics.ServiceModelSink");
                        if (vs != null)
                        {
                            this.client.Endpoint.EndpointBehaviors.Remove(vs);
                        }
                    }
                }
            }
        }

        private void UpdatePerformer()
        {
            this.ThrowIfClosed();
            if (this.performer == null)
            {
                lock (lockObject)
                {
                    if (this.performer == null)
                    {
                        this.performer = new SyncActionPerformer(this.Container, this);
                    }
                }
            }
        }

        private void Init(string remoteAddress, string login, string password, Guid? sessionId = null, bool silentMode = false, TypeIntegration typeIntegration = TypeIntegration.Automatic)
        {
            this.remoteAddress = remoteAddress;
            this.login = login;
            this.password = password;
            if (!silentMode)
            {
                this.Session = sessionId.HasValue ? new SyncSession(sessionId.Value) : new SyncSession(typeIntegration);
                this.Logger = new SyncLogger(this.Container, this.Session);
            }
            else
            {
                this.Logger = new NullSyncLogger();
            }
        }

        private void ThrowIfClosed()
        {
            if (this.closed)
            {
                throw new InvalidOperationException();
            }
        }

        #endregion
    }
}