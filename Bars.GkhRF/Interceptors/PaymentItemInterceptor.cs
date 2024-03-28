namespace Bars.GkhRf.Interceptors
{
    using System.Linq;

    using Bars.B4;
    using Bars.GkhRf.Entities;

    public class PaymentItemInterceptor : EmptyDomainInterceptor<PaymentItem>
    {
        public override IDataResult BeforeUpdateAction(IDomainService<PaymentItem> service, PaymentItem entity)
        {
            CheckPayment(service, entity);
            return Success();
        }

        public override IDataResult BeforeCreateAction(IDomainService<PaymentItem> service, PaymentItem entity)
        {
            return BeforeUpdateAction(service, entity);
        }

        private void CheckPayment(IDomainService<PaymentItem> service, PaymentItem entity)
        {
            // Изменения этого метода могут сказаться на ErcImport !!
            var payments = service.GetAll()
                       .Where(x =>
                           x.Payment.Id == entity.Payment.Id 
                           && x.ManagingOrganization.Id == entity.ManagingOrganization.Id && x.ChargeDate != null
                           && x.TypePayment == entity.TypePayment && x.Id != entity.Id);

            if (!payments.Any() || entity.ChargeDate == null)
            {
                return;
            }

            var date = entity.ChargeDate.Value;

            var curPayment = payments.Any(x => x.ChargeDate.Value.Year == date.Year && x.ChargeDate.Value.Month == date.Month);
            if (curPayment)
            {
                throw new ValidationException("Данные за этот месяц уже имеются");
            }

            var nextPaymentList = payments.Where(x => x.ChargeDate.Value > date).OrderBy(x => x.ChargeDate.Value);
            if (nextPaymentList.Any())
            {
                var chargeDate = nextPaymentList.First().ChargeDate;
                if (chargeDate != null)
                {
                    throw new ValidationException(string.Format("Присутствуют данные за расчетный месяц: {0}", chargeDate.Value.ToString("MM.yyyy")));
                }
            }

            var prevDate = date.AddMonths(-1);
            var prevPayment = payments.Any(x => x.ChargeDate.Value.Year == prevDate.Year && x.ChargeDate.Value.Month == prevDate.Month);
            if (!prevPayment)
            {
                throw new ValidationException(string.Format("Отсутствуют данные за расчетный месяц: {0}", prevDate.ToString("MM.yyyy")));
            }
        }        
    }
}