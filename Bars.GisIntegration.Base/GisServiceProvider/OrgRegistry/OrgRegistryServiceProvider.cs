namespace Bars.GisIntegration.Base.GisServiceProvider.OrgRegistry
{
    using System.ServiceModel;

    using Bars.GisIntegration.Base.Enums;
    using Bars.GisIntegration.Base.GisServiceProvider;
    using Bars.GisIntegration.Base.OrgRegistryAsync;

    /// <summary>
    /// Сервис провайдер для сервиса OrgRegistryAsync
    /// </summary>
    public class OrgRegistryServiceProvider : BaseGisServiceProvider<RegOrgPortsTypeAsyncClient, RegOrgPortsTypeAsync>
    {
        protected override bool IsAsync => true;

        /// <summary>Сервис, к которому относится провайдер</summary>
        public override IntegrationService IntegrationService => IntegrationService.OrgRegistry;

        protected override string DefaultUrl => "http://127.0.0.1:8080/ext-bus-org-registry-common-service/services/OrgRegistryAsync";

        /// <summary>
        /// Метод получения Soap-клиента
        /// </summary>
        /// <param name="binding">Http-биндинг</param>
        /// <param name="remoteAddress">Endpoint адрес</param>
        /// <returns></returns>
        protected override RegOrgPortsTypeAsyncClient GetClient(BasicHttpBinding binding, EndpointAddress remoteAddress)
        {
            return new RegOrgPortsTypeAsyncClient(binding, remoteAddress);
        }
    }
}
