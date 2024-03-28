namespace Bars.GisIntegration.Base.GisServiceProvider.Services
{
    using System.ServiceModel;

    using Bars.GisIntegration.Base.Enums;
    using Bars.GisIntegration.Base.GisServiceProvider;
    using Bars.GisIntegration.Base.ServicesAsync;

    public class ServicesServiceProvider : BaseGisServiceProvider<ServicesPortsTypeAsyncClient, ServicesPortsTypeAsync>
    {
        protected override bool IsAsync => true;

        /// <summary>Сервис, к которому относится провайдер</summary>
        public override IntegrationService IntegrationService { get; } = IntegrationService.Organization;

        protected override string DefaultUrl { get; } = "http://127.0.0.1:8080/ext-bus-organization-service/services/Organization";

        protected override ServicesPortsTypeAsyncClient GetClient(BasicHttpBinding binding, EndpointAddress remoteAddress)
        {
            return new ServicesPortsTypeAsyncClient(binding, remoteAddress);
        }
    }
}
