namespace Bars.Gkh.Overhaul.Nso.Interceptors
{
    using System.Linq;

    using Bars.B4;
    using Overhaul.Entities;

    public class PaymentSizeCrObjectInterceptor : EmptyDomainInterceptor<PaymentSizeCr>
    {
        public override IDataResult BeforeDeleteAction(IDomainService<PaymentSizeCr> service, PaymentSizeCr entity)
        {
            var paymentSizeMuRecordDomain = Container.Resolve<IDomainService<PaymentSizeMuRecord>>();
            var paymentSizeMuRecordList = paymentSizeMuRecordDomain.GetAll().Where(x => x.PaymentSizeCr.Id == entity.Id).Select(x => x.Id).ToList();
            foreach (var value in paymentSizeMuRecordList)
            {
                paymentSizeMuRecordDomain.Delete(value);
            }

            return Success();
        }
    }
}
