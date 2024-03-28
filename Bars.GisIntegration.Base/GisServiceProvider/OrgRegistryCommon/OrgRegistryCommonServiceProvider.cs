namespace Bars.GisIntegration.Base.GisServiceProvider.OrgRegistryCommon
{
    using System.ServiceModel;

    using Bars.GisIntegration.Base.Enums;
    using Bars.GisIntegration.Base.OrgRegistryCommonAsync;

    public class OrgRegistryCommonServiceProvider : BaseGisServiceProvider<RegOrgPortsTypeAsyncClient, RegOrgPortsTypeAsync>
    {
        protected override bool IsAsync => true;

        /// <summary>Сервис, к которому относится провайдер</summary>
        public override IntegrationService IntegrationService { get; } = IntegrationService.OrgRegistryCommon;
        protected override string DefaultUrl { get; } = "http://127.0.0.1:8080/ext-bus-org-registry-common-service/services/OrgRegistryCommonAsync";

        protected override RegOrgPortsTypeAsyncClient GetClient(BasicHttpBinding binding, EndpointAddress remoteAddress)
        {
            return new RegOrgPortsTypeAsyncClient(binding, remoteAddress);
        }
    }
}