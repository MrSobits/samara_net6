namespace Bars.GisIntegration.Base.GisServiceProvider.HouseManagement
{
    using System.ServiceModel;

    using Bars.GisIntegration.Base.Enums;
    using Bars.GisIntegration.Base.GisServiceProvider;
    using Bars.GisIntegration.Base.HouseManagementAsync;

    public class HouseManagementServiceProvider : BaseGisServiceProvider<HouseManagementPortsTypeAsyncClient, HouseManagementPortsTypeAsync>
    {
        protected override bool IsAsync => true;

        /// <summary>Сервис, к которому относится провайдер</summary>
        public override IntegrationService IntegrationService => IntegrationService.HouseManagement;

        protected override string DefaultUrl => "http://127.0.0.1:8080/ext-bus-home-management-service/services/HomeManagementAsync";

        protected override HouseManagementPortsTypeAsyncClient GetClient(BasicHttpBinding binding, EndpointAddress remoteAddress)
        {
            return new HouseManagementPortsTypeAsyncClient(binding, remoteAddress);
        }
    }
}
