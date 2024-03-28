namespace Bars.Gkh.RegOperator.Interceptors
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.RegOperator.Entities;

    public class CashPaymentCenterRealObjInterceptor : EmptyDomainInterceptor<CashPaymentCenterRealObj>
    {
        public override IDataResult BeforeCreateAction(IDomainService<CashPaymentCenterRealObj> service, CashPaymentCenterRealObj entity)
        {
            return CheckDates(service, entity);
        }

        public override IDataResult BeforeUpdateAction(IDomainService<CashPaymentCenterRealObj> service, CashPaymentCenterRealObj entity)
        {
            return CheckDates(service, entity);
        }

        // проверка пересечений с другими договорами по дому
        private IDataResult CheckDates(IDomainService<CashPaymentCenterRealObj> service, CashPaymentCenterRealObj entity)
        {
            var isAddedRoInPeriod =
                service.GetAll()
                    .Where(x => x.RealityObject.Id == entity.RealityObject.Id)
                    .Where(x => (x.DateStart <= entity.DateStart && (!x.DateEnd.HasValue || x.DateEnd >= entity.DateStart))
                                || (!entity.DateEnd.HasValue && x.DateStart >= entity.DateStart)
                                || (entity.DateEnd.HasValue && x.DateStart <= entity.DateEnd && (!x.DateEnd.HasValue || x.DateEnd >= entity.DateEnd)))
                    .Any(x => x.Id != entity.Id);

            return isAddedRoInPeriod 
                ? Failure("У данного дома есть действующий договор в этом периоде. Для добавления нового договора необходимо закрыть прошлый") 
                : Success();
        }
    }
}