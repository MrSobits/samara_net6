namespace Bars.Gkh.RegOperator.Utils
{
    using Enums;

    public static class EnumExtentions
    {
        public static bool IsIncome(this PaymentOperationType operationType)
        {
            return operationType != PaymentOperationType.OutcomeAccountPayment
                   && operationType != PaymentOperationType.ExpenseLoan
                   && operationType != PaymentOperationType.OutcomeLoan
                   && operationType != PaymentOperationType.CashService
                   && operationType != PaymentOperationType.OpeningAcc;
        }
    }
}