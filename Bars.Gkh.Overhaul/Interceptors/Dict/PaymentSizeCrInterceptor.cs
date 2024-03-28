using Bars.B4.DataAccess;

namespace Bars.Gkh.Overhaul.Interceptors
{
    using System.Linq;
    using B4.Utils;
    using B4;
    using Entities;

    public class PaymentSizeCrInterceptor : EmptyDomainInterceptor<PaymentSizeCr>
    {
        public override IDataResult BeforeDeleteAction(IDomainService<PaymentSizeCr> service, PaymentSizeCr entity)
        {
            var paymentSizeMuRecordDomain = Container.Resolve<IDomainService<PaymentSizeMuRecord>>();
            paymentSizeMuRecordDomain.GetAll()
                .Where(x => x.PaymentSizeCr.Id == entity.Id)
                .Select(x => x.Id)
                .ForEach(x => paymentSizeMuRecordDomain.Delete(x));

            return Success();
        }

        public override IDataResult BeforeCreateAction(IDomainService<PaymentSizeCr> service, PaymentSizeCr entity)
        {
            string msg;
            if (!CheckDate(entity, out msg)){
                return Failure(msg);
            }

            return Success();
        }
        
        public override IDataResult BeforeUpdateAction(IDomainService<PaymentSizeCr> service, PaymentSizeCr entity)
        {
            string msg;
            if (!CheckDate(entity,out msg)){
                return Failure(msg);
            }

            return Success();
        }

        private bool CheckDate(PaymentSizeCr entity, out string msg)
        {
            var paymentSizeMuRecDomain = Container.ResolveDomain<PaymentSizeMuRecord>();

            try
            {
                msg = "";
                if (entity.DateEndPeriod.HasValue &&
                    entity.DateStartPeriod > entity.DateEndPeriod)
                {
                    msg = "Дата начала периода не может быть меньше даты окончания!";
                    return false;
                }

                var existPaySizeMuRecQuery = paymentSizeMuRecDomain.GetAll()
                    .Where(x => x.PaymentSizeCr.Id == entity.Id);

                var hasIntersectValues = paymentSizeMuRecDomain.GetAll()
                    .Where(x => x.PaymentSizeCr.TypeIndicator == entity.TypeIndicator )
                    .Where(x => x.PaymentSizeCr.Id != entity.Id)
                    .Where(x => existPaySizeMuRecQuery.Any(y => y.Municipality.Id == x.Municipality.Id))
                    .Any(x => (x.PaymentSizeCr.DateStartPeriod <= entity.DateStartPeriod && (!x.PaymentSizeCr.DateEndPeriod.HasValue || x.PaymentSizeCr.DateEndPeriod >= entity.DateStartPeriod))
                                || (!entity.DateEndPeriod.HasValue && x.PaymentSizeCr.DateStartPeriod >= entity.DateStartPeriod)
                                || (entity.DateEndPeriod.HasValue && x.PaymentSizeCr.DateStartPeriod <= entity.DateEndPeriod && (!x.PaymentSizeCr.DateEndPeriod.HasValue || x.PaymentSizeCr.DateEndPeriod >= entity.DateEndPeriod)));


                if (hasIntersectValues)
                {
                    msg = " У некоторых муниципальных образований есть существующие записи в данном периоде!";
                    return false;
                }

            }
            finally
            {
                Container.Release(paymentSizeMuRecDomain);
            }

            return true;
        }
    }
}
