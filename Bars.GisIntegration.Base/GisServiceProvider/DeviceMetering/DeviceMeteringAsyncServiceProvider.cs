namespace Bars.GisIntegration.Base.GisServiceProvider.DeviceMetering
{
    using System.ServiceModel;

    using Bars.GisIntegration.Base.DeviceMeteringAsync;
    using Bars.GisIntegration.Base.Enums;
    using Bars.GisIntegration.Base.GisServiceProvider;

    public class DeviceMeteringAsyncServiceProvider : BaseGisServiceProvider<DeviceMeteringPortTypesAsyncClient, DeviceMeteringPortTypesAsync>
    {
        protected override bool IsAsync => true;

        /// <summary>Сервис, к которому относится провайдер</summary>
        public override IntegrationService IntegrationService => IntegrationService.DeviceMetering;

        protected override string DefaultUrl => "http://127.0.0.1:8080/ext-bus-device-metering-service/services/DeviceMeteringAsync";

        protected override DeviceMeteringPortTypesAsyncClient GetClient(BasicHttpBinding binding, EndpointAddress remoteAddress)
        {
            return new DeviceMeteringPortTypesAsyncClient(binding, remoteAddress);
        }
    }
}
