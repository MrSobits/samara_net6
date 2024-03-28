namespace Bars.Gkh.RegOperator.Interceptors
{
    using System.Linq;
    using B4.DataAccess;
    using B4.Utils;
    using Bars.B4;
    using Bars.Gkh.RegOperator.Entities;

    public class CashPaymentCenterManOrgInterceptor : EmptyDomainInterceptor<CashPaymentCenterManOrg>
    {
        public override IDataResult BeforeCreateAction(IDomainService<CashPaymentCenterManOrg> service, CashPaymentCenterManOrg entity)
        {
            if (entity.DateEnd.HasValue && entity.DateEnd.Value < entity.DateStart)
            {
                return Failure("Дата окончания действия договора не может быть раньше даты начала");
            }

            return CheckDates(service, entity);
        }

        public override IDataResult BeforeUpdateAction(IDomainService<CashPaymentCenterManOrg> service, CashPaymentCenterManOrg entity)
        {
            if (entity.DateEnd.HasValue && entity.DateEnd.Value < entity.DateStart.Date)
            {
                return Failure("Дата окончания действия договора не может быть раньше даты начала");
            }

            return CheckDates(service, entity);
        }

        public override IDataResult BeforeDeleteAction(IDomainService<CashPaymentCenterManOrg> service, CashPaymentCenterManOrg entity)
        {
            var cpcManOrgRoDomain = Container.ResolveDomain<CashPaymentCenterManOrgRo>();

            try
            {
                cpcManOrgRoDomain.GetAll()
                    .Where(x => x.CashPaymentCenterManOrg.Id == entity.Id)
                    .Select(x => x.Id)
                    .ForEach(x => cpcManOrgRoDomain.Delete(x));

                return Success();
            }

            finally
            {
                Container.Release(cpcManOrgRoDomain);
            }
        }

        // проверка пересечений с другими договорами по УК
        private IDataResult CheckDates(IDomainService<CashPaymentCenterManOrg> service, CashPaymentCenterManOrg entity)
        {
            var isAddedRoInPeriod =
                service.GetAll()
                    .Where(x => x.ManOrg.Id == entity.ManOrg.Id)
                    .Where(x => (x.DateStart <= entity.DateStart && (!x.DateEnd.HasValue || x.DateEnd >= entity.DateStart))
                        || (!entity.DateEnd.HasValue && x.DateStart >= entity.DateStart)
                        || (entity.DateEnd.HasValue && x.DateStart <= entity.DateEnd && (!x.DateEnd.HasValue || x.DateEnd >= entity.DateEnd)))
                    .Any(x => x.Id != entity.Id);

            return isAddedRoInPeriod
                ? Failure(string.Format(
                    "У управляющей компании {0} есть действующий договор в указанном периоде.",
                    entity.ManOrg.Contragent.ShortName)) 
                : Success();
        }
    }
}