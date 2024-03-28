namespace Bars.Gkh.Overhaul.Interceptors
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Overhaul.Entities;

    public class PaymentSizeMuRecordInterceptor : EmptyDomainInterceptor<PaymentSizeMuRecord>
    {
        public override IDataResult BeforeCreateAction(IDomainService<PaymentSizeMuRecord> service, PaymentSizeMuRecord entity)
        {
            var hasSomeValues = service.GetAll()
                     .Where(x => entity.Municipality.Id == x.Municipality.Id)
                    .Any(x => (x.PaymentSizeCr.DateStartPeriod <= entity.PaymentSizeCr.DateStartPeriod && (!x.PaymentSizeCr.DateEndPeriod.HasValue || x.PaymentSizeCr.DateEndPeriod >= entity.PaymentSizeCr.DateStartPeriod))
                                || (!entity.PaymentSizeCr.DateEndPeriod.HasValue && x.PaymentSizeCr.DateStartPeriod >= entity.PaymentSizeCr.DateStartPeriod)
                                || (entity.PaymentSizeCr.DateEndPeriod.HasValue && x.PaymentSizeCr.DateStartPeriod <= entity.PaymentSizeCr.DateEndPeriod && (!x.PaymentSizeCr.DateEndPeriod.HasValue || x.PaymentSizeCr.DateEndPeriod >= entity.PaymentSizeCr.DateEndPeriod)));

            if (hasSomeValues)
            {
                return new BaseDataResult(false, "В данный период уже есть действующее значение показателя");
            }

            return base.BeforeCreateAction(service, entity);
        }
    }
}