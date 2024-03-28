using Bars.KP60.Protocol.Entities;

namespace Bars.Gkh.Gis.DomainService.CalcVerification.Impl
{
    using System;
    using System.Data;
    using B4;
    using Castle.MicroKernel.Registration;
    using Castle.Windsor;
    using Intf;
    using KP60.Protocol.DomainService;
    using KP60.Protocol.DomainService.Impl;
    using KP60.Protocol.Entities;
    using Npgsql;

    public class InstanceProtocolHandler : IDisposable
    {
        public readonly IWindsorContainer Container;
        public IDbConnection ConnectionCHD;
        private InstanceProtocolHandler()
        {
        }

        public InstanceProtocolHandler(BaseParams baseParam)
        {
            var chdConnectionString = baseParam.Params.GetAs<string>("ChdConnectionString");

            Container = new WindsorContainer();
            ConnectionCHD = new NpgsqlConnection(chdConnectionString);
            if (ConnectionCHD.State != ConnectionState.Open)
            {
                ConnectionCHD.Open();
            }

            Container.Register(Component.For<IWindsorContainer>().UsingFactoryMethod(() => Container));
            Container.Register(Component.For<BillingInstrumentary>().ImplementedBy<BillingInstrumentary>());
            Container.Register(Component.For<ICalcVerificationProtocol>().ImplementedBy<CalcVerificationProtocol>());
            Container.Register(Component.For<IProtocolService>().ImplementedBy<ProtocolService>());

            Container.Register(Component.For<IDbConnection>().UsingFactoryMethod(() => ConnectionCHD));
        }

        public string GetProtocol(BaseParams baseParams)
        {
            return Container.Resolve<ICalcVerificationProtocol>().GetProtocol(baseParams);
        }

        public TreeData GetTree(BaseParams baseParams)
        {
            return Container.Resolve<IProtocolService>().GetTree(baseParams);
        }

        public void Dispose()
        {
            Container.Dispose();
            ConnectionCHD.Close();
            ConnectionCHD = null;
        }
    }
}
