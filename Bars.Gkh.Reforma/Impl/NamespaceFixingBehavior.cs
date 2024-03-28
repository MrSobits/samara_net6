namespace Bars.Gkh.Reforma.Impl
{
    using System;
    using System.ServiceModel;
    using System.ServiceModel.Channels;
    using System.ServiceModel.Description;
    using System.ServiceModel.Dispatcher;

    using Bars.Gkh.Reforma.Utils;

    /// <summary>
    ///     Расковыриватель сообщений, заменяющий шаблон на актуальный адрес.
    ///     Нужен для незаметного переключения между версиями сервиса Реформы
    /// </summary>
    public class NamespaceFixingBehavior : IClientMessageInspector, IEndpointBehavior
    {
        #region Constants

        /// <summary>
        ///     Шаблон
        /// </summary>
        public const string PlaceholderName = "ACTUAL_ADDRESS_PLACEHOLDER";

        #endregion

        #region Fields

        private readonly string actualAddress;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Конструктор
        /// </summary>
        /// <param name="actualAddress">Актуальный адрес сервиса</param>
        public NamespaceFixingBehavior(string actualAddress)
        {
            this.actualAddress = actualAddress;
        }

        #endregion

        #region Methods

        private Message ModifyMessage(Message message, bool reply)
        {
            var content = WcfUtils.MessageToString(ref message);
            if (content == null)
            {
                throw new Exception("Не удалось получить текст сообщения");
            }

            content = reply ? content.Replace(actualAddress, PlaceholderName) : content.Replace(PlaceholderName, actualAddress);

            return WcfUtils.CreateMessageFromString(content, message.Version);
        }

        #endregion

        #region Explicit Interface Methods

        void IClientMessageInspector.AfterReceiveReply(ref Message reply, object correlationState)
        {
            reply = ModifyMessage(reply, true);
        }

        object IClientMessageInspector.BeforeSendRequest(ref Message request, IClientChannel channel)
        {
            request = ModifyMessage(request, false);
            return null;
        }

        void IEndpointBehavior.AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
        {
        }

        void IEndpointBehavior.ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {
            clientRuntime.ClientMessageInspectors.Add(this);
        }

        void IEndpointBehavior.ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
        {
        }

        void IEndpointBehavior.Validate(ServiceEndpoint endpoint)
        {
        }

        #endregion
    }
}