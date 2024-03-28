namespace Bars.Gkh.Interceptors
{
    using B4;
    using Bars.Gkh.Entities.Hcs;

    public class MeterReadingInterceptor : EmptyDomainInterceptor<MeterReading>
    {
        public override IDataResult BeforeCreateAction(IDomainService<MeterReading> service, MeterReading entity)
        {
            this.SetCompositeKey(entity);

            return Success();
        }

        public override IDataResult BeforeUpdateAction(IDomainService<MeterReading> service, MeterReading entity)
        {
            this.SetCompositeKey(entity);

            return Success();
        }

        //формируем составной ключ
        private void SetCompositeKey(MeterReading entity)
        {
            entity.CompositeKey = string.Format("{0}#{1}#{2}",
                entity.Account.PaymentCode,
                entity.MeterSerial,
                entity.Service != null ? entity.Service.ToLower().Replace(" ", "") : "пусто");
        }
    }
}