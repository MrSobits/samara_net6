namespace Bars.GisIntegration.Base.GisServiceProvider.CapitalRepair
{
    using System.ServiceModel;
    using Bars.GisIntegration.Base.CapitalRepair;
    using Bars.GisIntegration.Base.Enums;
    using Bars.GisIntegration.Base.GisServiceProvider;

    /// <summary>
    /// Провайдер сервиса CapitalRepairAsync
    /// </summary>
    public class CapitalRepairAsyncServiceProvider : BaseGisServiceProvider<CapitalRepairAsyncPortClient, CapitalRepairAsyncPort>
    {
        /// <summary>
        /// Сервис, к которому относится провайдер
        /// </summary>
        public override IntegrationService IntegrationService => IntegrationService.CapitalRepair;

        /// <summary>
        /// Получить клиент для работы с сервисом
        /// </summary>
        protected override CapitalRepairAsyncPortClient GetClient(BasicHttpBinding binding, EndpointAddress remoteAddress)
        {
            return new CapitalRepairAsyncPortClient(binding, remoteAddress);
        }

        /// <summary>
        /// Признак асинхронности
        /// </summary>
        protected override bool IsAsync => true;

        /// <summary>
        /// URL по-умолчанию
        /// </summary>
        protected override string DefaultUrl => "http://127.0.0.1:8080/ext-bus-capital-repair-programs-service/services/CapitalRepairAsync";
    }
}
