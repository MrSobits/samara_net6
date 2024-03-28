namespace Bars.GisIntegration.Base.GisServiceProvider.Bills
{
    using System.ServiceModel;

    using Bars.GisIntegration.Base.BillsAsync;
    using Bars.GisIntegration.Base.Enums;
    using Bars.GisIntegration.Base.GisServiceProvider;

    /// <summary>
    /// Поставщик информации о сервисе BillsAsync
    /// </summary>
    public class BillsAsyncServiceProvider : BaseGisServiceProvider<BillsPortsTypeAsyncClient, BillsPortsTypeAsync>
    {
        protected override bool IsAsync => true;

        /// <summary>Сервис, к которому относится провайдер</summary>
        public override IntegrationService IntegrationService { get; } = IntegrationService.Bills;

        protected override string DefaultUrl => "http://127.0.0.1:8080/ext-bus-bills-service/services/BillsAsync";

        /// <summary>
        /// Получить soap клиент
        /// </summary>
        /// <param name="binding">Binding</param>
        /// <param name="remoteAddress">Адрес</param>
        /// <returns></returns>
        protected override BillsPortsTypeAsyncClient GetClient(BasicHttpBinding binding, EndpointAddress remoteAddress)
        {
            return new BillsPortsTypeAsyncClient(binding, remoteAddress);
        }
    }
}
