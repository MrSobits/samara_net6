namespace Bars.Gkh.SystemDataTransfer.Utils
{
    using System.ServiceModel.Channels;
    using System.ServiceModel.Description;
    using System.ServiceModel.Dispatcher;

    using Bars.B4.Config;
    using Bars.Gkh.Services.ServiceContracts.DataTransfer;
    using Bars.Gkh.SystemDataTransfer.Meta.Services;

    /// <summary>
    /// Расширение WCF для добавления модуля авторизация к сервису интеграции
    /// </summary>
    public class AuthHeaderEndpointBehavior : IEndpointBehavior
    {
        private readonly IConfigProvider configProvider;
        private DataTransferIntegrationConfigs configs => new DataTransferIntegrationConfigs(this.configProvider);

        public AuthHeaderEndpointBehavior(IConfigProvider configProvider)
        {
            this.configProvider = configProvider;
        }

        /// <inheritdoc />
        public void Validate(ServiceEndpoint endpoint)
        {
        }

        /// <inheritdoc />
        public void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
        {
        }

        /// <inheritdoc />
        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
        {
            if (endpoint.Contract.ContractType == typeof(IDataTransferService))
            {
                // TODO wcf
                // endpointDispatcher.DispatchRuntime.MessageInspectors.Add(new AuthTokenHeaderInspector(this.configs));
            }
        }

        /// <inheritdoc />
        public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {
            if (endpoint.Contract.ContractType == typeof(IDataTransferService))
            {
                // TODO wcf
                // clientRuntime.MessageInspectors.Add(new AuthTokenHeaderInspector(this.configs));
            }
        }
    }
}