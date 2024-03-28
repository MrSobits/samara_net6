namespace Bars.GisIntegration.Base.GisServiceProvider.Nsi
{
    using System.ServiceModel;

    using Bars.GisIntegration.Base.Enums;
    using Bars.GisIntegration.Base.GisServiceProvider;
    using Bars.GisIntegration.Base.NsiAsync;

    public class NsiServiceProvider : BaseGisServiceProvider<NsiPortsTypeAsyncClient, NsiPortsTypeAsync>
    {
        protected override bool IsAsync => true;

        /// <summary>Сервис, к которому относится провайдер</summary>
        public override IntegrationService IntegrationService => IntegrationService.Nsi;

        protected override string DefaultUrl => "http://127.0.0.1:8080/ext-bus-nsi-service/services/NsiAsync";

        protected override NsiPortsTypeAsyncClient GetClient(BasicHttpBinding binding, EndpointAddress remoteAddress)
        {
            return new NsiPortsTypeAsyncClient(binding, remoteAddress);
        }
    }
}