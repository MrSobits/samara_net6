namespace Bars.Gkh.Overhaul.Tat.Interceptors
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Overhaul.Tat.Entities;
    using Overhaul.Entities;
    using Overhaul.Enum;

    public class MinAmountDecisionInterceptor : EmptyDomainInterceptor<MinAmountDecision>
    {
        public override IDataResult BeforeUpdateAction(IDomainService<MinAmountDecision> service, MinAmountDecision entity)
        {
            var sizeOfPaymentSubject =
                Container.Resolve<IDomainService<PaymentSizeMuRecord>>().GetAll()
                    .Where(x => x.Municipality.Id == entity.RealityObject.Municipality.Id)
                    .Where(x => x.PaymentSizeCr.TypeIndicator == TypeIndicator.MinSizeSqMetLivinSpace)
                    .Where(x => x.PaymentSizeCr.DateStartPeriod < entity.ObjectCreateDate)
                    .Where(x => !x.PaymentSizeCr.DateEndPeriod.HasValue || x.PaymentSizeCr.DateEndPeriod > entity.ObjectCreateDate)
                    .Select(x => x.PaymentSizeCr.PaymentSize)
                    .FirstOrDefault();

            if (entity.SizeOfPaymentOwners < sizeOfPaymentSubject || sizeOfPaymentSubject == 0)
            {
                return
                    Failure(
                        "Размер вноса, установленный собственниками, не должен быть меньше Размера вноса, установленного субъектом");
            }

            return Success();
        }
    }
}