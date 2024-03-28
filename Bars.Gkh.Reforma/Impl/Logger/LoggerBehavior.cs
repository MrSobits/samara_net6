namespace Bars.Gkh.Reforma.Impl.Logger
{
    using System.ServiceModel;
    using System.ServiceModel.Channels;
    using System.ServiceModel.Description;
    using System.ServiceModel.Dispatcher;

    using Bars.Gkh.Reforma.Interface.Logger;
    using Bars.Gkh.Reforma.Utils;

    /// <summary>
    ///     Перехватывает все вызовы клиента и логирует их
    /// </summary>
    public class LoggerBehavior : IEndpointBehavior, IClientMessageInspector
    {
        #region Fields

        private readonly ISyncLogger logger;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Конструктор
        /// </summary>
        /// <param name="syncLogger">Логгер</param>
        public LoggerBehavior(ISyncLogger syncLogger)
        {
            logger = syncLogger;
        }

        #endregion

        #region Public Methods and Operators

        public void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
        {
        }

        public void AfterReceiveReply(ref Message reply, object correlationState)
        {
            logger.LogResponse(WcfUtils.MessageToString(ref reply));
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
            logger.LogRequest(WcfUtils.MessageToString(ref request));
            return null;
        }

        public void Validate(ServiceEndpoint endpoint)
        {
        }

        #endregion
    }
}