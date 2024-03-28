namespace Bars.Gkh.RegOperator.Interceptors
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.Gkh.RegOperator.Entities.Dict;

    public class FederalStandardFeeCrInterceptor : EmptyDomainInterceptor<FederalStandardFeeCr>
    {
        public override IDataResult BeforeCreateAction(IDomainService<FederalStandardFeeCr> service, FederalStandardFeeCr entity)
        {
            if (service.GetAll().Any(x => x.DateStart >= entity.DateStart))
            {
                return this.Failure("Внимание! Дата начала действия нового значения не может быть раньше уже действующего параметра!");
            }

            this.SetDateEnd(service, entity);
            return this.Success();
        }

        public override IDataResult BeforeUpdateAction(IDomainService<FederalStandardFeeCr> service, FederalStandardFeeCr entity)
        {
            if (entity.DateStart > entity.DateEnd)
            {
                return this.Failure("Внимание! Дата начала действия не может быть позже даты окончания!");
            }

            var coversAnotherPeriod = service.GetAll()
                .Where(x => x.Id != entity.Id)
                .WhereIf(entity.DateEnd.HasValue, x => x.DateStart <= entity.DateEnd)
                .Count(x => x.DateStart >= entity.DateStart) > 0;

            if (coversAnotherPeriod)
            {
                return this.Failure("Внимание! Дата начала действия значения должна быть позже даты начала предыдущего!");
            }

            this.SetDateEnd(service, entity);
            return this.Success();
        }

        private void SetDateEnd(IDomainService<FederalStandardFeeCr> service, FederalStandardFeeCr entity)
        {
            var lastItem = service.GetAll().Where(x => x.DateStart < entity.DateStart)
                                   .OrderByDescending(x => x.DateStart)
                                   .ThenByDescending(x => x.DateEnd)
                                   .FirstOrDefault();

            if (lastItem != null)
            {
                lastItem.DateEnd = entity.DateStart.AddDays(-1);

                Container.Resolve<IRepository<FederalStandardFeeCr>>().Update(lastItem);
            }
        }
    }
}