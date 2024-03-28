namespace Bars.GisIntegration.Base.GisServiceProvider.Infrastructure
{
    using System.ServiceModel;

    using Bars.GisIntegration.Base.Enums;
    using Bars.GisIntegration.Base.GisServiceProvider;
    using Bars.GisIntegration.Base.InfrastructureAsync;

    public class InfrastructureServiceProvider : BaseGisServiceProvider<InfrastructurePortsTypeAsyncClient, InfrastructurePortsTypeAsync>
    {
        protected override string DefaultUrl { get; } = "http://127.0.0.1:8080/ext-bus-rki-service/services/InfrastructureAsync";

        protected override bool IsAsync => true;

        /// <summary>Сервис, к которому относится провайдер</summary>
        public override IntegrationService IntegrationService { get; } = IntegrationService.Infrastructure;

        protected override InfrastructurePortsTypeAsyncClient GetClient(BasicHttpBinding binding, EndpointAddress remoteAddress)
        {
            return new InfrastructurePortsTypeAsyncClient(binding, remoteAddress);
        }
    }
}
