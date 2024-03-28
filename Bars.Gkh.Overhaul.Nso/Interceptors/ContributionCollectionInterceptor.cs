namespace Bars.Gkh.Overhaul.Nso.Interceptors
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Overhaul.Nso.Entities;
    using Bars.Gkh.Overhaul.Nso.Enum;
    using Overhaul.Entities;

    public class ContributionCollectionInterceptor : EmptyDomainInterceptor<ContributionCollection>
    {
        public override IDataResult BeforeCreateAction(IDomainService<ContributionCollection> service, ContributionCollection entity)
        {
            var roId = entity.LongTermPrObject.RealityObject.Id;

            var listMinAmount = Container.Resolve<IDomainService<MinAmountDecision>>().GetAll()
                         .Where(x => x.RealityObject.Id == roId)
                         .Where(x => x.PropertyOwnerDecisionType == PropertyOwnerDecisionType.SetMinAmount)
                         .Where(x => (!x.PaymentDateEnd.HasValue) || (x.PaymentDateEnd >= DateTime.Now))
                         .Select(x => x.SizeOfPaymentOwners)
                         .Distinct()
                         .ToList();

            var listPaymentSizeMu = Container.Resolve<IDomainService<PaymentSizeMuRecord>>().GetAll()
                         .Where(x => x.Municipality.Id == entity.LongTermPrObject.RealityObject.Municipality.Id)
                         .Where(x => (!x.PaymentSizeCr.DateEndPeriod.HasValue) || (x.PaymentSizeCr.DateEndPeriod >= DateTime.Now))
                         .Select(x => x.PaymentSizeCr.PaymentSize)
                         .Distinct()
                         .ToList();

            if (listMinAmount.Count > 0)
            {
                if (listPaymentSizeMu.Count > 0)
                {
                    var paymentSize = listPaymentSizeMu.Max();

                    var sizeOfPaymentOwners = listMinAmount.Max();

                    if (sizeOfPaymentOwners < paymentSize)
                    {
                        return this.Failure("Значение в поле \"Размер взноса на кв. м жилой площади (руб.)\" в актуальной записи справочника \"Размер взноса на КР\" больше чем значение в поле \"Размер взноса, установленный собственниками\"");
                    }
                }
            }
            else
            {
                return this.Failure("Не задано \"Установление минимального размера фонда кап.ремонта\"");
            }

            return this.Success();
        }
    }
}