namespace Bars.Gkh.RegOperator.Interceptors
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.RegOperator.Entities;

    public class CashPaymentCenterManOrgRoInterceptor : EmptyDomainInterceptor<CashPaymentCenterManOrgRo>
    {
        public override IDataResult BeforeCreateAction(IDomainService<CashPaymentCenterManOrgRo> service, CashPaymentCenterManOrgRo entity)
        {
            if (entity.DateEnd.HasValue && entity.DateEnd.Value < entity.DateStart)
            {
                return Failure("Дата исключения не может быть раньше даты включения");
            }

            return CheckDates(service, entity);
        }

        public override IDataResult BeforeUpdateAction(IDomainService<CashPaymentCenterManOrgRo> service, CashPaymentCenterManOrgRo entity)
        {
            if (entity.DateEnd.HasValue && entity.DateEnd.Value < entity.DateStart.Date)
            {
                return Failure("Дата исключения не может быть раньше даты включения");
            }

            return CheckDates(service, entity);
        }

        // проверка пересечений с другими договорами по дому
        private IDataResult CheckDates(IDomainService<CashPaymentCenterManOrgRo> service, CashPaymentCenterManOrgRo entity)
        {
            var isAddedRoInPeriod =
                service.GetAll()
                    .Where(x => x.RealityObject.Id == entity.RealityObject.Id)
                    .Where(x => (x.DateStart <= entity.DateStart && (!x.DateEnd.HasValue || x.DateEnd >= entity.DateStart))
                        || (!entity.DateEnd.HasValue && x.DateStart >= entity.DateStart)
                        || (entity.DateEnd.HasValue && x.DateStart <= entity.DateEnd && (!x.DateEnd.HasValue || x.DateEnd >= entity.DateEnd)))
                    .Any(x => x.Id != entity.Id);

            return isAddedRoInPeriod
                ? Failure(string.Format(
                    "У дома по адресу {0} есть действующий договор в указанном периоде.",
                    entity.RealityObject.Address)) 
                : Success();
        }
    }
}