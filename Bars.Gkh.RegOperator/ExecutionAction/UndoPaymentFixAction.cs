namespace Bars.Gkh.RegOperator.ExecutionAction
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.Gkh.ExecutionAction;
    using Bars.Gkh.RegOperator.Entities;

    public class UndoPaymentFixAction : BaseExecutionAction
    {
        public override string Description => "Исправление ошибки с отрицательными суммами в оплатах";

        public override string Name => "Исправление ошибки с отрицательными суммами в оплатах";

        public override Func<IDataResult> Action => this.Execute;

        private BaseDataResult Execute()
        {
            var paymentOperDomain = this.Container.ResolveDomain<RealityObjectPaymentAccountOperation>();
            var paymentAccDomain = this.Container.ResolveDomain<RealityObjectPaymentAccount>();
            var sessionProvider = this.Container.Resolve<ISessionProvider>();
            var unProxy = this.Container.Resolve<IUnProxy>();

            try
            {
                var opers = paymentOperDomain.GetAll()
                    .Where(x => x.OperationSum < 0)
                    .ToList();

                var accounts = paymentAccDomain.GetAll()
                    .Where(x => paymentOperDomain.GetAll().Any(y => y.OperationSum < 0 && x.Id == y.Account.Id))
                    .ToDictionary(x => x.Id);

                foreach (var operation in opers)
                {
                    var acc = accounts.Get(operation.Account.Id);

                    operation.OperationSum = -1 * operation.OperationSum;

                    acc.CreditTotal += operation.OperationSum * 2;
                }

                var session = sessionProvider.OpenStatelessSession();

                using (var transaction = session.BeginTransaction())
                {
                    try
                    {
                        opers.ForEach(session.Update);
                        accounts.Values.ForEach(x => session.Update(unProxy.GetUnProxyObject(x)));

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
                this.Container.Release(paymentOperDomain);
                this.Container.Release(paymentAccDomain);
                this.Container.Release(sessionProvider);
                this.Container.Release(unProxy);
            }
        }
    }
}