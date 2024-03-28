namespace Bars.GisIntegration.Smev
{
    using System.ServiceModel;

    using Bars.GisIntegration.Base.Enums;
    using Bars.GisIntegration.Base.GisServiceProvider;
    using Bars.GisIntegration.Smev.SmevExchangeService;

    /// <summary>
    /// Поставщик объектов для работы с шиной СМЭВ 3.0
    /// </summary>
    public class SmevServiceProvider : BaseGisServiceProvider<ServiceConsumerClient, ServiceConsumer>
    {
        /// <inheritdoc />
        protected override string DefaultUrl => "http://localhost/ServiceConsumer";

        /// <inheritdoc />
        public override IntegrationService IntegrationService => IntegrationService.Smev3;

        /// <inheritdoc />
        protected override ServiceConsumerClient GetClient(BasicHttpBinding binding, EndpointAddress remoteAddress)
        {
            return new ServiceConsumerClient(binding, remoteAddress);
        }
    }
}