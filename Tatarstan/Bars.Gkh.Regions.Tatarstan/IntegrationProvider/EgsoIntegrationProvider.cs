namespace Bars.Gkh.Regions.Tatarstan.IntegrationProvider
{
    using System;
    using System.ServiceModel;
    using System.ServiceModel.Description;

    using Bars.Gkh.Gis.ConfigSections;
    using Bars.Gkh.Regions.Tatarstan.Ias.Tatar.IndicatorService;
    using Bars.Gkh.Utils;

    using Castle.Windsor;

    public class EgsoIntegrationProvider : IIntegrationProvider<IndicatorClient>
    {
        /// <inheritdoc />
        public IWindsorContainer Container { get; set; }

        /// <inheritdoc />
        public IndicatorClient GetSoapClient()
        {
            var egsoIntegrationConfig = this.Container.GetGkhConfig<EgsoConfig>()?.EgsoIntegrationConfig;

            var binding = new BasicHttpsBinding
            {
                Security =
                {
                    Mode = BasicHttpsSecurityMode.Transport,
                    Transport = new HttpTransportSecurity
                    {
                        ClientCredentialType = HttpClientCredentialType.Basic
                    }
                },
                MaxReceivedMessageSize = 2147483647,
                SendTimeout = TimeSpan.FromMinutes(6),
                ReceiveTimeout = TimeSpan.FromMinutes(6)
            };

            var remoteAddress = new EndpointAddress(egsoIntegrationConfig.ServiceAddress);
            var client = new IndicatorClient(binding, remoteAddress);
            client.Endpoint.EndpointBehaviors.Remove(typeof(ClientCredentials));

            var loginCredentials = new ClientCredentials();
            loginCredentials.UserName.UserName = egsoIntegrationConfig.Login;
            loginCredentials.UserName.Password = egsoIntegrationConfig.Password;
            client.Endpoint.EndpointBehaviors.Add(loginCredentials);
            
            return client;
        }
    }
}