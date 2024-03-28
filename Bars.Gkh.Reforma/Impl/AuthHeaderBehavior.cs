namespace Bars.Gkh.Reforma.Impl
{
    using System;
    using System.ServiceModel;
    using System.ServiceModel.Channels;
    using System.ServiceModel.Description;
    using System.ServiceModel.Dispatcher;
    using System.Xml.Serialization;

    /// <summary>
    /// Расширение клиента, добавляющее заголовок авторизации в запросы
    /// </summary>
    public class AuthHeaderBehavior : IEndpointBehavior, IClientMessageInspector
    {
        private readonly string login;
        private readonly string password;

        #region Constructors and Destructors

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="login">Логин</param>
        /// <param name="password">Пароль</param>
        public AuthHeaderBehavior(string login, string password)
        {
            this.login = login;
            this.password = password;
        }

        #endregion

        #region Public Methods and Operators

        public void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
        {
        }

        public void AfterReceiveReply(ref Message reply, object correlationState)
        {
        }

        public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {
            clientRuntime.ClientMessageInspectors.Add(this);
        }

        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
        {
        }

        public object BeforeSendRequest(ref Message request, IClientChannel channel)
        {
            request.Headers.Add(MessageHeader.CreateHeader("Login", string.Empty, new LoginHeader(this.login, this.password)));
            return null;
        }

        public void Validate(ServiceEndpoint endpoint)
        {
        }

        #endregion

        [Serializable]
        private class LoginHeader
        {
            private readonly string login;

            private readonly string password;

            public LoginHeader(string login, string password)
            {
                this.login = login;
                this.password = password;
            }

            [XmlElement("login")]
            public string Login
            {
                get
                {
                    return this.login;
                }
            }

            [XmlElement("password")]
            public string Password
            {
                get
                {
                    return this.password;
                }
            }
        }
    }
}