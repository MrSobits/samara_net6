namespace Bars.Gkh.Gis
{
    using System;
    using System.ServiceModel;

    using Castle.MicroKernel.Registration;
    using Services.ServiceContracts.OpenTatarstan;

    public partial class Module
    {
        public void RegisterWcfRiaServices()
        {
            //var otBasicHttpBinding = new BasicHttpBinding
            //{
            //    Security =
            //    {
            //        Mode = BasicHttpSecurityMode.TransportCredentialOnly,
            //        Transport = { ProxyCredentialType = HttpProxyCredentialType.Basic }
            //    },
            //    UseDefaultWebProxy = false,
            //    ProxyAddress = new Uri(string.Format("http://{0}:{1}", "192.168.224.24", 3128))
            //};

            //Container
            //    .Register(
            //        Types
            //            .FromAssemblyContaining<IOpenTatarstanService>()
            //            .InSameNamespaceAs<IOpenTatarstanService>()
            //            .Configure(
            //                c => c.Named(c.Implementation.Name)
            //                    .AsWcfClient(new DefaultClientModel
            //                    {
            //                        Endpoint = WcfEndpoint
            //                            .BoundTo(otBasicHttpBinding)
            //                            .At("http://demo-ias.e-kazan.ru/service/wsdl")
            //                    }
            //                    .Credentials(new UserNameCredentials("117642", "OpaTaz09"))
            //                    )));
        }
    }
}