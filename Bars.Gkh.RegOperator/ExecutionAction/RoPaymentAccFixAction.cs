namespace Bars.Gkh.RegOperator.ExecutionAction
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.Gkh.ExecutionAction;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Enums;

    public class RoPaymentAccFixAction : BaseExecutionAction
    {
        public static string Code = "RoPaymentAccFixAction";

        public override string Description => "Исправление суммы счета оплат по операциям";

        public override string Name => "Исправление суммы счета оплат по операциям";

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
                    .ToList();

                var accounts = paymentAccDomain.GetAll()
                    .ToDictionary(x => x.Id);

                foreach (var account in accounts)
                {
                    account.Value.DebtTotal = 0;
                    account.Value.CreditTotal = 0;
                }

                foreach (var operation in opers)
                {
                    var acc = accounts.Get(operation.Account.Id);

                    if (operation.OperationType == PaymentOperationType.OutcomeAccountPayment
                        || operation.OperationType == PaymentOperationType.ExpenseLoan
                        || operation.OperationType == PaymentOperationType.OutcomeLoan
                        || operation.OperationType == PaymentOperationType.CashService
                        || operation.OperationType == PaymentOperationType.OpeningAcc
                        || operation.OperationType == PaymentOperationType.CreditPayment
                        || operation.OperationType == PaymentOperationType.CreditPercentPayment
                        || operation.OperationType == PaymentOperationType.GuaranteesObtainPayment
                        || operation.OperationType == PaymentOperationType.GuaranteesForCredit
                        || operation.OperationType == PaymentOperationType.UndoPayment
                        || operation.OperationType == PaymentOperationType.CancelPayment)
                    {
                        acc.CreditTotal += operation.OperationSum;
                    }
                    else
                    {
                        acc.DebtTotal += operation.OperationSum;
                    }
                }

                var session = sessionProvider.OpenStatelessSession();

                using (var transaction = session.BeginTransaction())
                {
                    try
                    {
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