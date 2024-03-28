namespace Bars.Gkh.RegOperator.Interceptors
{
    using B4;
    using Entities;
    using Enums;

    public class RealityObjectPaymentAccountOperationInterceptor : EmptyDomainInterceptor<RealityObjectPaymentAccountOperation>
    {
        public override IDataResult BeforeCreateAction(IDomainService<RealityObjectPaymentAccountOperation> service, RealityObjectPaymentAccountOperation entity)
        {
            ChangeAccountSum(entity.Account, entity.OperationType, entity.OperationSum);

            return base.BeforeCreateAction(service, entity);
        }

        private void ChangeAccountSum(RealityObjectPaymentAccount account, PaymentOperationType operationType, decimal operationSum)
        {
            if (operationType == PaymentOperationType.OutcomeAccountPayment
                || operationType == PaymentOperationType.ExpenseLoan
                || operationType == PaymentOperationType.OutcomeLoan
                || operationType == PaymentOperationType.CashService
                || operationType == PaymentOperationType.OpeningAcc
                || operationType == PaymentOperationType.CreditPayment
                || operationType == PaymentOperationType.CreditPercentPayment
                || operationType == PaymentOperationType.GuaranteesObtainPayment
                || operationType == PaymentOperationType.GuaranteesForCredit
                || operationType == PaymentOperationType.UndoPayment
                || operationType == PaymentOperationType.CancelPayment)
            {
                account.CreditTotal += operationSum;
            }
            else
            {
                account.DebtTotal += operationSum;
            }
        }

        public override IDataResult BeforeDeleteAction(IDomainService<RealityObjectPaymentAccountOperation> service, RealityObjectPaymentAccountOperation entity)
        {
            if (entity.OperationType == PaymentOperationType.OutcomeAccountPayment
                || entity.OperationType == PaymentOperationType.ExpenseLoan
                || entity.OperationType == PaymentOperationType.OutcomeLoan
                || entity.OperationType == PaymentOperationType.CashService
                || entity.OperationType == PaymentOperationType.OpeningAcc)
            {
                entity.Account.CreditTotal -= entity.OperationSum;
            }
            else
            {
                entity.Account.DebtTotal -= entity.OperationSum;
            }

            return Success();
        }
    }
}