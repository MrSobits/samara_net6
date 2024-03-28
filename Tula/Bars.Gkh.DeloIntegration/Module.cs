namespace Bars.Gkh.DeloIntegration
{
    using System;
    using System.ServiceModel;
    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.Gkh.DeloIntegration.Wcf.Services;

    using Castle.Facilities.WcfIntegration;
    using Castle.MicroKernel.Registration;
    using Bars.Gkh.DeloIntegration.DomainService;

    public class Module : AssemblyDefinedModule
    {
        public override void Install()
        {
            Container.RegisterTransient<IResourceManifest, ResourceManifest>("Gkh.Delo resources");

            Container.RegisterTransient<IDeloIntegrationService, DeloIntegrationService>();

            Component.For<IDeloService>().ImplementedBy<DeloService>()
                .AsWcfService(
                        new DefaultServiceModel(
                            WcfEndpoint
                                .BoundTo(
                                    new BasicHttpBinding
                                    {
                                        MaxReceivedMessageSize = int.MaxValue,
                                        OpenTimeout = TimeSpan.MaxValue,
                                        CloseTimeout = TimeSpan.MaxValue,
                                        ReceiveTimeout = TimeSpan.MaxValue
                                    })
                            )
                            .Hosted())
                    .RegisterIn(Container);
        }
    }
}
                