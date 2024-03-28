namespace Bars.GkhCr.Interceptors
{
    using B4;
    using Entities;

    using System.Linq;

    using Enums;
    using Gkh.Domain.CollectionExtensions;

    public class SpecialPerformedWorkActPaymentInterceptor : EmptyDomainInterceptor<SpecialPerformedWorkActPayment>
    {
        public override IDataResult BeforeCreateAction(IDomainService<SpecialPerformedWorkActPayment> service, SpecialPerformedWorkActPayment entity)
        {
            return this.CheckSum(entity, service);
        }

        public override IDataResult BeforeUpdateAction(IDomainService<SpecialPerformedWorkActPayment> service, SpecialPerformedWorkActPayment entity)
        {
            return this.CheckSum(entity, service);
        }

        private IDataResult CheckSum(SpecialPerformedWorkActPayment entity, IDomainService<SpecialPerformedWorkActPayment> service)
        {
            var paymentSumAndPercents =
                service.GetAll()
                    .Where(x => x.PerformedWorkAct.Id == entity.PerformedWorkAct.Id && x.Id != entity.Id)
                    .Select(x => new { x.Sum, x.TypeActPayment, x.Percent })
                    .ToArray();

            var paymentsTotalSum = paymentSumAndPercents.SafeSum(x => x.Sum) + entity.Sum;

            if (entity.PerformedWorkAct.Sum < paymentsTotalSum)
            {
                return this.Failure("Общая сумма оплат не должна превышать сумму по акту выполненных работ!");
            }

            if (entity.TypeActPayment == ActPaymentType.PrePayment)
            {
                var prepaymentSum =
                    paymentSumAndPercents
                        .Where(x => x.TypeActPayment == ActPaymentType.PrePayment)
                        .SafeSum(x => x.Sum) + entity.Sum;

                var actSum = entity.PerformedWorkAct.Sum ?? 0;

                var percent = actSum != 0 ? prepaymentSum * 100 / actSum : 0;

                if (percent > 30)
                {
                    return this.Failure("Сумма по выплате аванса не может быть более 30% (пункт 3. ст.176 ЖК РФ)!");
                }
            }

            return this.Success();
        }
    }
}