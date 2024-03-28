namespace Bars.Gkh.RegOperator.Tasks.Charges.Callbacks
{
    using System.Linq;
    using System.Reflection;
    using System.Threading;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Modules.Tasks.Common.Contracts.Result;
    using Bars.B4.Modules.Tasks.Common.Service;
    using Bars.B4.Utils;
    using Bars.Gkh.ConfigSections.RegOperator;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Enums;
    using Bars.Gkh.Utils.PerformanceLogging;

    using Castle.Windsor;

    using ExecutionContext = Bars.B4.Modules.Tasks.Common.Contracts.ExecutionContext;

    public class PersonalAccountChargeFailCallback : ITaskCallback
    {
        public static string Id = MethodBase.GetCurrentMethod().DeclaringType.FullName;

        private readonly IWindsorContainer container;
        private readonly ISessionProvider sessionProvider;
        private readonly IPerformanceLoggerFactory loggerFactory;

        public PersonalAccountChargeFailCallback(IWindsorContainer container, ISessionProvider sessionProvider, IPerformanceLoggerFactory loggerFactory)
        {
            this.container = container;
            this.sessionProvider = sessionProvider;
            this.loggerFactory = loggerFactory;
        }

        public CallbackResult Call(
            long taskId,
            BaseParams @params,
            ExecutionContext ctx,
            IProgressIndicator indicator,
            CancellationToken ct)
        {
            var cfg = @params.Params.GetAs<OperationLockConfig>("operationLock");
            var calcGuid = @params.Params.GetAs<string>("calcGuid");
            var packetId = @params.Params.GetAs<long>("packetId", ignoreCase: true);

            if (cfg != null && cfg.Enabled)
            {
                PersonalAccountChargeTableLocker.Unlock(this.container);
            }

            var logger = this.loggerFactory.GetLogger(calcGuid);
            var collector = this.loggerFactory.GetCollector();
            var packetDomain = this.container.ResolveDomain<UnacceptedChargePacket>();
            try
            {
                logger.SaveLogs(collector, x => x.OrderByDescending(y => y.TimeSpan).First());
                logger.ClearSession();

                var packet = packetDomain.Get(packetId);
                if (packet.IsNotNull())
                {
                    packet.PacketState = PaymentOrChargePacketState.Accepted;
                    packetDomain.Update(packet);
                }
            }
            finally
            {
                this.container.Release(logger);
                this.container.Release(collector);
                this.container.Release(packetDomain);
            }

            return new CallbackResult(true);
        }
    }
}