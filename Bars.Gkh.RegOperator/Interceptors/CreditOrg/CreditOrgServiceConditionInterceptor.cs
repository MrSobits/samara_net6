namespace Bars.Gkh.RegOperator.Interceptors
{
    using System.Linq;

    using B4;
    using Entities;

    public class CreditOrgServiceConditionInterceptor : EmptyDomainInterceptor<CreditOrgServiceCondition>
    {
        public override IDataResult BeforeCreateAction(IDomainService<CreditOrgServiceCondition> service, CreditOrgServiceCondition entity)
        {
            var creditOrgCount = service.GetAll().Count(x => x.CreditOrg.Id == entity.CreditOrg.Id);

            if (creditOrgCount >= 1)
            {
                return Failure("Наименование должно быть уникальным");
            }

            return CheckDates(service, entity);
        }

        public override IDataResult BeforeUpdateAction(IDomainService<CreditOrgServiceCondition> service, CreditOrgServiceCondition entity)
        {
            return CheckDates(service, entity);
        }

        private IDataResult CheckDates(IDomainService<CreditOrgServiceCondition> service, CreditOrgServiceCondition entity)
        {
            if (entity.CashServiceDateFrom > entity.CashServiceDateTo
                || entity.OpeningAccDateFrom > entity.OpeningAccDateTo)
            {
                return Failure("Дата начала периода не может превышать дату окончания периода");
            }

            var creditOrgServConds =
                service.GetAll()
                    .Where(x => x.Id != entity.Id)
                    .Where(x => x.CreditOrg.Id == entity.CreditOrg.Id)
                    .ToList();

            if (creditOrgServConds
                .Where(x => x.CashServiceDateFrom <= entity.CashServiceDateFrom)
                .Any(x => !x.CashServiceDateTo.HasValue || x.CashServiceDateTo >= entity.CashServiceDateTo))
            {
                return Failure("На данный период времени Размер обслуживания уже задан");
            }

            if (creditOrgServConds
                    .Where(x => x.OpeningAccDateFrom <= entity.OpeningAccDateFrom)
                    .Any(x => !x.OpeningAccDateTo.HasValue || x.OpeningAccDateTo >= entity.OpeningAccDateTo))
            {
                return Failure("На данный период времени Плата за открытие уже задана");
            }

            return Success();
        }
    }
}