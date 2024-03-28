namespace Bars.GisIntegration.Base.GisServiceProvider.NsiCommon
{
    using System.ServiceModel;
    
    using Bars.GisIntegration.Base.Enums;
    using Bars.GisIntegration.Base.NsiCommon;

    /// <summary>
    /// Service provider для сервиса общей НСИ
    /// </summary>
    public class NsiCommonServiceProvider : BaseGisServiceProvider<NsiPortsTypeClient, NsiPortsType>
    {
        /// <summary>
        /// Значение перечисления, соответствующее сервису
        /// </summary>
        public override IntegrationService IntegrationService => IntegrationService.NsiCommon;

        /// <summary>
        /// Получить клиент для работы с сервисом
        /// </summary>
        protected override NsiPortsTypeClient GetClient(BasicHttpBinding binding, EndpointAddress remoteAddress)
        {
            return new NsiPortsTypeClient(binding, remoteAddress);
        }

        /// <summary>
        /// Использует асинхронный сервис
        /// </summary>
        protected override bool IsAsync => false;

        /// <summary>
        /// Url по-умолчанию
        /// </summary>
        protected override string DefaultUrl => "http://127.0.0.1:8080/ext-bus-nsi-common-service/services/NsiCommon";
    }
}
