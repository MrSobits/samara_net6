namespace Bars.Gkh.Interceptors
{
    using B4;
    using Bars.Gkh.Entities.Hcs;
    using System.Linq;

    public class HouseAccountChargeInterceptor : EmptyDomainInterceptor<HouseAccountCharge>
    {
        public override IDataResult BeforeCreateAction(IDomainService<HouseAccountCharge> service, HouseAccountCharge entity)
        {
            this.SetCompositeKey(entity);

            var haveOtherAccChargeInMonth = service.GetAll()
                .Where(x => x.DateCharging != null)
                .Where(x => x.Account.Id == entity.Account.Id)
                .Any(x => (x.DateCharging.Month.ToString() + x.DateCharging.Year.ToString()) == (entity.DateCharging.Month.ToString() + entity.DateCharging.Year.ToString()));

            if (haveOtherAccChargeInMonth)
            {
                return Failure("В данном лицевом счете уже существует начисление за " + entity.DateCharging.ToString("MMMM yyyy"));
            }

            return Success();
        }

        public override IDataResult BeforeUpdateAction(IDomainService<HouseAccountCharge> service, HouseAccountCharge entity)
        {
            this.SetCompositeKey(entity);

            var haveOtherAccChargeInMonth = service.GetAll()
                .Where(x => x.DateCharging != null)
                .Where(x => x.Account.Id == entity.Account.Id)
                .Any(x => (x.DateCharging.Month.ToString() + x.DateCharging.Year.ToString()) == (entity.DateCharging.Month.ToString() + entity.DateCharging.Year.ToString()));

            if (haveOtherAccChargeInMonth)
            {
                return Failure("В данном лицевом счете уже существует начисление за " + entity.DateCharging.ToString("MMMM yyyy"));
            }

            return Success();
        }

        //формируем составной ключ
        private void SetCompositeKey(HouseAccountCharge entity)
        {
            entity.CompositeKey = string.Format("{0}#{1}#{2}",
                entity.Account.PaymentCode,
                entity.DateCharging.ToString("dd-MM-yyyy"),
                entity.Service != null ? entity.Service.ToLower().Replace(" ", "") : "пусто");
        }
    }
}