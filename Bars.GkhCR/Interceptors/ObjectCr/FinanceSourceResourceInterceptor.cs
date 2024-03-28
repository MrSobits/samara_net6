namespace Bars.GkhCr.Interceptors
{
    using B4;
    using Entities;

    public class FinanceSourceResourceInterceptor : FinanceSourceResourceInterceptor<FinanceSourceResource>
    {
        // Внимание !!! override делать в Generic классе
    }

    public class FinanceSourceResourceInterceptor<T> : EmptyDomainInterceptor<T>
        where T : FinanceSourceResource
    {
        public override IDataResult BeforeCreateAction(IDomainService<T> service,
            T entity)
        {
            return CheckIncomeSum(entity);
        }

        public override IDataResult BeforeUpdateAction(IDomainService<T> service,
            T entity)
        {
            return CheckIncomeSum(entity);
        }

        private IDataResult CheckIncomeSum(FinanceSourceResource entity)
        {
            if (entity.BudgetMuIncome > entity.BudgetMu)
            {
                return Failure("Поступление не может превышать бюджет МО");
            }

            if (entity.BudgetSubjectIncome > entity.BudgetSubject)
            {
                return Failure("Поступление не может превышать бюджет субъекта");
            }

            if (entity.FundResourceIncome > entity.FundResource)
            {
                return Failure("Поступление не может превышать средства фонда");
            }

            return Success();

        }
    }
}