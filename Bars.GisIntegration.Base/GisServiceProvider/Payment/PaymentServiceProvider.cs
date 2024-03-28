namespace Bars.GisIntegration.Base.GisServiceProvider.Payment
{
    using System.ServiceModel;
    using Bars.GisIntegration.Base.Enums;
    using Bars.GisIntegration.Base.GisServiceProvider;
    using Bars.GisIntegration.Base.PaymentAsync;

    public class PaymentServiceProvider : BaseGisServiceProvider<PaymentPortsTypeAsyncClient, PaymentPortsTypeAsync>
    {
        protected override string DefaultUrl { get; } = "http://127.0.0.1:8080/ext-bus-payment-service/services/PaymentAsync";

        protected override bool IsAsync => true;

        /// <summary>Сервис, к которому относится провайдер</summary>
        public override IntegrationService IntegrationService { get; } = IntegrationService.Payment;

        protected override PaymentPortsTypeAsyncClient GetClient(BasicHttpBinding binding, EndpointAddress remoteAddress)
        {
            return new PaymentPortsTypeAsyncClient(binding, remoteAddress);
        }
    }
}
