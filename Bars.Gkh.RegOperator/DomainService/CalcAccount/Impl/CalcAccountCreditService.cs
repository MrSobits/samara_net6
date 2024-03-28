namespace Bars.Gkh.RegOperator.DomainService.Impl
{
    using System;
    using B4;
    using Castle.Windsor;
    using Entities;
    using Gkh.Domain;

    public class CalcAccountCreditService : ICalcAccountCreditService
    {
        public IWindsorContainer Container { get; set; }

        public IDomainService<CalcAccountCredit> Domain { get; set; }

        public IDataResult ApplyCreditPayment(CalcAccountCredit credit, decimal creditSum, decimal percentSum)
        {
            Container.InTransaction(() =>
            {
                credit.CreditDebt -= creditSum;
                credit.PercentDebt -= percentSum;

                if (credit.CreditDebt == 0 && credit.PercentDebt == 0)
                {
                    credit.DateEnd = DateTime.Today;
                }

                Domain.Update(credit);
            });

            return new BaseDataResult();
        }

        public IDataResult UndoCreditPayment(CalcAccountCredit credit, decimal creditSum, decimal percentSum)
        {
            Container.InTransaction(() =>
            {
                credit.CreditDebt += creditSum;
                credit.PercentDebt += percentSum;

                if (credit.CreditDebt > 0 || credit.PercentDebt > 0)
                {
                    credit.DateEnd = null;
                }

                Domain.Update(credit);
            });

            return new BaseDataResult();
        }
    }
}