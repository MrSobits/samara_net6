namespace Bars.GisIntegration.Base.GisServiceProvider.Inspection
{
    using System.ServiceModel;

    using Bars.GisIntegration.Base.Enums;
    using Bars.GisIntegration.Base.GisServiceProvider;
    using Bars.GisIntegration.Base.InspectionAsync;

    public class InspectionServiceProvider : BaseGisServiceProvider<InspectionPortsTypeAsyncClient, InspectionPortsTypeAsync>
    {
        protected override bool IsAsync => true;

        /// <summary>Сервис, к которому относится провайдер</summary>
        public override IntegrationService IntegrationService => IntegrationService.Inspection;

        protected override string DefaultUrl => "http://127.0.0.1:8080/ext-bus-inspection-service/services/InspectionAsync";

        protected override InspectionPortsTypeAsyncClient GetClient(BasicHttpBinding binding, EndpointAddress remoteAddress)
        {
            return new InspectionPortsTypeAsyncClient(binding, remoteAddress);
        }
    }
}
