namespace Bars.Gkh.RegOperator.DomainService
{
    using System;
    using B4;
    using Entities;

    public interface ICalcAccountCreditService
    {
        IDataResult ApplyCreditPayment(CalcAccountCredit credit, decimal creditSum, decimal percentSum);

        IDataResult UndoCreditPayment(CalcAccountCredit credit, decimal creditSum, decimal percentSum);
    }
}