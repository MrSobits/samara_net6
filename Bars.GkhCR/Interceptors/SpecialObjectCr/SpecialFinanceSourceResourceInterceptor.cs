namespace Bars.GkhCr.Interceptors
{
    using B4;
    using Entities;

    public class SpecialFinanceSourceResourceInterceptor : SpecialFinanceSourceResourceInterceptor<SpecialFinanceSourceResource>
    {
        // Внимание !!! override делать в Generic классе
    }

    public class SpecialFinanceSourceResourceInterceptor<T> : EmptyDomainInterceptor<T>
        where T : SpecialFinanceSourceResource
    {
        public override IDataResult BeforeCreateAction(IDomainService<T> service,
            T entity)
        {
            return this.CheckIncomeSum(entity);
        }

        public override IDataResult BeforeUpdateAction(IDomainService<T> service,
            T entity)
        {
            return this.CheckIncomeSum(entity);
        }

        private IDataResult CheckIncomeSum(SpecialFinanceSourceResource entity)
        {
            if (entity.BudgetMuIncome > entity.BudgetMu)
            {
                return this.Failure("Поступление не может превышать бюджет МО");
            }

            if (entity.BudgetSubjectIncome > entity.BudgetSubject)
            {
                return this.Failure("Поступление не может превышать бюджет субъекта");
            }

            if (entity.FundResourceIncome > entity.FundResource)
            {
                return this.Failure("Поступление не может превышать средства фонда");
            }

            return this.Success();

        }
    }
}