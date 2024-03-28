namespace Bars.Gkh.RegOperator.ExecutionAction
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain.CollectionExtensions;
    using Bars.Gkh.ExecutionAction;
    using Bars.Gkh.RegOperator.Entities;

    public class UnacceptedPaymentPacketSumFixAction : BaseExecutionAction
    {
        public override string Description => "Обновление суммы пакета непотвержденных оплат";

        public override string Name => "Обновление суммы пакета непотвержденных оплат";

        public override Func<IDataResult> Action => this.Execute;

        private BaseDataResult Execute()
        {
            var unacceptedPaymentPacketDomain = this.Container.ResolveDomain<UnacceptedPaymentPacket>();
            var unacceptedPaymentDomain = this.Container.ResolveDomain<UnacceptedPayment>();
            var sessionProvider = this.Container.Resolve<ISessionProvider>();

            try
            {
                var dictSum = unacceptedPaymentDomain.GetAll()
                    .Select(
                        x => new
                        {
                            x.Packet.Id,
                            x.Sum
                        })
                    .ToList()
                    .GroupBy(x => x.Id)
                    .ToDictionary(x => x.Key, x => x.SafeSum(y => y.Sum));

                var session = sessionProvider.OpenStatelessSession();

                using (var transaction = session.BeginTransaction())
                {
                    try
                    {
                        unacceptedPaymentPacketDomain.GetAll()
                            .ToList()
                            .ForEach(
                                x =>
                                {
                                    var sum = dictSum.Get(x.Id);
                                    if (x.Sum != sum)
                                    {
                                        x.Sum = sum;
                                        session.Update(x);
                                    }
                                });

                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }

                return new BaseDataResult();
            }
            finally
            {
                this.Container.Release(unacceptedPaymentPacketDomain);
                this.Container.Release(unacceptedPaymentDomain);
                this.Container.Release(sessionProvider);
            }
        }
    }
}